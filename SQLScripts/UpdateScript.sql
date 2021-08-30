use MainCompanyPlannerDB
go
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Formatting](
	[Id] [uniqueidentifier] default newid() NOT NULL,
	[IsBold] [bit] NOT NULL,
	[IsItalic] [bit] not null default(0),
	[IsUnderline] [bit] not null default(0),
	[FontFamily] [nvarchar](max) default('Arial'),
	[FontSize] [int] default(14),
	[ProductionTaskId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Formatting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Formatting] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsBold]
GO

ALTER TABLE [dbo].[Formatting]  WITH CHECK ADD  CONSTRAINT [FK_Formatting_Production_Plan_ProductionTaskId] FOREIGN KEY([ProductionTaskId])
REFERENCES [dbo].[Production_Plan] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Formatting] CHECK CONSTRAINT [FK_Formatting_Production_Plan_ProductionTaskId]
GO

CREATE TABLE [dbo].[LaborCosts](
	[Id] [uniqueidentifier] default newid() NOT NULL,
	[Project] [nvarchar](max) NULL,
	[Subcont] [nvarchar](max) NULL,
	[MarkingRank] [nvarchar](max) NULL,
	[MarkingHours] [real] NULL,
	[AssemblyRank] [nvarchar](max) NULL,
	[AssemblyHours] [real] NULL,
	[SettingRank] [nvarchar](max) NULL,
	[SettingHours] [real] NULL,
	[Date] [date] NULL,
	[TotalTime]  AS (([MarkingHours]+[AssemblyHours])+[SettingHours]),
	[ProductionTaskId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_LaborCosts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[LaborCosts]  WITH CHECK ADD  CONSTRAINT [FK_LaborCosts_Production_Plan_ProductionTaskId] FOREIGN KEY([ProductionTaskId])
REFERENCES [dbo].[Production_Plan] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LaborCosts] CHECK CONSTRAINT [FK_LaborCosts_Production_Plan_ProductionTaskId]
GO

CREATE TABLE [dbo].[Payment](
	[Id] [uniqueidentifier] default newid() NOT NULL,
	[Contract] [nvarchar](max) NULL,
	[SpecificationSum] [nvarchar](max) NULL,
	[Project] [nvarchar](max) NULL,
	[PriceWithoutVAT] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[IsFirstPayment] [bit] NOT NULL,
	[FirstPaymentSum] [nvarchar](max) NULL,
	[FirstPaymentDate] [date] NULL,
	[IsSecondPayment] [bit] NOT NULL,
	[SecondPaymentSum] [nvarchar](max) NULL,
	[SecondPaymentDate] [date] NULL,
	[IsFullPayment] [bit] NOT NULL,
	[FullPaymentSum] [nvarchar](max) NULL,
	[FullPaymentDate] [date] NULL,
	[ProductionTaskId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Payment] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsFirstPayment]
GO

ALTER TABLE [dbo].[Payment] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsSecondPayment]
GO

ALTER TABLE [dbo].[Payment] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsFullPayment]
GO

ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_Production_Plan_ProductionTaskId] FOREIGN KEY([ProductionTaskId])
REFERENCES [dbo].[Production_Plan] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_Production_Plan_ProductionTaskId]
GO
SET NOCOUNT ON
declare ParentTasks cursor
for select Id 
from Production_Plan ;
declare @Id uniqueidentifier;
open ParentTasks
fetch ParentTasks into @Id;
while @@FETCH_STATUS=0
begin
	print @Id
	insert into Formatting(ProductionTaskId) values (@Id)
	insert into Payment(ProductionTaskId) values (@Id)
	insert into LaborCosts(ProductionTaskId) values (@Id)
	fetch ParentTasks into @Id;
end
close ParentTasks

deallocate ParentTasks
go
alter table Complectation
add Rack nvarchar(max) null, Shelf nvarchar(max) null, Note nvarchar(max) null, OnStorageDate date null, StateNumber nvarchar(max)
alter table Giving
add PurchaseGoods nvarchar(max) null, Note nvarchar(max) null
alter table Manufacture
add ExecutionAct bit not null default(0), ExecutionTerm date null, CalendarDays nvarchar(max), WorkingDays nvarchar(max), Note nvarchar(max)
alter table In_Production
add Note nvarchar(max) null
alter table Production_Plan
add EditingBy nvarchar(max) default('default') not null