import { makeAutoObservable, runInAction } from "mobx";
import i18n from "i18next";
import MainStore from "MainStore";
import { auth } from "api/Auth/useAuth";

interface UsbTokenData {
  tokenId: string;
  signature: string;
}

class NewStore {
  pin = "";
  error = "";
  navigate: (path: string) => void = () => {
  };

  constructor() {
    makeAutoObservable(this);
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

  onLogin = async () => {
    try {
      MainStore.changeLoader(true);
      const tokenData = await this.simulateUsbToken();
      var data = {
        pin: this.pin,
        tokenId: tokenData.tokenId,
        signature: tokenData.signature
      };
      const response = await auth(data);

      if (response.status === 201 || response.status === 200) {
        if (response.data) {
          const { token } = response.data;
          localStorage.setItem("token", token);
          this.navigate("/user");
          window.location.reload();
          this.clearStore();
        }
      } else {
        throw new Error();
      }
    } catch (err) {
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
