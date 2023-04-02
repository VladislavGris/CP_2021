USE [UsersTest1]
GO

/****** Object:  StoredProcedure [dbo].[SearchIn_Production]    Script Date: 02.04.2023 21:18:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[SearchIn_Production] @searchParam nvarchar(500) as
begin
	set nocount on;
	select p.Id, Task_Name as 'Name',
	(select Task_Name from dbo.GetParent(p.Id)) as RootTask,
	(select dbo.GetSubParent(p.Id)) as RootSubTask 
	from Production_Plan p inner join In_Production a
	on a.Production_Task_Id = p.Id
	where contains(Number, @searchParam)
end
GO


