import http from "api/https";
import { CustomerContactCreateModel } from "../../constants/CustomerContact";

export const getCustomerContacts = (): Promise<any> => {
  return http.get("/api/v1/CustomerContact/GetAll");
};

export const getCustomerContactsByCustomerId = (id: number): Promise<any> => {
  return http.get(`/api/v1/CustomerContact/GetByCustomerId?customer_id=${id}`);
};

export const getCustomerContact = (id: number): Promise<any> => {
  return http.get(`/api/v1/CustomerContact/GetOneById?id=${id}`);
};

export const createCustomerContact = (data: CustomerContactCreateModel): Promise<any> => {
  return http.post(`/api/v1/CustomerContact/Create`, data);
};

export const updateCustomerContact = (data: CustomerContactCreateModel): Promise<any> => {
  return http.put(`/api/v1/CustomerContact/Update`, data);
};

export const deleteCustomerContact = (id: number): Promise<any> => {
  return http.remove(`/api/v1/CustomerContact/Delete?id=${id}`, {});
};
