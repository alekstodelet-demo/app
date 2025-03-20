import http from "api/https";
import { RepresentativeCreateModel } from "constants/Representative";

export const getRepresentatives = (): Promise<any> => {
  return http.get("/api/v1/Representative/GetAll");
};

export const getRepresentative = (id: number): Promise<any> => {
  return http.get(`/api/v1/Representative/GetOneById?id=${id}`);
};

export const getRepresentativesByOrganizationId = (id: number): Promise<any> => {
  return http.get(`/api/v1/Representative/GetByOrganizationId?organization_id=${id}`);
};

export const createRepresentative = (data: RepresentativeCreateModel): Promise<any> => {
  return http.post(`/api/v1/Representative/Create`, data);
};

export const updateRepresentative = (data: RepresentativeCreateModel): Promise<any> => {
  return http.put(`/api/v1/Representative/Update`, data);
};

export const deleteRepresentative = (id: number): Promise<any> => {
  return http.remove(`/api/v1/Representative/Delete?id=${id}`, {});
};