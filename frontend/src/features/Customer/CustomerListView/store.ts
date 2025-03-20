import { runInAction, makeObservable, observable } from "mobx";
import i18n from "i18next";
import BaseStore from 'core/stores/BaseStore';
import { getCustomers, deleteCustomer } from "api/Customer";
import { Customer } from "constants/Customer";
import MainStore from "../../../MainStore";

/**
 * Store for managing Customer list data and operations
 */
class CustomerListStore extends BaseStore {
  @observable data: Customer[] = [];
  @observable openPanel: boolean = false;
  @observable currentId: number = 0;

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
      this.openPanel = false;
    });
  }

  /**
   * Handle edit button click
   * @param id - Customer ID to edit
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
   * Load all Customers from the API
   */
  loadCustomers = async () => {
    this.apiCall(
      getCustomers,
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
   * Delete a Customer by ID
   * @param id - Customer ID to delete
   */
  deleteCustomer = (id: number) => {
    this.showConfirmDialog(
      i18n.t("areYouSure"),
      i18n.t("delete"),
      i18n.t("no"),
      async () => {
        this.apiCall(
          () => deleteCustomer(id),
          () => {
            this.loadCustomers();
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

export default new CustomerListStore();