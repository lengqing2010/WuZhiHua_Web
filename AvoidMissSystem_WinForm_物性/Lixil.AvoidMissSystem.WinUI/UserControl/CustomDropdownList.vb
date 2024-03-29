Imports Lixil.AvoidMissSystem.Utilities

''' <summary>
''' dropdownlist
''' </summary>
''' <remarks></remarks>
Public Class CustomDropdownList
#Region "构造"



    Public Sub New(ByVal xiaokoubiaoqian As Boolean)

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()
        DoubleBuffered = True
        '双缓冲
        'Me.SetStyle(ControlStyles.UserPaint, True)
        'Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        'Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        'Me.SetStyle(ControlStyles.ResizeRedraw, True)
        'Me.SetStyle(ControlStyles.DoubleBuffer, True)

        drpList.BeginUpdate()
        drpList.DataSource = GetdrpListData(xiaokoubiaoqian)
        drpList.DisplayMember = "name"
        drpList.ValueMember = "value"
        drpList.EndUpdate()


    End Sub

    'DrowdownList数据作成
    Public Shared drpListData As DataTable
    Public Function GetdrpListData(ByVal xiaokoubiaoqian As Boolean) As DataTable
        If drpListData Is Nothing Then
            drpListData = New DataTable()
            drpListData.Columns.Add(New DataColumn("value", System.Type.GetType("System.String")))
            drpListData.Columns.Add(New DataColumn("name", System.Type.GetType("System.String")))
            drpListData.Rows.Add("", "")
            drpListData.Rows.Add("0", "0合格")
            drpListData.Rows.Add("1", "1不合格")
            drpListData.Rows.Add("2", "2微欠品")
            drpListData.Rows.Add("3", "3轻中重欠品")
            If xiaokoubiaoqian Then
                drpListData.Rows.Add("4", "4警告")
            End If

        End If
        Return drpListData
    End Function



#End Region

#Region "属性"

    'Public _TabIndex As Integer = 0
    '''' <summary>
    '''' TabIndex的获取与设定
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Property CTabIndex()
    '    Get
    '        Return _TabIndex ' drpList.TabIndex
    '    End Get
    '    Set(ByVal value)
    '        _TabIndex = value
    '        'drpList.TabIndex = value
    '    End Set
    'End Property

    Private isAlfterLoad As Boolean = False
    Private isFocused As Boolean = False

    ''' <summary>
    ''' 选择的值
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelecedtValue() As String
        Get
            Select Case drpList.SelectedValue.ToString
                Case "0" '合格
                    Return Consts.CheckResult.OK
                Case "1" '不合格
                    Return Consts.CheckResult.NG
                Case "2" '微欠品
                    Return Consts.CheckResult.SD
                Case "3" '轻重中欠品
                    Return Consts.CheckResult.MD
                Case "4" '警告
                    Return Consts.CheckResult.OW
                Case Else
                    Return Consts.CheckResult.INIT
            End Select
        End Get
        Set(ByVal value As String)
            Select Case value
                Case Consts.CheckResult.OK '合格
                    drpList.SelectedValue = "0"
                Case Consts.CheckResult.NG '不合格
                    drpList.SelectedValue = "1"
                Case Consts.CheckResult.SD '微欠品
                    drpList.SelectedValue = "2"
                Case Consts.CheckResult.MD '轻重中欠品
                    drpList.SelectedValue = "3"
                Case Consts.CheckResult.OW '警告
                    drpList.SelectedValue = "4"
                Case Else
                    drpList.SelectedValue = String.Empty
            End Select
        End Set
    End Property

    ''' <summary>
    ''' 选择的文本
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SelectedText() As String
        Get
            Return Me.drpList.SelectedText
        End Get
    End Property

    Private _isAutoTab As Boolean = False

    ''' <summary>
    ''' 是否为自动TAB
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsAutoTab() As Boolean
        Get
            Return _isAutoTab
        End Get
        Set(ByVal value As Boolean)
            _isAutoTab = value
        End Set
    End Property
#End Region

#Region "事件"
    ''' <summary>
    ''' 初始化
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CustomDropdownList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        isAlfterLoad = True

    End Sub

    Private Sub drpList_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpList.GotFocus
        If IsAutoTab = True Then
            System.Threading.Thread.Sleep(100)
            SendKeys.Send("{TAB 2}")
        End If
        isFocused = True
    End Sub

    ''' <summary>
    ''' 键盘按下事件，如果是按回车的话，自动将焦点设置在下一个项目上面
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>原理：利用按下回车执行Tab键的形式</remarks>
    Private Sub drpList_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles drpList.KeyPress

        Select Case e.KeyChar
            Case Chr(13) '回车
                '用回车代替Tab
                e.Handled = True
                System.Threading.Thread.Sleep(100)
                SendKeys.Send("{TAB 2}")

        End Select
    End Sub

    ''' <summary>
    ''' 选择后自动跳到下一项
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub drpList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpList.SelectedIndexChanged
        If isAlfterLoad = True AndAlso isFocused = True Then
            System.Threading.Thread.Sleep(100)
            SendKeys.Send("{TAB 2}")
        End If
    End Sub

    ''' <summary>
    ''' 注册一个失去焦点事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event CustomDropdownList_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

    ''' <summary>
    ''' 失去焦点
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub drpList_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpList.LostFocus
        IsAutoTab = False
        isFocused = False
        RaiseEvent CustomDropdownList_LostFocus(Me, EventArgs.Empty)
    End Sub
#End Region

End Class
