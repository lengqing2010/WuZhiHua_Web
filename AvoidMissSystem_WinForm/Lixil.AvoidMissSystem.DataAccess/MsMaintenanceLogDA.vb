Imports System.Text
Imports System.Data.SqlClient
Imports System.Reflection.MethodBase
Imports Itis.ApplicationBlocks.Data.SQLHelper
Imports Itis.ApplicationBlocks.ExceptionManagement.UnTrappedExceptionManager
Imports Lixil.AvoidMissSystem.Utilities

Public Class MsMaintenanceLogDA

    ''' <summary>
    ''' Log信息取得
    ''' </summary>
    ''' <param name="fromDate"></param>
    ''' <param name="toDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSearchData(ByVal fromDate As Date, ByVal toDate As Date) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, fromDate, toDate)

        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT ")
        sb.AppendLine(" log_cd ")
        sb.AppendLine(" , operate_tb  ")
        sb.AppendLine(" , operate_kind ")
        sb.AppendLine(" , operator_cd ")
        sb.AppendLine(" , operate_objcd ")
        sb.AppendLine(" , operate_date ")
        sb.AppendLine(" FROM ")
        sb.AppendLine("  t_log WITH(READCOMMITTED) ")
        sb.AppendLine(" WHERE ")
        sb.AppendLine(" CONVERT(varchar(10),operate_date ,111) BETWEEN  @fromDate AND @toDate ")
        sb.AppendLine(" ORDER BY operate_date DESC ")
        'パラメータ設定
        Dim ds As New DataSet
        paramList.Add(MakeParam("@fromDate", SqlDbType.VarChar, 10, String.Format("{0:yyyy/MM/dd}", fromDate)))
        paramList.Add(MakeParam("@toDate", SqlDbType.VarChar, 10, String.Format("{0:yyyy/MM/dd}", toDate)))

        '検索の実行
        Dim tableName As String = "LogData"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds
    End Function
End Class
