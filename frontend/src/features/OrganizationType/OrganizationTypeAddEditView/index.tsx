import React, { FC } from "react";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react";
import { withForm } from "components/hoc/withForm";
import OrganizationTypeAddEditBaseView from "./base";
import store from "./store";

/**
 * Interface for component props
 */
interface OrganizationTypeProps {
  id: string | null;
}

/**
 * OrganizationType form component for adding and editing organization types
 * @param props - Component props
 */
const OrganizationTypeAddEditView: FC<OrganizationTypeProps> = observer((props) => {
  const { t } = useTranslation();

  return (
    <OrganizationTypeAddEditBaseView />
  );
});

// Enhance component with form functionality using the HOC
export default withForm(OrganizationTypeAddEditView, store, "/user/OrganizationType");