Imports System.Net
Imports System.IO
Imports Lixil.AvoidMissSystem.BizLogic
'Imports Microsoft.Office.Interop
Imports Lixil.AvoidMissSystem.Utilities.Consts
Imports Lixil.AvoidMissSystem.Utilities.MsgConst
Imports Lixil.AvoidMissSystem.WinUI.Common
Imports Lixil.AvoidMissSystem.Utilities
Imports System.Windows

''' <summary>
''' 图片MS
''' </summary>
''' <remarks></remarks>
Public Class MsMaintenancePictureForm

    Dim bizPic As New PictureBizLogic
    Dim strUser As String = String.Empty
    Dim comBc As New CommonBizLogic
    '更新时的判定
    Dim picAddFlg As Boolean = False
    Dim headColorChange As Boolean = False
    Dim strDepartmentChangeBefore As String = String.Empty
    Dim strPicNameChangeBefore As String = String.Empty
    Dim strPicChangeBefore As String = String.Empty

    Dim updFlg As Boolean = False
    Dim picSelFlg As Boolean = False
    Dim rowDobClickFlg As Boolean = False

    Dim beforeUpdDt As DataTable
    Dim afterUpdDt As DataTable

    Dim dbClickLineIndex As Integer = -1

    Public Sub New()

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

    End Sub

    ''' <summary>
    ''' 窗体初期加载处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PictureForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strUser = Me.Login.UserId

        '窗体初期化处理
        Initialize()

        '权限初期化
        comAuthorityInit(sender, e)

    End Sub

    ''' <summary>
    ''' 权限初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub comAuthorityInit(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim dtDepartment As DataTable
        Dim dtDepartment1 As DataTable
        'Dim drDepartment As DataRow

        '权限取得
        If Me.Login.IsAdmin = True Then
            dtDepartment = comBc.GetAdminDepartment().Tables(0)
        Else
            dtDepartment = comBc.GetDepartment(Me.Login.UserId).Tables("Department")

            If dtDepartment.Rows.Count = 0 Then
                MessageBox.Show(M00027I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.btnReturn_Click(sender, e)
                Exit Sub
            End If

        End If

        'drDepartment = dtDepartment.NewRow()
        'drDepartment("mei_cd") = ""
        'drDepartment("mei") = ""
        'dtDepartment.Rows.InsertAt(drDepartment, 0)
        Me.cbDepartment.DataSource = dtDepartment
        Me.cbDepartment.ValueMember = "mei_cd"  'KEY
        Me.cbDepartment.DisplayMember = "mei" 'VALUD
        Me.cbDepartment.SelectedIndex = 0

        '权限取得
        If Me.Login.IsAdmin = True Then
            dtDepartment1 = comBc.GetAdminDepartment().Tables(0)
        Else
            dtDepartment1 = comBc.GetDepartment(Me.Login.UserId).Tables("Department")
        End If

        Me.chkLDepartment.DataSource = dtDepartment1
        Me.chkLDepartment.DisplayMember = "mei"
        Me.chkLDepartment.ValueMember = "mei_cd"

    End Sub

    ''' <summary>
    ''' 窗体初期化处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Initialize()
        Try
            Me.Text = "图片MS维护"
            Me.grdPictureData.DataSource = GenerateData()
            Me.btnSearch.Focus()
            InitializeGridView()
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' 数据生成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenerateData() As DataTable
        Try
            'Dim arrHeader() As String = {"picture_id", "goods_cd", "department_cd", "department_mei", "picture_name", "update_date", "picture_mei", "goods_id"}
            Dim arrHeader() As String = {"picture_id", "department_cd", "department_mei", "picture_nm", "picture_name", "update_date", "goods_id"}
            Dim dtPicture As New DataTable
            Dim dc As DataColumn

            For i As Integer = 0 To arrHeader.Length - 1
                dc = New DataColumn(arrHeader(i), GetType(String))
                dtPicture.Columns.Add(dc)
            Next

            Return dtPicture
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
            Me.grdPictureData.Columns("picture_id").HeaderText = "图片ID"
            'Me.grdPictureData.Columns("goods_cd").HeaderText = "商品CD"
            Me.grdPictureData.Columns("department_cd").HeaderText = "部门CD"
            Me.grdPictureData.Columns("department_mei").HeaderText = "部门"
            Me.grdPictureData.Columns("picture_nm").HeaderText = "图片名称"
            Me.grdPictureData.Columns("picture_name").HeaderText = "图片描述备注"
            Me.grdPictureData.Columns("update_date").HeaderText = "更新时间"
            Me.grdPictureData.Columns("goods_id").HeaderText = "商品ID"

            Me.grdPictureData.Columns("picture_id").Width = 80
            'Me.grdPictureData.Columns("goods_cd").Width = 100
            Me.grdPictureData.Columns("department_mei").Width = 100
            Me.grdPictureData.Columns("picture_nm").Width = 200
            Me.grdPictureData.Columns("picture_name").Width = 240
            Me.grdPictureData.Columns("update_date").Width = 120

            Me.grdPictureData.Columns("department_cd").Visible = False
            Me.grdPictureData.Columns("goods_id").Visible = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    '退出按钮按下
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    '查询按钮按下
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        strDepartmentChangeBefore = String.Empty
        strPicNameChangeBefore = String.Empty
        '更新Flg设为false
        updFlg = False

        'Me.txtGoodsCdEdit.Text = String.Empty
        Me.txtPicIdEdit.Text = String.Empty
        Me.cbDepartment.SelectedIndex = 0
        Me.txtPicNameEdit.Text = String.Empty
        Me.txtPicPath.Text = String.Empty

        Me.picBox.Image = Nothing

        Me.ltbxBefore.Visible = False
        Me.ltbxAfter.Visible = False
        Me.btnAdd.Visible = False
        Me.btnDel.Visible = False
        Me.btnAddAll.Visible = False
        Me.btnDelAll.Visible = False

        If headColorChange = True Then
            Me.grdPictureData.Columns("picture_id").HeaderCell.Style.BackColor = Color.White
            'Me.grdPictureData.Columns("goods_id").HeaderCell.Style.BackColor = Color.White
            Me.grdPictureData.Columns("department_mei").HeaderCell.Style.BackColor = Color.White
            Me.grdPictureData.Columns("picture_nm").HeaderCell.Style.BackColor = Color.White
            Me.grdPictureData.Columns("picture_name").HeaderCell.Style.BackColor = Color.White
            Me.grdPictureData.Columns("update_date").HeaderCell.Style.BackColor = Color.White
        End If

        Dim strDepartment As String = String.Empty
        Dim strAllDepartment As String = String.Empty
        Dim dtPicture As DataTable
   
        '选择的部门取得
        Dim dtDepartment As DataTable = DirectCast(chkLDepartment.DataSource, DataTable)

        For i As Integer = 0 To Me.chkLDepartment.Items.Count - 1
            If chkLDepartment.GetItemChecked(i) = True Then
                strDepartment = strDepartment & "'" & dtDepartment.Rows(i).Item("mei_cd").ToString() & "',"
            End If
            strAllDepartment = strAllDepartment & "'" & dtDepartment.Rows(i).Item("mei_cd").ToString() & "',"
        Next
        strDepartment = strDepartment.TrimEnd(CChar(","))
        strAllDepartment = strAllDepartment.TrimEnd(CChar(","))
        If strDepartment = String.Empty Then
            strDepartment = strAllDepartment
        End If
        '一览数据取得
        bizPic = New PictureBizLogic
        dtPicture = bizPic.GetPictureData(Me.txtGoodsCd.Text, strDepartment, Me.txtPicId.Text, Me.txtPicNm.Text, Me.txtPicName.Text).Tables("picture")
        Me.grdPictureData.DataSource = dtPicture
        Me.lblCnt.Text = "查询结果:" & dtPicture.Rows.Count & "条"
        '可以选择图片
        txtPicPath.Visible = True
        btnPicture.Visible = True

        If Me.grdPictureData.Rows.Count = 0 Then
            MessageBox.Show(M00005I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.picBox.Image = Nothing
            'Me.btnDelete.Enabled = False
        Else
            Me.grdPictureData.SelectedRows.Item(0).Selected = False
            Me.picBox.Image = Nothing
            'Me.btnDelete.Enabled = True
            Me.grdPictureData.EnableHeadersVisualStyles = False
        End If

        '默认显示图片
        Dim isShowPicture As Boolean = True
        For i As Integer = 0 To dtPicture.Rows.Count - 1
            If i <> dtPicture.Rows.Count - 1 Then
                If dtPicture.Rows(i)("picture_id").ToString <> dtPicture.Rows(i + 1)("picture_id").ToString Then
                    isShowPicture = False
                    Exit For
                End If
            End If
        Next
        If isShowPicture = True Then
            '默认显示图片
            If Me.grdPictureData.Rows.Count > 0 Then

                Try
                    Dim bt() As Byte = DirectCast(bizPic.GetPictureById(Me.grdPictureData.Rows(0).Cells("picture_id").Value.ToString).Tables("pictureById").Rows(0).Item("picture_content"), Byte())
                    Convert_byte_To_Image(bt)
                    Me.grdPictureData.EnableHeadersVisualStyles = False
                Catch ex As Exception

                End Try

            End If
        End If

        '删除按钮不可用
        Me.btnDelete.Enabled = False
        '清空按钮可用
        Me.btnClear.Enabled = True

        '编辑区背景色恢复初期值
        txtPicNameEdit.BackColor = Color.White
        cbDepartment.BackColor = Color.White

        '图片选择和双击Flg初始化
        picSelFlg = False
        rowDobClickFlg = False

    End Sub

    '保存按钮按下
    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Dim strUpdFPath As String = String.Empty
        Dim endTime As DateTime

        If Not InputCheck() Then
            Exit Sub
        End If

        If MessageBox.Show("确认登录吗！", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.Cancel Then
            Exit Sub
        End If

        Dim hsPicture As New Hashtable
        Dim updPara As Dictionary(Of String, String)

        If picAddFlg = True Then
            '新规
            bizPic = New PictureBizLogic
            Dim bizIndex As IndexBizLogic = New IndexBizLogic
            Dim result As String = bizIndex.GetIndex(TYPEKBN.PICTURE)
            Select Case result
                Case DBErro.InserError
                    MessageBox.Show(M00002D.Replace("{0}", "产品种类表采番"))
                    Exit Sub
                Case DBErro.UpdateError
                    MessageBox.Show(M00003D.Replace("{0}", "产品种类表采番"))
                    Exit Sub
                Case DBErro.GetIndexError
                    MessageBox.Show(M00001D.Replace("{0}", "产品种类表"))
                    Exit Sub
                Case DBErro.GetIndexMaxError
                    MessageBox.Show(M00005D.Replace("{0}", "产品种类表"))
                    Exit Sub
            End Select

            hsPicture.Add("picIdOld", Me.txtPicIdEdit.Text)
            hsPicture.Add("picId", result)
            hsPicture.Add("departmentCd", Me.cbDepartment.SelectedValue.ToString)
            hsPicture.Add("picName", Me.txtPicNameEdit.Text)
            If Me.imageToByte(Me.picBox.ImageLocation) Is Nothing Then
                MessageBox.Show(M00026I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            Else
                hsPicture.Add("picNm", Mid(Me.picBox.ImageLocation, Me.picBox.ImageLocation.LastIndexOf("\") + 2, Me.picBox.ImageLocation.Length - Me.picBox.ImageLocation.LastIndexOf("\") - 5))
                hsPicture.Add("picContent", Me.imageToByte(Me.picBox.ImageLocation))
            End If
            hsPicture.Add("User", strUser)

            '图片变更时，选择要更新的商品
            Dim strGoodsId As String = String.Empty
            If Me.ltbxAfter.Visible = True AndAlso Me.ltbxAfter.Items.Count > 0 Then
                Dim ltbx As String
                For Each ltbx In Me.ltbxAfter.Items
                    If String.IsNullOrEmpty(strGoodsId) = True Then
                        strGoodsId = "'" & ltbx & "',"
                    Else
                        strGoodsId = strGoodsId & "'" & ltbx & "',"
                    End If
                Next
            End If
            If strGoodsId <> String.Empty Then
                strGoodsId = strGoodsId.TrimEnd(CChar(","))
            End If
            hsPicture.Add("goodsId", strGoodsId)

            If bizPic.InsertPicture(hsPicture) Then
                If Me.ltbxAfter.Visible = True AndAlso Me.ltbxAfter.Items.Count > 0 Then
                    Dim i As Integer
                    For i = 0 To Me.grdPictureData.Rows.Count - 1
                        'If Me.grdPictureData.Rows(i).Cells("picture_id").Value.ToString = hsPicture("picIdOld").ToString AndAlso hsPicture("goodsId").ToString.Contains(Me.grdPictureData.Rows(i).Cells("goods_id").Value.ToString) Then
                        If Me.grdPictureData.Rows(i).Cells("picture_id").Value.ToString = hsPicture("picIdOld").ToString Then
                            Me.grdPictureData.Rows(i).Cells("picture_id").Value = hsPicture("picId").ToString
                            'Me.txtPicIdEdit.Text = hsPicture("picId").ToString
                            Me.txtPicId.Text = hsPicture("picId").ToString
                        End If
                    Next
                End If
                headColorChange = False
                Me.grdPictureData.Columns("picture_id").HeaderCell.Style.BackColor = Color.White
                'Me.grdPictureData.Columns("goods_id").HeaderCell.Style.BackColor = Color.White
                Me.grdPictureData.Columns("department_mei").HeaderCell.Style.BackColor = Color.White
                Me.grdPictureData.Columns("picture_nm").HeaderCell.Style.BackColor = Color.White
                Me.grdPictureData.Columns("picture_name").HeaderCell.Style.BackColor = Color.White
                Me.grdPictureData.Columns("update_date").HeaderCell.Style.BackColor = Color.White

                endTime = DateTime.Now()

                '日志表插入
                comBc.InsertLog("图片MS", OperateKind.INSERT, "", Me.Login.UserId, endTime)

                MessageBox.Show(M00013I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                If Me.ltbxAfter.Visible = True AndAlso Me.ltbxAfter.Items.Count > 0 Then
                    MessageBox.Show(String.Format(MsgConst.M00061I, Me.ltbxAfter.Items.Count), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If

            Else
                MessageBox.Show(M00025I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            '更新成功后刷新
            btnSearch_Click(sender, e)
        Else
            '更新
            If String.IsNullOrEmpty(Me.txtGoodsCd.Text) = True AndAlso _
                String.IsNullOrEmpty(Me.txtPicId.Text) = True AndAlso _
                String.IsNullOrEmpty(Me.txtPicName.Text) = True AndAlso _
                Me.chkLDepartment.CheckedItems.Count = 0 Then
                MessageBox.Show(M00062I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            Dim dt As DataTable = DirectCast(Me.grdPictureData.DataSource, DataTable)
            updPara = ChangeCheck()

            '更新前后数据保存用Datatable初始化
            InitTable()
            beforeUpdDt = DirectCast(grdPictureData.DataSource, DataTable)

            Dim updCnt As Integer = 0 '影响行数

            If bizPic.UpdatePicture(dt, strUser, updPara, updCnt) Then
                headColorChange = False
                Me.grdPictureData.Columns("picture_id").HeaderCell.Style.BackColor = Color.White
                'Me.grdPictureData.Columns("goods_id").HeaderCell.Style.BackColor = Color.White
                Me.grdPictureData.Columns("department_mei").HeaderCell.Style.BackColor = Color.White
                Me.grdPictureData.Columns("picture_nm").HeaderCell.Style.BackColor = Color.White
                Me.grdPictureData.Columns("picture_name").HeaderCell.Style.BackColor = Color.White
                Me.grdPictureData.Columns("update_date").HeaderCell.Style.BackColor = Color.White

                endTime = DateTime.Now()

                MessageBox.Show(M00016I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                MessageBox.Show(String.Format(MsgConst.M00061I, updCnt), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                MessageBox.Show(M00017I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            '更新成功后刷新
            btnSearch_Click(sender, e)

            afterUpdDt = DirectCast(grdPictureData.DataSource, DataTable)
            '错误数据及更新信息导出
            strUpdFPath = LogExport.LogExport("图片MS", New DataTable, beforeUpdDt, afterUpdDt, endTime)
            '日志表插入
            comBc.InsertLog("图片MS", OperateKind.UPDATE, strUpdFPath, Me.Login.UserId, endTime)

        End If

        Me.ltbxBefore.Visible = False
        Me.ltbxAfter.Visible = False
        Me.btnAdd.Visible = False
        Me.btnDel.Visible = False
        Me.btnAddAll.Visible = False
        Me.btnDelAll.Visible = False
        picAddFlg = False

        '图片选择和双击Flg初始化
        picSelFlg = False
        rowDobClickFlg = False

    End Sub

    '删除按钮按下
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Me.grdPictureData.Rows.Count = 0 Then
            MessageBox.Show(M00005I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        Else
            If MessageBox.Show("所要删除的图片正在补引用，确定要删除吗？", "确认", MessageBoxButtons.OKCancel) = DialogResult.OK Then
                Dim i As Integer
                Dim strPicId As String = String.Empty
                For i = 0 To Me.grdPictureData.Rows.Count - 1
                    If String.IsNullOrEmpty(strPicId) = True Then
                        strPicId = Me.grdPictureData.Rows(i).Cells("picture_id").Value.ToString
                    Else
                        strPicId = strPicId & "," & Me.grdPictureData.Rows(i).Cells("picture_id").Value.ToString
                    End If
                Next

                Dim hsPicture As New Hashtable
                If String.IsNullOrEmpty(strPicId) = True Then
                    MessageBox.Show(M00005I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                Else
                    hsPicture.Add("picId", strPicId)
                    hsPicture.Add("user", strUser)

                    If bizPic.DeletePicture(hsPicture) Then
                        '日志表插入
                        comBc.InsertLog("图片MS", OperateKind.DELETE, "", Me.Login.UserId, DateTime.Now)

                        MessageBox.Show(M00012I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        MessageBox.Show(M00019I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If

                    '图片部分
                    Me.picBox.Image = Nothing

                    Me.ltbxBefore.Visible = False
                    Me.ltbxAfter.Visible = False
                    Me.btnAdd.Visible = False
                    Me.btnDel.Visible = False
                    Me.btnAddAll.Visible = False
                    Me.btnDelAll.Visible = False
                End If

            End If
        End If
        '图片选择和双击Flg初始化
        picSelFlg = False
        rowDobClickFlg = False
        '删除成功后刷新
        btnSearch_Click(sender, e)
    End Sub

    '清空按钮按下
    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Me.txtGoodsCd.Text = String.Empty
        Me.txtPicId.Text = String.Empty
        For i As Integer = 0 To Me.chkLDepartment.Items.Count - 1
            chkLDepartment.SetItemCheckState(i, CheckState.Unchecked)
        Next
        Me.txtPicNm.Text = String.Empty
        Me.txtPicName.Text = String.Empty

        'Me.txtGoodsCdEdit.Text = String.Empty
        'Me.txtGoodsCdEdit.ReadOnly = False
        Me.txtPicIdEdit.Text = String.Empty
        'Me.txtPicIdEdit.ReadOnly = False
        Me.cbDepartment.SelectedIndex = 0
        Me.txtPicNameEdit.Text = String.Empty
        Me.txtPicPath.Text = String.Empty

        Me.lblCnt.Text = String.Empty

        Me.grdPictureData.DataSource = GenerateData()
        InitializeGridView()
        Me.picBox.Image = Nothing

        Me.ltbxBefore.Visible = False
        Me.ltbxAfter.Visible = False
        Me.btnAdd.Visible = False
        Me.btnDel.Visible = False
        Me.btnAddAll.Visible = False
        Me.btnDelAll.Visible = False

        Me.txtPicPath.Visible = True
        Me.btnPicture.Visible = True

        If headColorChange = True Then
            Me.grdPictureData.Columns("picture_id").HeaderCell.Style.BackColor = Color.White
            'Me.grdPictureData.Columns("goods_id").HeaderCell.Style.BackColor = Color.White
            Me.grdPictureData.Columns("department_mei").HeaderCell.Style.BackColor = Color.White
            Me.grdPictureData.Columns("picture_nm").HeaderCell.Style.BackColor = Color.White
            Me.grdPictureData.Columns("picture_name").HeaderCell.Style.BackColor = Color.White
            Me.grdPictureData.Columns("update_date").HeaderCell.Style.BackColor = Color.White
        End If

        picAddFlg = False

        '删除按钮不可用
        Me.btnDelete.Enabled = False

        strDepartmentChangeBefore = String.Empty
        strPicNameChangeBefore = String.Empty

        '更新Flg设为false
        updFlg = False

        '编辑区背景色恢复初期值
        txtPicNameEdit.BackColor = Color.White
        cbDepartment.BackColor = Color.White

        '图片选择和双击Flg初始化
        picSelFlg = False
        rowDobClickFlg = False

    End Sub

    '选择图片按钮按下
    Private Sub btnPicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPicture.Click
        Dim FileDialog As OpenFileDialog = New OpenFileDialog()
        FileDialog.Multiselect = False
        FileDialog.Title = "请选择文件"
        FileDialog.InitialDirectory = Me.txtPicPath.Text
        FileDialog.Filter = "画像文件|*.jpg"
        FileDialog.FilterIndex = 2
        FileDialog.RestoreDirectory = True
        If FileDialog.ShowDialog() = DialogResult.OK Then
            Dim file As String = FileDialog.FileName
            Me.picBox.ImageLocation = file
            Me.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.txtPicPath.Text = file

            If Me.grdPictureData.Rows.Count > 0 Then
                Me.ltbxBefore.Visible = True
                Me.ltbxAfter.Visible = True
                Me.btnAdd.Visible = True
                Me.btnDel.Visible = True
                Me.btnAddAll.Visible = True
                Me.btnDelAll.Visible = True
                Me.ltbxBefore.Items.Clear()
                Me.ltbxAfter.Items.Clear()
                Dim strGoodsCd As String = String.Empty
                Dim strPicId As String = String.Empty
                Dim dr As DataGridViewRow
                For Each dr In Me.grdPictureData.Rows
                    'If Me.txtPicIdEdit.Text = dr.Cells("picture_id").Value.ToString AndAlso strGoodsCd <> dr.Cells("goods_id").Value.ToString Then
                    '    strGoodsCd = dr.Cells("goods_id").Value.ToString
                    '    Me.ltbxBefore.Items.Add(strGoodsCd)
                    '    Me.ltbxBefore.Sorted = True
                    'End If
                    strPicId = strPicId & "'" & dr.Cells("picture_id").Value.ToString & "',"
                Next
                '对应的商品CD表示
                Dim dtGoods As DataTable

                dtGoods = bizPic.GetGoodsCdList(strPicId.TrimEnd(CChar(","))).Tables("Goods")

                '商品CD列表表示
                If dtGoods.Rows.Count > 0 Then
                    For Each dr1 As DataRow In dtGoods.Rows
                        strGoodsCd = dr1.Item(0).ToString
                        Me.ltbxBefore.Items.Add(strGoodsCd)
                        Me.ltbxBefore.Sorted = True
                        strGoodsCd = String.Empty
                    Next
                End If
            End If
            picAddFlg = True

            '图片选择
            picSelFlg = True

            '选择图片后变为新规
            updFlg = False

        End If
    End Sub

    'Gridview单击事
    Private Sub grdPictureData_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdPictureData.CellClick
        Try
            If e.RowIndex >= 0 Then
                If rowDobClickFlg = True AndAlso dbClickLineIndex <> e.RowIndex Then
                    updFlg = False
                    Me.txtPicIdEdit.Text = String.Empty
                    Me.cbDepartment.SelectedIndex = 0
                    Me.txtPicNameEdit.Text = String.Empty
                End If

                bizPic = New PictureBizLogic
                'If picSelFlg = False AndAlso rowDobClickFlg = False Then
                'If picSelFlg = False Then

                Dim bt() As Byte = DirectCast(bizPic.GetPictureById(Me.grdPictureData.SelectedRows.Item(0).Cells("picture_id").Value.ToString).Tables("pictureById").Rows(0).Item("picture_content"), Byte())
                Convert_byte_To_Image(bt)

                '对应的商品CD表示
                Dim strPictureId As String
                Dim dtGoods As DataTable
                Dim strGoodsCd As String = String.Empty

                Me.ltbxBefore.Items.Clear()

                strPictureId = Convert.ToString(Me.grdPictureData.Rows(e.RowIndex).Cells("picture_id").Value)
                dtGoods = bizPic.GetGoodsCdList("'" & strPictureId & "'").Tables("Goods")

                '商品CD列表表示
                If dtGoods.Rows.Count > 0 Then
                    Me.ltbxBefore.Visible = True
                    For Each dr As DataRow In dtGoods.Rows
                        strGoodsCd = dr.Item(0).ToString
                        Me.ltbxBefore.Items.Add(strGoodsCd)
                        Me.ltbxBefore.Sorted = True
                        strGoodsCd = String.Empty
                    Next
                Else
                    Me.ltbxBefore.Visible = False
                End If
                'End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Gridview双击事
    Private Sub grdPictureData_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdPictureData.CellDoubleClick
        If e.RowIndex >= 0 AndAlso Me.grdPictureData.SelectedRows.Count <> 0 Then
            dbClickLineIndex = e.RowIndex
            '初始化
            picAddFlg = False
            'Me.txtGoodsCdEdit.Text = String.Empty
            Me.txtPicNameEdit.Text = String.Empty
            Me.txtPicIdEdit.Text = String.Empty
            bizPic = New PictureBizLogic

            updFlg = True

            '赋值
            With Me.grdPictureData.SelectedRows.Item(0)
                'If .Cells("goods_id").Value IsNot Nothing Then
                '    Me.txtGoodsCdEdit.Text = .Cells("goods_id").Value.ToString
                'End If
                If .Cells("department_cd").Value IsNot Nothing Then
                    strDepartmentChangeBefore = .Cells("department_cd").Value.ToString
                    Me.cbDepartment.SelectedValue = .Cells("department_cd").Value.ToString
                End If
                If .Cells("picture_name").Value IsNot Nothing Then
                    strPicNameChangeBefore = .Cells("picture_name").Value.ToString
                    Me.txtPicNameEdit.Text = .Cells("picture_name").Value.ToString
                End If
                If .Cells("picture_id").Value IsNot Nothing Then
                    Me.txtPicIdEdit.Text = .Cells("picture_id").Value.ToString
                    Dim bt() As Byte = DirectCast(bizPic.GetPictureById(.Cells("picture_id").Value.ToString).Tables("pictureById").Rows(0).Item("picture_content"), Byte())
                    Convert_byte_To_Image(bt)
                End If
            End With

            Me.ltbxBefore.Visible = False
            Me.ltbxAfter.Visible = False
            Me.btnAdd.Visible = False
            Me.btnDel.Visible = False
            Me.btnAddAll.Visible = False
            Me.btnDelAll.Visible = False

            '清空按钮不可用
            Me.btnClear.Enabled = False
            '删除按钮可用
            Me.btnDelete.Enabled = True
            '图片选择部分
            If Me.grdPictureData.Rows.Count > 1 Then
                txtPicPath.Visible = False
                btnPicture.Visible = False
            Else
                txtPicPath.Visible = True
                btnPicture.Visible = True
            End If

            '行双击之后
            rowDobClickFlg = True
        End If
    End Sub

    '选择路径按钮按下
    Private Sub btnPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPath.Click
        Me.txtPath.Text = String.Empty
        '批量导入处理执行
        If Me.rdoImport.Checked = True Then
            Dim FileDialog As OpenFileDialog = New OpenFileDialog()
            FileDialog.InitialDirectory = Me.txtPath.Text
            FileDialog.Multiselect = False
            FileDialog.Title = "请选择文件"
            FileDialog.Filter = "Excel文件(2000-2003)|*.xls|Excel文件(2007以上)|*.xlsx"
            FileDialog.FilterIndex = 2
            FileDialog.RestoreDirectory = True
            If FileDialog.ShowDialog() = DialogResult.OK Then
                Dim file As String = FileDialog.FileName
                Me.txtPath.Text = file
            End If
        End If
        '批量导出处理执行
        If Me.rdoExport.Checked = True Then
            Dim dialog As FolderBrowserDialog = New FolderBrowserDialog()
            dialog.Description = "请选择文件路径"
            If dialog.ShowDialog() = DialogResult.OK Then
                Dim foldPath As String = dialog.SelectedPath
                Me.txtPath.Text = foldPath
            End If
        End If
    End Sub

    '执行按钮按下
    Private Sub btnExecute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecute.Click

        '批量导入处理执行
        If Me.rdoImport.Checked = True Then
            ImportExcel()
        End If

        '批量导出处理执行
        If Me.rdoExport.Checked = True Then
            ExportExcel()
        End If

    End Sub

    '图片MS-模板.xls link按下
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        'Dim dialog As FolderBrowserDialog = New FolderBrowserDialog()
        'dialog.Description = "请选择文件路径"
        'If dialog.ShowDialog() = DialogResult.OK Then
        '    Dim foldPath As String = dialog.SelectedPath
        '    Dim url As String = "\\dae434\大連情報システム部（ITIS共有）\開発資料\大連工場案件\20150002\04_開発成果物\jingl4\图片MS-模板.xlsx"
        '    Me.Download(url, foldPath)
        'End If
        Try

            Dim fileName As String
            ' Dim folderName As String
            Dim appPath As String
            Dim pathTemplate As String
            Dim saveFileDialog As New SaveFileDialog()

            appPath = Forms.Application.StartupPath

            'MsgBox("1" & appPath)

            'folderName = String.Empty
            'While folderName <> "Lixil.AvoidMissSystem.WinUI"
            '    appPath = appPath.Substring(0, appPath.LastIndexOf("\"))
            '    folderName = appPath.Substring(appPath.LastIndexOf("\") + 1)
            'End While

            'MsgBox("2" & appPath)

            'pathTemplate = GetConfig.GetConfigVal("ExcelFilePatch")

            'If pathTemplate = String.Empty Then
            '    MessageBox.Show(String.Format(MsgConst.M00010I, "配置"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            '    Exit Sub
            'End If

            'pathTemplate = pathTemplate & "/Picture.xls"

            pathTemplate = appPath & "/Template/Picture.xls"
            saveFileDialog.Filter = "下载模板(*.xls)|*.xls"
            saveFileDialog.FileName = "图片MS模版"
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

    '模板下载
    'Private Sub Download(ByVal url As String, ByVal Dir As String)
    '    Dim client As WebClient = New WebClient()
    '    Dim fileName As String = url.Substring(url.LastIndexOf("\") + 1)
    '    Dim Path As String = Dir & "\" & fileName
    '    If File.Exists(Path) Then
    '        MessageBox.Show(M00023I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '        Exit Sub
    '    Else
    '        Try
    '            client.DownloadFile(url, fileName)
    '            Dim fs As FileStream = New FileStream(fileName, FileMode.Open, FileAccess.Read)
    '            Dim r As BinaryReader = New BinaryReader(fs)
    '            Dim mbyte As Byte() = r.ReadBytes(CInt(fs.Length))
    '            Dim fstr As FileStream = New FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write)
    '            fstr.Write(mbyte, 0, CInt(fs.Length))
    '            fstr.Close()
    '            MessageBox.Show(M00011I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '            Exit Sub
    '        Catch ex As Exception
    '            MessageBox.Show(M00022I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '            Exit Sub
    '        End Try
    '    End If
    'End Sub

    ''' <summary>
    ''' 将图片转化成二进制
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function imageToByte(ByVal Imagepath As String) As Byte()
        If System.IO.File.Exists(Imagepath) Then '检查图片路径是否正确
            Dim fs As FileStream = New FileStream(Imagepath, FileMode.Open)
            Dim br As BinaryReader = New BinaryReader(fs)
            Dim imageByte() As Byte = br.ReadBytes(CInt(fs.Length))
            br.Close()
            fs.Close()
            Return imageByte
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' 将二进制图片转为图片
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Convert_byte_To_Image(ByVal imageByte() As Byte)
        Dim sm As MemoryStream = New MemoryStream(imageByte, True) '图片数组转为内存文件流
        Me.picBox.Image = Image.FromStream(DirectCast(sm, System.IO.Stream))
        Me.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        sm.Close()
    End Sub

    ''' <summary>
    ''' 输入检查
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InputCheck() As Boolean
        Try
            If Me.cbDepartment.SelectedValue.ToString = String.Empty Then
                MessageBox.Show(String.Format(M00002I, "部门"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.cbDepartment.Focus()
                Return False
            End If

            If String.IsNullOrEmpty(Me.txtPicNameEdit.Text) Then
                MessageBox.Show(String.Format(M00002I, "图片描述备注"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.txtPicNameEdit.Focus()
                Return False
            End If

            If Me.txtPicNameEdit.Text.Length > 200 Then
                MessageBox.Show(String.Format(M00003I, "图片描述备注"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.txtPicNameEdit.Focus()
                Return False
            End If

            If Me.grdPictureData.Rows.Count = 0 OrElse String.IsNullOrEmpty(Me.txtPicIdEdit.Text) Then
                If Me.picBox.ImageLocation = String.Empty Then
                    MessageBox.Show(String.Format(M00002I, "图片"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Me.btnPicture.Focus()
                    Return False
                End If
            End If

            If picAddFlg = True Then
                If Me.picBox.Image Is Nothing Then
                    MessageBox.Show(String.Format(M00002I, "图片"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Me.btnPicture.Focus()
                    Return False
                End If

                If Not System.IO.File.Exists(Me.picBox.ImageLocation) Then '检查图片路径是否正确
                    MessageBox.Show(String.Format(M00010I, "图片"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Me.btnPicture.Focus()
                    Return False
                End If
            End If

            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    ''' <summary>
    ''' 返回按钮事件
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
    ''' 退出按钮事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExist.Click
        Try
            NavigateToNextPage(Consts.PAGE.MS_LOGIN)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 添加按钮事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim item As String
        Dim arr As ArrayList = New ArrayList()
        For Each item In Me.ltbxBefore.SelectedItems
            arr.Add(item)
            Me.ltbxAfter.Items.Add(item)
            Me.ltbxAfter.Sorted = True
        Next

        For Each item In arr
            Me.ltbxBefore.Items.Remove(item)
            Me.ltbxBefore.Sorted = True
        Next
    End Sub

    ''' <summary>
    ''' 删除按钮事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDel.Click
        Dim item As String
        Dim arr As ArrayList = New ArrayList()
        For Each item In Me.ltbxAfter.SelectedItems
            arr.Add(item)
            Me.ltbxBefore.Items.Add(item)
            Me.ltbxBefore.Sorted = True
        Next

        For Each item In arr
            Me.ltbxAfter.Items.Remove(item)
            Me.ltbxAfter.Sorted = True
        Next
    End Sub

    ''' <summary>
    ''' 全添加按钮事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAll.Click
        Dim item As String
        Dim arr As ArrayList = New ArrayList()
        For Each item In Me.ltbxBefore.Items
            arr.Add(item)
            Me.ltbxAfter.Items.Add(item)
            Me.ltbxAfter.Sorted = True
        Next

        For Each item In arr
            Me.ltbxBefore.Items.Remove(item)
            Me.ltbxBefore.Sorted = True
        Next
    End Sub

    ''' <summary>
    ''' 全删除按钮事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAll.Click
        Dim item As String
        Dim arr As ArrayList = New ArrayList()
        For Each item In Me.ltbxAfter.Items
            arr.Add(item)
            Me.ltbxBefore.Items.Add(item)
            Me.ltbxBefore.Sorted = True
        Next

        For Each item In arr
            Me.ltbxAfter.Items.Remove(item)
            Me.ltbxAfter.Sorted = True
        Next
    End Sub

    '''' <summary>
    '''' 部门点击事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub cbDepartment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDepartment.Click
    '    Try
    '        strDepartmentChangeBefore = Me.cbDepartment.Text.ToString
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
    'Private Sub cbDepartment_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDepartment.Validated
    '    Try
    '        If strDepartmentChangeBefore = Me.cbDepartment.Text.ToString Then
    '            Exit Sub
    '        End If
    '        If Not String.IsNullOrEmpty(Me.cbDepartment.Text.ToString) AndAlso Me.grdPictureData.Rows.Count > 0 Then
    '            For i As Integer = 0 To Me.grdPictureData.Rows.Count - 1
    '                Me.grdPictureData.Rows(i).Cells("department_cd").Value = Me.cbDepartment.SelectedValue.ToString
    '                Me.grdPictureData.Rows(i).Cells("department_mei").Value = Me.cbDepartment.Text.ToString
    '            Next
    '            headColorChange = True
    '            Me.grdPictureData.Columns("department_mei").HeaderCell.Style.BackColor = Color.SkyBlue
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 图片描述备注获得焦点事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub txtPicNameEdit_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPicNameEdit.GotFocus
    '    Try
    '        strPicNameChangeBefore = Me.txtPicNameEdit.Text
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' 图片描述备注编辑事件
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub txtPicNameEdit_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPicNameEdit.Validated
    '    Try
    '        If strPicNameChangeBefore = Me.txtPicNameEdit.Text Then
    '            Exit Sub
    '        End If

    '        If Me.grdPictureData.Rows.Count > 0 Then
    '            For i As Integer = 0 To Me.grdPictureData.Rows.Count - 1
    '                Me.grdPictureData.Rows(i).Cells("picture_name").Value = Me.txtPicNameEdit.Text
    '            Next
    '            headColorChange = True
    '            Me.grdPictureData.Columns("picture_name").HeaderCell.Style.BackColor = Color.SkyBlue
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    ''' <summary>
    ''' 图片双击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub picBox_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picBox.DoubleClick
        ShowMaxPicture(Me.picBox.Image)
    End Sub

    '''' <summary>
    '''' 显示大图     
    '''' </summary>
    '''' <param name="picture"></param>
    '''' <remarks></remarks>
    'Private Sub ShowMaxPicture(ByVal picture As Image)
    '    Dim frmMaxImage As Form
    '    Dim maxPictureBox As PictureBox
    '    Dim frmName As String = "frmMaxPicture"

    '    Try
    '        For Each myForm As Form In Application.OpenForms
    '            If myForm.Name = frmName Then
    '                myForm.Close()
    '            End If
    '        Next
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try

    '    frmMaxImage = New Form '新建立一个form窗口
    '    maxPictureBox = New PictureBox '新建立一个图片显示控件
    '    frmMaxImage.Name = frmName
    '    With picture
    '        frmMaxImage.Width = .Width
    '        frmMaxImage.Height = .Height
    '        maxPictureBox.Width = .Width
    '        maxPictureBox.Height = .Height
    '    End With
    '    maxPictureBox.Image = picture '设置图片显示控件的图片，来自形参
    '    frmMaxImage.Controls.Add(maxPictureBox) '将图片控件加入到form窗口中
    '    frmMaxImage.Show() '显示form窗口
    'End Sub

    ''' <summary>
    ''' 'EXCEL文件导出处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExportExcel()

        Dim strFileName As String
        Dim saveFileDialog As SaveFileDialog

        saveFileDialog = New SaveFileDialog()
        'saveFileDialog.Filter = "导出Excel (*.xls)|*.xls"
        saveFileDialog.FilterIndex = 0
        saveFileDialog.RestoreDirectory = True
        saveFileDialog.Title = "导出文件保存"
        saveFileDialog.ShowDialog()
        strFileName = saveFileDialog.FileName
        If String.IsNullOrEmpty(strFileName) Then
            Return
        End If

        '取得DataSet
        Dim strDepartment As String = String.Empty

        Dim dtDepartment As DataTable = DirectCast(chkLDepartment.DataSource, DataTable)

        '选择的部门取得
        For i As Integer = 0 To Me.chkLDepartment.Items.Count - 1
            If chkLDepartment.GetItemChecked(i) = True Then
                'strKindCd = strKindCd & "'" & chkLKind.GetItemText(chkLKind.Items(i)) & "',"
                strDepartment = strDepartment & "'" & dtDepartment.Rows(i).Item("mei_cd").ToString() & "',"
            End If
        Next

        strDepartment = strDepartment.TrimEnd(CChar(","))

        '一览数据取得
        bizPic = New PictureBizLogic

        Dim saveFileName As String = strFileName & "\图片MS" & Now.ToString("yyyyMMddHHmmss") & ".xlsx"
        Dim savePictureName As String = strFileName & "\图片Images" & Now.ToString("yyyyMMddHHmmss")
        Dim fileSaved As Boolean = False

        '若文件夹不存在则新建文件夹   
        If Not System.IO.Directory.Exists(savePictureName) Then
            '新建文件夹  
            System.IO.Directory.CreateDirectory(savePictureName)
        End If
        '若文件存在
        If System.IO.File.Exists(saveFileName) Then
            '删除文件
            System.IO.File.Delete(saveFileName)
        End If


        Dim xlApp = CreateObject("Excel.Application")
        If xlApp Is Nothing Then
            MessageBox.Show(M00021I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If


        Dim workbooks = xlApp.Workbooks
        Dim workbook = workbooks.Add(-4167)
        Dim worksheet = Nothing

        For dtIdx As Integer = 1 To 500

            Dim dt As DataTable = bizPic.ExportPictureData(Me.txtGoodsCd.Text, strDepartment, Me.txtPicId.Text, Me.txtPicNm.Text, Me.txtPicName.Text, dtIdx).Tables("picture")

            If dt.Rows.Count < 1 AndAlso dtIdx = 1 Then
                MessageBox.Show("对象数据不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            ElseIf dt.Rows.Count < 1 Then
                Exit For
            End If

            worksheet = workbook.Worksheets.Add()
            worksheet.Name = "检查项目信息" & IIf(dtIdx <> 1, dtIdx.ToString, "").ToString

            '= DirectCast(workbook.Worksheets(1), Excel.Worksheet)
            worksheet.Name = "图片MS" & IIf(dtIdx <> 1, dtIdx.ToString, "").ToString
            Dim range

            '列索引，行索引，总列数，总行数          
            Dim colIndex As Integer = 0
            Dim rowIndex As Integer = 0
            Dim colCount As Integer = dt.Columns.Count
            Dim rowCount As Integer = dt.Rows.Count

            '写入图片
            Dim i As Integer
            Dim picId As String = String.Empty
            Dim picName As String = String.Empty
            For i = 0 To rowCount - 1
                If picId <> dt.Rows(i).Item("picture_id").ToString Then
                    Dim imageByte() As Byte = DirectCast(dt.Rows(i).Item("picture_content"), Byte())
                    Dim sm As MemoryStream = New MemoryStream(imageByte, True)
                    Dim px As New PictureBox
                    px.Image = Image.FromStream(DirectCast(sm, System.IO.Stream))
                    picName = dt.Rows(i).Item("picture_nm").ToString & ".jpg"
                    px.Image.Save(savePictureName & "\" & picName, px.Image.RawFormat)
                    sm.Close()
                    picId = dt.Rows(i).Item("picture_id").ToString
                End If
                'dt.Rows(i).Item("picture_mei") = picName
            Next


            '创建缓存数据
            Dim objData(rowCount + 1, colCount) As Object

            '获取具体数据
            For rowIndex = 0 To rowCount
                If rowIndex = 0 Then
                    objData(rowIndex, 0) = "图片ID"
                    'objData(rowIndex, 1) = "商品CD"
                    objData(rowIndex, 1) = "图片名称"
                    objData(rowIndex, 2) = "部门CD"
                    objData(rowIndex, 3) = "图片描述备注"
                Else
                    Dim j As Integer
                    colIndex = 0
                    For j = 0 To colCount - 1
                        If j <> 1 AndAlso j <> 2 AndAlso j <> 6 AndAlso j <> 7 AndAlso j <> 8 Then
                            objData(rowIndex, colIndex) = "'" & dt.Rows(rowIndex - 1)(j).ToString
                            colIndex = colIndex + 1
                        End If
                    Next
                End If
            Next


            '写入Excel
            range = worksheet.Range(xlApp.Cells(1, 1), xlApp.Cells(rowCount + 1, colCount))
            range.Value2 = objData
            Application.DoEvents()

        Next


        workbook.Saved = True
        workbook.SaveCopyAs(saveFileName)
        fileSaved = True

        System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet)
        worksheet = Nothing
        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook)
        workbook = Nothing
        workbooks.Close()
        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks)
        workbooks = Nothing
        xlApp.Quit()
        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)
        xlApp = Nothing

        If fileSaved AndAlso File.Exists(saveFileName) Then
            System.Diagnostics.Process.Start(saveFileName)
        End If


        Try

            MessageBox.Show(M00004I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception
            fileSaved = False
            '若文件存在
            If System.IO.File.Exists(saveFileName) Then
                '删除文件
                System.IO.File.Delete(saveFileName)
            End If
            '若文件夹存在
            If System.IO.Directory.Exists(savePictureName) Then
                '删除文件夹
                System.IO.Directory.Delete(savePictureName)
            End If
            MessageBox.Show(M00020I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
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
        Dim dicGridHeader As Dictionary(Of String, String)
        Dim hstb As New Hashtable
        Dim strSheetNames As String()
        Dim strExportFPath As String
        Dim endTime As DateTime

        dicGridHeader = New Dictionary(Of String, String)
        dicGridHeader.Add("picture_id", "图片ID")
        dicGridHeader.Add("picture_nm", "图片名称")
        dicGridHeader.Add("department_cd", "部门CD")
        dicGridHeader.Add("picture_name", "图片描述备注")

        Try
            strFilePath = Me.txtPath.Text.Trim
            If String.IsNullOrEmpty(strFilePath) Then
                MessageBox.Show(M00006I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            Else
                strFileType = System.IO.Path.GetExtension(strFilePath)
                If strFileType <> ".xls" AndAlso strFileType <> ".xlsx" Then
                    MessageBox.Show(M00007I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
            End If

            Me.Cursor = Cursors.WaitCursor
            If File.Exists(strFilePath) Then
                '[图片MS]sheet存在检查
                strSheetNames = LogExport.GetAllSheetName(strFilePath)
                For i As Integer = 0 To strSheetNames.Length - 1
                    If strSheetNames(i) = "图片MS$" Then
                        Exit For
                    End If
                    If i = strSheetNames.Length - 1 Then
                        MessageBox.Show("EXCEL文件中不存在[图片MS]Sheet页！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                Next

                '读取EXCEL数据
                dtExcel = GetExcelData("图片MS", strFilePath, strFileType)

                If dtExcel.Rows.Count < 1 Then
                    Me.Cursor = Cursors.Arrow
                    MessageBox.Show(M00008I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If

                dtImport = dtExcel.Clone()
                dtError = dtExcel.Clone()
                dcMsg = New DataColumn("错误信息", GetType(String))
                dtError.Columns.Remove("图片ID")
                dtError.Columns.Add(dcMsg)
                'EXCEL数据项目校验
                If dtExcel.Columns.Count <> dicGridHeader.Count Then
                    Me.Cursor = Cursors.Arrow
                    MessageBox.Show(M00009I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If

                For Each dcExcel As DataColumn In dtExcel.Columns
                    If Not dicGridHeader.ContainsValue(dcExcel.ColumnName) Then
                        Me.Cursor = Cursors.Arrow
                        MessageBox.Show(M00009I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                Next

                '更新前后数据保存用Datatable初始化
                InitTable()
                '新规图片信息导出用Datatable
                Dim dt As DataTable = New DataTable
                dt.Columns.Add("图片ID", System.Type.GetType("System.String"))
                dt.Columns.Add("图片名称", System.Type.GetType("System.String"))
                dt.Columns.Add("部门CD", System.Type.GetType("System.String"))
                dt.Columns.Add("图片描述备注", System.Type.GetType("System.String"))

                Dim tmpDt As New DataTable
                tmpDt = dtExcel.Clone
                tmpDt.Columns("图片名称").DataType = GetType(System.String)

                For kk As Integer = 0 To dtExcel.Rows.Count - 1

                    tmpDt.Rows.Add(dtExcel.Rows(kk).ItemArray)
                Next

                dtExcel = tmpDt

                dtExcel.Columns("图片名称").DataType = GetType(System.String)






                For Each drExcel As DataRow In dtExcel.Rows

                    drExcel("图片名称") = drExcel("图片名称").ToString & ".jpg"

                    'EXCEL数据校验
                    result = ImportCheck(drExcel, strFilePath)
                    If result.Success Then
                        drImport = dtImport.NewRow()
                        If drExcel("图片名称").ToString.IndexOf(".jpg") < 0 AndAlso _
                            drExcel("图片名称").ToString.IndexOf(".JPG") < 0 Then
                            drImport("图片ID") = drExcel("图片名称")
                            drImport("图片名称") = drExcel("图片名称")
                            drImport("部门CD") = drExcel("部门CD")
                            drImport("图片描述备注") = drExcel("图片描述备注")
                            hstb.Add(drExcel("图片名称").ToString, String.Empty)
                            dtImport.Rows.Add(drImport)
                        Else
                            drExcel("图片名称") = Mid(drExcel("图片名称").ToString, 1, drExcel("图片名称").ToString.Length - 4)
                            Dim bizIndex As IndexBizLogic = New IndexBizLogic
                            Dim strNewId As String = bizIndex.GetIndex(TYPEKBN.PICTURE)
                            Select Case strNewId
                                Case DBErro.InserError
                                    result.Message = String.Format(M00002D.Replace("{0}", "产品种类表采番"))
                                    result.Success = False
                                    drError = dtError.NewRow()
                                    drError("图片名称") = drExcel("图片名称")
                                    drError("部门CD") = drExcel("部门CD")
                                    drError("图片描述备注") = drExcel("图片描述备注")
                                    drError("错误信息") = result.Message
                                    dtError.Rows.Add(drError)
                                Case DBErro.UpdateError
                                    result.Message = String.Format(M00003D.Replace("{0}", "产品种类表采番"))
                                    result.Success = False
                                    drError = dtError.NewRow()
                                    drError("图片名称") = drExcel("图片名称")
                                    drError("部门CD") = drExcel("部门CD")
                                    drError("图片描述备注") = drExcel("图片描述备注")
                                    drError("错误信息") = result.Message
                                    dtError.Rows.Add(drError)
                                Case DBErro.GetIndexError
                                    result.Message = String.Format(M00001D.Replace("{0}", "产品种类表"))
                                    result.Success = False
                                    drError = dtError.NewRow()
                                    drError("图片名称") = drExcel("图片名称")
                                    drError("部门CD") = drExcel("部门CD")
                                    drError("图片描述备注") = drExcel("图片描述备注")
                                    drError("错误信息") = result.Message
                                    dtError.Rows.Add(drError)
                                Case DBErro.GetIndexMaxError
                                    result.Message = String.Format(M00005D.Replace("{0}", "产品种类表"))
                                    result.Success = False
                                    drError = dtError.NewRow()
                                    drError("图片名称") = drExcel("图片名称")
                                    drError("部门CD") = drExcel("部门CD")
                                    drError("图片描述备注") = drExcel("图片描述备注")
                                    drError("错误信息") = result.Message
                                    dtError.Rows.Add(drError)
                                Case Else
                                    drImport("图片ID") = strNewId
                                    drImport("图片名称") = drExcel("图片名称")
                                    drImport("部门CD") = drExcel("部门CD")
                                    drImport("图片描述备注") = drExcel("图片描述备注")
                                    hstb.Add(strNewId, imageToByte(Mid(strFilePath, 1, strFilePath.LastIndexOf("\")) & "\图片Images\" & drExcel("图片名称").ToString & ".jpg"))
                                    dtImport.Rows.Add(drImport)

                                    '新规的图片信息导出Datatable作成
                                    Dim dr As DataRow = dt.NewRow
                                    dr.Item("图片ID") = strNewId
                                    dr.Item("图片名称") = drExcel("图片名称").ToString
                                    dr.Item("部门CD") = drExcel("部门CD")
                                    dr.Item("图片描述备注") = drExcel("图片描述备注")
                                    dt.Rows.Add(dr)
                            End Select
                        End If
                    Else
                        drError = dtError.NewRow()
                        drError("图片名称") = drExcel("图片名称")
                        drError("部门CD") = drExcel("部门CD")
                        drError("图片描述备注") = drExcel("图片描述备注")
                        drError("错误信息") = result.Message
                        dtError.Rows.Add(drError)
                    End If
                Next

                'EXCEL数据导入
                bizPic = New PictureBizLogic
                If bizPic.InsOrUpdPicture(dtImport, strUser, hstb, afterUpdDt) Then
                    MessageBox.Show(String.Format(M00015I, dtExcel.Rows.Count.ToString, dtImport.Rows.Count.ToString, dtError.Rows.Count.ToString), _
                                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    '结束时间
                    endTime = DateTime.Now

                    '错误数据及更新信息导出
                    strExportFPath = LogExport.LogExport("图片MS", dtError, beforeUpdDt, afterUpdDt, endTime)
                    '日志表插入
                    comBc.InsertLog("图片MS", OperateKind.IMPORT, strExportFPath, Me.Login.UserId, endTime)

                    If dtError.Rows.Count > 0 AndAlso MessageBox.Show(M00003C, "確認", _
                       MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                        frmErrorData = New ErrorDataForm()

                        frmErrorData.Enabled = True
                        frmErrorData.ErrorData = dtError
                        Me.Cursor = Cursors.Arrow
                        frmErrorData.Show()
                    End If

                    '新规图片信息导出处理执行
                    If dt.Rows.Count > 0 Then

                        Dim fileSaved As Boolean = False
                        Dim xlApp = CreateObject("Excel.Application")
                        If xlApp Is Nothing Then
                            MessageBox.Show(M00021I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                        Dim workbooks = xlApp.Workbooks
                        Dim workbook = workbooks.Add(-4167)
                        Dim worksheet = workbook.Worksheets(1)
                        worksheet.Name = "图片导入后信息"
                        Dim range

                        '列索引，行索引，总列数，总行数          
                        Dim colIndex As Integer = 0
                        Dim rowIndex As Integer = 0
                        Dim colCount As Integer = dt.Columns.Count
                        Dim rowCount As Integer = dt.Rows.Count

                        '创建缓存数据
                        Dim objData(rowCount + 1, colCount) As Object

                        '获取具体数据
                        For rowIndex = 0 To rowCount
                            If rowIndex = 0 Then
                                objData(rowIndex, 0) = "图片ID"
                                objData(rowIndex, 1) = "图片名称"
                                objData(rowIndex, 2) = "部门CD"
                                objData(rowIndex, 3) = "图片描述备注"
                            Else
                                Dim j As Integer
                                colIndex = 0
                                For j = 0 To colCount - 1
                                    objData(rowIndex, colIndex) = "'" & dt.Rows(rowIndex - 1)(j).ToString
                                    colIndex = colIndex + 1
                                Next
                            End If
                        Next

                        '写入Excel
                        range = worksheet.Range(xlApp.Cells(1, 1), xlApp.Cells(rowCount + 1, colCount))
                        range.Value2 = objData
                        Application.DoEvents()

                        'Dim pathTemplate As String = GetConfig.GetConfigVal("ExcelFilePatch")
                        Dim fm As String = Mid(Me.txtPath.Text, 1, Me.txtPath.Text.LastIndexOf("\")) & "\図ID分配" & Now.ToString("yyyyMMddHHmmss") & ".xls"
                        workbook.Saved = True
                        workbook.SaveCopyAs(fm)
                        fileSaved = True

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet)
                        worksheet = Nothing
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook)
                        workbook = Nothing
                        workbooks.Close()
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks)
                        workbooks = Nothing
                        xlApp.Quit()
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)
                        xlApp = Nothing

                        If fileSaved AndAlso File.Exists(fm) Then
                            System.Diagnostics.Process.Start(fm)
                        End If

                    End If

                Else
                    MessageBox.Show(M00014I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Me.Cursor = Cursors.Arrow
                    Exit Sub
                End If
            Else
                MessageBox.Show(String.Format(M00010I, "批量导入"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
    ''' 批量导入数据检查
    ''' </summary>
    ''' <param name="drExcel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ImportCheck(ByVal drExcel As DataRow, ByVal path As String) As Result
        Try
            Dim strPictureId As String
            Dim strDepartmentCd As String
            Dim strPictureName As String
            Dim result As New Result

            strPictureId = drExcel("图片名称").ToString
            strDepartmentCd = drExcel("部门CD").ToString
            strPictureName = drExcel("图片描述备注").ToString

            '图片ID输入校验
            If String.IsNullOrEmpty(strPictureId) Then
                result.Message = String.Format(M00002I, "图片ID")
                result.Success = False
                Return result
            Else
                If strPictureId.IndexOf(".jpg") >= 0 OrElse _
                    strPictureId.IndexOf(".JPG") >= 0 Then
                    Dim imagesPath As String = Mid(path, 1, path.LastIndexOf("\")) & "\图片Images\"
                    '若图片文件夹不存在
                    If System.IO.Directory.Exists(imagesPath) = False Then
                        result.Message = String.Format(M00018I, "图片文件夹")
                        result.Success = False
                        Return result
                    End If
                    '若图片不存在
                    If System.IO.File.Exists(imagesPath & strPictureId) = False Then
                        result.Message = String.Format(M00018I, "图片")
                        result.Success = False
                        Return result
                    End If
                Else
                    '图片ID存在检查
                    Dim dtPic As New DataTable
                    dtPic = bizPic.GetPictureId(strPictureId)
                    If dtPic.Rows.Count = 0 Then
                        result.Message = String.Format(M00018I, "图片ID")
                        result.Success = False
                        Return result
                    Else
                        '更新的时候，更新前的数据保存到Datatable中
                        beforeUpdDt.Rows.Add(dtPic.Rows(0).ItemArray)
                    End If
                End If
            End If

            '部门CD输入校验
            If String.IsNullOrEmpty(strDepartmentCd) Then
                result.Message = String.Format(M00002I, "部门CD")
                result.Success = False
                Return result
            End If
            '部门CD正确校验
            If strDepartmentCd <> "001" AndAlso strDepartmentCd <> "002" AndAlso strDepartmentCd <> "003" AndAlso strDepartmentCd <> "004" Then
                result.Message = String.Format(M00041I, "部门CD")
                result.Success = False
                Return result
            End If

            '图片描述备注长度校验
            If strPictureName.Length > 200 Then
                result.Message = String.Format(M00003I, "图片描述备注")
                result.Success = False
                Return result
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
    ''' <param name="strFileType">EXCEL扩展名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetExcelData(ByVal strSheetName As String, _
                                  ByVal strExcelFile As String, _
                                  ByVal strFileType As String) As DataTable

        Dim strConn As String = LogExport.GetOleDbConn(strExcelFile)
        Dim strExcel As String
        Dim ds As DataSet
        Dim adapter As OleDb.OleDbDataAdapter = Nothing
        Dim conn As OleDb.OleDbConnection = Nothing
        'Dim xlApp = CreateObject("Excel.Application")

        Try
            'strFileType = System.IO.Path.GetExtension(strExcelFile)
            '源的定义
            'If strFileType = ".xls" Then
            '    strConn = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source={0};" + "Extended Properties='Excel 8.0;HDR=YES;IMEX=1';", strExcelFile)
            'Else
            '    strConn = String.Format("Provider=Microsoft.ACE.OLEDB." & xlApp.Version & ";" + "Data Source={0};" + "Extended Properties='Excel " & xlApp.Version & ";HDR=YES;IMEX=1';", strExcelFile)
            'End If

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

        If strDepartmentChangeBefore <> Me.cbDepartment.SelectedValue.ToString Then
            dicChange.Add(PDEPARTMENT, Me.cbDepartment.SelectedValue.ToString)
        End If

        If strPicNameChangeBefore <> Me.txtPicNameEdit.Text Then
            dicChange.Add(PREMARKS, Me.txtPicNameEdit.Text.Trim)
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
        beforeUpdDt.Columns.Add("图片ID", GetType(String))
        beforeUpdDt.Columns.Add("图片名称", GetType(String))
        beforeUpdDt.Columns.Add("部门CD", GetType(String))
        beforeUpdDt.Columns.Add("图片描述备注", GetType(String))

        afterUpdDt = New DataTable
        afterUpdDt.TableName = "AfterUpdate"
        afterUpdDt.Columns.Add("图片ID", GetType(String))
        afterUpdDt.Columns.Add("图片名称", GetType(String))
        afterUpdDt.Columns.Add("部门CD", GetType(String))
        afterUpdDt.Columns.Add("图片描述备注", GetType(String))
    End Sub

#Region "背景色变化处理"

    ''' <summary>
    ''' 部门变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cbDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbDepartment.SelectedIndexChanged
        If updFlg = True AndAlso Me.cbDepartment.SelectedValue.ToString <> strDepartmentChangeBefore Then
            cbDepartment.BackColor = Color.LightPink
        Else
            cbDepartment.BackColor = Color.White
        End If
    End Sub

    ''' <summary>
    ''' 图片描述变更背景色变化处理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtPicNameEdit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPicNameEdit.TextChanged
        If updFlg = True AndAlso Me.txtPicNameEdit.Text <> strPicNameChangeBefore Then
            txtPicNameEdit.BackColor = Color.LightPink
        Else
            txtPicNameEdit.BackColor = Color.White
        End If
    End Sub

#End Region

#Region "类"
    ''' <summary>
    ''' Result
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

#Region "其他"
    ''' <summary>
    ''' 点击一下就能选中或者取消选中（默认模式是先获得焦点，然后才能设置选中，需要点击两次）
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub chkLDepartment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLDepartment.Click
        Dim chk As ColorCodedCheckedListBox = DirectCast(sender, ColorCodedCheckedListBox)
        chk.SetSelected(chk.SelectedIndex, True)
    End Sub
#End Region

End Class
