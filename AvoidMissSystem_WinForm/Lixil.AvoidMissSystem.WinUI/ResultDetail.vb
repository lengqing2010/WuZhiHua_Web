Imports System.Windows
Imports Lixil.AvoidMissSystem.BizLogic
Imports Lixil.AvoidMissSystem.Utilities
'Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Runtime.InteropServices.Marshal

Public Class ResultDetail

#Region "变量声明"

    'BC层实例化
    Private bc As New ResultModifyBizLogic
    Private bcCom As New CommonBizLogic
    '检查结果ID
    Private strSelectId As String
    '商品ID
    Private strGoodsId As String
    '数据
    Private dt As New DataTable
    Private ltPicId As New List(Of String)

#End Region

#Region "系统响应事件"

    ''' <summary>
    ''' 重写构造函数
    ''' </summary>
    ''' <param name="strID"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal strGoodsID As String, ByVal strID As String)

        '继承构造函数
        InitializeComponent()
        Me.strGoodsId = strGoodsID
        Me.strSelectId = strID

    End Sub

    ''' <summary>
    ''' PageLoad
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ResultDetail_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '初期化
        Init()

    End Sub

#End Region

#Region "画面按钮响应事件"

    ''' <summary>
    ''' 检索按钮点击处理事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'If GetsData(getSearchKeys).Rows.Count > 0 Then
        '    dateShow(GetsData(getSearchKeys))
        'Else
        '    MsgBox("数据取得失败，请检查查询条件是否完整！", MsgBoxStyle.Critical, "错误信息")
        'End If

    End Sub

    ''' <summary>
    ''' 导出按钮点击处理事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'Dim dt As DataTable = dt
        If dt.Rows.Count > 0 Then
            Me.ExportToExcel(Me.dt)
        Else
            MsgBox("数据取得失败，请检查查询条件是否完整！", MsgBoxStyle.Critical, "错误信息")
        End If
    End Sub

    ''' <summary>
    ''' 清空按钮点击处理事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Init()
    End Sub

    ''' <summary>
    ''' 关闭按钮点击处理事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

#End Region

