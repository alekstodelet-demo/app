import { runInAction, makeObservable, observable } from "mobx";
import i18n from "i18next";
import BaseStore from 'core/stores/BaseStore';
import { getOrganizationTypes, deleteOrganizationType } from "api/OrganizationType";
import { OrganizationType } from "constants/OrganizationType";
import MainStore from "MainStore";

/**
 * Store for managing OrganizationType list data and operations
 */
class OrganizationTypeListStore extends BaseStore {
  @observable data: OrganizationType[] = [];
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
   * @param id - OrganizationType ID to edit
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
   * Load all organization types from the API
   */
  loadOrganizationTypes = async () => {
    this.apiCall(
      getOrganizationTypes,
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
   * Delete an organization type by ID
   * @param id - OrganizationType ID to delete
   */
  deleteOrganizationType = (id: number) => {
    this.showConfirmDialog(
      i18n.t("areYouSure"),
      i18n.t("delete"),
      i18n.t("no"),
      async () => {
        this.apiCall(
          () => deleteOrganizationType(id),
          () => {
            this.loadOrganizationTypes();
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

export default new OrganizationTypeListStore();