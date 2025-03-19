import React, { useEffect } from 'react';
import { observer } from 'mobx-react';
import { Grid, Box } from '@mui/material';
import styled from 'styled-components';
import AuthSidebar from './components/AuthSidebar';
import LoginForm from './components/LoginForm';
import PromoSidebar from './components/PromoSidebar';
import AuthStore from './store/AuthStore';
import { useNavigate } from 'react-router-dom';

const LoginPage = observer(() => {
  const navigate = useNavigate();
  
  // useEffect(() => {
  //   const checkAuth = async () => {
  //     const isAuthenticated = await AuthStore.checkAuthStatus();
  //     if (isAuthenticated) {
  //       navigate('/user');
  //     }
  //   };
    
  //   checkAuth();
  // }, [navigate]);
  
  const handleLogin = (login: string, password: string, rememberMe: boolean) => {
    AuthStore.setLogin(login);
    AuthStore.setPassword(password);
    AuthStore.setRememberMe(rememberMe);
    AuthStore.loginWithCredentials();
  };
  
  const handleMethodSelect = (method: string) => {
    console.log(`Selected auth method: ${method}`);
    // Здесь можно добавить логику для переключения между методами авторизации
  };
  
  return (
    <PageContainer>
      <Grid container>
        <Grid item xs={12} md={3}>
          <AuthSidebar onMethodSelect={handleMethodSelect} />
        </Grid>
        
        <Grid item xs={12} md={6}>
          <ContentContainer>
            <LogoContainer>
              <img src="/logo/gns-logo.png" alt="ГНС КР" height="80" />
              <img src="/logo/smart-salym-logo.png" alt="Smart Salym" height="50" />
            </LogoContainer>
            
            <FormContainer>
              <LoginForm 
                onLogin={handleLogin}
                loading={AuthStore.loading}
                error={AuthStore.error}
              />
            </FormContainer>
          </ContentContainer>
        </Grid>
        
        <Grid item xs={12} md={3}>
          <PromoSidebar />
        </Grid>
      </Grid>
    </PageContainer>
  );
});

const PageContainer = styled.div`
  min-height: 100vh;
  display: flex;
`;

const ContentContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 24px;
`;

const LogoContainer = styled.div`
  display: flex;
  justify-content: space-around;
  width: 100%;
  margin-bottom: 48px;
  
  img {
    margin: 0 16px;
  }
`;

const FormContainer = styled.div`
  display: flex;
  justify-content: center;
  width: 100%;
`;

export default LoginPage;