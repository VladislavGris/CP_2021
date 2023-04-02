USE [UsersTest1]
GO

/****** Object:  StoredProcedure [dbo].[SearchProductionPlan]    Script Date: 02.04.2023 20:58:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[SearchProductionPlan] @searchParam nvarchar(500) as
begin
	set nocount on;
	select Id, Task_Name as 'Name', 
	(select Task_Name from dbo.GetParent(Id)) as RootTask,
	(select dbo.GetSubParent(Id)) as RootSubTask 
	from Production_Plan
	where Task_Name like @searchParam or Inc_Doc like @searchParam or Manag_Doc like @searchParam or Expend_Num like @searchParam 
end
GO