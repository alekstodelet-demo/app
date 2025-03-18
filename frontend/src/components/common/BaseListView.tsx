import React, { FC, ReactNode, useEffect } from 'react';
import { Container } from '@mui/material';
import { observer } from "mobx-react";
import { GridColDef } from '@mui/x-data-grid';
import PageGrid from 'components/PageGrid';
import PopupGrid from 'components/PopupGrid';

interface BaseListViewProps {
  title: string;
  columns: GridColDef[];
  data: any[];
  tableName: string;
  onDeleteClicked?: (id: number) => void;
  onEditClicked?: (id: number) => void;
  store: {
    loadData: () => void;
    clearStore: () => void;
  };
  viewMode?: 'form' | 'popup';
  hideAddButton?: boolean;
  hideActions?: boolean;
  children?: ReactNode;
  maxWidth?: 'xs' | 'sm' | 'md' | 'lg' | 'xl' | false;
  marginTop?: number;
}

/**
 * A base list view component for displaying data grids with common functionality
 */
const BaseListView: FC<BaseListViewProps> = observer((props: BaseListViewProps) => {
  // Load data on component mount
  useEffect(() => {
    props.store.loadData();
    return () => {
      props.store.clearStore();
    };
  }, []);

  // Debug data changes
  useEffect(() => {
  }, [props.data]);

  let gridComponent = null;

  switch (props.viewMode) {
    case 'form':
      gridComponent = (
        <PageGrid
          key={JSON.stringify(props.data)}
          title={props.title}
          onDeleteClicked={props.onDeleteClicked}
          columns={props.columns}
          data={props.data}
          tableName={props.tableName}
          hideAddButton={props.hideAddButton}
          hideActions={props.hideActions}
        />
      );
      break;
    case 'popup':
      gridComponent = (
        <PopupGrid
          key={JSON.stringify(props.data)}
          title={props.title}
          onDeleteClicked={props.onDeleteClicked}
          onEditClicked={props.onEditClicked}
          columns={props.columns}
          data={props.data}
          tableName={props.tableName}
          hideAddButton={props.hideAddButton}
          hideActions={props.hideActions}
        />
      );
      break;
  }

  return (
    <Container maxWidth={props.maxWidth} style={{ marginTop: props.marginTop }}>
      {gridComponent}
      {props.children}
    </Container>
  );
});

export default BaseListView;