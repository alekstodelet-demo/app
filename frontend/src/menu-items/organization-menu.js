// assets
import {
    IconBuilding,
    IconBuildingCommunity,
    IconUser,
    IconUsers,
    IconAddressBook,
    IconFileInvoice
  } from "@tabler/icons-react";
  
  // constant
  export const icons = {
    IconBuilding,
    IconBuildingCommunity,
    IconUser,
    IconUsers,
    IconAddressBook,
    IconFileInvoice
  };
  
  // ==============================|| ORGANIZATION MENU ITEMS ||============================== //
  
  const organizationMenu = {
    id: "organization",
    title: "Организации",
    type: "group",
    children: [
      {
        id: "organizations",
        title: "Справочники организаций",
        type: "collapse",
        icon: icons.IconBuildingCommunity,
        children: [
          {
            id: "Organization",
            title: "Организации",
            type: "item",
            icon: icons.IconBuilding,
            url: "/user/Organization"
          },
          {
            id: "OrganizationType",
            title: "Типы организаций",
            type: "item",
            icon: icons.IconBuildingCommunity,
            url: "/user/OrganizationType"
          },
          {
            id: "Representative",
            title: "Представители",
            type: "item",
            icon: icons.IconUser,
            url: "/user/Representative"
          },
          {
            id: "RepresentativeType",
            title: "Типы представителей",
            type: "item",
            icon: icons.IconUsers,
            url: "/user/RepresentativeType"
          }
        ]
      }
    ]
  };
  
  export default organizationMenu;