Imports System.Threading
Imports System.Transactions
Imports Lixil.AvoidMissSystem.BizLogic
Imports Lixil.AvoidMissSystem.Utilities
Imports Lixil.AvoidMissSystem.Utilities.Consts
Imports Lixil.AvoidMissSystem.Utilities.MsgConst
Imports System.IO
Imports System.Configuration
Public Class CheckMainForm

#Region "变量声明"
    Public MS_Font_Size As Integer = CInt(ConfigurationManager.AppSettings("CheckDetail_MSFontSize").ToString())
    Public MS_MSLableFontFamily As String = (ConfigurationManager.AppSettings("CheckDetail_MSLableFontFamily").ToString())
    Public MS_MSButtonFontFamily As String = (ConfigurationManager.AppSettings("CheckDetail_MSButtonFontFamily").ToString())

    'BC层实体化
    Dim bc As New CheckBizLogic
    Dim bcIndex As New IndexBizLogic
    Dim bcCom As New CommonBizLogic
    '商品code
    Public strGoodsCode As String = String.Empty
    '商品作番
    Public strMakeNo As String = String.Empty
    '当前状态flg 0:初始状态 1：外观寸法检查中 2：治具检查中
    Public strState As String = StateType.STATEINIT
    '检察员code
    Public strChkUserCd As String = String.Empty
    '治具编号dic
    Dim dicToolsNo As New Dictionary(Of String, String)
    '治具条码List
    Dim lstBar As New List(Of String)
    ''治具验证flg 0：未验证 1;y已验证 
    Dim flgToolsVerify As Integer = Flag.[OFF]
    ''无条形码验证flg
    'Dim flgWToolsVerify As Integer = Flag.[OFF]
    '存放外观寸法分类检查信息 (key,state) state: 0 未检查  1 检查完了 2 不合格   
    Dim dicCFInfo As New Dictionary(Of String, CheckInfo)
    '治具编号检查信息
    Dim dicZJBInfo As New Dictionary(Of String, List(Of String))
    '治具分类检查信息
    Dim dicZJFInfo As New Dictionary(Of String, Dictionary(Of String, CheckInfo))

    '第一个治具编号ID
    Dim strFirstID As String = String.Empty

    '当前检查商品Code的结果ID
    Public strReulstID As String = String.Empty
    '当前检查商品Code的最近结果ID
    Dim strLastReulstID As String = String.Empty
    '当前检查商品Code的最近结果
    Dim strLastReulst As String = String.Empty
    '当前检查商品Code的所属部门
    Dim strDdepartment As String = String.Empty
    '最终检查结果
    Dim strResult As String = String.Empty
    '商品Id
    Public strGoodsID As String = String.Empty
    '微欠点计数
    Dim intSDno As Integer = 0
    '合格但须警告计数
    Dim intOWno As Integer = 0
    '轻中重欠品计数
    Dim intMDno As Integer = 0
    '不合格计数
    Dim intNGno As Integer = 0
    '漏检计数
    Dim intINITno As Integer = 0
    '漏检计数
    Dim intLCno As Integer = 0
    '光标所在分类标记
    Dim strCursorTp As String = String.Empty
    'log文件存放根路径设置
    Dim strLogPath As String = GetConfig.GetConfigVal("LogPath")
    'logt txt绝对路径
    Dim strLogTxtPath As String = String.Empty

    '继续检查Flg
    Dim flgCC As Integer = Flag.[OFF]

    '接受扫码Flg
    Dim flgReceiveCd As Integer = Flag.ON

    Public Function SetCDZUOFAN() As String
        Me.lblStaus.Text = "商品CD：" & Me.strGoodsCode & " 座番：" & Me.strMakeNo & " 商品ID：" & Me.strReulstID & " " & "状态：" & pubxinshengchanStr

        Return Me.lblStaus.Text
    End Function

#End Region

#Region "系统响应事件"

    ''' <summary>
    ''' PageLode
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CheckMainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '画面初始化
        Init()
        '打开COM和Service
        Me.LOpen()
    End Sub

    ''' <summary>
    ''' 用户关闭系统关闭按钮事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CheckMainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Select Case Me.strState
            Case StateType.STATEINIT, StateType.STATE1, StateType.STATE2, StateType.STATEERROR
                '初始状态下，什么也不做，直接退出
            Case Else
                '数据保存
                If SaveChkResult() Then
                    '关闭COM和Service
                    Me.LClose()
                    '继续关闭
                    e.Cancel = False
                    Init()

                    'NavigateToNextPage(PAGE.MAIN)
                Else
                    'Me.LOpen()
                    '停止关闭
                    e.Cancel = True
                End If
        End Select
    End Sub

#End Region

#Region "画面按钮相应事件"

    ''' <summary>
    ''' 退出按钮按下事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExits_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExits.Click
        ''Select Case Me.strState
        ''    Case StateType.STATEINIT, StateType.STATE1, StateType.STATE2, StateType.STATEERROR
        ''        Me.Close()
        ''    Case Else
        ''        数据保存()
        ''        If SaveChkResult() Then
        ''            关闭TCP服务器和COM口()
        ''            Me.LClose()
        ''            主页面跳转()
        ''            NavigateToNextPage(PAGE.MAIN)
        ''        End If

        ''End Select
        Me.Close()
    End Sub

    ''' <summary>
    ''' 外观寸法按钮按下事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFacedeInchchk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFacedeInchchk.Click
        Select Case Me.strState
            Case StateType.STATEINIT, StateType.STATE1, StateType.STATE2, StateType.STATEERROR
            Case Else
                lbltitle1.Visible = False
                lbltitle2.Visible = False
                lbltitle3.Visible = False
                '按钮寸法背景颜色变化
                Me.btnFacedeInchchk.BackColor = Color.LightBlue
                '按钮治具背景颜色变化
                Me.btnToolchk.BackColor = Color.LightGray
                'panle2清空
                Me.pnl2.Controls.Clear()
                '寸法检查数据取得
                If Not Me.strGoodsCode.Equals(String.Empty) AndAlso Not Me.strMakeNo.Equals(String.Empty) Then
                    '状态设定
                    Me.strState = StateType.STATE3
                    Me.getFacedeInchData()
                Else
                    '状态设定
                    Me.strState = StateType.STATEINIT
                End If
        End Select


    End Sub

    ''' <summary>
    ''' 治具检查按钮按下处理事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnToolchk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnToolchk.Click
        pub_haveToolsData = False
        Me.pnl2.Controls.Clear()
        Me.flgToolsVerify = Flag.OFF
        Select Case Me.strState
            Case StateType.STATEINIT, StateType.STATE1, StateType.STATE2, StateType.STATEERROR
            Case Else
                lbltitle1.Visible = True
                lbltitle2.Visible = True
                lbltitle3.Visible = True

                '如果治具确认未完了，需要重新确认。所以要进行治具条码照哦南宁歌新赋值
                If Me.flgToolsVerify = Flag.OFF AndAlso Me.dicToolsNo.Count > Me.lstBar.Count Then
                    lstBar.Clear()
                    For Each key As String In dicToolsNo.Keys
                        lstBar.Add(key)
                    Next


                End If
                '按钮寸法背景颜色变化
                Me.btnFacedeInchchk.BackColor = Color.LightGray
                '按钮治具背景颜色变化
                Me.btnToolchk.BackColor = Color.LightBlue
                '治具编号取得
                'strGoodsCode = "GO00001"
                'Me.strMakeNo = "12"
                'panle2清空
                Me.pnl2.Controls.Clear()
                If Not Me.strGoodsCode.Equals(String.Empty) AndAlso Not Me.strMakeNo.Equals(String.Empty) Then
                    '状态设定
                    Me.strState = StateType.STATE4
                    Me.getToolsListData()
                Else
                    '状态设定
                    Me.strState = StateType.STATEINIT
                End If
        End Select
    End Sub

#End Region

