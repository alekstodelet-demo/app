import { makeAutoObservable, runInAction } from "mobx";
import http from "api/https";
import { Auth } from "constants/Auth";
import MainStore from "MainStore";

export type AuthFormType = 'login' | 'registration' | 'rutoken' | 'jacarta' | 'enotoken' | 'smartid' | 'esicloud' | 'ettn';

class AuthStore {
  currentForm: AuthFormType = 'login';
  
  login: string = "";
  password: string = "";
  rememberMe: boolean = false;
  loading: boolean = false;
  error: string = "";
  isAuthenticated: boolean = false;

  constructor() {
    makeAutoObservable(this);
  }

  setCurrentForm = (formType: AuthFormType) => {
    runInAction(() => {
      this.currentForm = formType;
      this.error = "";
    });
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

  async loginWithCredentials() {
    try {
      this.loading = true;
      this.error = "";
      
      const authData: Auth = {
        pin: this.password,
        tokenId: this.login,
        signature: "PIN_LOGIN"
      };
      
      const response = await http.post('/api/v1/Auth/login', authData);

      runInAction(() => {
        if (response?.status === 200 || response?.status === 201) {
          this.isAuthenticated = true;
          // Редирект на главную страницу после успешной авторизации
          window.location.href = '/user';
        } else {
          this.error = "Ошибка авторизации. Пожалуйста, проверьте логин и пароль.";
        }
        this.loading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.error = "Произошла ошибка при авторизации. Попробуйте позже.";
        this.loading = false;
      });
    }
  }

  async logOut() {
    try {
      await http.post('/api/v1/Auth/logout', {});
      
      runInAction(() => {
        this.isAuthenticated = false;
        this.resetForm();
        // Редирект на страницу входа
        window.location.href = '/login';
      });
    } catch (error) {
      MainStore.setSnackbar("Ошибка при выходе из системы", "error");
    }
  }

  async checkAuthStatus() {
    try {
      const response = await http.get('/api/v1/Auth/status');
      
      runInAction(() => {
        this.isAuthenticated = Boolean(response && response.status === 200);
      });
      
      return this.isAuthenticated;
    } catch (error) {
      runInAction(() => {
        this.isAuthenticated = false;
      });
      return false;
    }
  }
}

export default new AuthStore();