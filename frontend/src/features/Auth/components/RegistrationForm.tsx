import React, { useState } from 'react';
import { observer } from 'mobx-react';
import styled from 'styled-components';
import CustomTextField from 'components/TextField';
import CustomButton from 'components/Button';
import registrationStore from '../store/RegistrationStore';

// Компонент для отображения выпадающего списка юр/физ лица
const UserTypeSelector = observer(() => {
  const [isOpen, setIsOpen] = useState(false);
  
  const toggleDropdown = () => {
    setIsOpen(!isOpen);
  };
  
  const selectType = (type: 'Юридическое лицо' | 'Физическое лицо') => {
    registrationStore.setUserType(type);
    setIsOpen(false);
  };
  
  return (
    <SelectorWrapper>
      <SelectorButton onClick={toggleDropdown}>
        <span>{registrationStore.userType}</span>
        <ArrowIcon isOpen={isOpen} />
      </SelectorButton>
      
      {isOpen && (
        <DropdownMenu>
          <DropdownItem 
            onClick={() => selectType('Юридическое лицо')}
            isSelected={registrationStore.userType === 'Юридическое лицо'}
          >
            Юридическое лицо
          </DropdownItem>
          <DropdownItem 
            onClick={() => selectType('Физическое лицо')}
            isSelected={registrationStore.userType === 'Физическое лицо'}
          >
            Физическое лицо
          </DropdownItem>
        </DropdownMenu>
      )}
    </SelectorWrapper>
  );
});

// Основной компонент формы регистрации
const RegistrationForm = observer(() => {
  const [innCount, setInnCount] = useState({ juridical: 0, physical: 0 });
  
  const handleInnChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    
    if (name === 'innJuridical') {
      setInnCount(prev => ({ ...prev, juridical: value.length }));
    } else if (name === 'innPhysical') {
      setInnCount(prev => ({ ...prev, physical: value.length }));
    }
    
    registrationStore.handleChange(e);
  };
  
  const togglePasswordVisibility = (fieldId: string) => {
    const field = document.getElementById(fieldId) as HTMLInputElement;
    if (field) {
      field.type = field.type === 'password' ? 'text' : 'password';
    }
  };
  
  return (
    <FormContainer>
      <FormTitle>Регистрация</FormTitle>
      
      <LoginPasswordLink href="/login">Логин и пароль</LoginPasswordLink>
      
      <UserTypeSelector />
      
      {registrationStore.userType === 'Юридическое лицо' ? (
        <FormField>
          <CustomTextField
            id="innJuridical"
            name="innJuridical"
            label="ИНН юридического лица"
            value={registrationStore.innJuridical}
            onChange={handleInnChange}
            error={!!registrationStore.errors.innJuridical}
            helperText={registrationStore.errors.innJuridical || ''}
          />
          <CharCounter>{innCount.juridical}/14</CharCounter>
        </FormField>
      ) : (
        <FormField>
          <CustomTextField
            id="innPhysical"
            name="innPhysical"
            label="ИНН физического лица"
            value={registrationStore.innPhysical}
            onChange={handleInnChange}
            error={!!registrationStore.errors.innPhysical}
            helperText={registrationStore.errors.innPhysical || ''}
          />
          <CharCounter>{innCount.physical}/14</CharCounter>
        </FormField>
      )}
      
      <SearchButton
        id="searchButton"
        variant="contained"
        onClick={() => console.log('Поиск ИНН')}
      >
        Поиск
      </SearchButton>
      
      <AdditionalField>Наименование юридического лица</AdditionalField>
      <AdditionalField>Код УГНС</AdditionalField>
      <AdditionalField>ФИО физического лица</AdditionalField>
      
      <LinkButton onClick={registrationStore.registerAsInternational}>
        Присоединиться как международная организация
      </LinkButton>
      
      <LinkButton onClick={registrationStore.registerAsBranch}>
        Присоединиться как филиал
      </LinkButton>
      
      <FormField>
        <PasswordContainer>
          <CustomTextField
            id="password"
            name="password"
            type="password"
            label="Пароль"
            value={registrationStore.password}
            onChange={registrationStore.handleChange}
            error={!!registrationStore.errors.password}
            helperText={registrationStore.errors.password || ''}
          />
          <PasswordToggle onClick={() => togglePasswordVisibility('password')}>
            <PasswordIcon />
          </PasswordToggle>
        </PasswordContainer>
      </FormField>
      
      <FormField>
        <PasswordContainer>
          <CustomTextField
            id="confirmPassword"
            name="confirmPassword"
            type="password"
            label="Повторите пароль"
            value={registrationStore.confirmPassword}
            onChange={registrationStore.handleChange}
            error={!!registrationStore.errors.confirmPassword}
            helperText={registrationStore.errors.confirmPassword || ''}
          />
          <PasswordToggle onClick={() => togglePasswordVisibility('confirmPassword')}>
            <PasswordIcon />
          </PasswordToggle>
        </PasswordContainer>
      </FormField>
      
      <SubmitButton
        id="registerButton"
        variant="contained"
        onClick={registrationStore.submitForm}
        disabled={registrationStore.loading}
      >
        {registrationStore.loading ? 'Регистрация...' : 'Зарегистрироваться'}
      </SubmitButton>
      
      {registrationStore.errors.general && (
        <ErrorMessage>{registrationStore.errors.general}</ErrorMessage>
      )}
    </FormContainer>
  );
});

