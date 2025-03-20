import React, { FC } from "react";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react";
import { withForm } from "components/hoc/withForm";
import OrganizationContactAddEditBaseView from "./base";
import store from "./store";

/**
 * Interface for component props
 */
interface OrganizationContactProps {
  id: string | null;
}

/**
 * OrganizationContact form component for adding and editing organization contacts
 * @param props - Component props
 */
const OrganizationContactAddEditView: FC<OrganizationContactProps> = observer((props) => {
  const { t } = useTranslation();

  return (
    <OrganizationContactAddEditBaseView />
  );
});

// Enhance component with form functionality using the HOC
export default withForm(OrganizationContactAddEditView, store, "/user/OrganizationContact");