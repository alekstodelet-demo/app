import { Dayjs } from "dayjs";

export type RepresentativeContact = {
  id: number;
  representative_id: number; // Main table foreign key
  value: string;
  allow_notification: boolean;
  t_type_id: number;
  type_name?: string;
  created_at: Dayjs;
  updated_at: Dayjs;
  created_by: number;
  updated_by: number;
};

export type RepresentativeContactCreateModel = {
  id: number;
  representative_id: number;
  value: string;
  allow_notification: boolean;
  t_type_id: number;
};