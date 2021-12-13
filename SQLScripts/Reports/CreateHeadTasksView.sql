use UsersTest
go
create view HeadTasks as
select Task_Name as Task
from Production_Plan inner join HierarchyDB on Production_Plan.Id = HierarchyDB.ChildId
where ParentId is null
go
create procedure GetHeadTasks as
begin
	set nocount on;
	select * from HeadTasks;
end

