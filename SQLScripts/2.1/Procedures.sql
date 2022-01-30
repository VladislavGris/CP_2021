create procedure SetBold @taskId uniqueidentifier, @value bit
as
begin
set nocount on;
update Formatting
set IsBold = @value
where ProductionTaskId = @taskId;
end
--drop procedure SetBold;
go
create procedure SetUnderline @taskId uniqueidentifier, @value bit
as
begin
set nocount on;
update Formatting
set IsUnderline = @value
where ProductionTaskId = @taskId;
end
--drop procedure SetUnderline;
go
create procedure SetFontSize @taskId uniqueidentifier, @value int
as
begin
set nocount on
update Formatting
set FontSize = @value
where ProductionTaskId = @taskId;
end
--drop procedure ProductionTaskId;
go
create table TimedGiving(
Id uniqueidentifier primary key default(newid()),
IsTimedGiving bit default(0) not null,
IsSKBCheck bit default(0) not null,
IsOECStorage bit default(0) not null,
SKBNumber nvarchar(50) null,
FIO nvarchar(150) null,
GivingDate date null,
Note nvarchar(300) null,
ProductionTaskId uniqueidentifier foreign key references Production_Plan(Id) on delete cascade
);
go
create index TimedGivingFK on TimedGiving(ProductionTaskId);
create index TimedGivingCover on TimedGiving(Id) include (IsTimedGiving,IsSKBCheck,IsOECStorage);
go
create procedure GetTimedGivingData @taskId uniqueidentifier
as
begin
set nocount on;
select t.Id, t.IsTimedGiving, t.IsSKBCheck, t.IsOECStorage, t.SKBNumber, t.FIO, t.GivingDate, t.Note
from TimedGiving t
where t.ProductionTaskId = @taskId;
end
--drop procedure GetTimedGivingData
exec GetTimedGivingData 'f9b55ad9-faff-448b-a150-32cd0668a2d2';
go
create procedure UpdateTimedGiving @id uniqueidentifier, @isTimedGiving bit, @isSKBCheck bit, @isOECStorage bit,
	@skbNumber nvarchar(50), @fio nvarchar(150), @givingDate date, @note nvarchar(300)
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
Note = @note
where Id = @id;
end;
--drop procedure UpdateTimedGiving
go
grant execute
on GetTimedGivingData
to user1;
grant execute
on UpdateTimedGiving
to user1;
grant execute
on InsertEmptyTask
to user1;
grant execute
on GetTaskById
to user1;
grant execute
on GetTasksByParent
to user1;
grant execute
on PasteTask
to user1;
grant execute
on PasteChildren
to user1;
go
create procedure InsertEmptyTask @parentId uniqueidentifier, @lineOrder int
as
begin
	set nocount on;
	begin try
		begin tran
		exec DownTasks @parentId, @lineOrder;
		declare @insertedId table(id uniqueidentifier);

		insert into Production_Plan(Task_Name)
		output inserted.Id into @insertedId
		values ('Новое изделие');

		declare @id uniqueidentifier;
		select top 1 @id = id from @insertedId;

		insert into Act(ProductionTaskId) values(@id);
		insert into Complectation(Production_Task_Id) values(@id);
		insert into Formatting(ProductionTaskId) values(@id);
		insert into Giving(Production_Task_Id) values(@id);
		insert into In_Production(Production_Task_Id) values(@id);
		insert into LaborCosts(ProductionTaskId) values(@id);
		insert into Manufacture(Production_Task_Id) values(@id);
		insert into Payment(ProductionTaskId) values(@id);
		insert into HierarchyDB(ParentId, ChildId, LineOrder) values(@parentId, @id, @lineOrder);
		insert into TimedGiving(ProductionTaskId) values(@id);
		exec GetTaskById @id;
		commit
	end try
	begin catch
		rollback
		print 'Error';
		THROW;
	end catch
	
