import { makeObservable, runInAction, observable } from "mobx";
import i18n from "i18next";
import BaseStore from 'core/stores/BaseStore';
import { validate, validateField } from "./valid";
import { getOrganizationContact, createOrganizationContact, updateOrganizationContact } from "api/OrganizationContact";
import { getContactTypes } from "api/ContactType";
import { OrganizationContact, OrganizationContactCreateModel } from "constants/OrganizationContact";
import MainStore from "MainStore";

interface OrganizationContactResponse {
  id: number;
}

/**
 * Store for managing OrganizationContact edit form data and operations
 */
class OrganizationContactStore extends BaseStore {
  // Form fields
  @observable id: number = 0;
  @observable organization_id: number = 0; // Parent organization ID
  @observable value: string = "";
  @observable allow_notification: boolean = false;
  @observable t_type_id: number = 0;
  
  // Reference data
  @observable ContactTypes: any[] = [];

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
      this.organization_id = 0;
      this.value = "";
      this.allow_notification = false;
      this.t_type_id = 0;
      
      this.ContactTypes = [];
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
    const data: OrganizationContactCreateModel = {
      id: this.id,
      organization_id: this.organization_id,
      value: this.value,
      allow_notification: this.allow_notification,
      t_type_id: this.t_type_id
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
      () => createOrganizationContact(data) :
      () => updateOrganizationContact(data);

    // Make API call with inherited method
    this.apiCall(
      apiMethod,
      (response: OrganizationContactResponse) => {
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
   * Load OrganizationContact data by ID
   * @param id - OrganizationContact ID to load
   */
  loadOrganizationContact = async (id: number) => {
    this.apiCall(
      () => getOrganizationContact(id),
      (data: OrganizationContact) => {
        runInAction(() => {
          this.id = data.id;
          this.organization_id = data.organization_id;
          this.value = data.value;
          this.allow_notification = data.allow_notification;
          this.t_type_id = data.t_type_id;
        });
      }
    );
  };

  /**
   * Load contact types for dropdown
   */
  loadContactTypes = async () => {
    this.apiCall(
      getContactTypes,
      (data) => {
        if (Array.isArray(data)) {
          runInAction(() => {
            this.ContactTypes = data;
          });
        }
      }
    );
  };

  /**
   * Initialize the form for a given ID
   * @param id - OrganizationContact ID to load
   */
  async doLoad(id: number) {
    await this.loadContactTypes();
    
    if (id) {
      this.id = id;
      await this.loadOrganizationContact(id);
    }
  }
}

export default new OrganizationContactStore();