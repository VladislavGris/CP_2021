USE [UsersTest1]
GO

/****** Object:  View [dbo].[WorkedDocs]    Script Date: 02.04.2023 19:37:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER view [dbo].[WorkedDocs] as
select p1.Id as Id,
		p1.Task_Name as Task,
		p1.Manag_Doc as ManagDoc,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		Manufacture.M_Name as Manufacturer,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		(select dbo.GetSubParent(p1.Id)) as SubProject,
		p1.P_Count as Count
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Manufacture on p1.Id = Manufacture.Production_Task_Id
where p1.Completion = 10
GO


