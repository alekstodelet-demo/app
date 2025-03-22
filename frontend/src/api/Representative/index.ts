import http from "api/https";
import { Representative } from "constants/Representative";

export const createRepresentative = (data: Representative): Promise<any> => {
  return http.post(`/api/v1/Representative/Create`, data);
};

export const deleteRepresentative = (id: number): Promise<any> => {
  return http.remove(`/api/v1/Representative/Delete?id=${id}`, {});
};

export const getRepresentative = (id: number): Promise<any> => {
  return http.get(`/api/v1/Representative/GetOneById?id=${id}`);
};

export const getRepresentatives = (): Promise<any> => {
  return http.get("/api/v1/Representative/GetAll");
};

export const updateRepresentative = (data: Representative): Promise<any> => {
  return http.put(`/api/v1/Representative/Update`, data);
};


export const getRepresentativesByCompanyId = (companyId: number): Promise<any> => {
  return http.get(`/api/v1/Representative/GetByCompanyId?CompanyId=${companyId}`);
};

// frontend/src/api/Representative/index.ts

// Add this new function
export const createRepresentativeWithPin = (pin: string): Promise<any> => {
  return http.post(`/api/v1/Representative/CreateWithPin`, { pin });
};

// Add this function to src/api/Representative/index.ts

export const createRepresentativeWithInn = (data: { inn: string }): Promise<any> => {
  return http.post(`/api/v1/Representative/CreateWithInn`, data);
};

// Add this to src/api/Representative/index.ts

export const updateRepresentativeHasAccess = (data: { id: number, hasAccess: boolean }): Promise<any> => {
  return http.put(`/api/v1/Representative/UpdateHasAccess`, data);
};