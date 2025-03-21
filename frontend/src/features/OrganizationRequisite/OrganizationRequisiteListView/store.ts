import { runInAction, makeObservable, observable } from "mobx";
import i18n from "i18next";
import BaseStore from 'core/stores/BaseStore';
import { getOrganizationRequisites, deleteOrganizationRequisite } from "api/OrganizationRequisite";
import { OrganizationRequisite } from "constants/OrganizationRequisite";
import MainStore from "MainStore";

/**
 * Store for managing OrganizationRequisite list data and operations
 */
class OrganizationRequisiteListStore extends BaseStore {
  @observable data: OrganizationRequisite[] = [];
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
  loadOrganizationRequisites = async () => {
    this.apiCall(
      getOrganizationRequisites,
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
  deleteOrganizationRequisite = (id: number) => {
    this.showConfirmDialog(
      i18n.t("areYouSure"),
      i18n.t("delete"),
      i18n.t("no"),
      async () => {
        this.apiCall(
          () => deleteOrganizationRequisite(id),
          () => {
            this.loadOrganizationRequisites();
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

export default new OrganizationRequisiteListStore();