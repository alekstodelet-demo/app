import React, { FC, useState } from "react";
import { useNavigate } from 'react-router-dom';
import { useLocation } from 'react-router';
import {
  Card,
  CardContent,
  CardHeader,
  Divider,
  Paper,
  Grid,
  Button,
  makeStyles,
  FormControlLabel,
  Container,
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import store from "./store"
import { observer } from "mobx-react"
import LookUp from 'components/LookUp';
import CustomTextField from "components/TextField";
import CustomCheckbox from "components/Checkbox";
import DateField from "components/DateField";

type CustomerAddEditProps = {
  children?: React.ReactNode;
  isPopup?: boolean;
};


const BaseView: FC<CustomerAddEditProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;
  return (
    <Container maxWidth='xl' style={{ marginTop: 20 }}>

      <form id="CustomerForm" autoComplete='off'>
        <Paper elevation={7}  >
          <Card>
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
                    helperText={store.errors.pin}
                    error={!!store.errors.pin}
                    id='id_f_Customer_pin'
                    label={translate('label:CustomerAddEditView.pin')}
                    value={store.pin}
                    onChange={(event) => store.handleChange(event)}
                    name="pin"
                  />
                </Grid>
                <Grid item md={12} xs={12}>
                  <CustomCheckbox
                    value={store.is_organization}
                    onChange={(event) => store.handleChange(event)}
                    name="is_organization"
                    label={translate('label:CustomerAddEditView.is_organization')}
                    id='id_f_is_organization'
                  />
                </Grid>


                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.full_name}
                    error={!!store.errors.full_name}
                    id='id_f_Customer_full_name'
                    label={translate('label:CustomerAddEditView.full_name')}
                    value={store.full_name}
                    onChange={(event) => store.handleChange(event)}
                    name="full_name"
                  />
                </Grid>

                <Grid item md={12} xs={12}>
                  <DateField
                    value={store.document_date_issue}
                    onChange={(event) => store.handleChange(event)}
                    name="document_date_issue"
                    id="id_f_customer_document_date_issue"
                    label={translate("label:CustomerAddEditView.document_date_issue")}
                    helperText={store.errors.document_date_issue}
                    error={!!store.errors.document_date_issue}
                  />
                </Grid>
                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.address}
                    error={!!store.errors.address}
                    id='id_f_Customer_address'
                    label={translate('label:CustomerAddEditView.address')}
                    value={store.address}
                    onChange={(event) => store.handleChange(event)}
                    name="address"
                  />
                </Grid>


                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.postal_code}
                    error={!!store.errors.postal_code}
                    id='id_f_Customer_postal_code'
                    label={translate('label:CustomerAddEditView.postal_code')}
                    value={store.postal_code}
                    onChange={(event) => store.handleChange(event)}
                    name="postal_code"
                  />
                </Grid>

                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.director}
                    error={!!store.errors.director}
                    id='id_f_Customer_director'
                    label={translate('label:CustomerAddEditView.director')}
                    value={store.director}
                    onChange={(event) => store.handleChange(event)}
                    name="director"
                  />
                </Grid>
                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.okpo}
                    error={!!store.errors.okpo}
                    id='id_f_Customer_okpo'
                    label={translate('label:CustomerAddEditView.okpo')}
                    value={store.okpo}
                    onChange={(event) => store.handleChange(event)}
                    name="okpo"
                  />
                </Grid>

                {/* <Grid item md={12} xs={12}>
                  <LookUp
                    helperText={store.errors.organization_type_id}
                    error={!!store.errors.organization_type_id}
                    data={store.OrganizationTypes}
                    id='id_f_Customer_organization_type_id'
                    label={translate('label:CustomerAddEditView.organization_type_id')}
                    value={store.organization_type_id}
                    onChange={(event) => store.handleChange(event)}
                    name="organization_type_id"
                  />
                </Grid> */}
                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.ugns}
                    error={!!store.errors.ugns}
                    id='id_f_Customer_ugns'
                    label={translate('label:CustomerAddEditView.ugns')}
                    value={store.ugns}
                    onChange={(event) => store.handleChange(event)}
                    name="ugns"
                  />
                </Grid>


                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.payment_account}
                    error={!!store.errors.payment_account}
                    id='id_f_Customer_payment_account'
                    label={translate('label:CustomerAddEditView.payment_account')}
                    value={store.payment_account}
                    onChange={(event) => store.handleChange(event)}
                    name="payment_account"
                  />
                </Grid>

                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.bank}
                    error={!!store.errors.bank}
                    id='id_f_Customer_bank'
                    label={translate('label:CustomerAddEditView.bank')}
                    value={store.bank}
                    onChange={(event) => store.handleChange(event)}
                    name="bank"
                  />
                </Grid>
                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.bik}
                    error={!!store.errors.bik}
                    id='id_f_Customer_bik'
                    label={translate('label:CustomerAddEditView.bik')}
                    value={store.bik}
                    onChange={(event) => store.handleChange(event)}
                    name="bik"
                  />
                </Grid>
                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.registration_number}
                    error={!!store.errors.registration_number}
                    id='id_f_Customer_registration_number'
                    label={translate('label:CustomerAddEditView.registration_number')}
                    value={store.registration_number}
                    onChange={(event) => store.handleChange(event)}
                    name="registration_number"
                  />
                </Grid>

              </Grid>
            </CardContent>
          </Card>
        </Paper>
      </form>
      {props.children}
    </Container >
  );
})


export default BaseView;
