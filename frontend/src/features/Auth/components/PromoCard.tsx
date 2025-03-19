import React, { FC, ReactNode } from 'react';
import { Card, CardContent, Typography, Box } from '@mui/material';
import styled from 'styled-components';
import CustomButton from 'components/Button';

interface PromoCardProps {
  title: string;
  children: ReactNode;
  buttonText?: string;
  onButtonClick?: () => void;
  image?: ReactNode;
  backgroundColor?: string;
}

const PromoCard: FC<PromoCardProps> = ({
  title,
  children,
  buttonText,
  onButtonClick,
  image,
  backgroundColor = '#E3F2FD'
}) => {
  return (
    <StyledCard $backgroundColor={backgroundColor}>
      <StyledCardContent>
        <Typography variant="h6" mb={1}>
          {title}
        </Typography>
        <Box mb={2}>
          {children}
        </Box>
        {buttonText && (
          <CustomButton
            variant="contained"
            onClick={onButtonClick}
            fullWidth
          >
            {buttonText}
          </CustomButton>
        )}
      </StyledCardContent>
      {image && (
        <ImageContainer>
          {image}
        </ImageContainer>
      )}
    </StyledCard>
  );
};

const StyledCard = styled(Card)<{ $backgroundColor: string }>`
  position: relative;
  overflow: hidden;
  margin-bottom: 16px;
  background-color: ${props => props.$backgroundColor};
  border-radius: 12px;
`;

const StyledCardContent = styled(CardContent)`
  position: relative;
  z-index: 1;
`;

const ImageContainer = styled.div`
  position: absolute;
  right: 0;
  bottom: 0;
  max-width: 50%;
  z-index: 0;
`;

export default PromoCard;