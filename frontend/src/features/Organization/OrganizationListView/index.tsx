import React, { FC, useEffect } from 'react';
import { observer } from "mobx-react";
import { useTranslation } from 'react-i18next';
import { GridColDef } from '@mui/x-data-grid';
import BaseListView from 'components/common/BaseListView';
import OrganizationPopupForm from '../OrganizationAddEditView/popupForm';
import store from "./store";

interface OrganizationListProps {
  mainId?: number;
}

/**
 * Organization list view component that displays all organizations
 */
const OrganizationListView: FC<OrganizationListProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;

  // Define grid columns
  const columns: GridColDef[] = [
    { field: 'name', headerName: translate("label:OrganizationListView.name"), flex: 1 },
    { field: 'address', headerName: translate("label:OrganizationListView.address"), flex: 1 },
    { field: 'director', headerName: translate("label:OrganizationListView.director"), flex: 1 },
    { field: 'pin', headerName: translate("label:OrganizationListView.pin"), flex: 1 },
    { field: 'okpo', headerName: translate("label:OrganizationListView.okpo"), flex: 1 },
    { field: 'organization_type_name', headerName: translate("label:OrganizationListView.organization_type_name"), flex: 1 }
  ];

  return (
    <BaseListView
      maxWidth={"xl"}
      title={translate("label:OrganizationListView.entityTitle")}
      columns={columns}
      data={store.data}
      tableName="Organization"
      onDeleteClicked={(id) => store.deleteOrganization(id)}
      onEditClicked={(id) => store.onEditClicked(id)}
      store={{
        loadData: store.loadOrganizations,
        clearStore: store.clearStore
      }}
      viewMode="popup"
    >
      {/* Popup form for editing/creating organizations */}
      <OrganizationPopupForm
        openPanel={store.openPanel}
        id={store.currentId}
        onBtnCancelClick={() => store.closePanel()}
        onSaveClick={() => {
          store.closePanel();
          store.loadOrganizations();
        }}
      />
    </BaseListView>
  );
});

export default OrganizationListView;