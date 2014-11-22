USE [avaliacao_desempenho]
GO

/****************************************************************************************************
* SituacaoCicloAvaliacao
*****************************************************************************************************/

INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Criada')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Em definição de objetivos e metas')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Objetivos e metas definidos')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Em realização das auto-avaliações')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Em avaliação pelo gestor')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Em calibragem de rating e recomendação')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('PDI')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Encerrada')
GO

/****************************************************************************************************
* StatusAvaliacaoColaborador
*****************************************************************************************************/

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Criada')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Definição de objetivos e metas')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Aprovação dos objetivos e metas definidos')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Objetivos e metas definidos')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Realização de auto-avaliação')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Em avaliação pelo gestor')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Em avaliação pelo RH')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Encerrada')
GO

/****************************************************************************************************
* StatusPDI
*****************************************************************************************************/

INSERT INTO [dbo].[StatusPDI] ([Nome]) VALUES ('Criada')
GO

INSERT INTO [dbo].[StatusPDI] ([Nome]) VALUES ('Submetida à aprovação do gestor')
GO

INSERT INTO [dbo].[StatusPDI] ([Nome]) VALUES ('Aprovada')
GO


/****************************************************************************************************
* Papel
*****************************************************************************************************/

INSERT INTO [dbo].[Papel] ([Nome]) VALUES ('Administrador')
GO

INSERT INTO [dbo].[Papel] ([Nome]) VALUES ('Colaborador')
GO