import React, { FC } from "react";
import {
  Card,
  CardContent,
  CardHeader,
  Divider,
  Paper,
  Grid,
  Container,
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import store from "./store"
import { observer } from "mobx-react"
import LookUp from 'components/LookUp';
import CustomTextField from "components/TextField";
import CustomCheckbox from "components/Checkbox";
import DateTimeField from "components/DateTimeField";

type CustomerTableProps = {
  children ?: React.ReactNode;
  isPopup ?: boolean;
};

const BaseCustomerView: FC<CustomerTableProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;
  return (
    <Container maxWidth='xl' sx={{ mt: 3 }}>
      <Grid container spacing={3}>
        <Grid item md={props.isPopup ? 12 : 6}>
          <form data-testid="CustomerForm" id="CustomerForm" autoComplete='off'>
            <Card component={Paper} elevation={5}>
              <CardHeader title={
                <span id="Customer_TitleName">
                  {translate('label:CustomerAddEditView.entityTitle')}
                </span>
              } />
              <Divider />
              <CardContent>
                <Grid container spacing={3}>
                  
                  <Grid item md={12} xs={12}>
                    <CustomTextField
                      value={store.pin}
                      onChange={(event) => store.handleChange(event)}
                      name="pin"
                      data-testid="id_f_Customer_pin"
                      id='id_f_Customer_pin'
                      label={translate('label:CustomerAddEditView.pin')}
                      helperText={store.errors.pin}
                      error={!!store.errors.pin}
                    />
                  </Grid>
                  <Grid item md={12} xs={12}>
                    <CustomTextField
                      value={store.okpo}
                      onChange={(event) => store.handleChange(event)}
                      name="okpo"
                      data-testid="id_f_Customer_okpo"
                      id='id_f_Customer_okpo'
                      label={translate('label:CustomerAddEditView.okpo')}
                      helperText={store.errors.okpo}
                      error={!!store.errors.okpo}
                    />
                  </Grid>
                  <Grid item md={12} xs={12}>
                    <CustomTextField
                      value={store.postalCode}
                      onChange={(event) => store.handleChange(event)}
                      name="postalCode"
                      data-testid="id_f_Customer_postalCode"
                      id='id_f_Customer_postalCode'
                      label={translate('label:CustomerAddEditView.postalCode')}
                      helperText={store.errors.postalCode}
                      error={!!store.errors.postalCode}
                    />
                  </Grid>
                  <Grid item md={12} xs={12}>
                    <CustomTextField
                      value={store.ugns}
                      onChange={(event) => store.handleChange(event)}
                      name="ugns"
                      data-testid="id_f_Customer_ugns"
                      id='id_f_Customer_ugns'
                      label={translate('label:CustomerAddEditView.ugns')}
                      helperText={store.errors.ugns}
                      error={!!store.errors.ugns}
                    />
                  </Grid>
                  <Grid item md={12} xs={12}>
                    <CustomTextField
                      value={store.regNumber}
                      onChange={(event) => store.handleChange(event)}
                      name="regNumber"
                      data-testid="id_f_Customer_regNumber"
                      id='id_f_Customer_regNumber'
                      label={translate('label:CustomerAddEditView.regNumber')}
                      helperText={store.errors.regNumber}
                      error={!!store.errors.regNumber}
                    />
                  </Grid>
                  <Grid item md={12} xs={12}>
                    <LookUp
                      value={store.organizationTypeId}
                      onChange={(event) => store.handleChange(event)}
                      name="organizationTypeId"
                      data={store.organizationTypes}
                      id='id_f_Customer_organizationTypeId'
                      label={translate('label:CustomerAddEditView.organizationTypeId')}
                      helperText={store.errors.organizationTypeId}
                      error={!!store.errors.organizationTypeId}
                    />
                  </Grid>
                  <Grid item md={12} xs={12}>
                    <CustomTextField
                      value={store.name}
                      onChange={(event) => store.handleChange(event)}
                      name="name"
                      data-testid="id_f_Customer_name"
                      id='id_f_Customer_name'
                      label={translate('label:CustomerAddEditView.name')}
                      helperText={store.errors.name}
                      error={!!store.errors.name}
                    />
                  </Grid>
                  <Grid item md={12} xs={12}>
                    <CustomTextField
                      value={store.address}
                      onChange={(event) => store.handleChange(event)}
                      name="address"
                      data-testid="id_f_Customer_address"
                      id='id_f_Customer_address'
                      label={translate('label:CustomerAddEditView.address')}
                      helperText={store.errors.address}
                      error={!!store.errors.address}
                    />
                  </Grid>
                  <Grid item md={12} xs={12}>
                    <CustomTextField
                      value={store.director}
                      onChange={(event) => store.handleChange(event)}
                      name="director"
                      data-testid="id_f_Customer_director"
                      id='id_f_Customer_director'
                      label={translate('label:CustomerAddEditView.director')}
                      helperText={store.errors.director}
                      error={!!store.errors.director}
                    />
                  </Grid>
                  <Grid item md={12} xs={12}>
                    <CustomTextField
                      value={store.nomer}
                      onChange={(event) => store.handleChange(event)}
                      name="nomer"
                      data-testid="id_f_Customer_nomer"
                      id='id_f_Customer_nomer'
                      label={translate('label:CustomerAddEditView.nomer')}
                      helperText={store.errors.nomer}
                      error={!!store.errors.nomer}
                    />
                  </Grid>
                </Grid>
              </CardContent>
            </Card>
          </form>
        </Grid>
        {props.children}
      </Grid>
    </Container>
  );
})

export default BaseCustomerView;
