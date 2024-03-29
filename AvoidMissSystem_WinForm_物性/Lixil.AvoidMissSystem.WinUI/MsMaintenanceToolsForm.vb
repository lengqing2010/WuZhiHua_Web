Imports System.Data.SqlClient
'Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices.Marshal
Imports System.IO
Imports System.Windows
Imports Lixil.AvoidMissSystem.BizLogic
Imports Lixil.AvoidMissSystem.Utilities
Imports Lixil.AvoidMissSystem.Utilities.Consts

''' <summary>
''' 治具MS维护
''' </summary>
''' <remarks></remarks>
Public Class MsMaintenanceToolsForm
    Private ComBizLogic As CommonBizLogic
    Private toolsBL As New MsMaintenanceToolsBizLogic
    Private isFormLoad As Boolean
    Private strDepartCd As String
    Private strToolsNo As String
    Private strDistinguish As String
    Private strBarcodeFlg As String
    Private strBarcode As String
    Private strRemarks As String
    Private dicGridHeader As Dictionary(Of String, String)
    Private dicExcelHeader As Dictionary(Of String, String)
    Private dicBarcodeFlg As Dictionary(Of String, String)
    Private dicSearchBarcodeFlg As Dictionary(Of String, String)
    Private updFlg As Boolean = False
    Dim beforeUpdDt As DataTable
    Dim afterUpdDt As DataTable

