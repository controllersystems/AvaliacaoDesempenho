USE [master]
GO

/****** Object:  Database [avaliacao_desempenho]    Script Date: 19/08/2014 16:26:08 ******/

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = 'avaliacao_desempenho'
OR name = 'avaliacao_desempenho')))
DROP DATABASE [avaliacao_desempenho]
GO

/****** Object:  Database [avaliacao_desempenho]    Script Date: 19/08/2014 16:26:08 ******/
CREATE DATABASE [avaliacao_desempenho]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'avaliacao_desempenho', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\avaliacao_desempenho.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'avaliacao_desempenho_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\avaliacao_desempenho_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [avaliacao_desempenho] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [avaliacao_desempenho].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [avaliacao_desempenho] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET ARITHABORT OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [avaliacao_desempenho] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [avaliacao_desempenho] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET  DISABLE_BROKER 
GO

ALTER DATABASE [avaliacao_desempenho] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [avaliacao_desempenho] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET RECOVERY FULL 
GO

ALTER DATABASE [avaliacao_desempenho] SET  MULTI_USER 
GO

ALTER DATABASE [avaliacao_desempenho] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [avaliacao_desempenho] SET DB_CHAINING OFF 
GO

ALTER DATABASE [avaliacao_desempenho] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [avaliacao_desempenho] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [avaliacao_desempenho] SET  READ_WRITE 
GO


