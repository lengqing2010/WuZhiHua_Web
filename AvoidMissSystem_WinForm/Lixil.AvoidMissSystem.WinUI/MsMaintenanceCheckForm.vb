Imports Lixil.AvoidMissSystem.BizLogic
'Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices.Marshal
Imports System.IO
Imports System.Windows
Imports System.Data
Imports System.Transactions
Imports Lixil.AvoidMissSystem.Utilities.Consts
Imports Lixil.AvoidMissSystem.Utilities
Imports Lixil.AvoidMissSystem.WinUI.Common

Public Class MsMaintenanceCheckForm

#Region "全局变量，常量定义"

    Dim searchFlg As Boolean = False
    Dim updFlg As Boolean = False
    Dim bc As New MsMaintenceCheckBizLogic
    Dim singleFlg As Boolean = False
    Dim toolsNo As String = String.Empty
    Dim strMessage As String = String.Empty
    Dim comBc As New CommonBizLogic
    Dim beforeUpdDt As DataTable
    Dim afterUpdDt As DataTable
    Dim dicSaveRequirment As Dictionary(Of String, String)
    Dim dicExcelHeader As Dictionary(Of String, String)

    '旧值记录用变量
    Dim GOODSNAME As String = String.Empty
    Dim TOOLNO As String = String.Empty
    Dim TOOLDISP As String = String.Empty
    Dim CLASSIFY As String = String.Empty
    Dim CLASSIFYDISP As String = String.Empty
    Dim DEPARTMENT As String = String.Empty
    Dim GOODSKIND As String = String.Empty
    Dim CHKPOSITION As String = String.Empty
    Dim CHKITEM As String = String.Empty
    Dim BMTYPE As String = String.Empty
    Dim BMVALUE1 As String = String.Empty
    Dim BMVALUE2 As String = String.Empty
    Dim BMVALUE3 As String = String.Empty
    Dim CHKWAY As String = String.Empty
    Dim CHKTIMES As String = String.Empty
    Dim IMGID As String = String.Empty

#End Region

