use UsersTest
go
create view ManufactureNames as
select M_Name as Name
from Manufacture
where M_Name is not null
group by M_Name
go
create procedure GetManufactureNames as
begin
	set nocount on
	select * from ManufactureNames
end
exec GetManufactureNames