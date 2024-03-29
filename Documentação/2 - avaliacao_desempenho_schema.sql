USE [avaliacao_desempenho]
GO
/****** Object:  Table [dbo].[AssociacaoCargoCompetencia]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssociacaoCargoCompetencia](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CargoRubiID] [int] NOT NULL,
	[CargoRubi] [nvarchar](200) NOT NULL,
	[AreaRubiID] [int] NOT NULL,
	[AreaRubi] [nvarchar](200) NOT NULL,
	[SetorRubiID] [int] NULL,
	[SetorRubi] [nvarchar](200) NULL,
	[CargoCompetenciaID] [int] NULL,
	[CargoCompetencia] [nvarchar](200) NULL,
	[AreaCompetenciaID] [int] NULL,
	[AreaCompetencia] [nvarchar](200) NULL,
	[SetorCompetenciaID] [int] NULL,
	[SetorCompetencia] [nvarchar](200) NULL,
	[CicloAvaliacao_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AssociacaoCargoCompetencia] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AvaliacaoColaborador]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AvaliacaoColaborador](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DataCriacao] [datetime] NOT NULL,
	[JustificativaReprovacao] [nvarchar](max) NULL,
	[Aprovada] [bit] NOT NULL,
	[CicloAvaliacao_ID] [int] NOT NULL,
	[Colaborador_ID] [int] NOT NULL,
	[GestorRubi_ID] [int] NULL,
	[GestorRubiEmp_ID] [int] NULL,
	[StatusAvaliacaoColaborador_ID] [int] NOT NULL,
	[CargoRubiID] [int] NULL,
	[AreaRubiID] [int] NULL,
	[SetorRubiID] [int] NULL,
	[DataUltimaAlteracao] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.AvaliacaoColaborador] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AvaliacaoGestor]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AvaliacaoGestor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Avaliacao] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.AvaliacaoGestor] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AvaliacaoPDIColaborador]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AvaliacaoPDIColaborador](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DataCriacao] [date] NOT NULL,
	[Idiomas] [nvarchar](max) NULL,
	[Graduacao] [nvarchar](max) NULL,
	[PontosFortes] [nvarchar](max) NULL,
	[PontosDesenvolvimento] [nvarchar](max) NULL,
	[ComentariosColaborador] [nvarchar](max) NULL,
	[ComentariosGestor] [nvarchar](max) NULL,
	[Aprovada] [bit] NOT NULL,
	[CicloAvaliacao_ID] [int] NOT NULL,
	[StatusPDI_ID] [int] NOT NULL,
	[Colaborador_ID] [int] NOT NULL,
	[GestorRubi_ID] [int] NULL,
	[GestorRubiEmp_ID] [int] NULL,
	[CargoRubiID] [int] NULL,
	[AreaRubiID] [int] NULL,
	[SetorRubiID] [int] NULL,
 CONSTRAINT [PK_dbo.AvaliacaoPDIColaborador] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CicloAvaliacao]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CicloAvaliacao](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [nvarchar](200) NOT NULL,
	[DataInicioVigencia] [date] NOT NULL,
	[DataFimVigencia] [date] NOT NULL,
	[URLCompetencia] [nvarchar](200) NULL,
	[OrientacaoCompetencia] [nvarchar](200) NULL,
	[DataInicioObjetivosMetas] [date] NULL,
	[DataTerminoObjetivosMetas] [date] NULL,
	[TituloOrientacaoObjetivosMetas] [nvarchar](200) NULL,
	[TextoOrientacaoObjetivosMetas] [nvarchar](max) NULL,
	[DataInicioAutoAvaliacao] [date] NULL,
	[DataTerminoAutoAvaliacao] [date] NULL,
	[TituloOrientacaoAutoAvaliacao] [nvarchar](200) NULL,
	[TextoOrientacaoAutoAvaliacao] [nvarchar](max) NULL,
	[DataInicioAvaliacaoGestor] [date] NULL,
	[DataTerminoAvaliacaoGestor] [date] NULL,
	[TituloOrientacaoAvaliacaoGestor] [nvarchar](200) NULL,
	[TextoOrientacaoAvaliacaoGestor] [nvarchar](max) NULL,
	[DataInicioGerenciamentoPDI] [date] NULL,
	[DataTerminoGerenciamentoPDI] [date] NULL,
	[SituacaoCicloAvaliacao_ID] [int] NULL,
 CONSTRAINT [PK_dbo.CicloAvaliacao] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompetenciaColaborador]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CompetenciaColaborador](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AvaliacaoColaborador_ID] [int] NOT NULL,
	[CompetenciaID] [int] NOT NULL,
	[NivelColaborador] [int] NULL,
	[NivelRequerido] [int] NULL,
	[NivelGestor] [int] NULL,
	[ComentariosGestor] [varchar](max) NULL,
 CONSTRAINT [PK_CompetenciaColaborador] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ContribuicaoColaborador]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContribuicaoColaborador](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Contribuicao] [nvarchar](max) NOT NULL,
	[AvaliacaoGestor_ID] [int] NULL,
	[AvaliacaoColaborador_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ContribuicaoColaborador] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DesenvolvimentoCompetencia]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DesenvolvimentoCompetencia](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AcaoDesenvolvimento] [nvarchar](max) NULL,
	[RecursoSuporte] [nvarchar](max) NULL,
	[AvaliacaoPDIColaborador_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.DesenvolvimentoCompetencia] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MetaColaborador]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetaColaborador](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Meta] [nvarchar](max) NOT NULL,
	[ResultadoAtingidoColaborador_ID] [int] NULL,
 CONSTRAINT [PK_dbo.MetaColaborador] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ObjetivoColaborador]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjetivoColaborador](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Objetivo] [nvarchar](max) NOT NULL,
	[MetaColaborador_ID] [int] NULL,
	[AvaliacaoColaborador_ID] [int] NULL,
	[AutoAvaliacao] [bit] NOT NULL CONSTRAINT [DF_ObjetivoColaborador_AutoAvaliacao]  DEFAULT ((0)),
 CONSTRAINT [PK_dbo.ObjetivoColaborador] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Papel]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Papel](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_dbo.Papel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ResultadoAtingidoColaborador]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResultadoAtingidoColaborador](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ResultadoAtingido] [nvarchar](max) NULL,
	[AvaliacaoGestor_ID] [int] NULL,
 CONSTRAINT [PK_dbo.ResultadoAtingidoColaborador] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SituacaoCicloAvaliacao]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SituacaoCicloAvaliacao](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_dbo.SituacaoCicloAvaliacao] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StatusAvaliacaoColaborador]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusAvaliacaoColaborador](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_dbo.StatusAvaliacaoColaborador] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StatusPDI]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusPDI](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_dbo.StatusPDI] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Papel_ID] [int] NOT NULL,
	[GestorRubiID] [int] NULL,
	[UsuarioRubiID] [int] NOT NULL,
	[CodigoEmpresaRubiUD] [int] NOT NULL CONSTRAINT [DF_Usuario_CodigoEmpresaRubiUD]  DEFAULT ((1)),
	[Login] [varchar](200) NOT NULL CONSTRAINT [DF_Usuario_Login]  DEFAULT (''),
 CONSTRAINT [PK_dbo.Usuario] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PerformanceColaborador](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AvaliacaoPerformance] [nvarchar](max) NULL,
	[AvaliacaoColaborador_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.PerformanceColaborador] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RecomendacaoColaborador](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AvaliacaoColaborador_ID] [int] NOT NULL,
	[RecomendacaoDeRating] [int] NULL,
	[RecomendacaoDePromocao] [bit] NULL,
	[Justificativa] [nvarchar](max) NULL,
	[JustificativaDaJustificativa] [nvarchar](max) NULL,
	[RatingFinalPosCalibragem] [int] NULL,
	[JustificativaRatingFinalPosCalibragem] [nvarchar](max) NULL,
	[IndicacaoPromocaoPosCalibragem] [bit] NULL,
	[JustificativaIndicacaoPromocaoPosCalibragem] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.RecomendacaoColaborador] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[SituacaoCicloAvaliacao]    Script Date: 08/09/2014 22:50:00 ******/
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[AssociacaoCargoCompetencia]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AssociacaoCargoCompetencia_dbo.CicloAvaliacao_CicloAvaliacao_ID] FOREIGN KEY([CicloAvaliacao_ID])
REFERENCES [dbo].[CicloAvaliacao] ([ID])
GO
ALTER TABLE [dbo].[AssociacaoCargoCompetencia] CHECK CONSTRAINT [FK_dbo.AssociacaoCargoCompetencia_dbo.CicloAvaliacao_CicloAvaliacao_ID]
GO
ALTER TABLE [dbo].[AvaliacaoColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AvaliacaoColaborador_dbo.CicloAvaliacao_CicloAvaliacao_ID] FOREIGN KEY([CicloAvaliacao_ID])
REFERENCES [dbo].[CicloAvaliacao] ([ID])
GO
ALTER TABLE [dbo].[AvaliacaoColaborador] CHECK CONSTRAINT [FK_dbo.AvaliacaoColaborador_dbo.CicloAvaliacao_CicloAvaliacao_ID]
GO
ALTER TABLE [dbo].[AvaliacaoColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AvaliacaoColaborador_dbo.StatusAvaliacaoColaborador_StatusAvaliacaoColaborador_ID] FOREIGN KEY([StatusAvaliacaoColaborador_ID])
REFERENCES [dbo].[StatusAvaliacaoColaborador] ([ID])
GO
ALTER TABLE [dbo].[AvaliacaoColaborador] CHECK CONSTRAINT [FK_dbo.AvaliacaoColaborador_dbo.StatusAvaliacaoColaborador_StatusAvaliacaoColaborador_ID]
GO
ALTER TABLE [dbo].[AvaliacaoColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AvaliacaoColaborador_dbo.Usuario_Colaborador_ID] FOREIGN KEY([Colaborador_ID])
REFERENCES [dbo].[Usuario] ([ID])
GO
ALTER TABLE [dbo].[AvaliacaoColaborador] CHECK CONSTRAINT [FK_dbo.AvaliacaoColaborador_dbo.Usuario_Colaborador_ID]
GO
ALTER TABLE [dbo].[AvaliacaoPDIColaborador]  WITH CHECK ADD  CONSTRAINT [FK_AvaliacaoPDIColaborador_CicloAvaliacao] FOREIGN KEY([CicloAvaliacao_ID])
REFERENCES [dbo].[CicloAvaliacao] ([ID])
GO
ALTER TABLE [dbo].[AvaliacaoPDIColaborador] CHECK CONSTRAINT [FK_AvaliacaoPDIColaborador_CicloAvaliacao]
GO
ALTER TABLE [dbo].[AvaliacaoPDIColaborador]  WITH CHECK ADD  CONSTRAINT [FK_AvaliacaoPDIColaborador_Usuario] FOREIGN KEY([Colaborador_ID])
REFERENCES [dbo].[Usuario] ([ID])
GO
ALTER TABLE [dbo].[AvaliacaoPDIColaborador] CHECK CONSTRAINT [FK_AvaliacaoPDIColaborador_Usuario]
GO
ALTER TABLE [dbo].[AvaliacaoPDIColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AvaliacaoPDIColaborador_dbo.StatusPDI_StatusPDI_ID] FOREIGN KEY([StatusPDI_ID])
REFERENCES [dbo].[StatusPDI] ([ID])
GO
ALTER TABLE [dbo].[AvaliacaoPDIColaborador] CHECK CONSTRAINT [FK_dbo.AvaliacaoPDIColaborador_dbo.StatusPDI_StatusPDI_ID]
GO
ALTER TABLE [dbo].[CicloAvaliacao]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CicloAvaliacao_dbo.SituacaoCicloAvaliacao_SituacaoCicloAvaliacao_ID] FOREIGN KEY([SituacaoCicloAvaliacao_ID])
REFERENCES [dbo].[SituacaoCicloAvaliacao] ([ID])
GO
ALTER TABLE [dbo].[CicloAvaliacao] CHECK CONSTRAINT [FK_dbo.CicloAvaliacao_dbo.SituacaoCicloAvaliacao_SituacaoCicloAvaliacao_ID]
GO
ALTER TABLE [dbo].[CompetenciaColaborador]  WITH CHECK ADD  CONSTRAINT [FK_CompetenciaColaborador_AvaliacaoColaborador] FOREIGN KEY([AvaliacaoColaborador_ID])
REFERENCES [dbo].[AvaliacaoColaborador] ([ID])
GO
ALTER TABLE [dbo].[CompetenciaColaborador] CHECK CONSTRAINT [FK_CompetenciaColaborador_AvaliacaoColaborador]
GO
ALTER TABLE [dbo].[ContribuicaoColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ContribuicaoColaborador_dbo.AvaliacaoColaborador_AvaliacaoColaborador_ID] FOREIGN KEY([AvaliacaoColaborador_ID])
REFERENCES [dbo].[AvaliacaoColaborador] ([ID])
GO
ALTER TABLE [dbo].[ContribuicaoColaborador] CHECK CONSTRAINT [FK_dbo.ContribuicaoColaborador_dbo.AvaliacaoColaborador_AvaliacaoColaborador_ID]
GO
ALTER TABLE [dbo].[ContribuicaoColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ContribuicaoColaborador_dbo.AvaliacaoGestor_AvaliacaoGestor_ID] FOREIGN KEY([AvaliacaoGestor_ID])
REFERENCES [dbo].[AvaliacaoGestor] ([ID])
GO
ALTER TABLE [dbo].[ContribuicaoColaborador] CHECK CONSTRAINT [FK_dbo.ContribuicaoColaborador_dbo.AvaliacaoGestor_AvaliacaoGestor_ID]
GO
ALTER TABLE [dbo].[DesenvolvimentoCompetencia]  WITH CHECK ADD  CONSTRAINT [FK_dbo.DesenvolvimentoCompetencia_dbo.AvaliacaoPDIColaborador_AvaliacaoPDIColaborador_ID] FOREIGN KEY([AvaliacaoPDIColaborador_ID])
REFERENCES [dbo].[AvaliacaoPDIColaborador] ([ID])
GO
ALTER TABLE [dbo].[DesenvolvimentoCompetencia] CHECK CONSTRAINT [FK_dbo.DesenvolvimentoCompetencia_dbo.AvaliacaoPDIColaborador_AvaliacaoPDIColaborador_ID]
GO
ALTER TABLE [dbo].[MetaColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.MetaColaborador_dbo.ResultadoAtingidoColaborador_ResultadoAtingidoColaborador_ID] FOREIGN KEY([ResultadoAtingidoColaborador_ID])
REFERENCES [dbo].[ResultadoAtingidoColaborador] ([ID])
GO
ALTER TABLE [dbo].[MetaColaborador] CHECK CONSTRAINT [FK_dbo.MetaColaborador_dbo.ResultadoAtingidoColaborador_ResultadoAtingidoColaborador_ID]
GO
ALTER TABLE [dbo].[ObjetivoColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ObjetivoColaborador_dbo.AvaliacaoColaborador_AvaliacaoColaborador_ID] FOREIGN KEY([AvaliacaoColaborador_ID])
REFERENCES [dbo].[AvaliacaoColaborador] ([ID])
GO
ALTER TABLE [dbo].[ObjetivoColaborador] CHECK CONSTRAINT [FK_dbo.ObjetivoColaborador_dbo.AvaliacaoColaborador_AvaliacaoColaborador_ID]
GO
ALTER TABLE [dbo].[ObjetivoColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ObjetivoColaborador_dbo.MetaColaborador_MetaColaborador_ID] FOREIGN KEY([MetaColaborador_ID])
REFERENCES [dbo].[MetaColaborador] ([ID])
GO
ALTER TABLE [dbo].[ObjetivoColaborador] CHECK CONSTRAINT [FK_dbo.ObjetivoColaborador_dbo.MetaColaborador_MetaColaborador_ID]
GO
ALTER TABLE [dbo].[ResultadoAtingidoColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ResultadoAtingidoColaborador_dbo.AvaliacaoGestor_AvaliacaoGestor_ID] FOREIGN KEY([AvaliacaoGestor_ID])
REFERENCES [dbo].[AvaliacaoGestor] ([ID])
GO
ALTER TABLE [dbo].[ResultadoAtingidoColaborador] CHECK CONSTRAINT [FK_dbo.ResultadoAtingidoColaborador_dbo.AvaliacaoGestor_AvaliacaoGestor_ID]
GO
ALTER TABLE [dbo].[PerformanceColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PerformanceColaborador_dbo.AvaliacaoColaborador_AvaliacaoColaborador_ID] FOREIGN KEY([AvaliacaoColaborador_ID])
REFERENCES [dbo].[AvaliacaoColaborador] ([ID])
GO
ALTER TABLE [dbo].[PerformanceColaborador] CHECK CONSTRAINT [FK_dbo.PerformanceColaborador_dbo.AvaliacaoColaborador_AvaliacaoColaborador_ID]
GO
ALTER TABLE [dbo].[RecomendacaoColaborador]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RecomendacaoColaborador_dbo.AvaliacaoColaborador_AvaliacaoColaborador_ID] FOREIGN KEY([AvaliacaoColaborador_ID])
REFERENCES [dbo].[AvaliacaoColaborador] ([ID])
GO
ALTER TABLE [dbo].[RecomendacaoColaborador] CHECK CONSTRAINT [FK_dbo.RecomendacaoColaborador_dbo.AvaliacaoColaborador_AvaliacaoColaborador_ID]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Papel] FOREIGN KEY([Papel_ID])
REFERENCES [dbo].[Papel] ([ID])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Papel]
GO

