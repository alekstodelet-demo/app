import React, { FC, useEffect } from 'react';
import { observer } from "mobx-react";
import { useTranslation } from 'react-i18next';
import { GridColDef } from '@mui/x-data-grid';
import BaseListView from 'components/common/BaseListView';
import RepresentativePopupForm from '../RepresentativeAddEditView/popupForm';
import store from "./store";


type RepresentativeListViewProps = {
  mainId: number;

};


const RepresentativeListView: FC<RepresentativeListViewProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;

  useEffect(() => {
    store.setMainId(props.mainId)
  }, [props.mainId]);
  

  const columns: GridColDef[] = [
    
    {
      field: 'firstName',
      headerName: translate("label:RepresentativeListView.firstName"),
      flex: 1,
      renderCell: (param) => (<div data-testid="table_Representative_column_firstName"> {param.row.firstName} </div>),
      renderHeader: (param) => (<div data-testid="table_Representative_header_firstName">{param.colDef.headerName}</div>)
    },
    {
      field: 'secondName',
      headerName: translate("label:RepresentativeListView.secondName"),
      flex: 1,
      renderCell: (param) => (<div data-testid="table_Representative_column_secondName"> {param.row.secondName} </div>),
      renderHeader: (param) => (<div data-testid="table_Representative_header_secondName">{param.colDef.headerName}</div>)
    },
    {
      field: 'pin',
      headerName: translate("label:RepresentativeListView.pin"),
      flex: 1,
      renderCell: (param) => (<div data-testid="table_Representative_column_pin"> {param.row.pin} </div>),
      renderHeader: (param) => (<div data-testid="table_Representative_header_pin">{param.colDef.headerName}</div>)
    },
    {
      field: 'hasAccess',
      headerName: translate("label:RepresentativeListView.hasAccess"),
      flex: 1,
      renderCell: (param) => (<div data-testid="table_Representative_column_hasAccess"> {param.row.hasAccess} </div>),
      renderHeader: (param) => (<div data-testid="table_Representative_header_hasAccess">{param.colDef.headerName}</div>)
    },
    {
      field: 'typeId',
      headerName: translate("label:RepresentativeListView.typeId"),
      flex: 1,
      renderCell: (param) => (<div data-testid="table_Representative_column_typeId"> {param.row.typeId} </div>),
      renderHeader: (param) => (<div data-testid="table_Representative_header_typeId">{param.colDef.headerName}</div>)
    },
    {
      field: 'lastName',
      headerName: translate("label:RepresentativeListView.lastName"),
      flex: 1,
      renderCell: (param) => (<div data-testid="table_Representative_column_lastName"> {param.row.lastName} </div>),
      renderHeader: (param) => (<div data-testid="table_Representative_header_lastName">{param.colDef.headerName}</div>)
    },
  ];

  return (
    <BaseListView
      maxWidth={"xl"}
      title={translate("label:RepresentativeListView.entityTitle")}
      columns={columns}
      data={store.data}
      tableName="Representative"
      onDeleteClicked={(id) => store.deleteRepresentative(id)}
      onEditClicked={(id) => store.onEditClicked(id)}
      store={{
        loadData: store.loadRepresentatives,
        clearStore: store.clearStore
      }}
      viewMode="popup"
    >
      <RepresentativePopupForm
        openPanel={store.openPanel}
        id={store.currentId}
        onBtnCancelClick={() => store.closePanel()}
        onSaveClick={() => {
          store.closePanel();
          store.loadRepresentatives();
        }}
      />
    </BaseListView>
  );
})



export default RepresentativeListView
