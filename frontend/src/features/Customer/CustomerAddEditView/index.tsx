import React, { FC } from "react";
import { Grid } from "@mui/material";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react";
import { withForm } from "components/hoc/withForm";
import CustomerAddEditBaseView from "./base";
import store from "./store";
import MtmTabs from "./mtmTabs";

/**
 * Interface for component props
 */
interface CustomerProps {
  id: string | null;
}

/**
 * Customer form component for adding and editing Customers
 * @param props - Component props
 */
const CustomerAddEditView: FC<CustomerProps> = observer((props) => {
  const { t } = useTranslation();
  const { id } = props;

  return (
    <CustomerAddEditBaseView>
      {/* Show many-to-many relationship tabs only when editing existing Customer */}
      {Number(id) > 0 && (
        <Grid item xs={12} spacing={0}>
          <MtmTabs />
        </Grid>
      )}
    </CustomerAddEditBaseView>
  );
});

// Enhance component with form functionality using the HOC
export default withForm(CustomerAddEditView, store, "/user/Customer");