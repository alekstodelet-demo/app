using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Services;

namespace WebApi.Services
{
    /// <summary>
    /// Service to send alerts via various channels (email, SMS, Telegram, Slack, etc.)
    /// </summary>
    public interface IAlertService
    {
        Task<bool> SendAlertAsync(string subject, string message, AlertSeverity severity);
        Task<bool> SendEmailAlertAsync(string subject, string message, AlertSeverity severity, List<string> recipients = null);
        Task<bool> SendTelegramAlertAsync(string subject, string message, AlertSeverity severity);
        Task<bool> SendSlackAlertAsync(string subject, string message, AlertSeverity severity);
    }

    public class AlertService : IAlertService
    {
        private readonly ILogger<AlertService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Dictionary<AlertSeverity, string> _severityColors = new()
        {
            { AlertSeverity.Info, "#17a2b8" },
            { AlertSeverity.Warning, "#ffc107" },
            { AlertSeverity.Error, "#dc3545" },
            { AlertSeverity.Critical, "#9c27b0" }
        };

        public AlertService(
            ILogger<AlertService> logger,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> SendAlertAsync(string subject, string message, AlertSeverity severity)
        {
            try
            {
                var tasks = new List<Task<bool>>();
                
                // Only send emails for Warning, Error, and Critical alerts
                if (severity >= AlertSeverity.Warning)
                {
                    tasks.Add(SendEmailAlertAsync(subject, message, severity));
                }
                
                // Only send to Telegram for Error and Critical alerts
                if (severity >= AlertSeverity.Error)
                {
                    tasks.Add(SendTelegramAlertAsync(subject, message, severity));
                }
                
                // Only send to Slack for Error and Critical alerts
                if (severity >= AlertSeverity.Error)
                {
                    tasks.Add(SendSlackAlertAsync(subject, message, severity));
                }
                
                // Wait for all tasks to complete and check if any succeeded
                var results = await Task.WhenAll(tasks);
                return results.Any(success => success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send alert: {Subject}", subject);
                return false;
            }
        }

        public async Task<bool> SendEmailAlertAsync(string subject, string message, AlertSeverity severity, List<string> recipients = null)
        {
            try
            {
                // Get default recipients from config if none provided
                recipients ??= _configuration.GetSection("Alerts:Email:Recipients").Get<List<string>>();
                if (recipients == null || recipients.Count == 0)
                {
                    _logger.LogWarning("No email recipients configured for alerts");
                    return false;
                }

                var smtpServer = _configuration["Alerts:Email:SmtpServer"];
                var fromAddress = _configuration["Alerts:Email:FromAddress"];
                
                // Check if SMTP is configured
                if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(fromAddress))
                {
                    _logger.LogWarning("SMTP is not configured for sending email alerts");
                    return false;
                }
                
                // In a real implementation, you'd send an actual email here
                // For now, we'll just log it
                _logger.LogInformation(
                    "Email alert would be sent to {Recipients} with subject '{Subject}' and severity {Severity}",
                    string.Join(", ", recipients), subject, severity);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email alert: {Subject}", subject);
                return false;
            }
        }

        public async Task<bool> SendTelegramAlertAsync(string subject, string message, AlertSeverity severity)
        {
            try
            {
                var botToken = _configuration["Alerts:Telegram:BotToken"];
                var chatId = _configuration["Alerts:Telegram:ChatId"];
                
                if (string.IsNullOrEmpty(botToken) || string.IsNullOrEmpty(chatId))
                {
                    _logger.LogWarning("Telegram bot token or chat ID not configured");
                    return false;
                }
                
                var httpClient = _httpClientFactory.CreateClient("TelegramBot");
                
                // Format message with emoji based on severity
                var emoji = severity switch
                {
                    AlertSeverity.Info => "ℹ️",
                    AlertSeverity.Warning => "⚠️",
                    AlertSeverity.Error => "🔴",
                    AlertSeverity.Critical => "🚨",
                    _ => "❓"
                };
                
                var formattedMessage = $"{emoji} *{subject}*\n\n{message}\n\n_Severity: {severity}_\n_Time: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC_";
                
                var requestUrl = $"https://api.telegram.org/bot{botToken}/sendMessage";
                
                var response = await httpClient.PostAsJsonAsync(requestUrl, new
                {
                    chat_id = chatId,
                    text = formattedMessage,
                    parse_mode = "Markdown"
                });
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Telegram alert sent successfully: {Subject}", subject);
                    return true;
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to send Telegram alert: {ErrorContent}", errorContent);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send Telegram alert: {Subject}", subject);
                return false;
            }
        }

        public async Task<bool> SendSlackAlertAsync(string subject, string message, AlertSeverity severity)
        {
            try
            {
                var webhookUrl = _configuration["Alerts:Slack:WebhookUrl"];
                
                if (string.IsNullOrEmpty(webhookUrl))
                {
                    _logger.LogWarning("Slack webhook URL not configured");
                    return false;
                }
                
                var httpClient = _httpClientFactory.CreateClient("SlackWebhook");
                
                var color = _severityColors[severity];
                var environment = _configuration["Environment"] ?? "Unknown";
                
                var payload = new
                {
                    attachments = new[]
                    {
                        new
                        {
                            fallback = $"{subject}: {message}",
                            color,
                            title = subject,
                            text = message,
                            fields = new[]
                            {
                                new { title = "Severity", value = severity.ToString(), @short = true },
                                new { title = "Environment", value = environment, @short = true },
                                new { title = "Time", value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), @short = true }
                            }
                        }
                    }
                };
                
                var response = await httpClient.PostAsJsonAsync(webhookUrl, payload);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Slack alert sent successfully: {Subject}", subject);
                    return true;
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to send Slack alert: {ErrorContent}", errorContent);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send Slack alert: {Subject}", subject);
                return false;
            }
        }
    }
}