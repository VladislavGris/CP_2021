USE [UsersTest1]
GO

/****** Object:  View [dbo].[ActCreation]    Script Date: 02.04.2023 18:11:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER view [dbo].[ActCreation] as
select p1.Id as Id,
		p1.Task_Name as Task,
		p1.Manag_Doc as ManagDoc,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		p1.P_Count as Count,
		Manufacture.M_Name as Manufacturer,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		(select dbo.GetSubParent(p1.Id)) as SubProject
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Manufacture on p1.Id = Manufacture.Production_Task_Id
inner join Act a on a.ProductionTaskId = p1.Id
where a.ActCreation = 1;
GO