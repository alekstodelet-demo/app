import http from "api/https";
import { OrganizationTypeCreateModel } from "constants/OrganizationType";

export const getOrganizationTypes = (): Promise<any> => {
  return http.get("/api/v1/OrganizationType/GetAll");
};

export const getOrganizationType = (id: number): Promise<any> => {
  return http.get(`/api/v1/OrganizationType/GetOneById?id=${id}`);
};

export const createOrganizationType = (data: OrganizationTypeCreateModel): Promise<any> => {
  return http.post(`/api/v1/OrganizationType/Create`, data);
};

export const updateOrganizationType = (data: OrganizationTypeCreateModel): Promise<any> => {
  return http.put(`/api/v1/OrganizationType/Update`, data);
};

export const deleteOrganizationType = (id: number): Promise<any> => {
  return http.remove(`/api/v1/OrganizationType/Delete?id=${id}`, {});
};