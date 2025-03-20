import React, { FC, useEffect } from 'react';
import { observer } from "mobx-react";
import { useTranslation } from 'react-i18next';
import { GridColDef } from '@mui/x-data-grid';
import BaseListView from 'components/common/BaseListView';
import CustomerPopupForm from '../CustomerAddEditView/popupForm';
import store from "./store";

/**
 * Customer list view component that displays all Customers
 */
const CustomerListView: FC = observer(() => {
  const { t } = useTranslation();
  const translate = t;



  // Define grid columns
  const columns: GridColDef[] = [
    { field: 'pin', headerName: translate("label:CustomerListView.pin"), flex: 1 },
    { field: 'is_organization', headerName: translate("label:CustomerListView.is_organization"), flex: 1 },
    { field: 'full_name', headerName: translate("label:CustomerListView.full_name"), flex: 1 },
    { field: 'address', headerName: translate("label:CustomerListView.address"), flex: 1 },
    { field: 'director', headerName: translate("label:CustomerListView.director"), flex: 1 },
    { field: 'okpo', headerName: translate("label:CustomerListView.okpo"), flex: 1 },
    { field: 'organization_type_id', headerName: translate("label:CustomerListView.organization_type_id"), flex: 1 },
    { field: 'payment_account', headerName: translate("label:CustomerListView.payment_account"), flex: 1 },
    { field: 'postal_code', headerName: translate("label:CustomerListView.postal_code"), flex: 1 },
    { field: 'ugns', headerName: translate("label:CustomerListView.ugns"), flex: 1 },
    { field: 'bank', headerName: translate("label:CustomerListView.bank"), flex: 1 },
    { field: 'bik', headerName: translate("label:CustomerListView.bik"), flex: 1 },
    { field: 'registration_number', headerName: translate("label:CustomerListView.registration_number"), flex: 1 },
    { field: 'document_date_issue', headerName: translate("label:CustomerListView.document_date_issue"), flex: 1 },
    { field: 'document_serie', headerName: translate("label:CustomerListView.document_serie"), flex: 1 },
    { field: 'identity_document_type_id', headerName: translate("label:CustomerListView.identity_document_type_id"), flex: 1 },
    { field: 'document_whom_issued', headerName: translate("label:CustomerListView.document_whom_issued"), flex: 1 },
    { field: 'individual_surname', headerName: translate("label:CustomerListView.individual_surname"), flex: 1 },
    { field: 'individual_name', headerName: translate("label:CustomerListView.individual_name"), flex: 1 },
    { field: 'individual_secondname', headerName: translate("label:CustomerListView.individual_secondname"), flex: 1 },
    { field: 'is_foreign', headerName: translate("label:CustomerListView.is_foreign"), flex: 1 },
    { field: 'foreign_country', headerName: translate("label:CustomerListView.foreign_country"), flex: 1 },
  ];

  return (
    <BaseListView
      maxWidth={"xl"}
      title={translate("label:CustomerListView.entityTitle")}
      columns={columns}
      data={store.data}
      tableName="Customer"
      onDeleteClicked={(id) => store.deleteCustomer(id)}
      onEditClicked={(id) => store.onEditClicked(id)}
      store={{
        loadData: store.loadCustomers,
        clearStore: store.clearStore
      }}
      viewMode="form"
    >
      {/* Popup form for editing/creating Customers */}
      <CustomerPopupForm
        openPanel={store.openPanel}
        id={store.currentId}
        onBtnCancelClick={() => store.closePanel()}
        onSaveClick={() => {
          store.closePanel();
          store.loadCustomers();
        }}
      />
    </BaseListView>
  );
});

export default CustomerListView;