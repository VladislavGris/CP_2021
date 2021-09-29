use CompanyPlannerDB0
go
create view ActForm as
select p1.Id as Id,
		p1.Task_Name as Task,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		p1.Manag_Doc as ManagDoc,
		p1.P_Count as Count,
		c.Complectation as Complectation,
		a.ActNumber as ActNumber,
		a.ActDate as ActDate
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Manufacture on p1.Id = Manufacture.Production_Task_Id
inner join Complectation c on p1.Id = c.Production_Task_Id
inner join Act a on p1.Id = a.ProductionTaskId
where a.ActCreation = 1
go