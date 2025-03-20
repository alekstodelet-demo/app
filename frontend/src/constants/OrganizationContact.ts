import { Dayjs } from "dayjs";

export type OrganizationContact = {
  id: number;
  organization_id: number; // Main table foreign key
  value: string;
  allow_notification: boolean;
  t_type_id: number;
  type_name?: string;
  created_at: Dayjs;
  updated_at: Dayjs;
  created_by: number;
  updated_by: number;
};

export type OrganizationContactCreateModel = {
  id: number;
  organization_id: number;
  value: string;
  allow_notification: boolean;
  t_type_id: number;
};