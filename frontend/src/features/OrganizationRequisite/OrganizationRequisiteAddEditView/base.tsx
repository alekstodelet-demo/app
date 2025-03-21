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

type OrganizationRequisiteAddEditProps = {
  children?: React.ReactNode;
  isPopup?: boolean;
};

const BaseView: FC<OrganizationRequisiteAddEditProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;
  
  return (
    <Container maxWidth='xl' style={{ marginTop: 20 }}>
      <form id="OrganizationRequisiteForm" autoComplete='off'>
        <Paper elevation={7}>
          <Card>
            <CardHeader title={
              <span id="OrganizationRequisite_TitleName">
                {translate('label:OrganizationRequisiteAddEditView.entityTitle')}
              </span>
            } />
            <Divider />
            <CardContent>
              <Grid container spacing={3}>
                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.payment_account}
                    error={!!store.errors.payment_account}
                    id='id_f_OrganizationRequisite_payment_account'
                    label={translate('label:OrganizationRequisiteAddEditView.payment_account')}
                    value={store.payment_account}
                    onChange={(event) => store.handleChange(event)}
                    name="payment_account"
                  />
                </Grid>
                
                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.bank}
                    error={!!store.errors.bank}
                    id='id_f_OrganizationRequisite_bank'
                    label={translate('label:OrganizationRequisiteAddEditView.bank')}
                    value={store.bank}
                    onChange={(event) => store.handleChange(event)}
                    name="bank"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.bik}
                    error={!!store.errors.bik}
                    id='id_f_OrganizationRequisite_bik'
                    label={translate('label:OrganizationRequisiteAddEditView.bik')}
                    value={store.bik}
                    onChange={(event) => store.handleChange(event)}
                    name="bik"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.organization_id}
                    error={!!store.errors.organization_id}
                    id='id_f_OrganizationRequisite_organization_id'
                    label={translate('label:OrganizationRequisiteAddEditView.organization_id')}
                    value={store.organization_id}
                    onChange={(event) => store.handleChange(event)}
                    name="organization_id"
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