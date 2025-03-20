import { runInAction, makeObservable, observable } from "mobx";
import i18n from "i18next";
import BaseStore from 'core/stores/BaseStore';
import { getOrganizationContactsByOrganizationId, deleteOrganizationContact } from "api/OrganizationContact";
import { OrganizationContact } from "constants/OrganizationContact";
import MainStore from "MainStore";

/**
 * Store for managing OrganizationContact list data and operations
 */
class OrganizationContactListStore extends BaseStore {
  @observable data: OrganizationContact[] = [];
  @observable openPanel: boolean = false;
  @observable currentId: number = 0;
  @observable mainId: number = 0; // Organization ID

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

  /**
   * Set the main organization ID and load contacts
   */
  setMainId = async (id: number) => {
    if (id !== this.mainId) {
      this.mainId = id;
      await this.loadOrganizationContacts()
    }
  }

  /**
   * Handle edit button click
   * @param id - OrganizationContact ID to edit
   */
  onEditClicked = (id: number) => {
    runInAction(() => {
      this.openPanel = true;
      this.currentId = id;
    });
  }

  /**
   * Close the edit panel
   */
  closePanel = () => {
    runInAction(() => {
      this.openPanel = false;
      this.currentId = 0;
    });
  }

  /**
   * Load all organization contacts from the API for a specific organization
   */
  loadOrganizationContacts = async () => {
    if (this.mainId === 0) return;
    
    this.apiCall(
      () => getOrganizationContactsByOrganizationId(this.mainId),
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
   * Delete an organization contact by ID
   * @param id - OrganizationContact ID to delete
   */
  deleteOrganizationContact = (id: number) => {
    this.showConfirmDialog(
      i18n.t("areYouSure"),
      i18n.t("delete"),
      i18n.t("no"),
      async () => {
        this.apiCall(
          () => deleteOrganizationContact(id),
          () => {
            this.loadOrganizationContacts();
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

export default new OrganizationContactListStore();