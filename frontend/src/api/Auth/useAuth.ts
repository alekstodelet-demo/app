import http from "api/https";
import { Auth } from "constants/Auth";

export const auth = (data: Auth): Promise<any> => {
  // Authentication endpoint that will set HttpOnly cookie
  return http.post(`/api/v1/Auth/login`, data, {}, { withCredentials: true });
};

export const logout = (): Promise<any> => {
  // Logout endpoint that will clear the HttpOnly cookie
  return http.post(`/api/v1/Auth/logout`, {}, {}, { withCredentials: true });
};

export const refreshToken = (): Promise<any> => {
  // Refresh token endpoint
  return http.post(`/api/v1/Auth/refresh-token`, {}, {}, { withCredentials: true });
};

export const checkAuthStatus = (): Promise<any> => {
  // Endpoint to check if the user is authenticated
  return http.get(`/api/v1/Auth/status`, {}, { withCredentials: true });
};