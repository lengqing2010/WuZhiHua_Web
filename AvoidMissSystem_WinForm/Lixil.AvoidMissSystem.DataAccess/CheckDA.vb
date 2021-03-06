Imports System.Text
Imports System.Data.SqlClient
Imports System.Reflection.MethodBase
Imports Itis.ApplicationBlocks.Data.SQLHelper
Imports Itis.ApplicationBlocks.ExceptionManagement.UnTrappedExceptionManager
Imports Lixil.AvoidMissSystem.Utilities

''' <summary>
''' 现场检查用
''' </summary>
''' <remarks></remarks>
Public Class CheckDA

#Region "检查主页面用"

#Region "数据取得"


    ''' <summary>
    ''' 分类列表取得s
    ''' </summary>
    ''' <param name="strGoodsId"></param>
    ''' <param name="strToolsCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClassifyList(ByVal strGoodsId As String, Optional ByVal strToolsCd As String = "") As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, strGoodsId, strToolsCd)
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)
        If strToolsCd.Equals("") Then
            '外观寸法分类取得
            sb.AppendLine(" SELECT * from (")
            sb.AppendLine(" SELECT ")
            sb.AppendLine(" DISTINCT(m_check.classify_id) ")
            sb.AppendLine(" , ISNULL(m_classify.classify_name, '') AS classify_name ")
            sb.AppendLine(" , ISNULL(m_classify.picture_id, '') AS picture_id ")
            sb.AppendLine(" , ISNULL(m_classify.disp_no, '') AS disp_no ")
            sb.AppendLine(" FROM m_check WITH(READCOMMITTED) ")
            sb.AppendLine(" INNER JOIN m_classify WITH(READCOMMITTED) ")
            sb.AppendLine(" ON m_check.classify_id = m_classify.id ")
            sb.AppendLine(" AND m_classify.delete_flg = @delete_flg ")
            sb.AppendLine(" WHERE m_check.goods_id = @goods_id ")
            sb.AppendLine(" AND m_check.kind_cd='01' ")
            sb.AppendLine(" AND m_check.delete_flg= @delete_flg ) a")
            sb.AppendLine(" order by Convert(int, a.disp_no) ")

        Else
            '治具分类列表取得
            sb.AppendLine(" SELECT * from (")
            sb.AppendLine(" SELECT ")
            sb.AppendLine(" DISTINCT(m_check.classify_id) ")
            sb.AppendLine(" ,ISNULL(m_classify.classify_name, '') AS classify_name ")
            sb.AppendLine(" ,ISNULL(m_classify.picture_id, '') AS picture_id ")
            sb.AppendLine(" ,ISNULL(m_classify.disp_no, '') AS disp_no ")
            sb.AppendLine(" FROM m_check WITH(READCOMMITTED) ")
            sb.AppendLine(" INNER JOIN m_classify WITH(READCOMMITTED) ")
            sb.AppendLine(" ON m_check.classify_id = m_classify.id ")
            sb.AppendLine(" AND m_classify.delete_flg = @delete_flg ")
            sb.AppendLine(" WHERE m_check.goods_id = @goods_id ")
            sb.AppendLine(" AND m_check.kind_cd='02' ")
            sb.AppendLine(" AND m_check.tools_id = @tools_id ")
            sb.AppendLine(" AND m_check.delete_flg= @delete_flg ) a")

            sb.AppendLine(" order by Convert(int, a.disp_no) ")

            paramList.Add(MakeParam("@tools_id", SqlDbType.Char, 7, strToolsCd))
        End If
        paramList.Add(MakeParam("@goods_id", SqlDbType.Char, 7, strGoodsId))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, Consts.UNDELETED))
        'パラメータ設定
        Dim ds As New DataSet


        '検索の実行
        Dim tableName As String = "classifyList"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds

    End Function



    ''' <summary>
    ''' 治具列表的取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckToolsOK(ByVal result_id As String, ByVal tools_id As String) As Integer

        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        With sb
            .AppendLine(" select distinct isnull(t_result_detail.result,'NG') as result  from t_result_detail")
            .AppendLine(" left join m_check")
            .AppendLine(" on t_result_detail.check_id = m_check.id ")
            .AppendLine(" and t_result_detail.result_id='" & result_id & "'")
            .AppendLine(" where m_check.tools_id = '" & tools_id & "'")
            .AppendLine(" AND m_check.kind_cd = '02' ")
            .AppendLine(" AND m_check.delete_flg = '0' ")

        End With

        'パラメータ設定
        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "CheckToolsOK"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        If ds.Tables(0).Rows.Count = 0 Then
            Return 0 '没有数据
        ElseIf ds.Tables(0).Select("result <> 'OK'").Length > 0 Then
            Return 0 '
        End If

        Return 1 '都是OK
    End Function

    ''' <summary>
    ''' 治具列表的取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckToolsHaveData(ByVal result_id As String, ByVal tools_id As String) As Boolean

        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        With sb
            .AppendLine(" select distinct isnull(t_result_detail.result,'NG') as result  from t_result_detail")
            .AppendLine(" left join m_check")
            .AppendLine(" on t_result_detail.check_id = m_check.id ")
            .AppendLine(" and t_result_detail.result_id='" & result_id & "'")
            .AppendLine(" where m_check.tools_id = '" & tools_id & "'")
            .AppendLine(" AND m_check.kind_cd = '02' ")
            .AppendLine(" AND m_check.delete_flg = '0' ")

        End With

        'パラメータ設定
        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "CheckToolsOK"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        If ds.Tables(0).Rows.Count = 0 Then
            Return False
        Else
            Return True
        End If

    End Function



    ''' <summary>
    ''' 治具列表的取得
    ''' </summary>
    ''' <param name="strGoodsId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetToolsNoList(ByVal strGoodsId As String) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, strGoodsId)
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" DISTINCT( m_check.tools_id ) ")
        sb.AppendLine(" , ISNULL(m_tools.tools_no, '') AS tools_no ")
        sb.AppendLine(" , ISNULL(m_check.tools_order, '') AS tools_order ")
        sb.AppendLine(" , ISNULL(m_tools.barcode_flg, '') AS barcode_flg ")
        sb.AppendLine(" , ISNULL(m_tools.barcode, '') AS barcode ")
        sb.AppendLine(" , ISNULL(m_tools.remarks, '') AS remarks ")
        sb.AppendLine(" FROM ")
        sb.AppendLine(" m_check WITH(READCOMMITTED) ")
        sb.AppendLine(" INNER JOIN m_tools WITH(READCOMMITTED) ")
        sb.AppendLine(" ON m_check.tools_id = m_tools.id ")
        sb.AppendLine(" AND m_tools.delete_flg = @delete_flg ")
        sb.AppendLine(" WHERE m_check.goods_id = @goods_id ")
        sb.AppendLine(" AND m_check.kind_cd = '02' ")
        sb.AppendLine(" AND m_check.delete_flg = @delete_flg ")
        sb.AppendLine(" ORDER BY m_check.tools_order ")

        paramList.Add(MakeParam("@goods_id", SqlDbType.Char, 7, strGoodsId))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, Consts.UNDELETED))
        'パラメータ設定
        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "ToolsList"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds
    End Function
    ''' <summary>
    ''' 当天该商品code的最新检查结果取得
    ''' </summary>
    ''' <param name="strGoodsCode"></param>
    ''' <param name="strMakeNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLastReultInfo(ByVal strGoodsCode As String, ByVal strMakeNo As String) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, strGoodsCode, strMakeNo)

        '因为商品表的中的商品CD为带“-”的,而在生产表中是不带“-”。所以要对商品CD加处理
        Dim strPGoodCd As String = Replace(strGoodsCode, "-", "")
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)



        sb.AppendLine("declare @s varchar(30)")
        sb.AppendLine("declare @e varchar(30)")

        sb.AppendLine("declare @p nvarchar(4000)")

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" @s=(SUBSTRING(CONVERT(CHAR(19), Finish_Date, 120),1,10) + ' 08:00:00.000') ")
        sb.AppendLine(" FROM TB_CompleteData WITH(READCOMMITTED) ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" Code = @Pgoods_cd ")
        sb.AppendLine(" AND MakeNumber = @MakeNumber  ")


        sb.AppendLine(" SELECT ")
        sb.AppendLine(" @e=(SUBSTRING(CONVERT(CHAR(19), DATEADD(DAY,1,Finish_Date), 120),1,10) + ' 07:59:59.999') ")
        sb.AppendLine(" FROM TB_CompleteData WITH(READCOMMITTED) ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" Code = @Pgoods_cd ")
        sb.AppendLine(" AND MakeNumber = @MakeNumber  ")


        sb.AppendLine("SELECT @p=Product_Line")
        sb.AppendLine("FROM TB_CompleteData WITH(READCOMMITTED) ")
        sb.AppendLine("WHERE Code=@Pgoods_cd ")
        sb.AppendLine("AND MakeNumber=@MakeNumber")
   

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" A.id AS 'ID' ")
        sb.AppendLine(" , B.goods_cd AS '商品CD' ")
        sb.AppendLine(" , A.make_number AS '作番'")
        sb.AppendLine(" , D.Product_Line AS '生产线'")
        sb.AppendLine(" , D.Finish_Date  AS '生产实际日'")
        sb.AppendLine(" , A.result AS '结果' ")
        sb.AppendLine(" , CASE A.continue_chk_flg WHEN '0' THEN '完了' WHEN '1' THEN '待判' WHEN '2' THEN '默认' END AS '状态' ")
        sb.AppendLine(" , C.UserName AS '检查员' ")
        sb.AppendLine(" , A.start_time AS '开始时间' ")
        sb.AppendLine(" , A.end_time AS '结束时间' ")
        sb.AppendLine(" , A.shareResult_id AS '继承结果'")
        sb.AppendLine(" , A.check_user AS '检查员CD'")

        sb.AppendLine("FROM t_check_result AS A  WITH(READCOMMITTED)")
        sb.AppendLine("LEFT JOIN m_goods AS B WITH(READCOMMITTED)")
        sb.AppendLine("ON B.id = A.goods_id")
        sb.AppendLine("AND B.delete_flg = @delete_flg ")
        sb.AppendLine("LEFT JOIN TB_User AS C WITH(READCOMMITTED)  ")
        sb.AppendLine("ON A.check_user = C.UserCode  ")
        sb.AppendLine("LEFT JOIN TB_CompleteData AS D WITH(READCOMMITTED)  ")
        sb.AppendLine("ON A.make_number = D.MakeNumber")
        sb.AppendLine("AND D.Code = @Pgoods_cd")
        sb.AppendLine("WHERE A.delete_flg = @delete_flg ")
        'sb.AppendLine("AND (B.goods_cd+A.make_number)")
        ' sb.AppendLine("AND (replace(cast(B.goods_cd as varchar(30)),'-','')+A.make_number)")

        sb.AppendLine("AND (replace(cast(B.goods_cd as varchar(30)),'-',''))")
        sb.AppendLine("IN  (")
        sb.AppendLine("SELECT Code")
        sb.AppendLine("FROM TB_CompleteData WITH(READCOMMITTED)")
        sb.AppendLine("WHERE Product_Line = @p")
        sb.AppendLine("AND Finish_Date BETWEEN @s AND @e")
        sb.AppendLine("AND Code = @Pgoods_cd ")
        sb.AppendLine(" )")
        sb.AppendLine(" AND continue_chk_flg <> 3 ")
        sb.AppendLine(" AND Product_Line = @p ")
        sb.AppendLine(" ORDER BY A.end_time DESC ")

        'パラメータ設定
        paramList.Add(MakeParam("@goods_cd", SqlDbType.VarChar, 30, strGoodsCode))
        paramList.Add(MakeParam("@Pgoods_cd", SqlDbType.VarChar, 30, strPGoodCd))
        paramList.Add(MakeParam("@MakeNumber", SqlDbType.VarChar, 50, strMakeNo))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, Consts.UNDELETED))
        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "ResultInfo"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds
    End Function



    ''' <summary>
    ''' 当天该商品code的最新检查结果取得
    ''' </summary>
    ''' <param name="strGoodsCode"></param>
    ''' <param name="strMakeNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLastReultInfoFromt_check_result(ByVal strGoodsCode As String, ByVal strMakeNo As String) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, strGoodsCode, strMakeNo)

        '因为商品表的中的商品CD为带“-”的,而在生产表中是不带“-”。所以要对商品CD加处理
        Dim strPGoodCd As String = Replace(strGoodsCode, "-", "")
        Dim paramList As New List(Of SqlParameter)

        Dim varname1 As New System.Text.StringBuilder
        varname1.Append("SELECT " & vbCrLf)
        varname1.Append(" A.id AS 'ID' " & vbCrLf)
        varname1.Append(" , B.goods_cd AS '商品CD' " & vbCrLf)
        varname1.Append(" , A.make_number AS '作番' " & vbCrLf)
        varname1.Append(" , '' AS '生产线' " & vbCrLf)
        varname1.Append(" , ''  AS '生产实际日' " & vbCrLf)
        varname1.Append(" , A.result AS '结果' " & vbCrLf)
        varname1.Append(" , CASE A.continue_chk_flg WHEN '0' THEN '完了' WHEN '1' THEN '待判' WHEN '2' THEN '默认' END AS '状态' " & vbCrLf)
        varname1.Append(" , C.UserName AS '检查员' " & vbCrLf)
        varname1.Append(" , A.start_time AS '开始时间' " & vbCrLf)
        varname1.Append(" , A.end_time AS '结束时间' " & vbCrLf)
        varname1.Append(" , A.shareResult_id AS '继承结果' " & vbCrLf)
        varname1.Append(" , A.check_user AS '检查员CD'")
        varname1.Append("FROM t_check_result AS A  WITH(READCOMMITTED) " & vbCrLf)
        varname1.Append("LEFT JOIN m_goods AS B WITH(READCOMMITTED) " & vbCrLf)
        varname1.Append("ON B.id = A.goods_id " & vbCrLf)
        varname1.Append("AND B.delete_flg = '0' " & vbCrLf)
        varname1.Append("LEFT JOIN TB_User AS C WITH(READCOMMITTED) " & vbCrLf)
        varname1.Append("ON A.check_user = C.UserCode " & vbCrLf)
        varname1.Append(" " & vbCrLf)
        varname1.Append("WHERE A.delete_flg = '0' " & vbCrLf)
        varname1.Append("and B.goods_cd = '" & strGoodsCode & "' " & vbCrLf)
        varname1.Append("and A.make_number = '" & strMakeNo & "' " & vbCrLf)
        varname1.Append("AND continue_chk_flg <> 3 " & vbCrLf)
        varname1.Append("ORDER BY A.end_time DESC")


        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "ResultInfo"
        FillDataset(DataAccessManager.Connection, CommandType.Text, varname1.ToString(), ds, tableName, paramList.ToArray)
        Return ds
    End Function


    ''' <summary>
    ''' 获取部门信息
    ''' </summary>
    ''' <param name="strGoodsCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDepartmentInfo(ByVal strGoodsCd As String) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, strGoodsCd)
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" DISTINCT(m_check.department_cd) AS department_cd ")
        sb.AppendLine(" FROM ")
        sb.AppendLine(" m_goods WITH(READCOMMITTED)")
        sb.AppendLine(" INNER JOIN ")
        sb.AppendLine(" m_check WITH(READCOMMITTED)")
        sb.AppendLine(" ON m_goods.id = m_check.goods_id ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" m_goods.goods_cd = @goods_cd ")
        sb.AppendLine(" AND m_goods.delete_flg = @delete_flg ")
        'パラメータ設定
        paramList.Add(MakeParam("@goods_cd", SqlDbType.VarChar, 30, strGoodsCd))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, Consts.UNDELETED))
        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "DepartmentInfo"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds
    End Function

    ''' <summary>
    ''' 获取检查次数信息
    ''' </summary>
    ''' <param name="strGoodsId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChkTimesInfo(ByVal strGoodsId As String, ByVal strMakeNo As String) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, strGoodsId, strMakeNo)

        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" COUNT(id) AS check_times ")
        sb.AppendLine(" FROM ")
        sb.AppendLine(" t_check_result WITH(READCOMMITTED) ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" goods_id = @goods_id ")
        sb.AppendLine(" AND make_number = @make_number ")
        sb.AppendLine(" AND continue_chk_flg = '0' ")
        sb.AppendLine(" AND delete_flg = @delete_flg ")

        'パラメータ設定
        paramList.Add(MakeParam("@goods_id", SqlDbType.VarChar, 7, strGoodsId))
        paramList.Add(MakeParam("@make_number", SqlDbType.VarChar, 30, strMakeNo))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, Consts.UNDELETED))
        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "CheckTimesInfo"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds
    End Function
    ''' <summary>
    ''' 继续检查信息获得
    ''' </summary>
    ''' <param name="strGoodsId"></param>
    ''' <param name="strMakeNo"></param>
    ''' <param name="strDateNow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetContinueChkInfo(ByVal strGoodsId As String, ByVal strMakeNo As String, ByVal strDateNow As String) As DataSet
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" id ")
        sb.AppendLine(" ,shareResult_id ")
        sb.AppendLine(" FROM ")
        sb.AppendLine(" t_check_result WITH(READCOMMITTED) ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" goods_id = @goods_id ")
        sb.AppendLine(" AND make_number = @make_number ")
        sb.AppendLine(" AND id  LIKE @strDateNow ")
        sb.AppendLine(" AND continue_chk_flg = '1' ")
        sb.AppendLine(" AND delete_flg = @delete_flg ")

        'パラメータ設定
        paramList.Add(MakeParam("@goods_id", SqlDbType.VarChar, 7, strGoodsId))
        paramList.Add(MakeParam("@make_number", SqlDbType.VarChar, 30, strMakeNo))
        paramList.Add(MakeParam("@strDateNow", SqlDbType.VarChar, 14, strDateNow + "%"))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, Consts.UNDELETED))

        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "ContinueChkInfo"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)

        Return ds
    End Function
    ''' <summary>
    ''' 根据商品code取得商品ID
    ''' </summary>
    ''' <param name="strGoodsCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetGoodsIdByGoodsCd(ByVal strGoodsCd As String) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, strGoodsCd)

        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" id ")
        sb.AppendLine(" FROM ")
        sb.AppendLine(" m_goods WITH(READCOMMITTED) ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" goods_cd = @goods_cd ")

        'パラメータ設定
        paramList.Add(MakeParam("@goods_cd", SqlDbType.VarChar, 30, strGoodsCd))

        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "GoodsId"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)

        Return ds
    End Function

    ''' <summary>
    ''' 该分类下结果取得
    ''' </summary>
    ''' <param name="strGoodsId"></param>
    ''' <param name="strResultId"></param>
    ''' <param name="strClassfyId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChkDetailResult(ByVal strGoodsId As String, ByVal strResultId As String, ByVal strClassfyId As String) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, strResultId, strClassfyId)
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        'sb.AppendLine(" SELECT ")
        'sb.AppendLine(" t_result_detail.check_id ")
        'sb.AppendLine(" FROM ")
        'sb.AppendLine(" t_result_detail WITH(READCOMMITTED) ")
        'sb.AppendLine(" INNER JOIN m_check WITH(READCOMMITTED) ")
        'sb.AppendLine(" ON  t_result_detail.check_id = m_check.id ")
        'sb.AppendLine(" AND m_check.goods_id = @strGoodsId ")
        'sb.AppendLine(" AND m_check.classify_id = @strClassfyId ")
        'sb.AppendLine(" WHERE t_result_detail.result_id = @strResultId ")
        'sb.AppendLine(" AND t_result_detail.result IN ('MD','NG') ")
        'sb.AppendLine(" AND t_result_detail.delete_flg = @delete_flg ")

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" m_check.id  AS id ")
        sb.AppendLine(" , ISNULL( t_result_detail.result,'INIT') AS result ")
        sb.AppendLine(" FROM ")
        sb.AppendLine(" m_check WITH(READCOMMITTED) ")
        sb.AppendLine(" LEFT JOIN t_result_detail WITH(READCOMMITTED) ")
        sb.AppendLine(" ON  t_result_detail.check_id = m_check.id ")
        sb.AppendLine(" AND t_result_detail.result_id = @strResultId ")
        sb.AppendLine(" AND t_result_detail.delete_flg = @delete_flg ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine("  m_check.goods_id = @strGoodsId ")
        sb.AppendLine(" AND m_check.classify_id = @strClassfyId ")
        sb.AppendLine(" AND m_check.delete_flg = @delete_flg ")

        'パラメータ設定
        paramList.Add(MakeParam("@strGoodsId", SqlDbType.VarChar, 7, strGoodsId))
        paramList.Add(MakeParam("@strClassfyId", SqlDbType.VarChar, 8, strClassfyId))
        paramList.Add(MakeParam("@strResultId", SqlDbType.VarChar, 13, strResultId))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, Consts.UNDELETED))
        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "DetailResultInfo"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)

        Return ds
    End Function
    ''' <summary>
    ''' 用户信息取得
    ''' </summary>
    ''' <param name="strUserCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCheckUser(ByVal strUserCd As String) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, strUserCd)
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" ID ")
        sb.AppendLine(" FROM ")
        sb.AppendLine(" TB_User WITH(READCOMMITTED) ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" UserCode = @UserCode ")

        'パラメータ設定
        paramList.Add(MakeParam("@UserCode", SqlDbType.VarChar, 50, strUserCd))
        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "UserMsg"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)

        Return ds
    End Function

#End Region

#Region "插入数据"

    ''' <summary>
    ''' 向检查结果表内插入数据
    ''' </summary>
    ''' <param name="hsInsert"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertResult(ByVal hsInsert As Hashtable) As Integer

        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, hsInsert)
        Dim result As Integer
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)
        sb.AppendLine(" INSERT INTO ")
        sb.AppendLine(" t_check_result (")
        sb.AppendLine(" id ")
        sb.AppendLine(" , goods_id ")
        sb.AppendLine(" , make_number ")
        sb.AppendLine(" , department_cd ")
        sb.AppendLine(" , check_user ")
        sb.AppendLine(" , start_time ")
        sb.AppendLine(" , end_time ")
        sb.AppendLine(" , up_check_user ")
        sb.AppendLine(" , result ")
        sb.AppendLine(" , remarks ")
        sb.AppendLine(" , check_times ")
        sb.AppendLine(" , shareResult_id ")
        sb.AppendLine(" , continue_chk_flg ")
        sb.AppendLine(" , delete_flg ")
        sb.AppendLine(" , insert_user ")
        sb.AppendLine(" , insert_date ")
        sb.AppendLine(" , update_user ")
        sb.AppendLine(" , update_date ")
        sb.AppendLine(" ) ")
        sb.AppendLine(" VALUES( ")
        sb.AppendLine(" @id ")
        sb.AppendLine(" , @goods_id ")
        sb.AppendLine(" , @make_number ")
        sb.AppendLine(" , @department_cd ")
        sb.AppendLine(" , @check_user ")
        sb.AppendLine(" , GETDATE() ")
        sb.AppendLine(" , GETDATE() ")
        sb.AppendLine(" , @up_check_user ")
        sb.AppendLine(" , @result ")
        sb.AppendLine(" , @remarks ")
        sb.AppendLine(" , @check_times ")
        sb.AppendLine(" , @shareResult_id ")
        sb.AppendLine(" , @continue_chk_flg ")
        sb.AppendLine(" , @delete_flg ")
        sb.AppendLine(" , @check_user ")
        sb.AppendLine(" , GETDATE() ")
        sb.AppendLine(" , @update_user ")
        sb.AppendLine(" , GETDATE() ")
        sb.AppendLine(" ) ")

        'パラメータ設定
        paramList.Add(MakeParam("@id", SqlDbType.VarChar, 13, Convert.ToString(hsInsert("id"))))
        paramList.Add(MakeParam("@goods_id", SqlDbType.VarChar, 7, Convert.ToString(hsInsert("goods_id"))))
        paramList.Add(MakeParam("@make_number", SqlDbType.VarChar, 30, Convert.ToString(hsInsert("make_number"))))
        paramList.Add(MakeParam("@department_cd", SqlDbType.Char, 3, Convert.ToString(hsInsert("department_cd"))))
        paramList.Add(MakeParam("@check_user", SqlDbType.VarChar, 30, Convert.ToString(hsInsert("check_user"))))
        paramList.Add(MakeParam("@up_check_user", SqlDbType.VarChar, 30, Convert.ToString(hsInsert("up_check_user"))))
        paramList.Add(MakeParam("@result", SqlDbType.Char, 2, Convert.ToString(hsInsert("result"))))
        paramList.Add(MakeParam("@remarks", SqlDbType.VarChar, 200, Convert.ToString(hsInsert("remarks"))))
        paramList.Add(MakeParam("@shareResult_id", SqlDbType.VarChar, 13, Convert.ToString(hsInsert("shareResult_id"))))
        paramList.Add(MakeParam("@continue_chk_flg", SqlDbType.Char, 1, Convert.ToString(hsInsert("continue_chk_flg"))))
        paramList.Add(MakeParam("@check_times", SqlDbType.Char, 1, Convert.ToString(hsInsert("check_times"))))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, Convert.ToString(hsInsert("delete_flg"))))
        paramList.Add(MakeParam("@update_user", SqlDbType.VarChar, 30, String.Empty))
        '挿入の実行
        result = ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

        Return result
    End Function

#End Region

#Region "更新数据"


    ''' <summary>
    ''' 更新检查结果表
    ''' </summary>
    ''' <param name="hsUpdate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateResult(ByVal hsUpdate As Hashtable) As Integer
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, hsUpdate)
        Dim result As Integer
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        Select Case hsUpdate("UpType").ToString
            Case "INIT"
                '非继续检查情况下的最终保存
                sb.AppendLine(" UPDATE ")
                sb.AppendLine(" t_check_result WITH (UPDLOCK) ")
                sb.AppendLine(" SET ")
                sb.AppendLine(" result = @result ")
                sb.AppendLine(" , check_user = @check_user ")
                sb.AppendLine(" , insert_user = @check_user ")
                sb.AppendLine(" , end_time = GETDATE() ")
                sb.AppendLine(" , continue_chk_flg = @continue_chk_flg ")
                sb.AppendLine(" WHERE ")
                sb.AppendLine(" id = @id ")
                'パラメータ設定
                paramList.Add(MakeParam("@id", SqlDbType.VarChar, 13, Convert.ToString(hsUpdate("id"))))
                paramList.Add(MakeParam("@result", SqlDbType.Char, 2, Convert.ToString(hsUpdate("result"))))
                paramList.Add(MakeParam("@check_user", SqlDbType.VarChar, 30, Convert.ToString(hsUpdate("check_user"))))
                paramList.Add(MakeParam("@continue_chk_flg", SqlDbType.Char, 1, Convert.ToString(hsUpdate("continue_chk_flg"))))
            Case "TEMP"
                '继续检查下的临时保存
                sb.AppendLine(" UPDATE ")
                sb.AppendLine(" t_check_result WITH (UPDLOCK) ")
                sb.AppendLine(" SET ")
                sb.AppendLine(" up_start_time = GETDATE() ")
                sb.AppendLine(" , continue_chk_flg = @continue_chk_flg ")
                sb.AppendLine(" WHERE ")
                sb.AppendLine(" id = @id ")
                paramList.Add(MakeParam("@id", SqlDbType.VarChar, 13, Convert.ToString(hsUpdate("id"))))
                paramList.Add(MakeParam("@continue_chk_flg", SqlDbType.Char, 1, Convert.ToString(hsUpdate("continue_chk_flg"))))
            Case "FINAL"
                '继续检擦下的最终保存
                sb.AppendLine(" UPDATE ")
                sb.AppendLine(" t_check_result WITH (UPDLOCK) ")
                sb.AppendLine(" SET ")
                sb.AppendLine(" result = @result ")
                sb.AppendLine(" , up_check_user = @up_check_user ")
                sb.AppendLine(" , up_end_time = GETDATE() ")
                sb.AppendLine(" , continue_chk_flg = @continue_chk_flg ")
                sb.AppendLine(" WHERE ")
                sb.AppendLine(" id = @id ")
                'パラメータ設定
                paramList.Add(MakeParam("@id", SqlDbType.VarChar, 13, Convert.ToString(hsUpdate("id"))))
                paramList.Add(MakeParam("@result", SqlDbType.Char, 2, Convert.ToString(hsUpdate("result"))))
                paramList.Add(MakeParam("@up_check_user", SqlDbType.VarChar, 30, Convert.ToString(hsUpdate("up_check_user"))))
                paramList.Add(MakeParam("@continue_chk_flg", SqlDbType.Char, 1, Convert.ToString(hsUpdate("continue_chk_flg"))))
            Case Else

        End Select


        '更新の実行
        result = ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

        Return result
    End Function

#End Region

#End Region

#Region "检查详细页面用"

#Region "数据取得"


    ''' <summary>
    ''' 检查项目详细信息取得
    ''' </summary>
    ''' <param name="hsSearch"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChkDetailInfo(ByVal hsSearch As Hashtable) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, hsSearch)

        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" m_check.id ")
        sb.AppendLine(" , m_check.kind ")
        sb.AppendLine(" , m_check.check_position ")
        sb.AppendLine(" , m_check.check_item ")
        sb.AppendLine(" , m_check.benchmark_type ")
        sb.AppendLine(" , m_check.benchmark_value1 ")
        sb.AppendLine(" , m_check.benchmark_value2 ")
        sb.AppendLine(" , m_check.benchmark_value3 ")
        sb.AppendLine(" , m_check.check_way ")
        sb.AppendLine(" , m_check.check_times ")
        sb.AppendLine(" , ISNULL(t_result_detail.measure_value1, '') AS measure_value1 ")
        sb.AppendLine(" , ISNULL(t_result_detail.measure_value2, '') AS measure_value2 ")
        sb.AppendLine(" , ISNULL(t_result_detail.result, '') AS result ")
        sb.AppendLine(" , ISNULL(t_result_detail.dis_no, '1') AS dis_no ")
        sb.AppendLine(" , ISNULL(t_result_detail.remarks, '') AS remarks ")
        sb.AppendLine(" , ISNULL(m_kbn.mei, '') AS mei ")
        sb.AppendLine(" , ISNULL(m_kbn.mei_cd, '') AS mei_cd ")

        sb.AppendLine(" , t_result_detail.benchmark_type as benchmark_type_old")
        sb.AppendLine(" , t_result_detail.benchmark_value1 as benchmark_value1_old")
        sb.AppendLine(" , t_result_detail.benchmark_value2 as benchmark_value2_old")
        sb.AppendLine(" , t_result_detail.benchmark_value3 as benchmark_value3_old")


        sb.AppendLine(" FROM ")
        sb.AppendLine(" m_check WITH(READCOMMITTED) ")
        sb.AppendLine(" LEFT JOIN ")
        sb.AppendLine(" t_result_detail WITH(READCOMMITTED) ")
        sb.AppendLine(" ON m_check.id = t_result_detail.check_id ")
        sb.AppendLine(" AND t_result_detail.result_id  = @result_id ")
        sb.AppendLine(" LEFT JOIN ")
        sb.AppendLine(" m_kbn WITH(READCOMMITTED) ")
        sb.AppendLine(" ON m_check.type_cd = m_kbn.mei_cd ")
        sb.AppendLine(" AND m_kbn.mei_kbn = '0002' ")
        sb.AppendLine(" WHERE  ")
        sb.AppendLine(" m_check.goods_id = @goods_id ")
        sb.AppendLine(" AND m_check.kind_cd = @kind_cd ")
        '种类判断
        If hsSearch("kind_cd").ToString.Equals("02") Then
            sb.AppendLine(" AND m_check.tools_id = @tools_id ")
            paramList.Add(MakeParam("@tools_id", SqlDbType.VarChar, 7, Convert.ToString(hsSearch("tools_id"))))
        End If

        sb.AppendLine(" AND m_check.classify_id = @classify_id ")
        sb.AppendLine(" ORDER BY m_check.type_cd,m_check.id ")

        'パラメータ設定
        paramList.Add(MakeParam("@result_id", SqlDbType.VarChar, 13, Convert.ToString(hsSearch("result_id"))))
        paramList.Add(MakeParam("@goods_id", SqlDbType.VarChar, 7, Convert.ToString(hsSearch("goods_id"))))
        paramList.Add(MakeParam("@kind_cd", SqlDbType.VarChar, 3, Convert.ToString(hsSearch("kind_cd"))))
        paramList.Add(MakeParam("@classify_id", SqlDbType.VarChar, 8, Convert.ToString(hsSearch("classify_id"))))

        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "CheckDetailInfo"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds
    End Function

    ''' <summary>
    ''' 根据图片ID取得图片二进制
    ''' </summary>
    ''' <param name="strPicId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPicInfoById(ByVal strPicId As String) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, strPicId)

        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" picture_content ")
        sb.AppendLine(" FROM ")
        sb.AppendLine("  m_picture WITH(READCOMMITTED) ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" id = @picId ")
        sb.AppendLine(" AND delete_flg = @delete_flg ")

        'パラメータ設定
        paramList.Add(MakeParam("@picId", SqlDbType.Char, 7, strPicId))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, Consts.UNDELETED))

        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "PicInfo"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds

    End Function
    ''' <summary>
    ''' 根据检查ID获取检查结果信息
    ''' </summary>
    ''' <param name="strResultId"></param>
    ''' <param name="strChkId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDetailInfoByChkId(ByVal strResultId As String, ByVal strChkId As String, ByVal strDisno As String) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, strResultId, strChkId, strDisno)

        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" measure_value1 ")
        sb.AppendLine(" , measure_value2 ")
        sb.AppendLine(" , result ")
        sb.AppendLine(" FROM ")
        sb.AppendLine("  t_result_detail WITH(READCOMMITTED) ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" result_id = @result_id ")
        sb.AppendLine(" AND check_id = @check_id ")
        sb.AppendLine(" AND dis_no = @dis_no ")
        sb.AppendLine(" AND delete_flg = @delete_flg ")
        'パラメータ設定
        paramList.Add(MakeParam("@result_id", SqlDbType.VarChar, 13, strResultId))
        paramList.Add(MakeParam("@check_id", SqlDbType.VarChar, 9, strChkId))
        paramList.Add(MakeParam("@dis_no", SqlDbType.VarChar, 2, strDisno))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, Consts.UNDELETED))

        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "DetaileInfo"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds

    End Function