CREATE TRIGGER [dbo].[TrUpdateStatusAvaliacao]
ON [dbo].[CicloAvaliacao]
AFTER UPDATE
AS

DECLARE @STATUS_OLD AS NUMERIC
DECLARE @STATUS_NEW AS NUMERIC
DECLARE @CICLO AS NUMERIC

BEGIN
	SET @STATUS_OLD = (SELECT SituacaoCicloAvaliacao_ID FROM DELETED)
	SET @STATUS_NEW = (SELECT SituacaoCicloAvaliacao_ID  FROM INSERTED)
	SET @CICLO = (SELECT ID FROM INSERTED)

	if @STATUS_OLD != @STATUS_NEW 
		if @STATUS_NEW = 1 or @STATUS_NEW = 2
			UPDATE [dbo].[AvaliacaoColaborador]
			SET    StatusAvaliacaoColaborador_ID = @STATUS_NEW
			WHERE  CicloAvaliacao_ID = @CICLO
		else
			if @STATUS_NEW = 3
				UPDATE [dbo].[AvaliacaoColaborador]
				SET    StatusAvaliacaoColaborador_ID = 4
				WHERE  CicloAvaliacao_ID = @CICLO
			else
				if @STATUS_NEW = 4
					UPDATE [dbo].[AvaliacaoColaborador]
					SET    StatusAvaliacaoColaborador_ID = 5
					WHERE  CicloAvaliacao_ID = @CICLO
				else
					if @STATUS_NEW = 5
						UPDATE [dbo].[AvaliacaoColaborador]
						SET    StatusAvaliacaoColaborador_ID = 6
						WHERE  CicloAvaliacao_ID = @CICLO
					else
						if @STATUS_NEW = 6
							UPDATE [dbo].[AvaliacaoColaborador]
							SET    StatusAvaliacaoColaborador_ID = 7
							WHERE  CicloAvaliacao_ID = @CICLO
						else
							if @STATUS_NEW = 7
							BEGIN
								UPDATE [dbo].[AvaliacaoColaborador]
								SET    StatusAvaliacaoColaborador_ID = 8
								WHERE  CicloAvaliacao_ID = @CICLO

								UPDATE [dbo].[AvaliacaoPDIColaborador]
								SET    StatusPDI_ID = 1
								WHERE  CicloAvaliacao_ID = @CICLO
							END
END
GO
