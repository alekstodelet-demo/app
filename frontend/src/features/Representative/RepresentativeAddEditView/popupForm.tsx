import { FC, useEffect } from 'react';
import RepresentativeAddEditBaseView from './base';
import store from "./store"
import { observer } from "mobx-react"
import { Dialog, Grid, DialogActions, DialogContent, DialogTitle } from '@mui/material';
import { useTranslation } from 'react-i18next';
import CustomButton from 'components/Button';
import FastInputView from "features/RepresentativeContact/RepresentativeContactAddEditView/fastInput";

type PopupFormProps = {
  openPanel: boolean;
  id: number;
  onBtnCancelClick: () => void;
  onSaveClick: (id: number) => void;
}

const RepresentativePopupForm: FC<PopupFormProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;

  useEffect(() => {
    if (props.openPanel) {
      store.doLoad(props.id)
    } else {
      store.clearStore()
    }
  }, [props.openPanel])

  return (
    <Dialog open={props.openPanel} onClose={props.onBtnCancelClick} maxWidth="xl" fullWidth>
      <DialogTitle>{translate('label:RepresentativeAddEditView.entityTitle')}</DialogTitle>
      <DialogContent>
        <RepresentativeAddEditBaseView
          isPopup={true}
        >

          {store.id > 0 &&
            <Grid item md={12}>
              <FastInputView mainId={store.id} onAdded={() => {
                store.doLoad(store.id);
              }} />
            </Grid>
          }
        </RepresentativeAddEditBaseView>
      </DialogContent>
      <DialogActions>
        <DialogActions>
          <CustomButton
            variant="contained"
            id="id_RepresentativeSaveButton"
            name={'RepresentativeAddEditView.save'}
            onClick={() => {
              store.onSaveClick((id: number, isNew: boolean) => {
                if (isNew) {
                  store.loadRepresentative(id);
                } else {
                  props.onSaveClick(id);
                }
              })
            }}
          >
            {translate("common:save")}
          </CustomButton>
          <CustomButton
            variant="contained"
            id="id_RepresentativeCancelButton"
            name={'RepresentativeAddEditView.cancel'}
            onClick={() => props.onBtnCancelClick()}
          >
            {translate("common:cancel")}
          </CustomButton>
        </DialogActions>
      </DialogActions >
    </Dialog >
  );
})

export default RepresentativePopupForm
