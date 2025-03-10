import { makeAutoObservable, runInAction } from "mobx";
import { validate } from "./valid";
import i18n from "i18next";
import MainStore from "MainStore";
import { getService } from "api/Service/useGetService";
import { createService } from "api/Service/useCreateService";
import { updateService } from "api/Service/useUpdateService";

class NewStore {
  id = 0;
  name = "";
  short_name = "";
  code = "";
  description = "";
  day_count = 0;
  price = 0;
  workflow_id = 0;
  workflow_name = "";
  errorname = "";
  errorshort_name = "";
  errorcode = "";
  errordescription = "";
  errorday_count = "";
  errorprice = "";
  errorworkflow_id = "";
  errorworkflow_name = "";
  Workflows = [];

  constructor() {
    makeAutoObservable(this);
  }

  clearStore() {
    runInAction(() => {
      this.id = 0;
      this.name = "";
      this.short_name = "";
      this.code = "";
      this.description = "";
      this.day_count = 0;
      this.price = 0;
      this.workflow_id = 0;
      this.workflow_name = "";
      this.errorname = "";
      this.errorshort_name = "";
      this.errorcode = "";
      this.errordescription = "";
      this.errorday_count = "";
      this.errorprice = "";
      this.errorworkflow_id = "";
      this.errorworkflow_name = "";
      this.Workflows = [];
    });
  }

  handleChange(event) {
    this[event.target.name] = event.target.value;
    validate(event);
  }

  onSaveClick = async (onSaved: (id: number) => void) => {
    let canSave = true;
    let event: { target: { name: string; value: any } } = {
      target: { name: "id", value: this.id },
    };
    canSave = validate(event) && canSave;
    event = { target: { name: "name", value: this.name } };
    canSave = validate(event) && canSave;
    event = { target: { name: "description", value: this.description } };
    canSave = validate(event) && canSave;
    event = { target: { name: "code", value: this.code } };
    canSave = validate(event) && canSave;
    event = { target: { name: "day_count", value: this.day_count } };
    canSave = validate(event) && canSave;
    event = { target: { name: "price", value: this.price } };
    canSave = validate(event) && canSave;

    if (canSave) {
      try {
        MainStore.changeLoader(true);
        var data = {
          id: this.id,
          name: this.name,
          short_name: this.short_name,
          code: this.code,
          description: this.description,
          day_count: this.day_count,
          price: this.price,
          workflow_id: this.workflow_id,
        };

        const response = data.id === 0
          ? await createService(data)
          : await updateService(data);

          if (response.status === 201 || response.status === 200) {
            onSaved(response);
            console.log(i18n.language)
            if (data.id === 0) {
              this.id = response.data.id;
              MainStore.setSnackbar(i18n.t("message:snackbar.successSave"), "success");
            } else {
              MainStore.setSnackbar(i18n.t("message:snackbar.successEdit"), "success");
            }
          } else {
            throw new Error();
          }
      } catch (err) {
        MainStore.setSnackbar(i18n.t("message:somethingWentWrong"), "error");
      } finally {
        MainStore.changeLoader(false);
      }
    } else {
      MainStore.openErrorDialog(i18n.t("message:error.alertMessageAlert"));
    }
  };

  loadService = async (id: number) => {
    try {
      MainStore.changeLoader(true);
      const response = await getService(id);
      if ((response.status === 201 || response.status === 200) && response?.data !== null) {
        runInAction(() => {
          this.id = response.data.id;
          this.name = response.data.name;
          this.short_name = response.data.short_name;
          this.code = response.data.code;
          this.description = response.data.description;
          this.day_count = response.data.day_count;
          this.price = response.data.price;
          this.workflow_id = response.data.workflow_id;
        });
      } else {
        throw new Error();
      }
    } catch (err) {
      MainStore.setSnackbar(i18n.t("message:somethingWentWrong"), "error");
    } finally {
      MainStore.changeLoader(false);
    }
  };

  async doLoad(id: number) {
    if (id == null || id == 0) {
      return;
    }
    this.id = id;
    this.loadService(id);
  }
}

export default new NewStore();
