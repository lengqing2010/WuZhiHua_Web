Imports System.Text
Imports System.Data.SqlClient
Imports System.Reflection.MethodBase
Imports Itis.ApplicationBlocks.Data.SQLHelper
Imports Itis.ApplicationBlocks.ExceptionManagement.UnTrappedExceptionManager
Imports Lixil.AvoidMissSystem.Utilities

Public Class ShouInputCheckResultDA

    ''' <summary>
    ''' 根据检索条件取得信息
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSearchData(ByVal ID As String, ByVal syainCd As String, ByVal result As String, ByVal makenumber As String) As DataSet

        Dim whereFlg As Integer = 0
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.Append("SELECT distinct '" & ID & "'                       AS ID, " & vbCrLf)
        sb.Append("       tb_completedata.id         AS '商品ID', " & vbCrLf)
        sb.Append("       tb_completedata.Code         AS '商品CD', " & vbCrLf)
        sb.Append("       tb_completedata.makenumber AS '商品作番', " & vbCrLf)
        sb.Append("       tb_completedata.Product_Line AS '生产线汉字', " & vbCrLf)
        sb.Append("       tb_completedata.ProductionQuantity AS 'ProductionQuantity', " & vbCrLf)
        sb.Append("       tb_completedata.Finish_Date AS 'Finish_Date', " & vbCrLf)
        sb.Append("       tb_completedata.Pay_Date AS 'Pay_Date', " & vbCrLf)
        sb.Append("       tb_completedata.Direction AS 'Direction', " & vbCrLf)
        sb.Append("       m_check.department_cd      AS '生产线' --取不到怎么办 " & vbCrLf)
        sb.Append("       , " & vbCrLf)
        sb.Append("       '" & syainCd & "'                      AS '检查员', " & vbCrLf)
        sb.Append("       Getdate()                  AS '开始时间', " & vbCrLf)
        sb.Append("       Getdate()                  AS '结束时间', " & vbCrLf)
        sb.Append("       NULL                       AS up_check_user, " & vbCrLf)
        sb.Append("       NULL                       AS up_start_time, " & vbCrLf)
        sb.Append("       NULL                       AS up_endtime, " & vbCrLf)
        sb.Append("       '" & result & "'                      AS '检查结果', " & vbCrLf)
        sb.Append("       ''                         AS remarks, " & vbCrLf)
        sb.Append("       1                          AS check_times, " & vbCrLf)
        sb.Append("       '" & ID & "'                       AS ID, " & vbCrLf)
        sb.Append("       0, " & vbCrLf)
        sb.Append("       0, " & vbCrLf)
        sb.Append("       0, " & vbCrLf)
        sb.Append("       '" & syainCd & "'                      AS '检查员', " & vbCrLf)
        sb.Append("       Getdate(), " & vbCrLf)
        sb.Append("       '" & syainCd & "'                      AS '检查员', " & vbCrLf)
        sb.Append("       Getdate(), " & vbCrLf)
        sb.Append("       TB_User.UserName                 AS '检查员名' " & vbCrLf)
        sb.Append("FROM   tb_completedata WITH(readcommitted) " & vbCrLf)
        sb.Append("       LEFT JOIN m_goods WITH(readcommitted) " & vbCrLf)
        sb.Append("              ON tb_completedata.code = m_goods.goods_cd " & vbCrLf)
        sb.Append("       LEFT JOIN m_classify WITH(readcommitted) " & vbCrLf)
        sb.Append("              ON m_classify.goods_id = m_goods.id " & vbCrLf)
        sb.Append("       LEFT JOIN m_kbn WITH(readcommitted) " & vbCrLf)
        sb.Append("              ON m_kbn.mei_cd = m_classify.department_cd " & vbCrLf)
        sb.Append("                 AND m_kbn.mei_kbn = '0004' " & vbCrLf)
        sb.Append("       LEFT JOIN m_check " & vbCrLf)
        sb.Append("              ON m_goods.id = m_check.goods_id " & vbCrLf)
        sb.AppendLine("LEFT JOIN TB_User WITH(READCOMMITTED)  ")
        sb.AppendLine("ON TB_User.UserCode = '" & syainCd & "'  ")

        sb.Append("WHERE  makenumber = '" & makenumber & "'")
        'パラメータ設定
        Dim ds As New DataSet


        '検索の実行
        Dim tableName As String = "SearchData"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds
    End Function

    Public Function Gett_check_result(ByVal makenumber As String) As DataTable

        Dim whereFlg As Integer = 0
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.Append("select  * from t_check_result " & vbCrLf)
        sb.Append("WHERE  make_number = '" & makenumber & "'")
        'パラメータ設定
        Dim ds As New DataSet


        '検索の実行
        Dim tableName As String = "SearchData"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds.Tables(0)
    End Function



    Public Function InsData(ByVal ID As String, ByVal syainCd As String, ByVal result As String, ByVal makenumber As String) As Boolean

        Dim whereFlg As Integer = 0
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)
        sb.Append("INSERT INTO t_check_result" & vbCrLf)
        sb.Append("SELECT distinct '" & ID & "'                       AS ID, " & vbCrLf)
        sb.Append("       tb_completedata.id         AS '商品CD', " & vbCrLf)
        sb.Append("       tb_completedata.makenumber AS '商品作番', " & vbCrLf)
        sb.Append("       isnull(m_check.department_cd,-999)       AS '生产线' --取不到怎么办 " & vbCrLf)
        sb.Append("       , " & vbCrLf)
        sb.Append("       '" & syainCd & "'                      AS '检查员', " & vbCrLf)
        sb.Append("       Getdate()                  AS '开始时间', " & vbCrLf)
        sb.Append("       Getdate()                  AS '结束时间', " & vbCrLf)
        sb.Append("       NULL                       AS up_check_user, " & vbCrLf)
        sb.Append("       NULL                       AS up_start_time, " & vbCrLf)
        sb.Append("       NULL                       AS up_endtime, " & vbCrLf)
        sb.Append("       '" & result & "'                      AS '检查结果', " & vbCrLf)
        sb.Append("       ''                         AS remarks, " & vbCrLf)
        sb.Append("       1                          AS check_times, " & vbCrLf)
        sb.Append("       '手入力'                       AS ID, " & vbCrLf)
        sb.Append("       0, " & vbCrLf)
        sb.Append("       0, " & vbCrLf)
        sb.Append("       0, " & vbCrLf)
        sb.Append("       '" & syainCd & "'                      AS '检查员', " & vbCrLf)
        sb.Append("       Getdate(), " & vbCrLf)
        sb.Append("       '" & syainCd & "'                      AS '检查员', " & vbCrLf)
        sb.Append("       Getdate()" & vbCrLf)
        'sb.Append("       TB_User.UserName                 AS '检查员名' " & vbCrLf)
        sb.Append("FROM   tb_completedata WITH(readcommitted) " & vbCrLf)
        sb.Append("       LEFT JOIN m_goods WITH(readcommitted) " & vbCrLf)
        sb.Append("              ON tb_completedata.code = m_goods.goods_cd " & vbCrLf)
        sb.Append("       LEFT JOIN m_classify WITH(readcommitted) " & vbCrLf)
        sb.Append("              ON m_classify.goods_id = m_goods.id " & vbCrLf)
        sb.Append("       LEFT JOIN m_kbn WITH(readcommitted) " & vbCrLf)
        sb.Append("              ON m_kbn.mei_cd = m_classify.department_cd " & vbCrLf)
        sb.Append("                 AND m_kbn.mei_kbn = '0004' " & vbCrLf)
        sb.Append("       LEFT JOIN m_check " & vbCrLf)
        sb.Append("              ON m_goods.id = m_check.goods_id " & vbCrLf)
        sb.AppendLine("LEFT JOIN TB_User WITH(READCOMMITTED)  ")
        sb.AppendLine("ON TB_User.UserCode = '" & syainCd & "'  ")

        sb.Append("WHERE  makenumber = '" & makenumber & "'")

        '挿入の実行
        ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

        Return True

    End Function



    Public Function InsDataZhijie(ByVal ID As String, ByVal syouhincd As String, ByVal syainCd As String, ByVal result As String, ByVal makenumber As String, ByVal line As String) As Boolean

        Dim whereFlg As Integer = 0
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)
        sb.Append("INSERT INTO t_check_result" & vbCrLf)
        sb.Append("SELECT top 1 '" & ID & "'                       AS ID, " & vbCrLf)
        sb.Append("       '" & syouhincd & "'         AS '商品CD', " & vbCrLf)
        sb.Append("       '" & makenumber & "' AS '商品作番', " & vbCrLf)
        sb.Append("       '" & line & "'       AS '生产线' --取不到怎么办 " & vbCrLf)
        sb.Append("       , " & vbCrLf)
        sb.Append("       '" & syainCd & "'                      AS '检查员', " & vbCrLf)
        sb.Append("       Getdate()                  AS '开始时间', " & vbCrLf)
        sb.Append("       Getdate()                  AS '结束时间', " & vbCrLf)
        sb.Append("       NULL                       AS up_check_user, " & vbCrLf)
        sb.Append("       NULL                       AS up_start_time, " & vbCrLf)
        sb.Append("       NULL                       AS up_endtime, " & vbCrLf)
        sb.Append("       '" & result & "'                      AS '检查结果', " & vbCrLf)
        sb.Append("       ''                         AS remarks, " & vbCrLf)
        sb.Append("       1                          AS check_times, " & vbCrLf)
        sb.Append("       '手入力'                       AS ID, " & vbCrLf)
        sb.Append("       0, " & vbCrLf)
        sb.Append("       0, " & vbCrLf)
        sb.Append("       0, " & vbCrLf)
        sb.Append("       '" & syainCd & "'                      AS '检查员', " & vbCrLf)
        sb.Append("       Getdate(), " & vbCrLf)
        sb.Append("       '" & syainCd & "'                      AS '检查员', " & vbCrLf)
        sb.Append("       Getdate()" & vbCrLf)
        'sb.Append("       TB_User.UserName                 AS '检查员名' " & vbCrLf)
        sb.Append("FROM    m_check " & vbCrLf)

        '挿入の実行
        ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

        Return True

    End Function



    Public Function GetGoodID(ByVal GoodCd As String) As String

        Dim whereFlg As Integer = 0
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.Append("select id from m_goods " & vbCrLf)
        sb.Append("WHERE  goods_cd = '" & GoodCd & "'")
        'パラメータ設定
        Dim ds As New DataSet


        '検索の実行
        Dim tableName As String = "SearchData"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds.Tables(0).Rows(0).Item(0).ToString

    End Function
End Class
