import { Dayjs } from "dayjs";

export type Representative = {
  id: number;
  last_name: string;
  first_name: string;
  second_name: string;
  pin: string;
  company_id: number;
  inn: string;
  type_id: number;
  type_name?: string;
  created_at: Dayjs;
  updated_at: Dayjs;
  created_by: number;
  updated_by: number;
};

export type RepresentativeCreateModel = {
  id: number;
  last_name: string;
  first_name: string;
  second_name: string;
  pin: string;
  company_id: number;
  inn: string;
  type_id: number;
};