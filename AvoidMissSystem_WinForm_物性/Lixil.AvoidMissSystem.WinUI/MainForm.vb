Imports System.Net
Imports System.Random
Imports System.Configuration

''' <summary>
''' main
''' </summary>
''' <remarks></remarks>
Public Class frmTest
    Dim HostName As String = System.Net.Dns.GetHostName '获得本机的机器名
    Private Sub MenuForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim Debug As String = ConfigurationManager.AppSettings("Debug").ToString()
        Dim startFormName As String = ConfigurationManager.AppSettings("StartFormName").ToString()

        If startFormName = "CheckMainForm" Then
            Dim fm As New CheckMainForm
            fm.Show()
        ElseIf startFormName = "MsMaintenanceLoginForm" Then
            Dim fm As New MsMaintenanceLoginForm
            fm.Show()
        End If

        Me.Close()

        Exit Sub

        '遍历画面上的控件，并添加事件
        For Each ctrl As Control In GetChildControls(Me)
            If ctrl.GetType() Is GetType(CustomTextBox) Then
                DirectCast(ctrl, CustomTextBox).ReferenceBaseForm = Me
                AddHandler DirectCast(ctrl.Controls.Find("txtBox", True)(0), TextBox).LostFocus, AddressOf Control_LostFocus
            End If
        Next

    End Sub

    ''' <summary>
    ''' 失去焦点事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Control_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Console.WriteLine("this is mainform lostfocus")
    End Sub

    Private Sub btnMsMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMsMaintenance.Click
        NavigateToNextPage("msLogin")
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    '寸法检查按钮
    Private Sub btnCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheck.Click
        '检查主页面跳转
        NavigateToNextPage("chkMain")
    End Sub

    ''' <summary>
    ''' 打开本机COM口
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnOpenCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.OpenSerialPort()
    End Sub

    ''' <summary>
    ''' 启动服务器
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnOpenServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.OpenServer()
    End Sub

    ''' <summary>
    ''' 接收到Tcp数据时
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmTest_OnTcpReceiveData(ByVal sender As Object, ByVal e As EventArgsSD) Handles Me.OnTcpReceiveData
        Console.WriteLine("OnTcpData:" & e.data)
        Me.Label1.Text = e.data
    End Sub

    ''' <summary>
    ''' 获取画面上所有的项目
    ''' </summary>
    ''' <param name="parent"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetChildControls(ByVal parent As Control) As ArrayList
        Dim result As New ArrayList()
        For Each ctrl As Control In parent.Controls
            result.Add(ctrl)
            result.AddRange(GetChildControls(ctrl))
        Next
        Return result
    End Function
End Class
