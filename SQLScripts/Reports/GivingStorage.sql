use CompanyPlannerDB1
go
create view GivingStorage as
select p1.Id as Id,
		p1.Task_Name as Task,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		p1.P_Count as Count,
		c1.Complectation as Complectation,
		g1.Receiving_Date as ReceivingDate
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Complectation c1 on p1.Id = c1.Production_Task_Id
inner join Giving g1 on p1.Id = g1.Production_Task_Id
inner join Manufacture m1 on p1.Id = m1.Production_Task_Id
where g1.G_State = 1 and g1.Receiving_Date is not null and (m1.Letter_Num is null or m1.Letter_Num = '')
go