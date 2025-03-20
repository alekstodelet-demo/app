import { Dayjs } from "dayjs";

export type Customer = {
  id: number;
  pin: string;
  is_organization: boolean;
  full_name: string;
  address: string;
  director: string;
  okpo: string;
  organization_type_id: number;
  payment_account: string;
  postal_code: string;
  ugns: string;
  bank: string;
  bik: string;
  registration_number: string;
  document_date_issue?: Dayjs;
  document_serie?: string;
  identity_document_type_id?: number;
  document_whom_issued?: string;
  individual_surname: string;
  individual_name: string;
  individual_secondname: string;
  is_foreign?: boolean;
  foreign_country?: number;
};


export type CustomerCreateModel = {
  id: number;
  pin: string;
  is_organization: boolean;
  full_name: string;
  address: string;
  director: string;
  okpo: string;
  organization_type_id: number;
  payment_account: string;
  postal_code: string;
  ugns: string;
  bank: string;
  bik: string;
  registration_number: string;
  document_date_issue?: Dayjs;
  document_serie?: string;
  identity_document_type_id?: number;
  document_whom_issued?: string;
  individual_surname: string;
  individual_name: string;
  individual_secondname: string;
  is_foreign?: boolean;
  foreign_country?: number;
};