#Region "内部调用函数"

    ''' <summary>
    ''' 初期化处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Init()
        'CheckBox设置
        'Me.chkBoxAll.Checked = True
        'Me.chkBoxMD.Checked = False
        'Me.chkBoxOK.Checked = False
        'Me.chkBoxOW.Checked = False
        'Me.chkBoxSD.Checked = False
        '一览显示
        GetsData()
        dateShow(Me.dt)
        Me.Text = "检查结果详细"
    End Sub

    ''' <summary>
    ''' 条件取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Function getSearchKeys() As Hashtable
        Dim flg As Integer = 0
        '临时变量声明
        Dim hstab As New Hashtable
        '全部
        'If Me.chkBoxAll.Checked = True Then
        '    flg = 1
        '    hstab.Add("All", "1")
        'Else
        '    hstab.Add("All", String.Empty)
        'End If
        ''MD
        'If Me.chkBoxMD.Checked = True Then
        '    flg = 1
        '    hstab.Add("MD", "1")
        'Else
        '    hstab.Add("MD", String.Empty)
        'End If
        ''NG
        'If Me.chkBoxNG.Checked = True Then
        '    flg = 1
        '    hstab.Add("NG", "1")
        'Else
        '    hstab.Add("NG", String.Empty)
        'End If
        ''OK
        'If Me.chkBoxOK.Checked = True Then
        '    flg = 1
        '    hstab.Add("OK", "1")
        'Else
        '    hstab.Add("OK", String.Empty)
        'End If
        ''OW
        'If Me.chkBoxOW.Checked = True Then
        '    flg = 1
        '    hstab.Add("OW", "1")
        'Else
        '    hstab.Add("OW", String.Empty)
        'End If
        ''SD
        'If Me.chkBoxSD.Checked = True Then
        '    flg = 1
        '    hstab.Add("SD", "1")
        'Else
        '    hstab.Add("SD", String.Empty)
        'End If
        'hstab.Add("flg", flg)
        Return hstab

    End Function

    ''' <summary>
    ''' 一览显示
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Sub dateShow(ByVal dt As DataTable)
        '数据源绑定
        Me.dgvChkResultDetail.DataSource = dt
        '列绑定
        For i As Integer = 0 To Me.dgvChkResultDetail.ColumnCount - 1
            Me.dgvChkResultDetail.Columns(i).DataPropertyName = dt.Columns(i).ToString
            Me.dgvChkResultDetail.Columns(i).Width = 100
        Next
        Me.dgvChkResultDetail.Columns(1).Visible = False
        Me.dgvChkResultDetail.Columns(0).Width = 77
        'Me.dgvChkResultDetail.Columns(7).Width = 70
    End Sub

    ''' <summary>
    ''' 导出到Excel
    ''' </summary>
    ''' <param name="Table"></param>
    ''' <remarks></remarks>
    Private Sub ExportToExcel(ByVal Table As DataTable)

        Dim strFileName As String
        Dim saveFileDialog As SaveFileDialog
        Dim xlBook = Nothing
        Dim xlSheet = Nothing
        Dim xlApp = Nothing
        Dim rowCount As Integer
        Dim colCount As Integer
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
            Dim strFilePath As String = strFileName.Substring(0, strFileName.LastIndexOf("\"))

            Me.Cursor = Cursors.WaitCursor
            xlApp = CreateObject("Excel.Application")
            xlApp.Visible = False
            xlBook = xlApp.Workbooks.Add
            rowCount = Table.Rows.Count
            colCount = Table.Columns.Count + 1
            ReDim arrData(rowCount, colCount)

            Dim cn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

            If cn.IndexOf("AvoidMiss_Experiment") > 0 Then

                arrData(0, 0) = "编号"
                arrData(0, 1) = "检查项目ID"
                arrData(0, 2) = "类别"
                arrData(0, 3) = "检查位置"
                arrData(0, 4) = "检查项目"
                arrData(0, 5) = "检查方法"
                arrData(0, 6) = "实测值1"
                arrData(0, 7) = "实测值2"
                arrData(0, 8) = "实测值3"
                arrData(0, 9) = "实测值4"
                arrData(0, 10) = "实测值5"
                arrData(0, 11) = "实测值6"
                arrData(0, 12) = "图片ID"
                arrData(0, 13) = "判定结果"
                arrData(0, 14) = "备注"
                arrData(0, 15) = "表示顺"


                arrData(0, 16) = "基准类型"
                arrData(0, 17) = "基准值1"
                arrData(0, 18) = "基准值2"
                arrData(0, 19) = "基准值3"
            Else
                '列名设置
                arrData(0, 0) = "编号"
                arrData(0, 1) = "检查项目ID"
                arrData(0, 2) = "类别"
                arrData(0, 3) = "检查位置"
                arrData(0, 4) = "检查项目"
                arrData(0, 5) = "检查方法"
                arrData(0, 6) = "实测值1"
                arrData(0, 7) = "实测值2"
                arrData(0, 8) = "图片ID"
                arrData(0, 9) = "判定结果"
                arrData(0, 10) = "备注"
                arrData(0, 11) = "表示顺"
                arrData(0, 12) = "基准类型"
                arrData(0, 13) = "基准值1"
                arrData(0, 14) = "基准值2"
                arrData(0, 15) = "基准值3"


            End If

            For i As Integer = 1 To rowCount
                With Table.Rows(i - 1)

                    If cn.IndexOf("AvoidMiss_Experiment") > 0 Then
                        arrData(i, 0) = i.ToString
                        arrData(i, 1) = Convert.ToString(.Item("检查项目ID"))
                        arrData(i, 2) = Convert.ToString(.Item("类别"))
                        arrData(i, 3) = Convert.ToString(.Item("检查位置"))
                        arrData(i, 4) = Convert.ToString(.Item("检查项目"))
                        arrData(i, 5) = Convert.ToString(.Item("检查方法"))
                        arrData(i, 6) = Convert.ToString(.Item("实测值1"))
                        arrData(i, 7) = Convert.ToString(.Item("实测值2"))
                        arrData(i, 8) = Convert.ToString(.Item("实测值3"))
                        arrData(i, 9) = Convert.ToString(.Item("实测值4"))
                        arrData(i, 10) = Convert.ToString(.Item("实测值5"))
                        arrData(i, 11) = Convert.ToString(.Item("实测值6"))
                        arrData(i, 12) = Convert.ToString(.Item("图片ID"))


                        arrData(i, 13) = Convert.ToString(.Item("基准类型"))
                        arrData(i, 14) = Convert.ToString(.Item("基准值1"))
                        arrData(i, 15) = Convert.ToString(.Item("基准值2"))
                        arrData(i, 16) = Convert.ToString(.Item("基准值3"))

                    Else
                        arrData(i, 0) = i.ToString
                        arrData(i, 1) = Convert.ToString(.Item("检查项目ID"))
                        arrData(i, 2) = Convert.ToString(.Item("类别"))
                        arrData(i, 3) = Convert.ToString(.Item("检查位置"))
                        arrData(i, 4) = Convert.ToString(.Item("检查项目"))
                        arrData(i, 5) = Convert.ToString(.Item("检查方法"))
                        arrData(i, 6) = Convert.ToString(.Item("实测值1"))
                        arrData(i, 7) = Convert.ToString(.Item("实测值2"))
                        arrData(i, 8) = Convert.ToString(.Item("图片ID"))

                        arrData(i, 12) = Convert.ToString(.Item("基准类型"))
                        arrData(i, 13) = Convert.ToString(.Item("基准值1"))
                        arrData(i, 14) = Convert.ToString(.Item("基准值2"))
                        arrData(i, 15) = Convert.ToString(.Item("基准值3"))
                    End If

                    Dim jieguo As String = Convert.ToString(.Item("判定结果"))
                    If jieguo = "OK" Then
                        jieguo = "合"
                    ElseIf jieguo = "SD" Then
                        jieguo = "微"
                    ElseIf jieguo = "M1" Then
                        jieguo = "轻"
                    ElseIf jieguo = "M2" Then
                        jieguo = "中"
                    ElseIf jieguo = "M3" Then
                        jieguo = "重"
                    ElseIf jieguo = "NG" Then
                        jieguo = "误"
                    ElseIf jieguo = "JN" Then
                        jieguo = "警"
                    End If
                    arrData(i, 9) = jieguo


                    arrData(i, 10) = Convert.ToString(.Item("备注"))
                    arrData(i, 11) = Convert.ToString(.Item("表示顺"))
                End With
            Next
            '导出数据设定
            xlSheet = xlBook.Worksheets.Add()
            xlSheet.Name = "检查结果详细"
            For Each wkSheet  In xlBook.Worksheets
                If wkSheet.Name <> "检查结果详细" Then
                    wkSheet.Delete()
                End If
            Next
            xlSheet.Activate()
            xlSheet.Range("A1").Resize(rowCount + 1, colCount).Value = arrData
            xlBook.SaveAs(strFileName)
            xlBook.Close()
            xlApp.Quit()

            Me.Cursor = Cursors.Arrow


            '图片保存处理 ，保存在出力数据EXCEl路径下的Image文件夹下
            If Me.ltPicId.Count > 0 Then
                Try
                    Dim strImagePath As String = strFilePath & "\Image" & Now.ToString("yyyyMMddHHmmss")
                    Dim strPicName As String = String.Empty
                    '若文件夹不存在则新建文件夹   
                    If Not System.IO.Directory.Exists(strImagePath) Then
                        '创建图片文件夹  
                        System.IO.Directory.CreateDirectory(strImagePath)
                    End If
                    '图片输出
                    For j As Integer = 0 To Me.ltPicId.Count - 1

                        Dim dtPic As New DataTable
                        dtPic = bc.GetPictureMsgById(ltPicId.Item(j).ToString).Tables("DetailInfo")
                        Dim imageByte() As Byte = DirectCast(dtPic.Rows(0).Item("picture_content"), Byte())
                        Dim sm As MemoryStream = New MemoryStream(imageByte, True)
                        Dim px As New PictureBox
                        px.Image = Image.FromStream(DirectCast(sm, System.IO.Stream))
                        strPicName = ltPicId.Item(j).ToString & ".jpg"
                        px.Image.Save(strImagePath & "\" & strPicName, px.Image.RawFormat)
                        sm.Close()
                    Next
                    MessageBox.Show(MsgConst.M00004I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("图片文件夹出错", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)

                End Try

            Else
                '无图片的场合
                MessageBox.Show(MsgConst.M00058I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
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
        End Try
    End Sub

#End Region

#Region "数据取得"
    ''' <summary>
    ''' 取得数据并格式化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetsData()
        'Dim dtTemp As New DataTable
        'Dim hsSearch As New Hashtable
        ''条件判断
        'If CInt(hsKeys("flg")) = 0 Then
        '    '条件不完整的情况下直接返回空置
        '    Return dtTemp
        '    Exit Sub
        'End I
        Dim dtAdd As New DataTable
        Dim dtTemp As New DataTable
        dtTemp = bc.GetAllDetaileById(Me.strGoodsId, Me.strSelectId).Tables("DetailInfo")
        '检查次数 > 1 的场合，增加行处理
        dtAdd = dtTemp.Copy
        If dtTemp.Rows.Count > 0 Then
            Dim strPictureId As String = String.Empty
            For i As Integer = 0 To dtTemp.Rows.Count - 1

                Dim jieguo As String = dtTemp.Rows(i).Item("判定结果").ToString.Trim

                If jieguo = "OK" Then
                    jieguo = "合"
                ElseIf jieguo = "SD" Then
                    jieguo = "微"
                ElseIf jieguo = "M1" Then
                    jieguo = "轻"
                ElseIf jieguo = "M2" Then
                    jieguo = "中"
                ElseIf jieguo = "M3" Then
                    jieguo = "重"
                ElseIf jieguo = "NG" Then
                    jieguo = "误"
                ElseIf jieguo = "JN" Then
                    jieguo = "警"
                End If
                dtTemp.Rows(i).Item("判定结果") = jieguo


                If CInt(dtTemp.Rows(i).Item("检查次数").ToString) > 1 Then
                    '当前行为检查多次的场合
                    Dim iCount As Integer = i
                    Dim intICount As Integer = CInt(dtTemp.Rows(i).Item("检查次数").ToString)
                    For j As Integer = 1 To intICount
                        If dtTemp.Rows(i).Item("检查项目ID").ToString.Equals(dtTemp.Rows(iCount).Item("检查项目ID").ToString) AndAlso CInt(dtTemp.Rows(i).Item("表示顺").ToString) = j Then
                            '大循环 + 1
                            i = i + 1
                        Else
                            '当前行的信息与表示顺不同。增加一行
                            Dim dtRow As DataRow = dtAdd.NewRow
                            dtRow.ItemArray = dtTemp.Rows(iCount).ItemArray
                            '信息重置
                            dtRow.Item("实测值1") = String.Empty
                            dtRow.Item("实测值2") = String.Empty
                            dtRow.Item("判定结果") = String.Empty
                            dtRow.Item("表示顺") = j.ToString
                            dtRow.Item("备注") = String.Empty
                            'dtAdd.ImportRow(dtRow)
                            dtAdd.Rows.Add(dtRow)
                            dtAdd.AcceptChanges()
                        End If
                    Next
                    '总体i要退回一步
                    i = i - 1
                End If
                '图片ID保存
                If strPictureId.Equals(dtTemp.Rows(i).Item("图片ID").ToString.Trim) = False AndAlso dtTemp.Rows(i).Item("图片ID").ToString.Trim.Equals(String.Empty) = False Then
                    Me.ltPicId.Add(dtTemp.Rows(i).Item("图片ID").ToString.Trim)
                    strPictureId = dtTemp.Rows(i).Item("图片ID").ToString.Trim
                End If
            Next
        Else
            MessageBox.Show(MsgConst.M00005I)
        End If
        '重新排序
        Dim tempDv As DataView = dtAdd.DefaultView
        tempDv.Sort = "检查项目ID,表示顺"
        Me.dt = tempDv.ToTable()

        For i As Integer = 0 To Me.dt.Rows.Count - 1

            Dim jieguo As String = Me.dt.Rows(i).Item("判定结果").ToString.Trim

            If jieguo = "OK" Then
                jieguo = "合"
            ElseIf jieguo = "SD" Then
                jieguo = "微"
            ElseIf jieguo = "M1" Then
                jieguo = "轻"
            ElseIf jieguo = "M2" Then
                jieguo = "中"
            ElseIf jieguo = "M3" Then
                jieguo = "重"
            ElseIf jieguo = "NG" Then
                jieguo = "误"
            ElseIf jieguo = "JN" Then
                jieguo = "警"
            End If
            Me.dt.Rows(i).Item("判定结果") = jieguo

        Next


        'dtAdd.Sort("检查项目ID")
        '列名设置
        'Dim ds1 As New DataColumn
        'ds1.ColumnName = "ID"
        'Dim ds2 As New DataColumn
        'ds2.ColumnName = "类别"
        'Dim ds3 As New DataColumn
        'ds3.ColumnName = "检查位置"
        'Dim ds4 As New DataColumn
        'ds4.ColumnName = "检查项目"
        'Dim ds5 As New DataColumn
        'ds5.ColumnName = "基准值"
        'Dim ds6 As New DataColumn
        'ds6.ColumnName = "检查方法"
        'Dim ds7 As New DataColumn
        'ds7.ColumnName = "实测值1"
        'Dim ds8 As New DataColumn
        'ds8.ColumnName = "实测值2"
        'Dim ds9 As New DataColumn
        'ds9.ColumnName = "判定结果"
        ''列名添加
        'dtTemp.Columns.Add(ds1)
        'dtTemp.Columns.Add(ds2)
        'dtTemp.Columns.Add(ds3)
        'dtTemp.Columns.Add(ds4)
        'dtTemp.Columns.Add(ds5)
        'dtTemp.Columns.Add(ds6)
        'dtTemp.Columns.Add(ds7)
        'dtTemp.Columns.Add(ds8)
        'dtTemp.Columns.Add(ds9)

        'If dtAllDate.Rows.Count > 0 Then
        '    For i As Integer = 0 To dtAllDate.Rows.Count - 1
        '        Dim dtRow As DataRow

        '        dtRow = dtTemp.NewRow
        '        'ID
        '        dtRow.Item(0) = dtAllDate.Rows(i).Item("check_id").ToString
        '        '类别
        '        dtRow.Item(1) = dtAllDate.Rows(i).Item("kind").ToString
        '        '检查位置
        '        dtRow.Item(2) = dtAllDate.Rows(i).Item("check_position").ToString
        '        '检查项目
        '        dtRow.Item(3) = dtAllDate.Rows(i).Item("check_item").ToString
        '        '基准值
        '        hsSearch.Add("value1", dtAllDate.Rows(i).Item("benchmark_value1").ToString)
        '        hsSearch.Add("value2", dtAllDate.Rows(i).Item("benchmark_value2").ToString)
        '        hsSearch.Add("value3", dtAllDate.Rows(i).Item("benchmark_value3").ToString)
        '        dtRow.Item(4) = bcCom.GetBenchmarkShow(dtAllDate.Rows(i).Item("benchmark_type").ToString, hsSearch)
        '        '检查方法
        '        dtRow.Item(5) = dtAllDate.Rows(i).Item("check_way").ToString
        '        '实测值1
        '        dtRow.Item(6) = dtAllDate.Rows(i).Item("measure_value1").ToString
        '        '实测值2
        '        dtRow.Item(7) = dtAllDate.Rows(i).Item("measure_value2").ToString
        '        '判定结果
        '        dtRow.Item(8) = dtAllDate.Rows(i).Item("result").ToString
        '        '行添加
        '        dtTemp.Rows.Add(dtRow)
        '    Next
        'End If
        '有基准场合
        'Return dtTemp
        '没有基准场合
        'Return dtAllDate
    End Sub
#End Region

End Class
