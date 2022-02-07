go
alter table TimedGiving
add ReturnDate date null;
go
create procedure GetTimedGivingData @taskId uniqueidentifier
as
begin
set nocount on;
select t.Id, t.IsTimedGiving, t.IsSKBCheck, t.IsOECStorage, t.SKBNumber, t.FIO, t.GivingDate,t.ReturnDate, t.Note
from TimedGiving t
where t.ProductionTaskId = @taskId;
end
--drop procedure GetTimedGivingData
exec GetTimedGivingData 'f9b55ad9-faff-448b-a150-32cd0668a2d2';
go
create procedure UpdateTimedGiving @id uniqueidentifier, @isTimedGiving bit, @isSKBCheck bit, @isOECStorage bit,
	@skbNumber nvarchar(50), @fio nvarchar(150), @givingDate date, @returnDate date, @note nvarchar(300)
as
begin
set nocount on;
update TimedGiving
set IsTimedGiving = @isTimedGiving,
IsSKBCheck = @isSKBCheck,
IsOECStorage = @isOECStorage,
SKBNumber = @skbNumber,
FIO = @fio,
GivingDate = @givingDate,
ReturnDate = @returnDate,
Note = @note
where Id = @id
end;
--drop procedure UpdateTimedGiving
go
--drop view SKBCheck
create view SKBCheck as
select p1.Id as Id,
		p1.Task_Name as Task,
		p1.Manag_Doc as ManagDoc,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		Manufacture.M_Name as Manufacturer,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		t.GivingDate,
		t.ReturnDate,
		t.FIO,
		t.SKBNumber,
		t.Note
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Manufacture on p1.Id = Manufacture.Production_Task_Id
inner join TimedGiving t on t.ProductionTaskId = p1.Id
where t.IsSKBCheck = 1
go
--drop view SKBNumbers
create view SKBNumbers as
select SKBNumber as Number
from TimedGiving
where SKBNumber is not null and SKBNumber <> ''
group by SKBNumber
go
create procedure GetSKBNumbers as
begin
	set nocount on
	select * from SKBNumbers
end
go
alter table Formatting
add Color nvarchar(10) not null default('#FF000000');
go
create procedure SetColor @id uniqueidentifier, @color nvarchar(10)
as
begin
set nocount on;
update Formatting
set Color = @color
where ProductionTaskId = @id;
end;
--drop procedure SerColor
go
create procedure GetTaskById @id uniqueidentifier
as
begin
	set nocount on;
	declare @idCopy uniqueidentifier;
	set @idCopy = @id
	select top 1 p.Id, p.Inc_Doc, p.Manag_Doc, p.Task_Name, p.P_Count, p.Specification_Cost, p.Expend_Num, p.Note, p.Expanded, p.Completion, p.EditingBy,
				h.ChildId, h.LineOrder, h.ParentId,
				f.FontFamily, f.FontSize, f.IsBold, f.IsItalic, f.IsUnderline,f.Color,
				c.Complectation, c.Comp_Percentage, c.Rack, c.Shelf,
				g.G_State,
				m.M_Name,m.Specification_Num as 'SpecNum',
				a.ActNumber,a.ActDate,a.ActCreation,a.ByAct,
				i.Giving_Date,
				tg.IsTimedGiving, tg.IsSKBCheck, tg.IsOECStorage,
				(select count(*) from HierarchyDB h2 where h2.ParentId = p.Id) ChildrenCount
		from Production_Plan p 
		inner join Formatting f on p.Id = f.ProductionTaskId
		inner join Complectation c on p.Id = c.Production_Task_Id
		inner join Giving g on p.Id = g.Production_Task_Id
		inner join Manufacture m on p.Id = m.Production_Task_Id
		inner join Act a on p.Id = a.ProductionTaskId
		inner join In_Production i on p.Id = i.Production_Task_Id
		inner join HierarchyDB h on p.Id = h.ChildId
		inner join TimedGiving tg on tg.ProductionTaskId = p.Id
		where p.Id = @idCopy;
END
--DROP PROCEDURE GetTaskById;
go
create procedure GetTasksByParent @parentId uniqueidentifier = NULL
as
begin
	set nocount on;
	declare @idCopy uniqueidentifier;
	select @idCopy = @parentId;
	if @idCopy is null
		select p.Id, p.Inc_Doc, p.Manag_Doc, p.Task_Name, p.P_Count, p.Specification_Cost, p.Expend_Num, p.Note, p.Expanded, p.Completion, p.EditingBy,
				h.ChildId, h.LineOrder, h.ParentId,
				f.FontFamily, f.FontSize, f.IsBold, f.IsItalic, f.IsUnderline,f.Color,
				c.Complectation, c.Comp_Percentage, c.Rack, c.Shelf,
				g.G_State,
				m.M_Name,m.Specification_Num as 'SpecNum',
				a.ActNumber,a.ActDate,a.ActCreation,a.ByAct,
				i.Giving_Date,
				tg.IsTimedGiving, tg.IsSKBCheck, tg.IsOECStorage,
				(select count(*) from HierarchyDB h2 where h2.ParentId = p.Id) ChildrenCount
		from Production_Plan p 
		inner join Manufacture m on p.Id = m.Production_Task_Id
		inner join Formatting f on p.Id = f.ProductionTaskId
		inner join Complectation c on p.Id = c.Production_Task_Id
		inner join Giving g on p.Id = g.Production_Task_Id
		inner join Act a on p.Id = a.ProductionTaskId
		inner join In_Production i on p.Id = i.Production_Task_Id
		inner join HierarchyDB h on p.Id = h.ChildId
		inner join TimedGiving tg on tg.ProductionTaskId = p.Id
		where h.ParentId is null
		order by h.LineOrder
		--option(recompile);
	else
		select p.Id, p.Inc_Doc, p.Manag_Doc, p.Task_Name, p.P_Count, p.Specification_Cost, p.Expend_Num, p.Note, p.Expanded, p.Completion, p.EditingBy,
				h.ChildId, h.LineOrder, h.ParentId,
				f.FontFamily, f.FontSize, f.IsBold, f.IsItalic, f.IsUnderline,f.Color,
				c.Complectation, c.Comp_Percentage, c.Rack, c.Shelf,
				g.G_State,
				m.M_Name,m.Specification_Num as 'SpecNum',
				a.ActNumber,a.ActDate,a.ActCreation,a.ByAct,
				i.Giving_Date,
				tg.IsTimedGiving, tg.IsSKBCheck, tg.IsOECStorage,
				(select count(*) from HierarchyDB h2 where h2.ParentId = p.Id) ChildrenCount
		from Production_Plan p 
		inner join Manufacture m on p.Id = m.Production_Task_Id
		inner join Formatting f on p.Id = f.ProductionTaskId
		inner join Complectation c on p.Id = c.Production_Task_Id
		inner join Giving g on p.Id = g.Production_Task_Id
		inner join Act a on p.Id = a.ProductionTaskId
		inner join In_Production i on p.Id = i.Production_Task_Id
		inner join HierarchyDB h on p.Id = h.ChildId
		inner join TimedGiving tg on tg.ProductionTaskId = p.Id
		where h.ParentId = @idCopy
		order by h.LineOrder
		--option(recompile);
end
--drop procedure GetTasksByParent;
go
create view PaymentReport as
select p1.Id as Id,
		p1.Task_Name as Task,
		p1.Manag_Doc as ManagDoc,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		m.M_Name as Manufacturer,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
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
--drop view PaymentReport
go
create procedure GetPaymentReport as
begin
set nocount on;
select * from PaymentReport;
end
exec GetPaymentReport