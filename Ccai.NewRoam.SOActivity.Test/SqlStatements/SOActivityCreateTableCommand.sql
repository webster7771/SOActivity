/****** Object:  Table [dbo].[SOActivity]    Script Date: 5/29/2014 6:31:52 PM ******/
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;

CREATE TABLE [dbo].[SOActivity](
	[Id] [nvarchar](60) NOT NULL,
	[ActivityType] [varchar](60) NOT NULL,
	[Date] [date] NOT NULL,
	[SONumber] [varchar](60) NOT NULL,
	[CheckInCheckOutState] [varchar](60) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
SET ANSI_PADDING OFF;