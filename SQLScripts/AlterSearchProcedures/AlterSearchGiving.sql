USE [UsersTest1]
GO

/****** Object:  StoredProcedure [dbo].[SearchGiving]    Script Date: 02.04.2023 21:15:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[SearchGiving] @searchParam nvarchar(500) as
begin
	set nocount on;
	select p.Id, Task_Name as 'Name',
	(select Task_Name from dbo.GetParent(p.Id)) as RootTask,
	(select dbo.GetSubParent(p.Id)) as RootSubTask 
	from Production_Plan p inner join Giving a
	on a.Production_Task_Id = p.Id
	where contains(Bill, @searchParam) or contains(Report, @searchParam) or contains(Return_Report, @searchParam) or contains(PurchaseGoods, @searchParam)
end
GO


