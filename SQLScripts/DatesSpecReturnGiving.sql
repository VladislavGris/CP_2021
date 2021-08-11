use CompanyPlannerDB2
go
alter table Manufacture
add VipiskSpec bit not null default(0)
go
alter table Manufacture
add PredictDate date null
go
alter table Manufacture
add FactDate date null
go
alter table Giving
add ReturnGiving bit not null default(0)