''' <summary>
''' メニューフォーム
''' </summary>
''' <remarks></remarks>
Public Class MenuForm

    Private Sub MenuForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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
End Class
