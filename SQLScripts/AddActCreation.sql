use CompanyPlannerDB1
go
alter table Production_Plan
add ActCreation bit default(0)
