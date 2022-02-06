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
exec GetSKBNumbers