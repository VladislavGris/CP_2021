USE [CompanyPlannerDB1]
GO

/****** Object:  View [dbo].[NoSpecifications]    Script Date: 18.09.2021 10:25:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[NoSpecifications] as
select p1.Id as Id,
		p1.Task_Name as Task,
		Manufacture.M_Name as Manufacturer,
		Manufacture.Letter_Num as LetterNum,
		Manufacture.Specification_Num as SpecNum,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project
from Production_Plan p1 inner join Manufacture on p1.Id = Manufacture.Production_Task_Id
inner join HierarchyDB h1 on p1.Id = h1.ChildId
where not (Manufacture.M_Name is null or Manufacture.M_Name='') and
	not (Manufacture.Letter_Num is null or Manufacture.Letter_Num='') and
	(Manufacture.Specification_Num is null or Manufacture.Specification_Num='')
GO

