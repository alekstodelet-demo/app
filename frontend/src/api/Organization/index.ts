import http from "api/https";
import { OrganizationCreateModel } from "constants/Organization";

export const getOrganizations = (): Promise<any> => {
  return http.get("/api/v1/Organization/GetAll");
};

export const getOrganization = (id: number): Promise<any> => {
  return http.get(`/api/v1/Organization/GetOneById?id=${id}`);
};

export const createOrganization = (data: OrganizationCreateModel): Promise<any> => {
  return http.post(`/api/v1/Organization/Create`, data);
};

export const updateOrganization = (data: OrganizationCreateModel): Promise<any> => {
  return http.put(`/api/v1/Organization/Update`, data);
};

export const deleteOrganization = (id: number): Promise<any> => {
  return http.remove(`/api/v1/Organization/Delete?id=${id}`, {});
};