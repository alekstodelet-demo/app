import { runInAction, makeObservable, observable } from "mobx";
import i18n from "i18next";
import BaseStore from 'core/stores/BaseStore';
import { getOrganizations, deleteOrganization } from "api/Organization";
import { Organization } from "constants/Organization";
import MainStore from "MainStore";

/**
 * Store for managing Organization list data and operations
 */
class OrganizationListStore extends BaseStore {
  @observable data: Organization[] = [];
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
   * @param id - Organization ID to edit
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
   * Load all organizations from the API
   */
  loadOrganizations = async () => {
    this.apiCall(
      getOrganizations,
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
   * Delete an organization by ID
   * @param id - Organization ID to delete
   */
  deleteOrganization = (id: number) => {
    this.showConfirmDialog(
      i18n.t("areYouSure"),
      i18n.t("delete"),
      i18n.t("no"),
      async () => {
        this.apiCall(
          () => deleteOrganization(id),
          () => {
            this.loadOrganizations();
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

export default new OrganizationListStore();