#Region "事件"

    ''' <summary>
    ''' 窗体加载处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MsMaintenanceToolsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            isFormLoad = True
            '治具一览列头设定
            dicGridHeader = New Dictionary(Of String, String)
            dicGridHeader.Add("id", "ID")
            dicGridHeader.Add("tools_no", "治具编号")
            dicGridHeader.Add("department", "部门")
            dicGridHeader.Add("distinguish", "治具区分")
            dicGridHeader.Add("barcode", "条形码")
            dicGridHeader.Add("barcode_flg", "基准值")
            dicGridHeader.Add("remarks", "备注")

            '导入EXCEL文件列头设定
            dicExcelHeader = New Dictionary(Of String, String)
            dicExcelHeader.Add("id", "ID")
            dicExcelHeader.Add("tools_no", "治具编号")
            dicExcelHeader.Add("department_cd", "部门CD")
            dicExcelHeader.Add("distinguish", "治具区分")
            dicExcelHeader.Add("barcode_flg", "基准值")
            dicExcelHeader.Add("barcode", "条形码")
            dicExcelHeader.Add("remarks", "备注")

            '基准值下拉列表设定
            dicSearchBarcodeFlg = New Dictionary(Of String, String)
            dicSearchBarcodeFlg.Add("", "")
            dicSearchBarcodeFlg.Add("0", "无条码")
            dicSearchBarcodeFlg.Add("1", "有条码")

            dicBarcodeFlg = New Dictionary(Of String, String)
            dicBarcodeFlg.Add("0", "无条码")
            dicBarcodeFlg.Add("1", "有条码")

            '窗体初期化设定
            Initialize()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    ''' <summary>
    ''' 模板下载连接点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lblTemplateDown_LinkClicked(ByVal sender As System.Object, _
                                            ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
                                            Handles lblTemplateDown.LinkClicked
        Dim fileName As String
        Dim folderName As String
        Dim appPath As String
        Dim pathTemplate As String
        Dim saveFileDialog As SaveFileDialog
        Try

            saveFileDialog = New SaveFileDialog()
            appPath = Forms.Application.StartupPath
            folderName = String.Empty
            'While folderName <> "Lixil.AvoidMissSystem.WinUI"
            '    appPath = appPath.Substring(0, appPath.LastIndexOf("\"))
            '    folderName = appPath.Substring(appPath.LastIndexOf("\") + 1)
            'End While

            pathTemplate = appPath & "/Template/Tools.xls"
            saveFileDialog.Filter = "下载模板(*.xls)|*.xls"
            saveFileDialog.FileName = "治具MS模版"
            If saveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If File.Exists(pathTemplate) Then
                    fileName = saveFileDialog.FileName
                    File.Copy(pathTemplate, fileName, True)
                    MessageBox.Show(MsgConst.M00011I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(String.Format(MsgConst.M00010I, "模板"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            saveFileDialog = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' 选择路径点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFilePath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFilePath.Click
        Dim openFileDialog As OpenFileDialog
        Dim strFileType As String
        Try
            openFileDialog = New OpenFileDialog()
            'openFileDialog.Filter = "选择导入文件(*.xls)|*.xls"
            openFileDialog.Filter = "选择导入文件(*.xls)|*.xls|选择导入文件(*.xlsx)|*.xlsx"

            '路径有值且存在的场合，打开该文件路径
            openFileDialog.FileName = Me.txtFilePath.Text
            If Not openFileDialog.CheckFileExists() AndAlso Not openFileDialog.CheckPathExists() Then
                openFileDialog.FileName = String.Empty
            End If

            If openFileDialog.ShowDialog() = Forms.DialogResult.OK Then
                strFileType = Path.GetExtension(openFileDialog.FileName)
                '选择文件不是EXCEL时
                If strFileType <> ".xls" AndAlso strFileType <> ".xlsx" Then
                    MessageBox.Show(MsgConst.M00007I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
                Me.txtFilePath.Text = openFileDialog.FileName
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            openFileDialog = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' 执行按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Try
            If rdoImport.Checked Then
                'EXCEL文件导入处理
                ImportExcel()
            Else
                'EXCEL文件导出处理
                ExportExcel()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    ''' <summary>
    ''' 检索按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            DoSearch()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 清空按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Try
            '窗体初期化处理
            Initialize()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 删除按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            Dim dtGrid As DataTable


            If Me.dgvTools.Rows.Count <= 0 Then
                MessageBox.Show(MsgConst.M00005I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If MessageBox.Show(MsgConst.M00002C, "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Forms.DialogResult.Cancel Then
                Exit Sub
            End If

            dtGrid = CType(Me.dgvTools.DataSource, DataTable)
            '删除处理
            If Not toolsBL.DeleteTools(dtGrid, Me.Login.UserName) Then
                MessageBox.Show(String.Format(MsgConst.M00004D, "治具"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            '日志表插入
            ComBizLogic = New CommonBizLogic
            ComBizLogic.InsertLog("治具MS", OperateKind.DELETE, "", Me.Login.UserId, DateTime.Now)

            MessageBox.Show(MsgConst.M00012I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '删除成功后，再检索
            DoSearch()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 保存按钮点击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim dicTools As New Dictionary(Of String, String)
        Dim sysTime As DateTime
        Dim dtGrid As DataTable
        Dim bizIndex As IndexBizLogic
        Dim strIndexResult As String
        Dim updPara As Dictionary(Of String, String)
        Dim strUpdFPath As String
        Dim endTime As DateTime

        Try
            If Not InputCheck() Then
                Exit Sub
            End If

            If MessageBox.Show(MsgConst.M00001C, "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Forms.DialogResult.Cancel Then
                Exit Sub
            End If
            ComBizLogic = New CommonBizLogic
            sysTime = ComBizLogic.GetSystemDate()
            dicTools.Clear()
            'If Me.dgvTools.Rows.Count = 0 Then
            If updFlg = False Then
                '插入的时候
                bizIndex = New IndexBizLogic
                strIndexResult = bizIndex.GetIndex(TYPEKBN.TOOLS)
                Select Case strIndexResult
                    Case DBErro.InserError
                        MessageBox.Show(String.Format(MsgConst.M00002D, "治具表采番"), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    Case DBErro.UpdateError
                        MessageBox.Show(String.Format(MsgConst.M00003D, "治具表采番"), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    Case DBErro.GetIndexError
                        MessageBox.Show(String.Format(MsgConst.M00001D, "治具表"), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    Case DBErro.GetIndexMaxError
                        MessageBox.Show(String.Format(MsgConst.M00005D, "治具表"), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                End Select

                '插入处理
                dicTools.Add("ID", strIndexResult)
                dicTools.Add("ToolsNo", Me.txtToolsNo.Text.Trim)
                dicTools.Add("DepartmentCd", Me.ddlDepartment.SelectedValue.ToString)
                dicTools.Add("Distinguish", Me.txtDistinguish.Text.Trim)
                dicTools.Add("BarcodeFlg", Me.ddlBarcodeFlg.SelectedValue.ToString)
                dicTools.Add("Barcode", Me.txtBarcode.Text.Trim)
                dicTools.Add("Remarks", Me.txtRemarks.Text.Trim)
                dicTools.Add("User", Me.Login.UserName)
                dicTools.Add("SysTime", sysTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                If Not toolsBL.InsertTools(dicTools) Then
                    MessageBox.Show(String.Format(MsgConst.M00002D, "治具"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Else
                    endTime = DateTime.Now

                    '日志表插入
                    ComBizLogic.InsertLog("治具MS", OperateKind.INSERT, "", Me.Login.UserId, endTime)

                    MessageBox.Show(MsgConst.M00013I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                '更新处理
                updPara = ChangeCheck()
                dtGrid = CType(Me.dgvTools.DataSource, DataTable)

                '更新前后数据保存用Datatable初始化
                InitTable()
                beforeUpdDt = dtGrid

                If Not toolsBL.UpdateTools(dtGrid, Me.Login.UserName, updPara) Then
                    MessageBox.Show(String.Format(MsgConst.M00002D, "治具"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Else
                    endTime = DateTime.Now()
                    '更新成功后，再检索
                    DoSearch()
                    '更新后数据保存
                    afterUpdDt = DirectCast(dgvTools.DataSource, DataTable)
                    '错误数据及更新信息导出
                    strUpdFPath = LogExport.LogExport("治具MS", New DataTable, beforeUpdDt, afterUpdDt, endTime)
                    '日志表插入
                    ComBizLogic.InsertLog("治具MS", OperateKind.UPDATE, strUpdFPath, Me.Login.UserId, endTime)

                    MessageBox.Show(MsgConst.M00016I & " " & Me.dgvTools.Rows.Count & "行更新", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            'MessageBox.Show(MsgConst.M00013I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ''登录成功后，再检索
            'DoSearch()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ComBizLogic = Nothing
        End Try
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


    Dim selectRow As Integer = -1
    ''' <summary>
    ''' GridView单击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvTools_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTools.CellClick

        '双击数据行后，数据自动填充到编辑区域，表示可以修改，但是，当再次单击换行后，希望自动清空编辑区域数据。再双击时再填充。
        If e.RowIndex <> selectRow Then
            Me.txtToolsNo.Text = ""
            Me.ddlDepartment.SelectedValue = ""
            Me.txtDistinguish.Text = ""
            Me.ddlBarcodeFlg.SelectedValue = ""
            Me.txtBarcode.Text = ""
            Me.txtRemarks.Text = ""
            '编辑区背景色恢复初期值
            txtToolsNo.BackColor = Color.White
            ddlDepartment.BackColor = Color.White
            txtDistinguish.BackColor = Color.White
            ddlBarcodeFlg.BackColor = Color.White
            txtBarcode.BackColor = Color.White
            txtRemarks.BackColor = Color.White

            selectRow = e.RowIndex
        End If







        Dim strToolId As String
        Dim dtGoods As DataTable
        Try
            If e.RowIndex >= 0 Then
                strToolId = Convert.ToString(Me.dgvTools.Rows(e.RowIndex).Cells("id").Value)
                dtGoods = toolsBL.GetGoodsCdList(strToolId).Tables("Goods")
                Me.dgvGoods.DataSource = dtGoods
                Me.dgvGoods.ReadOnly = True
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' GridView双击处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvTools_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTools.CellDoubleClick
        Try
            If e.RowIndex >= 0 Then
                With Me.dgvTools.Rows(e.RowIndex)

                    strToolsNo = Convert.ToString(.Cells("tools_no").Value)
                    strDepartCd = Convert.ToString(.Cells("department_cd").Value)
                    strDistinguish = Convert.ToString(.Cells("distinguish").Value)
                    strBarcodeFlg = Convert.ToString(.Cells("barcode_flg").Value)
                    strBarcode = Convert.ToString(.Cells("barcode").Value)
                    strRemarks = Convert.ToString(.Cells("remarks").Value)

                    Me.txtToolsNo.Text = Convert.ToString(.Cells("tools_no").Value)
                    Me.ddlDepartment.SelectedValue = Convert.ToString(.Cells("department_cd").Value)
                    Me.txtDistinguish.Text = Convert.ToString(.Cells("distinguish").Value)
                    Me.ddlBarcodeFlg.SelectedValue = Convert.ToString(.Cells("barcode_flg").Value)
                    Me.txtBarcode.Text = Convert.ToString(.Cells("barcode").Value)
                    Me.txtRemarks.Text = Convert.ToString(.Cells("remarks").Value)

                End With
            End If

            '删除按钮可用
            Me.btnDelete.Enabled = True

            '清空按钮不可用
            Me.btnClear.Enabled = False

            '更新Flg设为true
            updFlg = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '''' <summary>
    '''' 治具编号获得焦点事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub txtToolsNo_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchToolsNo.GotFocus, txtToolsNo.GotFocus
    '    Try
    '        strToolsNo = Me.txtToolsNo.Text
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 治具编号编辑事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub txtToolsNo_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtToolsNo.Validated
    '    Try
    '        If strToolsNo = Me.txtToolsNo.Text Then
    '            Exit Sub
    '        End If

    '        If Me.dgvTools.SelectedCells.Count > 0 Then
    '            For i As Integer = 0 To Me.dgvTools.Rows.Count - 1
    '                Me.dgvTools.Rows(i).Cells("tools_no").Value = Me.txtToolsNo.Text.Trim
    '                Me.dgvTools.Rows(i).Cells("tools_no").Style.ForeColor = Color.Red
    '            Next
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 部门获得焦点事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub ddlDepartment_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlDepartment.GotFocus
    '    Try
    '        strDepartCd = Me.ddlDepartment.SelectedValue.ToString
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 部门编辑事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub ddlDepartment_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlDepartment.Validated
    '    Try
    '        If strDepartCd = Me.ddlDepartment.SelectedValue.ToString Then
    '            Exit Sub
    '        End If
    '        If Not String.IsNullOrEmpty(Me.ddlDepartment.SelectedValue.ToString) AndAlso _
    '            Me.dgvTools.SelectedCells.Count > 0 Then
    '            For i As Integer = 0 To Me.dgvTools.Rows.Count - 1
    '                Me.dgvTools.Rows(i).Cells("department_cd").Value = Me.ddlDepartment.SelectedValue.ToString
    '                Me.dgvTools.Rows(i).Cells("department").Value = Me.ddlDepartment.Text
    '                Me.dgvTools.Rows(i).Cells("department").Style.ForeColor = Color.Red
    '            Next
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 区分获得焦点事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub txtDistinguish_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDistinguish.GotFocus
    '    Try
    '        strDistinguish = Me.txtDistinguish.Text
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 区分编辑事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub txtDistinguish_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDistinguish.Validated
    '    Try
    '        If strDistinguish = Me.txtDistinguish.Text Then
    '            Exit Sub
    '        End If

    '        If Me.dgvTools.SelectedCells.Count > 0 Then
    '            For i As Integer = 0 To Me.dgvTools.Rows.Count - 1
    '                Me.dgvTools.Rows(i).Cells("distinguish").Value = Me.txtDistinguish.Text.Trim
    '                Me.dgvTools.Rows(i).Cells("distinguish").Style.ForeColor = Color.Red
    '            Next
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 基准值获得焦点事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub ddlBarcodeFlg_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBarcodeFlg.GotFocus
    '    Try
    '        strBarcodeFlg = Me.ddlBarcodeFlg.SelectedValue.ToString
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 基准值编辑事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub ddlBarcodeFlg_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBarcodeFlg.Validated
    '    Try
    '        If strBarcodeFlg = Me.ddlBarcodeFlg.SelectedValue.ToString Then
    '            Exit Sub
    '        End If

    '        If Not String.IsNullOrEmpty(Me.ddlBarcodeFlg.SelectedValue.ToString) AndAlso _
    '            Me.dgvTools.SelectedCells.Count > 0 Then
    '            For i As Integer = 0 To Me.dgvTools.Rows.Count - 1
    '                Me.dgvTools.Rows(i).Cells("barcode_flg").Value = Me.ddlBarcodeFlg.SelectedValue.ToString
    '                Me.dgvTools.Rows(i).Cells("barcode_flg_name").Value = Me.ddlBarcodeFlg.Text
    '                Me.dgvTools.Rows(i).Cells("barcode_flg_name").Style.ForeColor = Color.Red
    '            Next
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 条形码获得焦点事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub txtBarcode_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarcode.GotFocus
    '    Try
    '        strBarcode = Me.txtBarcode.Text
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 条形码编辑事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub txtBarcode_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarcode.Validated
    '    Try
    '        If strBarcode = Me.txtBarcode.Text Then
    '            Exit Sub
    '        End If

    '        If Me.dgvTools.SelectedCells.Count > 0 Then
    '            For i As Integer = 0 To Me.dgvTools.Rows.Count - 1
    '                Me.dgvTools.Rows(i).Cells("barcode").Value = Me.txtBarcode.Text.Trim
    '                Me.dgvTools.Rows(i).Cells("barcode").Style.ForeColor = Color.Red
    '            Next
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 备注获得焦点事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub txtRemarks_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRemarks.GotFocus
    '    Try
    '        strRemarks = Me.txtRemarks.Text
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 备注编辑事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub txtRemarks_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRemarks.Validated
    '    Try
    '        If strRemarks = Me.txtRemarks.Text Then
    '            Exit Sub
    '        End If

    '        If Me.dgvTools.SelectedCells.Count > 0 Then
    '            For i As Integer = 0 To Me.dgvTools.Rows.Count - 1
    '                Me.dgvTools.Rows(i).Cells("remarks").Value = Me.txtRemarks.Text.Trim
    '                Me.dgvTools.Rows(i).Cells("remarks").Style.ForeColor = Color.Red
    '            Next
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub
#End Region

#Region "方法"

    ''' <summary>
    ''' 数据生成
    ''' </summary>
    ''' <param name="strGridKbn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenerateHead(ByVal strGridKbn As String) As DataTable
        Try
            Dim arrHeader() As String = {"id", "tools_no", "department_cd", "department", "distinguish", "barcode_flg", "barcode_flg_name", "barcode", "remarks"}
            Dim dtHeader As New DataTable
            Dim dc As DataColumn
            If strGridKbn = "Tools" Then
                For i As Integer = 0 To arrHeader.Length - 1
                    dc = New DataColumn(arrHeader(i), GetType(String))
                    dtHeader.Columns.Add(dc)
                Next
            Else
                dc = New DataColumn("goods_cd", GetType(String))
                dtHeader.Columns.Add(dc)
            End If

            Return dtHeader
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 执行检索
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DoSearch()
        Dim strDepartmentCd As String
        Dim strSelectDepartCd As String
        Dim dtTools As DataTable
        Dim dtSelect As New DataTable
        Dim dicSearch As New Dictionary(Of String, String)
        Dim chkDepart As CheckBox
        Try

            strToolsNo = String.Empty
            strDepartCd = String.Empty
            strDistinguish = String.Empty
            strBarcodeFlg = String.Empty
            strBarcode = String.Empty
            strRemarks = String.Empty

            strDepartmentCd = String.Empty
            strSelectDepartCd = String.Empty
            For Each chkObject As Control In Me.gbSearch.Controls
                If TypeOf chkObject Is CheckBox Then
                    chkDepart = CType(chkObject, CheckBox)
                    If chkDepart.Checked Then
                        '选中部门CD
                        If String.IsNullOrEmpty(strSelectDepartCd) Then
                            strSelectDepartCd = chkObject.Name
                        Else
                            strSelectDepartCd = strSelectDepartCd & "," & chkObject.Name
                        End If
                    Else
                        '未选中部门CD
                        If String.IsNullOrEmpty(strDepartmentCd) Then
                            strDepartmentCd = chkObject.Name
                        Else
                            strDepartmentCd = strDepartmentCd & "," & chkObject.Name
                        End If
                    End If
                End If
            Next
            dicSearch.Add("ID", Me.txtSearchID.Text.Trim)
            dicSearch.Add("GoodsCD", Me.txtSearchGoodsCD.Text.Trim)
            dicSearch.Add("ToolsNo", Me.txtSearchToolsNo.Text.Trim)
            If String.IsNullOrEmpty(strSelectDepartCd) Then
                dicSearch.Add("DepartmentCD", strDepartmentCd)
            Else
                dicSearch.Add("DepartmentCD", strSelectDepartCd)
            End If
            dicSearch.Add("Distinguish", Me.txtSearchDistinguish.Text.Trim)
            dicSearch.Add("BarcodeFlg", Convert.ToString(Me.ddlSearchBarcodeFlg.SelectedValue))
            dicSearch.Add("Barcode", Me.txtSearchBarcode.Text.Trim)
            dicSearch.Add("Remarks", Me.txtSearchRemarks.Text.Trim)

            dtTools = toolsBL.GetTools(dicSearch).Tables("mtools")
            Me.dgvTools.DataSource = dtTools
            Me.dgvGoods.DataSource = GenerateHead("Goods")
            If dtTools.Rows.Count = 0 Then
                MessageBox.Show(MsgConst.M00005I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Me.lblRowCount.Text = dtTools.Rows.Count.ToString & " 行"
            '编辑项目清空
            Me.ddlDepartment.SelectedIndex = 0
            Me.txtToolsNo.Text = String.Empty
            Me.txtDistinguish.Text = String.Empty
            Me.ddlBarcodeFlg.SelectedIndex = 0
            Me.txtBarcode.Text = String.Empty
            Me.txtRemarks.Text = String.Empty

            '删除按钮不可用
            Me.btnDelete.Enabled = False
            '清空按钮可用
            Me.btnClear.Enabled = True

            '更新Flg设为false
            updFlg = False

            '编辑区背景色恢复初期值
            txtToolsNo.BackColor = Color.White
            ddlDepartment.BackColor = Color.White
            txtDistinguish.BackColor = Color.White
            ddlBarcodeFlg.BackColor = Color.White
            txtBarcode.BackColor = Color.White
            txtRemarks.BackColor = Color.White

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' EXCEL文件导入处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ImportExcel()
        Dim strFilePath As String
        Dim strFileType As String
        Dim dtExcel As DataTable
        Dim dtImport As DataTable
        Dim drImport As DataRow
        Dim dtError As DataTable
        Dim drError As DataRow
        Dim dcMsg As DataColumn
        Dim result As Result
        Dim frmErrorData As ErrorDataForm
        Dim strDicHeader As String = String.Empty
        Dim strExcelHeader As String = String.Empty
        'Dim strSheetNames As String()
        Dim strExportFPath As String
        'Dim startTime As DateTime
        Dim endTime As DateTime
        'Dim spTime As TimeSpan
        Try
            strFilePath = Me.txtFilePath.Text.Trim
            If String.IsNullOrEmpty(strFilePath) Then
                MessageBox.Show(MsgConst.M00006I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                strFileType = System.IO.Path.GetExtension(strFilePath)
                If strFileType <> ".xls" AndAlso strFileType <> ".xlsx" Then
                    MessageBox.Show(MsgConst.M00007I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            End If
           
            If File.Exists(strFilePath) Then
                ''[治具信息]sheet存在检查
                'strSheetNames = LogExport.GetAllSheetName(strFilePath)
                'For i As Integer = 0 To strSheetNames.Length - 1
                '    If strSheetNames(i) = "治具信息$" Then
                '        Exit For
                '    End If
                '    If i = strSheetNames.Length - 1 Then
                '        MessageBox.Show("EXCEL文件中不存在[治具信息]Sheet页！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                '        Exit Sub
                '    End If
                'Next

                '读取EXCEL数据
                dtExcel = GetExcelData("治具信息", strFilePath)

                If dtExcel.Rows.Count < 1 Then
                    MessageBox.Show(MsgConst.M00008I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

                dtImport = dtExcel.Clone()
                dtError = dtExcel.Clone()
                dcMsg = New DataColumn("错误信息", GetType(String))
                dtError.Columns.Add(dcMsg)
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

                '开始时间
                'startTime = DateTime.Now
                Me.Cursor = Cursors.WaitCursor

                '更新前后数据保存用Datatable初始化
                InitTable()

                For Each drExcel As DataRow In dtExcel.Rows
                    'EXCEL数据校验
                    result = ImportCheck(drExcel)
                    If result.Success Then
                        drImport = dtImport.NewRow()
                        drImport("ID") = drExcel("ID")
                        drImport("治具编号") = drExcel("治具编号")
                        drImport("部门CD") = drExcel("部门CD")
                        drImport("治具区分") = drExcel("治具区分")
                        drImport("基准值") = drExcel("基准值")
                        drImport("条形码") = drExcel("条形码")
                        drImport("备注") = drExcel("备注")
                        dtImport.Rows.Add(drImport)
                    Else
                        drError = dtError.NewRow()
                        drError("ID") = drExcel("ID")
                        drError("治具编号") = drExcel("治具编号")
                        drError("部门CD") = drExcel("部门CD")
                        drError("治具区分") = drExcel("治具区分")
                        drError("基准值") = drExcel("基准值")
                        drError("条形码") = drExcel("条形码")
                        drError("备注") = drExcel("备注")
                        drError("错误信息") = result.Message
                        dtError.Rows.Add(drError)
                    End If
                Next

                'EXCEL数据导入
                If toolsBL.InsOrUpdTools(dtImport, Me.Login.UserName, afterUpdDt) Then
                    '结束时间
                    endTime = DateTime.Now
                    '错误数据及更新信息导出
                    strExportFPath = LogExport.LogExport("治具MS", dtError, beforeUpdDt, afterUpdDt, endTime)
                    '日志表插入
                    ComBizLogic = New CommonBizLogic
                    ComBizLogic.InsertLog("治具MS", OperateKind.IMPORT, strExportFPath, Me.Login.UserId, endTime)

                    MessageBox.Show(String.Format(MsgConst.M00015I, dtExcel.Rows.Count.ToString, dtImport.Rows.Count.ToString, dtError.Rows.Count.ToString), _
                                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If dtError.Rows.Count > 0 AndAlso MessageBox.Show(MsgConst.M00003C, "確認", _
                       MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Forms.DialogResult.OK Then
                        frmErrorData = New ErrorDataForm()
                        frmErrorData.ErrorData = dtError
                        frmErrorData.Show()
                    End If
                    '导入成功后，再检索
                    DoSearch()
                Else
                    If String.IsNullOrEmpty(toolsBL.strDBError) Then
                        MessageBox.Show(MsgConst.M00014I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        Select Case toolsBL.strDBError
                            Case DBErro.InserError
                                MessageBox.Show(String.Format(MsgConst.M00002D, "治具表采番"), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Case DBErro.UpdateError
                                MessageBox.Show(String.Format(MsgConst.M00003D, "治具表采番"), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Case DBErro.GetIndexError
                                MessageBox.Show(String.Format(MsgConst.M00001D, "治具表"), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Case DBErro.GetIndexMaxError
                                MessageBox.Show(String.Format(MsgConst.M00005D, "治具表"), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End Select
                    End If
                    Me.Cursor = Cursors.Arrow
                    Exit Sub
                End If
            Else
                MessageBox.Show(String.Format(MsgConst.M00010I, "批量导入"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Cursor = Cursors.Arrow
                Exit Sub
            End If
            '结束时间
            'endTime = DateTime.Now
            'spTime = endTime - startTime
            'MessageBox.Show("批量导入数据" & dtExcel.Rows.Count.ToString & "件!耗时" & spTime.TotalSeconds.ToString & "秒", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Throw ex
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub

    ''' <summary>
    ''' 'EXCEL文件导出处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExportExcel()
        Dim strDepartmentCd As String
        Dim strSelectDepartCd As String
        Dim chkDepart As CheckBox
        Dim dicSearch As Dictionary(Of String, String)
        Dim dtTools As DataTable
        Dim rowCount As Integer
        Dim colCount As Integer
        Dim strFileName As String
        Dim saveFileDialog As SaveFileDialog
        Dim xlBook = Nothing
        Dim xlSheet = Nothing
        Dim xlApp = Nothing
        Dim arrData(0, 0) As String
        'Dim startTime As DateTime
        'Dim endTime As DateTime
        'Dim spTime As TimeSpan
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
            '开始时间
            'startTime = DateTime.Now
            Me.Cursor = Cursors.WaitCursor
            strDepartmentCd = String.Empty
            strSelectDepartCd = String.Empty
            For Each chkObject As Control In Me.gbSearch.Controls
                If TypeOf chkObject Is CheckBox Then
                    chkDepart = CType(chkObject, CheckBox)
                    If chkDepart.Checked Then
                        '选中部门CD
                        If String.IsNullOrEmpty(strSelectDepartCd) Then
                            strSelectDepartCd = chkObject.Name
                        Else
                            strSelectDepartCd = strSelectDepartCd & "," & chkObject.Name
                        End If
                    Else
                        '未选中部门CD
                        If String.IsNullOrEmpty(strDepartmentCd) Then
                            strDepartmentCd = chkObject.Name
                        Else
                            strDepartmentCd = strDepartmentCd & "," & chkObject.Name
                        End If
                    End If
                End If
            Next

            '根据检索条件，取得导出数据
            dicSearch = New Dictionary(Of String, String)
            dicSearch.Add("ID", Me.txtSearchID.Text.Trim)
            dicSearch.Add("GoodsCD", Me.txtSearchGoodsCD.Text.Trim)
            dicSearch.Add("ToolsNo", Me.txtSearchToolsNo.Text.Trim)
            If String.IsNullOrEmpty(strSelectDepartCd) Then
                dicSearch.Add("DepartmentCD", strDepartmentCd)
            Else
                dicSearch.Add("DepartmentCD", strSelectDepartCd)
            End If
            dicSearch.Add("Distinguish", Me.txtSearchDistinguish.Text.Trim)
            dicSearch.Add("BarcodeFlg", Me.ddlSearchBarcodeFlg.SelectedValue.ToString)
            dicSearch.Add("Barcode", Me.txtSearchBarcode.Text.Trim)
            dicSearch.Add("Remarks", Me.txtSearchRemarks.Text.Trim)

            dtTools = toolsBL.GetTools(dicSearch).Tables("mtools")
            If dtTools.Rows.Count < 1 Then
                MessageBox.Show(MsgConst.M00005I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            rowCount = dtTools.Rows.Count
            colCount = dtTools.Columns.Count
            ReDim arrData(rowCount, colCount)

            xlApp = CreateObject("Excel.Application")
            xlApp.Visible = False
            xlBook = xlApp.Workbooks.Add
            For i As Integer = 0 To rowCount
                If i = 0 Then
                    arrData(0, 0) = dicExcelHeader("id")
                    arrData(0, 1) = dicExcelHeader("tools_no")
                    arrData(0, 2) = dicExcelHeader("department_cd")
                    arrData(0, 3) = dicExcelHeader("distinguish")
                    arrData(0, 4) = dicExcelHeader("barcode_flg")
                    arrData(0, 5) = dicExcelHeader("barcode")
                    arrData(0, 6) = dicExcelHeader("remarks")
                Else
                    With dtTools.Rows(i - 1)
                        arrData(i, 0) = Convert.ToString(.Item("id"))
                        arrData(i, 1) = Convert.ToString(.Item("tools_no"))
                        arrData(i, 2) = Convert.ToString(.Item("department_cd"))
                        arrData(i, 3) = Convert.ToString(.Item("distinguish"))
                        arrData(i, 4) = Convert.ToString(.Item("barcode_flg"))
                        arrData(i, 5) = Convert.ToString(.Item("barcode"))
                        arrData(i, 6) = Convert.ToString(.Item("remarks"))
                    End With
                End If
            Next

            '导出数据设定
            xlSheet = xlBook.Worksheets.Add()
            xlSheet.Name = "治具信息"
            For Each wkSheet In xlBook.Worksheets
                If wkSheet.Name <> "治具信息" Then
                    wkSheet.Delete()
                End If
            Next
            xlSheet.Activate()
            xlSheet.Range("A1").Resize(rowCount + 1, colCount).Value = arrData
            xlBook.SaveAs(strFileName)
            xlBook.Close()
            xlApp.Quit()
            '结束时间
            'endTime = DateTime.Now
            'spTime = endTime - startTime
            'MessageBox.Show("批量导出数据" & rowCount.ToString & "件!耗时" & spTime.TotalSeconds.ToString & "秒", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Cursor = Cursors.Arrow
            MessageBox.Show(MsgConst.M00004I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Throw ex
        Finally
            Me.Cursor = Cursors.Arrow
            If Not xlSheet Is Nothing Then
                ReleaseComObject(xlSheet)
            End If
            If Not xlBook Is Nothing Then
                ReleaseComObject(xlBook)
            End If
            If Not xlApp Is Nothing Then
                ReleaseComObject(xlApp)
            End If
            saveFileDialog = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' 窗体初期化处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Initialize()
        Dim chkDepart As CheckBox
        Dim bsBarcodeFlg As BindingSource
        Dim bsSrarchBarcodeFlg As BindingSource
        Try
            Me.Text = "治具MS维护"
            Me.txtSearchID.Text = String.Empty
            Me.txtSearchGoodsCD.Text = String.Empty
            Me.txtSearchToolsNo.Text = String.Empty
            Me.txtSearchDistinguish.Text = String.Empty
            Me.txtSearchBarcode.Text = String.Empty
            Me.txtSearchRemarks.Text = String.Empty
            Me.txtToolsNo.Text = String.Empty
            Me.txtDistinguish.Text = String.Empty
            Me.txtBarcode.Text = String.Empty
            Me.txtRemarks.Text = String.Empty
            Me.txtFilePath.Text = String.Empty
            Me.lblRowCount.Text = String.Empty

            Me.rdoImport.Checked = True
            Me.rdoExport.Checked = False
            Me.dgvTools.DataSource = GenerateHead("Tools")
            Me.dgvTools.ReadOnly = True
            Me.dgvTools.RowHeadersVisible = False

            Me.dgvGoods.DataSource = GenerateHead("Goods")
            Me.dgvGoods.ReadOnly = True
            Me.dgvGoods.RowHeadersVisible = False

            If isFormLoad Then
                If Not InitializeDepart() Then Exit Sub
            Else
                '部门选择框
                For Each chkObject As Control In Me.gbSearch.Controls
                    If TypeOf chkObject Is CheckBox Then
                        chkDepart = CType(chkObject, CheckBox)
                        chkDepart.Checked = False
                    End If
                Next
            End If
            Me.ddlDepartment.SelectedIndex = 0
            InitializeGridView()

            bsBarcodeFlg = New BindingSource()
            bsBarcodeFlg.DataSource = Me.dicBarcodeFlg

            Me.ddlBarcodeFlg.DataSource = bsBarcodeFlg
            Me.ddlBarcodeFlg.DisplayMember = "Value"
            Me.ddlBarcodeFlg.ValueMember = "Key"

            bsSrarchBarcodeFlg = New BindingSource()
            bsSrarchBarcodeFlg.DataSource = Me.dicSearchBarcodeFlg
            Me.ddlSearchBarcodeFlg.DataSource = bsSrarchBarcodeFlg

            Me.ddlSearchBarcodeFlg.DisplayMember = "Value"
            Me.ddlSearchBarcodeFlg.ValueMember = "Key"
            Me.txtSearchID.Focus()
            isFormLoad = False

            '删除按钮不可用
            Me.btnDelete.Enabled = False
            '清空按钮可用
            Me.btnClear.Enabled = True

            strToolsNo = String.Empty
            strDepartCd = String.Empty
            strDistinguish = String.Empty
            strBarcodeFlg = String.Empty
            strBarcode = String.Empty
            strRemarks = String.Empty

            '更新Flg设为false
            updFlg = False

            '编辑区背景色恢复初期值
            txtToolsNo.BackColor = Color.White
            ddlDepartment.BackColor = Color.White
            txtDistinguish.BackColor = Color.White
            ddlBarcodeFlg.BackColor = Color.White
            txtBarcode.BackColor = Color.White
            txtRemarks.BackColor = Color.White

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' 部门初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Function InitializeDepart() As Boolean
        InitializeDepart = True
        Dim dtDepart As DataTable
        'Dim drDepart As DataRow
        Dim chkObject As CheckBox
        Try
            dtDepart = toolsBL.GetDepartment(Me.Login.UserId, Me.Login.IsAdmin).Tables("Department")

            If dtDepart.Rows.Count = 0 Then
                Try
                    MessageBox.Show("部门数据不存在", "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    NavigateToNextPage(Consts.PAGE.MS_MAIN)
                    InitializeDepart = False
                    Exit Function
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
            '部门选择框初期化
            For i As Integer = 0 To dtDepart.Rows.Count - 1
                chkObject = New CheckBox
                chkObject.Name = Convert.ToString(dtDepart.Rows(i).Item("mei_cd"))
                chkObject.Text = Convert.ToString(dtDepart.Rows(i).Item("mei"))
                chkObject.Location = New System.Drawing.Point(88 + i * 54, 10)
                chkObject.Visible = True
                chkObject.Checked = False
                chkObject.Width = 50
                chkObject.TabIndex = i + 1
                Me.gbSearch.Controls.Add(chkObject)
            Next

            '部门下拉列表初期化
            'drDepart = dtDepart.NewRow()
            'drDepart("mei_cd") = ""
            'drDepart("mei") = ""
            'dtDepart.Rows.InsertAt(drDepart, 0)
            Me.ddlDepartment.DataSource = dtDepart
            Me.ddlDepartment.ValueMember = Convert.ToString(dtDepart.Columns("mei_cd"))
            Me.ddlDepartment.DisplayMember = Convert.ToString(dtDepart.Columns("mei"))
            ddlDepartment.SelectedIndex = 0

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' GridView初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeGridView()
        Try
            Me.dgvTools.Columns("department").HeaderText = dicGridHeader("department")
            Me.dgvTools.Columns("id").HeaderText = dicGridHeader("id")
            Me.dgvTools.Columns("tools_no").HeaderText = dicGridHeader("tools_no")
            Me.dgvTools.Columns("distinguish").HeaderText = dicGridHeader("distinguish")
            Me.dgvTools.Columns("barcode").HeaderText = dicGridHeader("barcode").ToString()
            Me.dgvTools.Columns("barcode_flg_name").HeaderText = dicGridHeader("barcode_flg")
            Me.dgvTools.Columns("remarks").HeaderText = dicGridHeader("remarks")

            Me.dgvTools.Columns("department").Width = 50
            Me.dgvTools.Columns("id").Width = 55
            Me.dgvTools.Columns("tools_no").Width = 100
            Me.dgvTools.Columns("distinguish").Width = 70
            Me.dgvTools.Columns("barcode").Width = 110
            Me.dgvTools.Columns("barcode_flg_name").Width = 60
            Me.dgvTools.Columns("remarks").Width = 200

            Me.dgvTools.Columns("id").SortMode = DataGridViewColumnSortMode.NotSortable
            Me.dgvTools.Columns("tools_no").SortMode = DataGridViewColumnSortMode.NotSortable
            Me.dgvTools.Columns("department").SortMode = DataGridViewColumnSortMode.NotSortable
            Me.dgvTools.Columns("distinguish").SortMode = DataGridViewColumnSortMode.NotSortable
            Me.dgvTools.Columns("barcode").SortMode = DataGridViewColumnSortMode.NotSortable
            Me.dgvTools.Columns("barcode_flg_name").SortMode = DataGridViewColumnSortMode.NotSortable
            Me.dgvTools.Columns("remarks").SortMode = DataGridViewColumnSortMode.NotSortable

            Me.dgvTools.Columns("tools_no").DefaultCellStyle.ForeColor = Color.Black
            Me.dgvTools.Columns("department").DefaultCellStyle.ForeColor = Color.Black
            Me.dgvTools.Columns("distinguish").DefaultCellStyle.ForeColor = Color.Black
            Me.dgvTools.Columns("barcode").DefaultCellStyle.ForeColor = Color.Black
            Me.dgvTools.Columns("barcode_flg_name").DefaultCellStyle.ForeColor = Color.Black
            Me.dgvTools.Columns("remarks").DefaultCellStyle.ForeColor = Color.Black

            Me.dgvTools.Columns("department_cd").Visible = False
            Me.dgvTools.Columns("barcode_flg").Visible = False

            Me.dgvGoods.Columns("goods_cd").Width = 140
            Me.dgvGoods.Columns("goods_cd").HeaderText = "商品CD"
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' 输入检查
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InputCheck() As Boolean
        Try

            If Me.dgvTools.Rows.Count = 0 Then
                '部门输入校验
                Dim strDepartmentCD As String = Me.ddlDepartment.SelectedValue.ToString
                If String.IsNullOrEmpty(strDepartmentCD) Then
                    MessageBox.Show(String.Format(MsgConst.M00002I, "部门"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.ddlDepartment.Focus()
                    Return False
                End If

                '治具编号校验
                Dim strToolsNo As String = Me.txtToolsNo.Text.Trim
                If String.IsNullOrEmpty(strToolsNo) Then
                    MessageBox.Show(String.Format(MsgConst.M00002I, "治具编号"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.txtToolsNo.Focus()
                    Return False
                End If
            Else
                '治具编号有变更的时候
                If strToolsNo <> Me.txtToolsNo.Text Then
                    If String.IsNullOrEmpty(Me.txtToolsNo.Text) Then
                        MessageBox.Show(String.Format(MsgConst.M00002I, "治具编号"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Me.txtToolsNo.Focus()
                        Return False
                    End If
                End If
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 批量导入数据检查
    ''' </summary>
    ''' <param name="drExcel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ImportCheck(ByVal drExcel As DataRow) As Result
        Try
            Dim strID As String
            Dim strToolsNo As String
            Dim strDistinguish As String
            Dim strBarcodeFlg As String
            Dim strBarcode As String
            Dim strRemarks As String
            Dim result As New Result

            strID = Convert.ToString(drExcel("ID"))
            strToolsNo = Convert.ToString(drExcel("治具编号"))
            strDistinguish = Convert.ToString(drExcel("治具区分"))
            strBarcodeFlg = Convert.ToString(drExcel("基准值"))
            strBarcode = Convert.ToString(drExcel("条形码"))
            strRemarks = Convert.ToString(drExcel("备注"))

            '治具编号输入校验
            If String.IsNullOrEmpty(strToolsNo) Then
                result.Message = String.Format(MsgConst.M00002I, "治具编号")
                result.Success = False
                Return result
            End If

            '部门输入校验
            If String.IsNullOrEmpty(Convert.ToString(drExcel("部门CD"))) Then
                result.Message = String.Format(MsgConst.M00002I, "部门CD")
                result.Success = False
                Return result
            End If

            '治具编号长度校验
            If strToolsNo.Length > 30 Then
                result.Message = String.Format(MsgConst.M00003I, "治具编号")
                result.Success = False
                Return result
            End If

            '治具区分长度校验
            If strDistinguish.Length > 100 Then
                result.Message = String.Format(MsgConst.M00003I, "治具区分")
                result.Success = False
                Return result
            End If

            '是否有条码校验
            If Not String.IsNullOrEmpty(strBarcodeFlg) Then
                If strBarcodeFlg <> "0" AndAlso strBarcodeFlg <> "1" Then
                    result.Message = String.Format(MsgConst.M00041I, "是否有条码")
                    result.Success = False
                    Return result
                End If
            End If

            '条形码长度校验
            If strBarcode.Length > 30 Then
                result.Message = String.Format(MsgConst.M00003I, "条形码")
                result.Success = False
                Return result
            End If

            '备注长度校验
            If strRemarks.Length > 200 Then
                result.Message = String.Format(MsgConst.M00003I, "备注")
                result.Success = False
                Return result
            End If

            '治具ID存在校验
            If Not String.IsNullOrEmpty(strID) Then
                Dim dtTool As New DataTable
                dtTool = toolsBL.ExistsToolsID(strID)
                If dtTool.Rows.Count = 0 Then
                    result.Message = String.Format(MsgConst.M00018I, "ID在治具表中")
                    result.Success = False
                    Return result
                Else
                    '更新的时候，更新前的数据保存到Datatable中
                    beforeUpdDt.Rows.Add(dtTool.Rows(0).ItemArray)
                End If
            End If

            result.Success = True
            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 读取EXCEL指定页中的内容
    ''' </summary>
    ''' <param name="strSheetName">页名</param>
    ''' <param name="strExcelFile">EXCEL路径</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetExcelData(ByVal strSheetName As String, _
                                  ByVal strExcelFile As String) As DataTable

        Dim strConn As String = LogExport.GetOleDbConn(strExcelFile)
        Dim strExcel As String
        Dim ds As DataSet
        Dim adapter As OleDb.OleDbDataAdapter = Nothing
        Dim conn As OleDb.OleDbConnection = Nothing

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

        If strToolsNo <> Me.txtToolsNo.Text Then
            dicChange.Add(TOOLSNO, Me.txtToolsNo.Text.Trim)
        End If

        If strDepartCd <> Me.ddlDepartment.SelectedValue.ToString Then
            dicChange.Add(DEPARTMENT, Me.ddlDepartment.SelectedValue.ToString)
        End If

        If strDistinguish <> Me.txtDistinguish.Text Then
            dicChange.Add(DISTINGUISH, Me.txtDistinguish.Text.Trim)
        End If

        If strBarcodeFlg <> Me.ddlBarcodeFlg.SelectedValue.ToString Then
            dicChange.Add(BARCODEFLG, Me.ddlBarcodeFlg.SelectedValue.ToString)
        End If

        If strBarcode <> Me.txtBarcode.Text Then
            dicChange.Add(BARCODE, Me.txtBarcode.Text.Trim)
        End If

        If strRemarks <> Me.txtRemarks.Text Then
            dicChange.Add(REMARKS, Me.txtRemarks.Text.Trim)
        End If

        Return dicChange
    End Function

    ''' <summary>
    ''' Log文件出力用Datatable初始化
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitTable()
        '更新前数据保存用Datatable初始化
        beforeUpdDt = New DataTable
        beforeUpdDt.TableName = "BeforeUpdate"
        beforeUpdDt.Columns.Add("ID", GetType(String))
        beforeUpdDt.Columns.Add("治具编号", GetType(String))
        beforeUpdDt.Columns.Add("部门CD", GetType(String))
        beforeUpdDt.Columns.Add("治具区分", GetType(String))
        beforeUpdDt.Columns.Add("基准值", GetType(String))
        beforeUpdDt.Columns.Add("条形码", GetType(String))
        beforeUpdDt.Columns.Add("备注", GetType(String))

        afterUpdDt = New DataTable
        afterUpdDt.TableName = "AfterUpdate"
        afterUpdDt.Columns.Add("ID", GetType(String))
        afterUpdDt.Columns.Add("治具编号", GetType(String))
        afterUpdDt.Columns.Add("部门CD", GetType(String))
        afterUpdDt.Columns.Add("治具区分", GetType(String))
        afterUpdDt.Columns.Add("基准值", GetType(String))
        afterUpdDt.Columns.Add("条形码", GetType(String))
        afterUpdDt.Columns.Add("备注", GetType(String))
    End Sub

#End Region

#Region "背景色变化处理"
    ''' <summary>
    ''' 治具编号变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtToolsNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToolsNo.TextChanged
        If updFlg = True AndAlso Me.txtToolsNo.Text <> strToolsNo Then
            txtToolsNo.BackColor = Color.LightPink
        Else
            txtToolsNo.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 部门变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDepartment.SelectedIndexChanged
        If Me.ddlDepartment.SelectedValue Is Nothing Then
            ddlDepartment.BackColor = Color.White
            Exit Sub
        End If

        If updFlg = True AndAlso Me.ddlDepartment.SelectedValue IsNot Nothing AndAlso Me.ddlDepartment.SelectedValue.ToString <> strDepartCd Then
            ddlDepartment.BackColor = Color.LightPink
        Else
            ddlDepartment.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 治具区分变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtDistinguish_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDistinguish.TextChanged
        If updFlg = True AndAlso Me.txtDistinguish.Text <> strDistinguish Then
            txtDistinguish.BackColor = Color.LightPink
        Else
            txtDistinguish.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 基准值变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlBarcodeFlg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBarcodeFlg.SelectedIndexChanged
        If Me.ddlBarcodeFlg.SelectedValue Is Nothing Then
            ddlBarcodeFlg.BackColor = Color.White
            Exit Sub
        End If

        If updFlg = True AndAlso Me.ddlBarcodeFlg.SelectedValue IsNot Nothing AndAlso Me.ddlBarcodeFlg.SelectedValue.ToString <> strBarcodeFlg Then
            ddlBarcodeFlg.BackColor = Color.LightPink
        Else
            ddlBarcodeFlg.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 条形码变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtBarcode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBarcode.TextChanged
        If updFlg = True AndAlso Me.txtBarcode.Text <> strBarcode Then
            txtBarcode.BackColor = Color.LightPink
        Else
            txtBarcode.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 备注变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtRemarks_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRemarks.TextChanged
        If updFlg = True AndAlso Me.txtRemarks.Text <> strRemarks Then
            txtRemarks.BackColor = Color.LightPink
        Else
            txtRemarks.BackColor = Color.White
        End If
    End Sub

#End Region

#Region "类"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Result

        Private strMessage As String
        Private isSuccess As Boolean

        Public Property Message() As String
            Get
                Return strMessage
            End Get
            Set(ByVal value As String)
                strMessage = value
            End Set
        End Property

        Public Property Success() As Boolean
            Get
                Return isSuccess
            End Get
            Set(ByVal value As Boolean)
                isSuccess = value
            End Set
        End Property
    End Class
#End Region

End Class