use CompanyPlannerDB1
go
create view SpecificationsInVipisk as
select p1.Id as Id,
		p1.Task_Name as Task,
		p1.Manag_Doc as ManagDoc,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		p1.P_Count as Count,
		Manufacture.M_Name as Manufacturer,
		Manufacture.Specification_Num as SpecNum,
		Manufacture.VipiskSpec as VipiskSpec,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Manufacture on p1.Id = Manufacture.Production_Task_Id
where Manufacture.VipiskSpec = 1
go