end
--drop procedure InsertEmptyTask;
go
create procedure GetTaskById @id uniqueidentifier
as
begin
	set nocount on;
	declare @idCopy uniqueidentifier;
	set @idCopy = @id
	select top 1 p.Id, p.Inc_Doc, p.Manag_Doc, p.Task_Name, p.P_Count, p.Specification_Cost, p.Expend_Num, p.Note, p.Expanded, p.Completion, p.EditingBy,
				h.ChildId, h.LineOrder, h.ParentId,
				f.FontFamily, f.FontSize, f.IsBold, f.IsItalic, f.IsUnderline,
				c.Complectation, c.Comp_Percentage, c.Rack, c.Shelf,
				g.G_State,
				m.M_Name,
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
				f.FontFamily, f.FontSize, f.IsBold, f.IsItalic, f.IsUnderline,
				c.Complectation, c.Comp_Percentage, c.Rack, c.Shelf,
				g.G_State,
				m.M_Name,
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
				f.FontFamily, f.FontSize, f.IsBold, f.IsItalic, f.IsUnderline,
				c.Complectation, c.Comp_Percentage, c.Rack, c.Shelf,
				g.G_State,
				m.M_Name,
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
create proc PasteTask @taskId uniqueidentifier, @parentId uniqueidentifier, @lineOrder int
as
begin
set nocount on;
SET XACT_ABORT ON
	begin try
		begin tran;
		declare @newId uniqueidentifier = newid(), @oldId uniqueidentifier = @taskId;

		select * into ##ProductionPlanPaste
		from Production_Plan
		where Id = @taskId;
		select * into ##ActPaste
		from Act
		where ProductionTaskId = @taskId;
		select * into ##ComplectationPaste
		from Complectation
		where Production_Task_Id = @taskId;
		select * into ##FormattingPaste
		from Formatting
		where ProductionTaskId = @taskId;
		select * into ##GivingPaste
		from Giving
		where Production_Task_Id = @taskId;
		select * into ##HierarchyPaste
		from HierarchyDB
		where ChildId = @taskId;
		select * into ##InProductionPaste
		from In_Production
		where Production_Task_Id = @taskId;
		select * into ##LaborCostsPaste
		from LaborCosts
		where ProductionTaskId = @taskId;
		select * into ##ManufacturePaste
		from Manufacture
		where Production_Task_Id = @taskId;
		select * into ##PaymentPaste
		from Payment
		where ProductionTaskId = @taskId;
		select * into ##TimedGivingPaste
		from TimedGiving
		where ProductionTaskId = @taskId;

		update ##ProductionPlanPaste set Id = @newId;
		update ##ActPaste set Id = newid(), ProductionTaskId = @newId;
		update ##ComplectationPaste set Id = newid(), Production_Task_Id = @newId;
		update ##FormattingPaste set Id = newid(), ProductionTaskId = @newId;
		update ##GivingPaste set Id = newid(), Production_Task_Id = @newId;
		update ##HierarchyPaste set Id = newid(), ParentId = @parentId, ChildId = @newId, LineOrder = @lineOrder;
		update ##InProductionPaste set Id = newid(), Production_Task_Id = @newId;
		update ##LaborCostsPaste set Id = newid(), ProductionTaskId = @newId;
		update ##ManufacturePaste set Id = newid(), Production_Task_Id = @newId;
		update ##PaymentPaste set Id = newid(), ProductionTaskId = @newId;
		update ##TimedGivingPaste set Id = newid(), ProductionTaskId = @newId;

		exec DownTasks @parentId, @lineOrder;

		exec PasteChildren @taskId, @newId;

		insert into Production_Plan
		select * from ##ProductionPlanPaste;
		insert into Act
		select * from ##ActPaste;
		insert into Complectation
		select * from ##ComplectationPaste;
		insert into Formatting
		select * from ##FormattingPaste;
		insert into Giving
		select * from ##GivingPaste;
		insert into HierarchyDB
		select * from ##HierarchyPaste;
		insert into In_Production
		select * from ##InProductionPaste;
		insert into LaborCosts(Id, Project, Subcont, MarkingRank, MarkingHours, AssemblyRank, AssemblyHours, SettingRank, SettingHours, Date, ProductionTaskId)
		select Id, Project, Subcont, MarkingRank, MarkingHours, AssemblyRank, AssemblyHours, SettingRank, SettingHours, l.Date, l.ProductionTaskId from ##LaborCostsPaste l;
		insert into Manufacture
		select * from ##ManufacturePaste;
		insert into Payment
		select * from ##PaymentPaste;
		insert into TimedGiving
		select * from ##TimedGivingPaste

		--select * from ##ProductionPlanPaste
		--select * from ##HierarchyPaste

		exec GetTaskById @newId;

		drop table ##ProductionPlanPaste;
		drop table ##ActPaste;
		drop table ##ComplectationPaste;
		drop table ##FormattingPaste;
		drop table ##GivingPaste;
		drop table ##HierarchyPaste;
		drop table ##InProductionPaste;
		drop table ##LaborCostsPaste;
		drop table ##ManufacturePaste;
		drop table ##PaymentPaste;
		drop table ##TimedGivingPaste;
		--rollback;
		commit;
	end try
	begin catch
		rollback;
		print 'Error';
		throw;
	end catch
