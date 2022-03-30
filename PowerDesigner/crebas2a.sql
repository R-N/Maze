drop index RELATIONSHIP_1_FK;

drop index ANSWER_PK;

drop table ANSWER;

drop index RELATIONSHIP_2_FK;

drop index LEVEL_PK;

drop table "LEVEL";

drop index RELATIONSHIP_8_FK;

drop index LEVEL_COMPLETION_PK;

drop table LEVEL_COMPLETION;

drop index LEVEL_PACK_PK;

drop table LEVEL_PACK;

drop index OBSTACLE_PK;

drop table OBSTACLE;

drop index RELATIONSHIP_7_FK;

drop index OBSTACLE_ITEM_PK;

drop table OBSTACLE_ITEM;

drop index RELATIONSHIP_11_FK;

drop index POWER_UP_PK;

drop table POWER_UP;

drop index PROBLEM_PK;

drop table PROBLEM;

drop index RELATIONSHIP_12_FK;

drop index SPRAY_PK;

drop table SPRAY;

drop index TRANSFORM_PK;

drop table TRANSFORM;

create table PROBLEM (
PROBLEM_ID           INTEGER                        not null,
TEXT                 VARCHAR(100),
DIFFICULTY           INTEGER,
CATEGORY_ID          INTEGER,
primary key (PROBLEM_ID)
);

create table ANSWER (
ANSWER_ID            INTEGER                        not null,
PROBLEM_ID           INTEGER                        not null,
TEXT                 VARCHAR(100),
CORRECT              INTEGER,
primary key (ANSWER_ID),
foreign key (PROBLEM_ID)
      references PROBLEM (PROBLEM_ID)
);

create unique index ANSWER_PK on ANSWER (
ANSWER_ID ASC
);

create  index RELATIONSHIP_1_FK on ANSWER (
PROBLEM_ID ASC
);

create table LEVEL_PACK (
LEVEL_PACK_ID        INTEGER                        not null,
NAME                 VARCHAR(50),
BACKGROUND           VARCHAR(50),
UNLOCKED             INTEGER,
primary key (LEVEL_PACK_ID)
);

create table "LEVEL" (
LEVEL_ID             INTEGER                        not null,
LEVEL_PACK_ID        INTEGER                        not null,
SCENE                VARCHAR(50),
NUMBER               INTEGER,
THUMBNAIL            VARCHAR(50),
TYPE                 SMALLINT,
primary key (LEVEL_ID),
foreign key (LEVEL_PACK_ID)
      references LEVEL_PACK (LEVEL_PACK_ID)
);

create unique index LEVEL_PK on "LEVEL" (
LEVEL_ID ASC
);

create  index RELATIONSHIP_2_FK on "LEVEL" (
LEVEL_PACK_ID ASC
);

create table LEVEL_COMPLETION (
LEVEL_ID             INTEGER                        not null,
LEV_LEVEL_ID         INTEGER,
PLAYER_HP            FLOAT,
primary key (LEVEL_ID),
foreign key (LEVEL_ID)
      references "LEVEL" (LEVEL_ID),
foreign key (LEV_LEVEL_ID)
      references LEVEL_COMPLETION (LEVEL_ID)
);

create unique index LEVEL_COMPLETION_PK on LEVEL_COMPLETION (
LEVEL_ID ASC
);

create  index RELATIONSHIP_8_FK on LEVEL_COMPLETION (
LEV_LEVEL_ID ASC
);

create unique index LEVEL_PACK_PK on LEVEL_PACK (
LEVEL_PACK_ID ASC
);

create table OBSTACLE (
LEVEL_ID             INTEGER                        not null,
NAME                 VARCHAR(50),
ROT_Y                FLOAT,
OBSTACLE_ID_LEVEL    INTEGER,
primary key (LEVEL_ID),
foreign key (LEVEL_ID)
      references LEVEL_COMPLETION (LEVEL_ID)
);

create unique index OBSTACLE_PK on OBSTACLE (
LEVEL_ID ASC
);

create table OBSTACLE_ITEM (
ANSWER_ID            INTEGER                        not null,
LEVEL_ID             INTEGER                        not null,
STATE                INTEGER,
OBSTACLE_ITEM_ID_OBSTACLE INTEGER,
OBSTACLE_ID_LEVEL    INTEGER,
primary key (ANSWER_ID),
foreign key (ANSWER_ID)
      references ANSWER (ANSWER_ID),
foreign key (LEVEL_ID)
      references OBSTACLE (LEVEL_ID)
);

create unique index OBSTACLE_ITEM_PK on OBSTACLE_ITEM (
ANSWER_ID ASC
);

create  index RELATIONSHIP_7_FK on OBSTACLE_ITEM (
LEVEL_ID ASC
);

create table TRANSFORM (
POS_X                FLOAT,
POS_Y                FLOAT,
POS_Z                FLOAT,
ROT_X                FLOAT,
ROT_Y                FLOAT,
ROT_Z                FLOAT,
ROT_W                FLOAT,
TRANSFORM_ID         INTEGER                        not null,
primary key (TRANSFORM_ID)
);

create table POWER_UP (
LEVEL_ID             INTEGER                        not null,
TRANSFORM_ID         INTEGER                        not null,
POWER_UP_TYPE        INTEGER,
primary key (LEVEL_ID),
foreign key (LEVEL_ID)
      references LEVEL_COMPLETION (LEVEL_ID),
foreign key (TRANSFORM_ID)
      references TRANSFORM (TRANSFORM_ID)
);

create unique index POWER_UP_PK on POWER_UP (
LEVEL_ID ASC
);

create  index RELATIONSHIP_11_FK on POWER_UP (
TRANSFORM_ID ASC
);

create unique index PROBLEM_PK on PROBLEM (
PROBLEM_ID ASC
);

create table SPRAY (
LEVEL_ID             INTEGER                        not null,
TRANSFORM_ID         INTEGER                        not null,
primary key (LEVEL_ID),
foreign key (LEVEL_ID)
      references LEVEL_COMPLETION (LEVEL_ID),
foreign key (TRANSFORM_ID)
      references TRANSFORM (TRANSFORM_ID)
);

create unique index SPRAY_PK on SPRAY (
LEVEL_ID ASC
);

create  index RELATIONSHIP_12_FK on SPRAY (
TRANSFORM_ID ASC
);

create unique index TRANSFORM_PK on TRANSFORM (
TRANSFORM_ID ASC
);