#End Region

#Region "数据插入"

    ''' <summary>
    ''' 向检查结果详细表内插入数据
    ''' </summary>
    ''' <param name="hsInsert"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertResultDetaile(ByVal hsInsert As Hashtable) As Integer

        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, hsInsert)
        Dim result As Integer
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)
        sb.AppendLine(" INSERT INTO ")
        sb.AppendLine(" t_result_detail (")
        sb.AppendLine(" result_id ")
        sb.AppendLine(" , check_id ")
        sb.AppendLine(" , measure_value1 ")
        sb.AppendLine(" , measure_value2 ")
        sb.AppendLine(" , picture_id ")
        sb.AppendLine(" , result ")
        sb.AppendLine(" , dis_no ")
        sb.AppendLine(" , remarks ")
        sb.AppendLine(" , delete_flg ")
        sb.AppendLine(" , benchmark_type ")
        sb.AppendLine(" , benchmark_value1 ")
        sb.AppendLine(" , benchmark_value2 ")
        sb.AppendLine(" , benchmark_value3 ")
        ' sb.AppendLine(" , qianpin_flg ")
        sb.AppendLine(" ) ")
        sb.AppendLine(" VALUES( ")
        sb.AppendLine(" @result_id ")
        sb.AppendLine(" , @check_id ")
        sb.AppendLine(" , @measure_value1 ")
        sb.AppendLine(" , @measure_value2 ")
        sb.AppendLine(" , @picture_id ")
        sb.AppendLine(" , @result ")
        sb.AppendLine(" , @dis_no ")
        sb.AppendLine(" , @remarks ")
        sb.AppendLine(" , @delete_flg ")
        sb.AppendLine(" , @benchmark_type ")
        sb.AppendLine(" , @benchmark_value1 ")
        sb.AppendLine(" , @benchmark_value2 ")
        sb.AppendLine(" , @benchmark_value3 ")
        'sb.AppendLine(" , '0' ")
        sb.AppendLine(" ) ")

        'パラメータ設定
        paramList.Add(MakeParam("@result_id", SqlDbType.VarChar, 13, Convert.ToString(hsInsert("result_id"))))
        paramList.Add(MakeParam("@check_id", SqlDbType.VarChar, 9, Convert.ToString(hsInsert("check_id"))))
        If hsInsert("measure_value1").Equals("NULL") Then
            paramList.Add(MakeParam("@measure_value1", SqlDbType.VarChar, 100, String.Empty))
        Else
            paramList.Add(MakeParam("@measure_value1", SqlDbType.VarChar, 100, Convert.ToString(hsInsert("measure_value1"))))
        End If
        If hsInsert("measure_value2").Equals("NULL") Then
            paramList.Add(MakeParam("@measure_value2", SqlDbType.VarChar, 100, String.Empty))
        Else
            paramList.Add(MakeParam("@measure_value2", SqlDbType.VarChar, 100, Convert.ToString(hsInsert("measure_value2"))))
        End If
        paramList.Add(MakeParam("@result", SqlDbType.Char, 2, Convert.ToString(hsInsert("result"))))
        paramList.Add(MakeParam("@picture_id", SqlDbType.Char, 7, Convert.ToString(hsInsert("picture_id"))))
        paramList.Add(MakeParam("@dis_no", SqlDbType.VarChar, 2, Convert.ToString(hsInsert("dis_no"))))
        paramList.Add(MakeParam("@remarks", SqlDbType.VarChar, 200, Convert.ToString(hsInsert("remarks"))))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, Convert.ToString(hsInsert("delete_flg"))))

        paramList.Add(MakeParam("@benchmark_type", SqlDbType.VarChar, 4, Convert.ToString(hsInsert("benchmark_type"))))
        paramList.Add(MakeParam("@benchmark_value1", SqlDbType.VarChar, 20, Convert.ToString(hsInsert("benchmark_value1"))))
        paramList.Add(MakeParam("@benchmark_value2", SqlDbType.VarChar, 20, Convert.ToString(hsInsert("benchmark_value2"))))
        paramList.Add(MakeParam("@benchmark_value3", SqlDbType.VarChar, 20, Convert.ToString(hsInsert("benchmark_value3"))))

        '挿入の実行
        result = ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

                Return result
    End Function

