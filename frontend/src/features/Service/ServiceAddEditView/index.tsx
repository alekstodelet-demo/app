import React, { FC } from "react";
import { Grid } from "@mui/material";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react";
import { withForm } from "components/hoc/withForm";
import ServiceAddEditBaseView from "./base";
import store from "./store";
import MtmTabs from "./mtmTabs";

/**
 * Interface for component props
 */
interface ServiceProps {
  id: string | null;
}

/**
 * Service form component for adding and editing services
 * @param props - Component props
 */
const ServiceAddEditView: FC<ServiceProps> = observer((props) => {
  const { t } = useTranslation();
  const { id } = props;

  return (
    <ServiceAddEditBaseView>
      {/* Show many-to-many relationship tabs only when editing existing service */}
      {Number(id) > 0 && (
        <Grid item xs={12} spacing={0}>
          <MtmTabs />
        </Grid>
      )}
    </ServiceAddEditBaseView>
  );
});

// Enhance component with form functionality using the HOC
export default withForm(ServiceAddEditView, store, "/user/Service");