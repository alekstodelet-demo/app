import { notification } from "constants/notification";
import i18n from "i18n";
import MainStore from "MainStore";
import { makeAutoObservable, runInAction } from "mobx";

interface EmployeeResponse {
  last_name: string;
  first_name: string;
}

class NewStore {
  drawerOpened = false;
  notifications: notification[] = [];
  curentUserName = '';
  last_name = '';
  employee_id = 0;
  user_id = 0;
  first_name = '';
  head_of_structures = [];
  openPanel = false;
  open = false; // Menu open state
  selectedIndex = 0; // Index of the selected list item
  isSuperAdmin: boolean = false;

  constructor() {
    makeAutoObservable(this);
  }

  clearStore() {
    runInAction(() => {
      // Reset or clear state as needed
    });
  }

  // Method to toggle the menu open state
  setOpen(isOpen: boolean) {
    this.open = isOpen;
  }

  // Method to set the selected index
  setSelectedIndex(index: number) {
    this.selectedIndex = index;
  }

  // Method to toggle the drawer state
  changeDrawer() {
    this.drawerOpened = !this.drawerOpened;
  }
}

export default new NewStore();