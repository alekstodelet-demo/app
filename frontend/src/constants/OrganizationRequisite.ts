import { Dayjs } from "dayjs";

export type OrganizationRequisite = {
  id: number;
  organization_id: number; // Main table foreign key
  payment_account: string;
  bank: string;
  bik: string;
  created_at: Dayjs;
  updated_at: Dayjs;
  created_by: number;
  updated_by: number;
};

export type OrganizationRequisiteCreateModel = {
  id: number;
  organization_id: number;
  payment_account: string;
  bank: string;
  bik: string;
};