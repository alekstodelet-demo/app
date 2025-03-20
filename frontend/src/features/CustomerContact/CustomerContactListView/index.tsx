import React, { FC, useEffect } from 'react';
import { observer } from "mobx-react";
import { useTranslation } from 'react-i18next';
import { GridColDef } from '@mui/x-data-grid';
import BaseListView from 'components/common/BaseListView';
import CustomerContactPopupForm from '../CustomerContactAddEditView/popupForm';
import store from "./store";

type CustomerContactListProps = {
  mainId: number;
};


/**
 * CustomerContact list view component that displays all CustomerContacts
 */
const CustomerContactListView: FC<CustomerContactListProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;

  useEffect(() => {
    store.setMainId(props.mainId)
  }, [props.mainId]);

  // Define grid columns
  const columns: GridColDef[] = [
    { field: 'pin', headerName: translate("label:CustomerContactListView.pin"), flex: 1 },
    { field: 'value', headerName: translate("label:CustomerContactListView.value"), flex: 1 },
    { field: 'allow_notification', headerName: translate("label:CustomerContactListView.allow_notification"), flex: 1 },
    { field: 'type_name', headerName: translate("label:CustomerContactListView.type_name"), flex: 1 },
  ];

  return (
    <BaseListView
      maxWidth={"xl"}
      title={translate("label:CustomerContactListView.entityTitle")}
      columns={columns}
      data={store.data}
      tableName="CustomerContact"
      onDeleteClicked={(id) => store.deleteCustomerContact(id)}
      onEditClicked={(id) => store.onEditClicked(id)}
      store={{
        loadData: store.loadCustomerContacts,
        clearStore: store.clearStore
      }}
      viewMode="popup"
    >
      {/* Popup form for editing/creating CustomerContacts */}
      <CustomerContactPopupForm
        openPanel={store.openPanel}
        id={store.currentId}
        mainId={store.mainId}
        onBtnCancelClick={() => store.closePanel()}
        onSaveClick={() => {
          store.closePanel();
          store.loadCustomerContacts();
        }}
      />
    </BaseListView>
  );
});

export default CustomerContactListView;