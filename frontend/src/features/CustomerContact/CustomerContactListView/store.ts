import { runInAction, makeObservable, observable } from "mobx";
import i18n from "i18next";
import BaseStore from 'core/stores/BaseStore';
import { getCustomerContactsByCustomerId, deleteCustomerContact } from "api/CustomerContact";
import { CustomerContact } from "constants/CustomerContact";
import MainStore from "../../../MainStore";

/**
 * Store for managing CustomerContact list data and operations
 */
class CustomerContactListStore extends BaseStore {
  @observable data: CustomerContact[] = [];
  @observable openPanel: boolean = false;
  @observable currentId: number = 0;
  @observable mainId: number = 0;

  constructor() {
    super();
    makeObservable(this);
  }

  /**
   * Clear store state to initial values
   */
  clearStore() {
    super.clearStore(); // Call parent's clearStore first
    runInAction(() => {
      this.data = [];
      this.currentId = 0;
      this.mainId = 0;
      this.openPanel = false;
    });
  }


  setMainId = async (id: number) => {
    if (id !== this.mainId) {
      this.mainId = id;
      await this.loadCustomerContacts()
    }
  }

  /**
   * Handle edit button click
   * @param id - CustomerContact ID to edit
   */
  onEditClicked(id: number) {
    runInAction(() => {
      this.openPanel = true;
      this.currentId = id;
    });
  }

  /**
   * Close the edit panel
   */
  closePanel() {
    runInAction(() => {
      this.openPanel = false;
      this.currentId = 0;
    });
  }

  /**
   * Load all CustomerContacts from the API
   */
  loadCustomerContacts = async () => {
    if (this.mainId === 0) return;
    this.apiCall(
      () => getCustomerContactsByCustomerId(this.mainId),
      (data) => {
        if (Array.isArray(data)) {
          runInAction(() => {
            this.data = data;
          });
        }
      }
    );
  };

  /**
   * Delete a CustomerContact by ID
   * @param id - CustomerContact ID to delete
   */
  deleteCustomerContact = (id: number) => {
    this.showConfirmDialog(
      i18n.t("areYouSure"),
      i18n.t("delete"),
      i18n.t("no"),
      async () => {
        this.apiCall(
          () => deleteCustomerContact(id),
          () => {
            this.loadCustomerContacts();
            this.showSuccessSnackbar(i18n.t("message:snackbar.successDelete"));
          },
          (err) => {
            MainStore.openErrorDialog(i18n.t("message:error.documentIsAlreadyInUse"));
          }
        );
        MainStore.onCloseConfirm();
      }
    );
  };
}

export default new CustomerContactListStore();