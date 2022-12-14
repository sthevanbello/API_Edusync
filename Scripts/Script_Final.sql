
USE MASTER;
GO

--DROP DATABASE MAIS_EVENTOS;
--GO

CREATE DATABASE MAIS_EVENTOS;
GO

USE [MAIS_EVENTOS]
GO
/****** Object:  Table [dbo].[RL_USUARIO_EVENTO]    Script Date: 8/8/2022 8:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RL_USUARIO_EVENTO](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [int] NULL,
	[EventoId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_CATEGORIAS]    Script Date: 8/8/2022 8:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CATEGORIAS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NomeCategoria] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_EVENTOS]    Script Date: 8/8/2022 8:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_EVENTOS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataHora] [datetime] NULL,
	[Ativo] [bit] NULL,
	[Preco] [decimal](6, 2) NULL,
	[CategoriaId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_USUARIOS]    Script Date: 8/8/2022 8:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_USUARIOS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Senha] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[RL_USUARIO_EVENTO] ON 

INSERT [dbo].[RL_USUARIO_EVENTO] ([Id], [UsuarioId], [EventoId]) VALUES (5, 1, 1)
INSERT [dbo].[RL_USUARIO_EVENTO] ([Id], [UsuarioId], [EventoId]) VALUES (6, 2, 1)
INSERT [dbo].[RL_USUARIO_EVENTO] ([Id], [UsuarioId], [EventoId]) VALUES (7, NULL, 1)
INSERT [dbo].[RL_USUARIO_EVENTO] ([Id], [UsuarioId], [EventoId]) VALUES (8, 1, NULL)
SET IDENTITY_INSERT [dbo].[RL_USUARIO_EVENTO] OFF
GO
SET IDENTITY_INSERT [dbo].[TB_CATEGORIAS] ON 

INSERT [dbo].[TB_CATEGORIAS] ([Id], [NomeCategoria]) VALUES (1, N'Show')
INSERT [dbo].[TB_CATEGORIAS] ([Id], [NomeCategoria]) VALUES (2, N'Teatro')
INSERT [dbo].[TB_CATEGORIAS] ([Id], [NomeCategoria]) VALUES (3, N'Formatura')
INSERT [dbo].[TB_CATEGORIAS] ([Id], [NomeCategoria]) VALUES (4, N'Musical')
INSERT [dbo].[TB_CATEGORIAS] ([Id], [NomeCategoria]) VALUES (5, N'Aniversario')
INSERT [dbo].[TB_CATEGORIAS] ([Id], [NomeCategoria]) VALUES (6, N'Meetup')
INSERT [dbo].[TB_CATEGORIAS] ([Id], [NomeCategoria]) VALUES (7, N'Stand-up')
SET IDENTITY_INSERT [dbo].[TB_CATEGORIAS] OFF
GO
SET IDENTITY_INSERT [dbo].[TB_EVENTOS] ON 

INSERT [dbo].[TB_EVENTOS] ([Id], [DataHora], [Ativo], [Preco], [CategoriaId]) VALUES (1, CAST(N'2022-08-08T22:30:00.000' AS DateTime), 1, CAST(649.99 AS Decimal(6, 2)), 1)
INSERT [dbo].[TB_EVENTOS] ([Id], [DataHora], [Ativo], [Preco], [CategoriaId]) VALUES (2, CAST(N'2022-10-08T22:00:00.000' AS DateTime), 1, CAST(249.99 AS Decimal(6, 2)), 2)
INSERT [dbo].[TB_EVENTOS] ([Id], [DataHora], [Ativo], [Preco], [CategoriaId]) VALUES (3, CAST(N'2022-11-08T21:30:00.000' AS DateTime), 1, CAST(149.99 AS Decimal(6, 2)), 3)
SET IDENTITY_INSERT [dbo].[TB_EVENTOS] OFF
GO
SET IDENTITY_INSERT [dbo].[TB_USUARIOS] ON 

INSERT [dbo].[TB_USUARIOS] ([Id], [Nome], [Email], [Senha]) VALUES (1, N'Homer', N'homer@simpson.com', N'duff')
INSERT [dbo].[TB_USUARIOS] ([Id], [Nome], [Email], [Senha]) VALUES (2, N'Marge', N'marge@simpson.com', N'hair')
INSERT [dbo].[TB_USUARIOS] ([Id], [Nome], [Email], [Senha]) VALUES (3, N'Bart', N'bart@simpson.com', N'skate')
SET IDENTITY_INSERT [dbo].[TB_USUARIOS] OFF
GO
ALTER TABLE [dbo].[RL_USUARIO_EVENTO]  WITH CHECK ADD FOREIGN KEY([EventoId])
REFERENCES [dbo].[TB_EVENTOS] ([Id])
GO
ALTER TABLE [dbo].[RL_USUARIO_EVENTO]  WITH CHECK ADD FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[TB_USUARIOS] ([Id])
GO
ALTER TABLE [dbo].[TB_EVENTOS]  WITH CHECK ADD FOREIGN KEY([CategoriaId])
REFERENCES [dbo].[TB_CATEGORIAS] ([Id])
GO
