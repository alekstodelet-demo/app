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
import CustomTextField from "components/TextField";

type OrganizationTypeAddEditProps = {
  children?: React.ReactNode;
  isPopup?: boolean;
};

const BaseView: FC<OrganizationTypeAddEditProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;
  
  return (
    <Container maxWidth='xl' style={{ marginTop: 20 }}>
      <form id="OrganizationTypeForm" autoComplete='off'>
        <Paper elevation={7}>
          <Card>
            <CardHeader title={
              <span id="OrganizationType_TitleName">
                {translate('label:OrganizationTypeAddEditView.entityTitle')}
              </span>
            } />
            <Divider />
            <CardContent>
              <Grid container spacing={3}>
                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.name}
                    error={!!store.errors.name}
                    id='id_f_OrganizationType_name'
                    label={translate('label:OrganizationTypeAddEditView.name')}
                    value={store.name}
                    onChange={(event) => store.handleChange(event)}
                    name="name"
                  />
                </Grid>
                
                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.code}
                    error={!!store.errors.code}
                    id='id_f_OrganizationType_code'
                    label={translate('label:OrganizationTypeAddEditView.code')}
                    value={store.code}
                    onChange={(event) => store.handleChange(event)}
                    name="code"
                  />
                </Grid>

                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.description}
                    error={!!store.errors.description}
                    id='id_f_OrganizationType_description'
                    label={translate('label:OrganizationTypeAddEditView.description')}
                    value={store.description}
                    multiline
                    rows={3}
                    onChange={(event) => store.handleChange(event)}
                    name="description"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.name_kg}
                    error={!!store.errors.name_kg}
                    id='id_f_OrganizationType_name_kg'
                    label={translate('label:OrganizationTypeAddEditView.name_kg')}
                    value={store.name_kg}
                    onChange={(event) => store.handleChange(event)}
                    name="name_kg"
                  />
                </Grid>

                <Grid item md={12} xs={12}>
                  <CustomTextField
                    helperText={store.errors.description_kg}
                    error={!!store.errors.description_kg}
                    id='id_f_OrganizationType_description_kg'
                    label={translate('label:OrganizationTypeAddEditView.description_kg')}
                    value={store.description_kg}
                    multiline
                    rows={3}
                    onChange={(event) => store.handleChange(event)}
                    name="description_kg"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.text_color}
                    error={!!store.errors.text_color}
                    id='id_f_OrganizationType_text_color'
                    label={translate('label:OrganizationTypeAddEditView.text_color')}
                    value={store.text_color}
                    onChange={(event) => store.handleChange(event)}
                    name="text_color"
                    type="color"
                  />
                </Grid>

                <Grid item md={6} xs={12}>
                  <CustomTextField
                    helperText={store.errors.background_color}
                    error={!!store.errors.background_color}
                    id='id_f_OrganizationType_background_color'
                    label={translate('label:OrganizationTypeAddEditView.background_color')}
                    value={store.background_color}
                    onChange={(event) => store.handleChange(event)}
                    name="background_color"
                    type="color"
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