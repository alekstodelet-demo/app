import { makeObservable, runInAction, observable } from "mobx";
import i18n from "i18next";
import { Dayjs } from "dayjs";
import BaseStore from 'core/stores/BaseStore';
import { validate, validateField } from "./valid";
import { getOrganization, createOrganization, updateOrganization } from "api/Organization";
import { getOrganizationTypes } from "api/OrganizationType";
import { Organization, OrganizationCreateModel } from "constants/Organization";
import MainStore from "MainStore";

interface OrganizationResponse {
  id: number;
}

/**
 * Store for managing Organization edit form data and operations
 */
class OrganizationStore extends BaseStore {
  // Form fields
  @observable id: number = 0;
  @observable name: string = "";
  @observable address: string = "";
  @observable director: string = "";
  @observable nomer: string = "";
  @observable pin: string = "";
  @observable okpo: string = "";
  @observable postal_code: string = "";
  @observable ugns: string = "";
  @observable reg_number: string = "";
  @observable organization_type_id: number = 0;
  @observable organization_type_name: string = "";

  // Reference data
  @observable OrganizationTypes: any[] = [];

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
      this.name = "";
      this.address = "";
      this.director = "";
      this.nomer = "";
      this.pin = "";
      this.okpo = "";
      this.postal_code = "";
      this.ugns = "";
      this.reg_number = "";
      this.organization_type_id = 0;
      this.organization_type_name = "";
      
      this.OrganizationTypes = [];
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
    const data: OrganizationCreateModel = {
      id: this.id,
      name: this.name,
      address: this.address,
      director: this.director,
      nomer: this.nomer,
      pin: this.pin,
      okpo: this.okpo,
      postal_code: this.postal_code,
      ugns: this.ugns,
      reg_number: this.reg_number,
      organization_type_id: this.organization_type_id
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
      () => createOrganization(data) :
      () => updateOrganization(data);

    // Make API call with inherited method
    this.apiCall(
      apiMethod,
      (response: OrganizationResponse) => {
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
   * Load Organization data by ID
   * @param id - Organization ID to load
   */
  loadOrganization = async (id: number) => {
    this.apiCall(
      () => getOrganization(id),
      (data: Organization) => {
        runInAction(() => {
          this.id = data.id;
          this.name = data.name;
          this.address = data.address;
          this.director = data.director;
          this.nomer = data.nomer;
          this.pin = data.pin;
          this.okpo = data.okpo;
          this.postal_code = data.postal_code;
          this.ugns = data.ugns;
          this.reg_number = data.reg_number;
          this.organization_type_id = data.organization_type_id;
          this.organization_type_name = data.organization_type_name || "";
        });
      }
    );
  };

  /**
   * Load organization types for dropdowns
   */
  loadOrganizationTypes = async () => {
    this.apiCall(
      getOrganizationTypes,
      (data) => {
        if (Array.isArray(data)) {
          runInAction(() => {
            this.OrganizationTypes = data;
          });
        }
      }
    );
  };

  /**
   * Initialize the form for a given ID
   * @param id - Organization ID to load
   */
  async doLoad(id: number) {
    await this.loadOrganizationTypes();
    
    if (id) {
      this.id = id;
      await this.loadOrganization(id);
    }
  }
}

export default new OrganizationStore();