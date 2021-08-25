USE [CompanyPlannerDB1]
GO

/****** Object:  Table [dbo].[Formatting]    Script Date: 25.08.2021 15:50:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Formatting](
	[Id] [uniqueidentifier] NOT NULL,
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