#Region "事件"

    ''' <summary>
    ''' Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MsMaintenanceCheckForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '种类初始化
        KindInit()
        EditKindInit()

        '类型初始化
        TypeInit()
        EditTypeInit()

        '部门初始化
        DepartmentInit()
        EditDepartmentInit()

        dtList.AutoGenerateColumns = False

        Me.txtSelGoodsCd.Focus()

        'Form宽度和名称设置
        'Me.Width = 1024
        Me.Width = 1220
        Me.Height = 800

        Me.Text = "检查项目表维护"
        Me.Location = New Point(0, 0)
        Me.StartPosition = FormStartPosition.WindowsDefaultLocation

        Me.Location = New Point(1, 1)
        Me.Height = My.Computer.Screen.Bounds.Height - 40
        Me.Width = My.Computer.Screen.Bounds.Width - 10
        Me.AutoScroll = True



        '导入EXCEL文件列头设定
        dicExcelHeader = New Dictionary(Of String, String)
        dicExcelHeader.Add(CHECKID_TEXT, "id")
        dicExcelHeader.Add(GOODSCD_TEXT, "商品cd")
        dicExcelHeader.Add(GOODSNAME_TEXT, "商品名称")
        dicExcelHeader.Add(DEPARTMENTNAME_TEXT, "部门名称")
        dicExcelHeader.Add(KINDNAME_TEXT, "种类名称")
        dicExcelHeader.Add(TYPENAME_TEXT, "分类名")
        dicExcelHeader.Add(CLASSIFY_TEXT, "分类表示顺")
        dicExcelHeader.Add(CLASSIFYDISP_TEXT, "类型名称")
        dicExcelHeader.Add(IMGID_TEXT, "图片ID")
        dicExcelHeader.Add(TOOLNO_TEXT, "治具ID")
        dicExcelHeader.Add(TOOLDISP_TEXT, "治具表示顺")
        dicExcelHeader.Add(GOODSKIND_TEXT, "商品种类")
        dicExcelHeader.Add(CHKPOSITION_TEXT, "检查位置")
        dicExcelHeader.Add(CHKITEM_TEXT, "检查项目")
        dicExcelHeader.Add(BMTYPE_TEXT, "基准类型")
        dicExcelHeader.Add(BMVALUE1_TEXT, "基准值1")
        dicExcelHeader.Add(BMVALUE2_TEXT, "基准值2")
        dicExcelHeader.Add(BMVALUE3_TEXT, "基准值3")
        dicExcelHeader.Add(CHKWAY_TEXT, "检查方式")
        dicExcelHeader.Add(CHKTIMES_TEXT, "检查次数")

    End Sub

    ''' <summary>
    ''' 查询按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        '查询条件保存
        dicSaveRequirment = GetSearchRequirement()

        '清空编辑部分内容
        ClearEditGroup()

        DoSearch(dicSaveRequirment)

    End Sub

    ''' <summary>
    ''' 清空按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        '清空检索部分内容
        ClearSelGroup()
        '清空编辑部分内容
        ClearEditGroup()

    End Sub

    ''' <summary>
    ''' 保存按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim errFlg As Boolean = False
        Dim updResult As Integer
        Dim checkResult As Boolean
        Dim updPara As Dictionary(Of String, String)
        Dim strUpdFPath As String = String.Empty
        Dim endTime As DateTime
        Try
            If updFlg = True Then
                '更新的时候(批量更新)
                '输入内容检查
                If InputCheck(True) = True Then

                    '变更项目的判定
                    updPara = ChangeCheck()
                    If updPara.Count > 0 Then
                        '画面项目有变更时
                        '更新前后数据保存用Datatable初始化
                        InitTable()
                        beforeUpdDt = DirectCast(dtList.DataSource, DataTable)

                        '分类表中情报存在check
                        checkResult = True
                        ''只要有一条情报在分类表中不存在，就不进行数据库更新
                        For Each row As DataGridViewRow In Me.dtList.Rows

                            If updPara.ContainsKey(CLASSIFY_TEXT) OrElse
                                updPara.ContainsKey(IMGID_TEXT) OrElse
                                updPara.ContainsKey(DEPARTMENT_TEXT) OrElse
                                updPara.ContainsKey(TOOLNO_TEXT) OrElse
                                updPara.ContainsKey(CLASSIFYDISP_TEXT) Then

                                checkResult = CheckMClassify(row.Cells("classify_id").Value.ToString)
                            End If
                            If checkResult = False Then
                                MessageBox.Show(String.Format(MsgConst.M00063I, row.Cells("goods_cd").Value.ToString), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit Sub
                            End If
                        Next
                        '用户名保存
                        updPara.Add(USER_TEXT, Me.Login.UserId)
                        '更新
                        Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction


                        For Each row As DataGridViewRow In Me.dtList.Rows
                            updResult = bc.UpdateMCheck(PerDbTraneAction, row.Cells("id").Value.ToString,
                                                        row.Cells("goods_id").Value.ToString,
                                                        row.Cells("classify_id").Value.ToString,
                                                        updPara)
                            If updResult <> 1 Then
                                errFlg = False
                            Else
                                errFlg = True
                            End If
                            If errFlg = False Then
                                PerDbTraneAction.CloseRollback()
                                Exit For
                            End If
                        Next
                        If errFlg = True Then
                            PerDbTraneAction.CloseCommit()
                        Else
                            PerDbTraneAction.CloseRollback()
                        End If

                        endTime = DateTime.Now

                        '结果提示
                        If errFlg = True Then

                            '更新成功后刷新
                            DoSearch(dicSaveRequirment)

                            afterUpdDt = DirectCast(dtList.DataSource, DataTable)
                            '错误数据及更新信息导出
                            strUpdFPath = LogExport.LogExport("基础MS", New DataTable, beforeUpdDt, afterUpdDt, endTime)
                            '日志表插入
                            comBc.InsertLog("基础MS", OperateKind.UPDATE, strUpdFPath, Me.Login.UserId, endTime)

                            MessageBox.Show(MsgConst.M00016I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                            '清空编辑部分内容
                            ClearEditGroup()
                        Else
                            MessageBox.Show(MsgConst.M00017I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Else
                        MessageBox.Show("没有数据变更", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Else
                '新规的时候(单条)
                singleFlg = True
                '输入内容检查
                If InputCheck(False) = True Then

                    '画面数据保存
                    Dim dicInsert As New Dictionary(Of String, String)

                    '商品CD
                    dicInsert.Add(GOODSCD_TEXT, Me.txtEditGoodsCd.Text.Trim)
                    '种类
                    dicInsert.Add(KINDCD_TEXT, Me.drpEditKindCd.SelectedValue.ToString)
                    '类型
                    dicInsert.Add(TYPECD_TEXT, Me.drpEditType.SelectedValue.ToString)
                    '商品名称
                    dicInsert.Add(GOODSNAME_TEXT, Me.txtGoodsName.Text.Trim)
                    '治具编号
                    'dicInsert.Add(TOOLNO_TEXT, Me.txtEditToolNo.Text.Trim)
                    dicInsert.Add(TOOLNO_TEXT, toolsNo)
                    toolsNo = String.Empty
                    '治具表示顺
                    dicInsert.Add(TOOLDISP_TEXT, Me.txtToolDispNo.Text.Trim)
                    '分类
                    dicInsert.Add(CLASSIFY_TEXT, Me.txtEditClassify.Text.Trim)
                    '分类表示顺
                    dicInsert.Add(CLASSIFYDISP_TEXT, Me.txtClassifyDispNo.Text.Trim)
                    '部门
                    dicInsert.Add(DEPARTMENTCD_TEXT, Me.drpEditDepartment.SelectedValue.ToString)
                    '类别
                    dicInsert.Add(GOODSKIND_TEXT, Me.txtKind.Text.Trim)
                    '检查位置
                    dicInsert.Add(CHKPOSITION_TEXT, Me.txtChkPosition.Text.Trim)
                    '检查项目
                    dicInsert.Add(CHKITEM_TEXT, Me.txtChkProject.Text.Trim)
                    '基准类型
                    dicInsert.Add(BMTYPE_TEXT, Me.txtBenchmarkType.Text.Trim)
                    '基准值1
                    dicInsert.Add(BMVALUE1_TEXT, Me.txtBenchmarkValue1.Text.Trim)
                    '基准值2
                    dicInsert.Add(BMVALUE2_TEXT, Me.txtBenchmarkValue2.Text.Trim)
                    '基准值3
                    'dicInsert.Add(BMVALUE3_TEXT, Me.txtBenchmarkValue3.Text.Trim)


                    'If Me.txtBenchmarkValue3.Text.Trim <> "" AndAlso Me.txtBenchmarkValue3.Text.Trim.Substring(0, 1) = "-" Then
                    '    dicInsert.Add(BMVALUE3_TEXT, "-" & Me.txtBenchmarkValue3.Text.Trim)
                    'Else
                    '    dicInsert.Add(BMVALUE3_TEXT, Me.txtBenchmarkValue3.Text.Trim)
                    'End If
                    dicInsert.Add(BMVALUE3_TEXT, SetBenchmarkValue3fu(Me.txtBenchmarkValue3.Text.Trim))

                    '检查方式
                    dicInsert.Add(CHKWAY_TEXT, Me.txtChkWay.Text.Trim)
                    '检查次数
                    dicInsert.Add(CHKTIMES_TEXT, Me.txtChktimes.Text.Trim)
                    '图片ID
                    dicInsert.Add(IMGID_TEXT, Me.txtImgId.Text.Trim)
                    '用户
                    dicInsert.Add(USER_TEXT, Me.Login.UserId)

                    '执行插入操作
                    If bc.InsertCheckMs(dicInsert, singleFlg) = 0 Then

                        endTime = DateTime.Now

                        '日志表插入
                        comBc.InsertLog("基础MS", OperateKind.INSERT, "", Me.Login.UserId, endTime)

                        MessageBox.Show(MsgConst.M00013I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Else
                        MessageBox.Show(String.Format(MsgConst.M00002D, "检查表"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 负数取得
    ''' </summary>
    ''' <param name="v"></param>
    ''' <returns></returns>
    Public Function SetBenchmarkValue3fu(ByVal v As String) As String
        If v.Trim = "" Then
            Return ""
        Else
            Dim value As Decimal = Math.Abs(CDec(v)) * -1
            Return value.ToString
        End If
    End Function

    ''' <summary>
    ''' 整数取得
    ''' </summary>
    ''' <param name="v"></param>
    ''' <returns></returns>
    Public Function SetBenchmarkValue3Zheng(ByVal v As String) As String
        If v.Trim = "" Then
            Return ""
        Else
            Dim value As Decimal = Math.Abs(CDec(v))
            Return value.ToString
        End If
    End Function

    ''' <summary>
    ''' 删除按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        '批量删除
        Dim errFlg As Boolean = False
        Dim updResult As Integer
        Try
            If MessageBox.Show(MsgConst.M00002C, "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Forms.DialogResult.Cancel Then
                Exit Sub
            End If

            '删除
            Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction

            For Each row As DataGridViewRow In Me.dtList.Rows
                updResult = bc.DeleteMCheck(PerDbTraneAction, row.Cells("id").Value.ToString,
                                            row.Cells("classify_id").Value.ToString,
                                            Me.Login.UserId)
                If updResult <> 1 Then
                    errFlg = False
                Else
                    errFlg = True
                End If
                If errFlg = False Then
                    'PerDbTraneAction.CloseRollback()
                    Exit For
                End If
            Next
            If errFlg = True Then
                PerDbTraneAction.CloseCommit()
            Else
                PerDbTraneAction.CloseRollback()
            End If

            '结果提示
            If errFlg = True Then

                '日志表插入
                comBc.InsertLog("基础MS", OperateKind.DELETE, "", Me.Login.UserId, DateTime.Now)

                MessageBox.Show(MsgConst.M00012I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                '删除成功后刷新
                btnSearch_Click(sender, e)
            Else
                MessageBox.Show(MsgConst.M00019I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' CSV 出力
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCsv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCsv.Click
        ExportCSV()
    End Sub

    ''' <summary>
    ''' 返回按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'NavigateToNextPage("msMain")
        Try
            NavigateToNextPage(Consts.PAGE.MS_MAIN)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 退出按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        NavigateToNextPage("msLogin")
    End Sub

    ''' <summary>
    ''' 批量导入导出实行按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExcute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcute.Click
        Try
            If Me.rdbImport.Checked = True Then
                '批量导入处理
                ImportExcel()
            Else
                '批量导出处理
                ExportExcel()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    ''' <summary>
    ''' 选择路径按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnChoosePath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChoosePath.Click
        Dim ofd As New OpenFileDialog    '选择文件框 对象
        '打开时指定默认路径
        ofd.InitialDirectory = "C:\Documents and Settings\Administrator.ICBCOA-6E96E6BE\桌面"
        Try
            '如果用户点击确定
            If ofd.ShowDialog() = DialogResult.OK Then
                '将用户选择的文件路径 显示 在文本框中
                txtFilePath.Text = ofd.FileName
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 一览行双击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub dtList_CellMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dtList.CellMouseDoubleClick
        Dim dtImg As New DataTable
        '商品CD
        Me.txtEditGoodsCd.Text = dtList.CurrentRow.Cells(2).Value.ToString
        Me.txtEditGoodsCd.Enabled = False

        '商品名称
        '初始值记录（判断变化用）
        GOODSNAME = dtList.CurrentRow.Cells(3).Value.ToString
        Me.txtGoodsName.Text = dtList.CurrentRow.Cells(3).Value.ToString
        Me.txtGoodsName.Tag = dtList.CurrentRow.Cells(1).Value.ToString

        '种类
        Me.drpEditKindCd.SelectedValue = dtList.CurrentRow.Cells(4).Value.ToString
        Me.drpEditKindCd.Enabled = False

        '治具编号
        '初始值记录（判断变化用）
        TOOLNO = dtList.CurrentRow.Cells(6).Value.ToString
        Me.txtEditToolNo.Text = dtList.CurrentRow.Cells(6).Value.ToString

        '治具表示顺
        '初始值记录（判断变化用）
        TOOLDISP = dtList.CurrentRow.Cells(7).Value.ToString
        Me.txtToolDispNo.Text = dtList.CurrentRow.Cells(7).Value.ToString

        '分类
        '初始值记录（判断变化用）
        CLASSIFY = dtList.CurrentRow.Cells(9).Value.ToString
        Me.txtEditClassify.Text = dtList.CurrentRow.Cells(9).Value.ToString
        Me.txtEditClassify.Tag = dtList.CurrentRow.Cells(8).Value.ToString

        '分类表示顺
        '初始值记录（判断变化用）
        CLASSIFYDISP = dtList.CurrentRow.Cells(10).Value.ToString
        Me.txtClassifyDispNo.Text = dtList.CurrentRow.Cells(10).Value.ToString

        '类型
        Me.drpEditType.SelectedValue = dtList.CurrentRow.Cells(11).Value.ToString
        Me.drpEditType.Enabled = False

        '部门
        '初始值记录（判断变化用）
        DEPARTMENT = dtList.CurrentRow.Cells(13).Value.ToString
        Me.drpEditDepartment.SelectedValue = dtList.CurrentRow.Cells(13).Value.ToString

        '类别
        '初始值记录（判断变化用）
        GOODSKIND = dtList.CurrentRow.Cells(15).Value.ToString
        Me.txtKind.Text = dtList.CurrentRow.Cells(15).Value.ToString

        '检查位置
        '初始值记录（判断变化用）
        CHKPOSITION = dtList.CurrentRow.Cells(16).Value.ToString
        Me.txtChkPosition.Text = dtList.CurrentRow.Cells(16).Value.ToString

        '检查项目
        '初始值记录（判断变化用）
        CHKITEM = dtList.CurrentRow.Cells(17).Value.ToString
        Me.txtChkProject.Text = dtList.CurrentRow.Cells(17).Value.ToString

        '基准类型
        '初始值记录（判断变化用）
        BMTYPE = dtList.CurrentRow.Cells(18).Value.ToString
        Me.txtBenchmarkType.Text = dtList.CurrentRow.Cells(18).Value.ToString

        '基准值1
        '初始值记录（判断变化用）
        BMVALUE1 = dtList.CurrentRow.Cells(19).Value.ToString
        Me.txtBenchmarkValue1.Text = dtList.CurrentRow.Cells(19).Value.ToString

        '基准值2
        '初始值记录（判断变化用）
        BMVALUE2 = dtList.CurrentRow.Cells(20).Value.ToString
        Me.txtBenchmarkValue2.Text = dtList.CurrentRow.Cells(20).Value.ToString

        '基准值3
        '初始值记录（判断变化用）
        BMVALUE3 = dtList.CurrentRow.Cells(21).Value.ToString
        Me.txtBenchmarkValue3.Text = dtList.CurrentRow.Cells(21).Value.ToString

        '检查方式
        '初始值记录（判断变化用）
        CHKWAY = dtList.CurrentRow.Cells(22).Value.ToString
        Me.txtChkWay.Text = dtList.CurrentRow.Cells(22).Value.ToString

        '检查次数
        '初始值记录（判断变化用）
        CHKTIMES = dtList.CurrentRow.Cells(23).Value.ToString
        Me.txtChktimes.Text = dtList.CurrentRow.Cells(23).Value.ToString

        '图片ID
        '初始值记录（判断变化用）
        IMGID = dtList.CurrentRow.Cells(24).Value.ToString
        Me.txtImgId.Text = dtList.CurrentRow.Cells(24).Value.ToString

        '图片表示
        dtImg = bc.GetPictureContent(IMGID)
        If dtImg.Rows.Count > 0 Then
            Try
                Me.pbImg.Image = GetImageFromByteArray(DirectCast(dtImg.Rows(0).Item("picture_content"), Byte()))
            Catch ex As Exception

            End Try

        End If

        '删除按钮可用
        Me.btnDelete.Enabled = True

        '清空按钮不可用
        Me.btnClear.Enabled = False

        '更新Flg设为true
        updFlg = True

    End Sub

    ''' <summary>
    ''' 一览行点击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>2015/06/18 追加</remarks>
    ''' 
    Private Sub dtList_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dtList.CellMouseClick
        Dim dtImg As New DataTable
        '旧值记录用变量清空
        GOODSNAME = String.Empty
        TOOLNO = String.Empty
        TOOLDISP = String.Empty
        CLASSIFY = String.Empty
        CLASSIFYDISP = String.Empty
        DEPARTMENT = String.Empty
        GOODSKIND = String.Empty
        CHKPOSITION = String.Empty
        CHKITEM = String.Empty
        BMTYPE = String.Empty
        BMVALUE1 = String.Empty
        BMVALUE2 = String.Empty
        BMVALUE3 = String.Empty
        CHKWAY = String.Empty
        IMGID = String.Empty

        '商品CD
        Me.txtEditGoodsCd.Text = String.Empty
        Me.txtEditGoodsCd.Enabled = True

        '商品名称
        Me.txtGoodsName.Text = String.Empty
        Me.txtGoodsName.Tag = String.Empty

        '种类
        Me.drpEditKindCd.SelectedIndex = 0
        Me.drpEditKindCd.Enabled = True

        '治具编号
        Me.txtEditToolNo.Text = String.Empty

        '治具表示顺
        Me.txtToolDispNo.Text = String.Empty

        '分类
        Me.txtEditClassify.Text = String.Empty
        Me.txtEditClassify.Tag = String.Empty

        '分类表示顺
        Me.txtClassifyDispNo.Text = String.Empty

        '类型
        Me.drpEditType.SelectedIndex = 0
        Me.drpEditType.Enabled = True

        '部门
        Me.drpEditDepartment.SelectedIndex = 0

        '类别
        Me.txtKind.Text = String.Empty

        '检查位置
        Me.txtChkPosition.Text = String.Empty

        '检查项目
        Me.txtChkProject.Text = String.Empty

        '基准类型
        Me.txtBenchmarkType.Text = String.Empty

        '基准值1
        Me.txtBenchmarkValue1.Text = String.Empty

        '基准值2
        Me.txtBenchmarkValue2.Text = String.Empty

        '基准值3
        Me.txtBenchmarkValue3.Text = String.Empty

        '检查方式
        Me.txtChkWay.Text = String.Empty

        '检查次数
        Me.txtChktimes.Text = String.Empty

        '图片ID
        Me.txtImgId.Text = String.Empty

        '图片表示
        Me.pbImg.Image = Nothing

        '删除按钮可用
        Me.btnDelete.Enabled = False

        '清空按钮不可用
        Me.btnClear.Enabled = True

        '更新Flg设为true
        updFlg = False

        '编辑区背景色恢复初期值
        txtEditToolNo.BackColor = Color.White
        txtGoodsName.BackColor = Color.White
        txtToolDispNo.BackColor = Color.White
        txtEditClassify.BackColor = Color.White
        txtClassifyDispNo.BackColor = Color.White
        drpEditDepartment.BackColor = Color.White
        txtKind.BackColor = Color.White
        txtChkPosition.BackColor = Color.White
        txtChkProject.BackColor = Color.White
        txtBenchmarkType.BackColor = Color.White
        txtBenchmarkValue1.BackColor = Color.White
        txtBenchmarkValue2.BackColor = Color.White
        txtBenchmarkValue3.BackColor = Color.White
        txtChkWay.BackColor = Color.White
        txtChktimes.BackColor = Color.White
        txtImgId.BackColor = Color.White

    End Sub

    ''' <summary>
    ''' 图片双击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub pbImg_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbImg.DoubleClick
        ShowMaxPicture(Me.pbImg.Image)
    End Sub

    ''' <summary>
    ''' 检查项目批量导入模板链接点击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lblTemplateDown_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblTemplateDown.LinkClicked
        Try
            Dim fileName As String = String.Empty
            Dim appPath As String = Forms.Application.StartupPath
            Dim pathTemplate As String = GetConfig.GetConfigVal("ExcelFilePatch")
            Dim saveFileDialog As New SaveFileDialog()

            pathTemplate = appPath & "/Template/CheckMSTemplate.xls"
            saveFileDialog.Filter = "下载模板(*.xls)|*.xls"
            saveFileDialog.FileName = "检查项目MS模版"
            If saveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If File.Exists(pathTemplate) Then
                    fileName = saveFileDialog.FileName
                    File.Copy(pathTemplate, fileName, True)
                    MessageBox.Show(MsgConst.M00011I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(String.Format(MsgConst.M00010I, "模板"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

#End Region

#Region "方法"

    ''' <summary>
    ''' 种类初始化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub KindInit()
        Dim dtKind As DataTable

        dtKind = bc.GetKbn("0001")

        Me.chkLKind.DataSource = dtKind
        Me.chkLKind.DisplayMember = "mei"
        Me.chkLKind.ValueMember = "mei_cd"

    End Sub

    ''' <summary>
    ''' 类型初始化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TypeInit()
        Dim dtType As DataTable

        dtType = bc.GetKbn("0002")

        Me.chkLType.DataSource = dtType
        Me.chkLType.DisplayMember = "mei"
        Me.chkLType.ValueMember = "mei_cd"

    End Sub

    ''' <summary>
    ''' 部门初始化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DepartmentInit()
        Dim dtDepartment As DataTable

        '权限判定
        If Me.Login.IsAdmin = True Then
            dtDepartment = comBc.GetAdminDepartment().Tables(0)
        Else
            dtDepartment = comBc.GetDepartment(Me.Login.UserId).Tables("Department")
        End If

        '数据绑定
        Me.chkLDepartment.DataSource = dtDepartment
        Me.chkLDepartment.DisplayMember = "mei"
        Me.chkLDepartment.ValueMember = "mei_cd"

    End Sub

    ''' <summary>
    ''' 种类初始化(编辑用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EditKindInit()
        Dim dtKind As DataTable

        dtKind = bc.GetKbn("0001")

        Me.drpEditKindCd.DataSource = dtKind
        Me.drpEditKindCd.DisplayMember = "mei"
        Me.drpEditKindCd.ValueMember = "mei_cd"

    End Sub

    ''' <summary>
    ''' 类型初始化(编辑用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EditTypeInit()
        Dim dtType As DataTable

        dtType = bc.GetKbn("0002")

        Me.drpEditType.DataSource = dtType
        Me.drpEditType.DisplayMember = "mei"
        Me.drpEditType.ValueMember = "mei_cd"
    End Sub

    ''' <summary>
    ''' 部门初始化(编辑用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EditDepartmentInit()
        Dim dtDepartment As DataTable

        '权限判定
        If Me.Login.IsAdmin = True Then
            dtDepartment = comBc.GetAdminDepartment().Tables(0)
        Else
            dtDepartment = comBc.GetDepartment(Me.Login.UserId).Tables("Department")
        End If

        '数据绑定
        Me.drpEditDepartment.DataSource = dtDepartment
        Me.drpEditDepartment.DisplayMember = "mei"
        Me.drpEditDepartment.ValueMember = "mei_cd"
    End Sub

    ''' <summary>
    ''' 检查项目数据取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCheckData(ByVal dicRequirement As Dictionary(Of String, String), ByVal pageIdx As Integer) As DataTable
        Dim dtCheck As DataTable

        'dtCheck = bc.GetCheckData(Me.txtSelGoodsCd.Text, _
        '                         Me.drpSelKind.SelectedValue.ToString, _
        '                         Me.txtSelToolNo.Text, _
        '                         Me.txtSelClassify.Text, _
        '                         Me.drpSelType.SelectedValue.ToString, _
        '                         Me.txtSelImgId.Text.ToString)
        dtCheck = bc.GetCheckData(dicRequirement, pageIdx)

        Return dtCheck
    End Function

    ''' <summary>
    ''' 检索条件保存
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSearchRequirement() As Dictionary(Of String, String)
        Dim dicRequirement As New Dictionary(Of String, String)
        Dim strKindCd As String = String.Empty
        Dim strType As String = String.Empty
        Dim strDepartment As String = String.Empty
        Dim strAllDepartment As String = String.Empty
        Dim dtKind As DataTable = DirectCast(chkLKind.DataSource, DataTable)
        Dim dtType As DataTable = DirectCast(chkLType.DataSource, DataTable)
        Dim dtDepartment As DataTable = DirectCast(chkLDepartment.DataSource, DataTable)

        'dtCheck.Clear()
        dicRequirement.Add("goodsCd", Me.txtSelGoodsCd.Text)
        dicRequirement.Add("goodsName", Me.txtSelGoodsName.Text)

        '选择的种类取得
        For i As Integer = 0 To Me.chkLKind.Items.Count - 1
            If chkLKind.GetItemChecked(i) = True Then
                'strKindCd = strKindCd & "'" & chkLKind.GetItemText(chkLKind.Items(i)) & "',"
                strKindCd = strKindCd & "'" & dtKind.Rows(i).Item("mei_cd").ToString() & "',"
            End If
        Next
        strKindCd = strKindCd.TrimEnd(CChar(","))
        dicRequirement.Add("kindCd", strKindCd)

        dicRequirement.Add("toolNo", Me.txtSelToolNo.Text)
        dicRequirement.Add("classify", Me.txtSelClassify.Text)

        '选择的类型取得
        For i As Integer = 0 To Me.chkLType.Items.Count - 1
            If chkLType.GetItemChecked(i) = True Then
                'strKindCd = strKindCd & "'" & chkLKind.GetItemText(chkLKind.Items(i)) & "',"
                strType = strType & "'" & dtType.Rows(i).Item("mei_cd").ToString() & "',"
            End If
        Next
        strType = strType.TrimEnd(CChar(","))
        dicRequirement.Add("type", strType)
        '选择的部门取得
        For i As Integer = 0 To Me.chkLDepartment.Items.Count - 1
            If chkLDepartment.GetItemChecked(i) = True Then
                'strKindCd = strKindCd & "'" & chkLKind.GetItemText(chkLKind.Items(i)) & "',"
                strDepartment = strDepartment & "'" & dtDepartment.Rows(i).Item("mei_cd").ToString() & "',"
            End If
            strAllDepartment = strAllDepartment & "'" & dtDepartment.Rows(i).Item("mei_cd").ToString() & "',"
        Next
        strDepartment = strDepartment.TrimEnd(CChar(","))
        strAllDepartment = strAllDepartment.TrimEnd(CChar(","))
        If strDepartment = String.Empty Then
            dicRequirement.Add("department", strAllDepartment)
        Else
            dicRequirement.Add("department", strDepartment)
        End If

        dicRequirement.Add("kind", Me.txtSelKind.Text)
        dicRequirement.Add("chkPosition", Me.txtSelChkPosition.Text)
        dicRequirement.Add("chkItem", Me.txtSelChkProject.Text)
        dicRequirement.Add("BMType", Me.txtSelBenchmarkType.Text)
        dicRequirement.Add("BMValue1", Me.txtSelBMValue1.Text)
        dicRequirement.Add("BMValue2", Me.txtSelBMValue2.Text)
        dicRequirement.Add("BMValue3", Me.txtSelBMValue3.Text)
        dicRequirement.Add("chkWay", Me.txtSelChkWay.Text)
        dicRequirement.Add("imgId", Me.txtSelImgId.Text.ToString)

        Return dicRequirement
    End Function

    ''' <summary>
    ''' 执行检索
    ''' </summary>
    ''' <param name="dicRequirement"></param>
    ''' <remarks></remarks>
    Private Sub DoSearch(ByVal dicRequirement As Dictionary(Of String, String))
        Dim dtListData As DataTable
        Dim tmpImgId As String = String.Empty
        Dim showImgFlg As Boolean = False
        Dim dtImg As New DataTable

        '数据取得 -1 全件
        dtListData = GetCheckData(dicRequirement, -1)

        Me.dtList.DataSource = dtListData

        searchFlg = True
        '图片设置
        If dtListData.Rows.Count = 1 Then
            '图片表示
            'Me.pbImg.Image = Image.FromFile("C:\Users\Public\Pictures\Sample Pictures\01.jpg")
            '图片取得
            dtImg = bc.GetPictureContent(dtListData.Rows(0).Item("picture_id").ToString)
            If dtImg.Rows.Count > 0 Then
                Me.pbImg.Image = GetImageFromByteArray(DirectCast(dtImg.Rows(0).Item("picture_content"), Byte()))
            Else
                Me.pbImg.Image = Nothing
            End If

        ElseIf dtListData.Rows.Count > 1 Then
            For i As Integer = 0 To dtListData.Rows.Count - 1
                If i = 0 Then
                    tmpImgId = dtListData.Rows(i).Item("picture_id").ToString
                Else
                    If dtListData.Rows(i).Item("picture_id").ToString = tmpImgId Then
                        showImgFlg = True
                    Else
                        showImgFlg = False
                        Exit For
                    End If
                End If
            Next
            If showImgFlg = True Then
                '图片表示
                'Me.pbImg.Image = Image.FromFile("C:\Users\Public\Pictures\Sample Pictures\01.jpg")
                '图片取得
                dtImg = bc.GetPictureContent(dtListData.Rows(0).Item("picture_id").ToString)
                If dtImg.Rows.Count > 0 Then
                    Try
                        Me.pbImg.Image = GetImageFromByteArray(DirectCast(dtImg.Rows(0).Item("picture_content"), Byte()))
                    Catch ex As Exception

                    End Try
                End If
            Else
                Me.pbImg.Image = Nothing
            End If
            Me.lblRowCount.Text = dtListData.Rows.Count.ToString & " 行"
        Else
            Me.pbImg.Image = Nothing
        End If

    End Sub

    '''' <summary>
    '''' 双击显示大图
    '''' </summary>
    '''' <param name="picture"></param>
    '''' <remarks></remarks>
    'Private Sub ShowMaxPicture(ByVal picture As Image)
    '    Dim frmMaxImage As Form
    '    Dim maxPictureBox As PictureBox
    '    Dim frmName As String = "frmMaxPicture"

    '    '如果大图窗口已经打开，那么先关掉
    '    Try
    '        For Each myForm As Form In System.Windows.Forms.Application.OpenForms

    '            If myForm.Name = frmName Then
    '                myForm.Close()
    '            End If
    '        Next
    '    Catch ex As Exception
    '    End Try

    '    frmMaxImage = New Form '新建立一个form窗口
    '    maxPictureBox = New PictureBox '新建立一个图 片显 示控件
    '    frmMaxImage.Name = frmName
    '    With picture
    '        frmMaxImage.Width = .Width
    '        frmMaxImage.Height = .Height
    '        maxPictureBox.Width = .Width
    '        maxPictureBox.Height = .Height
    '    End With
    '    maxPictureBox.Image = picture '设置图 片显 示控件的图 片，来自形参
    '    frmMaxImage.Controls.Add(maxPictureBox) '将图 片控件加入到form窗口中
    '    frmMaxImage.Show() '显示form窗口
    'End Sub

    ''' <summary>
    ''' 清空检索部分内容
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearSelGroup()
        '商品CD
        Me.txtSelGoodsCd.Text = String.Empty
        '商品名称
        Me.txtSelGoodsName.Text = String.Empty
        '种类
        'Me.drpSelKind.SelectedValue = String.Empty
        For i As Integer = 0 To Me.chkLKind.Items.Count - 1
            chkLKind.SetItemCheckState(i, CheckState.Unchecked)
        Next
        '类型
        'Me.drpSelType.SelectedValue = String.Empty
        For i As Integer = 0 To Me.chkLType.Items.Count - 1
            chkLType.SetItemCheckState(i, CheckState.Unchecked)
        Next
        '部门
        For i As Integer = 0 To Me.chkLDepartment.Items.Count - 1
            chkLDepartment.SetItemCheckState(i, CheckState.Unchecked)
        Next
        '治具编号
        Me.txtSelToolNo.Text = String.Empty
        '分类
        Me.txtSelClassify.Text = String.Empty
        '类别
        Me.txtSelKind.Text = String.Empty
        '检查位置
        Me.txtSelChkPosition.Text = String.Empty
        '检查项目
        Me.txtSelChkProject.Text = String.Empty
        '基准类型
        Me.txtSelBenchmarkType.Text = String.Empty
        '基准值1
        Me.txtSelBMValue1.Text = String.Empty
        '基准值2
        Me.txtSelBMValue2.Text = String.Empty
        '基准值3
        Me.txtSelBMValue3.Text = String.Empty
        '检查方式
        Me.txtSelChkWay.Text = String.Empty
        '图片
        Me.txtSelImgId.Text = String.Empty
    End Sub

    ''' <summary>
    ''' 清空编辑部分内容
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearEditGroup()
        '更新Flg设为false
        updFlg = False

        '旧值记录用变量清空
        GOODSNAME = String.Empty
        TOOLNO = String.Empty
        TOOLDISP = String.Empty
        CLASSIFY = String.Empty
        CLASSIFYDISP = String.Empty
        DEPARTMENT = String.Empty
        GOODSKIND = String.Empty
        CHKPOSITION = String.Empty
        CHKITEM = String.Empty
        BMTYPE = String.Empty
        BMVALUE1 = String.Empty
        BMVALUE2 = String.Empty
        BMVALUE3 = String.Empty
        CHKWAY = String.Empty
        IMGID = String.Empty
        Me.lblRowCount.Text = String.Empty

        '商品CD
        Me.txtEditGoodsCd.Text = String.Empty
        Me.txtEditGoodsCd.Enabled = True
        '商品名称
        Me.txtGoodsName.Text = String.Empty
        Me.txtGoodsName.Tag = String.Empty
        '种类
        Me.drpEditKindCd.SelectedIndex = 0
        Me.drpEditKindCd.Enabled = True
        '治具编号
        Me.txtEditToolNo.Text = String.Empty
        '治具表示顺
        Me.txtToolDispNo.Text = String.Empty
        '分类
        Me.txtEditClassify.Text = String.Empty
        Me.txtEditClassify.Tag = String.Empty
        '分类表示顺
        Me.txtClassifyDispNo.Text = String.Empty
        '类型
        Me.drpEditType.SelectedIndex = 0
        Me.drpEditType.Enabled = True
        '部门
        Me.drpEditDepartment.SelectedIndex = 0
        '类别
        Me.txtKind.Text = String.Empty
        '检查位置
        Me.txtChkPosition.Text = String.Empty
        '检查项目
        Me.txtChkProject.Text = String.Empty
        '基准类型
        Me.txtBenchmarkType.Text = String.Empty
        '基准值1
        Me.txtBenchmarkValue1.Text = String.Empty
        '基准值2
        Me.txtBenchmarkValue2.Text = String.Empty
        '基准值3
        Me.txtBenchmarkValue3.Text = String.Empty
        '检查方式
        Me.txtChkWay.Text = String.Empty
        '检查次数
        Me.txtChktimes.Text = String.Empty
        '图片ID
        Me.txtImgId.Text = String.Empty
        '图片清空
        Me.pbImg.Image = Nothing
        '删除按钮不可用
        Me.btnDelete.Enabled = False
        '清空按钮可用
        Me.btnClear.Enabled = True

        If searchFlg = True Then
            '一览内容清空
            Dim dtEmpty As DataTable
            dtEmpty = DirectCast(dtList.DataSource, DataTable)
            dtEmpty.Rows.Clear()
            Me.dtList.DataSource = dtEmpty
        End If

        '编辑区背景色恢复初期值
        txtEditToolNo.BackColor = Color.White
        txtGoodsName.BackColor = Color.White
        txtToolDispNo.BackColor = Color.White
        txtEditClassify.BackColor = Color.White
        txtClassifyDispNo.BackColor = Color.White
        drpEditDepartment.BackColor = Color.White
        txtKind.BackColor = Color.White
        txtChkPosition.BackColor = Color.White
        txtChkProject.BackColor = Color.White
        txtBenchmarkType.BackColor = Color.White
        txtBenchmarkValue1.BackColor = Color.White
        txtBenchmarkValue2.BackColor = Color.White
        txtBenchmarkValue3.BackColor = Color.White
        txtChkWay.BackColor = Color.White
        txtChktimes.BackColor = Color.White
        txtImgId.BackColor = Color.White
    End Sub

    ''' <summary>
    ''' EXCEL文件导入处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ImportExcel()
        Dim strFilePath As String
        Dim strFileType As String
        Dim dicInsert As New Dictionary(Of String, String)
        Dim dtExcel As DataTable
        'Dim dtImport As DataTable
        'Dim drImport As DataRow
        Dim dtError As DataTable
        Dim drError As DataRow
        Dim successCnt As Integer = 0
        Dim failCnt As Integer = 0
        Dim frmErrorData As ErrorDataForm
        Dim strExportFPath As String = String.Empty
        Dim strDicHeader As String = String.Empty
        Dim strExcelHeader As String = String.Empty
        Dim xlBook = Nothing
        Dim strSheetNames As String()

        Dim startTime As DateTime
        Dim endTime As DateTime
        Dim spTime As TimeSpan
        Dim delRow As Integer = 0

        Try
            strFilePath = Me.txtFilePath.Text.Trim
            If String.IsNullOrEmpty(strFilePath) Then
                MessageBox.Show("请选择您要导入的EXCEL文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            Else
                strFileType = System.IO.Path.GetExtension(strFilePath).ToLower
                If strFileType <> ".xls" AndAlso strFileType <> ".xlsx" Then
                    MessageBox.Show("您选择的不是EXCEL文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
            End If

            If File.Exists(strFilePath) Then
                '读取EXCEL数据

                '[检查项目信息]sheet存在检查
                strSheetNames = LogExport.GetAllSheetName(strFilePath)
                For i As Integer = 0 To strSheetNames.Length - 1
                    If strSheetNames(i) = "检查项目MS$" Then
                        Exit For
                    End If
                    If i = strSheetNames.Length - 1 Then
                        MessageBox.Show("EXCEL文件中不存在[检查项目MS]Sheet页！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                Next

                '开始时间
                startTime = DateTime.Now
                Me.Cursor = Cursors.WaitCursor

                dtExcel = GetExcelData("检查项目MS", strFilePath, strFileType)

                If dtExcel.Rows.Count < 2 Then
                    MessageBox.Show("EXCEL文件中要导入的正确数据不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If



                If dtExcel.Rows.Count < 2 Then
                    MessageBox.Show("EXCEL文件中要导入的对象数据不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If

                '关联用的列和行删除

                If dtExcel.Columns.Contains("图片名称") Then
                    dtExcel.Columns.Remove("图片名称")
                End If
                If dtExcel.Columns.Contains("治具编号") Then
                    dtExcel.Columns.Remove("治具编号")
                End If
                If dtExcel.Columns.Contains("图片描述") Then
                    dtExcel.Columns.Remove("图片描述")
                End If

                dtExcel.Rows.Remove(dtExcel.Rows(0))

                'dtImport = dtExcel.Clone()
                dtError = dtExcel.Clone()
                dtError.Columns.Add("错误信息")

                'EXCEL数据项目校验
                If dtExcel.Columns.Count <> dicExcelHeader.Count Then
                    MessageBox.Show(MsgConst.M00009I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

                For Each strHeader As String In dicExcelHeader.Values
                    strDicHeader = strDicHeader & strHeader
                Next

                For Each dcExcel As DataColumn In dtExcel.Columns
                    strExcelHeader = strExcelHeader & dcExcel.ColumnName
                Next

                If Not strDicHeader.Equals(strExcelHeader) Then
                    MessageBox.Show(MsgConst.M00009I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

                '更新前后数据保存用Datatable初始化
                InitTable()

                '单行数据保存用
                dicInsert.Add(CHECKID_TEXT, String.Empty)
                dicInsert.Add(GOODSCD_TEXT, String.Empty)
                dicInsert.Add(GOODSNAME_TEXT, String.Empty)
                dicInsert.Add(DEPARTMENTNAME_TEXT, String.Empty)
                dicInsert.Add(KINDNAME_TEXT, String.Empty)
                dicInsert.Add(TYPENAME_TEXT, String.Empty)
                'dicInsert.Add(KINDCD_TEXT, String.Empty)
                'dicInsert.Add(DEPARTMENT_TEXT, String.Empty)
                dicInsert.Add(CLASSIFY_TEXT, String.Empty)
                dicInsert.Add(CLASSIFYDISP_TEXT, String.Empty)
                dicInsert.Add(IMGID_TEXT, String.Empty)
                dicInsert.Add(TOOLNO_TEXT, String.Empty)
                dicInsert.Add(TOOLDISP_TEXT, String.Empty)
                dicInsert.Add(GOODSKIND_TEXT, String.Empty)
                dicInsert.Add(CHKPOSITION_TEXT, String.Empty)
                dicInsert.Add(CHKITEM_TEXT, String.Empty)
                dicInsert.Add(BMTYPE_TEXT, String.Empty)
                dicInsert.Add(BMVALUE1_TEXT, String.Empty)
                dicInsert.Add(BMVALUE2_TEXT, String.Empty)
                dicInsert.Add(BMVALUE3_TEXT, String.Empty)
                dicInsert.Add(CHKWAY_TEXT, String.Empty)
                dicInsert.Add(CHKTIMES_TEXT, String.Empty)
                dicInsert.Add(USER_TEXT, String.Empty)

                For Each drExcel As DataRow In dtExcel.Rows
                    If ImportCheck(drExcel) Then
                        dicInsert.Clear()
                        dicInsert(CHECKID_TEXT) = drExcel("id").ToString.Trim
                        dicInsert(GOODSCD_TEXT) = drExcel("商品cd").ToString.Trim.Replace(" ", "").Replace("-", "")
                        dicInsert(GOODSNAME_TEXT) = drExcel("商品名称").ToString.Trim
                        dicInsert(DEPARTMENTNAME_TEXT) = drExcel("部门名称").ToString.Trim
                        dicInsert(KINDNAME_TEXT) = drExcel("种类名称").ToString.Trim
                        dicInsert(TYPENAME_TEXT) = drExcel("类型名称").ToString.Trim
                        dicInsert(CLASSIFY_TEXT) = drExcel("分类名").ToString.Trim
                        dicInsert(CLASSIFYDISP_TEXT) = drExcel("分类表示顺").ToString.Trim
                        dicInsert(IMGID_TEXT) = drExcel("图片ID").ToString.Trim
                        'dicInsert(TOOLNO_TEXT) = drExcel("治具编号").ToString.Trim
                        dicInsert(TOOLNO_TEXT) = toolsNo
                        toolsNo = String.Empty
                        dicInsert(TOOLDISP_TEXT) = drExcel("治具表示顺").ToString.Trim
                        dicInsert(GOODSKIND_TEXT) = drExcel("商品种类").ToString.Trim
                        dicInsert(CHKPOSITION_TEXT) = drExcel("检查位置").ToString.Trim
                        dicInsert(CHKITEM_TEXT) = drExcel("检查项目").ToString.Trim
                        dicInsert(BMTYPE_TEXT) = drExcel("基准类型").ToString.Trim
                        dicInsert(BMVALUE1_TEXT) = drExcel("基准值1").ToString.Trim
                        dicInsert(BMVALUE2_TEXT) = drExcel("基准值2").ToString.Trim

                        'If drExcel("基准值3").ToString.Trim <> "" AndAlso drExcel("基准值3").ToString.Trim.Substring(0, 1) = "-" Then
                        '    dicInsert(BMVALUE3_TEXT) = "-" & drExcel("基准值3").ToString.Trim
                        'Else
                        '    dicInsert(BMVALUE3_TEXT) = drExcel("基准值3").ToString.Trim
                        'End If

                        dicInsert.Add(BMVALUE3_TEXT, SetBenchmarkValue3fu(drExcel("基准值3").ToString.Trim))


                        dicInsert(CHKWAY_TEXT) = drExcel("检查方式").ToString.Trim
                        dicInsert(CHKTIMES_TEXT) = drExcel("检查次数").ToString.Trim
                        dicInsert(USER_TEXT) = Me.Login.UserId

                        If bc.InsertCheckMs(dicInsert, False) <> 0 Then
                            drError = dtError.NewRow()
                            drError.ItemArray = drExcel.ItemArray
                            drError("错误信息") = "该条数据导入失败"
                            dtError.Rows.Add(drError)
                            'dtError.ImportRow(drExcel)
                            failCnt = failCnt + 1
                        Else
                            If dicInsert(CHECKID_TEXT) <> String.Empty Then
                                '更新的时候，更新成功的数据保存到更新后的Datatable
                                afterUpdDt.Rows.Add(drExcel.ItemArray)
                            End If
                            successCnt = successCnt + 1
                        End If

                    Else
                        drError = dtError.NewRow()
                        drError.ItemArray = drExcel.ItemArray
                        drError("错误信息") = strMessage
                        dtError.Rows.Add(drError)
                        'dtError.ImportRow(drExcel)
                        failCnt = failCnt + 1
                        'Console.WriteLine(strMessage & ":" & drExcel.ItemArray.ToString)
                    End If
                Next

                '结束时间
                endTime = DateTime.Now
                spTime = endTime - startTime

                '错误数据及更新信息导出
                strExportFPath = LogExport.LogExport("基础MS", dtError, beforeUpdDt, afterUpdDt, endTime)
                '日志表插入
                comBc.InsertLog("基础MS", OperateKind.IMPORT, strExportFPath, Me.Login.UserId, endTime)

                'Excel数据导入结果
                If successCnt > 0 Then
                    MessageBox.Show(String.Format(MsgConst.M00015I, dtExcel.Rows.Count.ToString, successCnt, failCnt),
                                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    'MessageBox.Show("批量导入数据" & dtExcel.Rows.Count.ToString & "件!耗时" & spTime.TotalSeconds.ToString & "秒", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else
                    MessageBox.Show(MsgConst.M00014I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If

                '失败数据表示
                If dtError.Rows.Count > 0 AndAlso MessageBox.Show(MsgConst.M00003C, "確認",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Forms.DialogResult.OK Then
                    'WriteErrorData(dtError)
                    frmErrorData = New ErrorDataForm()
                    frmErrorData.ErrorData = dtError
                    frmErrorData.Show()
                End If

                '种类初始化
                KindInit()
                EditKindInit()

                '类型初始化
                TypeInit()
                EditTypeInit()

            Else
                MessageBox.Show(String.Format(MsgConst.M00010I, "批量导入"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            Me.Cursor = Cursors.Arrow
        Catch ex As Exception
            Me.Cursor = Cursors.Arrow
            Throw ex
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub

    ''' <summary>
    ''' EXCEL文件导出处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExportExcel()
        Dim rowCount As Integer
        Dim colCount As Integer
        Dim strFileName As String
        Dim saveFileDialog As SaveFileDialog
        Dim xlBook = Nothing
        Dim xlSheet = Nothing
        Dim xlApp = Nothing
        Dim arrData(0, 0) As String
        Dim dtListData As New DataTable

        Dim lstArrData As New List(Of String(,))

        Try
            saveFileDialog = New SaveFileDialog()
            saveFileDialog.Filter = "导出Excel (*.xls)|*.xls"
            saveFileDialog.FilterIndex = 0
            saveFileDialog.RestoreDirectory = True
            saveFileDialog.Title = "导出文件保存"
            saveFileDialog.ShowDialog()
            strFileName = saveFileDialog.FileName
            If String.IsNullOrEmpty(strFileName) Then
                Return
            End If



            'ReDim arrData(rowCount, colCount)

            xlApp = CreateObject("Excel.Application")
            xlApp.Visible = False
            xlBook = xlApp.Workbooks.Add()

            Dim arrRowCnt As Integer = 0



            For dtIdx As Integer = 1 To 500



                '数据取得
                dtListData = GetCheckData(GetSearchRequirement(), dtIdx)

                If dtListData.Rows.Count < 1 AndAlso dtIdx = 1 Then
                    MessageBox.Show("对象数据不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit For
                ElseIf dtListData.Rows.Count < 1 Then
                    Exit For
                End If

                rowCount = dtListData.Rows.Count
                colCount = dtListData.Columns.Count


                For i As Integer = 0 To rowCount


                    If i = 0 Then
                        ReDim arrData(rowCount, 22)
                        arrData(0, 0) = "id"
                        arrData(0, 1) = "商品cd"
                        arrData(0, 2) = "商品名称"
                        arrData(0, 3) = "部门名称"
                        arrData(0, 4) = "种类名称"
                        arrData(0, 5) = "图片名称"
                        arrData(0, 6) = "分类名"
                        arrData(0, 7) = "分类表示顺"
                        arrData(0, 8) = "类型名称"
                        arrData(0, 9) = "图片ID"
                        arrData(0, 10) = "治具编号"
                        arrData(0, 11) = "治具ID"
                        arrData(0, 12) = "治具表示顺"
                        arrData(0, 13) = "商品种类"
                        arrData(0, 14) = "检查位置"
                        arrData(0, 15) = "检查项目"
                        arrData(0, 16) = "基准类型"
                        arrData(0, 17) = "基准值1"
                        arrData(0, 18) = "基准值2"
                        arrData(0, 19) = "基准值3"
                        arrData(0, 20) = "检查方式"
                        arrData(0, 21) = "检查次数"
                        arrData(0, 22) = "图片描述"

                    Else
                        With dtListData.Rows(i - 1)
                            arrData(i, 0) = Convert.ToString(.Item("id"))
                            arrData(i, 1) = Convert.ToString(.Item("goods_cd"))
                            arrData(i, 2) = Convert.ToString(.Item("goods_name"))
                            arrData(i, 3) = Convert.ToString(.Item("department_name"))
                            arrData(i, 4) = Convert.ToString(.Item("kind_name"))
                            arrData(i, 5) = Convert.ToString(.Item("picture_nm"))
                            arrData(i, 6) = Convert.ToString(.Item("classify_name"))
                            arrData(i, 7) = Convert.ToString(.Item("classify_order"))
                            arrData(i, 8) = Convert.ToString(.Item("type_name"))
                            arrData(i, 9) = Convert.ToString(.Item("picture_id"))
                            arrData(i, 10) = Convert.ToString(.Item("tools_no"))
                            arrData(i, 11) = Convert.ToString(.Item("tools_id"))
                            arrData(i, 12) = Convert.ToString(.Item("tools_order"))
                            arrData(i, 13) = Convert.ToString(.Item("kind"))
                            arrData(i, 14) = Convert.ToString(.Item("check_position"))
                            arrData(i, 15) = Convert.ToString(.Item("check_item"))
                            arrData(i, 16) = Convert.ToString(.Item("benchmark_type"))
                            arrData(i, 17) = Convert.ToString(.Item("benchmark_value1"))
                            arrData(i, 18) = Convert.ToString(.Item("benchmark_value2"))
                            arrData(i, 19) = SetBenchmarkValue3Zheng(Convert.ToString(.Item("benchmark_value3")))
                            arrData(i, 20) = Convert.ToString(.Item("check_way"))
                            arrData(i, 21) = Convert.ToString(.Item("check_times"))
                            arrData(i, 22) = Convert.ToString(.Item("picture_name"))
                        End With

                    End If



                    If i = rowCount Then
                        xlSheet = xlBook.Worksheets.Add()
                        xlSheet.Name = "检查项目信息" & IIf(dtIdx <> 1, dtIdx.ToString, "").ToString
                        xlSheet.Activate()
                        xlSheet.Range("A1").Resize(rowCount + 1, 22).Value = arrData
                    End If





                    'If i = 0 Then
                    '    arrData(0, 0) = "id"
                    '    arrData(0, 1) = "商品cd"
                    '    arrData(0, 2) = "商品名称"
                    '    arrData(0, 3) = "部门名称"
                    '    arrData(0, 4) = "种类名称"
                    '    arrData(0, 5) = "图片名称"
                    '    arrData(0, 6) = "分类名"
                    '    arrData(0, 7) = "分类表示顺"
                    '    arrData(0, 8) = "类型名称"
                    '    arrData(0, 9) = "图片ID"
                    '    arrData(0, 10) = "治具编号"
                    '    arrData(0, 11) = "治具ID"
                    '    arrData(0, 12) = "治具表示顺"
                    '    arrData(0, 13) = "商品种类"
                    '    arrData(0, 14) = "检查位置"
                    '    arrData(0, 15) = "检查项目"
                    '    arrData(0, 16) = "基准类型"
                    '    arrData(0, 17) = "基准值1"
                    '    arrData(0, 18) = "基准值2"
                    '    arrData(0, 19) = "基准值3"
                    '    arrData(0, 20) = "检查方式"
                    '    arrData(0, 21) = "检查次数"
                    '    arrData(0, 22) = "图片描述"
                    'Else
                    '    With dtListData.Rows(i - 1)
                    '        arrData(i, 0) = Convert.ToString(.Item("id"))
                    '        arrData(i, 1) = Convert.ToString(.Item("goods_cd"))
                    '        arrData(i, 2) = Convert.ToString(.Item("goods_name"))
                    '        arrData(i, 3) = Convert.ToString(.Item("department_name"))
                    '        arrData(i, 4) = Convert.ToString(.Item("kind_name"))
                    '        arrData(i, 5) = Convert.ToString(.Item("picture_nm"))
                    '        arrData(i, 6) = Convert.ToString(.Item("classify_name"))
                    '        arrData(i, 7) = Convert.ToString(.Item("classify_order"))
                    '        arrData(i, 8) = Convert.ToString(.Item("type_name"))
                    '        arrData(i, 9) = Convert.ToString(.Item("picture_id"))
                    '        arrData(i, 10) = Convert.ToString(.Item("tools_no"))
                    '        arrData(i, 11) = Convert.ToString(.Item("tools_id"))
                    '        arrData(i, 12) = Convert.ToString(.Item("tools_order"))
                    '        arrData(i, 13) = Convert.ToString(.Item("kind"))
                    '        arrData(i, 14) = Convert.ToString(.Item("check_position"))
                    '        arrData(i, 15) = Convert.ToString(.Item("check_item"))
                    '        arrData(i, 16) = Convert.ToString(.Item("benchmark_type"))
                    '        arrData(i, 17) = Convert.ToString(.Item("benchmark_value1"))
                    '        arrData(i, 18) = Convert.ToString(.Item("benchmark_value2"))
                    '        arrData(i, 19) = SetBenchmarkValue3Zheng(Convert.ToString(.Item("benchmark_value3")))
                    '        arrData(i, 20) = Convert.ToString(.Item("check_way"))
                    '        arrData(i, 21) = Convert.ToString(.Item("check_times"))
                    '        arrData(i, 22) = Convert.ToString(.Item("picture_name"))

                    '    End With
                    'End If



                Next



            Next

            xlBook.SaveAs(strFileName)
            xlBook.Close()
            xlApp.Quit()
            MessageBox.Show("批量导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Throw ex
        Finally
            If Not xlSheet Is Nothing Then
                ReleaseComObject(xlSheet)
            End If
            If Not xlBook Is Nothing Then
                ReleaseComObject(xlBook)
            End If
            If Not xlApp Is Nothing Then
                ReleaseComObject(xlApp)
            End If
        End Try
    End Sub


    ''' <summary>
    ''' EXCEL文件导出处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExportCSV()
        Dim rowCount As Integer
        Dim colCount As Integer
        Dim strFileName As String
        Dim saveFileDialog As SaveFileDialog

        Dim arrData(0, 0) As String
        Dim dtListData As New DataTable

        Dim lstArrData As New List(Of String(,))

        Try
            saveFileDialog = New SaveFileDialog()
            saveFileDialog.Filter = "导出Excel (*.csv)|*.csv"
            saveFileDialog.FilterIndex = 0
            saveFileDialog.RestoreDirectory = True
            saveFileDialog.Title = "导出文件保存"
            saveFileDialog.ShowDialog()
            strFileName = saveFileDialog.FileName
            If String.IsNullOrEmpty(strFileName) Then
                Return
            End If

            Dim arrRowCnt As Integer = 0

            Dim sb As New System.Text.StringBuilder

            sb.Append("id")
            sb.Append("," & "商品cd")
            sb.Append("," & "商品名称")
            sb.Append("," & "部门名称")
            sb.Append("," & "种类名称")
            sb.Append("," & "图片名称")
            sb.Append("," & "分类名")
            sb.Append("," & "分类表示顺")
            sb.Append("," & "类型名称")
            sb.Append("," & "图片ID")
            sb.Append("," & "治具编号")
            sb.Append("," & "治具ID")
            sb.Append("," & "治具表示顺")
            sb.Append("," & "商品种类")
            sb.Append("," & "检查位置")
            sb.Append("," & "检查项目")
            sb.Append("," & "基准类型")
            sb.Append("," & "基准值1")
            sb.Append("," & "基准值2")
            sb.Append("," & "基准值3")
            sb.Append("," & "检查方式")
            sb.Append("," & "检查次数")
            sb.Append("," & "图片描述")
            sb.Append(vbNewLine)
            For dtIdx As Integer = 1 To 500



                '数据取得
                dtListData = GetCheckData(GetSearchRequirement(), dtIdx)

                If dtListData.Rows.Count < 1 AndAlso dtIdx = 1 Then
                    MessageBox.Show("对象数据不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit For
                ElseIf dtListData.Rows.Count < 1 Then
                    Exit For
                End If

                rowCount = dtListData.Rows.Count
                colCount = dtListData.Columns.Count


                For i As Integer = 1 To rowCount

                    With dtListData.Rows(i - 1)
                        sb.Append(Convert.ToString(.Item("id")))
                        sb.Append("," & Convert.ToString(.Item("goods_cd")))
                        sb.Append("," & Convert.ToString(.Item("goods_name")))
                        sb.Append("," & Convert.ToString(.Item("department_name")))
                        sb.Append("," & Convert.ToString(.Item("kind_name")))
                        sb.Append("," & Convert.ToString(.Item("picture_nm")))
                        sb.Append("," & Convert.ToString(.Item("classify_name")))
                        sb.Append("," & Convert.ToString(.Item("classify_order")))
                        sb.Append("," & Convert.ToString(.Item("type_name")))
                        sb.Append("," & Convert.ToString(.Item("picture_id")))
                        sb.Append("," & Convert.ToString(.Item("tools_no")))
                        sb.Append("," & Convert.ToString(.Item("tools_id")))
                        sb.Append("," & Convert.ToString(.Item("tools_order")))
                        sb.Append("," & Convert.ToString(.Item("kind")))
                        sb.Append("," & Convert.ToString(.Item("check_position")))
                        sb.Append("," & Convert.ToString(.Item("check_item")))
                        sb.Append("," & Convert.ToString(.Item("benchmark_type")))
                        sb.Append("," & Convert.ToString(.Item("benchmark_value1")))
                        sb.Append("," & Convert.ToString(.Item("benchmark_value2")))
                        sb.Append("," & SetBenchmarkValue3Zheng(Convert.ToString(.Item("benchmark_value3"))))
                        sb.Append("," & Convert.ToString(.Item("check_way")))
                        sb.Append("," & Convert.ToString(.Item("check_times")))
                        sb.Append("," & Convert.ToString(.Item("picture_name")))
                        sb.Append(vbNewLine)
                    End With

                Next

                Dim sr As New IO.StreamWriter(strFileName, True)
                sr.WriteLine(sb.ToString)
                sr.Close()
                sb.Length = 0
            Next



            MessageBox.Show("批量导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Throw ex
        Finally

        End Try
    End Sub

    Public Function GetNewExcelArr() As String(,)
        Dim arrData(0, 22) As String
        arrData(0, 0) = "id"
        arrData(0, 1) = "商品cd"
        arrData(0, 2) = "商品名称"
        arrData(0, 3) = "部门名称"
        arrData(0, 4) = "种类名称"
        arrData(0, 5) = "图片名称"
        arrData(0, 6) = "分类名"
        arrData(0, 7) = "分类表示顺"
        arrData(0, 8) = "类型名称"
        arrData(0, 9) = "图片ID"
        arrData(0, 10) = "治具编号"
        arrData(0, 11) = "治具ID"
        arrData(0, 12) = "治具表示顺"
        arrData(0, 13) = "商品种类"
        arrData(0, 14) = "检查位置"
        arrData(0, 15) = "检查项目"
        arrData(0, 16) = "基准类型"
        arrData(0, 17) = "基准值1"
        arrData(0, 18) = "基准值2"
        arrData(0, 19) = "基准值3"
        arrData(0, 20) = "检查方式"
        arrData(0, 21) = "检查次数"
        arrData(0, 22) = "图片描述"
        Return arrData
    End Function

    Public Function AddToArr(ByRef arrData(,) As String, ByVal dr As DataRow) As Boolean

        Dim i As Integer = UBound(arrData) + 1

        ReDim Preserve arrData(i, 22)

        With dr
            arrData(i, 0) = Convert.ToString(.Item("id"))
            arrData(i, 1) = Convert.ToString(.Item("goods_cd"))
            arrData(i, 2) = Convert.ToString(.Item("goods_name"))
            arrData(i, 3) = Convert.ToString(.Item("department_name"))
            arrData(i, 4) = Convert.ToString(.Item("kind_name"))
            arrData(i, 5) = Convert.ToString(.Item("picture_nm"))
            arrData(i, 6) = Convert.ToString(.Item("classify_name"))
            arrData(i, 7) = Convert.ToString(.Item("classify_order"))
            arrData(i, 8) = Convert.ToString(.Item("type_name"))
            arrData(i, 9) = Convert.ToString(.Item("picture_id"))
            arrData(i, 10) = Convert.ToString(.Item("tools_no"))
            arrData(i, 11) = Convert.ToString(.Item("tools_id"))
            arrData(i, 12) = Convert.ToString(.Item("tools_order"))
            arrData(i, 13) = Convert.ToString(.Item("kind"))
            arrData(i, 14) = Convert.ToString(.Item("check_position"))
            arrData(i, 15) = Convert.ToString(.Item("check_item"))
            arrData(i, 16) = Convert.ToString(.Item("benchmark_type"))
            arrData(i, 17) = Convert.ToString(.Item("benchmark_value1"))
            arrData(i, 18) = Convert.ToString(.Item("benchmark_value2"))
            arrData(i, 19) = SetBenchmarkValue3Zheng(Convert.ToString(.Item("benchmark_value3")))
            arrData(i, 20) = Convert.ToString(.Item("check_way"))
            arrData(i, 21) = Convert.ToString(.Item("check_times"))
            arrData(i, 22) = Convert.ToString(.Item("picture_name"))
        End With

    End Function


    ''' <summary>
    ''' 读取EXCEL指定页中的内容
    ''' </summary>
    ''' <param name="strSheetName">页名</param>
    ''' <param name="strExcelFile">EXCEL路径</param>
    ''' <param name="strFileType">EXCEL扩展名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetExcelData(ByVal strSheetName As String,
                                  ByVal strExcelFile As String,
                                  ByVal strFileType As String) As DataTable

        Dim strConn As String = LogExport.GetOleDbConn(strExcelFile)
        Dim strExcel As String
        Dim ds As DataSet
        Dim adapter As OleDb.OleDbDataAdapter = Nothing
        Dim conn As OleDb.OleDbConnection = Nothing
        Dim xlApp = CreateObject("Excel.Application")

        Try

            'Sql语句
            strExcel = String.Format("SELECT * from [{0}$]", strSheetName)
            '定义存放的数据表
            ds = New DataSet()
            '连接数据源
            conn = New OleDb.OleDbConnection(strConn)
            conn.Open()
            '适配到数据源
            adapter = New OleDb.OleDbDataAdapter(strExcel, strConn)
            adapter.Fill(ds, strSheetName)

            Return ds.Tables(strSheetName)
        Catch ex As Exception
            Throw ex
        Finally
            If Not conn Is Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                conn.Dispose()
            End If
            If Not adapter Is Nothing Then
                adapter.Dispose()
            End If
        End Try
    End Function

    ''' <summary>
    ''' 变更项目的判定
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ChangeCheck() As Dictionary(Of String, String)
        Dim dicChange As New Dictionary(Of String, String)
        '商品名称
        If Me.txtGoodsName.Text <> GOODSNAME Then
            dicChange.Add(GOODSNAME_TEXT, Me.txtGoodsName.Text.Trim)
        End If

        '治具编号
        If Me.txtEditToolNo.Text <> TOOLNO Then
            'dicChange.Add(TOOLNO_TEXT, Me.txtEditToolNo.Text.Trim)
            dicChange.Add(TOOLNO_TEXT, toolsNo)
            toolsNo = String.Empty
        End If
        '治具表示顺
        If Me.txtToolDispNo.Text <> TOOLDISP Then
            dicChange.Add(TOOLDISP_TEXT, Me.txtToolDispNo.Text.Trim)
        End If

        '分类
        If Me.txtEditClassify.Text <> CLASSIFY Then
            dicChange.Add(CLASSIFY_TEXT, Me.txtEditClassify.Text.Trim)
        End If

        '分类表示顺
        If Me.txtClassifyDispNo.Text <> CLASSIFYDISP Then
            dicChange.Add(CLASSIFYDISP_TEXT, Me.txtClassifyDispNo.Text.Trim)
        End If

        '部门
        If Me.drpEditDepartment.SelectedValue.ToString <> DEPARTMENT Then
            dicChange.Add(DEPARTMENT_TEXT, Me.drpEditDepartment.SelectedValue.ToString)
        End If

        '类别
        If Me.txtKind.Text <> GOODSKIND Then
            dicChange.Add(GOODSKIND_TEXT, Me.txtKind.Text.Trim)
        End If

        '检查位置
        If Me.txtChkPosition.Text <> CHKPOSITION Then
            dicChange.Add(CHKPOSITION_TEXT, Me.txtChkPosition.Text.Trim)
        End If

        '检查项目
        If Me.txtChkProject.Text <> CHKITEM Then
            dicChange.Add(CHKITEM_TEXT, Me.txtChkProject.Text.Trim)
        End If

        '基准类型
        If Me.txtBenchmarkType.Text <> BMTYPE Then
            dicChange.Add(BMTYPE_TEXT, Me.txtBenchmarkType.Text.Trim)
        End If

        '基准值1
        If Me.txtBenchmarkValue1.Text <> BMVALUE1 Then
            dicChange.Add(BMVALUE1_TEXT, Me.txtBenchmarkValue1.Text.Trim)
        End If

        '基准值2
        If Me.txtBenchmarkValue2.Text <> BMVALUE2 Then
            dicChange.Add(BMVALUE2_TEXT, Me.txtBenchmarkValue2.Text.Trim)
        End If

        '基准值3
        If Me.txtBenchmarkValue3.Text <> BMVALUE3 Then
            dicChange.Add(BMVALUE3_TEXT, Me.txtBenchmarkValue3.Text.Trim)
        End If

        '检查方式
        If Me.txtChkWay.Text <> CHKWAY Then
            dicChange.Add(CHKWAY_TEXT, Me.txtChkWay.Text.Trim)
        End If

        '检查次数
        If Me.txtChktimes.Text <> CHKTIMES Then
            dicChange.Add(CHKTIMES_TEXT, Me.txtChktimes.Text.Trim)
        End If

        '图片ID
        If Me.txtImgId.Text <> IMGID Then
            dicChange.Add(IMGID_TEXT, Me.txtImgId.Text.Trim)
        End If

        Return dicChange
    End Function

    ''' <summary>
    ''' バイナリtoイメージ
    ''' </summary>
    ''' <param name="bytes">画像バイナリ</param>
    ''' <remarks></remarks>
    Public Function GetImageFromByteArray(ByRef bytes As Byte()) As Image
        Dim ms As New MemoryStream
        ms.Write(bytes, 0, bytes.Length)
        Dim originalImage As Image = Image.FromStream(ms)
        'originalImage.Dispose()
        ms.Dispose()
        Return originalImage

    End Function

    ''' <summary>
    ''' イメージtoバイナリ
    ''' </summary>
    ''' <param name="img"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByteArrayFromImage(ByVal img As Bitmap) As Byte()
        Dim ms As New System.IO.MemoryStream
        img.Save(ms, Imaging.ImageFormat.Bmp)
        Dim outBytes(CInt(ms.Length - 1)) As Byte
        ms.Seek(0, System.IO.SeekOrigin.Begin)
        ms.Read(outBytes, 0, CInt(ms.Length))
        ms.Dispose()
        Return outBytes
    End Function

    ''' <summary>
    ''' Log文件出力用Datatable初始化
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitTable()
        '更新前数据保存用Datatable初始化
        beforeUpdDt = New DataTable
        beforeUpdDt.TableName = "BeforeUpdate"
        beforeUpdDt.Columns.Add("id", GetType(String))
        beforeUpdDt.Columns.Add("商品cd", GetType(String))
        beforeUpdDt.Columns.Add("商品名称", GetType(String))
        beforeUpdDt.Columns.Add("部门名称", GetType(String))
        beforeUpdDt.Columns.Add("种类名称", GetType(String))
        beforeUpdDt.Columns.Add("分类名", GetType(String))
        beforeUpdDt.Columns.Add("分类表示顺", GetType(String))
        beforeUpdDt.Columns.Add("类型名称", GetType(String))
        beforeUpdDt.Columns.Add("图片ID", GetType(String))
        beforeUpdDt.Columns.Add("治具编号", GetType(String))
        beforeUpdDt.Columns.Add("治具表示顺", GetType(String))
        beforeUpdDt.Columns.Add("商品种类", GetType(String))
        beforeUpdDt.Columns.Add("检查位置", GetType(String))
        beforeUpdDt.Columns.Add("检查项目", GetType(String))
        beforeUpdDt.Columns.Add("基准类型", GetType(String))
        beforeUpdDt.Columns.Add("基准值1", GetType(String))
        beforeUpdDt.Columns.Add("基准值2", GetType(String))
        beforeUpdDt.Columns.Add("基准值3", GetType(String))
        beforeUpdDt.Columns.Add("检查方式", GetType(String))
        beforeUpdDt.Columns.Add("检查次数", GetType(String))

        afterUpdDt = New DataTable
        afterUpdDt.TableName = "AfterUpdate"
        afterUpdDt.Columns.Add("id", GetType(String))
        afterUpdDt.Columns.Add("商品cd", GetType(String))
        afterUpdDt.Columns.Add("商品名称", GetType(String))
        afterUpdDt.Columns.Add("部门名称", GetType(String))
        afterUpdDt.Columns.Add("种类名称", GetType(String))
        afterUpdDt.Columns.Add("分类名", GetType(String))
        afterUpdDt.Columns.Add("分类表示顺", GetType(String))
        afterUpdDt.Columns.Add("类型名称", GetType(String))
        afterUpdDt.Columns.Add("图片ID", GetType(String))
        afterUpdDt.Columns.Add("治具编号", GetType(String))
        afterUpdDt.Columns.Add("治具表示顺", GetType(String))
        afterUpdDt.Columns.Add("商品种类", GetType(String))
        afterUpdDt.Columns.Add("检查位置", GetType(String))
        afterUpdDt.Columns.Add("检查项目", GetType(String))
        afterUpdDt.Columns.Add("基准类型", GetType(String))
        afterUpdDt.Columns.Add("基准值1", GetType(String))
        afterUpdDt.Columns.Add("基准值2", GetType(String))
        afterUpdDt.Columns.Add("基准值3", GetType(String))
        afterUpdDt.Columns.Add("检查方式", GetType(String))
        afterUpdDt.Columns.Add("检查次数", GetType(String))
    End Sub

    '''' <summary>
    '''' 插入处理
    '''' </summary>
    '''' <param name="dicInsert"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Function InsertCheckMs(ByVal dicInsert As Dictionary(Of String, String), ByVal singleFlg As Boolean) As Integer
    '    Dim result As Integer

    '    result = bc.InsertCheckMs(dicInsert, singleFlg)

    '    Return result
    'End Function

#End Region

#Region "输入检查"

    ''' <summary>
    ''' 批量导入数据检查
    ''' </summary>
    ''' <param name="drExcel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ImportCheck(ByVal drExcel As DataRow) As Boolean
        Try
            If Not String.IsNullOrEmpty(drExcel("id").ToString.Trim) Then
                If drExcel("id").ToString.Trim.Length > 9 OrElse
                    Not FormatCheck.IsAlphaNumber(drExcel("id").ToString.Trim) Then
                    strMessage = String.Format(MsgConst.M00028I, "id")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("商品cd").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "商品cd")
                Return False
            Else
                If drExcel("商品cd").ToString.Trim.Length > 30 OrElse
                    Not FormatCheck.IsAlphaNumber(drExcel("商品cd").ToString.Trim) Then
                    strMessage = String.Format(MsgConst.M00028I, "商品cd")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("商品名称").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "商品名称")
                Return False
            Else
                If drExcel("商品名称").ToString.Trim.Length > 50 Then
                    strMessage = String.Format(MsgConst.M00047I, "商品名称")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("部门名称").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "部门名称")
                Return False
            Else
                If drExcel("部门名称").ToString.Trim.Length > 25 Then
                    strMessage = String.Format(MsgConst.M00047I, "部门名称")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("种类名称").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "种类名称")
                Return False
            Else
                If drExcel("种类名称").ToString.Trim.Length > 25 Then
                    strMessage = String.Format(MsgConst.M00047I, "种类名称")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("类型名称").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "类型名称")
                Return False
            Else
                If drExcel("类型名称").ToString.Trim.Length > 25 Then
                    strMessage = String.Format(MsgConst.M00047I, "类型名称")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("分类名").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "分类名")
                Return False
            Else
                If drExcel("分类名").ToString.Trim.Length > 50 Then
                    strMessage = String.Format(MsgConst.M00047I, "分类名")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("分类表示顺").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "分类表示顺")
                Return False
            Else
                If drExcel("分类表示顺").ToString.Trim.Length > 3 OrElse
                    Not FormatCheck.IsAlphaNumberOnly(drExcel("分类表示顺").ToString.Trim) Then
                    strMessage = String.Format(MsgConst.M00028I, "分类表示顺")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("图片ID").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "图片ID")
                Return False
            Else
                If drExcel("图片ID").ToString.Trim.Length > 7 OrElse
                    Not FormatCheck.IsAlphaNumberOnly(drExcel("图片ID").ToString.Trim) Then
                    strMessage = String.Format(MsgConst.M00028I, "图片ID")
                    Return False
                End If
            End If

            'If Not String.IsNullOrEmpty(drExcel("治具编号").ToString.Trim) Then
            '    If drExcel("治具编号").ToString.Trim.Length > 30 OrElse _
            '        Not FormatCheck.IsAlphaNumberOnly(drExcel("治具编号").ToString.Trim) Then
            '        strMessage = String.Format(MsgConst.M00028I, "治具编号")
            '        Return False
            '    End If
            'End If
            If Not String.IsNullOrEmpty(drExcel("治具ID").ToString.Trim) Then
                If drExcel("治具ID").ToString.Trim.Length > 7 Then
                    strMessage = String.Format(MsgConst.M00047I, "治具ID")
                    Return False
                End If
            End If

            If Not String.IsNullOrEmpty(drExcel("治具表示顺").ToString.Trim) Then
                If drExcel("治具表示顺").ToString.Trim.Length > 3 OrElse
                    Not FormatCheck.IsAlphaNumberOnly(drExcel("治具表示顺").ToString.Trim) Then
                    strMessage = String.Format(MsgConst.M00028I, "治具表示顺")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("商品种类").ToString.Trim) Then
                'strMessage = String.Format(MsgConst.M00002I, "商品种类")
                'Return False
            Else
                If drExcel("商品种类").ToString.Trim.Length > 50 Then
                    strMessage = String.Format(MsgConst.M00047I, "商品种类")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("检查位置").ToString.Trim) Then
                'strMessage = String.Format(MsgConst.M00002I, "检查位置")
                'Return False
            Else
                If drExcel("检查位置").ToString.Trim.Length > 50 Then
                    strMessage = String.Format(MsgConst.M00047I, "检查位置")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("检查项目").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "检查项目")
                Return False
            Else
                If drExcel("检查项目").ToString.Trim.Length > 250 Then
                    strMessage = String.Format(MsgConst.M00047I, "检查项目")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("基准类型").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "基准类型")
                Return False
            Else
                If drExcel("基准类型").ToString.Trim.Length > 2 OrElse
                    Not FormatCheck.IsAlphaNumberOnly(drExcel("基准类型").ToString.Trim) Then
                    strMessage = String.Format(MsgConst.M00028I, "基准类型")
                    Return False
                End If
            End If

            If Not String.IsNullOrEmpty(drExcel("基准值1").ToString.Trim) Then
                If drExcel("基准值1").ToString.Trim.Length > 20 OrElse
                    Not FormatCheck.IsAlphaNumber(drExcel("基准值1").ToString.Trim) Then
                    strMessage = String.Format(MsgConst.M00028I, "基准值1")
                    Return False
                End If
            End If

            If Not String.IsNullOrEmpty(drExcel("基准值2").ToString.Trim) Then
                If drExcel("基准值2").ToString.Trim.Length > 20 OrElse
                    Not FormatCheck.IsAlphaNumber(drExcel("基准值2").ToString.Trim) Then
                    strMessage = String.Format(MsgConst.M00028I, "基准值2")
                    Return False
                End If
            End If

            If Not String.IsNullOrEmpty(drExcel("基准值3").ToString.Trim) Then
                If drExcel("基准值3").ToString.Trim.Length > 20 OrElse
                    Not FormatCheck.IsAlphaNumber(drExcel("基准值3").ToString.Trim) Then
                    strMessage = String.Format(MsgConst.M00028I, "基准值3")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("检查方式").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "检查方式")
                Return False
            Else
                If drExcel("检查方式").ToString.Trim.Length > 25 Then
                    strMessage = String.Format(MsgConst.M00047I, "检查方式")
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(drExcel("检查次数").ToString.Trim) Then
                strMessage = String.Format(MsgConst.M00002I, "检查次数")
                Return False
            Else
                If drExcel("检查次数").ToString.Trim.Length > 2 OrElse
                    Not FormatCheck.IsHalfNumberOnly(drExcel("检查次数").ToString.Trim) Then
                    strMessage = String.Format(MsgConst.M00028I, "检查次数")
                    Return False
                Else
                    If CInt(drExcel("检查次数").ToString.Trim) < 1 OrElse
                        CInt(drExcel("检查次数").ToString.Trim) > 99 Then
                        strMessage = String.Format(MsgConst.M00055I, "检查次数")
                        Return False
                    End If
                End If
            End If

            '检查项目id存在检查
            If Not String.IsNullOrEmpty(drExcel("id").ToString.Trim) Then
                Dim dtCheck As New DataTable
                dtCheck = bc.GetCheckId(drExcel("id").ToString.Trim)
                If dtCheck.Rows.Count = 0 Then
                    strMessage = String.Format(MsgConst.M00018I, "id")
                    Return False
                Else
                    '更新的时候，更新前的数据保存到Datatable中
                    beforeUpdDt.Rows.Add(dtCheck.Rows(0).ItemArray)
                End If
            End If

            '治具编号存在检查
            If drExcel("治具ID").ToString.Trim <> String.Empty Then
                If CheckToolId(drExcel("治具ID").ToString.Trim) = False Then
                    strMessage = String.Format(MsgConst.M00018I, "治具ID")
                    Return False
                End If
            End If

            '图片ID存在检查
            If CheckImgId(drExcel("图片ID").ToString.Trim) = False Then
                strMessage = String.Format(MsgConst.M00018I, "图片ID")
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 登录/更新时输入项目检查
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InputCheck(ByVal updFlg As Boolean) As Boolean
        Dim dtGoodsName As New DataTable
        If updFlg = False Then
            '新建的时候
            '商品CD
            If String.IsNullOrEmpty(Me.txtEditGoodsCd.Text.Trim) Then
                MessageBox.Show(String.Format(MsgConst.M00002I, "商品CD"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.txtEditGoodsCd.Focus()
                Return False
            Else
                If Not FormatCheck.IsAlphaNumber(Me.txtEditGoodsCd.Text.Trim) Then
                    MessageBox.Show(String.Format(MsgConst.M00028I, "商品CD"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Me.txtEditGoodsCd.Focus()
                    Return False
                End If
            End If
            '种类
            If String.IsNullOrEmpty(Me.drpEditKindCd.SelectedValue.ToString) Then
                MessageBox.Show(String.Format(MsgConst.M00002I, "种类"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.drpEditKindCd.Focus()
                Return False
            End If
            '类型
            If String.IsNullOrEmpty(Me.drpEditType.SelectedValue.ToString) Then
                MessageBox.Show(String.Format(MsgConst.M00002I, "类型"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.drpEditType.Focus()
                Return False
            End If
        End If

        '商品名称
        If String.IsNullOrEmpty(Me.txtGoodsName.Text.Trim) Then
            MessageBox.Show(String.Format(MsgConst.M00002I, "商品名称"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.txtGoodsName.Focus()
            Return False
        Else
            If updFlg = False Then
                '新建的时候，商品CD存在的时候，商品名称一致性判断
                dtGoodsName = bc.GetGoodId(Me.txtEditGoodsCd.Text.Trim)
                If dtGoodsName.Rows.Count > 0 Then
                    If Me.txtGoodsName.Text.Trim <> dtGoodsName.Rows(0).Item("goods_name").ToString Then
                        MessageBox.Show(String.Format(MsgConst.M00054I, "商品名称"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Me.txtGoodsName.Focus()
                        Return False
                    End If
                End If
            End If

        End If
        '治具编号
        'If Not String.IsNullOrEmpty(Me.txtEditToolNo.Text.Trim) Then
        '    If Not FormatCheck.IsAlphaNumberOnly(Me.txtEditToolNo.Text.Trim) Then
        '        MessageBox.Show(String.Format(MsgConst.M00028I, "治具编号"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        Me.txtEditToolNo.Focus()
        '        Return False
        '    End If
        'End If

        '治具表示顺
        If Not String.IsNullOrEmpty(Me.txtToolDispNo.Text.Trim) Then
            If Not FormatCheck.IsAlphaNumberOnly(Me.txtToolDispNo.Text.Trim) Then
                MessageBox.Show(String.Format(MsgConst.M00028I, "治具表示顺"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.txtToolDispNo.Focus()
                Return False
            End If
        End If
        '分类
        If String.IsNullOrEmpty(Me.txtEditClassify.Text.Trim) Then
            MessageBox.Show(String.Format(MsgConst.M00002I, "分类名"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.txtEditClassify.Focus()
            Return False
        End If
        '分类表示顺
        If String.IsNullOrEmpty(Me.txtClassifyDispNo.Text.Trim) Then
            MessageBox.Show(String.Format(MsgConst.M00002I, "分类表示顺"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.txtClassifyDispNo.Focus()
            Return False
        Else
            If Not FormatCheck.IsAlphaNumberOnly(Me.txtClassifyDispNo.Text.Trim) Then
                MessageBox.Show(String.Format(MsgConst.M00028I, "分类表示顺"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.txtClassifyDispNo.Focus()
                Return False
            End If
        End If
        '部门
        If String.IsNullOrEmpty(Me.drpEditDepartment.SelectedValue.ToString) Then
            MessageBox.Show(String.Format(MsgConst.M00002I, "部门"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.drpEditDepartment.Focus()
            Return False
        End If
        '类别
        'If String.IsNullOrEmpty(Me.txtKind.Text.Trim) Then
        '    MessageBox.Show(String.Format(MsgConst.M00002I, "类别"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '    Me.txtKind.Focus()
        '    Return False
        'End If
        '检查位置
        'If String.IsNullOrEmpty(Me.txtChkPosition.Text.Trim) Then
        '    MessageBox.Show(String.Format(MsgConst.M00002I, "检查位置"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '    Me.txtChkPosition.Focus()
        '    Return False
        'End If
        '检查项目
        If String.IsNullOrEmpty(Me.txtChkProject.Text.Trim) Then
            MessageBox.Show(String.Format(MsgConst.M00002I, "检查项目"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.txtChkProject.Focus()
            Return False
        End If
        '基准类型
        If String.IsNullOrEmpty(Me.txtBenchmarkType.Text.Trim) Then
            MessageBox.Show(String.Format(MsgConst.M00002I, "基准类型"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.txtBenchmarkType.Focus()
            Return False
        Else
            If Not FormatCheck.IsAlphaNumberOnly(Me.txtBenchmarkType.Text.Trim) Then
                MessageBox.Show(String.Format(MsgConst.M00028I, "基准类型"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.txtBenchmarkType.Focus()
                Return False
            End If
        End If
        '基准值1
        If Not String.IsNullOrEmpty(Me.txtBenchmarkValue1.Text.Trim) Then
            If Not FormatCheck.IsAlphaNumber(Me.txtBenchmarkValue1.Text.Trim) Then
                MessageBox.Show(String.Format(MsgConst.M00028I, "基准值1"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.txtBenchmarkValue1.Focus()
                Return False
            End If
        End If
        '基准值2
        If Not String.IsNullOrEmpty(Me.txtBenchmarkValue2.Text.Trim) Then
            If Not FormatCheck.IsAlphaNumber(Me.txtBenchmarkValue2.Text.Trim) Then
                MessageBox.Show(String.Format(MsgConst.M00028I, "基准值2"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.txtBenchmarkValue2.Focus()
                Return False
            End If
        End If
        '基准值3
        If Not String.IsNullOrEmpty(Me.txtBenchmarkValue3.Text.Trim) Then
            If Not FormatCheck.IsAlphaNumber(Me.txtBenchmarkValue3.Text.Trim) Then
                MessageBox.Show(String.Format(MsgConst.M00028I, "基准值3"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.txtBenchmarkValue3.Focus()
                Return False
            End If
        End If
        '检查方式
        If String.IsNullOrEmpty(Me.txtChkWay.Text.Trim) Then
            MessageBox.Show(String.Format(MsgConst.M00002I, "检查方式"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.txtChkWay.Focus()
            Return False
        End If
        '检查次数
        If String.IsNullOrEmpty(Me.txtChktimes.Text.Trim) Then
            MessageBox.Show(String.Format(MsgConst.M00002I, "检查次数"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.txtChktimes.Focus()
            Return False
        Else
            If Me.txtChktimes.Text.Trim.Length > 2 OrElse
                   Not FormatCheck.IsHalfNumberOnly(Me.txtChktimes.Text.Trim) Then
                MessageBox.Show(String.Format(MsgConst.M00028I, "检查次数"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.txtChktimes.Focus()
                Return False
            Else
                If CInt(Me.txtChktimes.Text.Trim) < 1 OrElse
                    CInt(Me.txtChktimes.Text.Trim) > 99 Then
                    MessageBox.Show(String.Format(MsgConst.M00055I, "检查次数"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Me.txtChktimes.Focus()
                    Return False
                End If
            End If
        End If
        '图片ID
        If String.IsNullOrEmpty(Me.txtImgId.Text.Trim) Then
            MessageBox.Show(String.Format(MsgConst.M00002I, "图片ID"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.txtImgId.Focus()
            Return False
        Else
            If Not FormatCheck.IsAlphaNumberOnly(Me.txtImgId.Text.Trim) Then
                MessageBox.Show(String.Format(MsgConst.M00028I, "图片ID"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.txtImgId.Focus()
                Return False
            End If
        End If

        '治具编号存在检查
        If Me.txtEditToolNo.Text.Trim <> String.Empty Then
            If CheckToolNO(Me.txtEditToolNo.Text.Trim) = False Then
                '??????message
                MessageBox.Show(String.Format(MsgConst.M00018I, "治具编号"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If

        '图片ID存在检查
        If CheckImgId(Me.txtImgId.Text.Trim) = False Then
            '??????message
            MessageBox.Show(String.Format(MsgConst.M00018I, "图片ID"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' 治具编号存在检查
    ''' </summary>
    ''' <param name="toolNo"></param>
    ''' <returns>true:存在 false:不存在</returns>
    ''' <remarks></remarks>
    Public Function CheckToolNO(ByVal toolNo As String) As Boolean
        If bc.GetTool(toolNo).Rows.Count > 0 Then
            toolsNo = bc.GetTool(toolNo).Rows(0).Item("id").ToString
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 治具ID存在检查
    ''' </summary>
    ''' <param name="toolId"></param>
    ''' <returns>true:存在 false:不存在</returns>
    ''' <remarks></remarks>
    Public Function CheckToolId(ByVal toolId As String) As Boolean
        Dim dtTool As DataTable = Nothing
        dtTool = bc.GetToolId(toolId)
        If dtTool.Rows.Count > 0 Then
            toolsNo = dtTool.Rows(0).Item("id").ToString
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 图片ID存在检查
    ''' </summary>
    ''' <param name="imgId"></param>
    ''' <returns>true:存在 false:不存在</returns>
    ''' <remarks></remarks>
    Public Function CheckImgId(ByVal imgId As String) As Boolean
        If bc.GetPicture(imgId).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 分类表中情报存在检查
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckMClassify(ByVal id As String) As Boolean
        If bc.GetMClassify(id).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
#End Region

#Region "Back"
    '''' <summary>
    '''' 判断列表是初始化状态还是选择过(与类型联动判断用)
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub drpSelKind_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpSelKind.DropDown
    '    '列表选择时，选择Flg设为true
    '    drpFlg = True
    'End Sub

    '''' <summary>
    '''' 种类选择改变，对应的类型值改变
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub drpSelKind_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpSelKind.SelectedIndexChanged
    '    ' 种类选择改变时，类型值重新设定
    '    If drpFlg = True Then
    '        '类型初始化
    '        TypeInit()
    '        '选择Flg复位
    '        drpFlg = False
    '    End If
    'End Sub
#End Region

#Region "背景色变化处理"
    ''' <summary>
    ''' 治具编号变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtEditToolNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEditToolNo.TextChanged

        If updFlg = True AndAlso Me.txtEditToolNo.Text <> TOOLNO Then
            txtEditToolNo.BackColor = Color.LightPink
        Else
            txtEditToolNo.BackColor = Color.White
        End If

    End Sub

    ''' <summary>
    ''' 商品名称变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtGoodsName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGoodsName.TextChanged
        If updFlg = True AndAlso Me.txtGoodsName.Text <> GOODSNAME Then
            txtGoodsName.BackColor = Color.LightPink
        Else
            txtGoodsName.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 治具表示顺变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtToolDispNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToolDispNo.TextChanged
        If updFlg = True AndAlso Me.txtToolDispNo.Text <> TOOLDISP Then
            txtToolDispNo.BackColor = Color.LightPink
        Else
            txtToolDispNo.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 分类名变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtEditClassify_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEditClassify.TextChanged
        If updFlg = True AndAlso Me.txtEditClassify.Text <> CLASSIFY Then
            txtEditClassify.BackColor = Color.LightPink
        Else
            txtEditClassify.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 分类表示顺变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtClassifyDispNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtClassifyDispNo.TextChanged
        If updFlg = True AndAlso Me.txtClassifyDispNo.Text <> CLASSIFYDISP Then
            txtClassifyDispNo.BackColor = Color.LightPink
        Else
            txtClassifyDispNo.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 部门变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub drpEditDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpEditDepartment.SelectedIndexChanged
        If updFlg = True AndAlso Me.drpEditDepartment.SelectedValue.ToString <> DEPARTMENT Then
            drpEditDepartment.BackColor = Color.LightPink
        Else
            drpEditDepartment.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 类别变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtKind_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKind.TextChanged
        If updFlg = True AndAlso Me.txtKind.Text <> GOODSKIND Then
            txtKind.BackColor = Color.LightPink
        Else
            txtKind.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 检查位置变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtChkPosition_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtChkPosition.TextChanged
        If updFlg = True AndAlso Me.txtChkPosition.Text <> CHKPOSITION Then
            txtChkPosition.BackColor = Color.LightPink
        Else
            txtChkPosition.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 检查项目变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtChkProject_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtChkProject.TextChanged
        If updFlg = True AndAlso Me.txtChkProject.Text <> CHKITEM Then
            txtChkProject.BackColor = Color.LightPink
        Else
            txtChkProject.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 基准类型变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtBenchmarkType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBenchmarkType.TextChanged
        If updFlg = True AndAlso Me.txtBenchmarkType.Text <> BMTYPE Then
            txtBenchmarkType.BackColor = Color.LightPink
        Else
            txtBenchmarkType.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 基准值1变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtBenchmarkValue1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBenchmarkValue1.TextChanged
        If updFlg = True AndAlso Me.txtBenchmarkValue1.Text <> BMVALUE1 Then
            txtBenchmarkValue1.BackColor = Color.LightPink
        Else
            txtBenchmarkValue1.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 基准值2变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtBenchmarkValue2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBenchmarkValue2.TextChanged
        If updFlg = True AndAlso Me.txtBenchmarkValue2.Text <> BMVALUE2 Then
            txtBenchmarkValue2.BackColor = Color.LightPink
        Else
            txtBenchmarkValue2.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 基准值3变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtBenchmarkValue3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBenchmarkValue3.TextChanged
        If updFlg = True AndAlso Me.txtBenchmarkValue3.Text <> BMVALUE3 Then
            txtBenchmarkValue3.BackColor = Color.LightPink
        Else
            txtBenchmarkValue3.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 检查方式变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtChkWay_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtChkWay.TextChanged
        If updFlg = True AndAlso Me.txtChkWay.Text <> CHKWAY Then
            txtChkWay.BackColor = Color.LightPink
        Else
            txtChkWay.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 检查次数变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtChktimes_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtChktimes.TextChanged
        If updFlg = True AndAlso Me.txtChktimes.Text <> CHKTIMES Then
            txtChktimes.BackColor = Color.LightPink
        Else
            txtChktimes.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 图片ID变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtImgId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImgId.TextChanged
        If updFlg = True AndAlso Me.txtImgId.Text <> IMGID Then
            txtImgId.BackColor = Color.LightPink
        Else
            txtImgId.BackColor = Color.White
        End If
    End Sub

#End Region

#Region "其他"

    ''' <summary>
    ''' 点击一下就能选中或者取消选中（默认模式是先获得焦点，然后才能设置选中，需要点击两次）
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub chkLKind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLKind.Click, chkLDepartment.Click, chkLType.Click
        Dim chk As ColorCodedCheckedListBox = DirectCast(sender, ColorCodedCheckedListBox)
        chk.SetSelected(chk.SelectedIndex, True)
    End Sub
#End Region



End Class