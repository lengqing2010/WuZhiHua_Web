<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CheckDetailForm
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
        Me.btnBack = New System.Windows.Forms.Button
        Me.TabPal = New System.Windows.Forms.TableLayoutPanel
        Me.pnlBtn = New System.Windows.Forms.Panel
        Me.Button1 = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblResult = New System.Windows.Forms.Label
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.tblTitle = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanelInput = New System.Windows.Forms.TableLayoutPanel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.picBox = New System.Windows.Forms.PictureBox
        Me.picBoxBig = New System.Windows.Forms.PictureBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.btnGotoErrRow = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.picBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBoxBig, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnBack
        '
        Me.btnBack.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.Silver
        Me.btnBack.FlatAppearance.BorderSize = 2
        Me.btnBack.Font = New System.Drawing.Font("SimHei", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBack.Location = New System.Drawing.Point(1116, 1)
        Me.btnBack.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(79, 30)
        Me.btnBack.TabIndex = 99999
        Me.btnBack.Text = "返回"
        Me.btnBack.UseVisualStyleBackColor = False
        '
        'TabPal
        '
        Me.TabPal.AutoScroll = True
        Me.TabPal.AutoSize = True
        Me.TabPal.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        Me.TabPal.ColumnCount = 9
        Me.TabPal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TabPal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TabPal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260.0!))
        Me.TabPal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.TabPal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.TabPal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.TabPal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140.0!))
        Me.TabPal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.TabPal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260.0!))
        Me.TabPal.Location = New System.Drawing.Point(0, -2)
        Me.TabPal.Name = "TabPal"
        Me.TabPal.RowCount = 2
        Me.TabPal.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TabPal.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TabPal.Size = New System.Drawing.Size(1250, 109)
        Me.TabPal.TabIndex = 6
        '
        'pnlBtn
        '
        Me.pnlBtn.BackColor = System.Drawing.Color.Transparent
        Me.pnlBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.pnlBtn.Location = New System.Drawing.Point(217, 1)
        Me.pnlBtn.Name = "pnlBtn"
        Me.pnlBtn.Size = New System.Drawing.Size(246, 30)
        Me.pnlBtn.TabIndex = 3
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.Button1.FlatAppearance.BorderColor = System.Drawing.Color.Silver
        Me.Button1.FlatAppearance.BorderSize = 2
        Me.Button1.Font = New System.Drawing.Font("SimHei", 14.25!)
        Me.Button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Button1.Location = New System.Drawing.Point(13, 1)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(141, 30)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "图片全显示"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TabPal)
        Me.Panel1.Location = New System.Drawing.Point(-2, 57)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1257, 294)
        Me.Panel1.TabIndex = 1
        '
        'lblResult
        '
        Me.lblResult.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblResult.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lblResult.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResult.ForeColor = System.Drawing.Color.Black
        Me.lblResult.Location = New System.Drawing.Point(565, 1)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.Size = New System.Drawing.Size(261, 30)
        Me.lblResult.TabIndex = 13
        Me.lblResult.Text = "结果：不合格"
        Me.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoScroll = True
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.TableLayoutPanel1.ColumnCount = 8
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(200, 100)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'tblTitle
        '
        Me.tblTitle.AutoScroll = True
        Me.tblTitle.AutoSize = True
        Me.tblTitle.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.tblTitle.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        Me.tblTitle.ColumnCount = 9
        Me.tblTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.tblTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.tblTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260.0!))
        Me.tblTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.tblTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.tblTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.tblTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140.0!))
        Me.tblTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.tblTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260.0!))
        Me.tblTitle.Font = New System.Drawing.Font("Arial Black", 10.0!, System.Drawing.FontStyle.Bold)
        Me.tblTitle.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tblTitle.Location = New System.Drawing.Point(-1, 32)
        Me.tblTitle.Name = "tblTitle"
        Me.tblTitle.RowCount = 1
        Me.tblTitle.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tblTitle.Size = New System.Drawing.Size(1250, 27)
        Me.tblTitle.TabIndex = 7
        '
        'TableLayoutPanelInput
        '
        Me.TableLayoutPanelInput.AutoScroll = True
        Me.TableLayoutPanelInput.AutoSize = True
        Me.TableLayoutPanelInput.BackColor = System.Drawing.Color.Transparent
        Me.TableLayoutPanelInput.ColumnCount = 4
        Me.TableLayoutPanelInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanelInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanelInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanelInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanelInput.Location = New System.Drawing.Point(12, 15)
        Me.TableLayoutPanelInput.Name = "TableLayoutPanelInput"
        Me.TableLayoutPanelInput.RowCount = 1
        Me.TableLayoutPanelInput.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanelInput.Size = New System.Drawing.Size(320, 320)
        Me.TableLayoutPanelInput.TabIndex = 7
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.GroupBox1.Controls.Add(Me.TableLayoutPanelInput)
        Me.GroupBox1.Location = New System.Drawing.Point(923, 356)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(336, 363)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "keyboard"
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.GroupBox2.Controls.Add(Me.picBox)
        Me.GroupBox2.Controls.Add(Me.picBoxBig)
        Me.GroupBox2.Font = New System.Drawing.Font("MingLiU", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(-1, 357)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(930, 363)
        Me.GroupBox2.TabIndex = 100001
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "keyboard"
        '
        'picBox
        '
        Me.picBox.BackColor = System.Drawing.Color.White
        Me.picBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picBox.Location = New System.Drawing.Point(682, 14)
        Me.picBox.Margin = New System.Windows.Forms.Padding(4)
        Me.picBox.Name = "picBox"
        Me.picBox.Size = New System.Drawing.Size(200, 300)
        Me.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picBox.TabIndex = 0
        Me.picBox.TabStop = False
        '
        'picBoxBig
        '
        Me.picBoxBig.BackColor = System.Drawing.Color.White
        Me.picBoxBig.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picBoxBig.Location = New System.Drawing.Point(12, 14)
        Me.picBoxBig.Margin = New System.Windows.Forms.Padding(4)
        Me.picBoxBig.Name = "picBoxBig"
        Me.picBoxBig.Size = New System.Drawing.Size(573, 300)
        Me.picBoxBig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picBoxBig.TabIndex = 1
        Me.picBoxBig.TabStop = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.Button2.FlatAppearance.BorderColor = System.Drawing.Color.Silver
        Me.Button2.FlatAppearance.BorderSize = 2
        Me.Button2.Font = New System.Drawing.Font("SimHei", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Button2.Location = New System.Drawing.Point(711, 345)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(158, 31)
        Me.Button2.TabIndex = 100002
        Me.Button2.Text = "图片全显示"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'btnGotoErrRow
        '
        Me.btnGotoErrRow.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnGotoErrRow.FlatAppearance.BorderColor = System.Drawing.Color.Silver
        Me.btnGotoErrRow.FlatAppearance.BorderSize = 2
        Me.btnGotoErrRow.Font = New System.Drawing.Font("SimHei", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGotoErrRow.Location = New System.Drawing.Point(927, 1)
        Me.btnGotoErrRow.Margin = New System.Windows.Forms.Padding(4)
        Me.btnGotoErrRow.Name = "btnGotoErrRow"
        Me.btnGotoErrRow.Size = New System.Drawing.Size(155, 30)
        Me.btnGotoErrRow.TabIndex = 99999
        Me.btnGotoErrRow.Text = "不合格行移动"
        Me.btnGotoErrRow.UseVisualStyleBackColor = False
        '
        'CheckDetailForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ClientSize = New System.Drawing.Size(1254, 712)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnGotoErrRow)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.tblTitle)
        Me.Controls.Add(Me.pnlBtn)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "CheckDetailForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Text = "检查详细"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.picBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBoxBig, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents clm4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TabPal As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents pnlBtn As System.Windows.Forms.Panel
    Friend WithEvents lblResult As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tblTitle As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanelInput As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents picBox As System.Windows.Forms.PictureBox
    Friend WithEvents picBoxBig As System.Windows.Forms.PictureBox
    Friend WithEvents btnGotoErrRow As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button

End Class
