<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MsMaintenanceLogForm
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
        Me.gbSearch = New System.Windows.Forms.GroupBox
        Me.lblPDate = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.Label12 = New System.Windows.Forms.Label
        Me.PToDate = New System.Windows.Forms.DateTimePicker
        Me.PFromDate = New System.Windows.Forms.DateTimePicker
        Me.dtLogList = New System.Windows.Forms.DataGridView
        Me.日志cd = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.操作机能名 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.操作类型 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.操作者 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.操作信息 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.操作时间 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnReturn = New System.Windows.Forms.Button
        Me.gbSearch.SuspendLayout()
        CType(Me.dtLogList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbSearch
        '
        Me.gbSearch.Controls.Add(Me.lblPDate)
        Me.gbSearch.Controls.Add(Me.btnSearch)
        Me.gbSearch.Controls.Add(Me.Label12)
        Me.gbSearch.Controls.Add(Me.PToDate)
        Me.gbSearch.Controls.Add(Me.PFromDate)
        Me.gbSearch.Font = New System.Drawing.Font("Microsoft YaHei", 9.0!)
        Me.gbSearch.Location = New System.Drawing.Point(25, 30)
        Me.gbSearch.Name = "gbSearch"
        Me.gbSearch.Size = New System.Drawing.Size(837, 68)
        Me.gbSearch.TabIndex = 11
        Me.gbSearch.TabStop = False
        Me.gbSearch.Text = "查询"
        '
        'lblPDate
        '
        Me.lblPDate.AutoSize = True
        Me.lblPDate.Font = New System.Drawing.Font("Microsoft YaHei", 11.0!)
        Me.lblPDate.Location = New System.Drawing.Point(15, 32)
        Me.lblPDate.Name = "lblPDate"
        Me.lblPDate.Size = New System.Drawing.Size(84, 20)
        Me.lblPDate.TabIndex = 31
        Me.lblPDate.Text = "查询日期："
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Microsoft YaHei", 11.0!)
        Me.btnSearch.Location = New System.Drawing.Point(551, 26)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(85, 30)
        Me.btnSearch.TabIndex = 12
        Me.btnSearch.Text = "查询"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft YaHei", 9.0!)
        Me.Label12.Location = New System.Drawing.Point(293, 31)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(17, 17)
        Me.Label12.TabIndex = 30
        Me.Label12.Text = "~"
        '
        'PToDate
        '
        Me.PToDate.Font = New System.Drawing.Font("Microsoft YaHei", 9.0!)
        Me.PToDate.Location = New System.Drawing.Point(352, 26)
        Me.PToDate.Name = "PToDate"
        Me.PToDate.Size = New System.Drawing.Size(155, 23)
        Me.PToDate.TabIndex = 29
        '
        'PFromDate
        '
        Me.PFromDate.Font = New System.Drawing.Font("Microsoft YaHei", 9.0!)
        Me.PFromDate.Location = New System.Drawing.Point(125, 27)
        Me.PFromDate.Name = "PFromDate"
        Me.PFromDate.Size = New System.Drawing.Size(162, 23)
        Me.PFromDate.TabIndex = 28
        '
        'dtLogList
        '
        Me.dtLogList.AllowUserToAddRows = False
        Me.dtLogList.BackgroundColor = System.Drawing.SystemColors.Window
        Me.dtLogList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtLogList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.日志cd, Me.操作机能名, Me.操作类型, Me.操作者, Me.操作信息, Me.操作时间})
        Me.dtLogList.Location = New System.Drawing.Point(28, 104)
        Me.dtLogList.Name = "dtLogList"
        Me.dtLogList.RowTemplate.Height = 21
        Me.dtLogList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dtLogList.Size = New System.Drawing.Size(834, 445)
        Me.dtLogList.TabIndex = 31
        '
        '日志cd
        '
        Me.日志cd.DataPropertyName = "log_cd"
        Me.日志cd.HeaderText = "日志cd"
        Me.日志cd.Name = "日志cd"
        '
        '操作机能名
        '
        Me.操作机能名.DataPropertyName = "operate_tb"
        Me.操作机能名.HeaderText = "操作机能名"
        Me.操作机能名.Name = "操作机能名"
        '
        '操作类型
        '
        Me.操作类型.DataPropertyName = "operate_kind"
        Me.操作类型.HeaderText = "操作类型"
        Me.操作类型.Name = "操作类型"
        '
        '操作者
        '
        Me.操作者.DataPropertyName = "operator_cd"
        Me.操作者.HeaderText = "操作者"
        Me.操作者.Name = "操作者"
        '
        '操作信息
        '
        Me.操作信息.DataPropertyName = "operate_objcd"
        Me.操作信息.HeaderText = "操作信息"
        Me.操作信息.Name = "操作信息"
        '
        '操作时间
        '
        Me.操作时间.DataPropertyName = "operate_date"
        Me.操作时间.HeaderText = "操作时间"
        Me.操作时间.Name = "操作时间"
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Microsoft YaHei", 11.0!)
        Me.btnClose.Location = New System.Drawing.Point(784, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 31)
        Me.btnClose.TabIndex = 33
        Me.btnClose.Text = "退出"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnReturn
        '
        Me.btnReturn.Font = New System.Drawing.Font("Microsoft YaHei", 11.0!)
        Me.btnReturn.Location = New System.Drawing.Point(706, 3)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(75, 31)
        Me.btnReturn.TabIndex = 32
        Me.btnReturn.Text = "返回"
        Me.btnReturn.UseVisualStyleBackColor = True
        '
        'MsMaintenanceLogForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ClientSize = New System.Drawing.Size(892, 623)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnReturn)
        Me.Controls.Add(Me.dtLogList)
        Me.Controls.Add(Me.gbSearch)
        Me.Name = "MsMaintenanceLogForm"
        Me.Text = "日志查询"
        Me.gbSearch.ResumeLayout(False)
        Me.gbSearch.PerformLayout()
        CType(Me.dtLogList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbSearch As System.Windows.Forms.GroupBox
    Friend WithEvents lblPDate As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents PToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents PFromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtLogList As System.Windows.Forms.DataGridView
    Friend WithEvents 日志cd As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 操作机能名 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 操作类型 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 操作者 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 操作信息 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 操作时间 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnReturn As System.Windows.Forms.Button

End Class
