create procedure GetSubTasksByProjectName @name nvarchar(max)
as
begin
	set nocount on;
	declare @taskId uniqueidentifier
	select top 1 @taskId = Id from Production_Plan where Task_Name = @name;
	exec GetTasksByParent @taskId;
end