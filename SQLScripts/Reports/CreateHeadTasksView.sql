use CompanyPlannerDB1
go
create view HeadTasks as
select Task_Name as Task
from Production_Plan inner join HierarchyDB on Production_Plan.Id = HierarchyDB.ChildId
where ParentId is null

