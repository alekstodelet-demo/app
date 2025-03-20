import http from "api/https";
import { CustomerCreateModel } from "constants/Customer";

export const getCustomers = (): Promise<any> => {
  return http.get("/api/v1/Customer/GetAll");
};

export const getCustomer = (id: number): Promise<any> => {
  return http.get(`/api/v1/Customer/GetOneById?id=${id}`);
};

export const createCustomer = (data: CustomerCreateModel): Promise<any> => {
  return http.post(`/api/v1/Customer/Create`, data);
};

export const updateCustomer = (data: CustomerCreateModel): Promise<any> => {
  return http.put(`/api/v1/Customer/Update`, data);
};

export const deleteCustomer = (id: number): Promise<any> => {
  return http.remove(`/api/v1/Customer/Delete?id=${id}`, {});
};
