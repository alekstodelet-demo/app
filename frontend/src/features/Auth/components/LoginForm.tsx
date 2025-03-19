import React, { FC, useState } from 'react';
import { observer } from 'mobx-react';
import { Box, Typography, FormControlLabel, Checkbox, Link } from '@mui/material';
import CustomTextField from 'components/TextField';
import CustomButton from 'components/Button';
import styled from 'styled-components';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import { useTranslation } from 'react-i18next';

interface LoginFormProps {
  onLogin: (login: string, password: string, rememberMe: boolean) => void;
  loading?: boolean;
  error?: string;
}

const LoginForm: FC<LoginFormProps> = observer(({ onLogin, loading, error }) => {
  const { t } = useTranslation();
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onLogin(login, password, rememberMe);
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
            value={login}
            onChange={(e) => setLogin(e.target.value)}
            id="login-input"
            name="login"
            // fullWidth
            // autoComplete="username"
          />
        </Box>
        
        <Box mb={2}>
          <CustomTextField
            label="Пароль"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            type={showPassword ? "text" : "password"}
            id="password-input"
            name="password"
            // fullWidth
            // autoComplete="current-password"
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
                checked={rememberMe}
                onChange={(e) => setRememberMe(e.target.checked)}
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
        
        {error && (
          <ErrorMessage>
            {error}
          </ErrorMessage>
        )}
        
        <CustomButton
          variant="contained"
          type="submit"
          fullWidth
          disabled={loading}
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