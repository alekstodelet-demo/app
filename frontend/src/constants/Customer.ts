import { Dayjs } from "dayjs";

export type Customer = {
  
  id: number;
  pin: string;
  okpo: string;
  postalCode: string;
  ugns: string;
  regNumber: string;
  organizationTypeId: number;
  name: string;
  address: string;
  director: string;
  nomer: string;
};


export type CustomerCreateModel = {
  
  id: number;
  pin: string;
  okpo: string;
  postalCode: string;
  ugns: string;
  regNumber: string;
  organizationTypeId: number;
  name: string;
  address: string;
  director: string;
  nomer: string;
};
