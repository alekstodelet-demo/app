import React, { useEffect } from 'react';
import { observer } from 'mobx-react';
import { Grid, Box } from '@mui/material';
import styled from 'styled-components';
import AuthSidebar from './components/AuthSidebar';
import LoginForm from './components/LoginForm';
import RegistrationForm from './components/RegistrationForm';
import PromoSidebar from './components/PromoSidebar';
import AuthStore from './store/AuthStore';
import { useNavigate } from 'react-router-dom';

const LoginPage = observer(() => {
  const navigate = useNavigate();
  
  // useEffect(() => {
  //   const checkAuth = async () => {
  //     try {
  //       const isAuthenticated = await AuthStore.checkAuthStatus();
  //       if (isAuthenticated) {
  //         navigate('/user');
  //       }
  //     } catch (error) {
  //       console.error("Ошибка проверки авторизации:", error);
  //       // Здесь можно добавить обработку ошибки, например, показать уведомление
  //     }
  //   };
    
  //   checkAuth();
  // }, [navigate]);
  
  const renderAuthForm = () => {
    switch (AuthStore.currentForm) {
      case 'login':
        return <LoginForm />;
      case 'registration':
        return <RegistrationForm />;
      case 'rutoken':
      case 'jacarta':
      case 'enotoken':
      case 'smartid':
      case 'esicloud':
      case 'ettn':
        return (
          <ComingSoonContainer>
            <ComingSoonTitle>В разработке</ComingSoonTitle>
            <ComingSoonText>
              Данный метод авторизации находится в разработке и будет доступен в ближайшее время
            </ComingSoonText>
          </ComingSoonContainer>
        );
      default:
        return <LoginForm />;
    }
  };

  return (
    <PageContainer>
      <Grid container>
        <Grid item xs={12} md={3}>
          <AuthSidebar onMethodSelect={(method) => AuthStore.setCurrentForm(method)} />
        </Grid>
        
        <Grid item xs={12} md={6}>
          <ContentContainer>
            <LogoContainer>
              <img src="/logo/gns-logo.png" alt="ГНС КР" height="80" />
              <img src="/logo/smart-salym-logo.png" alt="Smart Salym" height="50" />
            </LogoContainer>
            
            <FormContainer>
              {renderAuthForm()}
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

const ComingSoonContainer = styled.div`
  text-align: center;
  padding: 40px;
  background: #f5f5f5;
  border-radius: 8px;
  margin: 20px;
`;

const ComingSoonTitle = styled.h2`
  color: #2196f3;
  margin-bottom: 16px;
`;

const ComingSoonText = styled.p`
  color: #666;
  font-size: 16px;
`;

export default LoginPage;