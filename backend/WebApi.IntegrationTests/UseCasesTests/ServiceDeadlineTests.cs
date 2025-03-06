using Application.UseCases;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests.UseCasesTests
{
    public class ApplicationUseCasesTests
    {
        private readonly DapperDbContext _context;
        private readonly MariaDbContext _mariaContext;
        private readonly ApplicationUseCases _useCases;

        public ApplicationUseCasesTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _context = new DapperDbContext(configuration);
            _mariaContext = new MariaDbContext(configuration);

            var unitOfWork = new UnitOfWork(_context, _mariaContext, null, null, null, null);

            _useCases = new ApplicationUseCases(unitOfWork, null, null, null, null, null, null);
        }

        [Fact]
        public async Task CalculateDeadlineService_ExcludesWeekends_ReturnsValidDeadline()
        {
            // Arrange
            var sql = "select * from service";
            var data = DatabaseHelper.RunQueryList<Service>(sql, reader => new Service
            {
                id = reader.GetInt32(reader.GetOrdinal("id")),
                name = reader.GetString(reader.GetOrdinal("name")),
                day_count = reader.GetInt32(reader.GetOrdinal("day_count")),
            });

            foreach (var service in data)
            {
                int testServiceId = service.id;

                // Act
                var result = await _useCases.CalculateDeadlineService(testServiceId);

                // Assert
                Assert.NotNull(result);
                Assert.NotEqual(DayOfWeek.Saturday, result.Value.DayOfWeek);
                Assert.NotEqual(DayOfWeek.Sunday, result.Value.DayOfWeek);
                Console.WriteLine($"Day: {result.Value}, Week: {result.Value.DayOfWeek}, Service id: {service.id}, Day count: {service.day_count}");
            }
        }

        [Fact]
        public async Task CalculateDeadlineService_WhenNextSaturdayIsWorkday_ReturnsCorrectDeadline()
        {
            // Arrange
            var sql = "select * from work_schedule where is_active = true";
            var workSchedules = DatabaseHelper.RunQueryList<WorkSchedule>(sql, reader => new WorkSchedule
            {
                id = reader.GetInt32(reader.GetOrdinal("id")),
                name = reader.GetString(reader.GetOrdinal("name")),
                year = reader.GetInt32(reader.GetOrdinal("year")),
            });
            
            DateTime nextSaturday = DateTime.Today.AddDays(((int)DayOfWeek.Saturday - (int)DateTime.Today.DayOfWeek + 7) % 7);

            var insertQuery = $@"INSERT INTO work_schedule_exception (date_start, date_end, is_holiday, schedule_id, name) 
                                    VALUES ('{nextSaturday:yyyy-MM-dd} 00:00:00.000000', 
                                            '{nextSaturday:yyyy-MM-dd} 23:59:00.000000',
                                            false,
                                            {workSchedules.FirstOrDefault(x =>x.year == DateTime.Now.Year)?.id},
                                            'Рабочая суббота') RETURNING id;";

            var daysUntilNextSaturday = ((int)DayOfWeek.Saturday - (int)DateTime.Today.DayOfWeek + 7) % 7;
            daysUntilNextSaturday = daysUntilNextSaturday == 0 ? 7 : daysUntilNextSaturday;
            daysUntilNextSaturday += 1;

            int idNextSaturday = DatabaseHelper.RunQuery(insertQuery);

            var insertService = $@"INSERT INTO public.service (name, day_count) 
                                    VALUES ('Тестовая услуга', {daysUntilNextSaturday}) RETURNING id";

            int idService = DatabaseHelper.RunQuery(insertService);
            
            // Act
            var result = await _useCases.CalculateDeadlineService(idService);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nextSaturday.Date, result.Value.Date);
            Console.WriteLine($"Day: {result.Value}, Week: {result.Value.DayOfWeek}");
        }
    }
}