// Стилизованные компоненты
const FormContainer = styled.div`
  width: 100%;
  max-width: 1000px;
  padding: 20px;
  margin: 0 auto;
`;

const FormTitle = styled.h2`
  font-size: 24px;
  font-weight: 600;
  margin-bottom: 20px;
`;

const LoginPasswordLink = styled.a`
  display: block;
  color: #2196f3;
  text-decoration: none;
  margin-bottom: 20px;
  
  &:hover {
    text-decoration: underline;
  }
`;

const SelectorWrapper = styled.div`
  position: relative;
  margin-bottom: 20px;
  width: 100%;
`;

const SelectorButton = styled.button`
  width: 100%;
  padding: 12px 16px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: white;
  border: 1px solid #cdd3ec;
  border-radius: 4px;
  font-size: 16px;
  cursor: pointer;
  text-align: left;
`;

interface ArrowIconProps {
  isOpen: boolean;
}

const ArrowIcon = styled.span<ArrowIconProps>`
  display: inline-block;
  width: 0;
  height: 0;
  border-left: 5px solid transparent;
  border-right: 5px solid transparent;
  border-top: 5px solid #000;
  transform: ${props => props.isOpen ? 'rotate(180deg)' : 'rotate(0)'};
  transition: transform 0.2s ease;
`;

const DropdownMenu = styled.div`
  position: absolute;
  top: 100%;
  left: 0;
  right: 0;
  background-color: white;
  border: 1px solid #cdd3ec;
  border-top: none;
  border-radius: 0 0 4px 4px;
  z-index: 10;
`;

interface DropdownItemProps {
  isSelected: boolean;
}

const DropdownItem = styled.div<DropdownItemProps>`
  padding: 12px 16px;
  cursor: pointer;
  background-color: ${props => props.isSelected ? '#f5f5f5' : 'white'};
  
  &:hover {
    background-color: #f5f5f5;
  }
`;

const FormField = styled.div`
  position: relative;
  margin-bottom: 16px;
`;

const CharCounter = styled.div`
  position: absolute;
  right: 16px;
  bottom: 8px;
  font-size: 12px;
  color: #707882;
`;

const SearchButton = styled(CustomButton)`
  width: 100%;
  margin-bottom: 20px;
  background-color: #2196f3;
  color: white;
`;

const AdditionalField = styled.div`
  padding: 12px 16px;
  margin-bottom: 10px;
  background-color: #f5f6fa;
  border-radius: 4px;
  color: #707882;
`;

const LinkButton = styled.button`
  background: none;
  border: none;
  color: #2196f3;
  text-align: left;
  padding: 8px 0;
  margin-bottom: 8px;
  cursor: pointer;
  width: 100%;
  font-size: 16px;
  
  &:hover {
    text-decoration: underline;
  }
`;

const PasswordContainer = styled.div`
  position: relative;
`;

const PasswordToggle = styled.button`
  position: absolute;
  right: 10px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
`;

const PasswordIcon = styled.div`
  width: 20px;
  height: 20px;
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24'%3E%3Cpath fill='%23707882' d='M12 4.5C7 4.5 2.73 7.61 1 12c1.73 4.39 6 7.5 11 7.5s9.27-3.11 11-7.5c-1.73-4.39-6-7.5-11-7.5zM12 17c-2.76 0-5-2.24-5-5s2.24-5 5-5 5 2.24 5 5-2.24 5-5 5zm0-8c-1.66 0-3 1.34-3 3s1.34 3 3 3 3-1.34 3-3-1.34-3-3-3z'/%3E%3C/svg%3E");
  background-size: contain;
  background-repeat: no-repeat;
`;

const SubmitButton = styled(CustomButton)`
  width: 100%;
  margin-top: 20px;
  background-color: #2196f3;
  color: white;
`;

const ErrorMessage = styled.div`
  color: #f44336;
  margin-top: 16px;
  font-size: 14px;
`;

export default RegistrationForm;