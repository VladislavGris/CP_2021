use CompanyPlannerDB1
go
create view ManufactureNames as
select M_Name as Name
from Manufacture
where M_Name is not null
group by M_Name