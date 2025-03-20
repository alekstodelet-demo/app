import { makeObservable, runInAction, observable } from "mobx";
import i18n from "i18next";
import { Dayjs } from "dayjs";
import BaseStore from 'core/stores/BaseStore';
import { validate, validateField } from "./valid";
import { getCustomer, createCustomer, updateCustomer } from "api/Customer";
import { Customer, CustomerCreateModel } from "constants/Customer";

import MainStore from "../../../MainStore";


interface CustomerResponse {
  id: number;
}

/**
 * Store for managing Customer edit form data and operations
 */
class CustomerStore extends BaseStore {
  // Form fields
  @observable id: number = 0;
  @observable pin: string;
  @observable is_organization: boolean;
  @observable full_name: string;
  @observable address: string;
  @observable director: string;
  @observable okpo: string;
  @observable organization_type_id: number;
  @observable payment_account: string;
  @observable postal_code: string;
  @observable ugns: string;
  @observable bank: string;
  @observable bik: string;
  @observable registration_number: string;
  @observable document_date_issue?: Dayjs;
  @observable document_serie?: string;
  @observable identity_document_type_id?: number;
  @observable document_whom_issued?: string
  @observable individual_surname: string;
  @observable individual_name: string;
  @observable individual_secondname: string
  @observable is_foreign?: boolean;
  @observable foreign_country?: number;

  // Reference data
  Workflows: any[] = [];

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
      this.pin = "";
      this.is_organization = false;
      this.full_name = "";
      this.address = "";
      this.director = "";
      this.okpo = "";
      this.organization_type_id = 0;
      this.payment_account = "";
      this.postal_code = "";
      this.ugns = "";
      this.bank = "";
      this.bik = "";
      this.registration_number = "";
      this.document_date_issue = null;
      this.document_serie = "";
      this.identity_document_type_id = 0;
      this.document_whom_issued = "";
      this.individual_surname = "";
      this.individual_name = "";
      this.individual_secondname = "";
      this.is_foreign = false;
      this.foreign_country = 0;

      this.Workflows = [];
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
    const data: CustomerCreateModel = {
      id: this.id,
      pin: this.pin,
      is_organization: this.is_organization,
      full_name: this.full_name,
      address: this.address,
      director: this.director,
      okpo: this.okpo,
      organization_type_id: this.organization_type_id,
      payment_account: this.payment_account,
      postal_code: this.postal_code,
      ugns: this.ugns,
      bank: this.bank,
      bik: this.bik,
      registration_number: this.registration_number,
      document_date_issue: this.document_date_issue,
      document_serie: this.document_serie,
      identity_document_type_id: this.identity_document_type_id,
      document_whom_issued: this.document_whom_issued,
      individual_surname: this.individual_surname,
      individual_name: this.individual_name,
      individual_secondname: this.individual_secondname,
      is_foreign: this.is_foreign,
      foreign_country: this.foreign_country,
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
      () => createCustomer(data) :
      () => updateCustomer(data);

    // Make API call with inherited method
    this.apiCall(
      apiMethod,
      (response: CustomerResponse) => {
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
   * Load Customer data by ID
   * @param id - Customer ID to load
   */
  loadCustomer = async (id: number) => {
    this.apiCall(
      () => getCustomer(id),
      (data: Customer) => {
        runInAction(() => {
          this.id = data.id;
          this.pin = data.pin;
          this.is_organization = data.is_organization;
          this.full_name = data.full_name;
          this.address = data.address;
          this.director = data.director;
          this.okpo = data.okpo;
          this.organization_type_id = data.organization_type_id;
          this.payment_account = data.payment_account;
          this.postal_code = data.postal_code;
          this.ugns = data.ugns;
          this.bank = data.bank;
          this.bik = data.bik;
          this.registration_number = data.registration_number;
          this.document_date_issue = data.document_date_issue;
          this.document_serie = data.document_serie;
          this.identity_document_type_id = data.identity_document_type_id;
          this.document_whom_issued = data.document_whom_issued;
          this.individual_surname = data.individual_surname;
          this.individual_name = data.individual_name;
          this.individual_secondname = data.individual_secondname;
          this.is_foreign = data.is_foreign;
          this.foreign_country = data.foreign_country;
        });
      }
    );
  };

  /**
   * Initialize the form for a given ID
   * @param id - Customer ID to load
   */
  async doLoad(id: number) {
    if (id) {
      this.id = id;
      await this.loadCustomer(id);
    }
  }
}

export default new CustomerStore();