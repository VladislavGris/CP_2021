use CompanyPlannerDB12
go
-- ���������� "������� �� ����"
--alter table Production_Plan
--add ActCreation bit default(0)
--go
--update Production_Plan
--set  ActCreation = 0
--where ActCreation is null
go
--alter table Production_Plan
--add EditingBy nvarchar(max) default('default')
--go
--update Production_Plan
--set EditingBy='default'
--where EditingBy is null
--go
-- ��������� ������� �� ������������ ����� � �����
--update Production_Plan
--set Completion = 9
--where Completion = 5
go
-- �������� ������� ��������� �������� ������� � ��������
create function GetParent(@ChildId uniqueidentifier) returns table
as
return WITH root
As
(
	select Id, ParentId, ChildId, 1 as Position from HierarchyDB where ChildId=@ChildId
	union all
	select ic.Id, ic.ParentId, ic.ChildId, Position + 1 from HierarchyDB ic inner join
	root rt on ic.ChildId = rt.ParentId
)
select top 1 Task_Name from root inner join Production_Plan p1 on p1.Id = root.ChildId order by Position desc
go
-- �������� ������������� ���� �������� �������� ������
create view HeadTasks as
select Task_Name as Task
from Production_Plan inner join HierarchyDB on Production_Plan.Id = HierarchyDB.ChildId
where ParentId is null
go
-- �������� ������������� ���� ���� �������������
create view ManufactureNames as
select M_Name as Name
from Manufacture
where M_Name is not null
group by M_Name
go
-- ������������� ���������� ������������
create view [dbo].[NoSpecifications] as
select p1.Id as Id,
		p1.Task_Name as Task,
		Manufacture.M_Name as Manufacturer,
		Manufacture.Letter_Num as LetterNum,
		Manufacture.Specification_Num as SpecNum,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project
from Production_Plan p1 inner join Manufacture on p1.Id = Manufacture.Production_Task_Id
inner join HierarchyDB h1 on p1.Id = h1.ChildId
where not (Manufacture.M_Name is null or Manufacture.M_Name='') and
	not (Manufacture.Letter_Num is null or Manufacture.Letter_Num='') and
	(Manufacture.Specification_Num is null or Manufacture.Specification_Num='')
go
-- ������������� ������������ �� ��������
create view SpecificationsOnControl as
select p1.Id as Id,
		p1.Task_Name as Task,
		p1.Manag_Doc as ManagDoc,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		p1.P_Count as Count,
		Manufacture.M_Name as Manufacturer,
		Manufacture.Letter_Num as LetterNum,
		Manufacture.Specification_Num as SpecNum,
		Manufacture.OnControl as OnControl,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Manufacture on p1.Id = Manufacture.Production_Task_Id
where Manufacture.OnControl = 1
go
-- ������������� ������������ � �������
create view SpecificationsInVipisk as
select p1.Id as Id,
		p1.Task_Name as Task,
		p1.Manag_Doc as ManagDoc,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		p1.P_Count as Count,
		Manufacture.M_Name as Manufacturer,
		Manufacture.Specification_Num as SpecNum,
		Manufacture.VipiskSpec as VipiskSpec,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Manufacture on p1.Id = Manufacture.Production_Task_Id
where Manufacture.VipiskSpec = 1
go
-- ������������� � ������ �� ����������
create view CoopWork as
select p1.Id as Id,
		p1.Task_Name as Task,
		p1.Manag_Doc as ManagDoc,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		p1.P_Count as Count,
		Manufacture.M_Name as Manufacturer,
		Manufacture.Specification_Num as SpecNum,
		Manufacture.Letter_Num as LetterNum,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Manufacture on p1.Id = Manufacture.Production_Task_Id
