--
-- PAR 05/04/2021
--
-- Conversion des scripts dispo sur : http://www.sqlines.com/online
--

--
--Script SQL Server
--
CREATE TABLE [dbo].[Erreur](
	[idErreur] [int] IDENTITY(1,1) NOT NULL,
	[DateErreur] [datetime] NULL,
	[FunctionCalled] [nvarchar](max) NULL,
	[ParametersCalled] [nvarchar](max) NULL,
	[StackTrace] [nvarchar](max) NULL,
	[Message] [nvarchar](max) NULL,
	[LoginUser] [nvarchar](max) NULL,
 CONSTRAINT [PK_Erreur] PRIMARY KEY CLUSTERED 
(
	[idErreur] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

