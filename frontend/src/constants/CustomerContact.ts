import { Dayjs } from "dayjs";

export type CustomerContact = {
  id: number;
  customer_id: number; //main table
  value: string;
  type_id: number;
  type_name: string;
  allow_notification: boolean;
};

export type CustomerContactCreateModel = {
  id: number;
  value: string;
  type_id: number;
  customer_id: number;
  allow_notification: boolean;
};
