
Public Class MsgConfirmForm
#Region "变量声明"

    '显示的数据源
    Private dt As New DataTable
    '类型
    Private strType As String = String.Empty
    '主函数
    Public mainFm As CheckMainForm
    '结果ID
    Public strResultId As String = String.Empty
    '用户选择 RC：重新检查 CC：继续检查 DC：默认 NC：不检查
    Private strChoese As String = String.Empty
    '返回Hashtable
    Dim hsBack As New Hashtable
    '筛选条件
    Dim strSearchKeys As String = String.Empty
    '默认结果ID
    Dim strKeyResultID As String = String.Empty
    '默认结果行
    Dim intRowCount As Integer = 0

#End Region

#Region "系统函数"

    ''' <summary>
    ''' 重写构造函数
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(ByVal dt As DataTable, ByVal strType As String, ByVal mainForm As CheckMainForm, Optional ByVal strKeyResultId As String = "")

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        '本地赋值
        Me.dt = dt
        Me.strType = strType
        mainFm = mainForm
        Me.strKeyResultID = strKeyResultId
        Me.Text = Me.Text & "(" & mainFm.SetCDZUOFAN & ")"
    End Sub

    ''' <summary>
    ''' 画面加载处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MsgConfirmForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Init()
    End Sub

#End Region

#Region "画面按钮事件"

    ''' <summary>
    ''' 继续检查按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnContinueChk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinueChk.Click
        '返回用户选择类型
        strChoese = "CC"
        '返回继续检查的结果ID
        hsBack.Add("strResultId", Me.strResultId)
        mainFm.MsgFormCallback(strChoese, hsBack)

        Me.Close()

    End Sub

    ''' <summary>
    ''' 默认结果按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDefaultResult_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefaultResult.Click
        '返回用户选择类型
        strChoese = "DC"

        Dim dtRows As DataRow()
        Dim jianchayuan As String = ""
        If Me.strType.Equals("1") Then
            'CASE2  有同作番无待判。用户选项：1 重新检查 2 默认结果 （检查最近的一次结果，如果最近一条且检查状态为[完了]的结果,则继承此结果，如果为默认情况，则默认此结果默认的结果）3 不检查
            strSearchKeys = "作番 = " & mainFm.strMakeNo
            dtRows = dt.Select(strSearchKeys, "结束时间 DESC")

            If dtRows(0).Item("状态").ToString.Equals("默认") Then
                '最近一条为待判的场合
                hsBack.Add("strLastReulstID", dtRows(0).Item("继承结果").ToString)
                hsBack.Add("strLastReulst", dtRows(0).Item("结果").ToString)
            Else
                '最近一条为完了的场合
                hsBack.Add("strLastReulstID", dtRows(0).Item("ID").ToString)
                hsBack.Add("strLastReulst", dtRows(0).Item("结果").ToString)
            End If
            jianchayuan = dtRows(0).Item("检查员CD").ToString
        ElseIf Me.strType.Equals("3") Then
            'CASE4 无同作番无待判。用户选项：1 重新检查 2 默认结果 3 不检查
            strSearchKeys = "状态 = '完了' "
            dtRows = dt.Select(strSearchKeys, "结束时间 DESC")
            hsBack.Add("strLastReulstID", dtRows(0).Item("ID").ToString)
            hsBack.Add("strLastReulst", dtRows(0).Item("结果").ToString)
            jianchayuan = dtRows(0).Item("检查员CD").ToString
        End If
        mainFm.MsgFormCallback(strChoese, hsBack, jianchayuan)
        Me.Close()
    End Sub

    ''' <summary>
    ''' 重新检查按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnReChk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReChk.Click


        If MessageBox.Show("确定重新检查吗？", "退出确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        strChoese = "RC"
        mainFm.MsgFormCallback(strChoese, hsBack)
        Me.Close()
    End Sub

    ''' <summary>
    ''' 不检查按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNoChk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoChk.Click
        strChoese = "NC"
        mainFm.MsgFormCallback(strChoese, hsBack)
        Me.Close()
    End Sub

#End Region

#Region "内部调用函数"

    ''' <summary>
    ''' 初始化 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Init()
        '按钮显示设置
        Select Case Me.strType
            Case "0"
                'CASE1  存在待判的场合。选项：1 重新检查 2 继续检查  3 不检查
                Me.btnContinueChk.Visible = True
                Me.btnReChk.Visible = True
            Case "1"
                'CASE2  无待判数据检查。用户选项：1 重新检查 2 默认结果 （检查最近的一次结果，如果最近一条且检查状态为[完了]的结果）3 不检查
                Me.btnReChk.Visible = True
                Me.btnDefaultResult.Visible = True
            Case "2"
                'CASE3 无同作番有待判。用户选项：1 重新检查 2 不检查
                Me.btnReChk.Visible = True
            Case "3"
                'CASE4 无同作番无待判。用户选项：1 重新检查 2 默认结果 3 不检查
                Me.btnReChk.Visible = True
                Me.btnDefaultResult.Visible = True
            Case Else
                '其他什么也不做
        End Select
        '数据筛选：不含默认状态下数据
        Dim dtShow As DataTable = dt.Clone
        Me.strSearchKeys = "状态 <> '默认' "
        Dim dtRoews As DataRow() = dt.Select(Me.strSearchKeys, "结束时间 DESC")
        For i As Integer = 0 To dtRoews.Length - 1
            If dtRoews(i).Item("ID").ToString.Equals(Me.strKeyResultID) Then
                Me.intRowCount = i
            End If
            dtShow.ImportRow(dtRoews(i))
        Next
        '数据源绑定
        Me.dtgv.DataSource = dtShow

        '列绑定
        For i As Integer = 0 To dt.Columns.Count - 1
            Me.dtgv.Columns(i).DataPropertyName = dt.Columns(i).ToString
            'Me.dtgv.Columns(i).Width = 100
        Next
        '最后一列继承结果不显示
        Me.dtgv.Columns(10).Visible = False
        If Me.strType.Equals("1") OrElse Me.strType.Equals("3") Then
            Me.dtgv.Rows(Me.intRowCount).Selected = True
        End If
        'Me.Text = "确认"
        Me.Text = "确认" & "(" & mainFm.SetCDZUOFAN & ")"
        Try
            lblSyohinCd.Text = CStr(Me.dtgv.Rows(0).Cells(1).Value)
        Catch ex As Exception
            lblSyohinCd.Text = ""

        End Try



        dtgv.Columns(0).Width = 105
        dtgv.Columns(1).Visible = False
        dtgv.Columns(2).Width = 90
        dtgv.Columns(3).Width = 180 '生产线
        dtgv.Columns(4).Width = 90 '
        dtgv.Columns(5).Width = 100 '结果
        dtgv.Columns(6).Width = 100 '状态
        dtgv.Columns(7).Width = 70 '检查员
        dtgv.Columns(8).Width = 130 '开始时间
        dtgv.Columns(9).Width = 130 '结束时间
        'For i As Integer = 0 To dtgv.Rows.Count - 1
        '    If dtgv.Rows(i).Cells(5).Value.ToString.Trim = "OK" Then
        '        dtgv.Rows(i).Cells(5).Style.ForeColor = Color.GreenYellow
        '    End If
        '    If dtgv.Rows(i).Cells(6).Value.ToString.Trim = "完了" Then
        '        dtgv.Rows(i).Cells(6).Style.ForeColor = Color.GreenYellow
        '    End If
        'Next

    End Sub
#End Region

    'Private Sub dtgv_DataBindingComplete(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewBindingCompleteEventArgs) Handles dtgv.DataBindingComplete


    'End Sub
End Class