// src/features/Auth/store/AuthStore.ts

import { makeAutoObservable, runInAction } from "mobx";
import { login, logout, validateToken, refreshToken, LoginRequest } from "api/Auth/useAuth";
import MainStore from "MainStore";

// Получение уникального ID устройства
const getDeviceId = (): string => {
  let deviceId = localStorage.getItem('deviceId');
  if (!deviceId) {
    deviceId = `device_${Math.random().toString(36).substring(2, 15)}`;
    localStorage.setItem('deviceId', deviceId);
  }
  return deviceId;
};

// В начале файла добавим экспорт типа
export type AuthFormType = 'login' | 'registration' | 'rutoken' | 'jacarta' | 'enotoken' | 'smartid' | 'esicloud' | 'ettn';

class AuthStore {
  login: string = "";
  password: string = "";
  rememberMe: boolean = false;
  loading: boolean = false;
  error: string = "";
  isAuthenticated: boolean = false;
  deviceId: string = getDeviceId();
  currentForm: AuthFormType = 'login';

  constructor() {
    makeAutoObservable(this);
  }

  setLogin(login: string) {
    this.login = login;
  }

  setPassword(password: string) {
    this.password = password;
  }

  setRememberMe(remember: boolean) {
    this.rememberMe = remember;
  }

  resetForm() {
    this.login = "";
    this.password = "";
    this.error = "";
  }

  setCurrentForm(form: AuthFormType) {
    this.currentForm = form;
  }

  async loginWithCredentials() {
    try {
      this.loading = true;
      this.error = "";
      
      const authData: LoginRequest = {
        pin: this.password,
        tokenId: this.login,
        signature: "SIMULATED_SIGNATURE",
        deviceId: this.deviceId
      };
      
      const response = await login(authData);

      runInAction(() => {
        if (response?.status === 200 || response?.status === 201) {
          this.isAuthenticated = true;
          
          if (this.rememberMe && response.data.accessToken) {
            localStorage.setItem('accessToken', response.data.accessToken);
            localStorage.setItem('tokenExpiry', 
              (Date.now() + response.data.expiresIn * 1000).toString());
          }
          
          window.location.href = '/user';
        } else {
          this.error = "Ошибка авторизации. Пожалуйста, проверьте логин и пароль.";
        }
        this.loading = false;
      });
    } catch (error: any) {
      runInAction(() => {
        if (error.response) {
          this.error = error.response.data?.message || 
            "Произошла ошибка при авторизации. Попробуйте позже.";
        } else {
          this.error = "Сетевая ошибка. Пожалуйста, проверьте ваше интернет-соединение.";
        }
        this.loading = false;
      });
    }
  }

  async logOut() {
    try {
      await logout();
      
      runInAction(() => {
        this.isAuthenticated = false;
        this.resetForm();
        
        // Удаляем данные из localStorage
        localStorage.removeItem('accessToken');
        localStorage.removeItem('tokenExpiry');
        
        // Редирект на страницу входа
        window.location.href = '/login';
      });
    } catch (error) {
      MainStore.setSnackbar("Ошибка при выходе из системы", "error");
    }
  }

  async checkAuthStatus() {
    try {
      // Проверяем, есть ли у нас локально сохраненный токен
      const token = localStorage.getItem('accessToken');
      const expiry = localStorage.getItem('tokenExpiry');
      
      // Если токен есть и он не истек, пробуем использовать его
      if (token && expiry && parseInt(expiry) > Date.now()) {
        this.isAuthenticated = true;
        return true;
      }
      
      // Если токен истек, пробуем обновить его
      if (token && expiry) {
        await this.refreshToken();
      }
      
      // Если нет локальных токенов, проверяем через API
      const response = await validateToken();
      
      runInAction(() => {
        this.isAuthenticated = response?.status === 200;
      });
      
      return this.isAuthenticated;
    } catch (error) {
      // Пытаемся обновить токен, если валидация не сработала
      try {
        await this.refreshToken();
        return true;
      } catch (refreshError) {
        runInAction(() => {
          this.isAuthenticated = false;
        });
        return false;
      }
    }
  }
  
  async refreshToken() {
    try {
      const response = await refreshToken();
      
      if (response?.status === 200 && response.data.accessToken) {
        runInAction(() => {
          this.isAuthenticated = true;
          if (this.rememberMe) {
            localStorage.setItem('accessToken', response.data.accessToken);
            localStorage.setItem('tokenExpiry', 
              (Date.now() + response.data.expiresIn * 1000).toString());
          }
        });
        return true;
      }
      return false;
    } catch (error) {
      runInAction(() => {
        this.isAuthenticated = false;
        localStorage.removeItem('accessToken');
        localStorage.removeItem('tokenExpiry');
      });
      return false;
    }
  }
}

export default new AuthStore();