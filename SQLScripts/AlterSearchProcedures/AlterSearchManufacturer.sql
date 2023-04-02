USE [UsersTest1]
GO

/****** Object:  StoredProcedure [dbo].[SearchManufacture]    Script Date: 02.04.2023 21:48:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[SearchManufacture] @searchParam nvarchar(500) as
begin
	set nocount on;
	select p.Id, Task_Name as 'Name',
	(select Task_Name from dbo.GetParent(p.Id)) as RootTask,
	(select dbo.GetSubParent(p.Id)) as RootSubTask 
	from Production_Plan p inner join Manufacture a
	on a.Production_Task_Id = p.Id
	where contains(M_Name, @searchParam) or contains(Letter_Num, @searchParam) or contains(Specification_Num, @searchParam)
end
GO


