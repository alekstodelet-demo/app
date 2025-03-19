import { makeAutoObservable, runInAction } from "mobx";
import i18n from "i18next";
import MainStore from "MainStore";
import { auth, logout, checkAuthStatus } from "api/Auth/useAuth";

interface UsbTokenData {
  tokenId: string;
  signature: string;
}

class NewStore {
  pin = "";
  error = "";
  isAuthenticated = false;
  navigate: (path: string) => void = () => {};
  
  constructor() {
    makeAutoObservable(this);
    
    // Check authentication status on store initialization
    this.checkAuth();
  }

  setNavigateFunction(navigate: (path: string) => void) {
    this.navigate = navigate;
  }

  handleChange(event) {
    const { name, value } = event.target;
    (this as any)[name] = value;
  }

  async simulateUsbToken(): Promise<UsbTokenData> {
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve({ tokenId: "SIMULATED_TOKEN_123", signature: "SIMULATED_SIGNATURE" });
      }, 1000);
    });
  }

  checkAuth = async () => {
    try {
      const response = await checkAuthStatus();
      
      runInAction(() => {
        this.isAuthenticated = response.status === 200;
      });
      
      return this.isAuthenticated;
    } catch (error) {
      runInAction(() => {
        this.isAuthenticated = false;
      });
      return false;
    }
  };

  onLogin = async () => {
    try {
      MainStore.changeLoader(true);
      const tokenData = await this.simulateUsbToken();
      
      var data = {
        pin: this.pin,
        tokenId: tokenData.tokenId,
        signature: tokenData.signature,
        DeviceId: 'device_id'
      };
      
      const response = await auth(data);

      if (response.status === 201 || response.status === 200) {
        runInAction(() => {
          this.isAuthenticated = true;
        });
        
        this.navigate("/user");
        this.clearStore();
      } else {
        throw new Error();
      }
    } catch (err) {
      MainStore.setSnackbar(i18n.t("message:somethingWentWrong"), "error");
    } finally {
      MainStore.changeLoader(false);
    }
  };

  onLogout = async () => {
    try {
      MainStore.changeLoader(true);
      await logout();
      
      runInAction(() => {
        this.isAuthenticated = false;
      });
      
      this.navigate("/login");
    } catch (error) {
      console.error("Logout failed:", error);
      MainStore.setSnackbar(i18n.t("message:somethingWentWrong"), "error");
    } finally {
      MainStore.changeLoader(false);
    }
  };

  clearStore = () => {
    runInAction(() => {
      this.pin = "";
      this.error = "";
    });
  };
}

export default new NewStore();