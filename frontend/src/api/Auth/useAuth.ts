import http from "api/https";
import { Auth } from "constants/Auth";

export const auth = (data: Auth): Promise<any> => {
  return http.post(`/api/v1/Auth/login`, data);
};