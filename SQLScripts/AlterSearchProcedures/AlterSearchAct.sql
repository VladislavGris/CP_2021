USE [UsersTest1]
GO

/****** Object:  StoredProcedure [dbo].[SearchAct]    Script Date: 02.04.2023 21:11:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[SearchAct] @searchParam nvarchar(500) as
begin
	set nocount on;
	select p.Id, Task_Name as 'Name',
	(select Task_Name from dbo.GetParent(p.Id)) as RootTask,
	(select dbo.GetSubParent(p.Id)) as RootSubTask 
	from Production_Plan p inner join Act a
	on a.ProductionTaskId = p.Id
	where contains(ActNumber, @searchParam)
end
GO


