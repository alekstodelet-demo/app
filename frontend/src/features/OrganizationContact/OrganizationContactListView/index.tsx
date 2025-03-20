import React, { FC, useEffect } from 'react';
import { observer } from "mobx-react";
import { useTranslation } from 'react-i18next';
import { GridColDef } from '@mui/x-data-grid';
import BaseListView from 'components/common/BaseListView';
import OrganizationContactPopupForm from '../OrganizationContactAddEditView/popupForm';
import store from "./store";

interface OrganizationContactListProps {
  mainId: number; // Organization ID
}

/**
 * OrganizationContact list view component that displays contacts for an organization
 */
const OrganizationContactListView: FC<OrganizationContactListProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;

  useEffect(() => {
    store.setMainId(props.mainId);
  }, [props.mainId]);

  // Define grid columns
  const columns: GridColDef[] = [
    { 
      field: 'type_name', 
      headerName: translate("label:OrganizationContactListView.type_name"), 
      flex: 1 
    },
    { 
      field: 'value', 
      headerName: translate("label:OrganizationContactListView.value"), 
      flex: 1.5 
    },
    { 
      field: 'allow_notification', 
      headerName: translate("label:OrganizationContactListView.allow_notification"), 
      flex: 1,
      type: 'boolean'
    }
  ];

  return (
    <BaseListView
      maxWidth={"xl"}
      title={translate("label:OrganizationContactListView.entityTitle")}
      columns={columns}
      data={store.data}
      tableName="OrganizationContact"
      onDeleteClicked={(id) => store.deleteOrganizationContact(id)}
      onEditClicked={(id) => store.onEditClicked(id)}
      store={{
        loadData: store.loadOrganizationContacts,
        clearStore: store.clearStore
      }}
      viewMode="popup"
    >
      {/* Popup form for editing/creating organization contacts */}
      <OrganizationContactPopupForm
        openPanel={store.openPanel}
        id={store.currentId}
        mainId={store.mainId}
        onBtnCancelClick={() => store.closePanel()}
        onSaveClick={() => {
          store.closePanel();
          store.loadOrganizationContacts();
        }}
      />
    </BaseListView>
  );
});

export default OrganizationContactListView;