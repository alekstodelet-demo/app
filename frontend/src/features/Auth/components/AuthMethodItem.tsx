import React, { FC } from 'react';
import { Box, Typography } from '@mui/material';
import styled from 'styled-components';

interface AuthMethodItemProps {
  icon: React.ReactNode;
  title: string;
  subtitle: string;
  onClick?: () => void;
  id?: string;
}

const AuthMethodItem: FC<AuthMethodItemProps> = ({ icon, title, subtitle, onClick, id }) => {
  return (
    <MethodContainer id={id} onClick={onClick}>
      <IconWrapper>
        {icon}
      </IconWrapper>
      <TextWrapper>
        <Typography variant="subtitle1" color="white" fontWeight={500}>
          {title}
        </Typography>
        <Typography variant="caption" color="white">
          {subtitle}
        </Typography>
      </TextWrapper>
    </MethodContainer>
  );
};

const MethodContainer = styled.div`
  display: flex;
  align-items: center;
  padding: 16px;
  border: 1px dashed rgba(255, 255, 255, 0.3);
  border-radius: 8px;
  margin-bottom: 12px;
  cursor: pointer;
  transition: background-color 0.2s;
  
  &:hover {
    background-color: rgba(255, 255, 255, 0.1);
  }
`;

const IconWrapper = styled.div`
  margin-right: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
`;

const TextWrapper = styled.div`
  display: flex;
  flex-direction: column;
`;

export default AuthMethodItem;