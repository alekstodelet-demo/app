// src/features/Auth/components/LoginForm.tsx
import React, { FC, useState } from 'react';
import { observer } from 'mobx-react';
import { Box, Typography, FormControlLabel, Checkbox, Link } from '@mui/material';
import CustomTextField from 'components/TextField';
import CustomButton from 'components/Button';
import styled from 'styled-components';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import { useTranslation } from 'react-i18next';
import AuthStore from '../store/AuthStore';

interface LoginFormProps {
  loading?: boolean;
  error?: string;
}

const LoginForm: FC<LoginFormProps> = observer(({ loading, error }) => {
  const { t } = useTranslation();
  const [showPassword, setShowPassword] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await AuthStore.loginWithCredentials();
  };

  return (
    <FormContainer>
      <Typography variant="h5" align="center" gutterBottom>
        Вход через Логин и пароль
      </Typography>
      <Typography variant="subtitle2" align="center" mb={3} color="textSecondary">
        Логин и пароль
      </Typography>
      
      <form onSubmit={handleSubmit}>
        <Box mb={2}>
          <CustomTextField
            label="Логин"
            value={AuthStore.login}
            onChange={(e) => AuthStore.setLogin(e.target.value)}
            id="login-input"
            name="login"
          />
        </Box>
        
        <Box mb={2}>
          <CustomTextField
            label="Пароль"
            value={AuthStore.password}
            onChange={(e) => AuthStore.setPassword(e.target.value)}
            type={showPassword ? "text" : "password"}
            id="password-input"
            name="password"
            InputProps={{
              endAdornment: (
                <PasswordToggle 
                  onClick={() => setShowPassword(!showPassword)}
                >
                  {showPassword ? <VisibilityOff /> : <Visibility />}
                </PasswordToggle>
              ),
            }}
          />
        </Box>
        
        <FormRow>
          <FormControlLabel
            control={
              <Checkbox 
                checked={AuthStore.rememberMe}
                onChange={(e) => AuthStore.setRememberMe(e.target.checked)}
                name="rememberMe"
                color="primary"
              />
            }
            label="Запомнить меня"
          />
          <ForgotPasswordLink href="#">
            Забыли пароль?
          </ForgotPasswordLink>
        </FormRow>
        
        {AuthStore.error && (
          <ErrorMessage>
            {AuthStore.error}
          </ErrorMessage>
        )}
        
        <CustomButton
          variant="contained"
          type="submit"
          fullWidth
          disabled={AuthStore.loading}
          id="login-button"
          sx={{ mt: 2, py: 1 }}
        >
          Войти
        </CustomButton>
      </form>
      
      <RegistrationPrompt>
        <Typography variant="body2" component="span">
          У вас еще нет аккаунта?
        </Typography>
        <RegisterLink href="#">
          Зарегистрируйтесь
        </RegisterLink>
      </RegistrationPrompt>
    </FormContainer>
  );
});

const FormContainer = styled.div`
  max-width: 400px;
  width: 100%;
  padding: 16px;
`;

const FormRow = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
`;

const PasswordToggle = styled.div`
  cursor: pointer;
  display: flex;
  align-items: center;
  color: grey;
`;

const ForgotPasswordLink = styled(Link)`
  text-decoration: none;
  color: #2196f3;
  font-size: 14px;
`;

const RegisterLink = styled(Link)`
  text-decoration: none;
  color: #2196f3;
  margin-left: 8px;
  font-weight: 500;
`;

const RegistrationPrompt = styled.div`
  margin-top: 24px;
  text-align: center;
`;

const ErrorMessage = styled.div`
  color: #f44336;
  font-size: 14px;
  margin-bottom: 16px;
`;

export default LoginForm;