Imports System.Data.SqlClient
'Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices.Marshal
Imports System.IO
Imports System.Windows
Imports Lixil.AvoidMissSystem.BizLogic
Imports Lixil.AvoidMissSystem.Utilities
Imports System.Text
Imports System.Text.RegularExpressions
Imports Lixil.AvoidMissSystem.Utilities.Consts

''' <summary>
''' 权限MS维护用form
''' </summary>
''' <remarks></remarks>
Public Class MsMaintenanceAuthorityForm

    Private ComBizLogic As New CommonBizLogic
    Private authorityBL As New MsMaintenanceAuthorityBizLogic
    Private dicExcelHeader As Dictionary(Of String, String)
    Dim beforeUpdDt As DataTable
    Dim afterUpdDt As DataTable

#Region "事件"
    ''' <summary>
    ''' 窗体初期加载事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MsMaintenanceAuthorityForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            '导入EXCEL文件列头设定
            dicExcelHeader = New Dictionary(Of String, String)
            dicExcelHeader.Add("user_id", "用户id")
            dicExcelHeader.Add("LoginName", "登录名")
            dicExcelHeader.Add("LoginPassword", "密码")
            dicExcelHeader.Add("UserName", "用户名")
            dicExcelHeader.Add("UserCode", "用户代码")
            dicExcelHeader.Add("UserType", "用户类型")
            dicExcelHeader.Add("access_type", "权限类型")
            dicExcelHeader.Add("access_cd", "权限区分")
            dicExcelHeader.Add("delete_flg", "删除区分")

            '窗体初期化处理
            Initialize()
            Me.Text = "权限MS维护"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 管理员权限按钮选择变更事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cbEditAdmin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEditAdmin.CheckedChanged
        '管理员权限选中
        If Me.cbEditAdmin.Checked = True Then
            ClearCheckedItems(Me.CheckedListFunctionAccess)
            Me.CheckedListFunctionAccess.Enabled = False
            ClearCheckedItems(Me.CheckedListDepartmentAccess)
            Me.CheckedListDepartmentAccess.Enabled = False
        Else '管理员权限没有选中
            Me.CheckedListFunctionAccess.Enabled = True
            Me.CheckedListDepartmentAccess.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' GridView双击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvAuthority_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvAuthority.CellDoubleClick
        '用户id
        Dim strUserId As String = String.Empty
        '删除区分：未删除
        Dim strDeleteFlg As String = Consts.UNDELETED
        Dim dtAuthority As DataTable

        Try
            If e.RowIndex >= 0 Then

                '编辑部项目初始化
                InitializeEdit()

                '删除按钮可用
                Me.btnDelete.Enabled = True

                '保存按钮可用
                Me.btnSave.Enabled = True

                '用户
                Me.txtUser.Text = Convert.ToString(Me.dgvAuthority.Rows(e.RowIndex).Cells("LoginName").Value)

                '密码
                Me.txtPassword.Text = Convert.ToString(Me.dgvAuthority.Rows(e.RowIndex).Cells("password").Value)
                '新密码可用
                Me.txtNewPassword.Enabled = True
                '确认新密码可用
                Me.txtConfimPassword.Enabled = True

                '取得用户id
                strUserId = Convert.ToString(Me.dgvAuthority.Rows(e.RowIndex).Cells("id").Value)

                '给隐藏项用户id赋值
                Me.txtHidId.Text = strUserId

                '取得权限信息一览
                dtAuthority = authorityBL.GetAuthorityList(strUserId, strDeleteFlg).Tables("mauthority")

                '机能权限
                If dtAuthority.Select("access_type='" & Consts.AccessType.KINOU & "'").Length > 0 Then
                    For Each row As DataRow In dtAuthority.Rows
                        If row.Item("access_type").ToString = Consts.AccessType.KINOU Then
                            Dim indexKinou As Integer = CInt(row.Item("access_cd").ToString) - 1

                            '避免DB脏数据造成数组越界错误
                            If indexKinou <= Me.CheckedListFunctionAccess.Items.Count - 1 Then
                                Me.CheckedListFunctionAccess.SetItemChecked(indexKinou, True)
                            End If
                        End If
                    Next
                End If

                '部门权限
                If dtAuthority.Select("access_type='" & Consts.AccessType.BUMON & "'").Length > 0 Then
                    For Each row As DataRow In dtAuthority.Rows
                        If row.Item("access_type").ToString = Consts.AccessType.BUMON Then
                            Dim indexBumon As Integer = CInt(row.Item("access_cd").ToString) - 1

                            '避免DB脏数据造成数组越界错误
                            If indexBumon <= Me.CheckedListDepartmentAccess.Items.Count - 1 Then
                                Me.CheckedListDepartmentAccess.SetItemChecked(indexBumon, True)
                            End If
                        End If
                    Next
                End If

                '管理员权限
                If dtAuthority.Select("access_type='" & Consts.AccessType.ADMIN & "'").Length > 0 Then
                    Me.cbEditAdmin.Checked = True
                End If

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 检索按钮点击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        DoSearch()
    End Sub

    ''' <summary>
    ''' 清空按钮点击事件
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
    ''' 删除按钮点击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            Dim dicAuthority As New Dictionary(Of String, String)

            If Me.dgvAuthority.Rows.Count <= 0 Then
                MessageBox.Show("没有要删除的记录！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            If MessageBox.Show("确认删除吗！", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Forms.DialogResult.Cancel Then
                Exit Sub
            End If

            '用户id
            dicAuthority.Add("user_id", Me.txtHidId.Text)
            '删除区分
            dicAuthority.Add("delete_flg", Consts.DELETED)
            '当前登录用户名
            dicAuthority.Add("user", Login.UserName)
            '更新时间
            Dim sysTime As DateTime
            sysTime = ComBizLogic.GetSystemDate()
            dicAuthority.Add("sysTime", sysTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))
            '执行删除处理
            If authorityBL.DeleteAuthority(dicAuthority) = False Then
                MessageBox.Show(MsgConst.M00019I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            '日志表插入
            ComBizLogic.InsertLog("权限MS", OperateKind.DELETE, "", Me.Login.UserId, DateTime.Now)

            MessageBox.Show(MsgConst.M00012I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)

            '重新执行检索操作
            DoSearch()
            Me.btnSearch.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 保存按钮点击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        '密码变更区分
        Dim hasPwChanged As Boolean
        '新密码
        Dim strNewPassword As String = Me.txtNewPassword.Text.Trim
        '确认新密码
        Dim strConfimPassword As String = Me.txtConfimPassword.Text.Trim
        '机能权限选中个数
        Dim cntFun As Integer = Me.CheckedListFunctionAccess.CheckedItems.Count
        '部门权限选中个数
        Dim cntDep As Integer = Me.CheckedListDepartmentAccess.CheckedItems.Count
        '用户id
        Dim strUserId As String = Me.txtHidId.Text
        '权限类型
        Dim strAccessType As String = String.Empty
        '权限区分
        Dim strAccessCd As String = String.Empty
        '当前登录用户名
        Dim strUser As String = Login.UserName
        '取得系统时间
        Dim sysTime As New DateTime
        Dim strSysTime As String = String.Empty
        '存放权限信息
        Dim dicAuthority As New Dictionary(Of String, String)

        Dim strUpdFPath As String = String.Empty
        Dim endTime As DateTime
        Try
            sysTime = ComBizLogic.GetSystemDate()
            strSysTime = sysTime.ToString("yyyy-MM-dd HH:mm:ss.fff")

            '输入校验:用户信息部分
            If (String.IsNullOrEmpty(strNewPassword) AndAlso String.IsNullOrEmpty(strConfimPassword)) Then
                hasPwChanged = False
            Else
                '新密码和确认新密码一致时
                If strNewPassword.Equals(strConfimPassword) Then
                    hasPwChanged = True
                Else
                    '新密码和确认新密码不一致时
                    MessageBox.Show(MsgConst.M00044I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            End If

            '输入校验:权限信息部分
            If cntFun > 0 AndAlso cntDep = 0 Then
                '只选择了机能权限没有选择部门权限时
                MessageBox.Show(String.Format(MsgConst.M00045I, "部门权限"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            ElseIf cntFun = 0 AndAlso cntDep > 0 Then
                '只选择了部门权限没有选择机能权限时
                MessageBox.Show(String.Format(MsgConst.M00045I, "机能权限"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            '更新前后数据保存用Datatable初始化
            InitTable()
            beforeUpdDt = authorityBL.GetUserId(strUserId, strAccessType, strAccessCd, False)

            '需要更新用户信息时
            If hasPwChanged = True Then
                '更新用户信息
                If authorityBL.UpdateUserInfo(strUserId, strNewPassword) = False Then
                    MessageBox.Show(MsgConst.M00017I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            End If

            '更新权限信息
            '首先进行伦理删除
            dicAuthority.Add("user_id", strUserId) '用户id
            dicAuthority.Add("user", strUser) '用户名
            dicAuthority.Add("sysTime", strSysTime) '系统时间
            dicAuthority.Add("delete_flg", Consts.DELETED) '删除区分
            '権限信息削除処理
            If authorityBL.DeleteAuthority(dicAuthority) = False Then
                MessageBox.Show(MsgConst.M00017I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            '删除区分变更
            dicAuthority.Remove("delete_flg")
            dicAuthority.Add("delete_flg", Consts.UNDELETED)

            If Me.cbEditAdmin.Checked = True Then
                '更新管理员权限
                dicAuthority.Add("access_type", Consts.AccessType.ADMIN) '权限类型
                dicAuthority.Add("access_cd", Consts.AccessCd.MS_ADMIN) '权限区分
                '执行权限信息更新处理
                If UpdateAuthority(dicAuthority) = False Then
                    MessageBox.Show(MsgConst.M00017I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            ElseIf cntFun > 0 AndAlso cntDep > 0 Then

                '更新机能权限
                dicAuthority.Add("access_type", Consts.AccessType.KINOU) '权限类型
                For indexFun As Integer = 0 To Me.CheckedListFunctionAccess.Items.Count - 1
                    If Me.CheckedListFunctionAccess.GetItemChecked(indexFun) = True Then
                        If dicAuthority.ContainsKey("access_cd") Then
                            dicAuthority.Remove("access_cd")
                        End If

                        dicAuthority.Add("access_cd", "00" & indexFun + 1) '权限区分
                        '执行权限信息更新处理
                        If UpdateAuthority(dicAuthority) = False Then
                            MessageBox.Show(MsgConst.M00017I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        End If
                    End If
                Next

                '更新部门权限
                dicAuthority.Remove("access_type")
                dicAuthority.Add("access_type", Consts.AccessType.BUMON) '权限类型
                For indexDep As Integer = 0 To Me.CheckedListDepartmentAccess.Items.Count - 1
                    If Me.CheckedListDepartmentAccess.GetItemChecked(indexDep) = True Then
                        If dicAuthority.ContainsKey("access_cd") Then
                            dicAuthority.Remove("access_cd")
                        End If
                        dicAuthority.Add("access_cd", "00" & indexDep + 1) '权限区分
                        '执行权限信息更新处理
                        If UpdateAuthority(dicAuthority) = False Then
                            MessageBox.Show(MsgConst.M00017I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        End If
                    End If
                Next
            End If

            endTime = DateTime.Now()

            MessageBox.Show(MsgConst.M00016I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '登录成功后，再检索
            DoSearch()

            afterUpdDt = authorityBL.GetUserId(strUserId, strAccessType, strAccessCd, False)
            '错误数据及更新信息导出
            strUpdFPath = LogExport.LogExport("权限MS", New DataTable, beforeUpdDt, afterUpdDt, endTime)
            '日志表插入
            ComBizLogic.InsertLog("权限MS", OperateKind.UPDATE, strUpdFPath, Me.Login.UserId, endTime)

            Me.dgvAuthority.Focus()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 返回按钮事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        Try
            NavigateToNextPage("msMain")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 退出按钮事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            NavigateToNextPage("msLogin")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 模板下载连接点击事件
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

            pathTemplate = appPath & "/Template/AuthorityMSTempate.xls"
            saveFileDialog.Filter = "下载模板(*.xls)|*.xls"
            saveFileDialog.FileName = "权限MS模板"
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
    ''' 选择路径点击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFilePath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFilePath.Click
        Dim openFileDialog As OpenFileDialog
        Dim strFileType As String
        Try
            openFileDialog = New OpenFileDialog()
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
    ''' 执行按钮点击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExcute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcute.Click
        If Me.rdoImport.Checked = True Then
            '执行导入处理
            ImportExcel()
        Else
            '执行导出处理
            ExportExcel()
        End If
    End Sub
#End Region

#Region "方法"

    ''' <summary>
    ''' 窗体初期化处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Initialize()
        Try
            '检索部项目初始化
            InitializeSearch()
            '编辑部项目初始化
            InitializeEdit()
            'GridView初期化
            InitializeGridView()
            '批量处理部初始化
            InitializeBat()

            Me.btnSearch.Enabled = True '检索按钮
            Me.btnClear.Enabled = True '清空按钮
            Me.btnDelete.Enabled = False '删除按钮
            Me.btnSave.Enabled = False '保存按钮
            Me.txtSearchUser.Focus()
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' 检索部项目初始化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeSearch()
        Me.txtSearchUser.Text = String.Empty '用户名
        Me.cbSearchAdmin.Checked = False '管理员权限
        ClearCheckedItems(Me.CheckedListSearchFunctionAccess) '机能权限
        ClearCheckedItems(Me.CheckedListSearchDepartmentAccess) '部门权限
    End Sub

    ''' <summary>
    ''' 编辑部项目初始化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeEdit()
        '用户名
        Me.txtUser.Text = String.Empty
        Me.txtUser.Enabled = False
        '旧密码
        Me.txtPassword.Text = String.Empty
        Me.txtPassword.Enabled = False
        '新密码
        Me.txtNewPassword.Text = String.Empty
        Me.txtNewPassword.Enabled = False
        '确认新密码
        Me.txtConfimPassword.Text = String.Empty
        Me.txtConfimPassword.Enabled = False
        '管理员权限
        Me.cbEditAdmin.Checked = False
        '机能权限
        ClearCheckedItems(Me.CheckedListFunctionAccess)
        '部门权限
        ClearCheckedItems(Me.CheckedListDepartmentAccess)
    End Sub

    ''' <summary>
    ''' 数据生成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenerateData(ByVal hasData As Boolean) As DataTable
        Try
            Dim arrHeader() As String = {"LoginName", "password", "UserName", "id"}
            Dim dtAuthority As New DataTable
            Dim dc As DataColumn

            For i As Integer = 0 To arrHeader.Length - 1
                dc = New DataColumn(arrHeader(i), GetType(String))
                dtAuthority.Columns.Add(dc)
            Next

            Return dtAuthority
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
            '数据生成
            Me.dgvAuthority.DataSource = GenerateData(False)
            Me.dgvAuthority.ReadOnly = True

            Me.dgvAuthority.Columns("LoginName").HeaderText = "登录名"
            Me.dgvAuthority.Columns("password").HeaderText = "密码"
            Me.dgvAuthority.Columns("UserName").HeaderText = "用户名"
            Me.dgvAuthority.Columns("id").HeaderText = "ID"

            Me.dgvAuthority.Columns("LoginName").Width = 120
            Me.dgvAuthority.Columns("password").Width = 120
            Me.dgvAuthority.Columns("UserName").Width = 120
            Me.dgvAuthority.Columns("id").Width = 120

            Me.dgvAuthority.Columns("LoginName").DefaultCellStyle.ForeColor = Color.Black
            Me.dgvAuthority.Columns("password").DefaultCellStyle.ForeColor = Color.Black
            Me.dgvAuthority.Columns("UserName").DefaultCellStyle.ForeColor = Color.Black

            Me.dgvAuthority.Columns("id").Visible = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' 批量处理部初始化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeBat()
        '文件路径
        Me.txtFilePath.Text = String.Empty
        '批量导入
        Me.rdoImport.Checked = True
        '批量导出
        Me.rdoExport.Checked = False
        '选择路径按钮
        Me.btnFilePath.Enabled = True
        '执行按钮
        Me.btnExcute.Enabled = True
    End Sub

    ''' <summary>
    ''' 执行检索操作
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DoSearch()

        Dim dtAuthority As DataTable
        Dim dtSelect As New DataTable
        Dim dicSearch As New Dictionary(Of String, String)
        Try
            '取得检索条件
            dicSearch = GetSearchCondition()

            '取得用戸信息一覧
            dtAuthority = authorityBL.GetUserInfoList(dicSearch).Tables("mauthority")

            Me.dgvAuthority.DataSource = dtAuthority

            '编辑部项目初始化
            InitializeEdit()

            '删除按钮不可用
            Me.btnDelete.Enabled = False

            '保存按钮不可用
            Me.btnSave.Enabled = False

            '检索到的记录为0条时，提示信息
            If dtAuthority.Rows.Count = 0 Then
                MessageBox.Show(MsgConst.M00005I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 取得检索条件
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSearchCondition() As Dictionary(Of String, String)

        Dim dicSearch As New Dictionary(Of String, String) '返回值
        Dim strAccessCd2 As String = String.Empty '机能权限
        Dim strAccessCd3 As String = String.Empty '部门权限

        dicSearch.Add("deleteFlg", Consts.UNDELETED) '删除区分
        dicSearch.Add("loginName", Me.txtSearchUser.Text.Trim) '用户

        '管理员权限
        If Me.cbSearchAdmin.Checked Then
            dicSearch.Add("accessType1", Consts.AccessType.ADMIN)
            dicSearch.Add("accessCd1", Consts.AccessCd.MS_ADMIN)
        Else
            dicSearch.Add("accessType1", String.Empty)
            dicSearch.Add("accessCd1", String.Empty)
        End If

        '机能权限
        If Me.CheckedListSearchFunctionAccess.CheckedItems.Count > 0 Then
            Dim sbAccessCd2 As New StringBuilder
            For i As Integer = 0 To CheckedListSearchFunctionAccess.Items.Count - 1
                If CheckedListSearchFunctionAccess.GetItemChecked(i) Then
                    sbAccessCd2.Append("'00" & CStr(i + 1))
                    sbAccessCd2.Append("',")
                End If
            Next
            strAccessCd2 = sbAccessCd2.ToString.TrimEnd(CChar(","))
            dicSearch.Add("accessType2", Consts.AccessType.KINOU)
            dicSearch.Add("accessCd2", strAccessCd2)
        Else
            dicSearch.Add("accessType2", String.Empty)
            dicSearch.Add("accessCd2", String.Empty)
        End If

        '部门权限
        If Me.CheckedListSearchDepartmentAccess.CheckedItems.Count > 0 Then
            Dim sbAccessCd3 As New StringBuilder
            For j As Integer = 0 To CheckedListSearchDepartmentAccess.Items.Count - 1
                If CheckedListSearchDepartmentAccess.GetItemChecked(j) Then
                    sbAccessCd3.Append("'00" & CStr(j + 1))
                    sbAccessCd3.Append("',")
                End If
            Next
            strAccessCd3 = sbAccessCd3.ToString.TrimEnd(CChar(","))
            dicSearch.Add("accessType3", Consts.AccessType.BUMON)
            dicSearch.Add("accessCd3", strAccessCd3)
        Else
            dicSearch.Add("accessType3", String.Empty)
            dicSearch.Add("accessCd3", String.Empty)
        End If

        Return dicSearch
    End Function

    ''' <summary>
    ''' 将checkedListBox取消选中
    ''' </summary>
    ''' <param name="chkListBox"></param>
    ''' <remarks></remarks>
    Private Sub ClearCheckedItems(ByVal chkListBox As System.Windows.Forms.CheckedListBox)
        For i As Integer = 0 To chkListBox.Items.Count - 1
            chkListBox.SetItemChecked(i, False)
        Next
    End Sub

    ''' <summary>
    ''' 权限信息更新处理
    ''' </summary>
    ''' <param name="dicAuthority"></param>
    ''' <remarks></remarks>
    Private Function UpdateAuthority(ByVal dicAuthority As Dictionary(Of String, String)) As Boolean
        '判断记录是否存在
        If authorityBL.IsAuthorityExsit(dicAuthority) = True Then
            '记录存在时，执行権限信息更新処理
            If authorityBL.UpdateAuthority(dicAuthority) = False Then
                Return False
            End If
        Else
            '记录不存在时，执行権限信息添加処理
            If authorityBL.InsertAuthority(dicAuthority) = False Then
                Return False
            End If
        End If
        Return True
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
            If Not xlApp Is Nothing Then
                xlApp = Nothing
            End If
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
            Dim result As New Result
            Dim strUserId As String = Convert.ToString(drExcel("用户id"))
            Dim strLoginName As String = Convert.ToString(drExcel("登录名"))
            Dim strLoginPassword As String = Convert.ToString(drExcel("密码"))
            Dim strUserName As String = Convert.ToString(drExcel("用户名"))
            Dim strUserCode As String = Convert.ToString(drExcel("用户代码"))
            Dim strUserType As String = Convert.ToString(drExcel("用户类型"))
            Dim strAccessType As String = Convert.ToString(drExcel("权限类型"))
            Dim strAccessCd As String = Convert.ToString(drExcel("权限区分"))
            Dim strDeleteFlg As String = Convert.ToString(drExcel("删除区分"))

            ''必须输入校验
            '用户id
            If String.IsNullOrEmpty(strUserId) Then
                result.Message = String.Format(MsgConst.M00002I, "用户id")
                result.Success = False
                Return result
            End If

            '权限类型和权限区分不都为空时，进行必须输入校验，否则不进行校验
            If String.IsNullOrEmpty(strAccessType) = False OrElse String.IsNullOrEmpty(strAccessCd) = False Then
                '权限类型
                If String.IsNullOrEmpty(strAccessType) Then
                    result.Message = String.Format(MsgConst.M00002I, "权限类型")
                    result.Success = False
                End If
                '权限区分
                If String.IsNullOrEmpty(strAccessCd) Then
                    result.Message = String.Format(MsgConst.M00002I, "权限区分")
                    result.Success = False
                End If
            End If

            ''输入字符串长度校验
            '用户id
            If strUserId.Length > 18 Then
                result.Message = String.Format(MsgConst.M00003I, "用户id")
                result.Success = False
                Return result
            End If
            '用户
            If strLoginName.Length > 30 Then
                result.Message = String.Format(MsgConst.M00003I, "登录名")
                result.Success = False
                Return result
            End If
            '密码
            If strLoginPassword.Length > 30 Then
                result.Message = String.Format(MsgConst.M00003I, "密码")
                result.Success = False
                Return result
            End If
            '用户名
            If strUserName.Length > 50 Then
                result.Message = String.Format(MsgConst.M00003I, "用户名")
                result.Success = False
                Return result
            End If
            '用户代码
            If strUserCode.Length > 50 Then
                result.Message = String.Format(MsgConst.M00003I, "用户代码")
                result.Success = False
                Return result
            End If
            '用户类型
            If strUserType.Length > 10 Then
                result.Message = String.Format(MsgConst.M00003I, "用户类型")
                result.Success = False
                Return result
            End If

            '记录中权限类型和权限区分都不为空时，对权限信息进行校验
            If String.IsNullOrEmpty(strAccessType) = False AndAlso String.IsNullOrEmpty(strAccessCd) = False Then

                '权限类型
                If strAccessType.Length > 1 Then
                    result.Message = String.Format(MsgConst.M00003I, "权限类型")
                    result.Success = False
                    Return result
                End If
                '权限区分
                If strAccessCd.Length > 3 Then
                    result.Message = String.Format(MsgConst.M00003I, "权限区分")
                    result.Success = False
                    Return result
                End If
                '删除区分
                If strDeleteFlg.Length > 1 Then
                    result.Message = String.Format(MsgConst.M00003I, "删除区分")
                    result.Success = False
                    Return result
                End If

                '提示信息参数
                Dim sbParam As New StringBuilder
                ''设定值范围校验
                '权限类型和权限区分
                Select Case strAccessType
                    Case Consts.AccessType.ADMIN  '管理员权限
                        Select Case strAccessCd '权限区分
                            Case Consts.AccessCd.MS_ADMIN
                            Case Else
                                '信息设定
                                result.Message = String.Format(MsgConst.M00049I, "权限区分", strAccessType, Consts.AccessCd.MS_ADMIN)
                                result.Success = False
                                Return result
                        End Select
                        Exit Select
                    Case Consts.AccessType.KINOU  '机能权限
                        Select Case strAccessCd '权限区分
                            Case Consts.AccessCd.MS_PICTRUE
                            Case Consts.AccessCd.MS_TOOLS
                            Case Consts.AccessCd.MS_BASIC
                            Case Consts.AccessCd.MS_RESULTMODIFY
                            Case Else
                                sbParam.Append(Consts.AccessCd.MS_PICTRUE)
                                sbParam.Append(", ")
                                sbParam.Append(Consts.AccessCd.MS_TOOLS)
                                sbParam.Append(", ")
                                sbParam.Append(Consts.AccessCd.MS_BASIC)
                                sbParam.Append(", ")
                                sbParam.Append(Consts.AccessCd.MS_RESULTMODIFY)
                                '信息设定
                                result.Message = String.Format(MsgConst.M00049I, "权限区分", strAccessType, sbParam.ToString)
                                result.Success = False
                                Return result
                        End Select
                        Exit Select
                    Case Consts.AccessType.BUMON  '部门权限
                        Select Case strAccessCd '权限区分
                            Case Consts.AccessCd.BM_FIRST
                            Case Consts.AccessCd.BM_SECOND
                            Case Consts.AccessCd.BM_THIRD
                            Case Else
                                sbParam.Append(Consts.AccessCd.BM_FIRST)
                                sbParam.Append(", ")
                                sbParam.Append(Consts.AccessCd.BM_SECOND)
                                sbParam.Append(", ")
                                sbParam.Append(Consts.AccessCd.BM_THIRD)
                                '信息设定
                                result.Message = String.Format(MsgConst.M00049I, "权限区分", strAccessType, sbParam.ToString)
                                result.Success = False
                                Return result
                        End Select
                    Case Else
                        sbParam.Append(Consts.AccessType.ADMIN)
                        sbParam.Append(", ")
                        sbParam.Append(Consts.AccessType.KINOU)
                        sbParam.Append(", ")
                        sbParam.Append(Consts.AccessType.BUMON)
                        '信息设定
                        result.Message = String.Format(MsgConst.M00048I, "权限类型", sbParam.ToString)
                        result.Success = False
                        Return result
                End Select

                '删除区分
                If strDeleteFlg <> Consts.UNDELETED AndAlso strDeleteFlg <> Consts.DELETED Then
                    sbParam.Append(Consts.UNDELETED)
                    sbParam.Append(", ")
                    sbParam.Append(Consts.DELETED)
                    '信息设定
                    result.Message = String.Format(MsgConst.M00048I, "删除区分", sbParam.ToString)
                    result.Success = False
                    Return result
                End If
            End If

            ''文字属性校验
            '用户id
            If FormatCheck.IsHalfNumberOnly(strUserId) = False Then
                result.Message = String.Format(MsgConst.M00046I, "用户id")
                result.Success = False
                Return result
            End If
            '用户类型
            If FormatCheck.IsHalfNumberOnly(strUserType) = False Then
                result.Message = String.Format(MsgConst.M00046I, "用户类型")
                result.Success = False
                Return result
            End If

            '用户id存在检查
            Dim dtAuthority As New DataTable
            dtAuthority = authorityBL.GetUserId(strUserId, strAccessType, strAccessCd, True)
            If dtAuthority.Rows.Count > 0 Then
                '更新的时候，更新前的数据保存到Datatable中
                beforeUpdDt.Rows.Add(dtAuthority.Rows(0).ItemArray)
            End If

            result.Success = True
            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function

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
        Dim strSheetNames As String()
        Dim strExportFPath As String
        Dim endTime As DateTime
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

            Me.Cursor = Cursors.WaitCursor
            If File.Exists(strFilePath) Then
                '[权限信息信息]sheet存在检查
                strSheetNames = LogExport.GetAllSheetName(strFilePath)
                For i As Integer = 0 To strSheetNames.Length - 1
                    If strSheetNames(i) = "权限信息$" Then
                        Exit For
                    End If
                    If i = strSheetNames.Length - 1 Then
                        MessageBox.Show("EXCEL文件中不存在[权限信息]Sheet页！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                Next

                '读取EXCEL数据
                dtExcel = GetExcelData("权限信息", strFilePath)

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

                '更新前后数据保存用Datatable初始化
                InitTable()

                For Each drExcel As DataRow In dtExcel.Rows
                    'EXCEL数据校验
                    result = ImportCheck(drExcel)
                    If result.Success Then
                        drImport = dtImport.NewRow()
                        drImport("用户id") = drExcel("用户id")
                        drImport("登录名") = drExcel("登录名")
                        drImport("密码") = drExcel("密码")
                        drImport("用户名") = drExcel("用户名")
                        drImport("用户代码") = drExcel("用户代码")
                        drImport("用户类型") = drExcel("用户类型")
                        drImport("权限类型") = drExcel("权限类型")
                        drImport("权限区分") = drExcel("权限区分")
                        drImport("删除区分") = drExcel("删除区分")
                        dtImport.Rows.Add(drImport)
                    Else
                        drError = dtError.NewRow()
                        drError("用户id") = drExcel("用户id")
                        drError("登录名") = drExcel("登录名")
                        drError("密码") = drExcel("密码")
                        drError("用户名") = drExcel("用户名")
                        drError("用户代码") = drExcel("用户代码")
                        drError("用户类型") = drExcel("用户类型")
                        drError("权限类型") = drExcel("权限类型")
                        drError("权限区分") = drExcel("权限区分")
                        drError("删除区分") = drExcel("删除区分")
                        drError("错误信息") = result.Message
                        dtError.Rows.Add(drError)
                    End If
                Next

                'EXCEL数据导入
                If authorityBL.DoImport(dtImport, Me.Login.UserName, dtError, afterUpdDt) Then
                    '结束时间
                    endTime = DateTime.Now

                    '错误数据及更新信息导出
                    strExportFPath = LogExport.LogExport("权限MS", dtError, beforeUpdDt, afterUpdDt, endTime)
                    '日志表插入
                    ComBizLogic.InsertLog("权限MS", OperateKind.IMPORT, strExportFPath, Me.Login.UserId, endTime)

                    MessageBox.Show(String.Format(MsgConst.M00015I, dtExcel.Rows.Count, dtExcel.Rows.Count - dtError.Rows.Count, dtError.Rows.Count),
                                                       "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    If dtError.Rows.Count > 0 AndAlso MessageBox.Show(MsgConst.M00003C, "確認",
                       MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Forms.DialogResult.OK Then
                        frmErrorData = New ErrorDataForm()
                        frmErrorData.ErrorData = dtError
                        frmErrorData.Show()
                    End If
                    '导入成功后，再检索
                    DoSearch()
                Else
                    MessageBox.Show(MsgConst.M00014I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Exit Sub
                End If
            Else
                MessageBox.Show(String.Format(MsgConst.M00010I, "批量导入"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Cursor = Cursors.Arrow
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox(ex.Message)

            'Throw ex
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub

    ''' <summary>
    ''' 'EXCEL文件导出处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExportExcel()

        Dim dicSearch As New Dictionary(Of String, String)
        Dim dtExportData As DataTable
        Dim rowCount As Integer
        Dim colCount As Integer
        Dim strFileName As String
        Dim saveFileDialog As SaveFileDialog
        Dim xlBook = Nothing
        Dim xlSheet = Nothing
        Dim xlApp = Nothing
        Dim arrData(0, 0) As String
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

            Me.Cursor = Cursors.WaitCursor
            '取得检索条件
            dicSearch = GetSearchCondition()

            '根据检索条件，取得导出数据
            dtExportData = authorityBL.GetExportData(dicSearch).Tables("mauthority")
            If dtExportData.Rows.Count < 1 Then
                MessageBox.Show(MsgConst.M00005I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            rowCount = dtExportData.Rows.Count
            colCount = dtExportData.Columns.Count
            ReDim arrData(rowCount, colCount)

            xlApp = CreateObject("Excel.Application")
            xlApp.Visible = False
            xlBook = xlApp.Workbooks.Add
            For i As Integer = 0 To rowCount
                If i = 0 Then
                    arrData(0, 0) = "用户id"
                    arrData(0, 1) = "登录名"
                    arrData(0, 2) = "密码"
                    arrData(0, 3) = "用户名"
                    arrData(0, 4) = "用户代码"
                    arrData(0, 5) = "用户类型"
                    arrData(0, 6) = "权限类型"
                    arrData(0, 7) = "权限区分"
                    arrData(0, 8) = "删除区分"
                Else
                    With dtExportData.Rows(i - 1)
                        arrData(i, 0) = Convert.ToString(.Item("id"))
                        arrData(i, 1) = Convert.ToString(.Item("LoginName"))
                        arrData(i, 2) = Convert.ToString(.Item("password"))
                        arrData(i, 3) = Convert.ToString(.Item("UserName"))
                        arrData(i, 4) = Convert.ToString(.Item("UserCode"))
                        arrData(i, 5) = Convert.ToString(.Item("UserType"))
                        arrData(i, 6) = Convert.ToString(.Item("access_type"))
                        arrData(i, 7) = Convert.ToString(.Item("access_cd"))
                        arrData(i, 8) = Convert.ToString(.Item("delete_flg"))
                    End With
                End If
            Next

            '导出数据设定
            xlSheet = xlBook.Worksheets.Add()
            xlSheet.Name = "权限信息"
            For Each wkSheet In xlBook.Worksheets
                If wkSheet.Name <> "权限信息" Then
                    wkSheet.Delete()
                End If
            Next
            xlSheet.Activate()
            xlSheet.Range("A1").Resize(rowCount + 1, colCount).Value = arrData
            xlBook.SaveAs(strFileName)
            xlBook.Close()
            xlApp.Quit()
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

    '''' <summary>
    '''' 根据下标转换成权限区分
    '''' </summary>
    '''' <param name="index"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Private Function IndexToAccessCd(ByVal index As Integer) As String
    '    Dim strAccessCd As String = String.Empty
    '    Select Case index
    '        Case 0
    '            strAccessCd = Consts.AccessCd.MS_PICTRUE
    '        Case 1
    '            strAccessCd = Consts.AccessCd.MS_TOOLS
    '        Case 2
    '            strAccessCd = Consts.AccessCd.MS_BASIC
    '        Case 3
    '            strAccessCd = Consts.AccessCd.MS_RESULTMODIFY
    '    End Select
    '    Return strAccessCd
    'End Function

    ''' <summary>
    ''' Log文件出力用Datatable初始化
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitTable()
        '更新前数据保存用Datatable初始化
        beforeUpdDt = New DataTable
        beforeUpdDt.TableName = "BeforeUpdate"
        beforeUpdDt.Columns.Add("用户id", GetType(String))
        beforeUpdDt.Columns.Add("登录名", GetType(String))
        beforeUpdDt.Columns.Add("密码", GetType(String))
        beforeUpdDt.Columns.Add("用户名", GetType(String))
        beforeUpdDt.Columns.Add("用户代码", GetType(String))
        beforeUpdDt.Columns.Add("用户类型", GetType(String))
        beforeUpdDt.Columns.Add("权限类型", GetType(String))
        beforeUpdDt.Columns.Add("权限区分", GetType(String))
        beforeUpdDt.Columns.Add("删除区分", GetType(String))

        afterUpdDt = New DataTable
        afterUpdDt.TableName = "AfterUpdate"
        afterUpdDt.Columns.Add("用户id", GetType(String))
        afterUpdDt.Columns.Add("登录名", GetType(String))
        afterUpdDt.Columns.Add("密码", GetType(String))
        afterUpdDt.Columns.Add("用户名", GetType(String))
        afterUpdDt.Columns.Add("用户代码", GetType(String))
        afterUpdDt.Columns.Add("用户类型", GetType(String))
        afterUpdDt.Columns.Add("权限类型", GetType(String))
        afterUpdDt.Columns.Add("权限区分", GetType(String))
        afterUpdDt.Columns.Add("删除区分", GetType(String))
    End Sub

#End Region

#Region "其他"
    ''' <summary>
    ''' 重写CheckedListBox（选中的时候没有背景色）
    ''' </summary>
    ''' <remarks></remarks>
    Class ColorCodedCheckedListBox
        Inherits CheckedListBox
        'Color.Orange为颜色，你可修改
        Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
            Dim e2 As DrawItemEventArgs = New DrawItemEventArgs(e.Graphics, e.Font, New Rectangle(e.Bounds.Location, e.Bounds.Size), e.Index, CType(((e.State & DrawItemState.Focus) Is IIf(CBool(DrawItemState.Focus), DrawItemState.Focus, DrawItemState.None)), DrawItemState), Color.Black, Me.BackColor)
            MyBase.OnDrawItem(e2)
        End Sub
    End Class

    ''' <summary>
    ''' 点击一下就能选中或者取消选中（默认模式是先获得焦点，然后才能设置选中，需要点击两次）
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub chkLKind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckedListFunctionAccess.Click, CheckedListDepartmentAccess.Click, CheckedListSearchFunctionAccess.Click, CheckedListSearchDepartmentAccess.Click
        Dim chk As ColorCodedCheckedListBox = DirectCast(sender, ColorCodedCheckedListBox)
        chk.SetSelected(chk.SelectedIndex, True)
    End Sub
#End Region

#Region "自定义类"
    ''' <summary>
    ''' 结果
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