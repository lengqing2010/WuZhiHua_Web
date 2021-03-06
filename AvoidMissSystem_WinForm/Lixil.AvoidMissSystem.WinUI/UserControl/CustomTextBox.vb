Imports System.Threading
Imports System.ComponentModel
Imports Lixil.AvoidMissSystem.BizLogic
Imports Lixil.AvoidMissSystem.Utilities

''' <summary>
''' 自定义TextBox
''' </summary>
''' <remarks></remarks>
Public Class CustomTextBox

#Region "属性"

    ''' <summary>
    ''' 获得内部用TextBox
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property InnerTxt() As TextBox
        Get
            Return Me.txtBox
        End Get
    End Property

    'Private _serialPortData As String
    '''' <summary>
    '''' 扫描枪数据
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Property SerialPortData() As String
    '    Get
    '        Return _serialPortData
    '    End Get
    '    Set(ByVal value As String)
    '        _serialPortData = value
    '    End Set
    'End Property

    Private _isFocusOn As Boolean = False
    ''' <summary>
    ''' 是否处于获得焦点状态
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsFocusOn() As Boolean
        Get
            Return _isFocusOn
        End Get
        Set(ByVal value As Boolean)
            _isFocusOn = value
        End Set
    End Property

    Public OldValue As String = ""


    Private WithEvents _baseForm As BaseForm
    ''' <summary>
    ''' 调用该控件的画面
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ReferenceBaseForm() As BaseForm
        Get
            Return _baseForm
        End Get
        Set(ByVal value As BaseForm)
            _baseForm = value
        End Set
    End Property

    Public _benchmarkType As String
    ''' <summary>
    ''' 基准类型
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BenchmarkType() As String
        Get
            Return _benchmarkType
        End Get
        Set(ByVal value As String)
            _benchmarkType = value
        End Set
    End Property

    Private _benchmarkValue1 As String
    ''' <summary>
    ''' 基准值1
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BenchmarkValue1() As String
        Get
            Return _benchmarkValue1
        End Get
        Set(ByVal value As String)
            _benchmarkValue1 = value
        End Set
    End Property
    Private _benchmarkValue2 As String
    ''' <summary>
    ''' 基准值2
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BenchmarkValue2() As String
        Get
            Return _benchmarkValue2
        End Get
        Set(ByVal value As String)
            _benchmarkValue2 = value
        End Set
    End Property
    Private _benchmarkValue3 As String
    ''' <summary>
    ''' 基准值3
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BenchmarkValue3() As String
        Get
            Return _benchmarkValue3
        End Get
        Set(ByVal value As String)
            _benchmarkValue3 = value
        End Set
    End Property

    Private _isTools As Boolean = False
    ''' <summary>
    ''' 是否为治具
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsTools() As Boolean
        Get
            Return _isTools
        End Get
        Set(ByVal value As Boolean)
            _isTools = value
        End Set
    End Property

    Private _reMeasureValue1 As CustomTextBox
    ''' <summary>
    ''' 与自己关联的实测值1
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>如果以后还有别的关联的实测值的话，就从2开始制作属性</remarks>
    Public Property ReMeasureValue1() As CustomTextBox
        Get
            Return _reMeasureValue1
        End Get
        Set(ByVal value As CustomTextBox)
            _reMeasureValue1 = value
        End Set
    End Property

    Private _checkResult As String
    ''' <summary>
    ''' 根据基准类型判断出来的检查结果(在调用该控件的画面会用到)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CheckResult() As String
        Get
            Return _checkResult
        End Get
        Set(ByVal value As String)
            _checkResult = value
        End Set
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
    ''' 获得焦点事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBox.GotFocus


        If CheckDetailForm.kariKeyKbn Then
            CheckDetailForm.kariKeyKbn = False
            Exit Sub
        End If

        IsFocusOn = True
        OldValue = Me.txtBox.Text
        CheckDetailForm.activeTxtControls = Me.txtBox

        Me.txtBox.Select()


        'CType(Me.Parent, TableLayoutPanel).AutoScrollPosition.Value = 10

        'Dim r As Integer = CType(Me.Parent, TableLayoutPanel).GetRow(Me)

    End Sub

    ''' <summary>
    ''' 失去焦点事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBox.LostFocus
        IsFocusOn = False
        '失去焦点的时候，根据基准值类型，进行判断
        InputCheck()
        RaiseEvent CustomTextBox_LostFocus(Me, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' TextBox的键盘按下事件，如果是按回车的话，自动将焦点设置在下一个项目上面
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>原理：利用按下回车执行Tab键的形式</remarks>
    Private Sub txtBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBox.KeyPress
        '用回车代替Tab 
        If e.KeyChar = Chr(13) Then
            'e.KeyChar = ChrW(10)

            e.Handled = True    
            IsAutoTab = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

        ''' <summary>
    ''' 内容全选择
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtBox_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtBox.MouseUp
        txtBox.SelectAll()
    End Sub
#End Region

#Region "其他方法"

    ''' <summary>
    ''' TCP接到数据的时候
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub _baseForm_OnTcpReceiveData(ByVal sender As Object, ByVal e As EventArgsSD) Handles _baseForm.OnTcpReceiveData
        If Me.IsFocusOn = True Then
            If Me.IsTools = False Then


                '不为治具的时候可以给textbox赋值
                Dim str As String = e.data.Replace(" ", "").Replace("-", "").Replace("　", "")


                '对应 扫描器扫出的是Y7150708  /R   应该再去除/R
                If BenchmarkType = Consts.BenchmarkType.TYPE12 Then
                    If Microsoft.VisualBasic.Right(str, 2) = "/R" Then
                        str = Microsoft.VisualBasic.Left(str, str.Length - 2).Trim
                    End If
                End If


                Me.txtBox.Text = str

            End If
            IsAutoTab = True
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Public KIJUN As String = ""


    ''' <summary>
    ''' 根据基准值类型，进行检查判断
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InputCheck()

        KIJUN = ""
        Dim value As Decimal

        If IsTools = False Then
            '不是治具的时候
            '共通检查
            If String.IsNullOrEmpty(Me.txtBox.Text.Trim) = True Then
                '没有输入值的时候默认错误
                Me.CheckResult = Consts.CheckResult.NG
                Exit Sub
            End If

            Dim shice1 As String = ""
            Dim shice2 As String = ""

            Dim ct As Control
            Dim flg As Boolean = False
            Dim cha As Double

            If BenchmarkType = Consts.BenchmarkType.TYPE12 OrElse BenchmarkType = Consts.BenchmarkType.TYPE00 Then
            Else

                If Me.txtBox.Text.Trim <> "" Then
                    Try
                        Dim num As String = (Math.Floor(CDbl(Me.txtBox.Text.Trim) * 10) / 10.0).ToString
                        If num.Contains(".") = False Then
                            num = num & ".0"
                        End If
                        Me.txtBox.Text = num
                    Catch ex As Exception

                    End Try

                End If

                Dim tlp As TableLayoutPanel = CType(Me.Parent, TableLayoutPanel)

                Dim r As Integer = tlp.GetRow(Me)

                For Each ct In tlp.Controls
                    If r = tlp.GetRow(ct) Then
                        If (TypeOf ct Is CustomTextBox) Then
                            If flg = False Then
                                shice1 = CType(ct, CustomTextBox).InnerTxt.Text
                                flg = True
                            Else
                                shice2 = CType(ct, CustomTextBox).InnerTxt.Text
                                cha = Math.Abs(CDbl(shice1) - CDbl(shice2))
                            End If
                        End If
                    End If
                Next

            End If

            Try


                '类型检查
                Select Case BenchmarkType
                    Case Consts.BenchmarkType.TYPE00 '目视
                        Me.CheckResult = Consts.CheckResult.OK

                    Case Consts.BenchmarkType.TYPE01
                        '输入一个值，和基准值1相同(=)
                        If Me.txtBox.Text.Trim = Me.BenchmarkValue1 Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                            KIJUN = "=" & Me.BenchmarkValue1

                        End If
                    Case Consts.BenchmarkType.TYPE02

                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try

                        '输入一个值，小于基准值1(<)
                        If value < CDec(Me.BenchmarkValue1) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                            KIJUN = "<" & Me.BenchmarkValue1
                        End If
                    Case Consts.BenchmarkType.TYPE03
                        '输入一个值，小于等于基准值1(<=)


                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try

                        If value <= CDec(Me.BenchmarkValue1) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                            KIJUN = "<=" & Me.BenchmarkValue1
                        End If
                    Case Consts.BenchmarkType.TYPE04
                        '输入一个值，大于基准值1(>)
                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try
                        If value > CDec(Me.BenchmarkValue1) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                            KIJUN = ">" & Me.BenchmarkValue1
                        End If
                    Case Consts.BenchmarkType.TYPE05
                        '输入一个值，大于等于基准值1(>=)
                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try
                        If value >= CDec(Me.BenchmarkValue1) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                            KIJUN = ">=" & Me.BenchmarkValue1
                        End If
                    Case Consts.BenchmarkType.TYPE06
                        '输入一个值，大于基准1-基准3（>=），小于基准1+基准2(<=)
                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try

                        Try
                            If value >= (CDec(Me.BenchmarkValue1) + CDec(BenchmarkValue3)) AndAlso _
                              value <= (CDec(Me.BenchmarkValue1) + CDec(BenchmarkValue2)) Then
                                Me.CheckResult = Consts.CheckResult.OK
                            Else
                                Me.CheckResult = Consts.CheckResult.NG
                                KIJUN = (CDec(Me.BenchmarkValue1) + CDec(BenchmarkValue3)) & "～" & (CDec(Me.BenchmarkValue1) + CDec(BenchmarkValue2))
                            End If

                        Catch ex As Exception
                            Me.CheckResult = Consts.CheckResult.NG
                            MsgBox("入力值不正确")
                        End Try
                    Case Consts.BenchmarkType.TYPE07
                        MsgBox("逻辑未实施")
                    Case Consts.BenchmarkType.TYPE08
                        MsgBox("逻辑未实施")
                    Case Consts.BenchmarkType.TYPE09
                        MsgBox("逻辑未实施")
                    Case Consts.BenchmarkType.TYPE10
                        '   >=0   <=(于基准1/1000)
                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try
                        If value >= 0 AndAlso _
                          value <= (CDec(Me.BenchmarkValue1) / 1000) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                            KIJUN = ("0") & "～" & (CDec(Me.BenchmarkValue1) / 1000)
                        End If



                    Case Consts.BenchmarkType.TYPE11
                        MsgBox("逻辑未实施")


                    Case Consts.BenchmarkType.TYPE12
                        '小口标签日期测试，1.有没有标签，有就算合格 2.没有就算NG   3.有的话扫描到的日期如果和当前 要在当前日期的加减3天以内，则为正确 不是的话就是OW
                        Dim tempStrDate As String = "20" & Me.txtBox.Text.Trim.ToUpper.Replace("Y7", "")
                        If FormatCheck.IsDate(tempStrDate) = True Then

                            Dim commonBC As New CommonBizLogic
                            Dim systemDate As Date = commonBC.GetSystemDate() '取得系统时间

                            If tempStrDate >= String.Format("{0:yyyyMMdd}", systemDate.AddDays(-3)) AndAlso tempStrDate <= String.Format("{0:yyyyMMdd}", systemDate.AddDays(3)) Then
                                Me.CheckResult = Consts.CheckResult.OK
                            Else
                                Me.CheckResult = Consts.CheckResult.OW
                                KIJUN = "前后3日以内"
                            End If
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                            KIJUN = "不是日期"
                        End If
                    Case Consts.BenchmarkType.TYPE13
                        '输入值   <  基准值1

                        value = CDec(cha)
                        If value > 0 AndAlso value < (CDec(Me.BenchmarkValue1)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If

                    Case Consts.BenchmarkType.TYPE14
                        '①输入一个值X，等于基准1+基准值2（>=），小于基准1+基准3(<)  这种情况填写基准值1、2、3
                        '②可以配合基准值表示文字来表示
                        '③基准值表示文字中的基准值需要用{0}等来表示
                        '输入一个值，大于基准1-基准3（>），小于基准1+基准2(<)
                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try

                        Try
                            If value >= (CDec(Me.BenchmarkValue1) + CDec(BenchmarkValue2)) AndAlso _
                              value < (CDec(Me.BenchmarkValue1) + CDec(BenchmarkValue3)) Then
                                Me.CheckResult = Consts.CheckResult.OK
                            Else
                                Me.CheckResult = Consts.CheckResult.NG
                                KIJUN = (CDec(Me.BenchmarkValue1) + CDec(BenchmarkValue2)) & "～" & (CDec(Me.BenchmarkValue1) + CDec(BenchmarkValue3))
                            End If

                        Catch ex As Exception
                            Me.CheckResult = Consts.CheckResult.NG
                            MsgBox("入力值不正确")
                        End Try
                    Case Consts.BenchmarkType.TYPE15_1
                        '输入值   >  基准值1
                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try
                        If value > (CDec(Me.BenchmarkValue1)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If

                    Case Consts.BenchmarkType.TYPE16_1
                        '输入值   <  基准值1
                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try
                        If value < (CDec(Me.BenchmarkValue1)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If
                    Case Consts.BenchmarkType.TYPE17_1
                        '输入值   >=  基准值1
                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try
                        If value >= (CDec(Me.BenchmarkValue1)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If
                    Case Consts.BenchmarkType.TYPE18_1
                        '输入值   <=  基准值1
                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try
                        If value <= (CDec(Me.BenchmarkValue1)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If

                    Case Consts.BenchmarkType.TYPE19_1
                        '输入值   <=  基准值1
                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try
                        If value > (CDec(Me.BenchmarkValue1) + CDec(Me.BenchmarkValue3)) AndAlso value < (CDec(Me.BenchmarkValue1) + CDec(Me.BenchmarkValue2)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If

                    Case Consts.BenchmarkType.TYPE20_1
                        '输入值   <=  基准值1
                        Try
                            value = CDec(Me.txtBox.Text.Trim)
                        Catch ex As Exception
                            value = 0
                        End Try
                        If value >= (CDec(Me.BenchmarkValue1) + CDec(Me.BenchmarkValue3)) AndAlso value <= (CDec(Me.BenchmarkValue1) + CDec(Me.BenchmarkValue2)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If


                    Case Consts.BenchmarkType.TYPE15_2
                        '输入值   >  基准值1

                        value = CDec(cha)

                        If value > (CDec(Me.BenchmarkValue1)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If

                    Case Consts.BenchmarkType.TYPE16_2
                        '输入值   <  基准值1

                        value = CDec(cha)
                        If value < (CDec(Me.BenchmarkValue1)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If
                    Case Consts.BenchmarkType.TYPE17_2
                        '输入值   >=  基准值1

                        value = CDec(cha)
                        If value >= (CDec(Me.BenchmarkValue1)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If
                    Case Consts.BenchmarkType.TYPE18_2
                        '输入值   <=  基准值1

                        value = CDec(cha)
                        If value <= (CDec(Me.BenchmarkValue1)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If

                    Case Consts.BenchmarkType.TYPE19_2
                        '输入值   <=  基准值1

                        value = CDec(cha)

                        If value > (CDec(Me.BenchmarkValue1) + CDec(Me.BenchmarkValue3)) AndAlso value < (CDec(Me.BenchmarkValue1) + CDec(Me.BenchmarkValue2)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If

                    Case Consts.BenchmarkType.TYPE20_2
                        '输入值   <=  基准值1

                        value = CDec(cha)
                        If value >= (CDec(Me.BenchmarkValue1) + CDec(Me.BenchmarkValue3)) AndAlso value <= (CDec(Me.BenchmarkValue1) + CDec(Me.BenchmarkValue2)) Then
                            Me.CheckResult = Consts.CheckResult.OK
                        Else
                            Me.CheckResult = Consts.CheckResult.NG
                        End If

                    Case Else
                        Me.CheckResult = Consts.CheckResult.NG

                End Select

            Catch ex As Exception
                Try
                    MsgBox("数据库中的数据出现错误" & vbCrLf & _
                                    "基准值1：" & Me.BenchmarkValue1 & _
                                    "基准值2：" & Me.BenchmarkValue2 & _
                                    "基准值3：" & Me.BenchmarkValue3 & _
                                    "值1：" & shice1 & _
                                    "值2：" & shice1 & vbCrLf & ex.Message)

                Catch ex1 As Exception

                End Try

                Me.CheckResult = Consts.CheckResult.NG

            End Try
            

            KIJUN = "(" & Me.BenchmarkValue1 _
                & ",+" & Me.BenchmarkValue2 _
                & "," & Me.BenchmarkValue3 & ")"

        Else
            '治具的时候
            If Me.txtBox.Text.Trim = Me.BenchmarkValue1 Then
                Me.CheckResult = Consts.CheckResult.OK
                Me.txtBox.ReadOnly = True '如果验证结果为正确，那么不能再改动
            Else
                Me.CheckResult = Consts.CheckResult.NG
            End If
        End If

    End Sub

    ''' <summary>
    ''' 注册一个失去焦点事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event CustomTextBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    ' Public Event CustomTextBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)


#End Region


    Public Sub New()

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        DoubleBuffered = True
        '双缓冲
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

    End Sub
End Class
