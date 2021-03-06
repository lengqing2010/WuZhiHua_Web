Imports Lixil.AvoidMissSystem.Utilities

''' <summary>
''' Main(MS维护)主页面用Form
''' </summary>
''' <remarks></remarks>
Public Class MsMaintenanceMainForm

#Region "事件"
    ''' <summary>
    ''' 窗体加载处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MsMaintenanceMainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim comBc As New BizLogic.MsMaintenanceToolsBizLogic
        Dim dtDepartment As DataTable = comBc.GetDepartment(Me.Login.UserId, Me.Login.IsAdmin).Tables("Department")

        If dtDepartment.Rows.Count = 0 Then
            '权限MS维护按钮可用
            btnAuthorityMs.Enabled = False
            '基础MS维护按钮可用
            btnKensaMs.Enabled = False
            '图片MS维护按钮可用
            btnPictrueMs.Enabled = False
            '治具MS维护按钮可用
            btnToolsMs.Enabled = False
            '检查结果修正按钮可用
            btnUpdResult.Enabled = False
            '查询日志按钮可用
            btnLog.Enabled = False
            MessageBox.Show("部门数据不存在", "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '跳转到MS维护登录页面
            NavigateToNextPage(Consts.PAGE.MS_LOGIN)
        End If


        '如果有管理员权限，所有按钮均为可用状态
        If Me.Login.IsAdmin Then
            '权限MS维护按钮可用
            btnAuthorityMs.Enabled = True
            '基础MS维护按钮可用
            btnKensaMs.Enabled = True
            '图片MS维护按钮可用
            btnPictrueMs.Enabled = True
            '治具MS维护按钮可用
            btnToolsMs.Enabled = True
            '检查结果修正按钮可用
            btnUpdResult.Enabled = True
            '查询日志按钮可用
            btnLog.Enabled = True
        Else
            '没有管理员权限时权限MS按钮不可用
            btnAuthorityMs.Enabled = False
            '没有管理员权限时查询日志按钮不可用
            btnLog.Enabled = False

            '做成权限区分list
            Dim lstCode As New ArrayList
            lstCode = Me.Login.Authority '从共有hashtable中取得权限区分list

            If lstCode.Count = 0 Then '如果没有权限，所有ms维护按钮都不可用
                btnKensaMs.Enabled = False
                btnPictrueMs.Enabled = False
                btnToolsMs.Enabled = False
                btnUpdResult.Enabled = False
            Else

                '有图片MS维护权限的时候
                If lstCode.Contains(Consts.AccessCd.MS_PICTRUE) Then
                    '图片MS维护按钮可用
                    btnPictrueMs.Enabled = True
                Else
                    '无权限时按钮不可用
                    btnPictrueMs.Enabled = False
                End If

                '有治具MS维护权限的时候
                If lstCode.Contains(Consts.AccessCd.MS_TOOLS) Then
                    '治具MS维护按钮可用
                    btnToolsMs.Enabled = True
                Else
                    '无权限时按钮不可用
                    btnToolsMs.Enabled = False
                End If

                '有基础MS维护权限的时候
                If lstCode.Contains(Consts.AccessCd.MS_BASIC) Then
                    '基础MS维护按钮可用
                    btnKensaMs.Enabled = True
                Else
                    '无权限时按钮不可用
                    btnKensaMs.Enabled = False
                End If

                '有检查结果修正权限的时候
                If lstCode.Contains(Consts.AccessCd.MS_RESULTMODIFY) Then
                    '检查结果修正按钮可用
                    btnUpdResult.Enabled = True
                Else
                    '无权限时按钮不可用
                    btnUpdResult.Enabled = False
                End If
            End If

        End If
        Me.Text = "功能菜单"
        Me.Width = 621
        Me.Height = 408
    End Sub

    ''' <summary>
    ''' 退出登录按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        '跳转到MS维护登录页面
        NavigateToNextPage(Consts.PAGE.MS_LOGIN)

    End Sub

    ''' <summary>
    ''' 图片MS按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPictrueMs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPictrueMs.Click
        '跳转到图片MS页面
        NavigateToNextPage(Consts.PAGE.MS_PICTURE)
    End Sub

    ''' <summary>
    ''' 治具MS按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnToolsMs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnToolsMs.Click
        '跳转到治具MS页面
        NavigateToNextPage(Consts.PAGE.MS_TOOLS)
    End Sub

    ''' <summary>
    ''' 基础MS按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnKensaMs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKensaMs.Click
        '跳转到基础MS页面
        NavigateToNextPage(Consts.PAGE.MS_BASIC)
    End Sub

    ''' <summary>
    ''' 权限MS按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAuthorityMs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorityMs.Click
        '跳转到权限MS页面
        NavigateToNextPage(Consts.PAGE.MS_AUTHORITY)
    End Sub

    ''' <summary>
    ''' 检查结果修正按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUpdResult_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdResult.Click
        '跳转到检查结果修正页面
        NavigateToNextPage(Consts.PAGE.MS_RESULTMODIFY)
    End Sub

    ''' <summary>
    ''' 查询日志按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLog.Click
        '跳转到日志查询
        NavigateToNextPage(Consts.PAGE.MS_LOG)
    End Sub
#End Region

    Private Sub btnBaobiao_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBaobiao.Click
        'NavigateToNextPage(Consts.PAGE.MS_BAOBIAO)
        msBaobiao.Show()

    End Sub
    'm_permission
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        User.Show()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        m_permission.Show()
    End Sub
End Class
