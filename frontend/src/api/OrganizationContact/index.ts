import http from "api/https";
import { OrganizationContactCreateModel } from "constants/OrganizationContact";

export const getOrganizationContacts = (): Promise<any> => {
  return http.get("/api/v1/OrganizationContact/GetAll");
};

export const getOrganizationContactsByOrganizationId = (id: number): Promise<any> => {
  return http.get(`/api/v1/OrganizationContact/GetByOrganizationId?organization_id=${id}`);
};

export const getOrganizationContact = (id: number): Promise<any> => {
  return http.get(`/api/v1/OrganizationContact/GetOneById?id=${id}`);
};

export const createOrganizationContact = (data: OrganizationContactCreateModel): Promise<any> => {
  return http.post(`/api/v1/OrganizationContact/Create`, data);
};

export const updateOrganizationContact = (data: OrganizationContactCreateModel): Promise<any> => {
  return http.put(`/api/v1/OrganizationContact/Update`, data);
};

export const deleteOrganizationContact = (id: number): Promise<any> => {
  return http.remove(`/api/v1/OrganizationContact/Delete?id=${id}`, {});
};