import { makeObservable, runInAction, observable } from "mobx";
import i18n from "i18next";
import { Dayjs } from "dayjs";
import BaseStore from 'core/stores/BaseStore';
import { validate, validateField } from "./valid";
import { getOrganizationRequisite, createOrganizationRequisite, updateOrganizationRequisite } from "api/OrganizationRequisite";
import { OrganizationRequisite, OrganizationRequisiteCreateModel } from "constants/OrganizationRequisite";
import MainStore from "MainStore";

interface OrganizationRequisiteResponse {
  id: number;
}

/**
 * Store for managing OrganizationRequisite edit form data and operations
 */
class OrganizationRequisiteStore extends BaseStore {
  // Form fields
  @observable id: number = 0;
  @observable payment_account: string = "";
  @observable bank: string = "";
  @observable bik: string = "";
  @observable organization_id: number = 0;

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
      this.id = 0;
      this.payment_account = "";
      this.bank = "";
      this.bik = "";
      this.organization_id = 0;
    });
  }

  /**
   * Validate a specific field
   * @param name - Field name
   * @param value - Field value
   */
  async validateField(name: string, value: any) {
    const { isValid, error } = await validateField(name, value);
    runInAction(() => {
      if (isValid) {
        this.errors[name] = "";
      } else {
        this.errors[name] = error;
      }
    });
  }

  /**
   * Handle save button click
   * @param onSaved - Callback function after successful save
   */
  onSaveClick = async (onSaved: (id: number) => void) => {
    // Create data object from form fields
    const data: OrganizationRequisiteCreateModel = {
      id: this.id,
      payment_account: this.payment_account,
      bank: this.bank,
      bik: this.bik,
      organization_id: this.organization_id,
      
    };

    // Validate all fields
    const { isValid, errors } = await validate(data);
    if (!isValid) {
      runInAction(() => {
        this.errors = errors;
      });
      MainStore.openErrorDialog(i18n.t("message:error.alertMessageAlert"));
      return;
    }

    // Determine whether to create or update
    const apiMethod = data.id === 0 ?
      () => createOrganizationRequisite(data) :
      () => updateOrganizationRequisite(data);

    // Make API call with inherited method
    this.apiCall(
      apiMethod,
      (response: OrganizationRequisiteResponse) => {
        if (data.id === 0) {
          runInAction(() => {
            this.id = response.id;
          });
          this.showSuccessSnackbar(i18n.t("message:snackbar.successSave"));
        } else {
          this.showSuccessSnackbar(i18n.t("message:snackbar.successEdit"));
        }
        onSaved(response.id || this.id);
      }
    );
  };

  /**
   * Load OrganizationRequisite data by ID
   * @param id - OrganizationRequisite ID to load
   */
  loadOrganizationRequisite = async (id: number) => {
    this.apiCall(
      () => getOrganizationRequisite(id),
      (data: OrganizationRequisite) => {
        runInAction(() => {
          this.id = data.id;
          this.payment_account = data.payment_account;
          this.bank = data.bank;
          this.bik = data.bik;
          this.organization_id = data.organization_id;
        });
      }
    );
  };

  /**
   * Initialize the form for a given ID
   * @param id - OrganizationRequisite ID to load
   */
  async doLoad(id: number) {
    if (id) {
      this.id = id;
      await this.loadOrganizationRequisite(id);
    }
  }
}

export default new OrganizationRequisiteStore();