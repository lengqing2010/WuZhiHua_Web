<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MsMaintenancePictureForm
    Inherits AvoidMissSystem.WinUI.BaseForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.grdPictureData = New System.Windows.Forms.DataGridView
        Me.txtPicPath = New System.Windows.Forms.TextBox
        Me.cbDepartment = New System.Windows.Forms.ComboBox
        Me.btnPicture = New System.Windows.Forms.Button
        Me.txtPicName = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtPicId = New System.Windows.Forms.TextBox
        Me.txtGoodsCd = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnClear = New System.Windows.Forms.Button
        Me.btnEdit = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.Label7 = New System.Windows.Forms.Label
        Me.btnExecute = New System.Windows.Forms.Button
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.btnPath = New System.Windows.Forms.Button
        Me.rdoExport = New System.Windows.Forms.RadioButton
        Me.rdoImport = New System.Windows.Forms.RadioButton
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.btnReturn = New System.Windows.Forms.Button
        Me.btnExist = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtPicNm = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.chkLDepartment = New Lixil.AvoidMissSystem.WinUI.Common.ColorCodedCheckedListBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lblCnt = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtPicIdEdit = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtPicNameEdit = New System.Windows.Forms.TextBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.picBox = New System.Windows.Forms.PictureBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.btnDelAll = New System.Windows.Forms.Button
        Me.btnAddAll = New System.Windows.Forms.Button
        Me.ltbxAfter = New System.Windows.Forms.ListBox
        Me.btnDel = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.ltbxBefore = New System.Windows.Forms.ListBox
        Me.Panel1.SuspendLayout()
        CType(Me.grdPictureData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.picBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.grdPictureData)
        Me.Panel1.Location = New System.Drawing.Point(12, 186)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(868, 187)
        Me.Panel1.TabIndex = 4
        '
        'grdPictureData
        '
        Me.grdPictureData.AllowUserToAddRows = False
        Me.grdPictureData.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdPictureData.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.grdPictureData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdPictureData.Location = New System.Drawing.Point(3, 3)
        Me.grdPictureData.MultiSelect = False
        Me.grdPictureData.Name = "grdPictureData"
        Me.grdPictureData.ReadOnly = True
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdPictureData.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.grdPictureData.RowHeadersVisible = False
        Me.grdPictureData.RowTemplate.Height = 21
        Me.grdPictureData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdPictureData.Size = New System.Drawing.Size(760, 183)
        Me.grdPictureData.TabIndex = 14
        '
        'txtPicPath
        '
        Me.txtPicPath.Location = New System.Drawing.Point(93, 64)
        Me.txtPicPath.Name = "txtPicPath"
        Me.txtPicPath.Size = New System.Drawing.Size(359, 19)
        Me.txtPicPath.TabIndex = 12
        '
        'cbDepartment
        '
        Me.cbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDepartment.FormattingEnabled = True
        Me.cbDepartment.Location = New System.Drawing.Point(320, 14)
        Me.cbDepartment.Name = "cbDepartment"
        Me.cbDepartment.Size = New System.Drawing.Size(132, 20)
        Me.cbDepartment.TabIndex = 10
        '
        'btnPicture
        '
        Me.btnPicture.Location = New System.Drawing.Point(465, 64)
        Me.btnPicture.Name = "btnPicture"
        Me.btnPicture.Size = New System.Drawing.Size(78, 21)
        Me.btnPicture.TabIndex = 13
        Me.btnPicture.Text = "图片选择"
        Me.btnPicture.UseVisualStyleBackColor = True
        '
        'txtPicName
        '
        Me.txtPicName.Location = New System.Drawing.Point(320, 39)
        Me.txtPicName.Name = "txtPicName"
        Me.txtPicName.Size = New System.Drawing.Size(132, 19)
        Me.txtPicName.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(229, 42)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(83, 12)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "图片描述备注："
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(272, 42)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(0, 12)
        Me.Label4.TabIndex = 10
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(487, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 12)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "部门："
        '
        'txtPicId
        '
        Me.txtPicId.Location = New System.Drawing.Point(320, 12)
        Me.txtPicId.Name = "txtPicId"
        Me.txtPicId.Size = New System.Drawing.Size(132, 19)
        Me.txtPicId.TabIndex = 7
        '
        'txtGoodsCd
        '
        Me.txtGoodsCd.Location = New System.Drawing.Point(93, 12)
        Me.txtGoodsCd.Name = "txtGoodsCd"
        Me.txtGoodsCd.Size = New System.Drawing.Size(122, 19)
        Me.txtGoodsCd.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(229, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 12)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "图片ID："
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 12)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "商品CD："
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(531, 7)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 30)
        Me.btnClear.TabIndex = 3
        Me.btnClear.Text = "清空"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(371, 7)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(75, 30)
        Me.btnEdit.TabIndex = 1
        Me.btnEdit.Text = "保存"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Location = New System.Drawing.Point(451, 7)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 30)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "删除"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(291, 7)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 30)
        Me.btnSearch.TabIndex = 0
        Me.btnSearch.Text = "查询"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.LinkLabel1)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.btnExecute)
        Me.Panel3.Controls.Add(Me.txtPath)
        Me.Panel3.Controls.Add(Me.btnPath)
        Me.Panel3.Controls.Add(Me.rdoExport)
        Me.Panel3.Controls.Add(Me.rdoImport)
        Me.Panel3.Location = New System.Drawing.Point(10, 557)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(870, 70)
        Me.Panel3.TabIndex = 6
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(560, 15)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(98, 12)
        Me.LinkLabel1.TabIndex = 6
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "图片MS-模板.xlsx"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(463, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(94, 12)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "EXCEL模板下载："
        '
        'btnExecute
        '
        Me.btnExecute.Location = New System.Drawing.Point(675, 10)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(75, 21)
        Me.btnExecute.TabIndex = 19
        Me.btnExecute.Text = "执行"
        Me.btnExecute.UseVisualStyleBackColor = True
        '
        'txtPath
        '
        Me.txtPath.BackColor = System.Drawing.SystemColors.Window
        Me.txtPath.Location = New System.Drawing.Point(102, 39)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(648, 19)
        Me.txtPath.TabIndex = 18
        '
        'btnPath
        '
        Me.btnPath.Location = New System.Drawing.Point(16, 37)
        Me.btnPath.Name = "btnPath"
        Me.btnPath.Size = New System.Drawing.Size(75, 21)
        Me.btnPath.TabIndex = 17
        Me.btnPath.Text = "选择路径"
        Me.btnPath.UseVisualStyleBackColor = True
        '
        'rdoExport
        '
        Me.rdoExport.AutoSize = True
        Me.rdoExport.Location = New System.Drawing.Point(102, 13)
        Me.rdoExport.Name = "rdoExport"
        Me.rdoExport.Size = New System.Drawing.Size(71, 16)
        Me.rdoExport.TabIndex = 16
        Me.rdoExport.TabStop = True
        Me.rdoExport.Text = "批量导出"
        Me.rdoExport.UseVisualStyleBackColor = True
        '
        'rdoImport
        '
        Me.rdoImport.AutoSize = True
        Me.rdoImport.Checked = True
        Me.rdoImport.Location = New System.Drawing.Point(16, 13)
        Me.rdoImport.Name = "rdoImport"
        Me.rdoImport.Size = New System.Drawing.Size(71, 16)
        Me.rdoImport.TabIndex = 15
        Me.rdoImport.TabStop = True
        Me.rdoImport.Text = "批量导入"
        Me.rdoImport.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.btnReturn)
        Me.Panel4.Controls.Add(Me.btnExist)
        Me.Panel4.Controls.Add(Me.btnSearch)
        Me.Panel4.Controls.Add(Me.btnEdit)
        Me.Panel4.Controls.Add(Me.btnDelete)
        Me.Panel4.Controls.Add(Me.btnClear)
        Me.Panel4.Location = New System.Drawing.Point(12, 2)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(871, 42)
        Me.Panel4.TabIndex = 1
        '
        'btnReturn
        '
        Me.btnReturn.Location = New System.Drawing.Point(611, 7)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(75, 30)
        Me.btnReturn.TabIndex = 4
        Me.btnReturn.Text = "返回"
        Me.btnReturn.UseVisualStyleBackColor = True
        '
        'btnExist
        '
        Me.btnExist.Location = New System.Drawing.Point(691, 7)
        Me.btnExist.Name = "btnExist"
        Me.btnExist.Size = New System.Drawing.Size(75, 30)
        Me.btnExist.TabIndex = 5
        Me.btnExist.Text = "退 出"
        Me.btnExist.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtPicNm)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.chkLDepartment)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtPicId)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.txtGoodsCd)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtPicName)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 38)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(870, 62)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "查询"
        '
        'txtPicNm
        '
        Me.txtPicNm.Location = New System.Drawing.Point(93, 39)
        Me.txtPicNm.Name = "txtPicNm"
        Me.txtPicNm.Size = New System.Drawing.Size(122, 19)
        Me.txtPicNm.TabIndex = 14
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 42)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(59, 12)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "图片名称："
        '
        'chkLDepartment
        '
        Me.chkLDepartment.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.chkLDepartment.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.chkLDepartment.ColumnWidth = 75
        Me.chkLDepartment.FormattingEnabled = True
        Me.chkLDepartment.Location = New System.Drawing.Point(548, 12)
        Me.chkLDepartment.Name = "chkLDepartment"
        Me.chkLDepartment.Size = New System.Drawing.Size(113, 42)
        Me.chkLDepartment.TabIndex = 8
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblCnt)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.txtPicIdEdit)
        Me.GroupBox2.Controls.Add(Me.txtPicPath)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.cbDepartment)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.btnPicture)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.txtPicNameEdit)
        Me.GroupBox2.Location = New System.Drawing.Point(10, 99)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(870, 86)
        Me.GroupBox2.TabIndex = 25
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "编辑"
        '
        'lblCnt
        '
        Me.lblCnt.AutoSize = True
        Me.lblCnt.Location = New System.Drawing.Point(627, 66)
        Me.lblCnt.Name = "lblCnt"
        Me.lblCnt.Size = New System.Drawing.Size(0, 12)
        Me.lblCnt.TabIndex = 30
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(229, 18)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(35, 12)
        Me.Label8.TabIndex = 27
        Me.Label8.Text = "部门："
        '
        'txtPicIdEdit
        '
        Me.txtPicIdEdit.Location = New System.Drawing.Point(93, 17)
        Me.txtPicIdEdit.Name = "txtPicIdEdit"
        Me.txtPicIdEdit.ReadOnly = True
        Me.txtPicIdEdit.Size = New System.Drawing.Size(122, 19)
        Me.txtPicIdEdit.TabIndex = 26
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(272, 42)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(0, 12)
        Me.Label9.TabIndex = 28
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(8, 47)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(83, 12)
        Me.Label10.TabIndex = 29
        Me.Label10.Text = "图片描述备注："
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(8, 18)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(46, 12)
        Me.Label11.TabIndex = 24
        Me.Label11.Text = "图片ID："
        '
        'txtPicNameEdit
        '
        Me.txtPicNameEdit.Location = New System.Drawing.Point(93, 40)
        Me.txtPicNameEdit.Name = "txtPicNameEdit"
        Me.txtPicNameEdit.Size = New System.Drawing.Size(359, 19)
        Me.txtPicNameEdit.TabIndex = 11
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.picBox)
        Me.GroupBox3.Location = New System.Drawing.Point(10, 375)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(407, 180)
        Me.GroupBox3.TabIndex = 5
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "图片显示"
        '
        'picBox
        '
        Me.picBox.Location = New System.Drawing.Point(11, 18)
        Me.picBox.Name = "picBox"
        Me.picBox.Size = New System.Drawing.Size(278, 150)
        Me.picBox.TabIndex = 0
        Me.picBox.TabStop = False
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.btnDelAll)
        Me.GroupBox4.Controls.Add(Me.btnAddAll)
        Me.GroupBox4.Controls.Add(Me.ltbxAfter)
        Me.GroupBox4.Controls.Add(Me.btnDel)
        Me.GroupBox4.Controls.Add(Me.btnAdd)
        Me.GroupBox4.Controls.Add(Me.ltbxBefore)
        Me.GroupBox4.Location = New System.Drawing.Point(383, 375)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(497, 180)
        Me.GroupBox4.TabIndex = 6
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "商品CD列表(更新用)"
        '
        'btnDelAll
        '
        Me.btnDelAll.Location = New System.Drawing.Point(149, 146)
        Me.btnDelAll.Name = "btnDelAll"
        Me.btnDelAll.Size = New System.Drawing.Size(89, 21)
        Me.btnDelAll.TabIndex = 23
        Me.btnDelAll.Text = "＜－－全删除"
        Me.btnDelAll.UseVisualStyleBackColor = True
        Me.btnDelAll.Visible = False
        '
        'btnAddAll
        '
        Me.btnAddAll.Location = New System.Drawing.Point(149, 27)
        Me.btnAddAll.Name = "btnAddAll"
        Me.btnAddAll.Size = New System.Drawing.Size(89, 21)
        Me.btnAddAll.TabIndex = 22
        Me.btnAddAll.Text = "全添加－－＞"
        Me.btnAddAll.UseVisualStyleBackColor = True
        Me.btnAddAll.Visible = False
        '
        'ltbxAfter
        '
        Me.ltbxAfter.FormattingEnabled = True
        Me.ltbxAfter.ItemHeight = 12
        Me.ltbxAfter.Location = New System.Drawing.Point(244, 16)
        Me.ltbxAfter.Name = "ltbxAfter"
        Me.ltbxAfter.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.ltbxAfter.Size = New System.Drawing.Size(133, 160)
        Me.ltbxAfter.TabIndex = 13
        Me.ltbxAfter.Visible = False
        '
        'btnDel
        '
        Me.btnDel.Location = New System.Drawing.Point(149, 105)
        Me.btnDel.Name = "btnDel"
        Me.btnDel.Size = New System.Drawing.Size(89, 21)
        Me.btnDel.TabIndex = 21
        Me.btnDel.Text = "＜－－删除"
        Me.btnDel.UseVisualStyleBackColor = True
        Me.btnDel.Visible = False
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(149, 66)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(89, 21)
        Me.btnAdd.TabIndex = 20
        Me.btnAdd.Text = "添加－－＞"
        Me.btnAdd.UseVisualStyleBackColor = True
        Me.btnAdd.Visible = False
        '
        'ltbxBefore
        '
        Me.ltbxBefore.FormattingEnabled = True
        Me.ltbxBefore.ItemHeight = 12
        Me.ltbxBefore.Location = New System.Drawing.Point(10, 16)
        Me.ltbxBefore.Name = "ltbxBefore"
        Me.ltbxBefore.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.ltbxBefore.Size = New System.Drawing.Size(133, 160)
        Me.ltbxBefore.TabIndex = 10
        Me.ltbxBefore.Visible = False
        '
        'MsMaintenancePictureForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ClientSize = New System.Drawing.Size(892, 623)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "MsMaintenancePictureForm"
        Me.Text = "图片维护"
        Me.Panel1.ResumeLayout(False)
        CType(Me.grdPictureData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.picBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtPicId As System.Windows.Forms.TextBox
    Friend WithEvents txtGoodsCd As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grdPictureData As System.Windows.Forms.DataGridView
    Friend WithEvents txtPicName As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rdoExport As System.Windows.Forms.RadioButton
    Friend WithEvents rdoImport As System.Windows.Forms.RadioButton
    Friend WithEvents btnPath As System.Windows.Forms.Button
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnExecute As System.Windows.Forms.Button
    Friend WithEvents btnPicture As System.Windows.Forms.Button
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents cbDepartment As System.Windows.Forms.ComboBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents btnReturn As System.Windows.Forms.Button
    Friend WithEvents btnExist As System.Windows.Forms.Button
    Friend WithEvents txtPicPath As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtPicIdEdit As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtPicNameEdit As System.Windows.Forms.TextBox
    Friend WithEvents chkLDepartment As Lixil.AvoidMissSystem.WinUI.Common.ColorCodedCheckedListBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents ltbxAfter As System.Windows.Forms.ListBox
    Friend WithEvents btnDel As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents ltbxBefore As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents picBox As System.Windows.Forms.PictureBox
    Friend WithEvents lblCnt As System.Windows.Forms.Label
    Friend WithEvents txtPicNm As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnDelAll As System.Windows.Forms.Button
    Friend WithEvents btnAddAll As System.Windows.Forms.Button

End Class