end;
go
--drop procedure PasteTask
create proc PasteChildren @parentId uniqueidentifier, @newParentId uniqueidentifier as
begin
	set nocount on
	declare Children cursor local for select ChildId from HierarchyDB where ParentId = @parentId;
	declare @childId uniqueidentifier;
	open Children;
	fetch Children into @childId;
	while @@FETCH_STATUS = 0
	begin
		declare @newId uniqueidentifier = newid();
		insert into ##ProductionPlanPaste
		select * from Production_Plan
		where Id = @childId;
		insert into ##ActPaste
		select * from Act
		where ProductionTaskId = @childId;
		insert into ##ComplectationPaste
		select * from Complectation
		where Production_Task_Id = @childId;
		insert into ##FormattingPaste
		select * from Formatting
		where ProductionTaskId = @childId;
		insert into ##GivingPaste
		select * from Giving
		where Production_Task_Id = @childId;
		insert into ##HierarchyPaste
		select * from HierarchyDB
		where ChildId = @childId;
		insert into ##InProductionPaste
		select * from In_Production
		where Production_Task_Id = @childId;
		insert into ##LaborCostsPaste
		select * from LaborCosts
		where ProductionTaskId = @childId;
		insert into ##ManufacturePaste
		select * from Manufacture
		where Production_Task_Id = @childId;
		insert into ##PaymentPaste
		select * from Payment
		where ProductionTaskId = @childId;
		insert into ##TimedGivingPaste
		select * from TimedGiving
		where ProductionTaskId = @childId;

		update ##ProductionPlanPaste set Id = @newId where Id = @childId;
		update ##ActPaste set Id = newid(), ProductionTaskId = @newId where ProductionTaskId = @childId;
		update ##ComplectationPaste set Id = newid(), Production_Task_Id = @newId where Production_Task_Id = @childId;
		update ##FormattingPaste set Id = newid(), ProductionTaskId = @newId where ProductionTaskId = @childId;
		update ##GivingPaste set Id = newid(), Production_Task_Id = @newId where Production_Task_Id = @childId;
		update ##HierarchyPaste set Id = newid(), ParentId = @newParentId, ChildId = @newId where ChildId = @childId;
		update ##InProductionPaste set Id = newid(), Production_Task_Id = @newId where Production_Task_Id = @childId;
		update ##LaborCostsPaste set Id = newid(), ProductionTaskId = @newId where ProductionTaskId = @childId;
		update ##ManufacturePaste set Id = newid(), Production_Task_Id = @newId where Production_Task_Id = @childId; 
		update ##PaymentPaste set Id = newid(), ProductionTaskId = @newId where ProductionTaskId = @childId;
		update ##TimedGivingPaste set Id = newid(), ProductionTaskId = @newId where ProductionTaskId = @childId;

		exec PasteChildren @childId, @newId;

		fetch Children into @childId;
	end
	close Children;
	deallocate Children;
end
--{f9b55ad9-faff-448b-a150-32cd0668a2d2}
--null
--10
exec PasteTask 'f9b55ad9-faff-448b-a150-32cd0668a2d2',null,10;
--drop procedure PasteChildren
go
set nocount on;
declare TimedGivingCursor cursor for
select Id from Production_Plan;
open TimedGivingCursor;
declare @id uniqueidentifier;
fetch next from TimedGivingCursor into @id;
while @@fetch_status = 0
begin
insert into TimedGiving(ProductionTaskId) values(@id);
fetch next from TimedGivingCursor into @id;
end
close TimedGivingCursor;
deallocate TimedGivingCursor;
go

