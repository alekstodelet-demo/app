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

type OrganizationAddEditProps = {
  children?: React.ReactNode;
  isPopup?: boolean;
};

const BaseView: FC<OrganizationAddEditProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;
  
  return (
    <Container maxWidth='xl' style={{ marginTop: 20 }}>
      <form id="OrganizationForm" autoComplete='off'>
        <Paper elevation={7}>
          <Card>
            <CardHeader title={
              <span id="Organization_TitleName">
                {translate('label:OrganizationAddEditView.entityTitle')}
              </span>
            } />
            <Divider />
            <CardContent>
              <Grid container spacing={3}>
                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.name}
                    error={!!store.errors.name}
                    id='id_f_Organization_name'
                    label={translate('label:OrganizationAddEditView.name')}
                    value={store.name}
                    onChange={(event) => store.handleChange(event)}
                    name="name"
                  />
                </Grid>
                
                <Grid item md={6} xs={12}>
                  <LookUp
                    value={store.organization_type_id}
                    onChange={(event) => store.handleChange(event)}
                    name="organization_type_id"
                    data={store.OrganizationTypes}
                    id='id_f_Organization_organization_type_id'
                    label={translate('label:OrganizationAddEditView.organization_type_id')}
                    helperText={store.errors.organization_type_id}
                    error={!!store.errors.organization_type_id}
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.address}
                    error={!!store.errors.address}
                    id='id_f_Organization_address'
                    label={translate('label:OrganizationAddEditView.address')}
                    value={store.address}
                    onChange={(event) => store.handleChange(event)}
                    name="address"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.director}
                    error={!!store.errors.director}
                    id='id_f_Organization_director'
                    label={translate('label:OrganizationAddEditView.director')}
                    value={store.director}
                    onChange={(event) => store.handleChange(event)}
                    name="director"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.nomer}
                    error={!!store.errors.nomer}
                    id='id_f_Organization_nomer'
                    label={translate('label:OrganizationAddEditView.nomer')}
                    value={store.nomer}
                    onChange={(event) => store.handleChange(event)}
                    name="nomer"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.pin}
                    error={!!store.errors.pin}
                    id='id_f_Organization_pin'
                    label={translate('label:OrganizationAddEditView.pin')}
                    value={store.pin}
                    onChange={(event) => store.handleChange(event)}
                    name="pin"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.okpo}
                    error={!!store.errors.okpo}
                    id='id_f_Organization_okpo'
                    label={translate('label:OrganizationAddEditView.okpo')}
                    value={store.okpo}
                    onChange={(event) => store.handleChange(event)}
                    name="okpo"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.postal_code}
                    error={!!store.errors.postal_code}
                    id='id_f_Organization_postal_code'
                    label={translate('label:OrganizationAddEditView.postal_code')}
                    value={store.postal_code}
                    onChange={(event) => store.handleChange(event)}
                    name="postal_code"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.ugns}
                    error={!!store.errors.ugns}
                    id='id_f_Organization_ugns'
                    label={translate('label:OrganizationAddEditView.ugns')}
                    value={store.ugns}
                    onChange={(event) => store.handleChange(event)}
                    name="ugns"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.reg_number}
                    error={!!store.errors.reg_number}
                    id='id_f_Organization_reg_number'
                    label={translate('label:OrganizationAddEditView.reg_number')}
                    value={store.reg_number}
                    onChange={(event) => store.handleChange(event)}
                    name="reg_number"
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