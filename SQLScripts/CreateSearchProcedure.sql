use CompanyPlannerDB12
go
create proc Search(@parm nvarchar(max))
as
select p.Id as Id, Task_Name as Name from Production_Plan p
inner join Act a on a.ProductionTaskId = p.Id
inner join Complectation c on c.Production_Task_Id = p.Id
inner join Giving g on g.Production_Task_Id = p.Id
inner join In_Production i on i.Production_Task_Id = p.Id
inner join LaborCosts l on l.ProductionTaskId = p.Id
inner join Manufacture m on m.Production_Task_Id = p.Id
inner join Payment py on py.ProductionTaskId = p.Id
where Inc_Doc like @parm or Manag_Doc like @parm or M_Name like @parm or Task_Name like @parm or Expend_Num like @parm or ActNumber like @parm or Complectation like @parm or
Bill like @parm or Report like @parm or Return_Report like @parm or Letter_Num like @parm or Specification_Num like @parm

exec Search @parm = '%изделие%'
drop proc Search
drop function Search