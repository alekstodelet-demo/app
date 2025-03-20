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

type OrganizationContactAddEditProps = {
  children?: React.ReactNode;
  isPopup?: boolean;
};

const BaseView: FC<OrganizationContactAddEditProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;
  
  return (
    <Container maxWidth='xl' style={{ marginTop: props.isPopup ? 0 : 20 }}>
      <form id="OrganizationContactForm" autoComplete='off'>
        <Paper elevation={7}>
          <Card>
            <CardHeader title={
              <span id="OrganizationContact_TitleName">
                {translate('label:OrganizationContactAddEditView.entityTitle')}
              </span>
            } />
            <Divider />
            <CardContent>
              <Grid container spacing={3}>
                <Grid item md={6} xs={12}>
                  <LookUp
                    value={store.t_type_id}
                    onChange={(event) => store.handleChange(event)}
                    name="t_type_id"
                    data={store.ContactTypes}
                    id='id_f_OrganizationContact_t_type_id'
                    label={translate('label:OrganizationContactAddEditView.t_type_id')}
                    helperText={store.errors.t_type_id}
                    error={!!store.errors.t_type_id}
                  />
                </Grid>
                
                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.value}
                    error={!!store.errors.value}
                    id='id_f_OrganizationContact_value'
                    label={translate('label:OrganizationContactAddEditView.value')}
                    value={store.value}
                    onChange={(event) => store.handleChange(event)}
                    name="value"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomCheckbox
                    value={store.allow_notification}
                    onChange={(event) => store.handleChange(event)}
                    name="allow_notification"
                    label={translate('label:OrganizationContactAddEditView.allow_notification')}
                    id='id_f_OrganizationContact_allow_notification'
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