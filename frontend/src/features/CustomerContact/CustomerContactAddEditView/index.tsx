import React, { FC } from "react";
import { Grid } from "@mui/material";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react";
import { withForm } from "components/hoc/withForm";
import CustomerContactAddEditBaseView from "./base";
import store from "./store";
import MtmTabs from "./mtmTabs";

/**
 * Interface for component props
 */
interface CustomerContactProps {
  id: string | null;
}

/**
 * CustomerContact form component for adding and editing CustomerContacts
 * @param props - Component props
 */
const CustomerContactAddEditView: FC<CustomerContactProps> = observer((props) => {
  const { t } = useTranslation();
  const { id } = props;

  return (
    <CustomerContactAddEditBaseView>
      {/* Show many-to-many relationship tabs only when editing existing CustomerContact */}
      {Number(id) > 0 && (
        <Grid item xs={12} spacing={0}>
          <MtmTabs />
        </Grid>
      )}
    </CustomerContactAddEditBaseView>
  );
});

// Enhance component with form functionality using the HOC
export default withForm(CustomerContactAddEditView, store, "/user/CustomerContact");