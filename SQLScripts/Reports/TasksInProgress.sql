use CompanyPlannerDB1
go
create view TasksInProgress as
select p1.Id as Id,
		p1.Task_Name as Task,
		p1.P_Count as Count,
		Complectation.Complectation as Complectation,
		In_Production.Number as MSLNumber,
		In_Production.Giving_Date as GivingDate,
		In_Production.Executor_Name as ExecutorName,
		In_Production.Install_Executor_Name as InstallExecName,
		In_Production.Projected_Date as ProjectedDate,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId
from Production_Plan p1 inner join Complectation on p1.Id = Complectation.Production_Task_Id
inner join In_Production on p1.Id = In_Production.Production_Task_Id
inner join HierarchyDB h1 on p1.Id=h1.ChildId
where p1.Completion = 4
go