where p1.Completion = 3
go
-- ������������� ������� ������� � ������
create view InProgress as
select p1.Id as Id,
		p1.Task_Name as Task,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		p1.P_Count as Count,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		c1.Complectation as Complectation,
		i1.Number as MSLNumber,
		i1.Giving_Date as GivingDate,
		i1.Executor_Name as Executor1,
		i1.Install_Executor_Name as Executor2,
		i1.Projected_Date as ProjectedDate
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Complectation c1 on p1.Id = c1.Production_Task_Id
inner join In_Production i1 on p1.Id = i1.Production_Task_Id
where p1.Completion = 4
go
-- ������������� ��������� ������������
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
-- ������������� �������� ���
create view SKBCheck as
select p1.Id as Id,
		p1.Task_Name as Task,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		p1.Manag_Doc as ManagDoc,
		p1.P_Count as Count,
		c1.Complectation as Complectation,
		p1.Note as Note
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Complectation c1 on p1.Id = c1.Production_Task_Id
where p1.Completion = 6
go
-- ������������� ����� ���
create view OETSStorage as
select p1.Id as Id,
		p1.Task_Name as Task,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		p1.Manag_Doc as ManagDoc,
		p1.P_Count as Count,
		c1.Complectation as Complectation,
		c1.Rack as Rack,
		c1.Shelf as Shelf,
		p1.Note as Note
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Complectation c1 on p1.Id = c1.Production_Task_Id
where p1.Completion = 7
go
-- ������������� ������� �� ������ ����������� ������������� ��� ��������
create view GivingStorage as
select p1.Id as Id,
		p1.Task_Name as Task,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		p1.P_Count as Count,
		c1.Complectation as Complectation,
		g1.Receiving_Date as ReceivingDate
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Complectation c1 on p1.Id = c1.Production_Task_Id
inner join Giving g1 on p1.Id = g1.Production_Task_Id
inner join Manufacture m1 on p1.Id = m1.Production_Task_Id
where g1.G_State = 1 and g1.Receiving_Date is not null and (m1.Letter_Num is null or m1.Letter_Num = '')
go
-- ������������� � �������������� ������������ �������
create view GivingReports as
select p1.Id as Id,
		p1.Task_Name as Task,
		(select p2.Task_Name from Production_Plan p2 where p2.Id=h1.ParentId) as ParentTask,
		(select p2.Id from Production_Plan p2 where p2.Id=h1.ParentId) as ParentId,
		(select Task_Name from dbo.GetParent(p1.Id)) as Project,
		p1.Manag_Doc as ManagDoc,
		p1.P_Count as Count,
		m1.M_Name as Manufacturer,
		m1.Specification_Num as SpecNum,
		p1.Inc_Doc as IncDoc,
		g1.Report as Report
from Production_Plan p1 inner join HierarchyDB h1 on p1.Id = h1.ChildId
inner join Complectation c1 on p1.Id = c1.Production_Task_Id
inner join Giving g1 on p1.Id = g1.Production_Task_Id
inner join Manufacture m1 on p1.Id = m1.Production_Task_Id
where g1.G_State = 1 and p1.Inc_Doc is not null and (g1.Report is null or g1.Report = '')
go
-- ������� ��� ������������
create table Act(
[Id] [uniqueidentifier] default newid() NOT NULL,
ActNumber nvarchar(max) null,
ActDate date null,
ActCreation bit default(0) not null,
ByAct bit default(0) not null,
Note nvarchar(max) null,
[ProductionTaskId] [uniqueidentifier] NOT NULL,
CONSTRAINT [PK_Act] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].Act  WITH CHECK ADD  CONSTRAINT [FK_Act_Production_Plan_ProductionTaskId] FOREIGN KEY([ProductionTaskId])
REFERENCES [dbo].[Production_Plan] ([Id])
ON DELETE CASCADE
GO
declare ParentTasks cursor
for select Id 
from Production_Plan ;
declare @Id uniqueidentifier;
open ParentTasks
fetch ParentTasks into @Id;
while @@FETCH_STATUS=0
begin
	print @Id
	insert into Act(ProductionTaskId) values (@Id)
	fetch ParentTasks into @Id;
end
close ParentTasks

deallocate ParentTasks
go