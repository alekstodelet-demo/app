import { Dayjs } from "dayjs";

export type Organization = {
  id: number;
  name: string;
  address: string;
  director: string;
  nomer: string;
  pin: string;
  okpo: string;
  postal_code: string;
  ugns: string;
  reg_number: string;
  organization_type_id: number;
  organization_type_name?: string;
  created_at: Dayjs;
  updated_at: Dayjs;
  created_by: number;
  updated_by: number;
};

export type OrganizationCreateModel = {
  id: number;
  name: string;
  address: string;
  director: string;
  nomer: string;
  pin: string;
  okpo: string;
  postal_code: string;
  ugns: string;
  reg_number: string;
  organization_type_id: number;
};