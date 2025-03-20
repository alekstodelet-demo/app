export type Service = {
  id: number;
  name: string;
  short_name: string;
  code: string;
  description: string;
  day_count: number;
  workflow_id: number;
  workflow_name?: string;
  price: number;
};


export type ServiceCreateModel = {
  id: number;
  name: string;
  short_name: string;
  code: string;
  description: string;
  day_count: number;
  workflow_id: number;
  price: number;
};
