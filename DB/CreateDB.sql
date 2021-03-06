USE [master]
GO
/****** Object:  Database [AvoidMiss_New]    Script Date: 2020/09/30 9:44:18 ******/
CREATE DATABASE [AvoidMiss_New] ON  PRIMARY 
( NAME = N'AvoidMiss_New', FILENAME = N'D:\SQLDATA\AvoidMiss_New\AvoidMiss_New.mdf' , SIZE = 12080192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AvoidMiss_New_log', FILENAME = N'D:\SQLDATA\AvoidMiss_New\AvoidMiss_New_log.ldf' , SIZE = 24822976KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [AvoidMiss_New] SET COMPATIBILITY_LEVEL = 90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AvoidMiss_New].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [AvoidMiss_New] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET ARITHABORT OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [AvoidMiss_New] SET AUTO_SHRINK ON 
GO
ALTER DATABASE [AvoidMiss_New] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AvoidMiss_New] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AvoidMiss_New] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET  ENABLE_BROKER 
GO
ALTER DATABASE [AvoidMiss_New] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AvoidMiss_New] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AvoidMiss_New] SET RECOVERY FULL 
GO
ALTER DATABASE [AvoidMiss_New] SET  MULTI_USER 
GO
ALTER DATABASE [AvoidMiss_New] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AvoidMiss_New] SET DB_CHAINING OFF 
GO
USE [AvoidMiss_New]
GO
/****** Object:  User [ds]    Script Date: 2020/09/30 9:44:18 ******/
CREATE USER [ds] FOR LOGIN [ds] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [avo]    Script Date: 2020/09/30 9:44:18 ******/
CREATE USER [avo] FOR LOGIN [avo] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  DatabaseRole [dsread]    Script Date: 2020/09/30 9:44:18 ******/
CREATE ROLE [dsread]
GO
ALTER ROLE [dsread] ADD MEMBER [ds]
GO
/****** Object:  StoredProcedure [dbo].[p_completedata_heng_delete]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  create proc [dbo].[p_completedata_heng_delete]

  as 

  set nocount on 
   update [AvoidMiss_New].[dbo].[TB_CompleteData]
  set [Code]=replace(code,'-','')
  where Finish_Date>=dateadd(day,-1,getdate())
GO
/****** Object:  Table [dbo].[bk_TB_CompleteData]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bk_TB_CompleteData](
	[ID] [numeric](10, 0) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[MakeNumber] [nvarchar](50) NOT NULL,
	[Product_Line] [nvarchar](50) NOT NULL,
	[ProductionQuantity] [numeric](18, 0) NOT NULL,
	[Finish_Date] [datetime] NULL,
	[Pay_Date] [datetime] NULL,
	[Direction] [nvarchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[m_check]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_check](
	[id] [char](9) NOT NULL,
	[goods_id] [char](7) NOT NULL,
	[kind_cd] [char](3) NOT NULL,
	[tools_id] [char](7) NULL,
	[tools_order] [char](3) NULL,
	[classify_id] [char](8) NOT NULL,
	[classify_order] [char](3) NULL,
	[type_cd] [char](3) NOT NULL,
	[department_cd] [char](3) NOT NULL,
	[kind] [varchar](100) NULL,
	[check_position] [varchar](100) NULL,
	[check_item] [nvarchar](500) NULL,
	[benchmark_type] [varchar](4) NULL,
	[benchmark_value1] [varchar](20) NULL,
	[benchmark_value2] [varchar](20) NULL,
	[benchmark_value3] [varchar](20) NULL,
	[check_times] [varchar](2) NULL,
	[check_way] [varchar](50) NULL,
	[delete_flg] [char](1) NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_m_check] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_check_ms_temp]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_check_ms_temp](
	[id] [char](9) NOT NULL,
	[goods_id] [char](7) NOT NULL,
	[kind_cd] [char](3) NOT NULL,
	[tools_id] [char](7) NULL,
	[tools_order] [char](3) NULL,
	[classify_id] [char](8) NOT NULL,
	[classify_order] [char](3) NULL,
	[type_cd] [char](3) NOT NULL,
	[department_cd] [char](3) NOT NULL,
	[kind] [varchar](100) NULL,
	[check_position] [varchar](100) NULL,
	[check_item] [nvarchar](500) NULL,
	[benchmark_type] [varchar](4) NULL,
	[benchmark_value1] [varchar](20) NULL,
	[benchmark_value2] [varchar](20) NULL,
	[benchmark_value3] [varchar](20) NULL,
	[check_way] [varchar](50) NULL,
	[check_times] [varchar](2) NULL,
	[delete_flg] [char](1) NULL,
	[kind_name] [varchar](50) NULL,
	[type_name] [varchar](50) NULL,
	[department_name] [varchar](50) NULL,
	[classify_name] [varchar](100) NULL,
	[picture_id] [char](7) NULL,
	[goods_cd] [varchar](30) NULL,
	[goods_name] [nvarchar](100) NULL,
	[tools_no] [varchar](30) NULL,
	[picture_nm] [nvarchar](400) NULL,
	[picture_name] [varchar](200) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_check_temp]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_check_temp](
	[id] [char](9) NOT NULL,
	[goods_id] [char](7) NOT NULL,
	[kind_cd] [char](3) NOT NULL,
	[tools_id] [char](7) NULL,
	[tools_order] [char](3) NULL,
	[classify_id] [char](8) NOT NULL,
	[classify_order] [char](3) NULL,
	[type_cd] [char](3) NOT NULL,
	[department_cd] [char](3) NOT NULL,
	[kind] [varchar](100) NULL,
	[check_position] [varchar](100) NULL,
	[check_item] [nvarchar](500) NULL,
	[benchmark_type] [char](2) NULL,
	[benchmark_value1] [varchar](20) NULL,
	[benchmark_value2] [varchar](20) NULL,
	[benchmark_value3] [varchar](20) NULL,
	[check_times] [varchar](2) NULL,
	[check_way] [varchar](50) NULL,
	[delete_flg] [char](1) NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_classify]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_classify](
	[id] [varchar](8) NOT NULL,
	[goods_id] [varchar](7) NOT NULL,
	[kind_cd] [varchar](3) NOT NULL,
	[department_cd] [char](3) NOT NULL,
	[tools_id] [varchar](7) NULL,
	[classify_name] [varchar](100) NULL,
	[picture_id] [varchar](8) NOT NULL,
	[disp_no] [varchar](3) NULL,
	[delete_flg] [char](1) NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_classify] PRIMARY KEY CLUSTERED 
(
	[goods_id] ASC,
	[kind_cd] ASC,
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_goods]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_goods](
	[id] [char](7) NOT NULL,
	[goods_cd] [varchar](30) NOT NULL,
	[goods_name] [nvarchar](100) NULL,
	[delete_flg] [char](1) NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_m_goods_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_kbn]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_kbn](
	[mei_kbn] [char](4) NOT NULL,
	[mei_cd] [char](3) NOT NULL,
	[mei] [varchar](50) NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL,
	[delete_flg] [char](1) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_kind]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_kind](
	[goods_id] [varchar](7) NOT NULL,
	[kind_cd] [varchar](3) NOT NULL,
	[disp_no] [varchar](3) NULL,
	[delete_flg] [char](1) NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_m_kind] PRIMARY KEY CLUSTERED 
(
	[goods_id] ASC,
	[kind_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_permission]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_permission](
	[user_id] [numeric](18, 0) NOT NULL,
	[access_type] [char](1) NOT NULL,
	[access_cd] [char](3) NOT NULL,
	[delete_flg] [char](1) NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_m_permission] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC,
	[access_type] ASC,
	[access_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_picture]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_picture](
	[id] [char](7) NOT NULL,
	[department_cd] [char](3) NOT NULL,
	[picture_nm] [nvarchar](400) NULL,
	[picture_name] [varchar](200) NULL,
	[picture_style] [char](1) NULL,
	[picture_content] [varbinary](max) NULL,
	[delete_flg] [char](1) NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_m_picture] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_tools]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_tools](
	[id] [char](7) NOT NULL,
	[tools_no] [varchar](30) NOT NULL,
	[department_cd] [char](3) NOT NULL,
	[distinguish] [varchar](100) NULL,
	[barcode_flg] [char](1) NULL,
	[barcode] [varchar](30) NULL,
	[remarks] [varchar](200) NULL,
	[delete_flg] [char](1) NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_m_tools_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_type]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_type](
	[id] [char](8) NOT NULL,
	[goods_id] [char](7) NOT NULL,
	[kind_cd] [char](3) NOT NULL,
	[tools_no] [varchar](100) NOT NULL,
	[classify_id] [char](8) NOT NULL,
	[type_name] [varchar](100) NULL,
	[disp_no] [varchar](3) NULL,
	[delete_flg] [char](1) NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL,
 CONSTRAINT [PK_m_type] PRIMARY KEY CLUSTERED 
(
	[goods_id] ASC,
	[kind_cd] ASC,
	[classify_id] ASC,
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[sysdiagrams]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sysdiagrams](
	[name] [nvarchar](128) NOT NULL,
	[principal_id] [int] NOT NULL,
	[diagram_id] [int] NOT NULL,
	[version] [int] NULL,
	[definition] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[t_check_result]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[t_check_result](
	[id] [varchar](13) NOT NULL,
	[goods_id] [varchar](7) NOT NULL,
	[make_number] [varchar](50) NOT NULL,
	[department_cd] [char](3) NOT NULL,
	[check_user] [varchar](30) NULL,
	[start_time] [datetime] NULL,
	[end_time] [datetime] NULL,
	[up_check_user] [varchar](30) NULL,
	[up_start_time] [datetime] NULL,
	[up_end_time] [datetime] NULL,
	[result] [char](2) NULL,
	[remarks] [varchar](200) NULL,
	[check_times] [char](1) NULL,
	[shareResult_id] [varchar](13) NULL,
	[continue_chk_flg] [char](1) NULL,
	[delete_flg] [char](1) NULL,
	[qianpin_flg] [char](1) NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL,
	[Product_Line_check] [nvarchar](50) NULL,
	[ProductionQuantity_check] [numeric](18, 0) NULL,
	[Finish_Date_check] [datetime] NULL,
 CONSTRAINT [PK_t_check_result] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[t_check_result_bk]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[t_check_result_bk](
	[id] [varchar](13) NOT NULL,
	[goods_id] [varchar](7) NOT NULL,
	[make_number] [varchar](50) NOT NULL,
	[department_cd] [char](3) NOT NULL,
	[check_user] [varchar](30) NULL,
	[start_time] [datetime] NULL,
	[end_time] [datetime] NULL,
	[up_check_user] [varchar](30) NULL,
	[up_start_time] [datetime] NULL,
	[up_end_time] [datetime] NULL,
	[result] [char](2) NULL,
	[remarks] [varchar](200) NULL,
	[check_times] [char](1) NULL,
	[shareResult_id] [varchar](13) NULL,
	[continue_chk_flg] [char](1) NULL,
	[delete_flg] [char](1) NULL,
	[qianpin_flg] [char](1) NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL,
	[update_user] [varchar](30) NULL,
	[update_date] [datetime] NULL,
	[Product_Line_check] [nvarchar](50) NULL,
	[ProductionQuantity_check] [numeric](18, 0) NULL,
	[Finish_Date_check] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[t_first_check]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[t_first_check](
	[good_cd] [varchar](50) NOT NULL,
	[tongyong_cd] [varchar](50) NOT NULL,
	[checked_flg] [char](1) NULL,
 CONSTRAINT [PK_t_first_check] PRIMARY KEY CLUSTERED 
(
	[good_cd] ASC,
	[tongyong_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[t_index]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[t_index](
	[type_kbn] [char](2) NOT NULL,
	[index_date] [varchar](8) NULL,
	[no] [varchar](8) NOT NULL,
	[max_no] [varchar](8) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[t_log]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[t_log](
	[log_cd] [varchar](15) NOT NULL,
	[operate_tb] [varchar](20) NULL,
	[operate_kind] [varchar](3) NULL,
	[operator_cd] [varchar](30) NULL,
	[operate_objcd] [varchar](200) NULL,
	[operate_date] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[t_result_detail]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[t_result_detail](
	[result_id] [varchar](13) NOT NULL,
	[check_id] [varchar](9) NOT NULL,
	[measure_value1] [varchar](100) NULL,
	[measure_value2] [varchar](100) NULL,
	[picture_id] [char](7) NULL,
	[result] [char](2) NULL,
	[dis_no] [varchar](2) NOT NULL,
	[remarks] [varchar](200) NULL,
	[delete_flg] [char](1) NULL,
	[benchmark_type] [varchar](4) NULL,
	[benchmark_value1] [varchar](20) NULL,
	[benchmark_value2] [varchar](20) NULL,
	[benchmark_value3] [varchar](20) NULL,
	[tools_scan_flg] [varchar](1) NULL,
	[classify_id] [varchar](8) NULL,
	[check_times] [char](1) NOT NULL,
 CONSTRAINT [PK_t_result_detail] PRIMARY KEY CLUSTERED 
(
	[result_id] ASC,
	[check_id] ASC,
	[dis_no] ASC,
	[check_times] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[t_result_detail_bak]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[t_result_detail_bak](
	[result_id] [varchar](13) NOT NULL,
	[check_id] [varchar](9) NOT NULL,
	[measure_value1] [varchar](100) NULL,
	[measure_value2] [varchar](100) NULL,
	[picture_id] [char](7) NULL,
	[result] [char](2) NULL,
	[dis_no] [varchar](2) NOT NULL,
	[remarks] [varchar](200) NULL,
	[delete_flg] [char](1) NULL,
	[benchmark_type] [varchar](4) NULL,
	[benchmark_value1] [varchar](20) NULL,
	[benchmark_value2] [varchar](20) NULL,
	[benchmark_value3] [varchar](20) NULL,
	[tools_scan_flg] [varchar](1) NULL,
	[classify_id] [varchar](8) NULL,
	[check_times] [char](1) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[t_result_detail_temp]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[t_result_detail_temp](
	[result_id] [varchar](13) NOT NULL,
	[check_id] [varchar](9) NOT NULL,
	[measure_value1] [varchar](100) NULL,
	[measure_value2] [varchar](100) NULL,
	[picture_id] [char](7) NULL,
	[result] [char](2) NULL,
	[dis_no] [varchar](2) NOT NULL,
	[remarks] [varchar](200) NULL,
	[delete_flg] [char](1) NULL,
	[benchmark_type] [char](2) NULL,
	[benchmark_value1] [varchar](20) NULL,
	[benchmark_value2] [varchar](20) NULL,
	[benchmark_value3] [varchar](20) NULL,
	[tools_scan_flg] [varchar](1) NULL,
	[classify_id] [varchar](8) NULL,
	[check_times] [char](1) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[t_sousa_rireki]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[t_sousa_rireki](
	[dousa] [varchar](100) NOT NULL,
	[result_id] [varchar](13) NOT NULL,
	[goods_id] [varchar](7) NOT NULL,
	[goods_cd] [varchar](30) NOT NULL,
	[make_number] [varchar](50) NOT NULL,
	[mark] [varchar](500) NOT NULL,
	[insert_user] [varchar](30) NULL,
	[insert_date] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_Afru]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Afru](
	[Afrurueck] [numeric](10, 0) NULL,
	[Afrurmzhl] [numeric](8, 0) NULL,
	[DeleteNumber] [numeric](18, 0) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CanSet]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CanSet](
	[ID] [numeric](18, 0) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[FNumber] [nvarchar](50) NULL,
 CONSTRAINT [PK_TB_CanSet] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CheckDetail]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_CheckDetail](
	[ID] [numeric](18, 0) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[MakeNumber] [nvarchar](50) NULL,
	[Product_Line] [nvarchar](50) NULL,
	[ProductionQuantity] [numeric](18, 0) NULL,
	[Finish_Date] [datetime] NULL,
	[Pay_Date] [datetime] NULL,
	[Direction] [nvarchar](50) NULL,
	[ScanDate] [datetime] NULL,
	[PrintFlg] [int] NULL,
	[CheckUser] [nvarchar](50) NULL,
	[ONOK] [nvarchar](50) NULL,
	[ONNG] [nvarchar](50) NULL,
	[Result] [varchar](4) NULL,
	[CompleteFlg] [int] NULL,
	[CompleteDate] [datetime] NULL,
	[Lack] [int] NULL,
	[InstructionBookState] [varchar](2) NULL,
	[CabinetPartsState] [varchar](2) NULL,
	[DYState] [varchar](2) NULL,
 CONSTRAINT [PK_TB_CheckDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_Codes]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Codes](
	[ID] [decimal](18, 0) NOT NULL,
	[CodeName] [nvarchar](50) NULL,
	[CodeKb] [nvarchar](50) NULL,
	[ObjectName] [nvarchar](50) NULL,
	[ObjectAddress] [nvarchar](50) NULL,
	[ObjTop] [decimal](18, 0) NULL,
	[ObjHeight] [decimal](18, 0) NULL,
	[ObjLeft] [decimal](18, 0) NULL,
	[ObjWidth] [decimal](18, 0) NULL,
	[ObjState] [int] NULL,
 CONSTRAINT [PK_TB_Codes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CompleteData]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CompleteData](
	[ID] [numeric](10, 0) IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[MakeNumber] [nvarchar](50) NOT NULL,
	[Product_Line] [nvarchar](50) NOT NULL,
	[ProductionQuantity] [numeric](18, 0) NOT NULL,
	[Finish_Date] [datetime] NULL,
	[Pay_Date] [datetime] NULL,
	[Direction] [nvarchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_DingChi_Log]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_DingChi_Log](
	[ID] [numeric](18, 0) NOT NULL,
	[fileName] [nvarchar](50) NOT NULL,
	[code] [nvarchar](50) NOT NULL,
	[makeNumber] [nvarchar](50) NOT NULL,
	[ProductionQuantity] [int] NOT NULL,
	[CompleteDate] [datetime] NOT NULL,
	[Result] [varchar](50) NULL,
	[jiachayuanName] [varchar](50) NULL,
	[qufen] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_NewCreation]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_NewCreation](
	[ID] [numeric](18, 0) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[FNumber] [nvarchar](20) NULL,
 CONSTRAINT [PK_TB_NewCreation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_NO]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_NO](
	[ID] [numeric](18, 0) NOT NULL,
	[Number] [numeric](18, 0) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_Picture]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_Picture](
	[ID] [numeric](18, 0) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[PType] [int] NOT NULL,
	[Patch] [varchar](50) NOT NULL,
	[CodeCS] [nvarchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TB_ProductionPlan]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_ProductionPlan](
	[ID] [int] NOT NULL,
	[AppDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TB_ProductionPlan] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_SetAllCheck]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_SetAllCheck](
	[ID] [numeric](18, 0) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[FNumber] [nvarchar](20) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_TypeMS]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_TypeMS](
	[ID] [numeric](18, 0) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[LabelAmount] [int] NOT NULL,
	[FileIndex] [nvarchar](50) NOT NULL,
	[GoodsName] [nvarchar](50) NULL,
	[CodeFile] [nvarchar](50) NULL,
 CONSTRAINT [PK_TB_TypeMS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_User]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_User](
	[ID] [numeric](18, 0) NOT NULL,
	[LoginName] [nvarchar](30) NULL,
	[LoginPassword] [nvarchar](30) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[UserCode] [nvarchar](50) NOT NULL,
	[UserType] [int] NOT NULL,
 CONSTRAINT [PK_TB_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[v_check_km_ms]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--drop VIEW [dbo].[v_check_km_ms]

CREATE VIEW [dbo].[v_check_km_ms]
AS
SELECT dbo.m_check.id,
       dbo.m_goods.goods_cd,
       dbo.m_goods.goods_name,
       m_department.mei AS department_name,
       m_kind.mei       AS kind_name,
       dbo.m_picture.picture_nm,
       dbo.m_classify.classify_name,
       dbo.m_check.classify_order,
       m_type.mei       AS type_name,
       dbo.m_picture.id AS picture_id,
       dbo.m_tools.tools_no,
       dbo.m_check.tools_id,
       dbo.m_check.goods_id,
       dbo.m_check.kind_cd,
       dbo.m_check.tools_order,
       dbo.m_check.kind,
       dbo.m_check.check_position,
       dbo.m_check.check_item,
       dbo.m_check.benchmark_type,
       dbo.m_check.benchmark_value1,
       dbo.m_check.benchmark_value2,
	   case when ltrim(isnull(dbo.m_check.benchmark_value3,'')) = '' then ''
	   else
       abs(dbo.m_check.benchmark_value3)+''
	   end as benchmark_value3,
       dbo.m_check.check_way,
       dbo.m_check.check_times,
       dbo.m_picture.picture_name
FROM   dbo.m_check WITH (READCOMMITTED)
       LEFT OUTER JOIN dbo.m_classify WITH (READCOMMITTED)
                    ON dbo.m_check.classify_id = dbo.m_classify.id
                       AND dbo.m_classify.delete_flg = '0'
       LEFT OUTER JOIN dbo.m_goods WITH (READCOMMITTED)
                    ON dbo.m_check.goods_id = dbo.m_goods.id
                       AND dbo.m_goods.delete_flg = '0'
       LEFT OUTER JOIN dbo.m_tools WITH (READCOMMITTED)
                    ON dbo.m_check.tools_id = dbo.m_tools.id
                       AND dbo.m_tools.delete_flg = '0'
       LEFT OUTER JOIN dbo.m_kbn AS m_kind WITH (READCOMMITTED)
                    ON dbo.m_check.kind_cd = m_kind.mei_cd
                       AND m_kind.mei_kbn = '0001'
                       AND m_kind.delete_flg = '0'
       LEFT OUTER JOIN dbo.m_kbn AS m_type WITH (READCOMMITTED)
                    ON dbo.m_check.type_cd = m_type.mei_cd
                       AND m_type.mei_kbn = '0002'
                       AND m_type.delete_flg = '0'
       LEFT OUTER JOIN dbo.m_kbn AS m_department WITH (READCOMMITTED)
                    ON dbo.m_check.department_cd = m_department.mei_cd
                       AND m_department.mei_kbn = '0004'
                       AND m_department.delete_flg = '0'
       LEFT OUTER JOIN dbo.m_picture WITH (READCOMMITTED)
                    ON dbo.m_picture.id = dbo.m_classify.picture_id
                       AND dbo.m_picture.delete_flg = '0'
WHERE  ( 1 = 1 )
       AND ( dbo.m_check.department_cd IN ( '001', '002', '003' ) )
       AND ( dbo.m_check.delete_flg = '0' )


GO
/****** Object:  View [dbo].[v_check_new_result]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--drop view [v_check_new_result]
CREATE VIEW [dbo].[v_check_new_result] AS
SELECT 
	DISTINCT
	'新'                                      AS sysnm --
	,tb_completedata.finish_date --完了日
	,m_goods.goods_name --商品名
	,m_goods.goods_cd                      AS goods_cd --商品コード
	,tcr.make_number as makenumber --作番
	,tb_completedata.productionquantity --完工数量
	,tb_completedata.product_line --生产线
	,tb_completedata.direction --向先
	,tb_completedata.pay_date --纳期日
	,CASE 
		WHEN BB.[id] IS NULL THEN '未检查' 
		WHEN tcr_kaisuu.cntid < CC.cntid THEN '未全检' 
		WHEN BB.result = 'NG' THEN '不合格未二检' 
		ELSE '' 
	END jieguo --原因
	,CONVERT(VARCHAR(100), tcr.end_time, 23)    AS Check_Time --检查终了时间 
	,CONVERT(VARCHAR(100), tcr.start_time, 120) AS start_time --检查开始时间 
	,CONVERT(VARCHAR(100), tcr.end_time, 120)   AS end_time --检查终了时间 
	,Datediff(ss, tcr.start_time, tcr.end_time) AS Check_use_Time --检查时长 
	,Isnull(tb_user.username, '')               AS UserName --检查员
	,remark.[remarks]
	
	
FROM t_check_result tcr 

LEFT JOIN m_goods 
      ON     m_goods.id = tcr.goods_id 
         
LEFT JOIN   tb_completedata 
      ON tb_completedata.makenumber = tcr.make_number 
         AND tb_completedata.Code = m_goods.goods_cd

LEFT JOIN (SELECT Count([id]) AS cntId, 
                 [goods_id], 
                 [make_number] 
          FROM   t_check_result 
          GROUP  BY [goods_id], 
                    [make_number]) tcr_kaisuu 
                    
      ON     tcr_kaisuu.make_number = tcr.make_number 
         AND tcr_kaisuu.goods_id = m_goods.id 
         
       LEFT JOIN (SELECT nom.* 
                  FROM   t_check_result AS nom 
                 INNER JOIN (SELECT goods_id, 
                                    make_number, 
                                    Max(id) AS id 
                             FROM   t_check_result 
                             GROUP  BY goods_id, make_number) AS maxt 
                 ON maxt.id = nom.id) AS BB 
       ON     BB.goods_id = m_goods.id 
          AND BB.make_number = tcr.make_number 
  
LEFT JOIN (SELECT Count(a.code) AS cntId, 
                 a.code, 
                 a.makenumber 
          FROM   tb_completedata a 
                 INNER JOIN tb_setallcheck b 
                         ON a.code = b.code 
          GROUP  BY a.code, 
                    a.makenumber) CC 
      ON CC.makenumber = tcr.make_number 
         AND CC.code = m_goods.goods_cd 
         
LEFT JOIN tb_user 
      ON tb_user.usercode = tcr.check_user 

LEFT JOIN (
	SELECT [result_id], 
	  [remarks] = stuff
		  ((SELECT   '' + isnull([remarks],'')
			  FROM         [t_result_detail] AS t
			  WHERE     t .[result_id] = [t_result_detail].[result_id] FOR xml path('')), 1, 0, '')
	FROM         [t_result_detail]
	where 
		[remarks] is not null
	and [remarks]<>''
	GROUP BY [result_id]
) remark
ON remark.[result_id] = tcr.[id]
Where tcr.delete_flg='0'
GO
/****** Object:  View [dbo].[v_check_old_result]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_check_old_result]
AS
SELECT                  '旧' AS sysnm, ScanCheck.dbo.TB_CompleteData.Finish_Date, ScanCheck.dbo.TB_TypeMS.GoodsName AS goods_name, 
                                  ScanCheck.dbo.TB_CompleteData.Code AS goods_cd, ScanCheck.dbo.TB_CompleteData.MakeNumber, 
                                  ScanCheck.dbo.TB_CompleteData.ProductionQuantity, ScanCheck.dbo.TB_CompleteData.Product_Line, ScanCheck.dbo.TB_CompleteData.Direction, 
                                  ScanCheck.dbo.TB_CompleteData.Pay_Date, CASE WHEN BB.[ID] IS NULL 
                                  THEN '未检查' WHEN AA.cntId < CC.cntId THEN '未全检' WHEN BB.result = 'NG' THEN '不合格未二检' ELSE '' END AS jieguo, CONVERT(VARCHAR(100), 
                                  tcr.CompleteDate, 23) AS Check_Time, CONVERT(VARCHAR(100), tcr.ScanDate, 120) AS start_time, CONVERT(VARCHAR(100), tcr.CompleteDate, 
                                  120) AS end_time, DATEDIFF(ss, tcr.ScanDate, tcr.CompleteDate) AS Check_use_Time, ISNULL(dbo.TB_User.UserName, N'') AS UserName, 
                                  '' AS remarks
FROM                     ScanCheck.dbo.TB_CompleteData LEFT OUTER JOIN
                                  ScanCheck.dbo.TB_TypeMS ON ScanCheck.dbo.TB_CompleteData.Code = ScanCheck.dbo.TB_TypeMS.Code LEFT OUTER JOIN
                                      (SELECT                  COUNT(ID) AS cntId, Code, MakeNumber
                                            FROM                     ScanCheck.dbo.TB_CheckDetail
                                            GROUP BY          Code, MakeNumber) AS AA ON AA.MakeNumber = ScanCheck.dbo.TB_CompleteData.MakeNumber AND 
                                  AA.Code = ScanCheck.dbo.TB_CompleteData.Code LEFT OUTER JOIN
                                      (SELECT                  b.ID, b.Code, b.MakeNumber, b.Result
                                            FROM                     (SELECT                  MAX(ID) AS maxId, Code, MakeNumber
                                                                                FROM                     ScanCheck.dbo.TB_CheckDetail AS TB_CheckDetail_1
                                                                                GROUP BY          Code, MakeNumber) AS a_1 INNER JOIN
                                                                              ScanCheck.dbo.TB_CheckDetail AS b ON a_1.maxId = b.ID) AS BB ON 
                                  BB.MakeNumber = ScanCheck.dbo.TB_CompleteData.MakeNumber AND BB.Code = ScanCheck.dbo.TB_CompleteData.Code LEFT OUTER JOIN
                                      (SELECT                  COUNT(a.Code) AS cntId, a.Code, a.MakeNumber
                                            FROM                     dbo.TB_CompleteData AS a INNER JOIN
                                                                              dbo.TB_SetAllCheck AS b ON a.Code = b.Code
                                            GROUP BY          a.Code, a.MakeNumber) AS CC ON CC.MakeNumber = ScanCheck.dbo.TB_CompleteData.MakeNumber AND 
                                  CC.Code = ScanCheck.dbo.TB_CompleteData.Code LEFT OUTER JOIN
                                  ScanCheck.dbo.TB_CheckDetail AS tcr ON ScanCheck.dbo.TB_CompleteData.MakeNumber = tcr.MakeNumber AND 
                                  ScanCheck.dbo.TB_CompleteData.Code = tcr.Code LEFT OUTER JOIN
                                  dbo.TB_User ON dbo.TB_User.UserCode = tcr.CheckUser



GO
/****** Object:  View [dbo].[v_check_result]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_check_result]
AS
SELECT                  CASE WHEN tnew.jieguo IS NOT NULL AND told.jieguo IS NOT NULL THEN '新旧' WHEN tnew.jieguo IS NOT NULL AND told.jieguo IS NULL 
                                  THEN '新' WHEN tnew.jieguo IS NULL AND told.jieguo IS NOT NULL THEN '旧' ELSE 'NG' END AS sysnm, ISNULL(tnew.finish_date, told.Finish_Date) 
                                  AS finish_date, ISNULL(tnew.goods_name, told.goods_name) AS GoodsName, ISNULL(tnew.goods_cd, told.goods_cd) AS goods_cd, 
                                  ISNULL(tnew.makenumber, told.MakeNumber) AS makenumber, ISNULL(tnew.productionquantity, told.ProductionQuantity) AS productionquantity, 
                                  ISNULL(tnew.product_line, told.Product_Line) AS product_line, ISNULL(tnew.direction, told.Direction) AS direction, ISNULL(tnew.pay_date, 
                                  told.Pay_Date) AS pay_date, ISNULL(tnew.Check_Time, told.Check_Time) AS Check_Time, ISNULL(tnew.start_time, told.start_time) AS start_time, 
                                  ISNULL(tnew.end_time, told.end_time) AS end_time, ISNULL(tnew.Check_use_Time, told.Check_use_Time) AS Check_use_Time, 
                                  CASE WHEN tnew.jieguo = '' OR
                                  told.jieguo = '' THEN 'OK' ELSE 'NG' END AS jieguo, CASE WHEN tnew.jieguo = '' OR
                                  told.jieguo = '' THEN '' ELSE '新：' + Isnull(tnew.jieguo, '不存在') + '--旧：' + Isnull(told.jieguo, '不存在') END AS 详细, ISNULL(tnew.UserName, 
                                  told.UserName) AS UserName, ISNULL(tnew.remarks, told.remarks) AS remarks
FROM                     dbo.v_check_new_result AS tnew FULL OUTER JOIN
                                  dbo.v_check_old_result AS told ON tnew.goods_cd = told.goods_cd AND tnew.makenumber = told.MakeNumber

GO
/****** Object:  View [dbo].[v_goods_kind]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--DROP VIEW [dbo].[v_goods_kind]

CREATE VIEW [dbo].[v_goods_kind]
AS
SELECT DISTINCT dbo.m_check.kind_cd,
                dbo.m_check.goods_id,
                dbo.m_check.tools_id,
                dbo.m_check.classify_id,
                dbo.m_goods.goods_cd,
                ISNULL(dbo.m_check.tools_order, '') AS tools_order,
                ISNULL(dbo.m_tools.tools_no, '') AS tools_no,
                ISNULL(dbo.m_tools.barcode_flg, '') AS barcode_flg,
                ISNULL(dbo.m_tools.barcode, '') AS barcode,
                ISNULL(dbo.m_tools.remarks, '') AS remarks,
                ISNULL(dbo.m_classify.classify_name, '') AS classify_name,
                ISNULL(dbo.m_classify.picture_id, '') AS picture_id,
                CONVERT(INT, ISNULL(dbo.m_classify.disp_no, '9999')) AS disp_no
FROM dbo.m_check WITH (READCOMMITTED)
INNER JOIN dbo.m_goods WITH (READCOMMITTED) ON dbo.m_goods.id = dbo.m_check.goods_id
LEFT OUTER JOIN dbo.m_tools WITH (READCOMMITTED) ON dbo.m_check.tools_id = dbo.m_tools.id
--AND dbo.m_tools.delete_flg = '0'
INNER JOIN dbo.m_classify WITH (READCOMMITTED) ON dbo.m_check.classify_id = dbo.m_classify.id
AND ISNULL(dbo.m_check.tools_id, '') = ISNULL(dbo.m_classify.tools_id, '')
--AND dbo.m_classify.delete_flg = '0'
WHERE (dbo.m_check.delete_flg = '0')
GO
/****** Object:  View [dbo].[v_goods_kind20161228]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_goods_kind20161228]
AS
SELECT DISTINCT 
                                  dbo.m_check.kind_cd, dbo.m_check.goods_id, dbo.m_check.tools_id, dbo.m_check.classify_id, dbo.m_goods.goods_cd, 
                                  ISNULL(dbo.m_check.tools_order, '') AS tools_order, ISNULL(dbo.m_tools.tools_no, '') AS tools_no, ISNULL(dbo.m_tools.barcode_flg, '') 
                                  AS barcode_flg, ISNULL(dbo.m_tools.barcode, '') AS barcode, ISNULL(dbo.m_tools.remarks, '') AS remarks, ISNULL(dbo.m_classify.classify_name, '') 
                                  AS classify_name, ISNULL(dbo.m_classify.picture_id, '') AS picture_id, CONVERT(INT, ISNULL(dbo.m_classify.disp_no, '9999')) AS disp_no
FROM                     dbo.m_check WITH (READCOMMITTED) INNER JOIN
                                  dbo.m_goods WITH (READCOMMITTED) ON dbo.m_goods.id = dbo.m_check.goods_id LEFT OUTER JOIN
                                  dbo.m_tools WITH (READCOMMITTED) ON dbo.m_check.tools_id = dbo.m_tools.id AND dbo.m_tools.delete_flg = '0' INNER JOIN
                                  dbo.m_classify WITH (READCOMMITTED) ON dbo.m_check.classify_id = dbo.m_classify.id AND ISNULL(dbo.m_check.tools_id, '') 
                                  = ISNULL(dbo.m_classify.tools_id, '') AND dbo.m_classify.delete_flg = '0'
WHERE                   (dbo.m_check.delete_flg = '0')
GO
/****** Object:  View [dbo].[v_goods_kind30160722BK]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_goods_kind30160722BK]
AS
SELECT DISTINCT 
                        dbo.m_check.kind_cd, dbo.m_check.goods_id, dbo.m_check.tools_id, dbo.m_check.classify_id, dbo.m_goods.goods_cd, ISNULL(dbo.m_check.tools_order, '') AS tools_order, 
                        ISNULL(dbo.m_tools.tools_no, '') AS tools_no, ISNULL(dbo.m_tools.barcode_flg, '') AS barcode_flg, ISNULL(dbo.m_tools.barcode, '') AS barcode, ISNULL(dbo.m_tools.remarks, 
                        '') AS remarks, ISNULL(dbo.m_classify.classify_name, '') AS classify_name, ISNULL(dbo.m_classify.picture_id, '') AS picture_id, CONVERT(INT, ISNULL(dbo.m_classify.disp_no, 
                        '9999')) AS disp_no
FROM              dbo.m_check WITH (READCOMMITTED) INNER JOIN
                        dbo.m_goods WITH (READCOMMITTED) ON dbo.m_goods.id = dbo.m_check.goods_id LEFT OUTER JOIN
                        dbo.m_tools WITH (READCOMMITTED) ON dbo.m_check.tools_id = dbo.m_tools.id AND dbo.m_tools.delete_flg = '0' INNER JOIN
                        dbo.m_classify WITH (READCOMMITTED) ON dbo.m_check.classify_id = dbo.m_classify.id AND dbo.m_classify.delete_flg = '0'
WHERE             (dbo.m_check.delete_flg = '0')

GO
/****** Object:  View [dbo].[v_ty_check_list]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_ty_check_list]
AS
SELECT                  '新旧' AS sysnm, ISNULL(tnew.finish_date, told.finish_date) AS finish_date, ISNULL(tnew.GoodsName, told.GoodsName) AS GoodsName, 
                                  ISNULL(tnew.goods_cd, told.goods_cd) AS goods_cd, ISNULL(tnew.makenumber, told.makenumber) AS makenumber, ISNULL(tnew.productionquantity, 
                                  told.productionquantity) AS productionquantity, ISNULL(tnew.product_line, told.product_line) AS product_line, ISNULL(tnew.direction, told.direction) 
                                  AS direction, ISNULL(tnew.pay_date, told.pay_date) AS pay_date, CASE WHEN tnew.jieguo = '' OR
                                  told.jieguo = '' THEN 'OK' ELSE 'NG' END AS jieguo, CASE WHEN tnew.jieguo = '' OR
                                  told.jieguo = '' THEN '' ELSE '新：' + tnew.jieguo + '--旧：' + told.jieguo END AS 详细
FROM                     (SELECT                  sysnm, finish_date, GoodsName, goods_cd, makenumber, productionquantity, product_line, direction, pay_date, jieguo
                                    FROM                     dbo.v_ty_miss_check_list
                                    WHERE                   (sysnm = '新')) AS tnew LEFT OUTER JOIN
                                      (SELECT                  sysnm, finish_date, GoodsName, goods_cd, makenumber, productionquantity, product_line, direction, pay_date, jieguo
                                            FROM                     dbo.v_ty_miss_check_list AS v_ty_miss_check_list_1
                                            WHERE                   (sysnm = '旧')) AS told ON tnew.goods_cd = told.goods_cd AND tnew.makenumber = told.makenumber

GO
/****** Object:  View [dbo].[v_ty_miss_check_list]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--drop view [v_ty_miss_check_list]
CREATE VIEW [dbo].[v_ty_miss_check_list]
AS
SELECT '旧'                AS sysnm,
       tb_completedata.finish_date,
       TB_TypeMS.GoodsName,
       tb_completedata.code AS goods_cd --商品コード  
       ,
       tb_completedata.makenumber--作番  
       ,
       tb_completedata.productionquantity--完工数量  
       ,
       tb_completedata.product_line--生产线  
       ,
       tb_completedata.direction--向先  
       ,
       tb_completedata.pay_date--纳期日 
       ,
       CASE
         WHEN BB.[ID] IS NULL THEN '未检查'
         WHEN AA.cntId < CC.cntId THEN '未全检'
         WHEN BB.result = 'NG' THEN '不合格未二检'
         ELSE ''
       END                  jieguo
--原因  
FROM   [ScanCheck].[dbo].tb_completedata
       LEFT JOIN [ScanCheck].[dbo].TB_TypeMS
              ON tb_completedata.code = TB_TypeMS.Code
       LEFT JOIN (SELECT Count([ID]) AS cntId,
                         [Code],
                         [MakeNumber]
                  FROM   [ScanCheck].[dbo].[TB_CheckDetail]
                  GROUP  BY [Code],
                            [MakeNumber]) AA
              ON AA.makenumber = tb_completedata.makenumber
                 AND AA.code = tb_completedata.code
       LEFT JOIN (SELECT b.[ID],
                         b.[Code],
                         b.[MakeNumber],
                         b.result
                  FROM   (SELECT Max([ID]) AS maxId,
                                 [Code],
                                 [MakeNumber]
                          FROM   [ScanCheck].[dbo].[TB_CheckDetail]
                          GROUP  BY [Code],
                                    [MakeNumber]) a
                         INNER JOIN [ScanCheck].[dbo].[TB_CheckDetail] b
                                 ON a.maxId = b.[ID]) BB
              ON BB.makenumber = tb_completedata.makenumber
                 AND BB.code = tb_completedata.code
       LEFT JOIN (SELECT Count(a.code) AS cntId,
                         a.code,
                         a.makenumber
                  FROM   tb_completedata a
                         INNER JOIN tb_setallcheck b
                                 ON a.code = b.code
                  GROUP  BY a.code,
                            a.makenumber) CC
              ON CC.makenumber = tb_completedata.makenumber
                 AND CC.code = tb_completedata.code
UNION
SELECT DISTINCT '新'                AS sysnm,
                tb_completedata.finish_date,
                m_goods.goods_name,--商品名  
                tb_completedata.code AS goods_cd --商品コード  
                ,
                tb_completedata.makenumber--作番  
                ,
                tb_completedata.productionquantity--完工数量  
                ,
                tb_completedata.product_line--生产线  
                ,
                tb_completedata.direction--向先  
                ,
                tb_completedata.pay_date--纳期日 
                ,
                CASE
                  WHEN BB.[ID] IS NULL THEN '未检查'
                  WHEN AA.cntId < CC.cntId THEN '未全检'
                  WHEN BB.result = 'NG' THEN '不合格未二检'
                  ELSE ''
                END                  jieguo
--原因  
FROM   tb_completedata
       LEFT JOIN m_goods
              ON m_goods.goods_cd = tb_completedata.code
                 AND m_goods.delete_flg = '0'
       LEFT JOIN (SELECT Count([id]) AS cntId,
                         [goods_id],
                         [make_number]
                  FROM   t_check_result
                  GROUP  BY [goods_id],
                            [make_number]) AA
              ON AA.make_number = tb_completedata.makenumber
                 AND AA.goods_id = m_goods.id
       LEFT JOIN (SELECT nom.*
                  FROM   t_check_result AS nom
                         INNER JOIN (SELECT goods_id,
                                            make_number,
                                            Max(id) AS id
                                     FROM   t_check_result
                                     GROUP  BY goods_id,
                                               make_number) AS maxt
                                 ON maxt.id = nom.id) AS BB
              ON BB.goods_id = m_goods.id
                 AND BB.make_number = tb_completedata.makenumber
       LEFT JOIN (SELECT Count(a.code) AS cntId,
                         a.code,
                         a.makenumber
                  FROM   tb_completedata a
                         INNER JOIN tb_setallcheck b
                                 ON a.code = b.code
                  GROUP  BY a.code,
                            a.makenumber) CC
              ON CC.makenumber = tb_completedata.makenumber
                 AND CC.code = tb_completedata.code


GO
/****** Object:  View [dbo].[v_ty_miss_check_list_newold]    Script Date: 2020/09/30 9:44:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_ty_miss_check_list_newold]
AS
SELECT                  '新旧' AS sysnm, ISNULL(tnew.finish_date, told.finish_date) AS finish_date, ISNULL(tnew.GoodsName, told.GoodsName) AS GoodsName, 
                                  ISNULL(tnew.goods_cd, told.goods_cd) AS goods_cd, ISNULL(tnew.makenumber, told.makenumber) AS makenumber, ISNULL(tnew.productionquantity, 
                                  told.productionquantity) AS productionquantity, ISNULL(tnew.product_line, told.product_line) AS product_line, ISNULL(tnew.direction, told.direction) 
                                  AS direction, ISNULL(tnew.pay_date, told.pay_date) AS pay_date, '新：' + tnew.jieguo + '--旧：' + told.jieguo AS jieguo
FROM                     (SELECT                  sysnm, finish_date, GoodsName, goods_cd, makenumber, productionquantity, product_line, direction, pay_date, jieguo
                                    FROM                     dbo.v_ty_miss_check_list
                                    WHERE                   (sysnm = '新')) AS tnew LEFT OUTER JOIN
                                      (SELECT                  sysnm, finish_date, GoodsName, goods_cd, makenumber, productionquantity, product_line, direction, pay_date, jieguo
                                            FROM                     dbo.v_ty_miss_check_list AS v_ty_miss_check_list_1
                                            WHERE                   (sysnm = '旧')) AS told ON tnew.goods_cd = told.goods_cd AND tnew.makenumber = told.makenumber
WHERE                   (tnew.jieguo <> '') AND (told.jieguo <> '')

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_m_check]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_m_check] ON [dbo].[m_check]
(
	[goods_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_m_check_1]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_m_check_1] ON [dbo].[m_check]
(
	[classify_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_m_check_2]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_m_check_2] ON [dbo].[m_check]
(
	[tools_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_m_classify]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_m_classify] ON [dbo].[m_classify]
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_t_check_result]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_t_check_result] ON [dbo].[t_check_result]
(
	[end_time] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_t_check_result_1]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_t_check_result_1] ON [dbo].[t_check_result]
(
	[goods_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_t_check_result_2]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_t_check_result_2] ON [dbo].[t_check_result]
(
	[make_number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [<Name of Missing Index, sysname,>]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>] ON [dbo].[TB_CompleteData]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TB_CompleteData]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_TB_CompleteData] ON [dbo].[TB_CompleteData]
(
	[MakeNumber] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TB_CompleteData_1]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_TB_CompleteData_1] ON [dbo].[TB_CompleteData]
(
	[MakeNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TB_CompleteData_2]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_TB_CompleteData_2] ON [dbo].[TB_CompleteData]
(
	[Finish_Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TB_CompleteData_3]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_TB_CompleteData_3] ON [dbo].[TB_CompleteData]
(
	[Product_Line] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TB_User]    Script Date: 2020/09/30 9:44:18 ******/
