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
                    name="name"
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
