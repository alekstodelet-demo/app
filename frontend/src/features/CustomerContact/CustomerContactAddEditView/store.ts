import { makeObservable, runInAction, observable } from "mobx";
import i18n from "i18next";
import { Dayjs } from "dayjs";
import BaseStore from 'core/stores/BaseStore';
import { validate, validateField } from "./valid";
import { getCustomerContact, createCustomerContact, updateCustomerContact } from "api/CustomerContact";
import { getContactTypes } from "api/ContactType";
import { CustomerContact, CustomerContactCreateModel } from "constants/CustomerContact";

import MainStore from "../../../MainStore";


interface CustomerContactResponse {
  id: number;
}

/**
 * Store for managing CustomerContact edit form data and operations
 */
class CustomerContactStore extends BaseStore {
  // Form fields
  @observable id: number = 0;
  @observable customer_id: number; //main table
  @observable value: string;
  @observable type_id: number;
  @observable allow_notification: boolean;

  // Reference data
  ContactTypes: any[] = [];

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
      this.customer_id = 0;
      this.value = "";
      this.type_id = 0;
      this.allow_notification = false;

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
    const data: CustomerContactCreateModel = {
      id: this.id,
      customer_id: this.customer_id,
      value: this.value,
      type_id: this.type_id,
      allow_notification: this.allow_notification,
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
      () => createCustomerContact(data) :
      () => updateCustomerContact(data);

    // Make API call with inherited method
    this.apiCall(
      apiMethod,
      (response: CustomerContactResponse) => {
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
   * Load CustomerContact data by ID
   * @param id - CustomerContact ID to load
   */
  loadCustomerContact = async (id: number) => {
    this.apiCall(
      () => getCustomerContact(id),
      (data: CustomerContact) => {
        runInAction(() => {
          this.id = data.id;
          this.customer_id = data.customer_id;
          this.value = data.value;
          this.type_id = data.type_id;
          this.allow_notification = data.allow_notification;
        });
      }
    );
  };

  loadContactTypes = async () => {
    this.apiCall(
      () => getContactTypes(),
      (data: CustomerContact[]) => {
        runInAction(() => {
          this.ContactTypes = data
        });
      }
    );
  };

  /**
   * Initialize the form for a given ID
   * @param id - CustomerContact ID to load
   */
  async doLoad(id: number) {
    if (id) {
      this.id = id;
      await this.loadCustomerContact(id);
    }
    await this.loadContactTypes();
  }
}

export default new CustomerContactStore();