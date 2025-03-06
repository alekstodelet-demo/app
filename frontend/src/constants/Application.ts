import { Dayjs } from "dayjs";
import { Customer } from "./Customer";
import { ArchObject } from "./ArchObject";

export type Application = {
  id: number;
  registration_date: Dayjs;
  customer_id: number;
  status_id: number;
  workflow_id: number;
  service_id: number;
  deadline: Dayjs;
  arch_object_id: number;
  arch_process_id?: number;
  updated_at: Dayjs;
  customer: Customer;
  archObjects: ArchObject[];
  number?: string;
  work_description?: string;
  service_name?: string;
  status_name?: string;
  status_code?: string;
  object_tag_id: number;
  object_tag_name?: number;
  arch_object_district?: string;
  incoming_numbers?: string;
  outgoing_numbers?: string;
  dp_outgoing_number?: string;
  tech_decision_id?: number;
};


export type FilterApplication = {
  pageSize: number;
  pageNumber: number;
  sort_by: string;
  sort_type: string;
  pin: string;
  customerName: string;
  date_start: string;
  date_end: string;
  service_ids: number[];
  status_ids: number[];
  address: string;
  number: string;
  district_id: number;
  deadline_day: number;
  tag_id: number;
  isExpired: boolean;
  isMyOrgApplication: boolean;
  withoutAssignedEmployee: boolean;
  employee_id: number;
  useCommon: boolean;
  common_filter: string;
  structure_ids: number[];
  incoming_numbers: string;
  outgoing_numbers: string;
  employee_arch_id?: number;
  dashboard_date_start?: string;
  dashboard_date_end?: string;
  issued_employee_id?: number;
  only_count: boolean;
  is_paid?: boolean;
  dp_outgoing_number?: string;
};

