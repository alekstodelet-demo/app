import { makeAutoObservable, runInAction } from 'mobx';
import React from 'react';

class RegistrationStore {
  // Поля формы
  userType: 'Юридическое лицо' | 'Физическое лицо' = 'Юридическое лицо';
  innJuridical: string = '';
  innPhysical: string = '';
  password: string = '';
  confirmPassword: string = '';
  
  // Ошибки валидации
  errors: Record<string, string> = {};
  
  // Флаг загрузки
  loading: boolean = false;

  constructor() {
    makeAutoObservable(this);
  }

  handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    runInAction(() => {
      (this as any)[name] = value;
      this.validateField(name, value);
    });
  }

  setUserType = (type: 'Юридическое лицо' | 'Физическое лицо') => {
    runInAction(() => {
      this.userType = type;
      this.errors = {};
    });
  }

  validateField = (name: string, value: string) => {
    switch(name) {
      case 'innJuridical':
        if (this.userType === 'Юридическое лицо') {
          if (!value) {
            this.errors.innJuridical = 'ИНН обязателен';
          } else if (!/^\d{10}$/.test(value)) {
            this.errors.innJuridical = 'ИНН должен содержать 10 цифр';
          } else {
            delete this.errors.innJuridical;
          }
        }
        break;
      case 'innPhysical':
        if (this.userType === 'Физическое лицо') {
          if (!value) {
            this.errors.innPhysical = 'ИНН обязателен';
          } else if (!/^\d{12}$/.test(value)) {
            this.errors.innPhysical = 'ИНН должен содержать 12 цифр';
          } else {
            delete this.errors.innPhysical;
          }
        }
        break;
      case 'password':
        if (!value) {
          this.errors.password = 'Пароль обязателен';
        } else if (value.length < 8) {
          this.errors.password = 'Пароль должен содержать не менее 8 символов';
        } else {
          delete this.errors.password;
        }
        
        if (this.confirmPassword && value !== this.confirmPassword) {
          this.errors.confirmPassword = 'Пароли не совпадают';
        } else if (this.confirmPassword) {
          delete this.errors.confirmPassword;
        }
        break;
      case 'confirmPassword':
        if (!value) {
          this.errors.confirmPassword = 'Повторите пароль';
        } else if (value !== this.password) {
          this.errors.confirmPassword = 'Пароли не совпадают';
        } else {
          delete this.errors.confirmPassword;
        }
        break;
    }
  }

  validateForm = () => {
    let isValid = true;
    
    if (this.userType === 'Юридическое лицо') {
      if (!this.innJuridical) {
        this.errors.innJuridical = 'ИНН обязателен';
        isValid = false;
      } else if (!/^\d{10}$/.test(this.innJuridical)) {
        this.errors.innJuridical = 'ИНН должен содержать 10 цифр';
        isValid = false;
      }
    } else {
      if (!this.innPhysical) {
        this.errors.innPhysical = 'ИНН обязателен';
        isValid = false;
      } else if (!/^\d{12}$/.test(this.innPhysical)) {
        this.errors.innPhysical = 'ИНН должен содержать 12 цифр';
        isValid = false;
      }
    }
    
    if (!this.password) {
      this.errors.password = 'Пароль обязателен';
      isValid = false;
    } else if (this.password.length < 8) {
      this.errors.password = 'Пароль должен содержать не менее 8 символов';
      isValid = false;
    }
    
    if (!this.confirmPassword) {
      this.errors.confirmPassword = 'Повторите пароль';
      isValid = false;
    } else if (this.confirmPassword !== this.password) {
      this.errors.confirmPassword = 'Пароли не совпадают';
      isValid = false;
    }
    
    return isValid;
  }

  submitForm = async () => {
    if (!this.validateForm()) {
      return;
    }
    
    try {
      runInAction(() => {
        this.loading = true;
      });
      
      // TODO: Здесь должна быть логика отправки данных на сервер
      console.log('Форма отправлена:', {
        userType: this.userType,
        inn: this.userType === 'Юридическое лицо' ? this.innJuridical : this.innPhysical,
        password: this.password
      });
      
      await new Promise(resolve => setTimeout(resolve, 1000));
      
    } catch (error) {
      runInAction(() => {
        this.errors.general = 'Произошла ошибка при регистрации. Пожалуйста, попробуйте снова.';
      });
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  }

  registerAsInternational = () => {
    console.log('Переход на форму регистрации международной организации');
  }

  registerAsBranch = () => {
    console.log('Переход на форму регистрации филиала');
  }

  resetForm = () => {
    runInAction(() => {
      this.innJuridical = '';
      this.innPhysical = '';
      this.password = '';
      this.confirmPassword = '';
      this.errors = {};
    });
  }
}

export default new RegistrationStore();