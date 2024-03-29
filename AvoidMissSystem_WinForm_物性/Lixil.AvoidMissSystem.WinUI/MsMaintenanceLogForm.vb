Imports System.Windows
Imports Lixil.AvoidMissSystem.BizLogic
Imports Lixil.AvoidMissSystem.Utilities

Public Class MsMaintenanceLogForm

    Dim bc As New MsMaintenanceLogBizLogic

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim dtLog As DataTable
        '取得Log信息
        dtLog = bc.GetSearchData(Me.PFromDate.Value, Me.PToDate.Value)

        Me.dtLogList.DataSource = dtLog

    End Sub

    ''' <summary>
    ''' 返回按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        Try
            NavigateToNextPage(Consts.PAGE.MS_MAIN)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 退出按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            NavigateToNextPage(Consts.PAGE.MS_LOGIN)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MsMaintenanceLogForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text = "日志查询"
    End Sub
End Class
