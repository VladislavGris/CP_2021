USE [CompanyPlannerDB1]
GO

/****** Object:  Table [dbo].[LaborCosts]    Script Date: 25.08.2021 16:26:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LaborCosts](
	[Id] [uniqueidentifier] NOT NULL,
	[Project] [nvarchar](max) NULL,
	[Subcont] [nvarchar](max) NULL,
	[MarkingRank] [nvarchar](max) NULL,
	[MarkingHours] [real] NOT NULL,
	[AssemblyRank] [nvarchar](max) NULL,
	[AssemblyHours] [real] NOT NULL,
	[SettingRank] [nvarchar](max) NULL,
	[SettingHours] [real] NOT NULL,
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


