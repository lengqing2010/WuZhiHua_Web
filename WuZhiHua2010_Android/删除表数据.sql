
declare @tblName varchar(50)
declare @fromDB varchar(50)
declare @ToDB varchar(50)
set @fromDB = '[AvoidMiss_New].[dbo].'
set @ToDB   = '[AvoidMiss_temp].[dbo].'

declare auth_cur cursor for

select name from sysobjects where xtype='U' and (name+'') not like '%_bk'

open auth_cur

fetch next from auth_cur into @tblName

while (@@fetch_status=0)

  begin

    print '删除表 导入: '+@tblName
	exec ('truncate table '+ @ToDB + @tblName + '')
	--exec ('insert into '+ @ToDB + @tblName + ' select * from '+ @fromDB + @tblName)

    fetch next from auth_cur into @tblName

  end

close auth_cur


--
--truncate table  AvoidMiss_temp.dbo.TB_CompleteData
--
--set IDENTITY_INSERT TB_CompleteData ON 
--insert into TB_CompleteData select * from AvoidMiss_New.dbo.TB_CompleteData
--set IDENTITY_INSERT TB_CompleteData Off
--
