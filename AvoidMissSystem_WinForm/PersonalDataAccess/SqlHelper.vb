﻿Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration  '必须要在管理器中添加引用    
''' <summary>  
''' SqlHelper类是专门提供给广大用户用于高性能、可升级和最佳练习的sql数据操作  
''' </summary>  
''' <remarks></remarks>  
Public Class SqlHelper
    '定义变量    
    '获得数据库的连接字符串    
    Private ReadOnly strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString
    '设置连接    
    Public conn As SqlConnection = New SqlConnection(strConnection)
    '定义cmd命令    
    Public cmd As New SqlCommand

    ''' <summary>    
    ''' 执行增删改三个操作,(有参)返回值为Boolean类型,确认是否执行成功    
    ''' </summary>    
    ''' <param name="cmdText">需要执行语句,一般是Sql语句,也有存储过程</param>    
    ''' <param name="cmdType">判断Sql语句的类型,一般都不是存储过程</param>    
    ''' <param name="paras">参数数组,无法确认有多少参数</param>    
    ''' <returns></returns>    
    ''' <remarks></remarks>    
    Public Function ExecAddDelUpdate(ByVal cmdText As String, ByVal cmdType As CommandType, ByVal paras As SqlParameter()) As Integer
        '将传入的值,分别为cmd的属性赋值    
        cmd.Parameters.AddRange(paras)   '将参数传入    
        cmd.CommandType = cmdType            '设置一个值,解释cmdText    
        cmd.Connection = conn                '设置连接,全局变量    
        cmd.CommandText = cmdText            '设置查询的语句    

        Try
            conn.Open()                      '打开连接    
            Return cmd.ExecuteNonQuery()     '执行增删改操作    
            cmd.Parameters.Clear()           '清除参数    
        Catch ex As Exception
            Return 0                         '如果出错,返回0    
        Finally
            Call CloseConn(conn)
            Call CloseCmd(cmd)
        End Try
    End Function
    ''' <summary>    
    ''' 执行增删改三个操作,(无参)    
    ''' </summary>    
    ''' <param name="cmdText">需要执行语句,一般是Sql语句,也有存储过程</param>    
    ''' <param name="cmdType">判断Sql语句的类型,一般都不是存储过程</param>    
    ''' <returns>Interger,受影响的行数</returns>    
    ''' <remarks>2013年2月2日8:19:59</remarks>    
    Public Function ExecAddDelUpdateNo(ByVal cmdText As String, ByVal cmdType As CommandType) As Integer
        '为要执行的命令cmd赋值    
        cmd.CommandText = cmdText       '先是查询的sql语句    
        cmd.CommandType = cmdType       '设置Sql语句如何解释    
        cmd.Connection = conn           '设置连接    

        '执行操作    
        Try
            conn.Open()
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Return 0
        Finally
            Call CloseConn(conn)
            Call CloseCmd(cmd)
        End Try
    End Function

    ''' <summary>    
    ''' 执行查询的操作,(有参),参数不限    
    ''' </summary>    
    ''' <param name="cmdText">需要执行语句,一般是Sql语句,也有存储过程</param>    
    ''' <param name="cmdType">判断Sql语句的类型,一般都不是存储过程</param>    
    ''' <param name="paras">传入的参数</param>    
    ''' <returns></returns>    
    ''' <remarks></remarks>    
    Public Function ExecSelect(ByVal cmdText As String, ByVal cmdType As CommandType, ByVal paras As SqlParameter()) As DataTable

        Dim sqlAdapter As SqlDataAdapter
        Dim dt As New DataTable
        Dim ds As New DataSet
        '还是给cmd赋值    
        cmd.CommandText = cmdText
        cmd.CommandType = cmdType
        cmd.Connection = conn
        cmd.Parameters.AddRange(paras)  '参数添加    
        sqlAdapter = New SqlDataAdapter(cmd)  '实例化adapter    
        Try
            sqlAdapter.Fill(ds)           '用adapter将dataSet填充     
            dt = ds.Tables(0)             'datatable为dataSet的第一个表    
            cmd.Parameters.Clear()        '清除参数    
        Catch ex As Exception
            MsgBox("查询失败", CType(vbOKOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "警告")
        Finally                            '最后一定要销毁cmd    
            Call CloseCmd(cmd)
        End Try
        Return dt
    End Function


    ''' <summary>    
    ''' 执行查询的操作,(无参)    
    ''' </summary>    
    ''' <param name="cmdText">需要执行语句,一般是Sql语句,也有存储过程</param>    
    ''' <param name="cmdType">判断Sql语句的类型,一般都不是存储过程</param>    
    ''' <returns>dataTable,查询到的表格</returns>    
    ''' <remarks></remarks>    
    Public Function ExecSelectNo(ByVal cmdText As String, ByVal cmdType As CommandType) As DataTable
        Dim sqlAdapter As SqlDataAdapter
        Dim ds As New DataSet
        '还是给cmd赋值    
        cmd.CommandText = cmdText
        cmd.CommandType = cmdType
        cmd.Connection = conn
        cmd.CommandTimeout = 0
        sqlAdapter = New SqlDataAdapter(cmd)  '实例化adapter    
        Try
            sqlAdapter.Fill(ds)           '用adapter将dataSet填充     
            Return ds.Tables(0)             'datatable为dataSet的第一个表    
        Catch ex As Exception
            Return Nothing
        Finally                            '最后一定要销毁cmd    
            Call CloseCmd(cmd)
        End Try
    End Function

    ''' <summary>    
    ''' 执行查询的操作,(无参)    
    ''' </summary>    
    ''' <param name="cmdText">需要执行语句,一般是Sql语句,也有存储过程</param>    
    ''' <param name="cmdType">判断Sql语句的类型,一般都不是存储过程</param>    
    ''' <returns>dataTable,查询到的表格</returns>    
    ''' <remarks></remarks>    
    Public Function ExecSelectNoRtvDataSet(ByVal cmdText As String, ByVal cmdType As CommandType) As DataSet
        Dim sqlAdapter As SqlDataAdapter
        Dim ds As New DataSet
        '还是给cmd赋值    
        cmd.CommandText = cmdText
        cmd.CommandType = cmdType
        cmd.Connection = conn
        sqlAdapter = New SqlDataAdapter(cmd)  '实例化adapter    
        Try
            sqlAdapter.Fill(ds)           '用adapter将dataSet填充     
            Return ds            'datatable为dataSet的第一个表    
        Catch ex As Exception
            Return Nothing
        Finally                            '最后一定要销毁cmd    
            Call CloseCmd(cmd)
        End Try
    End Function


    ''' <summary>    
    ''' 关闭连接    
    ''' </summary>    
    ''' <param name="conn">需要关闭的连接</param>    
    ''' <remarks></remarks>    
    Public Sub CloseConn(ByVal conn As SqlConnection)
        If (conn.State <> ConnectionState.Closed) Then  '如果没有关闭    
            conn.Close()                    '关闭连接    
            conn = Nothing                  '不指向原对象    
        End If

    End Sub
    ''' <summary>    
    ''' 关闭命令    
    ''' </summary>    
    ''' <param name="cmd">需要关闭的命令</param>    
    ''' <remarks></remarks>    
    Public Sub CloseCmd(ByVal cmd As SqlCommand)

        If Not IsNothing(cmd) Then          '如果cmd命令存在    
            cmd.Dispose()                   '销毁    
            cmd = Nothing
        End If
    End Sub
End Class