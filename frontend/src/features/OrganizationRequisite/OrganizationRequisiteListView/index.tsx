import React, { FC, useEffect } from 'react';
import { observer } from "mobx-react";
import { useTranslation } from 'react-i18next';
import { GridColDef } from '@mui/x-data-grid';
import BaseListView from 'components/common/BaseListView';
import OrganizationRequisitePopupForm from '../OrganizationRequisiteAddEditView/popupForm';
import store from "./store";

interface OrganizationRequisiteListProps {
  mainId?: number;
}

/**
 * OrganizationRequisite list view component that displays all OrganizationRequisites
 */
const OrganizationRequisiteListView: FC<OrganizationRequisiteListProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;

  // Define grid columns
  const columns: GridColDef[] = [
    { field: 'payment_account', headerName: translate("label:OrganizationRequisiteListView.payment_account"), flex: 1 },
    { field: 'bank', headerName: translate("label:OrganizationRequisiteListView.bank"), flex: 1 },
    { field: 'bik', headerName: translate("label:OrganizationRequisiteListView.bik"), flex: 1 },
    { field: 'organization_id', headerName: translate("label:OrganizationRequisiteListView.organization_id"), flex: 1 },
  ];

  return (
    <BaseListView
      maxWidth={"xl"}
      title={translate("label:OrganizationRequisiteListView.entityTitle")}
      columns={columns}
      data={store.data}
      tableName="OrganizationRequisiteListView"
      onDeleteClicked={(id) => store.deleteOrganizationRequisite(id)}
      onEditClicked={(id) => store.onEditClicked(id)}
      store={{
        loadData: store.loadOrganizationRequisites,
        clearStore: store.clearStore
      }}
      viewMode="popup"
    >
      {/* Popup form for editing/creating OrganizationRequisites */}
      <OrganizationRequisitePopupForm
        openPanel={store.openPanel}
        id={store.currentId}
        onBtnCancelClick={() => store.closePanel()}
        onSaveClick={() => {
          store.closePanel();
          store.loadOrganizationRequisites();
        }}
      />
    </BaseListView>
  );
});

export default OrganizationRequisiteListView;