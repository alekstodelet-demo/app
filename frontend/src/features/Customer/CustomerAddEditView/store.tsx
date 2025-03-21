import { makeObservable, runInAction, observable } from "mobx";
import i18n from "i18next";
import dayjs, { Dayjs } from "dayjs";

import MainStore from "MainStore";
import BaseStore from 'core/stores/BaseStore';
import { validate, validateField } from "./valid";
import { getCustomer, createCustomer, updateCustomer } from "api/Customer";
import { Customer, CustomerCreateModel } from "constants/Customer";

import { getOrganizationTypes } from "api/OrganizationType";
    

interface CustomerResponse {
  id: number;
}

class CustomerStore extends BaseStore {
  @observable id: number = 0
	@observable pin: string = ""
	@observable okpo: string = ""
	@observable postalCode: string = ""
	@observable ugns: string = ""
	@observable regNumber: string = ""
	@observable organizationTypeId: number = 0
	@observable name: string = ""
	@observable address: string = ""
	@observable director: string = ""
	@observable nomer: string = ""
	

  // Справочники
  @observable organizationTypes = []
	


  constructor() {
    super();
    makeObservable(this);
  }

  clearStore() {
    super.clearStore();
    runInAction(() => {
      this.id = 0
		this.pin = ""
		this.okpo = ""
		this.postalCode = ""
		this.ugns = ""
		this.regNumber = ""
		this.organizationTypeId = 0
		this.name = ""
		this.address = ""
		this.director = ""
		this.nomer = ""
		
    });
  }

  async validateField(name: string, value: any) {
    const { isValid, error } = await validateField(name, value);
    if (isValid) {
      this.errors[name] = ""; 
    } else {
      this.errors[name] = error;
    }
  }

  async onSaveClick(onSaved: (id: number) => void) {
    const data: CustomerCreateModel = {
      
      id: this.id - 0,
      pin: this.pin,
      okpo: this.okpo,
      postalCode: this.postalCode,
      ugns: this.ugns,
      regNumber: this.regNumber,
      organizationTypeId: this.organizationTypeId - 0,
      name: this.name,
      address: this.address,
      director: this.director,
      nomer: this.nomer,
    };

    const { isValid, errors } = await validate(data);
    if (!isValid) {
      this.errors = errors;
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

  async doLoad(id: number) {

    //загрузка справочников
    await this.loadOrganizationTypes();
		

    if (id) {
      this.id = id;
      await this.loadCustomer(id);
    }
  }

  loadCustomer = async (id: number) => {
    this.apiCall(
      () => getCustomer(id),
      (data: Customer) => {
        runInAction(() => {
          
          this.id = data.id;
          this.pin = data.pin;
          this.okpo = data.okpo;
          this.postalCode = data.postalCode;
          this.ugns = data.ugns;
          this.regNumber = data.regNumber;
          this.organizationTypeId = data.organizationTypeId;
          this.name = data.name;
          this.address = data.address;
          this.director = data.director;
          this.nomer = data.nomer;
        });
      }
    );
  };

  
  loadOrganizationTypes = async () => {
    try {
      MainStore.changeLoader(true);
      const response = await getOrganizationTypes();
      if ((response.status === 201 || response.status === 200) && response?.data !== null) {
        this.organizationTypes = response.data
      } else {
        throw new Error();
      }
    } catch (err) {
      MainStore.setSnackbar(i18n.t("message:somethingWentWrong"), "error");
    } finally {
      MainStore.changeLoader(false);
    }
  };
    

}

export default new CustomerStore();
