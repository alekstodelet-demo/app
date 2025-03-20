export type OrganizationType = {
    id: number;
    name: string;
    description: string;
    code: string;
    name_kg: string;
    description_kg: string;
    text_color: string;
    background_color: string;
    created_at: string;
    updated_at: string;
    created_by: number;
    updated_by: number;
  };
  
  export type OrganizationTypeCreateModel = {
    id: number;
    name: string;
    description: string;
    code: string;
    name_kg: string;
    description_kg: string;
    text_color: string;
    background_color: string;
  };