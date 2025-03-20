import { FC, useEffect } from 'react';
import OrganizationTypeAddEditBaseView from './base';
import store from "./store"
import { observer } from "mobx-react"
import { Dialog, DialogActions, DialogContent } from '@mui/material';
import { useTranslation } from 'react-i18next';
import CustomButton from 'components/Button';

type PopupFormProps = {
  openPanel: boolean;
  id: number;
  onBtnCancelClick: () => void;
  onSaveClick: (id: number) => void;
}

const PopupForm: FC<PopupFormProps> = observer((props) => {
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
    <Dialog open={props.openPanel} onClose={props.onBtnCancelClick} maxWidth="lg" fullWidth>
      <DialogContent>
        <OrganizationTypeAddEditBaseView isPopup={true} />
      </DialogContent>
      <DialogActions>
        <CustomButton
          variant="contained"
          id="id_OrganizationTypeSaveButton"
          onClick={() => {
            store.onSaveClick((id: number) => props.onSaveClick(id))
          }}
        >
          {translate("common:save")}
        </CustomButton>
        <CustomButton
          variant="contained"
          color="secondary"
          id="id_OrganizationTypeCancelButton"
          onClick={() => props.onBtnCancelClick()}
        >
          {translate("common:cancel")}
        </CustomButton>
      </DialogActions>
    </Dialog>
  );
})

export default PopupForm;