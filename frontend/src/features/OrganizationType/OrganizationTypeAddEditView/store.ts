import { makeObservable, runInAction, observable } from "mobx";
import i18n from "i18next";
import BaseStore from 'core/stores/BaseStore';
import { validate, validateField } from "./valid";
import { getOrganizationType, createOrganizationType, updateOrganizationType } from "api/OrganizationType";
import { OrganizationType, OrganizationTypeCreateModel } from "constants/OrganizationType";
import MainStore from "MainStore";

interface OrganizationTypeResponse {
  id: number;
}

/**
 * Store for managing OrganizationType edit form data and operations
 */
class OrganizationTypeStore extends BaseStore {
  // Form fields
  @observable id: number = 0;
  @observable name: string = "";
  @observable description: string = "";
  @observable code: string = "";
  @observable name_kg: string = "";
  @observable description_kg: string = "";
  @observable text_color: string = "#000000";
  @observable background_color: string = "#FFFFFF";

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
      this.description = "";
      this.code = "";
      this.name_kg = "";
      this.description_kg = "";
      this.text_color = "#000000";
      this.background_color = "#FFFFFF";
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
    const data: OrganizationTypeCreateModel = {
      id: this.id,
      name: this.name,
      description: this.description,
      code: this.code,
      name_kg: this.name_kg,
      description_kg: this.description_kg,
      text_color: this.text_color,
      background_color: this.background_color
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
      () => createOrganizationType(data) :
      () => updateOrganizationType(data);

    // Make API call with inherited method
    this.apiCall(
      apiMethod,
      (response: OrganizationTypeResponse) => {
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
   * Load OrganizationType data by ID
   * @param id - OrganizationType ID to load
   */
  loadOrganizationType = async (id: number) => {
    this.apiCall(
      () => getOrganizationType(id),
      (data: OrganizationType) => {
        runInAction(() => {
          this.id = data.id;
          this.name = data.name;
          this.description = data.description;
          this.code = data.code;
          this.name_kg = data.name_kg;
          this.description_kg = data.description_kg;
          this.text_color = data.text_color;
          this.background_color = data.background_color;
        });
      }
    );
  };

  /**
   * Initialize the form for a given ID
   * @param id - OrganizationType ID to load
   */
  async doLoad(id: number) {
    if (id) {
      this.id = id;
      await this.loadOrganizationType(id);
    }
  }
}

export default new OrganizationTypeStore();