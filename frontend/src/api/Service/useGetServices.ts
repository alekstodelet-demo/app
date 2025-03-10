import http from "api/https";

export const getServices = (): Promise<any> => {
  return http.get("/Service/GetAll");
};