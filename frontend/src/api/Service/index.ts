import http from "api/https";
import { Service } from "../../constants/Service";

export const getServices = (): Promise<any> => {
  return http.get("/Service/GetAll");
};

export const getService = (id: number): Promise<any> => {
  return http.get(`/Service/GetOneById?id=${id}`);
};

export const createService = (data: Service): Promise<any> => {
  return http.post(`/Service/Create`, data);
};

export const updateService = (data: Service): Promise<any> => {
  return http.put(`/Service/Update`, data);
};

export const deleteService = (id: number): Promise<any> => {
  return http.remove(`/Service/Delete?id=${id}`, {});
};