#Region "内部调用方法"

    ''' <summary>
    ''' 画面初始化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Init()
        '创建日志文件
        Me.CreateLogFile()
        '状态栏置文字提示
        Me.lblCheckState.Text = String.Empty
        '商品code
        strGoodsCode = String.Empty
        '商品作番
        strMakeNo = String.Empty
        '状态设定
        Me.strState = StateType.STATEINIT
        '检察员code
        strChkUserCd = String.Empty

        'dicToolsNo清空
        dicToolsNo.Clear()
        '有条形码治具验证flg设定
        flgToolsVerify = Flag.OFF
        ''无条形码治具验证flg设定
        'flgWToolsVerify = Flag.OFF
        '存放外观寸法分类检查信息 (key,state) state: 0 未检查  1 检查完了 2 不合格   
        dicCFInfo.Clear()
        '治具编号检查信息
        dicZJBInfo.Clear()
        '治具分类检查信息
        dicZJFInfo.Clear()
        '第一个治具编号ID
        strFirstID = String.Empty
        '当前检查作番的结果ID
        strReulstID = String.Empty
        '当前检查商品Code的最近结果ID
        strLastReulstID = String.Empty
        '当前检查商品Code的最近结果
        strLastReulst = String.Empty
        '当前检查商品Code的所属部门
        strDdepartment = String.Empty
        '最终检查结果
        strResult = String.Empty
        '商品Id
        strGoodsID = String.Empty
        strCursorTp = String.Empty

        '计数清零
        Me.intMDno = 0
        Me.intNGno = 0
        Me.intOWno = 0
        Me.intSDno = 0
        Me.intINITno = 0
        Me.intLCno = 0
        '画面清空
        Me.GroupBox1.Visible = False
        Me.GroupBox2.Visible = False
        'Me.GroupBox1.Controls.Clear()
        'Me.GroupBox2.Controls.Clear()
        Me.tabPalChk.Controls.Clear()
        'Me.Panel1.Controls.Clear()
        Me.pnl2.Controls.Clear()
        ' Me.lblCheckState.Text = MsgConst.M00051I
        Me.ShowStateMsg(MsgConst.M00051I)
        '标题
        Me.Text = "检查主页面"
        Me.btnFacedeInchchk.Enabled = True
        '治具按钮置灰
        Me.btnToolchk.Enabled = False

        Me.btnFacedeInchchk.BackColor = Color.LightBlue
        '按钮寸法背景颜色变化
        Me.btnToolchk.BackColor = Color.LightGray

        '继续检查Flg
        Me.flgCC = Flag.[OFF]
        '接受扫码Flg
        flgReceiveCd = Flag.ON

        If bc.GetQianpinInfo(Me.strReulstID) Then
            Me.cb1.Checked = False '没有欠品
        Else
            Me.cb1.Checked = True '有欠品

        End If
        If Me.strReulstID = "" Then
            Me.cb1.Enabled = False
        Else
            Me.cb1.Enabled = True
        End If


    End Sub
    ''' <summary>
    ''' 检查分类检查完了确认
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ChKFinishIsOrNo() As Boolean
        Dim infoTemp As New CheckInfo
        Dim dicTemp As New Dictionary(Of String, CheckInfo)
        Dim strMsg As String = String.Empty
        '1 检查员入力确认
        If Me.strChkUserCd.Equals(String.Empty) Then
            Me.strState = StateType.STATE5
            MessageBox.Show(MsgConst.M00031I)
            Return False
        End If
        '计数清零
        Me.intMDno = 0
        Me.intNGno = 0
        Me.intOWno = 0
        Me.intSDno = 0
        Me.intINITno = 0
        Me.intLCno = 0
        ' 2 检查确认是否存在漏检
        '2-1 外观寸法检查完了确认
        For Each pairKey As KeyValuePair(Of String, CheckInfo) In dicCFInfo
            infoTemp = pairKey.Value

            Select Case infoTemp.CheckResult
                Case CheckResult.INIT
                    Me.intINITno = Me.intINITno + 1
                Case CheckResult.OW
                    Me.intOWno = Me.intOWno + 1
                Case CheckResult.SD
                    Me.intSDno = Me.intSDno + 1
                Case CheckResult.MD
                    Me.intMDno = Me.intMDno + 1
                Case CheckResult.NG
                    Me.intNGno = Me.intNGno + 1
                Case CheckResult.LC
                    Me.intLCno = Me.intLCno + 1
                Case Else

            End Select

            'If infoTemp._checkResult.Equals(CheckResult.INIT) Then
            '    If MessageBox.Show(MsgConst.M00032I, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
            '        strResult = CheckResult.NG
            '        Return True
            '    Else
            '        Return False
            '    End If
            'ElseIf Not infoTemp._checkResult.Equals(CheckResult.INIT) AndAlso Not infoTemp._checkResult.Equals(CheckResult.OK) Then
            '    If MessageBox.Show(MsgConst.M00033I, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
            '        strResult = CheckResult.NG
            '        Return True
            '    Else
            '        Return False
            '    End If
            'End If
        Next

        '2-2 治具检查完了确认
        For Each pairKey As KeyValuePair(Of String, Dictionary(Of String, CheckInfo)) In dicZJFInfo
            dicTemp = pairKey.Value
            For Each pairKey1 As KeyValuePair(Of String, CheckInfo) In dicTemp
                infoTemp = pairKey1.Value

                Select Case infoTemp.CheckResult
                    Case CheckResult.INIT
                        Me.intINITno = Me.intINITno + 1
                    Case CheckResult.OW
                        Me.intOWno = Me.intOWno + 1
                    Case CheckResult.SD
                        Me.intSDno = Me.intSDno + 1
                    Case CheckResult.MD
                        Me.intMDno = Me.intMDno + 1
                    Case CheckResult.NG
                        Me.intNGno = Me.intNGno + 1
                    Case CheckResult.LC
                        Me.intLCno = Me.intLCno + 1
                    Case Else

                End Select
                'If infoTemp._checkResult.Equals(CheckResult.INIT) Then
                '    If MessageBox.Show(MsgConst.M00032I, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                '        strResult = CheckResult.NG
                '        Return True
                '    Else
                '        Return False
                '    End If
                'ElseIf Not infoTemp._checkResult.Equals(CheckResult.INIT) AndAlso Not infoTemp._checkResult.Equals(CheckResult.OK) Then
                '    If MessageBox.Show(MsgConst.M00033I, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                '        strResult = CheckResult.NG
                '        Return True
                '    Else
                '        Return False
                '    End If
                'End If
            Next
        Next
        '消息提示
        If Me.intINITno > 0 OrElse Me.intMDno > 0 OrElse Me.intNGno > 0 OrElse Me.intLCno > 0 Then
            If Me.intINITno > 0 OrElse Me.intLCno > 0 Then
                strMsg = "当前检查状况：" & vbCrLf & "漏检" & Me.intINITno + Me.intLCno & "件。" & vbCrLf
            End If
            If Me.intOWno > 0 Then
                strMsg = strMsg + "警告" & Me.intOWno & "件。" & vbCrLf
            End If
            If Me.intSDno > 0 Then
                strMsg = strMsg + "微欠品" & Me.intSDno & "件。" & vbCrLf
            End If
            If Me.intMDno > 0 Then
                strMsg = strMsg + "轻中重" & Me.intMDno & "件。" & vbCrLf
            End If
            If Me.intNGno > 0 Then
                strMsg = strMsg + "不合格" & Me.intNGno & "件。" & vbCrLf
            End If
            If MessageBox.Show(String.Format(MsgConst.M00042I, strMsg), "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                '结果赋值
                Me.strResult = CheckResult.NG
                Return True
            Else
                ShowStateMsg(String.Format(MsgConst.M00034I, strGoodsCode))
                Return False
            End If
        Else
            '结果赋值
            Me.strResult = CheckResult.OK
            Return True
        End If
    End Function
    ''' <summary>
    ''' 检查数据保存
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveChkResult() As Boolean
        Dim flg As Integer = Flag.OFF
        If Me.strLastReulstID.Equals(String.Empty) Then
            '检察员没有选择继承上次检查结果的场合
            If ChKFinishIsOrNo() Then
                '检查完了确认
                flg = Flag.ON
            Else
                flg = Flag.OFF
            End If
        Else
            '1 检查员入力确认
            If Me.strChkUserCd.Equals(String.Empty) Then
                Me.strState = StateType.STATE5
                MessageBox.Show(MsgConst.M00031I)
                Return False
            Else
                flg = Flag.ON
            End If
        End If

        If flg = Flag.ON Then
            '数据保存
            If ResultSave() = True Then
                '复原
                Init()
                'Me.LClose()
                'NavigateToNextPage(PAGE.CHK_MAIN)
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 解析扫描信息
    ''' </summary>
    ''' <param name="strCode"></param>
    ''' <remarks></remarks>
    Private Sub codeAnalysis(ByVal strCode As String)

        Select Case Me.strState
            Case StateType.STATEINIT
                '商品code作番为空的状态下，提示扫描开始标签
                '治具按钮不可用
                Me.btnToolchk.Enabled = False

            Case StateType.STATE1
                '接受商品code和商品作番
                '商品code赋值
                Me.strGoodsCode = strCode
                'Me.strGoodsCode = "W08520MKAE"
                If getGoodsId(Me.strGoodsCode).Equals(String.Empty) Then
                    'Me.lblCheckState.Text = MsgConst.M00040I
                    Me.ShowStateMsg(MsgConst.M00040I)
                    Exit Sub
                Else
                    Me.strGoodsID = getGoodsId(Me.strGoodsCode)
                End If

                '状态栏信息更新
                'Me.lblCheckState.Text = MsgConst.M00030I
                Me.ShowStateMsg(MsgConst.M00030I)
                '更改接受状态：准备接受商品作番
                Me.strState = StateType.STATE2
                '治具按钮不可用
                Me.btnToolchk.Enabled = False
            Case StateType.STATE2
                '接受商品作番
                '商品作番赋值
                Me.strMakeNo = strCode
                'Me.lblCheckState.Text = String.Format(MsgConst.M00034I, strGoodsCode)
                Me.ShowStateMsg(String.Format(MsgConst.M00034I, strGoodsCode))
                '更改接受状态：商品code及作番接受完毕。状态转为外观寸法检查下
                Me.strState = StateType.STATE3
        
                '批次检查完了确认
                ChkBatchOK()

                Me.cb1.Enabled = True

                '数据取得
                'GetAllData("INIT")

                ''部门查询
                'getDepartmentInfo()
                '继续检察判断
                'If ChkContinueChk() = True Then
                '    '治具按钮可用
                '    Me.btnToolchk.Enabled = True
                '    '外观寸法检查数据取得&页面显示
                '    getFacedeInchData()
                'Else
                '    '批次检查完了确认
                '    If ChkBatchOK() = False Then
                '        '数据临时保存
                '        If ResultSave() = False Then
                '            '保存失败,
                '            Me.strState = StateType.STATEERROR
                '            Me.lblCheckState.Text = String.Format(MsgConst.M00002D, "结果")
                '            Exit Sub
                '        End If
                '        '治具按钮可用
                '        Me.btnToolchk.Enabled = True
                '        '外观寸法检查数据取得&页面显示
                '        getFacedeInchData()
                '    End If
                'End If
            Case StateType.STATE3
                '外观寸法检查下,不需要扫码值，什么也不做

            Case StateType.STATE4
                '治具检查下
                '确认治具检查
                If Me.flgToolsVerify = Flag.OFF Then
                    '确认治具检查
                    chkTools(strCode)
                End If

            Case StateType.STATE5
                If strCode.StartsWith("JS") Then
                    '检查员code接受赋值
                    Me.strChkUserCd = strCode.Substring(2)
                    '日志记录
                    Me.ShowStateMsg(String.Format(MsgConst.M00053I, Me.strChkUserCd))
                    '数据保存
                    SaveChkResult()
                Else
                    'M00031I
                    MessageBox.Show(MsgConst.M00031I)
                End If

            Case StateType.STATEERROR
                'Me.lblCheckState.Text = MsgConst.M00040I
                Me.ShowStateMsg(MsgConst.M00040I)

            Case Else
        End Select
    End Sub
    ''' <summary>
    ''' 根据商品code取得商品ID
    ''' </summary>
    ''' <param name="strResultID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getGoodsId(ByVal strResultID As String) As String

        Dim dtGoodsId As DataTable = bc.GetGoodsIdByGoodsCd(Me.strGoodsCode).Tables("GoodsId")
        If dtGoodsId.Rows.Count > 0 Then
            Return dtGoodsId.Rows(0).Item(0).ToString
        Else
            Me.strState = StateType.STATEERROR
            MessageBox.Show(String.Format(MsgConst.M00035I, "商品CD"))
            Return ""

        End If
    End Function

    ''' <summary>
    ''' 结果表数据保存
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ResultSave(Optional ByVal strType As String = "") As Boolean
        '操作flg 0:插入（第一次调用，存储临时数据） 1 ：update （第二次调用，最终保存数据）2 插入（继承结果）
        Dim flg As Integer = 0

        '数据保存Hashtable
        Dim hsSearch As New Hashtable


        '再检检查员
        hsSearch.Add("up_check_user", String.Empty)
        If Me.strReulstID.Equals(String.Empty) Then
            '如果没有采番的话先采番处理
            Me.strReulstID = bcIndex.GetIndex(Consts.TYPEKBN.CHECK_RESULT)
            SetCDZUOFAN()
            Select Case strReulstID
                Case DBErro.InserError
                    MessageBox.Show(M00002D.Replace("{0}", "检查结果表采番"))
                    Return False
                Case DBErro.UpdateError
                    MessageBox.Show(M00003D.Replace("{0}", "检查结果表采番"))
                    Return False
                Case DBErro.GetIndexError
                    MessageBox.Show(M00001D.Replace("{0}", "检查结果表"))
                    Return False
                Case DBErro.GetIndexMaxError
                    MessageBox.Show(M00005D.Replace("{0}", "检查结果表"))
                    Return False
            End Select

            '数据整理
            If Not strLastReulst.Equals(String.Empty) Then
                'CASE1 继承结果最终保存
                '1 结果继承
                hsSearch.Add("result", Me.strLastReulst)
                '2 id
                hsSearch.Add("shareResult_id", Me.strLastReulstID)
                flg = 2
            Else
                'CASE2 临时结果保存
                '1 结果
                hsSearch.Add("result", "INIT")
                '2 id
                hsSearch.Add("shareResult_id", String.Empty)
                flg = 0
            End If

        Else
            If Me.flgCC = Flag.OFF Then
                'CASE3 最终保存(第二次调用 Update用)
                '1 结果
                hsSearch.Add("result", Me.strResult)
                '2 id
                hsSearch.Add("shareResult_id", String.Empty)
                flg = 1
            Else
                If strType.Equals("TEMP") Then
                    'CASE4 继续检查时的临时保存
                    '1 结果
                    hsSearch.Add("result", "INIT")
                    '2 id
                    hsSearch.Add("shareResult_id", String.Empty)
                    flg = 3
                Else
                    'CASE5 继续检查时的最终保存
                    '1 结果
                    hsSearch.Add("result", Me.strResult)
                    '2 id
                    hsSearch.Add("shareResult_id", String.Empty)
                    hsSearch("up_check_user") = Me.strChkUserCd
                    flg = 4
                End If

            End If

        End If

        '共同数据
        '1 结果ID
        hsSearch.Add("id", Me.strReulstID)
        '2 商品code
        hsSearch.Add("goods_id", Me.strGoodsID)
        '3 商品作番
        hsSearch.Add("make_number", Me.strMakeNo)
        '4 部门
        hsSearch.Add("department_cd", Me.getDepartmentInfo)
        '5 检查员
        hsSearch.Add("check_user", Me.strChkUserCd)
        '6 检查次数记录 
        hsSearch.Add("check_times", Me.getChkTimesInfo)
        '7 删除区分
        hsSearch.Add("delete_flg", "0")
        '8 备注
        hsSearch.Add("remarks", "")
        '是否是临时退出flg
        hsSearch.Add("continue_chk_flg", "0")
        '更新类型。
        hsSearch.Add("UpType", "INIT")


        'If flg <> 0 Then

        '    If MessageBox.Show(MsgConst.M00043I, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
        '        '是否是临时退出flg
        '        hsSearch("continue_chk_flg") = "1"
        '    Else
        '        hsSearch("continue_chk_flg") = "0"
        '    End If
        'End If
        Select Case flg
            Case 0
                '临时保存
                hsSearch("continue_chk_flg") = "3"
            Case 1
                '最终保存
                If Me.strResult.Equals(CheckResult.NG) Then
                    '只有在检查结果为NG的情况下才有提示框
                    If MessageBox.Show(MsgConst.M00043I, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                        '是否是临时退出flg
                        hsSearch("continue_chk_flg") = "1"
                    Else
                        hsSearch("continue_chk_flg") = "0"
                    End If
                Else
                    hsSearch("continue_chk_flg") = "0"
                End If
            Case 2
                '默认保存
                hsSearch("continue_chk_flg") = "2"
            Case 3
                '继续检查 临时保存
                hsSearch("UpType") = "TEMP"
                hsSearch("continue_chk_flg") = "3"
            Case 4
                '继续检查 最终保存
                hsSearch("UpType") = "FINAL"

                '最终保存
                If Me.strResult.Equals(CheckResult.NG) Then
                    '只有在检查结果为NG的情况下才有提示框
                    If MessageBox.Show(MsgConst.M00043I, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                        '是否是临时退出flg
                        hsSearch("continue_chk_flg") = "1"
                    Else
                        hsSearch("continue_chk_flg") = "0"
                    End If
                Else
                    hsSearch("continue_chk_flg") = "0"
                End If
        End Select
        'flg 变为插入更新Flg 0：插入 1：更新
        If flg = 2 Then
            flg = 0
        ElseIf flg = 3 OrElse flg = 4 Then
            flg = 1
        End If
        '数据保存
        Using scope As New TransactionScope(TransactionScopeOption.Required)
            Try
                If bc.SaveResult(flg, hsSearch) = 1 Then
                    scope.Complete()
                    Return True
                Else
                    scope.Dispose()
                    MessageBox.Show(String.Format(MsgConst.M00002D, "结果"))
                    Return False
                End If
            Catch ex As Exception
                scope.Dispose()
                Return False
            End Try
        End Using

        Return True

    End Function
    ''' <summary>
    ''' 是否继续检查判断
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ChkContinueChk() As Boolean
        '继续检查判断条件：1.商品CD相同 2.商品作番相同 3.检查当日内 4.继续检查flg为1
        '当前日期取得（yyyyMMdd）
        Dim strDateNow As String = String.Format("{0:yyyyMMdd}", bcCom.GetSystemDate())
        Dim dtReult As DataTable = bc.GetContinueChkInfo(Me.strGoodsCode, Me.strMakeNo, strDateNow).Tables("ContinueChkInfo")

        If dtReult.Rows.Count > 0 Then
            '存在继续检查数据
            '结果ID继承
            Me.strReulstID = dtReult.Rows(0).Item("id").ToString.Trim
            SetCDZUOFAN()
            Return True
        Else
            Return False
        End If

    End Function
    ''' <summary>
    ''' 今日间同批次处理确认
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ChkBatchOK()

        'Dim drResult As Integer = New DialogResult()
        'drResult = MessageBox.Show("请确认是否不存在欠品。" & vbCrLf & "不存在请单击Yes" & vbCrLf & "存在请单击No", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        'Select Case drResult
        '    Case DialogResult.Yes
        '        bc.Updatet_check_resultQinapinFlg(Me.strReulstID, "0") '没有欠品
        '    Case Else
        '        bc.Updatet_check_resultQinapinFlg(Me.strReulstID, "1") '没有欠品
        'End Select



        '同批次检查结果取得
        '同批次的确认条件：1：商品CODE相同 2：生产线相同 3：生产实际日相同 
        Dim dtReult As DataTable = bc.GetLastReultInfo(Me.strGoodsCode, Me.strMakeNo).Tables("ResultInfo")
        If dtReult.Rows.Count > 0 Then
            ''存在相同商品code的场合
            'If MessageBox.Show(MsgConst.M00036I, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.Cancel Then
            '    '用户选择继承上次的结果。给全局变量最近结果ID赋值，并提示扫描检查员码,
            '    '结果ID记录
            '    If dtReult.Rows(0).Item("shareResult_id").ToString.Equals(String.Empty) Then
            '        Me.strLastReulstID = dtReult.Rows(0).Item("id").ToString()
            '    Else
            '        Me.strLastReulstID = dtReult.Rows(0).Item("shareResult_id").ToString
            '    End If

            '    '结果记录
            '    Me.strLastReulst = dtReult.Rows(0).Item("result").ToString()
            '    '状态变更
            '    Me.strState = StateType.STATE5
            '    MessageBox.Show(MsgConst.M00031I)
            '    Return True
            'End If


            '存在同批次的情况
            Me.flgReceiveCd = Flag.OFF
            '1 结果集中同番号有无判断
            Dim strSearchKeys As String = String.Empty
            Dim dtNew As DataTable = dtReult.Clone
            strSearchKeys = "作番 = '" & Me.strMakeNo & "'"
            Dim dtRows As DataRow() = dtReult.Select(strSearchKeys, "结束时间 DESC")
            'strType: 0 存在同作番待判 1 存在同作番不存在待判  2 不存在同作番存在待判 3 不存在同作番不存在待判
            Dim strTypr As String = String.Empty
            Dim strKeyResultId As String = String.Empty
            If dtRows.Length > 0 Then
                '结果集中包含同作番数据
                Dim flgLC As Integer = 0
                For i As Integer = 0 To dtRows.Length - 1
                    '同作番添条件下，有无待判数据检查。continue_chk_flg：0 完了 1 待判 2 继承(最初的结果集已过滤)
                    If dtRows(i).Item("状态").ToString.Equals("待判") Then
                        'CASE1  存在待判的场合。选项：1 重新检查 2 继续检查  3 不检查
                        strTypr = "0"
                        Dim msgForm As New MsgConfirmForm(dtReult, strTypr, Me)
                        msgForm.strResultId = dtRows(i).Item("ID").ToString
                        msgForm.Show()
                        flgLC = 1
                        '住窗口禁用
                        Me.Enabled = False
                        pubxinshengchanStr = "新生产入力 待判"
                        Exit For
                    End If
                Next

                If flgLC = 0 Then
                    'CASE2  无待判数据检查。用户选项：1 重新检查 2 默认结果 （检查最近的一次结果，如果最近一条且检查状态为[完了]的结果）3 不检查
                    Dim j As Integer = 0
                    '循环所有的同作番数据，找到最近一条完了状态的结果记录。注：实现方式1：直接找到最近一条信息如果状态为完了则直接记录当前的结果ID，
                    '如果为默认；则记录其默认的的结果。缺点：数据可能不准确，优点：效率快。方式2：循环找到最近一条完了状态。
                    For j = 0 To dtRows.Length - 1
                        If dtRows(j).Item("状态").ToString.Equals("完了") Then
                            strKeyResultId = dtRows(j).Item("ID").ToString
                            Exit For
                        End If
                    Next
                    '因为存在同作番中所有数据都为默认状态，所以此时应该记录最近一条的默认状态的默认结果ID
                    If j = dtRows.Length Then
                        strKeyResultId = dtRows(0).Item("继承结果").ToString
                    End If
                    strTypr = "1"
                    Dim msgForm As New MsgConfirmForm(dtReult, strTypr, Me, strKeyResultId)
                    msgForm.Show()
                    pubxinshengchanStr = "新生产入力 无待判"
                    '住窗口禁用
                    Me.Enabled = False
                End If
            Else
                '最近一条的作番是否存在待判状态判断
                strSearchKeys = "作番 = '" & dtReult.Rows(0).Item("作番").ToString & "' AND 状态 = '待判'"
                Dim dtNewRows As DataRow() = dtReult.Select(strSearchKeys, "结束时间 DESC")
                If dtNewRows.Length > 0 Then
                    '最近一次检查的作番中存在待判状态的结果
                    'CASE3 无同作番有待判。用户选项：1 重新检查 2 不检查
                    strTypr = "2"
                    Dim msgForm As New MsgConfirmForm(dtReult, strTypr, Me)
                    msgForm.Show()
                    '住窗口禁用
                    pubxinshengchanStr = "新生产入力 最近一条的作番待判"
                    Me.Enabled = False
                Else
                    '最近一次检查的作番中不存在待判状态的结果
                    'CASE4 无同作番无待判。用户选项：1 重新检查 2 默认结果 3 不检查
                    '默认结果ID记录
                    For i As Integer = 0 To dtReult.Rows.Count - 1
                        If dtReult.Rows(i).Item("状态").ToString.Equals("完了") Then
                            strKeyResultId = dtReult.Rows(i).Item("ID").ToString
                            Exit For
                        End If
                    Next
                    strTypr = "3"
                    Dim msgForm As New MsgConfirmForm(dtReult, strTypr, Me, strKeyResultId)
                    msgForm.Show()
                    pubxinshengchanStr = "新生产入力 最近一条的作番无待判"
                    '住窗口禁用
                    Me.Enabled = False
                End If

                'If dtReult.Rows(0).Item("状态").ToString.Equals("待判") Then

                '    'CASE3 无同作番有待判。用户选项：1 重新检查 2 不检查
                '    strTypr = "2"
                '    Dim msgForm As New MsgConfirmForm(dtReult, strTypr, Me)
                '    msgForm.Show()
                '    '住窗口禁用
                '    Me.Enabled = False
                'Else
                '    'CASE4 无同作番无待判。用户选项：1 重新检查 2 默认结果 3 不检查
                '    '默认结果ID记录
                '    strKeyResultId = dtReult.Rows(0).Item("ID").ToString
                '    strTypr = "3"
                '    Dim msgForm As New MsgConfirmForm(dtReult, strTypr, Me, strKeyResultId)
                '    msgForm.Show()
                '    '住窗口禁用
                '    Me.Enabled = False
                'End If
                'strSearchKeys = "状态 = '待判' "
                'dtRows = dtReult.Select(strSearchKeys, "结束时间 DESC")

                'If dtRows.Length > 0 Then
                '    'CASE3 无同作番有待判。用户选项：1 重新检查 2 不检查
                '    strTypr = "2"
                '    Dim msgForm As New MsgConfirmForm(dtReult, strTypr, Me)
                '    msgForm.Show()
                '    '住窗口禁用
                '    Me.Enabled = False
                'Else
                '    'CASE4 无同作番无待判。用户选项：1 重新检查 2 默认结果 3 不检查
                '    strTypr = "3"
                '    Dim msgForm As New MsgConfirmForm(dtReult, strTypr, Me)
                '    msgForm.Show()
                '    '住窗口禁用
                '    Me.Enabled = False
                'End If
            End If
        Else


            dtReult = bc.GetLastReultInfoFromt_check_result(Me.strGoodsCode, Me.strMakeNo).Tables(0)

            If dtReult.Rows.Count > 0 Then


                For i As Integer = 0 To dtReult.Rows.Count - 1
                    '同作番添条件下，有无待判数据检查。continue_chk_flg：0 完了 1 待判 2 继承(最初的结果集已过滤)
                    If dtReult.Rows(i).Item("状态").ToString.Equals("待判") Then
                        'CASE1  存在待判的场合。选项：1 重新检查 2 继续检查  3 不检查
                        Dim strTypr As String = "0"
                        Dim msgForm As New MsgConfirmForm(dtReult, strTypr, Me)
                        msgForm.strResultId = dtReult.Rows(i).Item("ID").ToString
                        msgForm.Show()

                        '住窗口禁用
                        Me.Enabled = False

                        Exit For
                        Me.ShowStateMsg(String.Format(MsgConst.M00034I, strGoodsCode))
                    End If

                    ShowStateMsg(String.Format("商品CD：{0} 检查完了", strGoodsCode))

                    If i = dtReult.Rows.Count - 1 Then

                        If MsgBox("新生产未入力（新生产不存在同批次数据），但是已经检查完了,点击OK重新检查，不检查请点击取消.", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                            Me.ShowStateMsg(String.Format(MsgConst.M00034I, strGoodsCode))
                            'CASE1  存在待判的场合。选项：1 重新检查 2 继续检查  3 不检查
                            Dim strTypr As String = "0"
                            Dim msgForm As New MsgConfirmForm(dtReult, strTypr, Me)
                            msgForm.strResultId = dtReult.Rows(i).Item("ID").ToString
                            msgForm.Show()

                            '住窗口禁用
                            Me.Enabled = False
                            pubxinshengchanStr = "新生产未入力 检查完了"
                        Else
                            Init()
                            pubxinshengchanStr = "新生产未入力 初始化"
                        End If

                    Else

                    End If
                Next



            Else

                pubxinshengchanStr = "新生产未入力 未检查"


                '不存在同批次数据时
                MsgBox("1.新生产不存在同批次数据" & vbCrLf & _
                       "2.检查结果表中没有临时的检查数据，所以进行全新检查")

                '数据取得
                If GetAllData("INIT") = False Then
                    Exit Sub
                End If

                Me.strReulstID = String.Empty
                SetCDZUOFAN()
                '数据临时保存
                If ResultSave() = False Then
                    '保存失败,
                    Me.strState = StateType.STATEERROR
                    ShowStateMsg(String.Format(MsgConst.M00002D, "结果"))
                    'Me.lblCheckState.Text = String.Format(MsgConst.M00002D, "结果")
                    Exit Sub
                End If

                '治具按钮可用
                Me.btnToolchk.Enabled = True
                '外观寸法检查数据取得&页面显示
                getFacedeInchData()

            End If






        End If



    End Sub

    Public pubxinshengchanStr As String


    ''' <summary>
    ''' 获得部门信息
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getDepartmentInfo() As String
        Dim dtDepment As DataTable = bc.GetDepartmentInfo(Me.strGoodsCode).Tables("DepartmentInfo")
        If dtDepment.Rows.Count > 0 Then
            Return dtDepment.Rows(0).Item("department_cd").ToString
        Else
            Me.strState = StateType.STATEERROR
            MessageBox.Show(String.Format(MsgConst.M00035I, "部门信息"))
        End If
        Return ""
    End Function
    ''' <summary>
    ''' 获得同作番检查次数
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getChkTimesInfo() As String
        Dim intChkTimes As Integer
        Dim dtChkTimest As DataTable = bc.GetChkTimesInfo(Me.strGoodsID, Me.strMakeNo).Tables("CheckTimesInfo")
        If dtChkTimest.Rows.Count > 0 Then
            intChkTimes = CInt(dtChkTimest.Rows(0).Item("check_times").ToString) + 1
            Return intChkTimes.ToString
        Else
            Return "1"
        End If

    End Function
    ''' <summary>
    ''' 有条码治具确认
    ''' </summary>
    ''' <param name="strCode"></param>
    ''' <remarks></remarks>
    Private Sub chkTools(ByVal strCode As String)

        If lstBar.Count > 0 Then
            If dicToolsNo.ContainsKey(strCode) Then
                '治具扫码正确的场合
                '颜色变化
                Me.changColor(dicToolsNo(strCode).ToString)
                '如果存在移除
                lstBar.Remove(strCode)
            Else
                '治具扫码错误的场合，提示信息
                MessageBox.Show(MsgConst.M00059I)
                Exit Sub
            End If
        End If
        '检查治具确认完了
        IsChkToolsFinish()
        If Me.flgToolsVerify = Flag.ON Then
            '默认第一个linkLabel变色
            For Each c As Control In Me.tabPalChk.Controls
                If c.GetType Is GetType(LinkLabel) AndAlso c.Tag.ToString.Equals(strFirstID) Then
                    c.BackColor = Color.Yellow
                    Exit For
                End If
            Next
            '默认取第一个治具编号下的分类信息
            Me.ShowClassifyOfTools(Me.strFirstID)
        End If


    End Sub

    ''' <summary>
    ''' 颜色变化
    ''' </summary>
    ''' <param name="strToolID"></param>
    ''' <remarks></remarks>
    Private Sub changColor(ByVal strToolID As String)
        'tabPalChk.Controls

        Dim checkResult As Integer = -1
        '遍历控件
        For Each c As Control In Me.tabPalChk.Controls

            If c.Name.Equals(strToolID) Then

                If c.GetType Is GetType(LinkLabel) Then
                    c.Enabled = True
                End If
                If pub_haveToolsData Then

                    checkResult = bc.CheckToolsOK(Me.strReulstID, strToolID)


                    If c.GetType Is GetType(LinkLabel) Then
                        c.ForeColor = Color.LightGreen
                    End If




                End If

                If c.GetType Is GetType(Label) Then

                    If CheckResult = 0 Then '没有数据或者 有NG数据
                        c.BackColor = Color.Red
                    Else
                        c.BackColor = Color.LightGreen
                    End If


                End If

            End If

        Next
    End Sub
    ''' <summary>
    ''' 颜色变化
    ''' </summary>
    ''' <param name="strToolID"></param>
    ''' <remarks></remarks>
    Private Sub changColor2(ByVal strToolID As String)
        'tabPalChk.Controls

        Dim checkResult As Integer = -1
        '遍历控件
        For Each c As Control In Me.tabPalChk.Controls

            If c.Name.Equals(strToolID) Then

                If c.GetType Is GetType(LinkLabel) Then
                    If c.Enabled = True Then


                        If pub_haveToolsData Then
                            checkResult = bc.CheckToolsOK(Me.strReulstID, strToolID)

                            c.ForeColor = Color.LightGreen

                        End If



                    End If
                End If

                If c.GetType Is GetType(Label) Then

                    If bc.CheckToolsOK(Me.strReulstID, strToolID) = 0 Then '没有数据或者 有NG数据
                        c.BackColor = Color.Red
                    Else
                        c.BackColor = Color.LightGreen
                    End If


                End If

            End If

        Next
    End Sub
    ''' <summary>
    ''' 显示当前治具下的分类列表
    ''' </summary>
    ''' <param name="strToolID"></param>
    ''' <remarks></remarks>
    Private Sub ShowClassifyOfTools(ByVal strToolID As String)
        Dim flgL As Integer = 0
        Dim iCount As Integer = 0
        '清空
        Me.pnl2.Controls.Clear()
        Me.pnl2.Show()
        Me.GroupBox2.Visible = True
        Me.GroupBox2.Text = "检查项目"
        Dim x As Integer = 0
        Dim y As Integer = 0
        Me.pnl2.Focus()
        Dim dicTemp As New Dictionary(Of String, CheckInfo)
        '读取临时存储信息
        dicTemp = dicZJFInfo(strToolID)
        For Each pairKey As KeyValuePair(Of String, CheckInfo) In dicTemp

            '动态生成Linktable
            Dim link As New LinkLabel
            link.Text = pairKey.Value._classifyName
            link.Tag = pairKey.Value
            link.Name = strToolID
            link.Font = New Font(MS_MSLableFontFamily, MS_Font_Size - 1, FontStyle.Regular)
            link.BorderStyle = BorderStyle.FixedSingle
            link.Location = New Point(x, y)
            link.Width = 230
            '第一次进来光标设置
            If iCount = 0 And Me.strCursorTp.Equals(String.Empty) Then
                link.Focus()
            End If
            '检查状态判断：根据状态不同显示颜色不同
            Select Case pairKey.Value._checkResult

                Case CheckResult.INIT
                    '未检查场合无颜色

                Case CheckResult.OK
                    '检查完了 绿色
                    link.BackColor = Color.LightGreen
                Case CheckResult.NG
                    '不合格或存在未检查项目 红色
                    link.BackColor = Color.Red
                Case CheckResult.LC
                    link.BackColor = Color.Red
            End Select

            AddHandler link.LinkClicked, AddressOf lnk_LinkClicked '利用AddHandler关键字预订给定事件lnk_LinkClicked
            Me.pnl2.Controls.Add(link)
            '光标设置
            If flgL = 0 Then
                If pairKey.Value.ClassifyCd.ToString.Equals(Me.strCursorTp) Then
                    flgL = 1
                End If
            ElseIf flgL = 0 Then
                link.Focus()
                flgL = 2
            Else

            End If
            y = y + 35
            iCount = iCount + 1
        Next
    End Sub
    ''' <summary>
    ''' 建立log文件储存文件夹
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateLogFile()
        Dim strDateNow As String = String.Format("{0:yyyyMMdd}", bcCom.GetSystemDate())
        Try
            If Directory.Exists(Me.strLogPath + strDateNow) = False Then
                '根路径下以当天为文件夹
                Directory.CreateDirectory(Me.strLogPath + strDateNow)
            End If

            Dim strIP As String = System.Net.Dns.GetHostName
            'txt文件设置
            Me.strLogTxtPath = Me.strLogPath + strDateNow + "\" + strIP + ".txt"
        Catch ex As Exception
            MessageBox.Show(MsgConst.M00052I)
            Exit Sub
        End Try
    End Sub

    ''' <summary>
    ''' 检查状态更新
    ''' </summary>
    ''' <param name="strMsg"></param>
    ''' <remarks></remarks>
    Private Sub ShowStateMsg(ByVal strMsg As String)
        '状态变更
        Me.lblCheckState.Text = strMsg
        SetCDZUOFAN()
        Dim strDateNow As String = bcCom.GetSystemDate().ToLongTimeString
        '日志追加
        Dim fw As New StreamWriter(Me.strLogTxtPath, True)
        fw.WriteLine(strDateNow + "  " + strMsg)
        fw.Close()
    End Sub
#End Region

#Region "数据取得&画面显示"
    ''' <summary>
    ''' 取得所有数据
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetAllData(ByVal strGetType As String) As Boolean
        '外观寸法
        If dicCFInfo.Count = 0 Then
            '没有临时信息的场合，从数据库读取数据并保存到临时数据中
            Dim dtClassify As DataTable = bc.GetClassifyList(Me.strGoodsID).Tables("classifyList")
            If dtClassify.Rows.Count > 0 Then
                'Me.strCursorTp = dtClassify.Rows(0).Item("classify_id").ToString
                For i As Integer = 0 To dtClassify.Rows.Count - 1
                    Dim infoTemp As New CheckInfo
                    '分类code
                    infoTemp.ClassifyCd = dtClassify.Rows(i).Item("classify_id").ToString
                    '分类名
                    infoTemp.ClassifyName = dtClassify.Rows(i).Item("classify_name").ToString
                    '图片id
                    infoTemp.PictureCode = dtClassify.Rows(i).Item("picture_id").ToString()
                    '种类cd
                    infoTemp.KindCd = "01"
                    '治具code
                    infoTemp.ToolsID = String.Empty
                    '检查状态初始化
                    If strGetType.Equals("INIT") Then
                        '非继续检查场合
                        infoTemp.CheckResult = CheckResult.INIT
                    Else
                        '继续检查场合
                        Dim dtClassifyResult As DataTable = bc.GetChkDetailResult(Me.strGoodsID, Me.strReulstID, dtClassify.Rows(i).Item("classify_id").ToString).Tables("DetailResultInfo")
                        If dtClassifyResult.Rows.Count > 0 Then
                            Dim dtRows As DataRow() = dtClassifyResult.Select("result IN ('INIT','IN') ")

                            If dtRows.Length > 0 Then
                                '分类中存在漏检项
                                infoTemp.CheckResult = CheckResult.LC
                            Else
                                dtRows = dtClassifyResult.Select("result IN ('MD','NG') ")
                                If dtRows.Length > 0 Then
                                    '存在NG的检查项目
                                    infoTemp.CheckResult = CheckResult.NG
                                Else
                                    infoTemp.CheckResult = CheckResult.OK
                                End If

                            End If
                        Else
                            '初始状态
                            infoTemp.CheckResult = CheckResult.INIT
                        End If

                    End If


                    dicCFInfo.Add(infoTemp._classifyCd, infoTemp)
                Next
            Else
                'Me.strState = StateType.STATEERROR
                'MsgBox(String.Format(MsgConst.M00037I, strGoodsCode), MsgBoxStyle.Critical, "错误信息")
                'Exit Sub
            End If

        End If
        '治具

        '没有临时信息的场合，从数据库读取数据并保存到临时数据中
        If dicZJBInfo.Count = 0 Then
            '治具List清空
            If dicToolsNo.Count > 0 Then
                dicToolsNo.Clear()
            End If

            '治具编号信息取得
            Dim dtToolsList As DataTable = bc.GetToolsNoList(Me.strGoodsID).Tables("ToolsList")

            If dtToolsList.Rows.Count > 0 Then

                For i As Integer = 0 To dtToolsList.Rows.Count - 1

                    '治具List判断添加
                    If dtToolsList.Rows(i).Item("barcode_flg").ToString.Equals("1") Then
                        If Not dicToolsNo.ContainsKey(dtToolsList.Rows(i).Item("barcode").ToString) Then
                            '有条形码的场合
                            dicToolsNo.Add(dtToolsList.Rows(i).Item("barcode").ToString, dtToolsList.Rows(i).Item("tools_id").ToString)
                        End If
                     End If
                    '治具列表信息填充 Key：治具ID
                    Dim infoTemp As New List(Of String)
                    '治具ID 0
                    infoTemp.Add(dtToolsList.Rows(i).Item("tools_id").ToString)
                    '治具编号 1
                    infoTemp.Add(dtToolsList.Rows(i).Item("tools_no").ToString)
                    '条形码flg 2
                    infoTemp.Add(dtToolsList.Rows(i).Item("barcode_flg").ToString)
                    '条形码 3
                    infoTemp.Add(dtToolsList.Rows(i).Item("barcode").ToString)
                    '备注 4
                    infoTemp.Add(dtToolsList.Rows(i).Item("remarks").ToString)

                    If dicZJBInfo.ContainsKey(dtToolsList.Rows(i).Item("tools_id").ToString) Then
                        Continue For
                    End If

                    dicZJBInfo.Add(dtToolsList.Rows(i).Item("tools_id").ToString, infoTemp)
                    '该治具列表下的分类list填充 
                    Dim dicTemp As New Dictionary(Of String, CheckInfo)
                    Dim dtClassify As DataTable = bc.GetClassifyList(Me.strGoodsID, dtToolsList.Rows(i).Item("tools_id").ToString).Tables("classifyList")

                    If dtClassify.Rows.Count > 0 Then
                        'Me.strCursorTp = dtClassify.Rows(0).Item("classify_id").ToString
                        For j As Integer = 0 To dtClassify.Rows.Count - 1
                            Dim CinfoTemp As New CheckInfo
                            '分类code
                            CinfoTemp.ClassifyCd = dtClassify.Rows(j).Item("classify_id").ToString
                            '分类名
                            CinfoTemp.ClassifyName = dtClassify.Rows(j).Item("classify_name").ToString
                            '图片id
                            CinfoTemp.PictureCode = dtClassify.Rows(j).Item("picture_id").ToString()
                            '种类cd
                            CinfoTemp.KindCd = "02"
                            '治具code
                            CinfoTemp.ToolsID = dtToolsList.Rows(i).Item("tools_id").ToString

                            '检查状态初始化
                            If strGetType.Equals("INIT") Then
                                '非继续检查场合
                                CinfoTemp.CheckResult = CheckResult.INIT
                            Else
                                '继续检查场合
                                Dim dtClassifyResult As DataTable = bc.GetChkDetailResult(Me.strGoodsID, Me.strReulstID, dtClassify.Rows(j).Item("classify_id").ToString).Tables("DetailResultInfo")
                                If dtClassifyResult.Rows.Count > 0 Then
                                    Dim dtRows As DataRow() = dtClassifyResult.Select("result IN ('INIT','IN') ")
                                    If dtRows.Length > 0 Then
                                        '存在漏检项
                                        CinfoTemp.CheckResult = CheckResult.LC
                                    Else
                                        dtRows = dtClassifyResult.Select(" result IN ('MD','NG') ")
                                        If dtRows.Length > 0 Then
                                            CinfoTemp.CheckResult = CheckResult.NG
                                        Else
                                            CinfoTemp.CheckResult = CheckResult.OK
                                        End If
                                    End If
                                Else
                                    CinfoTemp.CheckResult = CheckResult.INIT
                                End If
                                'Dim dtRows As DataRow() = dtClassifyResult.Select("result IN ('MD','NG','IN') ")
                                'If dtRows.Length > 0 Then
                                '    '存在NG的检查项目
                                '    CinfoTemp.CheckResult = CheckResult.NG
                                'Else
                                '    CinfoTemp.CheckResult = CheckResult.OK
                                'End If

                            End If

                            dicTemp.Add(dtClassify.Rows(j).Item("classify_id").ToString, CinfoTemp)
                        Next
                    End If
                    '数据添加
                    dicZJFInfo.Add(dtToolsList.Rows(i).Item("tools_id").ToString, dicTemp)
                Next
                '第一个治具ID赋值
                strFirstID = dtToolsList.Rows(0).Item("tools_id").ToString
            Else
                'Me.strState = StateType.STATEERROR
                'MsgBox(String.Format(MsgConst.M00038I, strGoodsCode), MsgBoxStyle.Critical, "错误信息")
            End If

        End If
        '数据有无判断
        If dicCFInfo.Count = 0 AndAlso dicZJFInfo.Count = 0 Then
            '状态变更
            Me.strState = StateType.STATEERROR
            '信息提示，log记录
            ShowStateMsg(String.Format(MsgConst.M00057I, strGoodsCode))
            MsgBox(String.Format(MsgConst.M00057I, strGoodsCode), MsgBoxStyle.Critical, "错误信息")
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' 外观寸法检查数据取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getFacedeInchData()
        Dim flgL As Integer = 0

        'pnl1初始化
        Dim iCount As Integer = 0
        Dim tabIndexItem As Integer = 0
        Me.tabPalChk.Controls.Clear()
        Me.tabPalChk.ColumnCount = 1
        Me.GroupBox1.Visible = True
        Me.GroupBox2.Visible = False
        Me.GroupBox1.Text = "检查项目"
        GroupBox1.Focus()
        'Me.Panel1.Focus()
        'tabPalChk.Focus()
        'SendKeys.Send("{TAB}")

        '读取临时存储信息
        For Each pairKey As KeyValuePair(Of String, CheckInfo) In dicCFInfo

            '动态生成Linktable
            Dim link As New LinkLabel
            link.Text = pairKey.Value._classifyName
            '寸法区别
            link.Name = "CF"
            link.Tag = pairKey.Value
            link.AutoSize = True
            link.Dock = DockStyle.Fill
            link.TextAlign = ContentAlignment.MiddleCenter
            link.TabIndex = iCount
            link.Enabled = True
            link.Visible = True

            'Link字体设置
            link.Font = New Font(MS_MSLableFontFamily, 30, FontStyle.Regular)
            link.BorderStyle = BorderStyle.FixedSingle
            Me.tabPalChk.CellBorderStyle = TableLayoutPanelCellBorderStyle.None


            '检查状态判断：根据状态不同显示颜色不同
            Select Case pairKey.Value.CheckResult

                Case CheckResult.INIT
                    '未检查场合无颜色

                Case CheckResult.OK
                    '检查完了 绿色
                    link.BackColor = Color.LightGreen
                Case CheckResult.NG
                    '不合格或存在未检查项目 红色
                    link.BackColor = Color.Red
                Case CheckResult.LC
                    link.BackColor = Color.Red
            End Select

            AddHandler link.LinkClicked, AddressOf lnk_LinkClicked '利用AddHandler关键字预订给定事件lnk_LinkClicked
            Me.tabPalChk.Controls.Add(link, 0, iCount)
            '第一次进来光标设置
            If iCount = 0 And Me.strCursorTp.Equals(String.Empty) Then
                link.Focus()
            End If

            '光标设置
            If flgL = 0 Then
                If pairKey.Value.ClassifyCd.ToString.Equals(Me.strCursorTp) Then
                    flgL = 1
                End If
            ElseIf flgL = 1 Then
                link.Focus()
                tabIndexItem = iCount
                flgL = 2
            Else

            End If
            iCount = iCount + 1
        Next

        'If tabIndexItem <> 0 Then
        '    SendKeys.Send("{TAB " & tabIndexItem & "}")
        'End If
    End Sub

    Public pub_haveToolsData As Boolean = False

    ''' <summary>
    ''' 治具列表获取
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getToolsListData()

        '坐标设置
        Dim x As Integer = 0
        Dim y As Integer = 30


        '控件初期化
        Me.tabPalChk.Controls.Clear()
        Me.tabPalChk.ColumnCount = 3
        Me.GroupBox1.Visible = True
        Me.GroupBox2.Visible = False
        Me.GroupBox1.Text = "治具编号"

        Dim iCount As Integer = 0

        'Dim havedata As Boolean = False

        For Each pairKey As KeyValuePair(Of String, List(Of String)) In dicZJBInfo
            If pub_haveToolsData = False Then
                pub_haveToolsData = bc.CheckToolsHaveData(Me.strReulstID, pairKey.Value.Item(0).ToString.Trim)
            End If

        Next


        '读取临时存储信息
        For Each pairKey As KeyValuePair(Of String, List(Of String)) In dicZJBInfo
            '动态生成Linktable
            '治具编号
            Dim isCheck As Integer
            Dim linkBH As New LinkLabel
            '子窗口跳转区别用
            linkBH.Name = pairKey.Key
            linkBH.Text = pairKey.Value.Item(1).ToString.Trim
            linkBH.Tag = pairKey.Key
            linkBH.AutoSize = True
            linkBH.Dock = DockStyle.Fill
            linkBH.TextAlign = ContentAlignment.MiddleCenter
            linkBH.Font = New Font(MS_MSLableFontFamily, MS_Font_Size, FontStyle.Regular)
            linkBH.Enabled = False

            If pub_haveToolsData Then
                'tools_id 0
                isCheck = bc.CheckToolsOK(Me.strReulstID, pairKey.Value.Item(0).ToString.Trim)
                If isCheck = 1 OrElse isCheck = 2 Then
                    linkBH.ForeColor = Color.LightGreen
                End If

                If isCheck = 0 Then
                    linkBH.Enabled = False
                End If
            End If





            AddHandler linkBH.LinkClicked, AddressOf lnk1_LinkClicked
            Me.tabPalChk.Controls.Add(linkBH, 0, iCount)
            '输入框生成
            If pairKey.Value.Item(2).ToString.Equals("0") Then

                Dim ct As New CustomTextBox
                If Me.flgToolsVerify = Flag.ON Then
                    ct.txtBox.Text = dicZJBInfo(pairKey.Key).Item(3).ToString
                    ct.txtBox.ReadOnly = True
                End If

                ct.Tag = pairKey.Key
                ct.ReferenceBaseForm = Me
                ct.IsTools = True
                ct.BenchmarkValue1 = dicZJBInfo(pairKey.Key).Item(3).ToString
                linkBH.Font = New Font(MS_MSLableFontFamily, MS_Font_Size - 2, FontStyle.Regular)
                AddHandler ct.CustomTextBox_LostFocus, AddressOf txtBox_Leaved
                Me.tabPalChk.Controls.Add(ct, 1, iCount)
                If iCount = 0 Then
                    ct.Focus()
                End If
            End If
            '备注
            Dim linkBZ As New Label
            If Me.flgToolsVerify = Flag.ON OrElse isCheck = 1 OrElse isCheck = 2 Then
                linkBZ.BackColor = Color.LightGreen

                If isCheck = 2 Then
                    linkBZ.ForeColor = Color.Red
                End If

            Else
                linkBZ.BackColor = Color.Red
            End If

            '变更成 barcode
            linkBZ.Text = pairKey.Value.Item(4)
            linkBZ.Name = pairKey.Key
            linkBZ.AutoSize = True
            linkBZ.Dock = DockStyle.Fill
            linkBZ.TextAlign = ContentAlignment.MiddleCenter
            linkBH.Font = New Font(MS_MSLableFontFamily, MS_Font_Size, FontStyle.Regular)
            Me.tabPalChk.Controls.Add(linkBZ, 2, iCount)

            iCount = iCount + 1
        Next

        '判断治具是否确认完了
        If Me.flgToolsVerify = Flag.ON Then
            '默认第一个linkLabel变色
            For Each c As Control In Me.tabPalChk.Controls
                If c.GetType Is GetType(LinkLabel) AndAlso c.Tag.ToString.Equals(strFirstID) Then
                    c.BackColor = Color.Yellow
                    Exit For
                End If
            Next
            '确认完了的场合
            'ShowClassifyOfTools(strFirstID)
        End If
        'If Me.flgToolsVerify = Flag.[ON] AndAlso Me.flgWToolsVerify = Flag.[ON] Then
        '    '确认完了的场合
        '    ShowClassifyOfTools(strFirstID)
        'End If
    End Sub
#End Region

#Region "控件处理事件"


    ''' <summary>
    ''' linkLable点击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lnk_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        Dim lnk As LinkLabel = DirectCast(sender, LinkLabel)
        Dim strType As String
        lnk.BackColor = Color.Yellow
        strType = lnk.Name.ToString
        Dim info As CheckInfo = CType(lnk.Tag, CheckInfo)
        Me.LClose()
        Application.DoEvents()

        'If MessageBox.Show("是否要打开检查画面", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then

        Dim formDetail As New CheckDetailForm(Me, info, strType)
        formDetail.Owner = Me
        formDetail.Show()
        Me.Hide()

        ' End If





    End Sub

    ''' <summary>
    ''' 治具检查下，Panel1点击事件处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lnk1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        Dim lnk As LinkLabel = DirectCast(sender, LinkLabel)
        If Me.flgToolsVerify = Flag.ON OrElse lnk.ForeColor = Color.LightGreen Then

            '颜色变更
            '循环控件看颜色是否全部变为浅绿
            For Each c As Control In Me.tabPalChk.Controls
                If c.GetType Is GetType(LinkLabel) Then
                    c.BackColor = Nothing
                End If
            Next
            lnk.BackColor = Color.Yellow
            '显示此治具编号下的分类列表
            ShowClassifyOfTools(lnk.Tag.ToString)
        End If
    End Sub

    ''' <summary>
    ''' 无条形码治具控件（textBox）事件处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtBox_Leaved(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtBox As CustomTextBox = DirectCast(sender, CustomTextBox)
        '结果判断
        If txtBox.CheckResult.Equals(CheckResult.OK) Then
            'OK的场合
            '改变列表的颜色
            txtBox.BackColor = Color.LightGreen
            Me.changColor(txtBox.Tag.ToString)

            '    For Each c As Control In Me.tabPalChk.Controls
            '        If c.Controls.GetType Is GetType(LinkLabel) Then
            '            Dim lklblTemp As LinkLabel = DirectCast(c, LinkLabel)
            '            If lklblTemp.Tag.ToString.Equals(txtBox.Tag.ToString) Then
            '                lklblTemp.BackColor = Color.LightGreen
            '                Exit For
            '            End If
            '        End If
            '    Next

        Else
            'NG的场合
            Exit Sub
        End If

        'Dim flg As Integer = 0
        'Dim txtBoxTemp As TextBox
        'If txtBox.Text.ToString.Equals(dicZJBInfo(txtBox.Tag.ToString).Item(3).ToString) Then
        '    txtBox.ReadOnly = True
        '    'txtBox.BackColor = Color.Blue
        '    '遍历控件改变列表的颜色
        '    For Each c As Control In Me.tabPalChk.Controls
        '        If c.Controls.GetType Is GetType(LinkLabel) Then
        '            Dim lklblTemp As LinkLabel = DirectCast(c, LinkLabel)
        '            If lklblTemp.Tag.ToString.Equals(txtBox.Tag.ToString) Then
        '                lklblTemp.BackColor = Color.LightGreen
        '                Exit For
        '            End If
        '        End If
        '    Next
        'End If


        '遍历tabPalChk,找出TextBox控件判断是否是ReadOnly（已验证）
        'If Me.flgToolsVerify = Flag.ON Then
        '    For Each c As Control In Me.tabPalChk.Controls
        '        If c.Controls.GetType Is GetType(TextBox) Then
        '            txtBoxTemp = DirectCast(c, TextBox)
        '            If txtBoxTemp.ReadOnly = False Then
        '                '如果是改变flg
        '                flg = 1
        '                Exit For
        '            End If
        '        End If
        '    Next
        'End If

        '是否验证完了判断
        IsChkToolsFinish()

        If Me.flgToolsVerify = Flag.ON Then
            '第一个LinkLabel变色
            '默认第一个linkLabel变色
            For Each c As Control In Me.tabPalChk.Controls
                If c.GetType Is GetType(LinkLabel) AndAlso c.Tag.ToString.Equals(strFirstID) Then
                    c.BackColor = Color.Yellow
                    Exit For
                End If
            Next
            ShowClassifyOfTools(Me.strFirstID)
        End If
    End Sub

    ''' <summary>
    ''' 判断治具检查是否确认完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub IsChkToolsFinish()



        Dim flg1 As Integer = 0
        Dim flg2 As Integer = 0


        For Each c As Control In Me.tabPalChk.Controls

            '循环控件看颜色是否全部变为黄色
            If c.GetType Is GetType(LinkLabel) Then
                Dim lbl As LinkLabel = DirectCast(c, LinkLabel)
                If lbl.ForeColor = Color.LightGreen Then
                    flg1 = 1
                Else
                    flg1 = -1
                End If
            End If

            '循环控件看颜色是否全部变为浅绿
            If c.GetType Is GetType(Label) Then
                Dim lbl As Label = DirectCast(c, Label)
                If lbl.BackColor = Color.LightGreen Then
                    flg2 = 1
                Else
                    flg2 = -1
                End If
            End If

            If flg1 <> 0 AndAlso flg2 <> 0 Then

                If flg1 = 1 OrElse flg2 = 1 Then

                    flg1 = 0
                    flg2 = 0
                Else
                    Me.flgToolsVerify = Flag.OFF
                    Exit Sub

                End If




            End If


        Next




        Me.flgToolsVerify = Flag.ON



        ''循环控件看颜色是否全部变为浅绿
        'For Each c As Control In Me.tabPalChk.Controls
        '    If c.GetType Is GetType(Label) Then
        '        Dim lbl As Label = DirectCast(c, Label)
        '        If Not lbl.BackColor = Color.LightGreen Then
        '            Me.flgToolsVerify = Flag.OFF
        '            Exit Sub

        '        End If
        '    End If
        'Next
        'Me.flgToolsVerify = Flag.ON

    End Sub

#End Region

#Region "子类调用接口"

    ''' <summary>
    ''' 检查详细页子窗口调用接口
    ''' </summary>
    ''' <param name="strClsCode"></param>
    ''' <param name="strChkResult"></param>
    ''' <param name="strClassifyType"></param>
    ''' <remarks></remarks>
    Public Sub callback(ByVal strClsCode As String, ByVal strChkResult As String, ByVal strClassifyType As String)
        '画面显示
        Me.Show()

        Me.strCursorTp = strClsCode
        '根据返回检查结果对相应的分类做相应处理
        If strClassifyType.Equals("CF") Then
            '寸法检查下的分类，检查结果赋值
            Me.dicCFInfo(strClsCode).CheckResult = strChkResult
            '外观寸法显示
            Me.getFacedeInchData()
        Else
            '治具下的分类
            Me.dicZJFInfo(strClassifyType)(strClsCode).CheckResult = strChkResult
            '寸法显示
            Me.ShowClassifyOfTools(strClassifyType)


            For Each key As String In dicToolsNo.Keys
                Me.changColor2(dicToolsNo(key))
            Next

        End If
        '打开COM
        Me.LOpen()

    End Sub

    ''' <summary>
    ''' 信息Form回调函数
    ''' </summary>
    ''' <param name="strChosed"></param>
    ''' <param name="hsData"></param>
    ''' <remarks></remarks>
    Public Sub MsgFormCallback(ByVal strChosed As String, ByVal hsData As Hashtable, Optional ByVal jianchayuan As String = "")
        '主窗口可用
        Me.Enabled = True
        Me.lbltitle1.Visible = False
        Me.lbltitle2.Visible = False
        Me.lbltitle3.Visible = False
        Me.flgReceiveCd = Flag.ON
        '根据返回用户选择做相应处理
        Select Case strChosed
            Case "CC"
                '选择继续检查
                Me.strReulstID = hsData("strResultId").ToString
                '数据取得
                If GetAllData("CC") = False Then
                    Exit Sub
                End If

                '治具按钮可用
                Me.btnToolchk.Enabled = True
                'Flg变更
                Me.flgCC = Flag.ON
                '数据临时保存
                If ResultSave("TEMP") = False Then
                    '保存失败,
                    Me.strState = StateType.STATEERROR
                    ' Me.lblCheckState.Text = String.Format(MsgConst.M00002D, "结果")
                    Me.ShowStateMsg(String.Format(MsgConst.M00002D, "结果"))
                    Exit Sub
                End If


                '外观寸法检查数据取得&页面显示
                getFacedeInchData()

            Case "DC"
                '选择默认

                Me.strLastReulstID = hsData("strLastReulstID").ToString
                Me.strLastReulst = hsData("strLastReulst").ToString
                '状态变更
                Me.btnFacedeInchchk.Enabled = False
                Me.btnToolchk.Enabled = False





                Me.strState = StateType.STATE5
                'MessageBox.Show(MsgConst.M00031I)
                ShowStateMsg(MsgConst.M00031I)



                '检查员code接受赋值
                Me.strChkUserCd = jianchayuan
                '日志记录
                Me.ShowStateMsg(String.Format(MsgConst.M00053I, Me.strChkUserCd))
                '数据保存
                SaveChkResult()


            Case "RC"
                '选择重新检查

                '数据取得
                If GetAllData("INIT") = False Then
                    Exit Sub
                End If
                '治具按钮可用
                Me.btnToolchk.Enabled = True
                '数据临时保存
                If ResultSave() = False Then
                    '保存失败,
                    Me.strState = StateType.STATEERROR
                    ' Me.lblCheckState.Text = String.Format(MsgConst.M00002D, "结果")
                    Me.ShowStateMsg(String.Format(MsgConst.M00002D, "结果"))
                    Exit Sub
                End If


                '外观寸法检查数据取得&页面显示
                getFacedeInchData()
            Case "NC"
                '选择不检查
                Init()
        End Select
    End Sub
#End Region

#Region "父类继承事件"


    ''' <summary>
    ''' 监听Tcp传输数据事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CheckMainForm_OnTcpReceiveData(ByVal sender As Object, ByVal e As EventArgsSD) Handles Me.OnTcpReceiveData

        Dim scanData As String = e.data.Replace(" ", "").Replace("-", "").Replace("　", "")
        'If scanData = "L0720MKAA" Then
        '    scanData = "MZBC417A"
        'End If

        If Me.flgReceiveCd = Flag.ON Then

            '如果是开始标签，改变当前状态
            If scanData.Equals("KS") Then
                Select Case Me.strState
                    Case StateType.STATEINIT
                        '开始接受商品code和作番
                        Me.strState = StateType.STATE1
                        'Me.lblCheckState.Text = MsgConst.M00029I
                        Me.ShowStateMsg(MsgConst.M00029I)
                        Exit Sub
                    Case StateType.STATE1, StateType.STATE2, StateType.STATEERROR
                        Init()
                        '开始接受商品code和作番
                        Me.strState = StateType.STATE1
                        'Me.lblCheckState.Text = MsgConst.M00029I
                        Me.ShowStateMsg(MsgConst.M00029I)
                        Exit Sub
                    Case Else

                End Select

            ElseIf scanData.StartsWith("JS") Then
                Select Case Me.strState
                    Case StateType.STATEINIT, StateType.STATE1, StateType.STATE2, StateType.STATEERROR
                        Me.Init()
                        Exit Sub
                    Case Else
                        '检查员code接受赋值
                        Me.strChkUserCd = scanData.Substring(2)
                        '检查员存在Check
                        If bc.GetCheckUser(strChkUserCd) = True Then
                            '存在的场合
                            '日志记录
                            Me.ShowStateMsg(String.Format(MsgConst.M00053I, Me.strChkUserCd))
                            '数据保存
                            SaveChkResult()
                        Else
                            '不存在的场合
                            Me.ShowStateMsg(String.Format(MsgConst.M00056I, Me.strChkUserCd))
                            MessageBox.Show(String.Format(MsgConst.M00056I, Me.strChkUserCd))
                            Me.strChkUserCd = String.Empty
                        End If
                        Exit Sub
                End Select

            End If

            '扫码信息解析
            codeAnalysis(scanData)
        End If
    End Sub

    ''' <summary>
    ''' 打开COM和Service
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LOpen()
        '打开TCP服务器
        Me.OpenServer()
        '打开本机COM口
        'Me.OpenSerialPort()
    End Sub

    ''' <summary>
    ''' 关闭COM和Service
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LClose()
        '关闭TCP服务器
        Me.CloseServer()
    End Sub

#End Region

#Region "软键盘调用"
    ''' <summary>
    ''' 软键盘打开关闭
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnKeyboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKeyboard.Click
        'ShowKeybord()
    End Sub
#End Region

    Private Sub cb1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cb1.CheckedChanged
        Try
            If Me.cb1.Checked Then
                bc.Updatet_check_resultQinapinFlg(Me.strReulstID, "1") '有欠品
            Else
                bc.Updatet_check_resultQinapinFlg(Me.strReulstID, "0") '没有欠品
            End If
        Catch ex As Exception
            MsgBox("欠品更新失败")
        End Try
    End Sub

    ''' <summary>
    ''' 手入力 检查结果
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ShouInputCheckResult.Show()
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbltitle2.Click

    End Sub

    Private Sub lblStaus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblStaus.Click

    End Sub
End Class
