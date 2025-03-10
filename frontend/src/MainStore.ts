import { SnackbarOrigin } from "@mui/material";
import i18n from "i18n";
import { makeAutoObservable, runInAction, toJS } from "mobx";
import printJS from "print-js";
import pages, { icons } from "./menu-items/pages";

class NewStore {
  loader_counter = 0;
  loader = false;
  openSnackbar = false;
  positionSnackbar: SnackbarOrigin = { vertical: "bottom", horizontal: "center" };
  snackbarMessage = "";
  snackbarSeverity: "success" | "info" | "warning" | "error" = "success";
  alert = {
    messages: [],
    titles: [],
  };
  confirm = {
    errorMessage: [],
    alertYesNo: [],
    bodies: [],
    acceptBtnColor: [],
    cancelBtnColor: [],
    acceptBtnCustomIcon: [],
    cancelBtnCustomIcon: [],
    cancelBtn: [],
    acceptBtn: [],
    onCloseYes: [],
    onCloseNo: [],
    isDeleteReason: [],
  };
  error = {
    openError403: { error: false, message: "" },
    openError422: { error: false, message: "" },
  };
  myRoles: string[] = [];
  isAdmin = false;
  isRegistrar = false;
  isClerk = false;
  isFinancialPlan = false;
  isHeadStructure = false;
  isEmployee = false;
  isArchive = false;
  isAccountant = false;
  isLawyer = false;
  isSmm = false;
  isDutyPlan = false;
  isDeputyChief = false;
  TechCouncilCount = 0;
  BackUrl = ''

  menu = [];
  menuHeader = [];

  constructor() {
    makeAutoObservable(this);
  }

  setOpenError403 = (flag: boolean, message?: string) => {
    this.error.openError403.error = flag;
    if (flag === false) {
      this.error.openError403.message = "";
    } else {
      this.error.openError403.message = message;
    }
  };

  setOpenError422 = (flag: boolean, message?: string) => {
    this.error.openError422.error = flag;
    if (flag === false) {
      this.error.openError422.message = "";
    } else {
      this.error.openError422.message = message;
    }
  };

  logoutNavigate = () => {
    if (window.location.pathname !== "/login") {
      window.location.href = "/login";
    }
  };

  changeSnackbar = (flag: boolean) => {
    this.openSnackbar = flag;
    if ((flag = false)) {
      this.snackbarMessage = "";
      this.snackbarSeverity = "success";
    }
  };

  setSnackbar(
    message: string,
    severity: "success" | "info" | "warning" | "error" = "success",
    position?: SnackbarOrigin
  ) {
    this.openSnackbar = true;
    this.snackbarMessage = message;
    this.snackbarSeverity = severity;
    if (position) {
      this.positionSnackbar = position;
    }
  }

  openErrorDialog = (message: string, title?: string) => {
    this.alert.messages.push(message);
    if (title) this.alert.titles.push(title);
  };

  openErrorConfirm = (
    message: string,
    yesLabel: string,
    noLabel: string,
    yesCallback: any,
    noCallback: any,
    yesIcon?: any,
    noIcon?: any,
    yesColor?: string,
    noColor?: string,
    isDeleteReason?: boolean
  ) => {
    this.confirm.errorMessage.push(message);
    this.confirm.acceptBtn.push(yesLabel);
    this.confirm.cancelBtn.push(noLabel);
    this.confirm.onCloseYes.push(yesCallback);
    this.confirm.onCloseNo.push(noCallback);
    this.confirm.acceptBtnColor.push(yesColor);
    this.confirm.cancelBtnColor.push(noColor);
    this.confirm.acceptBtnCustomIcon.push(yesIcon);
    this.confirm.cancelBtnCustomIcon.push(noIcon);
    this.confirm.isDeleteReason.push(isDeleteReason);
  };

  onCloseAlert = () => {
    if (this.alert.messages.length > 0) this.alert.messages.shift();
    if (this.alert.titles.length > 0) this.alert.titles.shift();
  };

  onCloseConfirm = () => {
    if (this.confirm.errorMessage.length > 0) {
      this.confirm.errorMessage.shift();
      this.confirm.acceptBtn.shift();
      this.confirm.cancelBtn.shift();
      this.confirm.onCloseYes.shift();
      this.confirm.onCloseNo.shift();
      this.confirm.acceptBtnColor.shift();
      this.confirm.cancelBtnColor.shift();
      this.confirm.acceptBtnCustomIcon.shift();
      this.confirm.cancelBtnCustomIcon.shift();
    }
  };

  changeLoader(flag: boolean) {
    if (flag) {
      this.loader_counter += 1;
    } else {
      this.loader_counter -= 1;
    }
    if (this.loader_counter <= 0) {
      this.loader = false;
      this.loader_counter = 0;
    } else this.loader = true;
  }

}

const MainStore = new NewStore();
export default MainStore;
