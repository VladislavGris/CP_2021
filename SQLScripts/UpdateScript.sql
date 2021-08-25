use CompanyPlannerDB
go
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Formatting](
	[Id] [uniqueidentifier] default newid() NOT NULL,
	[IsBold] [bit] NOT NULL,
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
	[SpecificationSum] [decimal](18, 2) NULL,
	[Project] [nvarchar](max) NULL,
	[PriceWithoutVAT] [decimal](18, 2) NULL,
	[Note] [nvarchar](max) NULL,
	[IsFirstPayment] [bit] NOT NULL,
	[FirstPaymentSum] [decimal](18, 2) NULL,
	[FirstPaymentDate] [date] NULL,
	[IsSecondPayment] [bit] NOT NULL,
	[SecondPaymentSum] [decimal](18, 2) NULL,
	[SecondPaymentDate] [date] NULL,
	[IsFullPayment] [bit] NOT NULL,
	[FullPaymentSum] [decimal](18, 2) NULL,
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