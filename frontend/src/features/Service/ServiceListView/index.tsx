import { FC, useEffect } from 'react';
import {
  Container,
} from '@mui/material';
import PageGrid from 'components/PageGrid';
import { observer } from "mobx-react"
import store from "./store"
import { useTranslation } from 'react-i18next';
import { GridColDef } from '@mui/x-data-grid';
import PopupGrid from 'components/PopupGrid';
import ServicePopupForm from './../ServiceAddEditView/popupForm';

type ServiceListViewProps = {};


const ServiceListView: FC<ServiceListViewProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;

  useEffect(() => {
    store.loadServices()
    return () => {
      store.clearStore()
    }
  }, [])

  const columns: GridColDef[] = [
    {
      field: 'name',
      headerName: translate("label:ServiceListView.name"),
      flex: 1
    },
    {
      field: 'short_name',
      headerName: translate("label:ServiceListView.short_name"),
      flex: 1
    },
    {
      field: 'code',
      headerName: translate("label:ServiceListView.code"),
      flex: 1
    },
    {
      field: 'description',
      headerName: translate("label:ServiceListView.description"),
      flex: 1
    },
    {
      field: 'day_count',
      headerName: translate("label:ServiceListView.day_count"),
      flex: 1
    },
    {
      field: 'price',
      headerName: translate("label:ServiceListView.price"),
      flex: 1
    },
    {
      field: 'workflow_name',
      headerName: translate("label:ServiceListView.workflow_name"),
      flex: 1
    }
  ];

  let type1: string = 'form';
  let component = null;
  switch (type1) {
    case 'form':
      component = <PageGrid
        title={translate("label:ServiceListView.entityTitle")}
        onDeleteClicked={(id: number) => store.deleteService(id)}
        columns={columns}
        data={store.data}
        tableName="Service" />
      break
    case 'popup':
      component = <PopupGrid
        title={translate("label:ServiceListView.entityTitle")}
        onDeleteClicked={(id: number) => store.deleteService(id)}
        onEditClicked={(id: number) => store.onEditClicked(id)}
        columns={columns}
        data={store.data}
        tableName="Service" />
      break
  }


  return (
    <Container maxWidth='xl' style={{ marginTop: 30 }}>
      {component}

      <ServicePopupForm
        openPanel={store.openPanel}
        id={store.currentId}
        onBtnCancelClick={() => store.closePanel()}
        onSaveClick={() => {
          store.closePanel()
          store.loadServices()
        }}
      />

    </Container>
  );
})




export default ServiceListView
