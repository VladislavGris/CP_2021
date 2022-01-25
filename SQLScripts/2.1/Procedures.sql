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
grant execute on SetBold
to user1;