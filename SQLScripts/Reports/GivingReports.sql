use CompanyPlannerDB1
go
create view GivingReports as
select p1.Id as Id,
		p1.Task_Name as Task,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		p1.Manag_Doc as ManagDoc,
		p1.P_Count as Count,
		m1.M_Name as Manufacturer,
		m1.Specification_Num as SpecNum,
		p1.Inc_Doc as IncDoc,
		g1.Report as Report
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Complectation c1 on p1.Id = c1.Production_Task_Id
inner join Giving g1 on p1.Id = g1.Production_Task_Id
inner join Manufacture m1 on p1.Id = m1.Production_Task_Id
where g1.G_State = 1 and p1.Inc_Doc is not null and (g1.Report is null or g1.Report = '')
go