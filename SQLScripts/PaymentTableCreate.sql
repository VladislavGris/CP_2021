USE [CompanyPlannerDB1]
GO

/****** Object:  Table [dbo].[Payment]    Script Date: 25.08.2021 16:19:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Payment](
	[Id] [uniqueidentifier] NOT NULL,
	[Contract] [nvarchar](max) NULL,
	[SpecificationSum] [decimal](18, 2) NOT NULL,
	[Project] [nvarchar](max) NULL,
	[PriceWithoutVAT] [decimal](18, 2) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[IsFirstPayment] [bit] NOT NULL,
	[FirstPaymentSum] [decimal](18, 2) NOT NULL,
	[FirstPaymentDate] [date] NULL,
	[IsSecondPayment] [bit] NOT NULL,
	[SecondPaymentSum] [decimal](18, 2) NOT NULL,
	[SecondPaymentDate] [date] NULL,
	[IsFullPayment] [bit] NOT NULL,
	[FullPaymentSum] [decimal](18, 2) NOT NULL,
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


