// src/api/https.ts

import axios from "axios";
import MainStore from "MainStore";

const API_URL = process.env.REACT_APP_API_URL || "https://localhost:5001";

const http = axios.create({
  baseURL: API_URL,
  withCredentials: true, // Для работы с cookies
});

// Добавляем заголовок X-Device-Id, который ожидает контроллер
http.interceptors.request.use(
  (config) => {
    // Получаем deviceId из localStorage
    const deviceId = localStorage.getItem('deviceId') || 
      `device_${Math.random().toString(36).substring(2, 15)}`;
    
    if (!localStorage.getItem('deviceId')) {
      localStorage.setItem('deviceId', deviceId);
    }
    
    // Добавляем заголовок
    config.headers["X-Device-Id"] = deviceId;
    
    // Добавляем токен авторизации, если он есть
    const token = localStorage.getItem('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    // Добавляем заголовок для предотвращения кеширования в браузере
    config.headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    config.headers["Pragma"] = "no-cache";
    config.headers["Expires"] = "0";
    
    // Добавляем заголовок для защиты от CSRF-атак
    const csrfToken = document.cookie
      .split('; ')
      .find(row => row.startsWith('XSRF-TOKEN='))
      ?.split('=')[1];
    
    if (csrfToken) {
      config.headers["X-XSRF-TOKEN"] = csrfToken;
    }
    
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Обработка ответов и ошибок
http.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    const originalRequest = error.config;
    
    // Если ошибка 401 (Unauthorized) и это не повторный запрос
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      
      try {
        // Пытаемся обновить токен через refresh-token
        const res = await axios.post(
          `${API_URL}/api/v1/Auth/refresh-token`, 
          {}, 
          { 
            withCredentials: true,
            headers: {
              "X-Device-Id": localStorage.getItem('deviceId') || "",
            }
          }
        );
        
        if (res.status === 200) {
          // Сохраняем новый access token
          localStorage.setItem('accessToken', res.data.accessToken);
          localStorage.setItem('tokenExpiry', 
            (Date.now() + res.data.expiresIn * 1000).toString());
          
          // Повторяем оригинальный запрос с новым токеном
          originalRequest.headers.Authorization = `Bearer ${res.data.accessToken}`;
          return http(originalRequest);
        }
      } catch (refreshError) {
        // Если не удалось обновить токен, перенаправляем на страницу входа
        localStorage.removeItem('accessToken');
        localStorage.removeItem('tokenExpiry');
        
        // Проверяем, что текущий URL не является страницей логина
        // чтобы избежать бесконечного редиректа
        if (!window.location.pathname.includes('/login')) {
          window.location.href = '/login';
        }
        return Promise.reject(refreshError);
      }
    }
    
    // Обработка других HTTP ошибок
    if (error.response) {
      // Ответ сервера вне диапазона 2xx
      switch (error.response.status) {
        case 400:
          console.error('Bad Request:', error.response.data);
          MainStore.setSnackbar(
            error.response.data.message || "Некорректный запрос", 
            "error"
          );
          break;
        case 403:
          console.error('Forbidden:', error.response.data);
          MainStore.setSnackbar(
            "Доступ запрещен. У вас недостаточно прав.", 
            "error"
          );
          break;
        case 404:
          console.error('Not Found:', error.response.data);
          MainStore.setSnackbar(
            "Запрашиваемый ресурс не найден", 
            "error"
          );
          break;
        case 422:
          console.error('Validation Error:', error.response.data);
          MainStore.setSnackbar(
            error.response.data.message || "Ошибка валидации данных", 
            "error"
          );
          break;
        case 500:
          console.error('Server Error:', error.response.data);
          MainStore.setSnackbar(
            "Внутренняя ошибка сервера. Попробуйте позже.", 
            "error"
          );
          break;
        default:
          console.error('HTTP Error:', error.response.data);
          MainStore.setSnackbar(
            "Произошла ошибка при обработке запроса", 
            "error"
          );
      }
    } else if (error.request) {
      // Запрос был сделан, но ответ не получен
      console.error('Network Error:', error.request);
      MainStore.setSnackbar(
        "Ошибка сети. Проверьте подключение к интернету.", 
        "error"
      );
    } else {
      // Ошибка при настройке запроса
      console.error('Request Error:', error.message);
      MainStore.setSnackbar(
        "Ошибка при формировании запроса", 
        "error"
      );
    }
    
    return Promise.reject(error);
  }
);

// Экспорт методов для работы с API
const get = (url: string, headers = {}, params = {}) => {
  return http
    .get(url, {
      headers,
      params
    });
};

const post = (url: string, data = {}, headers = {}, config = {}) => {
  return http
    .post(url, data, {
      headers,
      ...config
    });
};

const put = (url: string, data = {}, headers = {}) => {
  return http
    .put(url, data, {
      headers
    });
};

const patch = (url: string, data = {}, headers = {}) => {
  return http
    .patch(url, data, {
      headers
    });
};

const del = (url: string, headers = {}) => {
  return http
    .delete(url, {
      headers
    });
};

export { get, post, put, patch, del };
export default http;