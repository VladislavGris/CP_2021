use CompanyPlannerDB1
go
declare @childId uniqueidentifier
set @childId = 'A37517DD-E5F5-40D4-0E6C-08D971E24239'
;WITH root
As
(
	select Id, ParentId, ChildId, 1 as Position from HierarchyDB where ChildId=@childId
	union all
	select ic.Id, ic.ParentId, ic.ChildId, Position + 1 from HierarchyDB ic inner join
	root rt on ic.ChildId = rt.ParentId
)

select top 1 Task_Name from root inner join Production_Plan p1 on p1.Id = root.ChildId order by Position desc