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
const BaseListView: FC<BaseListViewProps> = observer(({
                                                        title,
                                                        columns,
                                                        data,
                                                        tableName,
                                                        onDeleteClicked,
                                                        onEditClicked,
                                                        store,
                                                        viewMode = 'form',
                                                        hideAddButton = false,
                                                        hideActions = false,
                                                        children,
                                                        maxWidth = 'xl',
                                                        marginTop = 30
                                                      }) => {
  // Load data on component mount
  useEffect(() => {
    store.loadData();
    return () => {
      store.clearStore();
    };
  }, []);

  console.log(data)

  let gridComponent = null;

  switch (viewMode) {
    case 'form':
      gridComponent = (
        <PageGrid
          title={title}
          onDeleteClicked={onDeleteClicked}
          columns={columns}
          data={data}
          tableName={tableName}
          hideAddButton={hideAddButton}
          hideActions={hideActions}
        />
      );
      break;
    case 'popup':
      gridComponent = (
        <PopupGrid
          title={title}
          onDeleteClicked={onDeleteClicked}
          onEditClicked={onEditClicked}
          columns={columns}
          data={data}
          tableName={tableName}
          hideAddButton={hideAddButton}
          hideActions={hideActions}
        />
      );
      break;
  }

  return (
    <Container maxWidth={maxWidth} style={{ marginTop }}>
      {gridComponent}
      {children}
    </Container>
  );
});

export default BaseListView;