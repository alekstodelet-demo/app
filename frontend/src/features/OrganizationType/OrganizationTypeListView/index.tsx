import React, { FC } from 'react';
import { observer } from "mobx-react";
import { useTranslation } from 'react-i18next';
import { GridColDef } from '@mui/x-data-grid';
import BaseListView from 'components/common/BaseListView';
import OrganizationTypePopupForm from '../OrganizationTypeAddEditView/popupForm';
import store from "./store";

/**
 * OrganizationType list view component that displays all organization types
 */
const OrganizationTypeListView: FC = observer(() => {
  const { t } = useTranslation();
  const translate = t;

  // Define grid columns
  const columns: GridColDef[] = [
    { field: 'name', headerName: translate("label:OrganizationTypeListView.name"), flex: 1 },
    { field: 'code', headerName: translate("label:OrganizationTypeListView.code"), flex: 1 },
    { field: 'description', headerName: translate("label:OrganizationTypeListView.description"), flex: 1.5 },
    { 
      field: 'color_preview', 
      headerName: translate("label:OrganizationTypeListView.color_preview"), 
      flex: 1,
      renderCell: (params) => (
        <div 
          style={{ 
            backgroundColor: params.row.background_color,
            color: params.row.text_color,
            padding: '5px 10px',
            borderRadius: '4px',
            width: '100%',
            textAlign: 'center'
          }}
        >
          {params.row.name}
        </div>
      )
    }
  ];

  return (
    <BaseListView
      maxWidth={"xl"}
      title={translate("label:OrganizationTypeListView.entityTitle")}
      columns={columns}
      data={store.data}
      tableName="OrganizationType"
      onDeleteClicked={(id) => store.deleteOrganizationType(id)}
      onEditClicked={(id) => store.onEditClicked(id)}
      store={{
        loadData: store.loadOrganizationTypes,
        clearStore: store.clearStore
      }}
      viewMode="popup"
    >
      {/* Popup form for editing/creating organization types */}
      <OrganizationTypePopupForm
        openPanel={store.openPanel}
        id={store.currentId}
        onBtnCancelClick={() => store.closePanel()}
        onSaveClick={() => {
          store.closePanel();
          store.loadOrganizationTypes();
        }}
      />
    </BaseListView>
  );
});

export default OrganizationTypeListView;