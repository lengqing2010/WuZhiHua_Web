Imports System.Text
Imports System.Data.SqlClient
Imports System.Reflection.MethodBase
Imports Itis.ApplicationBlocks.Data.SQLHelper
Imports Itis.ApplicationBlocks.ExceptionManagement.UnTrappedExceptionManager
Imports Lixil.AvoidMissSystem.Utilities.Consts

Public Class MsBaobiaoDA

    Public JoinFlg As String = " or "

    ''' <summary>
    ''' 帐票1：检查结果明细表情报取得
    ''' </summary>
    ''' <param name="where">检索条件</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetJCJGMS(ByVal where As String) As DataTable

        'Dim ds As New DataSet
        'Dim paramList As New List(Of SqlParameter)

        'Dim sqlstr As New System.Text.StringBuilder

        'With sqlstr

        '    .AppendLine("SELECT    ")
        '    .AppendLine("    A.id --")
        '    .AppendLine("   , B.goods_name --商品名")
        '    .AppendLine("   , B.goods_cd --商品コード")
        '    .AppendLine("   , A.make_number --作番")
        '    .AppendLine("   , Isnull(CONVERT(VARCHAR(100), C.Finish_Date, 23), '') AS Finish_Date --生产实际日")
        '    .AppendLine("   , C.ProductionQuantity                                 AS ProductionQuantity --完了数量(生产数量???)")
        '    .AppendLine("   , Isnull(C.Product_Line, '')                           AS Product_Line --生产线")
        '    .AppendLine("   , Isnull(C.Direction, '')                              AS Direction --向先(方向？？？)")
        '    .AppendLine("   , Isnull(CONVERT(VARCHAR(100), C.Pay_Date, 23), '')    AS Pay_Date --纳期日")
        '    .AppendLine("   , Isnull(CONVERT(VARCHAR(100), A.end_time, 23), '')    AS Check_Date --检查日期")
        '    .AppendLine("   , Isnull(D.UserName, '')                               AS UserName --检查员")
        '    .AppendLine("   , F.tools_no --治具编号")
        '    .AppendLine("   , H.picture_id --图片名称")
        '    .AppendLine("   , CASE")
        '    .AppendLine("      WHEN Isnull(I.id, '') = '' THEN ''")
        '    .AppendLine("      ELSE '双击打开'")
        '    .AppendLine("    END  picture_content --图片二进制(需要做处理)")
        '    .AppendLine("   , H.classify_name --分类名称")
        '    .AppendLine("   , G.classify_order --分类表示顺序(检查顺序??)")
        '    .AppendLine("   , G.kind_cd --类别")
        '    .AppendLine("   , G.check_position --检查位置")
        '    .AppendLine("   , G.check_item --检查项目")
        '    .AppendLine("   , G.benchmark_type --基准类型")
        '    .AppendLine("   , G.benchmark_value1 --基准值1")
        '    .AppendLine("   , G.benchmark_value2 --基准值2")
        '    .AppendLine("   , G.benchmark_value3 --基准值3")



        '    .AppendLine("   , G.check_times --检查次数")
        '    .AppendLine("   , G.check_way --方法")
        '    '.AppendLine("   , E.measure_value1 --实测值1")
        '    '.AppendLine("   , E.measure_value2 --实测值2")



        '    Dim cn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

        '    If cn.IndexOf("AvoidMiss_Experiment") > 0 Then
        '        .Append("	   ,E.measure_value1 --实测值1  " & vbCrLf)
        '        .Append("	   ,E.measure_value2 --实测值2  " & vbCrLf)
        '        .Append("	   ,E.measure_value3 --实测值3  " & vbCrLf)
        '        .Append("	   ,E.measure_value4 --实测值4  " & vbCrLf)
        '        .Append("	   ,E.measure_value5 --实测值5  " & vbCrLf)
        '        .Append("	   ,E.measure_value6 --实测值6  " & vbCrLf)

        '    Else
        '        .Append("	   ,E.measure_value1 --实测值1  " & vbCrLf)
        '        .Append("	   ,E.measure_value2 --实测值2  " & vbCrLf)
        '    End If
        '    .AppendLine("   , E.result --检查结果")
        '    .AppendLine("   , E.remarks --备注")
        '    .AppendLine("   , A.qianpin_flg                                        AS Qianpin --欠品")
        '    .AppendLine("   , CONVERT(VARCHAR(100), A.start_time, 120)             AS start_time --检查开始时间")
        '    .AppendLine("   , CONVERT(VARCHAR(100), A.end_time, 120)               AS end_time --检查终了时间")
        '    .AppendLine("   , Datediff(ss, A.start_time, A.end_time)               AS Check_Time --检查时长")
        '    .AppendLine("   , A.shareResult_id --检查种类 ")
        '    .AppendLine("FROM   t_check_result A WITH(READCOMMITTED) --检查结果表")
        '    .AppendLine("       LEFT JOIN m_goods B")
        '    .AppendLine("              ON A.goods_id = B.id")
        '    .AppendLine("       LEFT JOIN TB_CompleteData C WITH(READCOMMITTED)")
        '    .AppendLine("              ON A.make_number = C.MakeNumber")
        '    .AppendLine("                 AND B.goods_cd = C.Code")
        '    .AppendLine("       LEFT JOIN TB_User D WITH(READCOMMITTED)")
        '    .AppendLine("              ON D.UserCode = A.check_user")
        '    .AppendLine("       LEFT JOIN t_result_detail E WITH(READCOMMITTED) --检查结果详细表")
        '    .AppendLine("              ON A.id = E.result_id")
        '    .AppendLine("       LEFT JOIN m_tools F WITH(READCOMMITTED) --治具表")
        '    .AppendLine("              ON E.check_id = F.id")
        '    .AppendLine("       LEFT JOIN m_check G WITH(READCOMMITTED) --检查表(基础表)")
        '    .AppendLine("              ON E.check_id = G.id")
        '    .AppendLine("       LEFT JOIN m_classify H WITH(READCOMMITTED) --分类表")
        '    .AppendLine("              ON G.classify_id = H.id")
        '    .AppendLine("       LEFT JOIN m_picture I WITH(READCOMMITTED) --图片表")
        '    .AppendLine("              ON H.picture_id = I.id")
        '    .AppendLine("                 AND I.delete_flg = 0")
        'End With

        'sqlstr.AppendLine(" WHERE A.continue_chk_flg<>'3' ")

        'If where.Replace(",", "").Trim <> "" Then
        '    Dim cols As New Generic.List(Of String)
        '    cols.Add("A.id")
        '    cols.Add("B.goods_name")
        '    cols.Add("B.goods_cd")
        '    cols.Add("A.make_number")
        '    cols.Add("C.Finish_Date")
        '    cols.Add("C.ProductionQuantity")
        '    cols.Add("C.Product_Line")
        '    cols.Add("C.Direction")
        '    cols.Add("C.Pay_Date")
        '    cols.Add("A.end_time")
        '    cols.Add("D.UserName")
        '    cols.Add("F.tools_no")
        '    cols.Add("H.picture_id")
        '    cols.Add("H.classify_name")
        '    cols.Add("G.classify_order")
        '    cols.Add("G.kind_cd")
        '    cols.Add("G.check_position")
        '    cols.Add("G.check_item")
        '    cols.Add("G.benchmark_type")
        '    cols.Add("G.benchmark_value1")
        '    cols.Add("G.benchmark_value2")
        '    cols.Add("G.benchmark_value3")
        '    cols.Add("G.check_times")
        '    cols.Add("G.check_way")
        '    'cols.Add("E.measure_value1")
        '    'cols.Add("E.measure_value2")
        '    Dim cn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

        '    If cn.IndexOf("AvoidMiss_Experiment") > 0 Then
        '        cols.Add("E.measure_value1" & vbCrLf)
        '        cols.Add("E.measure_value2" & vbCrLf)
        '        cols.Add("E.measure_value3" & vbCrLf)
        '        cols.Add("E.measure_value4" & vbCrLf)
        '        cols.Add("E.measure_value5" & vbCrLf)
        '        cols.Add("E.measure_value6" & vbCrLf)

        '    Else
        '        cols.Add("E.measure_value1" & vbCrLf)
        '        cols.Add("E.measure_value2" & vbCrLf)
        '    End If
        '    cols.Add("E.result")
        '    cols.Add("E.remarks")
        '    cols.Add("A.qianpin_flg")
        '    cols.Add("A.start_time")
        '    cols.Add("A.end_time")
        '    cols.Add("A.shareResult_id")

        '    Dim sqlValue() As String = where.Split(","c)

        '    sqlstr.AppendLine(GetWhereStrSql(cols, sqlValue))
        'End If

        'sqlstr.Append("ORDER  BY A.id,B.goods_name")

        ''検索の実行
        'Dim tableName As String = "mauthority"
        'FillDataset(DataAccessManager.Connection, CommandType.Text, sqlstr.ToString, ds, tableName, paramList.ToArray)
        'Return ds


        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        Dim strSqls As New System.Text.StringBuilder

        strSqls.AppendLine("SELECT [sysnm]")
        strSqls.AppendLine("      ,[finish_date]")
        strSqls.AppendLine("      ,[department_cd]")
        strSqls.AppendLine("      ,[line_name]")

        strSqls.AppendLine("      ,[goods_name]")
        strSqls.AppendLine("      ,[goods_cd]")
        strSqls.AppendLine("      ,[makenumber]")
        strSqls.AppendLine("      ,[productionquantity]")
        'strSqls.AppendLine("      ,[product_line]")
        strSqls.AppendLine("      ,[direction]")
        strSqls.AppendLine("      ,[pay_date]")
        strSqls.AppendLine("")
        strSqls.AppendLine("      ,[Check_Time]")
        strSqls.AppendLine("      ,[start_time]")
        strSqls.AppendLine("      ,[end_time]")
        strSqls.AppendLine("      ,[Check_use_Time]")
        strSqls.AppendLine("       ,CASE ")
        strSqls.AppendLine("         WHEN   jieguo = '' ")
        strSqls.AppendLine("              THEN 'OK' ")
        strSqls.AppendLine("         ELSE 'NG' ")
        strSqls.AppendLine("       END as jieguo ")
        strSqls.AppendLine("       ,jieguo as 详细")
        strSqls.AppendLine("      ,[UserName]")
        strSqls.AppendLine("      ,[remarks]")
        strSqls.AppendLine("  FROM [v_check_new_result_ws] ")

        strSqls.Append(" WHERE 1=1 and (")
        If where.Replace(",", "").Trim <> "" Then

            Dim ws() As String = where.Split(","c)
            Dim col() As String = "sysnm,finish_date,department_cd,line_name,goods_name,goods_cd,makenumber,productionquantity,direction,pay_date,Check_Time,start_time,end_time,Check_use_Time,jieguo,详细,UserName,remarks".Split(","c)
            Dim andFlg As Boolean = False

            For i As Integer = 0 To ws.Length - 1
                If ws(i) <> "" Then
                    If andFlg = True Then
                        strSqls.Append(JoinFlg)
                    End If
                    Try
                        strSqls.AppendLine(GetSqlWhere(col(i), ws(i)))
                    Catch ex As Exception
                        strSqls.AppendLine("1=1")
                    End Try
                    andFlg = True
                End If
            Next
        Else
            strSqls.Append(" 1=1 ")

        End If
        strSqls.Append(" ) ")
        strSqls.Append("ORDER  BY goods_cd")


        Dim sqlh As New PersonalDataAccess.SqlHelper

        Return sqlh.ExecSelectNo(strSqls.ToString, CommandType.Text)
        '検索の実行
        'Dim tableName As String = "mauthority"
        'FillDataset(DataAccessManager.Connection, CommandType.Text, strSqls.ToString, ds, tableName, paramList.ToArray)
        'Return ds

    End Function

    ''' <summary>
    ''' 帐票2：检查结果汇总表取得
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCheckResult(ByVal where As String) As DataTable
        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        Dim strSql As New System.Text.StringBuilder

        strSql.AppendLine("SELECT")
        strSql.AppendLine("    T.goods_name             --商品名")
        strSql.AppendLine("    ,T.goods_code            --商品コード")
        strSql.AppendLine("    ,T.make_number           --作番")
        strSql.AppendLine("    ,T.product_line          --生産線")
        strSql.AppendLine("    ,T.check_user            --検査者")
        strSql.AppendLine("    ,T.production_quantity   --完工数量")
        strSql.AppendLine("    ,T.Finish_Date           --生産日期")
        strSql.AppendLine("    ,T.Pay_Date              --納品日期")
        strSql.AppendLine("    ,T.check_finish          --検査完了")
        strSql.AppendLine("    ,T.check_default         --ディフォルト結果")
        strSql.AppendLine("    ,T.check_doing           --正在検査")
        strSql.AppendLine("    ,T.count_ok              --合格件数")
        strSql.AppendLine("    ,T.count_ng              --不合格件数")
        strSql.AppendLine("    ,case when T.qianpin_flg > 0 then '有' else '無' end as qianpin_flg          --欠品フラグ")
        strSql.AppendLine("FROM")
        strSql.AppendLine("    (")
        strSql.AppendLine("    SELECT")
        strSql.AppendLine("        G.goods_cd as goods_code")
        strSql.AppendLine("        ,G.goods_name as goods_name")
        strSql.AppendLine("        ,D.MakeNumber as make_number")
        strSql.AppendLine("        ,D.Product_Line as product_line")
        strSql.AppendLine("        ,U.UserName as check_user")
        strSql.AppendLine("        ,max(D.ProductionQuantity) as production_quantity")
        strSql.AppendLine("        ,max(CONVERT(varchar(100), D.Finish_Date, 23)) as Finish_Date")
        strSql.AppendLine("        ,max(CONVERT(varchar(100), D.Pay_Date, 23)) as Pay_Date")
        strSql.AppendLine("        ,sum(case when R.continue_chk_flg = '0' then 1 else 0 end) as check_finish") '完了
        strSql.AppendLine("        ,sum(case when R.continue_chk_flg = '1' or R.continue_chk_flg = '3' then 1 else 0 end) as check_doing")  '待判
        strSql.AppendLine("        ,sum(case when R.continue_chk_flg = '2' then 1 else 0 end) as check_default")    '继承
        strSql.AppendLine("        ,sum(case when R.result = 'OK' then 1 else 0 end) as count_ok")
        strSql.AppendLine("        ,sum(case when R.result = 'NG' then 1 else 0 end) as count_ng")
        strSql.AppendLine("        ,SUM(CASE WHEN R.qianpin_flg is null or R.qianpin_flg = '' then 0 else R.qianpin_flg end) as qianpin_flg")
        strSql.AppendLine("    FROM T_CHECK_RESULT R WITH(READCOMMITTED)")
        strSql.AppendLine("    LEFT JOIN TB_COMPLETEDATA D WITH(READCOMMITTED) ON")
        strSql.AppendLine("        D.makenumber = R.make_number")
        strSql.AppendLine("    LEFT JOIN M_GOODS G WITH(READCOMMITTED) ON")
        strSql.AppendLine("        G.id = R.goods_id")
        strSql.AppendLine("    LEFT JOIN TB_User U ON")
        strSql.AppendLine("        R.check_user = U.UserCode")

        If where.Replace(",", "").Trim <> "" Then
            strSql.Append(" WHERE ")
            Dim ws() As String = where.Split(","c)
            Dim sqlWhere As String = String.Empty
            sqlWhere = "T.goods_name,T.goods_code,T.make_number,T.product_line,T.check_user,T.production_quantity,T.Finish_Date,T.Pay_Date,T.count_ok,T.count_ng,R.start_time,R.end_time"
            Dim col() As String = sqlWhere.Split(","c)
            Dim andFlg As Boolean = False
            Dim subConditionFlg As Boolean = False
            Dim mainConditionFlg As Boolean = False

            '作成時間を検索条件に追加する
            For i As Integer = ws.Length - 2 To ws.Length - 1
                If ws(i) <> "" Then
                    subConditionFlg = True
                    If andFlg = True Then
                        strSql.Append(JoinFlg)
                    End If

                    Try
                        strSql.AppendLine(GetSqlWhere(col(i), ws(i)))
                    Catch ex As Exception
                        strSql.AppendLine("1=1")
                    End Try


                    andFlg = True
                End If
            Next

            If subConditionFlg = False Then
                strSql.AppendLine("1=1")
            End If

            strSql.AppendLine("    GROUP BY  G.goods_cd,G.goods_name, D.makenumber,D.Product_Line, U.UserName")
            strSql.AppendLine("    ) T")

            strSql.Append(" WHERE ")
            '初期化
            andFlg = False

            For i As Integer = 0 To ws.Length - 3
                If ws(i) <> "" Then
                    mainConditionFlg = True
                    If andFlg = True Then
                        strSql.Append(JoinFlg)
                    End If

                    Try
                        strSql.AppendLine(GetSqlWhere(col(i), ws(i)))
                    Catch ex As Exception
                        strSql.AppendLine("1=1")
                    End Try


                    andFlg = True
                End If
            Next

            If mainConditionFlg = False Then
                strSql.AppendLine("1=1")
            End If
        Else
            strSql.AppendLine("    GROUP BY  G.goods_cd,G.goods_name, D.makenumber,D.Product_Line, U.UserName")
            strSql.AppendLine("    ) T")
        End If

        ''検索の実行
        'Dim tableName As String = "t_check_result"
        'FillDataset(DataAccessManager.Connection, CommandType.Text, strSql.ToString, ds, tableName, paramList.ToArray)

        'Return ds


        Dim sqlh As New PersonalDataAccess.SqlHelper

        Return sqlh.ExecSelectNo(strSql.ToString, CommandType.Text)

    End Function

    ''' <summary>
    ''' 帐票3：未检查一览表
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWJCYL(ByVal where As String) As DataTable

        'Dim ds As New DataSet
        'Dim paramList As New List(Of SqlParameter)

        'Dim strSqls As New System.Text.StringBuilder

        'strSqls.Append("SELECT * FROM (  " & vbCrLf)
        'strSqls.Append("SELECT DISTINCT finish_date --生产实际日  " & vbCrLf)
        'strSqls.Append("                , " & vbCrLf)
        'strSqls.Append("                m_goods.goods_name --商品名  " & vbCrLf)
        'strSqls.Append("                , " & vbCrLf)
        ''strSqls.Append("                m_goods.goods_cd --商品コード  " & vbCrLf)
        'strSqls.Append("                tb_completedata.code as goods_cd --商品コード  " & vbCrLf)

        'strSqls.Append("                , " & vbCrLf)
        'strSqls.Append("                tb_completedata.makenumber--作番  " & vbCrLf)
        'strSqls.Append("                , " & vbCrLf)
        'strSqls.Append("                tb_completedata.productionquantity--完工数量  " & vbCrLf)
        'strSqls.Append("                , " & vbCrLf)
        'strSqls.Append("                tb_completedata.product_line--生产线  " & vbCrLf)
        'strSqls.Append("                , " & vbCrLf)
        'strSqls.Append("                tb_completedata.direction--向先  " & vbCrLf)
        'strSqls.Append("                , " & vbCrLf)
        'strSqls.Append("                tb_completedata.pay_date--纳期日  " & vbCrLf)
        'strSqls.Append("                , " & vbCrLf)
        'strSqls.Append("                CASE " & vbCrLf)
        'strSqls.Append("                  WHEN t_check_result.make_number IS NULL THEN '未检查' " & vbCrLf)
        'strSqls.Append("                  WHEN maxCheck.result = 'NG' THEN '不合格未二检' " & vbCrLf)
        'strSqls.Append("                  --when t_check_result.result='NG' then  " & vbCrLf)
        'strSqls.Append("                  --  '有欠品'  " & vbCrLf)
        'strSqls.Append("                  WHEN gghh.code IS NULL THEN '未全检' " & vbCrLf)
        'strSqls.Append("                  ELSE '' " & vbCrLf)
        'strSqls.Append("                END jieguo " & vbCrLf)
        'strSqls.Append("--原因  " & vbCrLf)
        'strSqls.Append("FROM   tb_completedata " & vbCrLf)
        'strSqls.Append("       LEFT JOIN m_goods " & vbCrLf)
        'strSqls.Append("              ON m_goods.goods_cd = tb_completedata.code " & vbCrLf)
        'strSqls.Append("                 AND m_goods.delete_flg = '0' " & vbCrLf)
        'strSqls.Append("       LEFT JOIN t_check_result " & vbCrLf)
        'strSqls.Append("              ON tb_completedata.makenumber = t_check_result.make_number " & vbCrLf)
        'strSqls.Append("                 AND t_check_result.goods_id = m_goods.id " & vbCrLf)
        'strSqls.Append("       LEFT JOIN (SELECT nom.* " & vbCrLf)
        'strSqls.Append("                  FROM   t_check_result AS nom " & vbCrLf)
        'strSqls.Append("                         INNER JOIN (SELECT goods_id, " & vbCrLf)
        'strSqls.Append("                                            make_number, " & vbCrLf)
        'strSqls.Append("                                            Max(start_time) a, " & vbCrLf)
        'strSqls.Append("                                            Max(end_time)   b " & vbCrLf)
        'strSqls.Append("                                     FROM   t_check_result " & vbCrLf)
        'strSqls.Append("                                     GROUP  BY goods_id, " & vbCrLf)
        'strSqls.Append("                                               make_number) AS maxt " & vbCrLf)
        'strSqls.Append("                                 ON maxt.goods_id = nom.goods_id " & vbCrLf)
        'strSqls.Append("                                    AND maxt.make_number = nom.make_number) AS " & vbCrLf)
        'strSqls.Append("                 maxCheck " & vbCrLf)
        'strSqls.Append("              ON maxCheck.goods_id = t_check_result.goods_id " & vbCrLf)
        'strSqls.Append("                 AND maxCheck.make_number = t_check_result.make_number " & vbCrLf)
        'strSqls.Append("       LEFT JOIN (SELECT a.goods_id, " & vbCrLf)
        'strSqls.Append("                         b.code, " & vbCrLf)
        'strSqls.Append("                         b.makenumber " & vbCrLf)
        'strSqls.Append("                  FROM   (SELECT Count(goods_id) AS gg, " & vbCrLf)
        'strSqls.Append("                                 goods_id, " & vbCrLf)
        'strSqls.Append("                                 make_number " & vbCrLf)
        'strSqls.Append("                          FROM   t_check_result a " & vbCrLf)
        'strSqls.Append("                          GROUP  BY goods_id, " & vbCrLf)
        'strSqls.Append("                                    make_number) AS a " & vbCrLf)
        'strSqls.Append("                         LEFT JOIN m_goods c " & vbCrLf)
        'strSqls.Append("                                ON a.goods_id = c.id " & vbCrLf)
        'strSqls.Append("                                AND c.delete_flg = '0' " & vbCrLf)
        'strSqls.Append("                         LEFT JOIN (SELECT Count(a.code) AS hh, " & vbCrLf)
        'strSqls.Append("                                           a.code, " & vbCrLf)
        'strSqls.Append("                                           a.makenumber " & vbCrLf)
        'strSqls.Append("                                    FROM   tb_completedata a " & vbCrLf)
        'strSqls.Append("                                           INNER JOIN tb_setallcheck b " & vbCrLf)
        'strSqls.Append("                                                   ON a.code = b.code " & vbCrLf)
        'strSqls.Append("                                    GROUP  BY a.code, " & vbCrLf)
        'strSqls.Append("                                              a.makenumber) AS b " & vbCrLf)
        'strSqls.Append("                                ON a.make_number = b.makenumber " & vbCrLf)
        'strSqls.Append("                                   AND c.goods_cd = b.code " & vbCrLf)
        'strSqls.Append("                  WHERE  gg < hh) AS gghh " & vbCrLf)
        'strSqls.Append("              ON tb_completedata.makenumber = gghh.makenumber " & vbCrLf)
        'strSqls.Append("                 AND tb_completedata.code = gghh.code " & vbCrLf)

        'strSqls.AppendLine(") mmain ")


        'If where.Replace(",", "").Trim <> "" Then
        '    strSqls.Append(" WHERE ")
        '    Dim ws() As String = where.Split(","c)
        '    'Dim col() As String = "finish_date,m_goods.goods_name,m_goods.goods_cd,tb_completedata.makenumber,tb_completedata.productionquantity,tb_completedata.product_line,tb_completedata.direction,tb_completedata.pay_date".Split(","c)
        '    Dim col() As String = "finish_date,goods_name,goods_cd,makenumber,productionquantity,product_line,direction,pay_date,jieguo".Split(","c)

        '    Dim andFlg As Boolean = False

        '    For i As Integer = 0 To ws.Length - 1
        '        If ws(i) <> "" Then
        '            If andFlg = True Then
        '                strSqls.Append(JoinFlg)
        '            End If
        '            Try
        '                strSqls.AppendLine(GetSqlWhere(col(i), ws(i)))
        '            Catch ex As Exception
        '                strSqls.AppendLine("1=1")
        '            End Try


        '            andFlg = True
        '        End If
        '    Next

        'End If

        'strSqls.Append("ORDER  BY goods_cd")


        ''検索の実行
        'Dim tableName As String = "mauthority"
        'FillDataset(DataAccessManager.Connection, CommandType.Text, strSqls.ToString, ds, tableName, paramList.ToArray)
        'Return ds


        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        Dim strSqls As New System.Text.StringBuilder

        strSqls.Append("SELECT [finish_date],goods_name ,[goods_cd],[makenumber],[productionquantity],[product_line],[direction],[pay_date],[jieguo] FROM v_check_new_result  " & vbCrLf)
        strSqls.Append(" WHERE sysnm = '新' and (")
        If where.Replace(",", "").Trim <> "" Then

            Dim ws() As String = where.Split(","c)
            Dim col() As String = "finish_date,GoodsName,goods_cd,makenumber,productionquantity,product_line,direction,pay_date,jieguo".Split(","c)
            Dim andFlg As Boolean = False

            For i As Integer = 0 To ws.Length - 1
                If ws(i) <> "" Then
                    If andFlg = True Then
                        strSqls.Append(JoinFlg)
                    End If
                    Try
                        strSqls.AppendLine(GetSqlWhere(col(i), ws(i)))
                    Catch ex As Exception
                        strSqls.AppendLine("1=1")
                    End Try
                    andFlg = True
                End If
            Next
        Else
            strSqls.Append(" 1=1 ")

        End If
        strSqls.Append(" ) and (jieguo<>'OK' and jieguo<>'')")
        strSqls.Append("ORDER  BY goods_cd")


        ''検索の実行
        'Dim tableName As String = "mauthority"
        'FillDataset(DataAccessManager.Connection, CommandType.Text, strSqls.ToString, ds, tableName, paramList.ToArray)
        'Return ds

        Dim sqlh As New PersonalDataAccess.SqlHelper

        Return sqlh.ExecSelectNo(strSqls.ToString, CommandType.Text)

    End Function

    ''' <summary>
    ''' 帐票4：图片情报取得
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPictureInfo(ByVal where As String) As DataTable
        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        Dim strSql As New System.Text.StringBuilder

        strSql.AppendLine(" SELECT DISTINCT")
        strSql.AppendLine("     m_picture.picture_nm AS picture_nm,           --图片名称")
        strSql.AppendLine("     m_picture.picture_name AS picture_name,       --图片描述")
        strSql.AppendLine("     '' AS picture_path,                           --图片位置")
        strSql.AppendLine("     m_picture.picture_content AS picture_content  --图片二进制")
        strSql.AppendLine(" FROM")
        strSql.AppendLine("     m_picture WITH(READCOMMITTED)")
        strSql.AppendLine(" LEFT JOIN")
        strSql.AppendLine("     m_classify WITH(READCOMMITTED)")
        strSql.AppendLine(" ON  m_picture.id = m_classify.picture_id")
        strSql.AppendLine(" AND m_classify.delete_flg <> '1' ")
        strSql.AppendLine(" LEFT JOIN")
        strSql.AppendLine("     m_goods WITH(READCOMMITTED)")
        strSql.AppendLine(" ON  m_classify.goods_id = m_goods.id")
        strSql.AppendLine(" AND m_goods.delete_flg <> '1' ")

        If where.Replace(",", "").Trim <> "" Then
            strSql.Append(" WHERE ")
            Dim ws() As String = where.Split(","c)
            Dim sqlWhere As String = String.Empty
            sqlWhere = "m_goods.goods_cd,m_picture.id,m_picture.picture_nm,m_picture.picture_name"
            Dim col() As String = sqlWhere.Split(","c)
            Dim andFlg As Boolean = False

            For i As Integer = 0 To ws.Length - 1
                If ws(i) <> "" Then
                    If andFlg = True Then
                        strSql.Append(JoinFlg)
                    End If

                    Try
                        strSql.AppendLine(GetSqlWhere(col(i), ws(i)))
                    Catch ex As Exception
                        strSql.AppendLine("1=1")
                    End Try


                    andFlg = True
                End If
            Next

        End If

        ''検索の実行
        'Dim tableName As String = "mpicuture"
        'FillDataset(DataAccessManager.Connection, CommandType.Text, strSql.ToString, ds, tableName, paramList.ToArray)

        'Return ds

        Dim sqlh As New PersonalDataAccess.SqlHelper

        Return sqlh.ExecSelectNo(strSql.ToString, CommandType.Text)

    End Function

    ''' <summary>
    ''' 帐票5：治具情报取得
    ''' </summary>
    ''' <param name="where">检索条件</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetToolsInfo(ByVal where As String) As DataTable
        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        Dim strSqls As New System.Text.StringBuilder

        strSqls.Append("SELECT  mt.remarks --治具名称  " & vbCrLf)
        strSqls.Append("       ,mt.id --治具ID  " & vbCrLf)
        strSqls.Append("       ,mt.barcode --条马  " & vbCrLf)
        strSqls.Append("       ,mt.barcode_flg --治具有无  " & vbCrLf)
        strSqls.Append("FROM m_tools mt WITH(READCOMMITTED)   --治具表  " & vbCrLf)

        If where.Replace(",", "").Trim <> "" Then
            strSqls.Append(" WHERE ")
            Dim ws() As String = where.Split(","c)
            Dim sqlWhere As String = String.Empty
            sqlWhere = "mt.remarks,mg.goods_cd,mt.id,mt.distinguish,mt.barcode,mt.barcode_flg"
            Dim col() As String = sqlWhere.Split(","c)
            Dim andFlg As Boolean = False

            For i As Integer = 0 To ws.Length - 1
                If ws(i) <> "" Then
                    If andFlg = True Then
                        strSqls.Append(JoinFlg)
                    End If

                    If i = 1 Then
                        strSqls.Append(" EXISTS(SELECT tools_id FROM m_goods mg WITH(READCOMMITTED)  " & vbCrLf)
                        strSqls.Append(" LEFT JOIN m_classify mc WITH(READCOMMITTED)  " & vbCrLf)
                        strSqls.Append(" ON mc.goods_id = mg.id  " & vbCrLf)
                        strSqls.Append(" AND mc.delete_flg = 0  " & vbCrLf)
                        strSqls.Append(" WHERE mg.goods_cd like (N'%" & ws(i) & "%')  " & vbCrLf)
                        strSqls.Append(" AND mc.tools_id = mt.id " & vbCrLf)
                        strSqls.Append(" AND mg.delete_flg = 0 )" & vbCrLf)
                    Else
                        Try
                            strSqls.AppendLine(GetSqlWhere(col(i), ws(i)))
                        Catch ex As Exception
                            strSqls.AppendLine("1=1")
                        End Try

                    End If

                    andFlg = True
                End If
            Next
        End If

        strSqls.Append(" ORDER  BY mt.id")

        ''検索の実行
        'Dim tableName As String = "mtools"
        'FillDataset(DataAccessManager.Connection, CommandType.Text, strSqls.ToString, ds, tableName, paramList.ToArray)

        'Return ds

        Dim sqlh As New PersonalDataAccess.SqlHelper

        Return sqlh.ExecSelectNo(strSqls.ToString, CommandType.Text)

    End Function

    ''' <summary>
    ''' 帐票6：检查项目取得
    ''' </summary>
    ''' <param name="where">检索条件</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetXM(ByVal where As String) As DataSet

        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        Dim strSqls As New System.Text.StringBuilder
        strSqls.Append("SELECT DISTINCT b.goods_cd --商品コード  " & vbCrLf)
        strSqls.Append("      ,A.make_number --作番  " & vbCrLf)

        Dim cn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

        If cn.IndexOf("AvoidMiss_Experiment") > 0 Then
            strSqls.Append("	   ,E.measure_value1 --实测值1  " & vbCrLf)
            strSqls.Append("	   ,E.measure_value2 --实测值2  " & vbCrLf)
            strSqls.Append("	   ,E.measure_value3 --实测值3  " & vbCrLf)
            strSqls.Append("	   ,E.measure_value4 --实测值4  " & vbCrLf)
            strSqls.Append("	   ,E.measure_value5 --实测值5  " & vbCrLf)
            strSqls.Append("	   ,E.measure_value6 --实测值6  " & vbCrLf)

        Else
            strSqls.Append("	   ,E.measure_value1 --实测值1  " & vbCrLf)
            strSqls.Append("	   ,E.measure_value2 --实测值2  " & vbCrLf)
        End If

        strSqls.Append("	   ,C.Finish_Date --生产日期  " & vbCrLf)
        strSqls.Append("	   ,C.Pay_Date --出荷日期  " & vbCrLf)
        strSqls.Append("	   ,C.ProductionQuantity --数量  " & vbCrLf)
        strSqls.Append("FROM t_check_result A WITH(READCOMMITTED)   --检查结果表  " & vbCrLf)
        strSqls.Append("LEFT JOIN m_goods B  " & vbCrLf)
        strSqls.Append("ON A.goods_id  = B.id   " & vbCrLf)
        strSqls.Append("LEFT JOIN TB_CompleteData C WITH(READCOMMITTED)  " & vbCrLf)
        strSqls.Append("ON A.make_number = C.MakeNumber   " & vbCrLf)
        strSqls.Append("and B.goods_cd = C.Code   " & vbCrLf)
        strSqls.Append("LEFT JOIN TB_User D WITH(READCOMMITTED)  " & vbCrLf)
        strSqls.Append("ON D.UserCode = A.check_user  " & vbCrLf)
        strSqls.Append("LEFT JOIN t_result_detail E WITH(READCOMMITTED)   --检查结果详细表  " & vbCrLf)
        strSqls.Append("ON A.id = E.result_id  " & vbCrLf)

        If where.Replace(",", "").Trim <> "" Then
            strSqls.Append(" WHERE ")
            Dim ws() As String = where.Split(","c)
            Dim sqlWhere As String = String.Empty
            If cn.IndexOf("AvoidMiss_Experiment") > 0 Then
                sqlWhere = "b.goods_cd,b.make_number,E.measure_value1,E.measure_value2,E.measure_value3,E.measure_value4,E.measure_value5,E.measure_value6,C.Finish_Date,C.Pay_Date,C.ProductionQuantity"

            Else
                sqlWhere = "b.goods_cd,b.make_number,E.measure_value1,E.measure_value2,C.Finish_Date,C.Pay_Date,C.ProductionQuantity"

            End If
            Dim col() As String = sqlWhere.Split(","c)
            Dim andFlg As Boolean = False

            For i As Integer = 0 To ws.Length - 1
                If ws(i) <> "" Then
                    If andFlg = True Then
                        strSqls.Append(JoinFlg)
                    End If
                    Try
                        strSqls.AppendLine(GetSqlWhere(col(i), ws(i)))
                    Catch ex As Exception
                        strSqls.AppendLine("1=1")
                    End Try


                    andFlg = True
                End If
            Next

        End If

        strSqls.Append("ORDER  BY b.goods_cd")

        '検索の実行
        Dim tableName As String = "mauthority"
        FillDataset(DataAccessManager.Connection, CommandType.Text, strSqls.ToString, ds, tableName, paramList.ToArray)
        Return ds

    End Function

    ''' <summary>
    ''' 帐票7：新 旧未检查一览表 
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMissCheckList(ByVal where As String) As DataTable

        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        Dim strSqls As New System.Text.StringBuilder

        'strSqls.Append("SELECT * FROM v_check_result  " & vbCrLf)
        strSqls.Append("SELECT sysnm,[finish_date], GoodsName, [goods_cd], [makenumber], [productionquantity], [product_line], [direction], [pay_date], [详细] FROM v_check_result  " & vbCrLf)

        strSqls.Append(" WHERE 1=1 and (")
        If where.Replace(",", "").Trim <> "" Then

            Dim ws() As String = where.Split(","c)
            Dim col() As String = "sysnm,finish_date,GoodsName,goods_cd,makenumber,productionquantity,product_line,direction,pay_date,详细".Split(","c)
            Dim andFlg As Boolean = False

            For i As Integer = 0 To ws.Length - 1
                If ws(i) <> "" Then
                    If andFlg = True Then
                        strSqls.Append(JoinFlg)
                    End If
                    Try
                        strSqls.AppendLine(GetSqlWhere(col(i), ws(i)))
                    Catch ex As Exception
                        strSqls.AppendLine("1=1")
                    End Try
                    andFlg = True
                End If
            Next
        Else
            strSqls.Append(" 1=1 ")

        End If
        strSqls.Append(" ) and jieguo<>'OK'")
        strSqls.Append("ORDER  BY goods_cd")


        '検索の実行
        'Dim tableName As String = "mauthority"
        'FillDataset(DataAccessManager.Connection, CommandType.Text, strSqls.ToString, ds, tableName, paramList.ToArray)

        Dim sqlh As New PersonalDataAccess.SqlHelper

        Return sqlh.ExecSelectNo(strSqls.ToString, CommandType.Text)

        'Return ds

    End Function

    ''' <summary>
    ''' 帐票8：新 旧检查结果明细表情报取得
    ''' </summary>
    ''' <param name="where">检索条件</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNewOldNewOldCheckResultMs(ByVal where As String) As DataTable

        'Dim ds As New DataSet
        'Dim paramList As New List(Of SqlParameter)

        'Dim sqlstr As New System.Text.StringBuilder

        'With sqlstr

        '    .AppendLine("SELECT  * ")
        '    .AppendLine("from ")
        '    .AppendLine("  (")
        '    .AppendLine("    SELECT ")
        '    .AppendLine("      '新' as syskind, ")
        '    .AppendLine("      B.goods_name --商品名")
        '    .AppendLine("      , ")
        '    .AppendLine("      B.goods_cd --商品コード")
        '    .AppendLine("      , ")
        '    .AppendLine("      A.make_number --作番")
        '    .AppendLine("      , ")
        '    .AppendLine("      Isnull(")
        '    .AppendLine("        CONVERT(")
        '    .AppendLine("          VARCHAR(100), ")
        '    .AppendLine("          C.Finish_Date, ")
        '    .AppendLine("          23")
        '    .AppendLine("        ), ")
        '    .AppendLine("        ''")
        '    .AppendLine("      ) AS Finish_Date --生产实际日")
        '    .AppendLine("      , ")
        '    .AppendLine("      C.ProductionQuantity AS ProductionQuantity --完了数量(生产数量???)")
        '    .AppendLine("      , ")
        '    .AppendLine("      Isnull(C.Product_Line, '') AS Product_Line --生产线")
        '    .AppendLine("      , ")
        '    .AppendLine("      Isnull(C.Direction, '') AS Direction --向先(方向？？？)")
        '    .AppendLine("      , ")
        '    .AppendLine("      Isnull(")
        '    .AppendLine("        CONVERT(")
        '    .AppendLine("          VARCHAR(100), ")
        '    .AppendLine("          C.Pay_Date, ")
        '    .AppendLine("          23")
        '    .AppendLine("        ), ")
        '    .AppendLine("        ''")
        '    .AppendLine("      ) AS Pay_Date --纳期日")
        '    .AppendLine("      , ")
        '    .AppendLine("      Isnull(")
        '    .AppendLine("        CONVERT(")
        '    .AppendLine("          VARCHAR(100), ")
        '    .AppendLine("          A.end_time, ")
        '    .AppendLine("          23")
        '    .AppendLine("        ), ")
        '    .AppendLine("        ''")
        '    .AppendLine("      ) AS Check_Date --检查日期")
        '    .AppendLine("      , ")
        '    .AppendLine("      Isnull(D.UserName, '') AS UserName --检查员")
        '    .AppendLine("      , ")
        '    .AppendLine("      F.tools_no --治具编号")
        '    .AppendLine("      , ")
        '    .AppendLine("      H.picture_id --图片名称")
        '    .AppendLine("      , ")
        '    .AppendLine("      CASE WHEN Isnull(I.id, '') = '' THEN '' ELSE '双击打开' END picture_content --图片二进制(需要做处理)")
        '    .AppendLine("      , ")
        '    .AppendLine("      H.classify_name --分类名称")
        '    .AppendLine("      , ")
        '    .AppendLine("      G.classify_order --分类表示顺序(检查顺序??)")
        '    .AppendLine("      , ")
        '    .AppendLine("      G.kind_cd --类别")
        '    .AppendLine("      , ")
        '    .AppendLine("      G.check_position --检查位置")
        '    .AppendLine("      , ")
        '    .AppendLine("      G.check_item --检查项目")
        '    .AppendLine("      , ")
        '    .AppendLine("      G.benchmark_type --基准类型")
        '    .AppendLine("      , ")
        '    .AppendLine("      G.benchmark_value1 --基准值1")
        '    .AppendLine("      , ")
        '    .AppendLine("      G.benchmark_value2 --基准值2")
        '    .AppendLine("      , ")
        '    .AppendLine("      G.benchmark_value3 --基准值3")
        '    .AppendLine("      , ")
        '    .AppendLine("      G.check_times --检查次数")
        '    .AppendLine("      , ")
        '    .AppendLine("      G.check_way --方法")
        '    .AppendLine("      , ")
        '    .AppendLine("      E.measure_value1 --实测值1")
        '    .AppendLine("      , ")
        '    .AppendLine("      E.measure_value2 --实测值2")
        '    .AppendLine("      , ")
        '    .AppendLine("      E.result --检查结果")
        '    .AppendLine("      , ")
        '    .AppendLine("      E.remarks --备注")
        '    .AppendLine("      , ")
        '    .AppendLine("      A.qianpin_flg AS Qianpin --欠品")
        '    .AppendLine("      , ")
        '    .AppendLine("      CONVERT(")
        '    .AppendLine("        VARCHAR(100), ")
        '    .AppendLine("        A.start_time, ")
        '    .AppendLine("        120")
        '    .AppendLine("      ) AS start_time --检查开始时间")
        '    .AppendLine("      , ")
        '    .AppendLine("      CONVERT(")
        '    .AppendLine("        VARCHAR(100), ")
        '    .AppendLine("        A.end_time, ")
        '    .AppendLine("        120")
        '    .AppendLine("      ) AS end_time --检查终了时间")
        '    .AppendLine("      , ")
        '    .AppendLine("      Datediff(ss, A.start_time, A.end_time) AS Check_Time --检查时长")
        '    .AppendLine("      , ")
        '    .AppendLine("      A.shareResult_id --检查种类 ")
        '    .AppendLine("    FROM ")
        '    .AppendLine("      t_check_result A WITH(READCOMMITTED) --检查结果表")
        '    .AppendLine("      LEFT JOIN m_goods B ON A.goods_id = B.id ")
        '    .AppendLine("      LEFT JOIN TB_CompleteData C WITH(READCOMMITTED) ON A.make_number = C.MakeNumber ")
        '    .AppendLine("      AND B.goods_cd = C.Code ")
        '    .AppendLine("      LEFT JOIN TB_User D WITH(READCOMMITTED) ON D.UserCode = A.check_user ")
        '    .AppendLine("      LEFT JOIN t_result_detail E WITH(READCOMMITTED) --检查结果详细表")
        '    .AppendLine("      ON A.id = E.result_id ")
        '    .AppendLine("      LEFT JOIN m_tools F WITH(READCOMMITTED) --治具表")
        '    .AppendLine("      ON E.check_id = F.id ")
        '    .AppendLine("      LEFT JOIN m_check G WITH(READCOMMITTED) --检查表(基础表)")
        '    .AppendLine("      ON E.check_id = G.id ")
        '    .AppendLine("      LEFT JOIN m_classify H WITH(READCOMMITTED) --分类表")
        '    .AppendLine("      ON G.classify_id = H.id ")
        '    .AppendLine("      LEFT JOIN m_picture I WITH(READCOMMITTED) --图片表")
        '    .AppendLine("      ON H.picture_id = I.id ")
        '    .AppendLine("      AND I.delete_flg = 0 ")
        '    .AppendLine("    WHERE ")
        '    .AppendLine("      A.continue_chk_flg <> '3' ")
        '    .AppendLine("    union all ")
        '    .AppendLine("    SELECT ")
        '    .AppendLine("      '旧' as syskind, ")
        '    .AppendLine("      tb_typems.goodsname --商品名")
        '    .AppendLine("      , ")
        '    .AppendLine("      tb_completedata.Code --商品コード")
        '    .AppendLine("      , ")
        '    .AppendLine("      tb_completedata.makenumber ")
        '    .AppendLine("      /*作番  */")
        '    .AppendLine("      , ")
        '    .AppendLine("      tb_completedata.Finish_date, ")
        '    .AppendLine("      tb_completedata.productionquantity ")
        '    .AppendLine("      /*完工数量  */")
        '    .AppendLine("      , ")
        '    .AppendLine("      tb_completedata.product_line ")
        '    .AppendLine("      /*生产线  */")
        '    .AppendLine("      , ")
        '    .AppendLine("      tb_completedata.direction ")
        '    .AppendLine("      /*向先  */")
        '    .AppendLine("      , ")
        '    .AppendLine("      tb_completedata.pay_date ")
        '    .AppendLine("      /*纳期日 */")
        '    .AppendLine("      , ")
        '    .AppendLine("      tb_checkdetail.CompleteDate AS Check_Date --检查日期")
        '    .AppendLine("      , ")
        '    .AppendLine("      Isnull(TB_User.UserName, '') AS UserName --检查员")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as tools_no --治具编号")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as picture_id --图片名称")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as picture_content, ")
        '    .AppendLine("      '' as classify_name, ")
        '    .AppendLine("      '' as classify_order, ")
        '    .AppendLine("      '' as kind_cd --类别")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as check_position --检查位置")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as check_item --检查项目")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as benchmark_type --基准类型")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as benchmark_value1 --基准值1")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as benchmark_value2 --基准值2")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as benchmark_value3 --基准值3")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as check_times --检查次数")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as check_way --方法")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as measure_value1 --实测值1")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as measure_value2 --实测值2")
        '    .AppendLine("      , ")
        '    .AppendLine("      CASE WHEN tb_checkdetail.Result = 'OK' ")
        '    .AppendLine("      AND tb_checkdetail.InstructionBookState = 'OK' ")
        '    .AppendLine("      AND tb_checkdetail.CabinetPartsState = 'OK' ")
        '    .AppendLine("      AND tb_checkdetail.DYState = 'OK' THEN 'OK' ELSE 'NG' END as result --检查结果")
        '    .AppendLine("      --, tb_checkdetail.Result as result --检查结果")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as remarks --备注")
        '    .AppendLine("      , ")
        '    .AppendLine("      tb_checkdetail.lack AS Qianpin --欠品")
        '    .AppendLine("      , ")
        '    .AppendLine("      CONVERT(")
        '    .AppendLine("        VARCHAR(100), ")
        '    .AppendLine("        tb_checkdetail.ScanDate, ")
        '    .AppendLine("        120")
        '    .AppendLine("      ) AS start_time --检查开始时间")
        '    .AppendLine("      , ")
        '    .AppendLine("      CONVERT(")
        '    .AppendLine("        VARCHAR(100), ")
        '    .AppendLine("        tb_checkdetail.CompleteDate, ")
        '    .AppendLine("        120")
        '    .AppendLine("      ) AS end_time --检查终了时间")
        '    .AppendLine("      , ")
        '    .AppendLine("      Datediff(")
        '    .AppendLine("        ss, ")
        '    .AppendLine("        isnull(")
        '    .AppendLine("          tb_checkdetail.ScanDate, ")
        '    .AppendLine("          getdate()")
        '    .AppendLine("        ), ")
        '    .AppendLine("        isnull(")
        '    .AppendLine("          tb_checkdetail.CompleteDate, ")
        '    .AppendLine("          getdate()")
        '    .AppendLine("        )")
        '    .AppendLine("      ) AS Check_Time --检查时长")
        '    .AppendLine("      , ")
        '    .AppendLine("      '' as shareResult_id --A.shareResult_id --检查种类 ")
        '    .AppendLine("    FROM ")
        '    .AppendLine("      [ScanCheck].[dbo].[tb_checkdetail] ")
        '    .AppendLine("      LEFT JOIN [ScanCheck].[dbo].tb_completedata ON tb_completedata.makenumber = tb_checkdetail.makenumber ")
        '    .AppendLine("      And tb_completedata.code = tb_checkdetail.code ")
        '    .AppendLine("      LEFT JOIN [ScanCheck].[dbo].tb_typems ON tb_completedata.code = tb_typems.code ")
        '    .AppendLine("      LEFT JOIN [ScanCheck].[dbo].[TB_User] ON tb_checkdetail.CheckUser = TB_User.UserCode")
        '    .AppendLine("  ) a ")
        'End With

        'sqlstr.AppendLine(" WHERE 1=1 ")

        'If where.Replace(",", "").Trim <> "" Then
        '    Dim cols As New Generic.List(Of String)
        '    cols.Add("a.syskind")
        '    cols.Add("a.goods_name")
        '    cols.Add("a.goods_cd")
        '    cols.Add("a.make_number")
        '    cols.Add("a.Finish_Date")
        '    cols.Add("a.ProductionQuantity")
        '    cols.Add("a.Product_Line")
        '    cols.Add("a.Direction")
        '    cols.Add("a.Pay_Date")
        '    cols.Add("a.Check_Date")
        '    cols.Add("a.UserName")
        '    cols.Add("a.tools_no")
        '    cols.Add("a.picture_id")
        '    cols.Add("a.classify_name")
        '    cols.Add("a.classify_order")
        '    cols.Add("a.kind_cd")
        '    cols.Add("a.check_position")
        '    cols.Add("a.check_item")
        '    cols.Add("a.benchmark_type")
        '    cols.Add("a.benchmark_value1")
        '    cols.Add("a.benchmark_value2")
        '    cols.Add("a.benchmark_value3")
        '    cols.Add("a.check_times")
        '    cols.Add("a.check_way")
        '    cols.Add("a.measure_value1")
        '    cols.Add("a.measure_value2")
        '    cols.Add("a.result")
        '    cols.Add("a.remarks")
        '    cols.Add("a.qianpin_flg")
        '    cols.Add("a.start_time")
        '    cols.Add("a.end_time")
        '    cols.Add("a.shareResult_id")

        '    Dim sqlValue() As String = where.Split(","c)

        '    sqlstr.AppendLine(GetWhereStrSql(cols, sqlValue))
        'End If

        'sqlstr.Append("ORDER  BY a.syskind, a.goods_name")

        ''検索の実行
        'Dim tableName As String = "mauthority"
        'FillDataset(DataAccessManager.Connection, CommandType.Text, sqlstr.ToString, ds, tableName, paramList.ToArray)
        'Return ds


        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        Dim strSqls As New System.Text.StringBuilder

        strSqls.Append("SELECT * FROM v_check_result  " & vbCrLf)
        strSqls.Append(" WHERE 1=1 and (")
        If where.Replace(",", "").Trim <> "" Then

            Dim ws() As String = where.Split(","c)
            Dim col() As String = "sysnm,finish_date,GoodsName,goods_cd,makenumber,productionquantity,product_line,direction,pay_date,Check_Time,start_time,end_time,Check_use_Time,jieguo,详细,UserName,remarks".Split(","c)
            Dim andFlg As Boolean = False

            For i As Integer = 0 To ws.Length - 1
                If ws(i) <> "" Then
                    If andFlg = True Then
                        strSqls.Append(JoinFlg)
                    End If
                    Try
                        strSqls.AppendLine(GetSqlWhere(col(i), ws(i)))
                    Catch ex As Exception
                        strSqls.AppendLine("1=1")
                    End Try
                    andFlg = True
                End If
            Next
        Else
            strSqls.Append(" 1=1 ")

        End If
        strSqls.Append(" ) ")
        strSqls.Append("ORDER  BY goods_cd")


        '検索の実行
        'Dim tableName As String = "mauthority"
        'FillDataset(DataAccessManager.Connection, CommandType.Text, strSqls.ToString, ds, tableName, paramList.ToArray)
        'Return ds

        Dim sqlh As New PersonalDataAccess.SqlHelper
        Return sqlh.ExecSelectNo(strSqls.ToString, CommandType.Text)

    End Function


    Public Function GetCheckMs(ByVal where As String) As DataSet

        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        Dim sqlstr As New System.Text.StringBuilder

        With sqlstr

            .AppendLine("SELECT    ")
            .AppendLine("    A.id --")
            .AppendLine("   , J.department_cd ")
            .AppendLine("   , J.line_name ")
            .AppendLine("   , B.goods_name --商品名")
            .AppendLine("   , B.goods_cd --商品コード")
            .AppendLine("   , A.make_number --作番")
            .AppendLine("   , Isnull(CONVERT(VARCHAR(100), C.Finish_Date, 23), '') AS Finish_Date --生产实际日")
            .AppendLine("   , C.ProductionQuantity                                 AS ProductionQuantity --完了数量(生产数量???)")
            .AppendLine("   , Isnull(C.Product_Line, '')                           AS Product_Line --生产线")
            .AppendLine("   , Isnull(C.Direction, '')                              AS Direction --向先(方向？？？)")
            .AppendLine("   , Isnull(CONVERT(VARCHAR(100), C.Pay_Date, 23), '')    AS Pay_Date --纳期日")
            .AppendLine("   , Isnull(CONVERT(VARCHAR(100), A.end_time, 23), '')    AS Check_Date --检查日期")

            Dim cn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString
            If cn.IndexOf("AvoidMiss_Experiment") > 0 Then
                .AppendLine("   , Isnull(E.check_user_cd, '')                               AS UserName --检查员")
            Else
                .AppendLine("   , Isnull(D.UserName, '')                               AS UserName --检查员")
            End If

            .AppendLine("   , F.tools_no --治具编号")
            .AppendLine("   , H.picture_id --图片名称")
            .AppendLine("   , CASE")
            .AppendLine("      WHEN Isnull(I.id, '') = '' THEN ''")
            .AppendLine("      ELSE '双击打开'")
            .AppendLine("    END  picture_content --图片二进制(需要做处理)")

            '.AppendLine("   , I.picture_content --图片名称")


            .AppendLine("   , H.classify_name --分类名称")
            .AppendLine("   , G.classify_order --分类表示顺序(检查顺序??)")
            .AppendLine("   , G.kind_cd --类别")
            .AppendLine("   , G.check_position --检查位置")
            .AppendLine("   , G.check_item --检查项目")
            .AppendLine("   , G.benchmark_type --基准类型")
            .AppendLine("   , G.benchmark_value1 --基准值1")
            .AppendLine("   , G.benchmark_value2 --基准值2")
            .AppendLine("   , G.benchmark_value3 --基准值3")



            .AppendLine("   , G.check_times --检查次数")
            .AppendLine("   , G.check_way --方法")
            '.AppendLine("   , E.measure_value1 --实测值1")
            '.AppendLine("   , E.measure_value2 --实测值2")



            If cn.IndexOf("AvoidMiss_Experiment") > 0 Then
                .Append("	   ,CASE WHEN E.measure_value1 = 'NaN' THEN '' ELSE ISNULL(E.measure_value1,'') END measure_value1 --实测值1  " & vbCrLf)
                .Append("	   ,CASE WHEN E.measure_value2 = 'NaN' THEN '' ELSE ISNULL(E.measure_value2,'') END measure_value2 --实测值2  " & vbCrLf)
                .Append("	   ,CASE WHEN E.measure_value3 = 'NaN' THEN '' ELSE ISNULL(E.measure_value3,'') END measure_value3 --实测值3  " & vbCrLf)
                .Append("	   ,CASE WHEN E.measure_value4 = 'NaN' THEN '' ELSE ISNULL(E.measure_value4,'') END measure_value4 --实测值4  " & vbCrLf)
                .Append("	   ,CASE WHEN E.measure_value5 = 'NaN' THEN '' ELSE ISNULL(E.measure_value5,'') END measure_value5 --实测值5  " & vbCrLf)
                .Append("	   ,CASE WHEN E.measure_value6 = 'NaN' THEN '' ELSE ISNULL(E.measure_value6,'') END measure_value6 --实测值6  " & vbCrLf)

            Else
                .Append("	   ,CASE WHEN E.measure_value1 = 'NaN' THEN '' ELSE ISNULL(E.measure_value1,'') END measure_value1 --实测值1  " & vbCrLf)
                .Append("	   ,CASE WHEN E.measure_value2 = 'NaN' THEN '' ELSE ISNULL(E.measure_value2,'') END measure_value2 --实测值2  " & vbCrLf)
            End If
            .AppendLine("   , E.result --检查结果")
            .AppendLine("   , E.remarks --备注")
            .AppendLine("   , A.qianpin_flg                                        AS Qianpin --欠品")
            .AppendLine("   , CONVERT(VARCHAR(100), A.start_time, 120)             AS start_time --检查开始时间")
            .AppendLine("   , CONVERT(VARCHAR(100), A.end_time, 120)               AS end_time --检查终了时间")
            .AppendLine("   , Datediff(ss, A.start_time, A.end_time)               AS Check_Time --检查时长")
            .AppendLine("   , A.shareResult_id --检查种类 ")
            .AppendLine("FROM   t_check_result A WITH(READCOMMITTED) --检查结果表")
            .AppendLine("       LEFT JOIN m_goods B")
            .AppendLine("              ON A.goods_id = B.id")
            .AppendLine("       LEFT JOIN TB_CompleteData C WITH(READCOMMITTED)")
            .AppendLine("              ON A.make_number = C.MakeNumber")
            .AppendLine("                 AND B.goods_cd = C.Code")
            .AppendLine("       LEFT JOIN TB_User D WITH(READCOMMITTED)")
            .AppendLine("              ON D.UserCode = A.check_user")
            .AppendLine("       LEFT JOIN t_result_detail E WITH(READCOMMITTED) --检查结果详细表")
            .AppendLine("              ON A.id = E.result_id")
            .AppendLine("       LEFT JOIN m_tools F WITH(READCOMMITTED) --治具表")
            .AppendLine("              ON E.check_id = F.id")
            .AppendLine("       LEFT JOIN m_check G WITH(READCOMMITTED) --检查表(基础表)")
            .AppendLine("              ON E.check_id = G.id")
            .AppendLine("       LEFT JOIN m_classify H WITH(READCOMMITTED) --分类表")
            .AppendLine("              ON G.classify_id = H.id")
            .AppendLine("       LEFT JOIN m_picture I WITH(READCOMMITTED) --图片表")
            .AppendLine("              ON H.picture_id = I.id")
            .AppendLine("                 AND I.delete_flg = 0")

            .AppendLine("       LEFT JOIN t_dlx_chk J WITH(READCOMMITTED) --图片表")
            .AppendLine("              ON A.id = J.id")


        End With

        'sqlstr.AppendLine(" WHERE A.continue_chk_flg<>'3' ")
        sqlstr.AppendLine(" WHERE 1=1 ")

        If where.Replace(",", "").Trim <> "" Then
            Dim cols As New Generic.List(Of String)
            cols.Add("A.id")
            cols.Add("J.department_cd ")
            cols.Add("J.line_name ")
            cols.Add("B.goods_name")
            cols.Add("B.goods_cd")
            cols.Add("A.make_number")
            cols.Add("C.Finish_Date")
            cols.Add("C.ProductionQuantity")
            cols.Add("C.Product_Line")
            cols.Add("C.Direction")
            cols.Add("C.Pay_Date")
            cols.Add("A.end_time")
            cols.Add("D.UserName")
            cols.Add("F.tools_no")
            cols.Add("H.picture_id")
            cols.Add("H.classify_name")
            cols.Add("G.classify_order")
            cols.Add("G.kind_cd")
            cols.Add("G.check_position")
            cols.Add("G.check_item")
            cols.Add("G.benchmark_type")
            cols.Add("G.benchmark_value1")
            cols.Add("G.benchmark_value2")
            cols.Add("G.benchmark_value3")
            cols.Add("G.check_times")
            cols.Add("G.check_way")
            'cols.Add("E.measure_value1")
            'cols.Add("E.measure_value2")
            Dim cn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

            If cn.IndexOf("AvoidMiss_Experiment") > 0 Then
                cols.Add("E.measure_value1" & vbCrLf)
                cols.Add("E.measure_value2" & vbCrLf)
                cols.Add("E.measure_value3" & vbCrLf)
                cols.Add("E.measure_value4" & vbCrLf)
                cols.Add("E.measure_value5" & vbCrLf)
                cols.Add("E.measure_value6" & vbCrLf)

            Else
                cols.Add("E.measure_value1" & vbCrLf)
                cols.Add("E.measure_value2" & vbCrLf)
            End If
            cols.Add("E.result")
            cols.Add("E.remarks")
            cols.Add("A.qianpin_flg")
            cols.Add("A.start_time")
            cols.Add("A.end_time")
            cols.Add("A.shareResult_id")

            Dim sqlValue() As String = where.Split(","c)

            sqlstr.AppendLine(GetWhereStrSql(cols, sqlValue))
        End If

        sqlstr.Append("ORDER  BY A.id,B.goods_name,G.ID")

        '検索の実行
        Dim tableName As String = "mauthority"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sqlstr.ToString, ds, tableName, paramList.ToArray)
        Return ds

        'Dim dt As DataTable = ds.Tables(0)




    End Function

    ''' <summary>
    ''' 组成Where条件
    ''' </summary>
    ''' <param name="col"></param>
    ''' <param name="ws"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetWhereStrSql(ByVal col As Generic.List(Of String), ByVal ws As String()) As String
        Dim sqlstr As New System.Text.StringBuilder
        sqlstr.AppendLine(" and (1=1")

        For i As Integer = 0 To ws.Length - 1
            If ws(i) <> "" Then
                Try
                    Dim str As String = JoinFlg & " " & GetSqlWhere(col(i), ws(i))
                    sqlstr.AppendLine(str)
                Catch ex As Exception
                End Try
            End If
        Next
        sqlstr.AppendLine(")")
        Return sqlstr.ToString
    End Function

    ''' <summary>
    ''' 特殊的列 生成=条件
    ''' </summary>
    ''' <param name="col"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetSqlWhere(ByVal col As String, ByVal value As String) As String

        If value.ToLower = "null" Then
            Return "(" & col & " is null or cast(" & col & " as nvarchar)=''" & ")"
        End If

        If col.ToLower.Contains("productionquantity") Then
            Return "isnull(" & col & ",0)" & " = (N'" & value & "')"
        End If

        If col.ToLower.Contains("name") Or col.ToLower.Contains("_nm") Or col.ToLower.Contains("product_line") _
        Or col.ToLower.Contains("check_position") _
        Or col.ToLower.Contains("check_item") _
        Or col.ToLower.Contains("check_way") _
        Or col.ToLower.Contains("remarks") Then
            Return col & " like (N'%" & value & "%')"
        ElseIf col.ToLower.Contains("check_times") Then
            Return col & " = (N'" & value & "')"
        ElseIf col.ToLower.Contains("_date") Or col.ToLower.Contains("_time") Then

            If value.Contains("#") Then
                Return "(CONVERT(varchar(100), convert(datetime," & col & "), 112) >= '" & CDate(value.Split("#"c)(0)).ToString("yyyyMMdd") & "' and CONVERT(varchar(100), " & col & ", 112) <= '" & CDate(value.Split("#"c)(1)).ToString("yyyyMMdd") & "')"
            Else
                Return "CONVERT(varchar(100),convert(datetime, " & col & "), 112) = '" & CDate(value).ToString("yyyyMMdd") & "'"
            End If

            'Return col & " like (N'%" & value & "%')"
        Else
            Return col & " = (N'" & value & "')"
        End If

    End Function



    Public Function GetPictureContent(ByVal id As String) As Object
        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        Dim strSql As New System.Text.StringBuilder

        strSql.AppendLine(" SELECT DISTINCT")
        strSql.AppendLine("     m_picture.picture_content AS picture_content  --图片二进制")
        strSql.AppendLine(" FROM")
        strSql.AppendLine("     m_picture WITH(READCOMMITTED)")

        strSql.Append(" WHERE id='" & id & "'")
        '検索の実行
        Dim tableName As String = "mpicuture"
        FillDataset(DataAccessManager.Connection, CommandType.Text, strSql.ToString, ds, tableName, paramList.ToArray)

        Try
            Return ds.Tables(0).Rows(0).Item(0)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
