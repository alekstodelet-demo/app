import http from "api/https";
import { RepresentativeContact } from "constants/RepresentativeContact";

export const createRepresentativeContact = (data: RepresentativeContact): Promise<any> => {
  return http.post(`/api/v1/RepresentativeContact/Create`, data);
};

export const deleteRepresentativeContact = (id: number): Promise<any> => {
  return http.remove(`/api/v1/RepresentativeContact/Delete?id=${id}`, {});
};

export const getRepresentativeContact = (id: number): Promise<any> => {
  return http.get(`/api/v1/RepresentativeContact/GetOneById?id=${id}`);
};

export const getRepresentativeContacts = (): Promise<any> => {
  return http.get("/api/v1/RepresentativeContact/GetAll");
};

export const updateRepresentativeContact = (data: RepresentativeContact): Promise<any> => {
  return http.put(`/api/v1/RepresentativeContact/Update`, data);
};


// frontend/src/api/RepresentativeContact/index.ts

// Add this method if it doesn't exist already
export const getRepresentativeContactsByRepresentativeId = (representativeId: number): Promise<any> => {
  return http.get(`/api/v1/RepresentativeContact/GetByRepresentativeId?RepresentativeId=${representativeId}`);
};