drop table if exists OpenWeatherServiceApiKeys cascade;
drop table if exists WeatherReportApiKeys cascade;
drop table if exists Audit cascade;

create table if not exists OpenWeatherServiceApiKeys
(
    Id int generated always as identity,
    ApiKey varchar(50) not null,
    CreatedBy varchar(100) not null default current_user,
    UpdatedBy varchar(100) not null default current_user,
    CreatedDate timestamp with time zone default(now() AT TIME ZONE 'utc'::text) not null,
    UpdatedDate timestamp with time zone default(now() AT TIME ZONE 'utc'::text) not null,
    primary key (Id)
);

truncate table OpenWeatherServiceApiKeys cascade;
insert into OpenWeatherServiceApiKeys (ApiKey) values ('8b7535b42fe1c551f18028f64e8688f7');
insert into OpenWeatherServiceApiKeys (ApiKey) values ('9f933451cebf1fa39de168a29a4d9a79');

create table if not exists WeatherReportApiKeys
(
    Id int generated always as identity,
    Name varchar(50) not null, 
    UniqueId uuid not null default gen_random_uuid(), 
    RateLimitPerHour int not null default 5,
    CreatedBy varchar(100) not null default current_user,
    UpdatedBy varchar(100) not null default current_user,
    CreatedDate timestamp with time zone default(now() AT TIME ZONE 'utc'::text) not null,
    UpdatedDate timestamp with time zone default(now() AT TIME ZONE 'utc'::text) not null,
    primary key (Id)
);

truncate table WeatherReportApiKeys cascade;
insert into WeatherReportApiKeys (Name) values ('Key #1');
insert into WeatherReportApiKeys (Name) values ('Key #2');
insert into WeatherReportApiKeys (Name) values ('Key #3');
insert into WeatherReportApiKeys (Name) values ('Key #4');
insert into WeatherReportApiKeys (Name) values ('Key #5');

create table if not exists Audit
(
    Id int generated always as identity,
    WeatherReportApiKeysId int not null,
    CreatedBy varchar(100) not null default current_user,
    UpdatedBy varchar(100) not null default current_user,
    CreatedDate timestamp with time zone default(now() AT TIME ZONE 'utc'::text) not null,
    UpdatedDate timestamp with time zone default(now() AT TIME ZONE 'utc'::text) not null,
    primary key (Id),
    constraint fk_Audit_WeatherReportApiKeys foreign key (WeatherReportApiKeysId) references WeatherReportApiKeys(id) on delete set null
);
