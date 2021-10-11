use CompanyPlannerDB0
go
create proc PaymentFirstPart @dateFrom date, @dateTo date
as
select p1.Id as Id,
		p1.Task_Name as Task,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		m1.M_Name as ManufactureName,
		m1.Specification_Num as SpecNum,
		p1.Manag_Doc as ManagDoc,
		pay.SpecificationSum as SpecSum,
		pay.Project as ProjectMan,
		pay.FirstPaymentSum as FirstPaymentSum,
		pay.FirstPaymentDate as FirstPaymentDate,
		p1.Inc_Doc as IncDoc,
		pay.Note as Note
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Complectation c on p1.Id = c.Production_Task_Id
inner join Act a on a.ProductionTaskId = p1.Id
inner join Manufacture m1 on m1.Production_Task_Id = p1.Id
inner join Payment pay on pay.ProductionTaskId = p1.Id
where pay.FirstPaymentDate between @dateFrom and @dateTo and pay.IsFirstPayment = 1