CREATE NONCLUSTERED INDEX [IX_TB_User] ON [dbo].[TB_User]
(
	[UserCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "m_check"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 150
               Right = 237
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_classify"
            Begin Extent = 
               Top = 6
               Left = 275
               Bottom = 150
               Right = 453
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_goods"
            Begin Extent = 
               Top = 6
               Left = 491
               Bottom = 150
               Right = 661
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_tools"
            Begin Extent = 
               Top = 6
               Left = 699
               Bottom = 150
               Right = 877
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_kind"
            Begin Extent = 
               Top = 150
               Left = 38
               Bottom = 294
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_type"
            Begin Extent = 
               Top = 150
               Left = 246
               Bottom = 294
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_department"
            Begin Extent = 
               Top = 150
               Left = 454
               Bottom = 294
               Right = 624
            End
            DisplayFlags = 2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_check_km_ms'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'80
            TopColumn = 0
         End
         Begin Table = "m_picture"
            Begin Extent = 
               Top = 150
               Left = 662
               Bottom = 294
               Right = 842
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_check_km_ms'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_check_km_ms'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tnew"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 218
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "told"
            Begin Extent = 
               Top = 6
               Left = 256
               Bottom = 125
               Right = 438
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_check_result'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_check_result'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "m_check"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 150
               Right = 237
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_goods"
            Begin Extent = 
               Top = 6
               Left = 275
               Bottom = 150
               Right = 445
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_tools"
            Begin Extent = 
               Top = 6
               Left = 483
               Bottom = 150
               Right = 661
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_classify"
            Begin Extent = 
               Top = 150
               Left = 38
               Bottom = 294
               Right = 216
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_goods_kind30160722BK'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_goods_kind30160722BK'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tnew"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 218
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "told"
            Begin Extent = 
               Top = 6
               Left = 256
               Bottom = 125
               Right = 436
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_ty_check_list'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_ty_check_list'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tnew"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 218
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "told"
            Begin Extent = 
               Top = 6
               Left = 256
               Bottom = 125
               Right = 436
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_ty_miss_check_list_newold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_ty_miss_check_list_newold'
GO
USE [master]
GO
ALTER DATABASE [AvoidMiss_New] SET  READ_WRITE 
GO
