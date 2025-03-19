import React, { FC } from 'react';
import styled from 'styled-components';
import { Box, Typography } from '@mui/material';
import PromoCard from './PromoCard';
import QrCodeIcon from '@mui/icons-material/QrCode';
import CustomButton from 'components/Button';

const PromoSidebar: FC = () => {
  return (
    <SidebarContainer>
      <PromoCard
        title="Наше предложение"
        backgroundColor="#E3F2FD"
        buttonText="Подать заявление"
        onButtonClick={() => console.log('Подать заявление')}
        image={<img src="/illustrations/register-illustration.png" alt="Регистрация ИП" width="200" />}
      >
        <Typography variant="h5" mb={2}>
          Хочешь открыть ИП?
        </Typography>
        <Typography variant="body2" mb={1}>
          Подать заявление на регистрацию ИП.
        </Typography>
        <Typography variant="body2" mb={3}>
          Подать заявление на регистрацию физического лица.
        </Typography>
      </PromoCard>

      <PromoCard backgroundColor="#FFFFFF" title="Скачай приложение">
        <Box display="flex" flexDirection="column" alignItems="center">
          <QrCodeContainer>
            <Box>
              <QrCodeIcon style={{ fontSize: 80 }} />
              <Typography variant="caption">СКАНИРУЙ</Typography>
            </Box>
            <Box>
              <QrCodeIcon style={{ fontSize: 80 }} />
              <Typography variant="caption">СКАНИРУЙ</Typography>
            </Box>
          </QrCodeContainer>
          
          <Typography variant="h6" mt={2} mb={1}>
            Salyk.kg
          </Typography>
          <Typography variant="body2" align="center">
            Мобильное приложение
          </Typography>
          <Typography variant="body2" align="center" mb={2}>
            Кабинет налогоплательщика
          </Typography>
          
          <AppStoreButtons>
            <img src="/icons/app-store.png" alt="App Store" height="40" />
            <img src="/icons/google-play.png" alt="Google Play" height="40" />
          </AppStoreButtons>
        </Box>
      </PromoCard>

      <PromoCard backgroundColor="#673AB7" title="Программная касса «ЭСЕП»">
        <Box display="flex" justifyContent="space-between">
          <Box>
            <Typography variant="h6" color="white">
              Программная касса «ЭСЕП»
            </Typography>
            <Typography variant="body2" color="white">
              для Вашего Бизнеса
            </Typography>
          </Box>
          <img src="/images/cash-register.png" alt="ЭСЕП касса" height="120" />
        </Box>
      </PromoCard>
    </SidebarContainer>
  );
};

const SidebarContainer = styled.div`
  padding: 24px;
  height: 100%;
  min-height: 100vh;
  background-color: #F5F5F5;
`;

const QrCodeContainer = styled.div`
  display: flex;
  justify-content: space-around;
  width: 100%;
  margin: 16px 0;
  
  > div {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
`;

const AppStoreButtons = styled.div`
  display: flex;
  justify-content: center;
  gap: 12px;
  margin-top: 16px;
`;

export default PromoSidebar;