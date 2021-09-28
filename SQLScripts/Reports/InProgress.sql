use CompanyPlannerDB1
go
create view InProgress as
select p1.Id as Id,
		p1.Task_Name as Task,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		p1.P_Count as Count,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		c1.Complectation as Complectation,
		i1.Number as MSLNumber,
		i1.Giving_Date as GivingDate,
		i1.Executor_Name as Executor1,
		i1.Install_Executor_Name as Executor2,
		i1.Projected_Date as ProjectedDate
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Complectation c1 on p1.Id = c1.Production_Task_Id
inner join In_Production i1 on p1.Id = i1.Production_Task_Id
where p1.Completion = 4
go