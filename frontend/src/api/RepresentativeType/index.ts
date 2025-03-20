import http from "api/https";
import { RepresentativeTypeCreateModel } from "constants/RepresentativeType";

export const getRepresentativeTypes = (): Promise<any> => {
  return http.get("/api/v1/RepresentativeType/GetAll");
};

export const getRepresentativeType = (id: number): Promise<any> => {
  return http.get(`/api/v1/RepresentativeType/GetOneById?id=${id}`);
};

export const createRepresentativeType = (data: RepresentativeTypeCreateModel): Promise<any> => {
  return http.post(`/api/v1/RepresentativeType/Create`, data);
};

export const updateRepresentativeType = (data: RepresentativeTypeCreateModel): Promise<any> => {
  return http.put(`/api/v1/RepresentativeType/Update`, data);
};

export const deleteRepresentativeType = (id: number): Promise<any> => {
  return http.remove(`/api/v1/RepresentativeType/Delete?id=${id}`, {});
};