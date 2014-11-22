USE [avaliacao_desempenho]
GO

/****************************************************************************************************
* SituacaoCicloAvaliacao
*****************************************************************************************************/

INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Criada')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Em defini��o de objetivos e metas')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Objetivos e metas definidos')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Em realiza��o das auto-avalia��es')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Em avalia��o pelo gestor')
GO
INSERT INTO [dbo].[SituacaoCicloAvaliacao] ([Nome]) VALUES ('Em calibragem de rating e recomenda��o')
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

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Defini��o de objetivos e metas')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Aprova��o dos objetivos e metas definidos')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Objetivos e metas definidos')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Realiza��o de auto-avalia��o')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Em avalia��o pelo gestor')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Em avalia��o pelo RH')
GO

INSERT INTO [dbo].[StatusAvaliacaoColaborador] ([Nome]) VALUES ('Encerrada')
GO

/****************************************************************************************************
* StatusPDI
*****************************************************************************************************/

INSERT INTO [dbo].[StatusPDI] ([Nome]) VALUES ('Criada')
GO

INSERT INTO [dbo].[StatusPDI] ([Nome]) VALUES ('Submetida � aprova��o do gestor')
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