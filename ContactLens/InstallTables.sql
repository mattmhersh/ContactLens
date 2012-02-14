USE [C:\OMATE32\OM_SQLData\OMATESQL.mdf]
GO
/****** Object:  Table [dbo].[Replenishment]    Script Date: 02/10/2010 20:05:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Replenishment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Rx_ID] [int] NOT NULL,
	[Notes] [nvarchar](255) NULL,
	[Category] [nvarchar](255) NULL,
	[Automail] [nvarchar](255) NULL,
	[DoNotSend] [int] NOT NULL,
	[InitialDispensingDate] [datetime] NULL,
 CONSTRAINT [PK_Replenishment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

USE [C:\OMATE32\OM_SQLData\OMATESQL.mdf]
GO
/****** Object:  Table [dbo].[ReplenishmentSchedule]    Script Date: 02/10/2010 20:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReplenishmentSchedule](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Rx_ID] [int] NOT NULL,
	[ReplenishmentDate] [datetime] NULL,
	[ComplianceDate] [datetime] NULL,
	[Delqt] [int] NULL,
 CONSTRAINT [PK_ReplenishmentSchedule] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

