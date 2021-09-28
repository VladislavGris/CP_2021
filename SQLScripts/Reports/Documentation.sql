use CompanyPlannerDB1
go
create view Documentation as
select p1.Id as Id,
		p1.Task_Name as Task,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		p1.Manag_Doc as ManagDoc


from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
where p1.Completion = 1
go