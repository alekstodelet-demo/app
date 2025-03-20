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

type CustomerContactAddEditProps = {
  children?: React.ReactNode;
  isPopup?: boolean;
};


const BaseView: FC<CustomerContactAddEditProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;
  return (
    <Container maxWidth='xl' style={{ marginTop: 20 }}>

      <form id="CustomerContactForm" autoComplete='off'>
        <Paper elevation={7}  >
          <Card>
            <CardHeader title={
              <span id="CustomerContact_TitleName">
                {translate('label:CustomerContactAddEditView.entityTitle')}
              </span>
            } />
            <Divider />
            <CardContent>
              <Grid container spacing={3}>
                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.value}
                    error={!!store.errors.value}
                    id='id_f_CustomerContact_value'
                    label={translate('label:CustomerContactAddEditView.value')}
                    value={store.value}
                    onChange={(event) => store.handleChange(event)}
                    name="value"
                  />
                </Grid>
                <Grid item md={12} xs={12}>
                  <CustomCheckbox
                    value={store.allow_notification}
                    onChange={(event) => store.handleChange(event)}
                    name="allow_notification"
                    label={translate('label:CustomerContactAddEditView.allow_notification')}
                    id='id_f_allow_notification'
                  />
                </Grid>

                <Grid item md={12} xs={12}>
                  <LookUp
                    value={store.type_id}
                    onChange={(event) => store.handleChange(event)}
                    name="type_id"
                    data={store.ContactTypes}
                    id='id_f_CustomerContact_type_id'
                    label={translate('label:ServiceAddEditView.type_id')}
                    helperText={store.errors.type_id}
                    error={!!store.errors.type_id}
                  />
                </Grid>

              </Grid>
            </CardContent>
          </Card>
        </Paper>
      </form>
      {props.children}
    </Container>
  );
})


export default BaseView;
