<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CheckMainForm
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
        Me.pnl2 = New System.Windows.Forms.Panel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.lbltitle3 = New System.Windows.Forms.Label
        Me.lbltitle1 = New System.Windows.Forms.Label
        Me.lbltitle2 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.tabPalChk = New System.Windows.Forms.TableLayoutPanel
        Me.cb1 = New System.Windows.Forms.CheckBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.btnFacedeInchchk = New System.Windows.Forms.Button
        Me.btnKeyboard = New System.Windows.Forms.Button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.lblCheckState = New System.Windows.Forms.Label
        Me.btnExits = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.btnToolchk = New System.Windows.Forms.Button
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.lblStaus = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl2
        '
        Me.pnl2.AutoScroll = True
        Me.pnl2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl2.Location = New System.Drawing.Point(15, 23)
        Me.pnl2.Name = "pnl2"
        Me.pnl2.Size = New System.Drawing.Size(270, 449)
        Me.pnl2.TabIndex = 6
        Me.pnl2.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox1.Controls.Add(Me.Panel3)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.GroupBox1.Location = New System.Drawing.Point(12, 124)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(569, 492)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "检查项目"
        Me.GroupBox1.Visible = False
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel3.Controls.Add(Me.lbltitle3)
        Me.Panel3.Controls.Add(Me.lbltitle1)
        Me.Panel3.Controls.Add(Me.lbltitle2)
        Me.Panel3.Location = New System.Drawing.Point(6, 18)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(526, 29)
        Me.Panel3.TabIndex = 3
        '
        'lbltitle3
        '
        Me.lbltitle3.AutoSize = True
        Me.lbltitle3.BackColor = System.Drawing.Color.Transparent
        Me.lbltitle3.Font = New System.Drawing.Font("Meiryo UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltitle3.ForeColor = System.Drawing.Color.Navy
        Me.lbltitle3.Location = New System.Drawing.Point(343, 5)
        Me.lbltitle3.Name = "lbltitle3"
        Me.lbltitle3.Size = New System.Drawing.Size(171, 20)
        Me.lbltitle3.TabIndex = 2
        Me.lbltitle3.Text = "备注（红色：不合格）"
        Me.lbltitle3.Visible = False
        '
        'lbltitle1
        '
        Me.lbltitle1.AutoSize = True
        Me.lbltitle1.BackColor = System.Drawing.Color.Transparent
        Me.lbltitle1.Font = New System.Drawing.Font("Meiryo UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltitle1.ForeColor = System.Drawing.Color.Navy
        Me.lbltitle1.Location = New System.Drawing.Point(57, 5)
        Me.lbltitle1.Name = "lbltitle1"
        Me.lbltitle1.Size = New System.Drawing.Size(62, 20)
        Me.lbltitle1.TabIndex = 2
        Me.lbltitle1.Text = "治具ID"
        Me.lbltitle1.Visible = False
        '
        'lbltitle2
        '
        Me.lbltitle2.AutoSize = True
        Me.lbltitle2.BackColor = System.Drawing.Color.Transparent
        Me.lbltitle2.Font = New System.Drawing.Font("Meiryo UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltitle2.ForeColor = System.Drawing.Color.Navy
        Me.lbltitle2.Location = New System.Drawing.Point(219, 5)
        Me.lbltitle2.Name = "lbltitle2"
        Me.lbltitle2.Size = New System.Drawing.Size(58, 20)
        Me.lbltitle2.TabIndex = 2
        Me.lbltitle2.Text = "条形码"
        Me.lbltitle2.Visible = False
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.tabPalChk)
        Me.Panel1.Location = New System.Drawing.Point(5, 44)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(545, 442)
        Me.Panel1.TabIndex = 1
        '
        'tabPalChk
        '
        Me.tabPalChk.AutoSize = True
        Me.tabPalChk.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.tabPalChk.ColumnCount = 3
        Me.tabPalChk.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180.0!))
        Me.tabPalChk.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 141.0!))
        Me.tabPalChk.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 204.0!))
        Me.tabPalChk.Location = New System.Drawing.Point(0, 6)
        Me.tabPalChk.Name = "tabPalChk"
        Me.tabPalChk.RowCount = 1
        Me.tabPalChk.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tabPalChk.Size = New System.Drawing.Size(529, 20)
        Me.tabPalChk.TabIndex = 0
        '
        'cb1
        '
        Me.cb1.AutoSize = True
        Me.cb1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cb1.Location = New System.Drawing.Point(529, 11)
        Me.cb1.Name = "cb1"
        Me.cb1.Size = New System.Drawing.Size(60, 24)
        Me.cb1.TabIndex = 100001
        Me.cb1.Text = "欠品"
        Me.cb1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.AutoSize = True
        Me.GroupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox2.Controls.Add(Me.pnl2)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.GroupBox2.Location = New System.Drawing.Point(587, 124)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(291, 495)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "检查项目"
        Me.GroupBox2.Visible = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cb1)
        Me.Panel2.Controls.Add(Me.btnFacedeInchchk)
        Me.Panel2.Controls.Add(Me.btnKeyboard)
        Me.Panel2.Controls.Add(Me.btnExits)
        Me.Panel2.Controls.Add(Me.Button1)
        Me.Panel2.Controls.Add(Me.btnToolchk)
        Me.Panel2.Controls.Add(Me.GroupBox3)
        Me.Panel2.Location = New System.Drawing.Point(2, 1)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(876, 135)
        Me.Panel2.TabIndex = 100000
        '
        'btnFacedeInchchk
        '
        Me.btnFacedeInchchk.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnFacedeInchchk.Font = New System.Drawing.Font("SimHei", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFacedeInchchk.Location = New System.Drawing.Point(16, 4)
        Me.btnFacedeInchchk.Name = "btnFacedeInchchk"
        Me.btnFacedeInchchk.Size = New System.Drawing.Size(120, 47)
        Me.btnFacedeInchchk.TabIndex = 100005
        Me.btnFacedeInchchk.Text = "外观寸法"
        Me.btnFacedeInchchk.UseVisualStyleBackColor = False
        '
        'btnKeyboard
        '
        Me.btnKeyboard.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnKeyboard.Enabled = False
        Me.btnKeyboard.Font = New System.Drawing.Font("SimHei", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnKeyboard.Location = New System.Drawing.Point(646, 1)
        Me.btnKeyboard.Margin = New System.Windows.Forms.Padding(4)
        Me.btnKeyboard.Name = "btnKeyboard"
        Me.btnKeyboard.Size = New System.Drawing.Size(107, 47)
        Me.btnKeyboard.TabIndex = 100004
        Me.btnKeyboard.Text = "打开键盘"
        Me.btnKeyboard.UseVisualStyleBackColor = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lblStaus)
        Me.GroupBox3.Controls.Add(Me.lblCheckState)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.GroupBox3.Location = New System.Drawing.Point(10, 49)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(854, 70)
        Me.GroupBox3.TabIndex = 100003
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "状态"
        '
        'lblCheckState
        '
        Me.lblCheckState.Font = New System.Drawing.Font("Malgun Gothic", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCheckState.Location = New System.Drawing.Point(43, 15)
        Me.lblCheckState.Name = "lblCheckState"
        Me.lblCheckState.Size = New System.Drawing.Size(713, 27)
        Me.lblCheckState.TabIndex = 4
        Me.lblCheckState.Text = "当前检查商品Code：12321321321321321"
        '
        'btnExits
        '
        Me.btnExits.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnExits.Font = New System.Drawing.Font("SimHei", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExits.Location = New System.Drawing.Point(760, 1)
        Me.btnExits.Name = "btnExits"
        Me.btnExits.Size = New System.Drawing.Size(70, 47)
        Me.btnExits.TabIndex = 100002
        Me.btnExits.Text = "退出"
        Me.btnExits.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.Button1.Font = New System.Drawing.Font("SimHei", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(293, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(168, 47)
        Me.Button1.TabIndex = 100001
        Me.Button1.Text = "手入力检查结果"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'btnToolchk
        '
        Me.btnToolchk.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnToolchk.Font = New System.Drawing.Font("SimHei", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnToolchk.Location = New System.Drawing.Point(142, 4)
        Me.btnToolchk.Name = "btnToolchk"
        Me.btnToolchk.Size = New System.Drawing.Size(131, 47)
        Me.btnToolchk.TabIndex = 100001
        Me.btnToolchk.Text = "治具"
        Me.btnToolchk.UseVisualStyleBackColor = False
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox1.Location = New System.Drawing.Point(325, 12)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(60, 24)
        Me.CheckBox1.TabIndex = 100001
        Me.CheckBox1.Text = "欠品"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'lblStaus
        '
        Me.lblStaus.Font = New System.Drawing.Font("Malgun Gothic", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStaus.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblStaus.Location = New System.Drawing.Point(48, 44)
        Me.lblStaus.Name = "lblStaus"
        Me.lblStaus.Size = New System.Drawing.Size(793, 25)
        Me.lblStaus.TabIndex = 5
        Me.lblStaus.Text = "@"
        '
        'CheckMainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ClientSize = New System.Drawing.Size(890, 628)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "CheckMainForm"
        Me.Text = "检查主页面"
        Me.GroupBox1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl2 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents tabPalChk As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnFacedeInchchk As System.Windows.Forms.Button
    Friend WithEvents btnKeyboard As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents lblCheckState As System.Windows.Forms.Label
    Friend WithEvents btnExits As System.Windows.Forms.Button
    Friend WithEvents btnToolchk As System.Windows.Forms.Button
    Friend WithEvents cb1 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lbltitle1 As System.Windows.Forms.Label
    Friend WithEvents lbltitle3 As System.Windows.Forms.Label
    Friend WithEvents lbltitle2 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents lblStaus As System.Windows.Forms.Label

End Class
