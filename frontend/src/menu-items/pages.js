// assets
import {
  IconKey,
  IconDesk,
  IconDeviceLaptop,
  IconMapSearch,
  IconId,
  IconUserSquareRounded,
  IconPoint,
  IconClipboardText,
  IconUser,
  IconArrowsRightLeft,
  IconFileTypeDoc,
  IconFileAnalytics,
  IconCalendarBolt,
  IconBuildingBridge2,
  IconRouteAltRight,
  IconArchive,
  IconUsers
} from "@tabler/icons-react";

// constant
export const icons = {
  IconKey,
  IconDesk,
  IconDeviceLaptop,
  IconMapSearch,
  IconId,
  IconUserSquareRounded,
  IconPoint,
  IconClipboardText,
  IconUser,
  IconArrowsRightLeft,
  IconFileTypeDoc,
  IconFileAnalytics,
  IconRouteAltRight,
  IconCalendarBolt,
  IconBuildingBridge2,
  IconArchive,
  IconUsers
};

// ==============================|| EXTRA PAGES MENU ITEMS ||============================== //

const pages = {
  id: "pages",
  title: "Pages",
  caption: "Pages Caption",
  type: "group",
  children: [
    {
      id: "Directories",
      title: "Справочники",
      type: "collapse",
      icon: icons.IconPoint,
      children: [
        {
          id: "Service",
          title: "Услуги",
          type: "item",
          icon: icons.IconDeviceLaptop,
          url: "/user/Service"
        },
        {
          id: "Workflow",
          title: "Рабочий процесс",
          type: "item",
          icon: icons.IconDesk,
          url: "/user/Workflow"
        },
        {
          id: "ArchObject",
          title: "Архитектурный объект",
          type: "item",
          icon: icons.IconMapSearch,
          url: "/user/ArchObject"
        },
        {
          id: 'ArchiveLogStatus',
          title: 'Статусы документа в архиве',
          type: 'item',
          icon: icons.IconArchive,
          url: '/user/ArchiveLogStatus',
        },
        {
          id: 'Unit_type',
          title: 'Единицы измерения',
          type: 'item',
          icon: icons.IconArchive,
          url: '/user/Unit_type',
        },
        {
          id: 'Structure_report_status',
          title: 'Статус отчета',
          type: 'item',
          icon: icons.IconArchive,
          url: '/user/Structure_report_status',
        },
        {
          id: "legal_act_registry_status",
          title: "Статус реестра правовых актов",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/legal_act_registry_status"
        },
        {
          id: "legal_registry_status",
          title: "Статус записи судебных дел",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/legal_registry_status"
        },
      ]
    },
    {
      id: "CustomPage",
      title: "Пользовательские",
      type: "collapse",
      icon: icons.IconPoint,
      children: [
        {
          id: "Application",
          title: "Заявка",
          type: "item",
          icon: icons.IconId,
          url: "/user/Application"
        },
        {
          id: "TechCouncil",
          title: "Технический совет",
          type: "item",
          icon: icons.IconId,
          url: "/user/TechCouncil"
        },
        {
          id: 'Reestrs',
          title: 'Реестры',
          type: 'item',
          icon: icons.IconUserSquareRounded,
          url: '/user/reestr',
        },
        {
          id: "AppFilter",
          title: "Статистика",
          type: "item",
          icon: icons.IconUserSquareRounded,
          url: "/user/AppFilter"
        },
        {
          id: "DashboardHeadDepartment",
          title: "Дашборд",
          type: "item",
          icon: icons.IconId,
          url: "/user/DashboardHeadDepartment"
        },
        {
          id: "DashboardEmployee",
          title: "Дашборд",
          type: "item",
          icon: icons.IconUserSquareRounded,
          url: "/user/DashboardEmployee"
        },
        {
          id: 'ApplicationFinPlan',
          title: 'Заявки фин.план',
          type: 'item',
          icon: icons.IconUserSquareRounded,
          url: '/user/ApplicationFinPlan',
        },
        {
          id: 'ReestrOtchet',
          title: 'Акты работ',
          type: 'item',
          icon: icons.IconUserSquareRounded,
          url: '/user/ReestrOtchet',
        },
        {
          id: "ApplicationReportOrganization",
          title: "Акт готовности работ по юр. лицам",
          type: "item",
          icon: icons.IconFileAnalytics,
          url: "/user/ApplicationOrganizationReport"
        },
        {
          id: "ApplicationReport",
          title: "Акт готовности работ по физ. лицам",
          type: "item",
          icon: icons.IconFileAnalytics,
          url: "/user/ApplicationReport"
        },
        {
          id: "Customer",
          title: "Заказчик",
          type: "item",
          icon: icons.IconUserSquareRounded,
          url: "/user/Customer"
        },
        {
          id: 'MyTasks',
          title: 'Мои задачи',
          type: 'item',
          icon: icons.IconUserSquareRounded,
          url: '/user/my_tasks',
        },
        {
          id: 'OrgTasks',
          title: 'Задачи структуры',
          type: 'item',
          icon: icons.IconUserSquareRounded,
          url: '/user/structure_tasks',
        },
        // {
        //   id: 'StructureReports',
        //   title: 'Отчеты структуры',
        //   type: 'item',
        //   icon: icons.IconUserSquareRounded,
        //   url: '/user/Structure_report',
        // },
        {
          id: 'AllTasks',
          title: 'Все задачи',
          type: 'item',
          icon: icons.IconUserSquareRounded,
          url: '/user/all_tasks',
        },
        {
          id: 'MyCustomSubscribtion',
          title: 'Мои подписки',
          type: 'item',
          icon: icons.IconUserSquareRounded,
          url: '/user/MyCustomSubscribtion',
        },
        {
          id: 'CustomSubscribtion',
          title: 'Подписки',
          type: 'item',
          icon: icons.IconUserSquareRounded,
          url: '/user/CustomSubscribtion',
        },
        {
          id: 'ArchiveObject',
          title: 'Архивный объект',
          type: 'item',
          icon: icons.IconBuildingBridge2,
          url: '/user/ArchiveObject',
        },
        {
          id: 'ArchitectureProcess',
          title: 'Объекты из заявок',
          type: 'item',
          icon: icons.IconBuildingBridge2,
          url: '/user/ArchitectureProcess',
        },
        {
          id: 'ArchitectureProcessToArchive',
          title: 'Объекты из ОЦиДП',
          type: 'item',
          icon: icons.IconBuildingBridge2,
          url: '/user/ArchitectureProcessToArchive',
        },
        {
          id: 'ArchiveLog',
          title: 'Журнал выдачи',
          type: 'item',
          icon: icons.IconArchive,
          url: '/user/ArchiveLog',
        },
        {
          id: 'StructureTemplates',
          title: 'Шаблоны структуры',
          type: 'item',
          icon: icons.IconArchive,
          url: '/user/StructureTemplates',
        },
        {
          id: 'ArchiveFolder',
          title: 'Архивная папка',
          type: 'item',
          icon: icons.IconArchive,
          url: '/user/archive_folder',
        },
        {
          id: 'AddArchiveFolder',
          title: 'Внести архивный документ в систему',
          type: 'item',
          icon: icons.IconArchive,
          url: '/user/archive_folder/addedit?id=0',
        },
        {
          id: "Dashboard",
          title: "Дашборд",
          type: "item",
          icon: icons.IconId,
          url: "/user/Dashboard"
        },
        {
          id: "TelegramAdmin",
          title: "Телеграм админ",
          type: "item",
          icon: icons.IconCalendarBolt,
          url: "/user/TelegramAdmin"
        },
        // TODO remove MyEmployees
        // {
        //   id: 'MyEmployees',
        //   title: 'Мои сотрудники',
        //   type: 'item',
        //   icon: icons.IconUsers,
        //   url: '/user/MyEmployees',
        // },
        {
          id: "Reports",
          title: "Отчеты",
          type: "item",
          icon: icons.IconUserSquareRounded,
          url: "/user/reports"
        },
        {
          id: "ConfigStructureReports",
          title: "Шаблон отчетов структуры",
          type: "item",
          icon: icons.IconUserSquareRounded,
          url: "/user/structureReportsConfig"
        },
        {
          id: "ListStructureReports",
          title: "Список отчетов структуры",
          type: "item",
          icon: icons.IconUserSquareRounded,
          url: "/user/Structure_report"
        },
        {
          id: "ArchiveFileNotLinked",
          title: "Неприкрепленные файлы архива",
          type: "item",
          icon: icons.IconId,
          url: "/user/ArchiveFileNotLinked"
        },
      ]
    },
    {
      id: "Admin",
      title: "Админка",
      type: "collapse",
      icon: icons.IconPoint,
      children: [
        {
          id: "1",
          title: "Шаблон документов",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/S_documenttemplate"
        },
        {
          id: "2",
          title: "Запросы",
          type: "item",
          icon: icons.IconArrowsRightLeft,
          url: "/user/S_Query"
        },
        {
          id: "documentType",
          title: "Тип документа",
          type: "item",
          icon: icons.IconFileTypeDoc,
          url: "/user/S_DocumentTemplateType"
        },
        {
          id: "HrmsEventType",
          title: "Тип события УЧР",
          type: "item",
          icon: icons.IconCalendarBolt,
          url: "/user/HrmsEventType"
        },
        {
          id: "application_road",
          title: "Путь заявки",
          type: "item",
          icon: icons.IconRouteAltRight,
          url: "/user/ApplicationRoad"
        },
        {
          id: "notificationLog",
          title: "История уведомлений",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/notificationLog"
        },
        {
          id: "f0865e9d-33ac-4bb7-9b49-f4084ab89525",
          title: "Фильтр заявки",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/FilterApplication"
        },
        {
          id: "c38ef2bb-e982-4c90-b9df-5cc46b831447",
          title: "Тип фильтра заявки",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/FilterTypeApplication"
        },
        {
          id: 'StructureReportsConfig',
          title: 'Настройка отчетов структуры',
          type: 'item',
          icon: icons.IconUserSquareRounded,
          url: '/user/Structure_report_config',
        },
        {
          id: "Unit_type",
          title: "Единица измерения",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/Unit_type"
        },
        {
          id: "Structure_report_status",
          title: "Статус отчета",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/Structure_report_status"
        },
        {
          id: "CustomerDiscount",
          title: "Скидки для заказчиков",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/CustomerDiscount"
        },
        {
          id: "DiscountDocuments",
          title: "Документы скидок",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/DiscountDocuments"
        },
        {
          id: "DiscountType",
          title: "Тип скидки",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/DiscountType"
        },
        {
          id: "DiscountDocumentType",
          title: "Тип документа скидки",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/DiscountDocumentType"
        },
        {
          id: "QueryFilters",
          title: "Фильтр запросов",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/QueryFilters"
        },
        {
          id: "WorkDocumentType",
          title: "Тип рабочего документа",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/WorkDocumentType"
        },
        {
          id: "LegalActRegistry",
          title: "Акты прокурорского реагирования",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/legal_act_registry"
        },
        {
          id: "LegalRecordRegistry",
          title: "База данных судебных дел",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/legal_record_registry"
        },
        {
          id: "LegalObject",
          title: "Объекты судебного дела",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/legal_object"
        },
        {
          id: "Release",
          title: "Релизы",
          type: "item",
          icon: icons.IconClipboardText,
          url: "/user/release"
        },
        {
          id: "TechCouncilParticipantsSettings",
          title: "Настройки участников технического совета",
          type: "item",
          icon: icons.IconKey,
          url: "/user/TechCouncilParticipantsSettings"
        }
      ]
    },
    {
      id: "StaffingTable",
      title: "Штатное расписание",
      type: "collapse",
      icon: icons.IconPoint,
      children: [
        {
          id: "employee",
          title: "Сотрудники",
          type: "item",
          icon: icons.IconUser,
          url: "/user/employee"
        },
        {
          id: "work_schedule",
          title: "Рабочий график",
          type: "item",
          icon: icons.IconKey,
          url: "/user/work_schedule"
        },
        {
          id: "structure",
          title: "Структура",
          type: "item",
          icon: icons.IconKey,
          url: "/user/org_structure"
        }
      ]
    }
  ]
};

export default pages;
