USE [UsersTest1]
GO

/****** Object:  View [dbo].[PaymentReport]    Script Date: 02.04.2023 19:38:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER view [dbo].[PaymentReport] as
select p1.Id as Id,
		p1.Task_Name as Task,
		p1.Manag_Doc as ManagDoc,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		m.M_Name as Manufacturer,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		(select dbo.GetSubParent(p1.Id)) as SubProject,
		pay.Contract,
		m.Specification_Num as 'SpecNum',
		pay.SpecificationSum as 'SpecSum',
		pay.Project as 'PaymentProject',
		pay.FirstPaymentDate,
		pay.SecondPaymentDate,
		pay.FullPaymentDate,
		p1.Inc_Doc as 'IncDoc'
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Manufacture m on p1.Id = m.Production_Task_Id
inner join Payment pay on p1.Id = pay.ProductionTaskId
where m.Specification_Num is not null and m.Specification_Num <> '';
GO


