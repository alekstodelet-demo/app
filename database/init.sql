create sequence application_number_sequence;

alter sequence application_number_sequence owner to postgres;

create sequence weather_forecast_id_seq;

alter sequence weather_forecast_id_seq owner to postgres;

create sequence workflow_task_template_id_seq;

alter sequence workflow_task_template_id_seq owner to postgres;

create table if not exists "CustomSvgIcon"
(
    name         varchar,
    "svgPath"    varchar,
    "usedTables" varchar,
    id           serial
        primary key,
    created_at   timestamp,
    created_by   integer,
    updated_at   timestamp,
    updated_by   integer
);

alter table "CustomSvgIcon"
    owner to postgres;

create table if not exists "Language"
(
    name             varchar,
    description      varchar,
    code             varchar,
    "isDefault"      boolean,
    "queueNumber"    integer,
    id               serial
        primary key,
    created_at       timestamp,
    created_by       integer,
    updated_at       timestamp,
    updated_by       integer,
    name_kg          text,
    description_kg   text,
    text_color       text,
    background_color text
);

alter table "Language"
    owner to postgres;

create table if not exists "S_DocumentTemplateType"
(
    id               serial
        primary key,
    name             varchar,
    description      varchar,
    code             varchar,
    "queueNumber"    integer,
    created_at       timestamp,
    created_by       integer,
    updated_at       timestamp,
    updated_by       integer,
    name_kg          text,
    description_kg   text,
    text_color       text,
    background_color text
);

alter table "S_DocumentTemplateType"
    owner to postgres;

create table if not exists "S_PlaceHolderType"
(
    name          varchar,
    description   varchar,
    code          varchar,
    "queueNumber" integer,
    id            serial
        primary key,
    created_at    timestamp,
    created_by    integer,
    updated_at    timestamp,
    updated_by    integer
);

alter table "S_PlaceHolderType"
    owner to postgres;

create table if not exists "S_PlaceHolderTemplate"
(
    id                  serial
        primary key,
    name                varchar,
    description         varchar,
    code                varchar,
    "idQuery"           integer not null,
    "idPlaceholderType" integer not null
        constraint "R_1212"
            references "S_PlaceHolderType",
    value               text,
    created_at          timestamp,
    created_by          integer,
    updated_at          timestamp,
    updated_by          integer,
    name_kg             text,
    description_kg      text,
    text_color          text,
    background_color    text
);

alter table "S_PlaceHolderTemplate"
    owner to postgres;

create table if not exists "S_Query"
(
    name             varchar,
    description      varchar,
    code             varchar,
    query            varchar,
    id               serial
        primary key,
    created_at       timestamp,
    created_by       integer,
    updated_at       timestamp,
    updated_by       integer,
    name_kg          text,
    description_kg   text,
    text_color       text,
    background_color text
);

alter table "S_Query"
    owner to postgres;

create table if not exists document_metadata
(
    id         serial
        primary key,
    metadata   jsonb,
    version    text,
    created_at timestamp,
    updated_at timestamp,
    created_by integer,
    updated_by integer,
    name       varchar
);

alter table document_metadata
    owner to postgres;

create table if not exists "S_DocumentTemplate"
(
    id                serial
        primary key,
    name              varchar,
    description       varchar,
    code              varchar,
    "idCustomSvgIcon" integer
        constraint "R_441"
            references "CustomSvgIcon",
    "iconColor"       varchar,
    "idDocumentType"  integer not null
        constraint "R_608"
            references "S_DocumentTemplateType",
    metadata_id       integer
        references document_metadata,
    created_at        timestamp,
    created_by        integer,
    updated_at        timestamp,
    updated_by        integer,
    name_kg           text,
    description_kg    text,
    text_color        text,
    background_color  text
);

alter table "S_DocumentTemplate"
    owner to postgres;

create table if not exists "S_DocumentTemplateTranslation"
(
    id                   serial
        primary key,
    template             varchar,
    "idDocumentTemplate" integer not null
        constraint "R_271"
            references "S_DocumentTemplate",
    "idLanguage"         integer not null
        constraint "R_1213"
            references "Language",
    created_at           timestamp,
    created_by           integer,
    updated_at           timestamp,
    updated_by           integer
);

alter table "S_DocumentTemplateTranslation"
    owner to postgres;

create table if not exists "S_QueriesDocumentTemplate"
(
    id                   serial
        primary key,
    "idDocumentTemplate" integer not null
        constraint "R_101"
            references "S_DocumentTemplateTranslation",
    "idQuery"            integer not null
        constraint "R_102"
            references "S_Query",
    created_at           timestamp,
    created_by           integer,
    updated_at           timestamp,
    updated_by           integer
);

alter table "S_QueriesDocumentTemplate"
    owner to postgres;

create table if not exists "S_TemplateDocumentPlaceholder"
(
    id                   serial
        primary key,
    "idTemplateDocument" integer not null
        constraint "R_138"
            references "S_DocumentTemplateTranslation",
    "idPlaceholder"      integer not null
        constraint "R_139"
            references "S_PlaceHolderTemplate",
    created_at           timestamp,
    created_by           integer,
    updated_at           timestamp,
    updated_by           integer
);

alter table "S_TemplateDocumentPlaceholder"
    owner to postgres;