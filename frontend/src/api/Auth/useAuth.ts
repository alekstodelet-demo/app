import http from "api/https";
import { Auth } from "constants/Auth";

export const auth = (data: Auth): Promise<any> => {
  // Authentication endpoint that will set HttpOnly cookie
  return http.post(`/api/v1/Auth/login`, data, { withCredentials: true });
};

export const logout = (): Promise<any> => {
  // Logout endpoint that will clear the HttpOnly cookie
  return http.post(`/api/v1/Auth/logout`, {}, { withCredentials: true });
};

export const refreshToken = (): Promise<any> => {
  // Refresh token endpoint
  return http.post(`/api/v1/Auth/refresh-token`, {}, { withCredentials: true });
};

export const checkAuthStatus = (): Promise<any> => {
  // Endpoint to check if the user is authenticated
  return http.get(`/api/v1/Auth/validate`, { withCredentials: true });
};

export interface LoginRequest {
  pin: string;
  tokenId: string;
  signature: string;
  deviceId?: string;
}

export interface AuthResponse {
  accessToken: string;
  expiresIn: number;
  tokenType: string;
  refreshToken: string;
  deviceId: string;
}

export const login = (data: LoginRequest): Promise<any> => {
  return http.post(`/api/v1/Auth/login`, data, { withCredentials: true });
};

export const validateToken = (): Promise<any> => {
  return http.get(`/api/v1/Auth/validate`, { withCredentials: true });
};