#End Region

#Region "数据更新"

    ''' <summary>
    ''' 更新检查结果详细表
    ''' </summary>
    ''' <param name="hsUpdate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateResultDetail(ByVal hsUpdate As Hashtable) As Integer
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, hsUpdate)
        Dim result As Integer
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)
        sb.AppendLine(" UPDATE ")
        sb.AppendLine(" t_result_detail WITH (UPDLOCK) ")
        sb.AppendLine(" SET ")
        sb.AppendLine(" result = @result ")
        sb.AppendLine(" , remarks = @remarks ")

        'パラメータ設定
        paramList.Add(MakeParam("@result_id", SqlDbType.VarChar, 13, Convert.ToString(hsUpdate("result_id"))))
        paramList.Add(MakeParam("@check_id", SqlDbType.VarChar, 9, Convert.ToString(hsUpdate("check_id"))))
        If hsUpdate("measure_value1").Equals("NULL") Then
        Else
            sb.AppendLine(" , measure_value1 = @measure_value1  ")
            paramList.Add(MakeParam("@measure_value1", SqlDbType.VarChar, 100, Convert.ToString(hsUpdate("measure_value1"))))
        End If
        If hsUpdate("measure_value2").Equals("NULL") Then
        Else
            sb.AppendLine(" , measure_value2 = @measure_value2 ")
            paramList.Add(MakeParam("@measure_value2", SqlDbType.VarChar, 100, Convert.ToString(hsUpdate("measure_value2"))))
        End If
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" result_id = @result_id ")
        sb.AppendLine(" AND check_id = @check_id ")
        sb.AppendLine(" AND dis_no = @dis_no ")
        paramList.Add(MakeParam("@result", SqlDbType.Char, 2, Convert.ToString(hsUpdate("result"))))
        paramList.Add(MakeParam("@dis_no", SqlDbType.VarChar, 2, Convert.ToString(hsUpdate("dis_no"))))
        paramList.Add(MakeParam("@remarks", SqlDbType.VarChar, 200, Convert.ToString(hsUpdate("remarks"))))
        '更新の実行
        result = ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

        Return result
    End Function


    Public Function Updatet_check_resultQinapinFlg(ByVal id As String, ByVal qianpin_flg As String) As Integer
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name)
        Dim result As Integer
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)


        '非继续检查情况下的最终保存
        sb.AppendLine(" UPDATE ")
        sb.AppendLine(" t_check_result WITH (UPDLOCK) ")
        sb.AppendLine(" SET ")
        sb.AppendLine(" qianpin_flg = @qianpin ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" id = @id ")

        'パラメータ設定
        paramList.Add(MakeParam("@id", SqlDbType.VarChar, 13, id))
        paramList.Add(MakeParam("@qianpin", SqlDbType.Char, 1, qianpin_flg))

        '更新の実行
        result = ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

        Return result
    End Function

    Public Function GetQianpinInfo(ByVal id As String) As Boolean
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" qianpin_flg ")
        sb.AppendLine(" FROM ")
        sb.AppendLine(" t_check_result WITH(READCOMMITTED) ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" id = @id ")


        'パラメータ設定
        paramList.Add(MakeParam("@id", SqlDbType.VarChar, 13, id))

        Dim ds As New DataSet

        '検索の実行
        Dim tableName As String = "GetQianpinInfo"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)

        If ds.Tables(0).Rows.Count > 0 Then

            If ds.Tables(0).Rows(0).Item(0) IsNot DBNull.Value AndAlso ds.Tables(0).Rows(0).Item(0).ToString = "1" Then
                Return False
            Else
                Return True
            End If
        Else
            Return True '没有欠品
        End If

    End Function

#End Region

#End Region

End Class
