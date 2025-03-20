import { FC, useEffect } from 'react';
import OrganizationContactAddEditBaseView from './base';
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
  mainId: number; // Organization ID
}

const PopupForm: FC<PopupFormProps> = observer((props) => {
  const { t } = useTranslation();
  const translate = t;

  useEffect(() => {
    // Set the organization ID when the form opens
    store.handleChange({ target: { value: props.mainId, name: "organization_id" } })
    
    if (props.openPanel) {
      store.doLoad(props.id)
    } else {
      store.clearStore()
    }
  }, [props.openPanel, props.mainId])

  return (
    <Dialog open={props.openPanel} onClose={props.onBtnCancelClick}>
      <DialogContent>
        <OrganizationContactAddEditBaseView isPopup={true} />
      </DialogContent>
      <DialogActions>
        <CustomButton
          variant="contained"
          id="id_OrganizationContactSaveButton"
          onClick={() => {
            store.onSaveClick((id: number) => props.onSaveClick(id))
          }}
        >
          {translate("common:save")}
        </CustomButton>
        <CustomButton
          variant="contained"
          color="secondary"
          id="id_OrganizationContactCancelButton"
          onClick={() => props.onBtnCancelClick()}
        >
          {translate("common:cancel")}
        </CustomButton>
      </DialogActions>
    </Dialog>
  );
})

export default PopupForm;