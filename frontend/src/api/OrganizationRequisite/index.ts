import http from "api/https";
import { OrganizationRequisiteCreateModel } from "constants/OrganizationRequisite";

export const getOrganizationRequisites = (): Promise<any> => {
  return http.get("/api/v1/OrganizationRequisite/GetAll");
};

export const getOrganizationRequisitesByOrganizationId = (id: number): Promise<any> => {
  return http.get(`/api/v1/OrganizationRequisite/GetByOrganizationId?organization_id=${id}`);
};

export const getOrganizationRequisite = (id: number): Promise<any> => {
  return http.get(`/api/v1/OrganizationRequisite/GetOneById?id=${id}`);
};

export const createOrganizationRequisite = (data: OrganizationRequisiteCreateModel): Promise<any> => {
  return http.post(`/api/v1/OrganizationRequisite/Create`, data);
};

export const updateOrganizationRequisite = (data: OrganizationRequisiteCreateModel): Promise<any> => {
  return http.put(`/api/v1/OrganizationRequisite/Update`, data);
};

export const deleteOrganizationRequisite = (id: number): Promise<any> => {
  return http.remove(`/api/v1/OrganizationRequisite/Delete?id=${id}`, {});
};