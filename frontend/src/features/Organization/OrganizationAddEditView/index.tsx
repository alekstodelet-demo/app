import React, { FC } from "react";
import { Grid } from "@mui/material";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react";
import { withForm } from "components/hoc/withForm";
import OrganizationAddEditBaseView from "./base";
import store from "./store";
import MtmTabs from "./mtmTabs";

/**
 * Interface for component props
 */
interface OrganizationProps {
  id: string | null;
}

/**
 * Organization form component for adding and editing organizations
 * @param props - Component props
 */
const OrganizationAddEditView: FC<OrganizationProps> = observer((props) => {
  const { t } = useTranslation();
  const { id } = props;

  return (
    <OrganizationAddEditBaseView>
      {/* Show many-to-many relationship tabs only when editing existing organization */}
      {Number(id) > 0 && (
        <Grid item xs={12} spacing={0}>
          <MtmTabs />
        </Grid>
      )}
    </OrganizationAddEditBaseView>
  );
});

// Enhance component with form functionality using the HOC
export default withForm(OrganizationAddEditView, store, "/user/Organization");