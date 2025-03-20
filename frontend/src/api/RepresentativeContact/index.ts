import http from "api/https";
import { RepresentativeContactCreateModel } from "constants/RepresentativeContact";

export const getRepresentativeContacts = (): Promise<any> => {
  return http.get("/api/v1/RepresentativeContact/GetAll");
};

export const getRepresentativeContactsByRepresentativeId = (id: number): Promise<any> => {
  return http.get(`/api/v1/RepresentativeContact/GetByRepresentativeId?representative_id=${id}`);
};

export const getRepresentativeContact = (id: number): Promise<any> => {
  return http.get(`/api/v1/RepresentativeContact/GetOneById?id=${id}`);
};

export const createRepresentativeContact = (data: RepresentativeContactCreateModel): Promise<any> => {
  return http.post(`/api/v1/RepresentativeContact/Create`, data);
};

export const updateRepresentativeContact = (data: RepresentativeContactCreateModel): Promise<any> => {
  return http.put(`/api/v1/RepresentativeContact/Update`, data);
};

export const deleteRepresentativeContact = (id: number): Promise<any> => {
  return http.remove(`/api/v1/RepresentativeContact/Delete?id=${id}`, {});
};