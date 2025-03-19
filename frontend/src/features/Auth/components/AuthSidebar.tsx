import React, { FC } from 'react';
import { Box, Typography } from '@mui/material';
import styled from 'styled-components';
import AuthMethodItem from './AuthMethodItem';
import KeyIcon from '@mui/icons-material/VpnKey';
import SmartphoneIcon from '@mui/icons-material/Smartphone';
import QrCodeIcon from '@mui/icons-material/QrCode';
import AccountBoxIcon from '@mui/icons-material/AccountBox';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import MemoryIcon from '@mui/icons-material/Memory';
import AppRegistrationIcon from '@mui/icons-material/AppRegistration';
import { AuthFormType } from '../store/AuthStore';

interface AuthSidebarProps {
  onMethodSelect: (method: AuthFormType) => void;
}

const AuthSidebar: FC<AuthSidebarProps> = ({ onMethodSelect }) => {
  return (
    <SidebarContainer>
      <Box mb={4}>
        <Typography variant="h6" color="white" mb={3}>
          Кабинет налогоплательщика
        </Typography>
      </Box>
      
      <Box mb={4}>
        <Typography variant="subtitle2" color="white" mb={2}>
          Войти через
        </Typography>
        
        <AuthMethodItem
          id="login-password-method"
          icon={<KeyIcon fontSize="large" style={{ color: 'white' }} />}
          title="Логин и пароль"
          subtitle="Логин(ИНН) и пароль"
          onClick={() => onMethodSelect('login')}
        />
        
        <AuthMethodItem
          id="rutoken-method"
          icon={<MemoryIcon fontSize="large" style={{ color: 'white' }} />}
          title="Рутокен"
          subtitle="Электронная подпись на USB носителе"
          onClick={() => onMethodSelect('rutoken')}
        />
        
        <AuthMethodItem
          id="jacarta-method"
          icon={<MemoryIcon fontSize="large" style={{ color: 'white' }} />}
          title="JaCarta"
          subtitle="Электронная подпись на USB носителе"
          onClick={() => onMethodSelect('jacarta')}
        />
        
        <AuthMethodItem
          id="enotoken-method"
          icon={<MemoryIcon fontSize="large" style={{ color: 'white' }} />}
          title="EnoToken"
          subtitle="Электронная подпись на USB носителе"
          onClick={() => onMethodSelect('enotoken')}
        />
        
        <AuthMethodItem
          id="smart-id-method"
          icon={<QrCodeIcon fontSize="large" style={{ color: 'white' }} />}
          title="Smart-ID KG"
          subtitle="Аутентификация по QR-коду через приложение Smart-ID KG"
          onClick={() => onMethodSelect('smartid')}
        />
        
        <AuthMethodItem
          id="eci-cloud-method"
          icon={<AccountBoxIcon fontSize="large" style={{ color: 'white' }} />}
          title="ЕСИ-Облако"
          subtitle="Единая система идентификации"
          onClick={() => onMethodSelect('esicloud')}
        />
        
        <AuthMethodItem
          id="ettn-method"
          icon={<ShoppingCartIcon fontSize="large" style={{ color: 'white' }} />}
          title="ЭТТН"
          subtitle="Электронная товарно-транспортная накладная"
          onClick={() => onMethodSelect('ettn')}
        />
      </Box>
      
      <Box>
        <Typography variant="subtitle2" color="white" mb={2}>
          Регистрация
        </Typography>
        
        <AuthMethodItem
          id="registration-method"
          icon={<AppRegistrationIcon fontSize="large" style={{ color: 'white' }} />}
          title="Зарегистрироваться"
          subtitle="Регистрация в кабинете налогоплательщика"
          onClick={() => onMethodSelect('registration')}
        />
      </Box>
    </SidebarContainer>
  );
};

const SidebarContainer = styled.div`
  background-color: #2196f3;
  padding: 24px;
  height: 100%;
  min-height: 100vh;
  color: white;
`;

export default AuthSidebar;