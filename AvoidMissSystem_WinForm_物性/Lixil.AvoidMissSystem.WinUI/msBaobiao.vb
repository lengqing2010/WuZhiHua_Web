Imports System.Net
Imports System.IO
Imports System.Data
Imports Lixil.AvoidMissSystem.BizLogic
'Imports Microsoft.Office.Interop
Imports Lixil.AvoidMissSystem.Utilities.Consts
Imports Lixil.AvoidMissSystem.Utilities.MsgConst
Imports Lixil.AvoidMissSystem.WinUI.Common
Imports Lixil.AvoidMissSystem.Utilities
Imports System.Windows

Public Class msBaobiao
    Dim pbImg As Image
    Dim bc As New MsMaintenceCheckBizLogic

    Public kmTitle As String


#Region "画面事件"

    ''' <summary>
    ''' 单选框选择变化
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoJCJGMS.CheckedChanged, _
                                                                                                   rdoWJCYL.CheckedChanged, _
                                                                                                   rdoMissCheckListNewAndOld.CheckedChanged, _
                                                                                                   rdoXM.CheckedChanged, _
                                                                                                   rdoTools.CheckedChanged, _
                                                                                                   rdoCheckResult.CheckedChanged, _
                                                                                                   rdoPicture.CheckedChanged, _
                                                                                                   rdoNewOldCheckResultMs.CheckedChanged, _
                                                                                                   rbCheckMS.CheckedChanged


        If rdoJCJGMS.Checked Then
            ' kmTitle = "生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,结果,详细"
            kmTitle = "系统,生产实际日,部门,生产线N,商品名,商品コード,作番,完工数量,向先,纳期日,检查日期,检查开始时间,检查终了时间,检查时长,结果,详细,检查者,备注"
            SetWhere(kmTitle)
        ElseIf rdoNewOldCheckResultMs.Checked Then '新旧
            kmTitle = "系统,生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,检查日期,检查开始时间,检查终了时间,检查时长,结果,详细,检查者,备注"
            'SetWhere("系统,商品名,商品コード,作番,生产实际日,完工数量,生产线,向先,纳期日,检查日期,检查者,治具编号,图片名称,分类名称,检查顺序,类别,检查位置,检查项目,基准类型,基准值1,基准值2,基准值3,检查次数,方法,检查结果,欠品,检查开始时间,检查终了时间,检查种类")
            SetWhere(kmTitle)

        ElseIf rdoWJCYL.Checked Then
            kmTitle = "生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,原因"
            SetWhere(kmTitle)
        ElseIf rdoMissCheckListNewAndOld.Checked Then '新旧
            kmTitle = "系统,生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,原因"
            SetWhere(kmTitle)
        ElseIf rdoXM.Checked Then
            Dim cn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

            If cn.IndexOf("AvoidMiss_Experiment") > 0 Then
                kmTitle = "商品コード,作番,值1,值2,值3,值4,值5,值6,生产日期,出荷日期,数量"
                SetWhere(kmTitle)
            Else
                kmTitle = "商品コード,作番,实测值1,实测值2,生产日期,出荷日期,数量"
                SetWhere(kmTitle)
            End If

        ElseIf rdoTools.Checked Then
            kmTitle = "治具名称,商品コード,治具ID,治具区分,条码,条码有无"
            SetWhere(kmTitle)
        ElseIf rdoCheckResult.Checked Then
            kmTitle = "商品名,商品コード,作番,生产线,检查者,完工数量,生産日期,納品日期,检查合格,检查不合格,検査開始時間,検査終了時間"
            SetWhere(kmTitle)
        ElseIf rdoPicture.Checked Then
            kmTitle = "商品コード,图片ID,图片名称,图片描述"
            SetWhere(kmTitle)
        ElseIf rbCheckMS.Checked Then

            Dim cn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

            If cn.IndexOf("AvoidMiss_Experiment") > 0 Then
                kmTitle = "id,部门,生产线N,商品名,商品コード,作番,生产实际日,完工数量,生产线,向先,纳期日,检查日期,检查者,治具编号,图片名称,picture_content,分类名称,检查顺序,类别,检查位置,检查项目,基准类型,基准值1,基准值2,基准值3,检查次数,方法,实测值1,实测值2,实测值3,实测值4,实测值5,实测值6,检查结果,备注,欠品,检查开始时间,检查终了时间,检查时长,检查种类"
                SetWhere(kmTitle)

            Else
                kmTitle = "id,商品名,商品コード,作番,生产实际日,完工数量,生产线,向先,纳期日,检查日期,检查者,治具编号,图片名称,picture_content,分类名称,检查顺序,类别,检查位置,检查项目,基准类型,基准值1,基准值2,基准值3,检查次数,方法,实测值1,实测值2,检查结果,备注,欠品,检查开始时间,检查终了时间,检查时长,检查种类"
                SetWhere(kmTitle)

            End If
        End If


    End Sub

    ''' <summary>
    ''' 浏览
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnList.Click

        Try

            dgvList.DataSource = Nothing

            Dim bc As New MsBaobiaoBizLogic
            If Me.rdoOr.Checked Then
                bc.JoinFlg = " or "
            Else
                bc.JoinFlg = " and "
            End If



            '1.検査結果詳細表
            If Me.rdoJCJGMS.Checked Then

                '  title = "生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,结果,详细"
                dgvList.DataSource = bc.GetJCJGMS(GetWhere())


            End If

            '2.检查结果汇总表
            If Me.rdoCheckResult.Checked Then
                kmTitle = "商品名,商品コード,作番,生产线,检查者,完工数量,生産日期,納品日期,検査完了,默認結果,正在検査,检查合格,检查不合格,欠品"
                dgvList.DataSource = (bc.GetCheckResult(GetWhere()))
            End If

            '3.未检查一览表
            If Me.rdoWJCYL.Checked Then
                ' title = "生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,原因"
                dgvList.DataSource = bc.GetWJCYL(GetWhere())

            End If

            '4.图片 一览表
            If Me.rdoPicture.Checked Then
                ' title = "图片名称,图片描述,图片位置,图片二进制"
                dgvList.DataSource = (bc.GetPictureInfo(GetWhere()))
                dgvList.Columns(3).Visible = False
            End If

            '5.治具 一览表
            If Me.rdoTools.Checked Then
                ' title = "治具名称,治具ID,条码,条码有无"
                dgvList.DataSource = (bc.GetToolsInfo(GetWhere()))
            End If


            '6.检查项目
            If Me.rdoXM.Checked Then

                Dim cn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

                If cn.IndexOf("AvoidMiss_Experiment") > 0 Then

                    ' title = "商品コード,作番,值1,值2,值3,值4,值5,值6,生产日期,出荷日期,数量"
                    dgvList.DataSource = (bc.GetXM(GetWhere()).Tables(0))
                Else

                    ' title = "商品コード,作番,实测值1,实测值2,生产日期,出荷日期,数量"
                    dgvList.DataSource = (bc.GetXM(GetWhere()).Tables(0))
                End If


            End If

            '7.新旧未检查一览表
            If Me.rdoMissCheckListNewAndOld.Checked Then
                ' title = "系统,生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,原因"
                Dim dt As DataTable = bc.GetMissCheckList(GetWhere())
                dgvList.DataSource = dt
            End If

            '8.新旧検査結果詳細表
            If Me.rdoNewOldCheckResultMs.Checked Then
                'Dim dt As DataTable = Nothing
                'Dim i As Integer = 0
                'title = "系统,商品名,商品コード,作番,生产实际日,完工数量,生产线,向先,纳期日,检查日期,检查者,治具编号,图片名称,图片链接,分类名称,检查顺序,类别,检查位置,检查项目,基准类型,基准值1,基准值2,基准值3,检查次数,方法,实测值1,实测值2,检查结果,备注,欠品,检查开始时间,检查终了时间,检查时长,检查种类"

                'dt = (bc.GetNewOldNewOldCheckResultMs(GetWhere()).Tables(0))
                'If dt.Rows.Count > 0 Then
                '    For i = 0 To dt.Rows.Count - 1
                '        If String.IsNullOrEmpty(dt.Rows(i).Item("picture_id").ToString) = False Then
                '            dt.Rows(i).Item("picture_content") = "双击打开"
                '        Else
                '            dt.Rows(0).Item("picture_content") = ""
                '        End If
                '    Next
                'End If
                'dgvList.DataSource = dt


                'title = "系统,生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,检查日期,检查开始时间,检查终了时间,检查时长,结果,详细,检查者,备注"

                Dim dt As DataTable = bc.GetNewOldNewOldCheckResultMs(GetWhere())
                dgvList.DataSource = dt

            End If


            If rbCheckMS.Checked Then
                Dim dt As DataTable = Nothing
                Dim i As Integer = 0
                'Dim cn As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

                'If cn.IndexOf("AvoidMiss_Experiment") > 0 Then
                '    title = "id,商品名,商品コード,作番,生产实际日,完工数量,生产线,向先,纳期日,检查日期,检查者,治具编号,图片名称,图片链接,分类名称,检查顺序,类别,检查位置,检查项目,基准类型,基准值1,基准值2,基准值3,检查次数,方法,值1,值2,值3,值4,值5,值6,检查结果,备注,欠品,检查开始时间,检查终了时间,检查时长,检查种类"

                'Else
                '    title = "id,商品名,商品コード,作番,生产实际日,完工数量,生产线,向先,纳期日,检查日期,检查者,治具编号,图片名称,图片链接,分类名称,检查顺序,类别,检查位置,检查项目,基准类型,基准值1,基准值2,基准值3,检查次数,方法,实测值1,实测值2,检查结果,备注,欠品,检查开始时间,检查终了时间,检查时长,检查种类"

                'End If
                dt = (bc.GetCheckMs(GetWhere()).Tables(0))
                'If dt.Rows.Count > 0 Then
                '    For i = 0 To dt.Rows.Count - 1
                '        If String.IsNullOrEmpty(dt.Rows(i).Item("picture_id").ToString) = False Then
                '            dt.Rows(i).Item("picture_content") = "双击打开"
                '        Else
                '            dt.Rows(0).Item("picture_content") = ""
                '        End If
                '    Next
                'End If

                Dim dicPictures As New Dictionary(Of String, String)





                For i = 0 To dt.Rows.Count - 1

                    Dim picId As String = dt.Rows(i).Item("picture_id").ToString
                    Dim picName As String
                    Dim savePictureName As String = Application.StartupPath & "\PICTURE_BK\"

                    If picId <> "" Then
                        If Not dicPictures.ContainsKey(picId) Then
                            dicPictures.Add(picId, "")

                            Dim imageByte() As Byte = DirectCast(bc.GetPictureContent(picId), Byte())

                            Dim sm As IO.MemoryStream = New IO.MemoryStream(imageByte, True)
                            Dim px As New PictureBox
                            px.Image = Image.FromStream(DirectCast(sm, System.IO.Stream))
                            picName = picId & ".jpg"
                            px.Image.Save(savePictureName & "\" & picName, px.Image.RawFormat)
                            sm.Close()
                        End If
                        dt.Rows(i).Item("picture_content") = "双击打开"
                    End If

                Next

                dgvList.DataSource = dt
                dicPictures.Clear()
            End If



            For i As Integer = 0 To kmTitle.Split(","c).Length - 1
                If dgvList.Columns.Count - 1 >= i Then
                    dgvList.Columns(i).HeaderText = kmTitle.Split(","c)(i)
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

 

    ''' <summary>
    ''' CSV出力按钮按下后
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCsv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCSV.Click

        Try
            exceltype = "导出Excel (*.csv)|*.csv"
            OpenExcel(True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' EXCEL2003出力按钮按下后
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnOutExcel2003_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOutExcel2003.Click

        Try
            exceltype = "导出Excel (*.xls)|*.xls"
            OpenExcel()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Excel2010出力按钮按下后
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnOutExcel2010_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOutExcel2010.Click

        Try
            exceltype = "导出Excel (*.xlsx)|*.xlsx"
            OpenExcel()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    '''' <summary>
    '''' 检查结果明细表条件设定
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub rdoJCJGMS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoJCJGMS.CheckedChanged
    '    Dim colstr As String = "商品名,商品コード,作番,生产实际日,完工数量,生产线,向先,纳期日,检查日期,检查者,治具编号,图片名称,分类名称,检查顺序,类别,检查位置,检查项目,基准类型,基准值1,基准值2,基准值3,检查次数,方法,检查结果,欠品,检查开始时间,检查终了时间,检查种类"
    '    SetWhere(colstr)
    'End Sub

    '''' <summary>
    '''' 未检查一览表条件设定
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub rdoWJCYL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoWJCYL.CheckedChanged
    '    SetWhere("生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,原因")
    'End Sub
    '''' <summary>
    '''' 未检查一览表条件设定 新旧系统
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub rdoMissCheckListNewAndOld_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoMissCheckListNewAndOld.CheckedChanged
    '    SetWhere("系统,生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,原因")
    'End Sub

    '''' <summary>
    '''' 检查项目条件设定
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub rdoXM_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoXM.CheckedChanged
    '    SetWhere("商品コード,作番,实测值1,实测值2,生产日期,出荷日期,数量")
    'End Sub

    '''' <summary>
    '''' 治具 一览表条件设定
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub rdoTools_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoTools.CheckedChanged
    '    SetWhere("治具名称,商品コード,治具ID,治具区分,条码,条码有无")
    'End Sub

    '''' <summary>
    '''' 检查结果汇总表条件设定
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub rdoCheckResult_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoCheckResult.CheckedChanged
    '    SetWhere("商品名,商品コード,作番,生产线,检查者,完工数量,生産日期,納品日期,检查合格,检查不合格,検査開始時間,検査終了時間")
    'End Sub

    '''' <summary>
    '''' 图片报表
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub rdoPicture_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoPicture.CheckedChanged
    '    SetWhere("商品コード,图片ID,图片名称,图片描述")
    'End Sub
#End Region

    Public Function GetBumonWhere() As String

        Dim a As New Generic.List(Of String)

        If Me.cbBumon1.Checked Then
            a.Add("'001'")
        End If
        If Me.cbBumon2.Checked Then
            a.Add("'002'")
        End If
        If Me.cbBumon3.Checked Then
            a.Add("'003'")
        End If
        If Me.cbBumon4.Checked Then
            a.Add("'004'")
        End If

        If a.Count > 0 Then
            Return "(" & String.Join(",", a.ToArray) & "'"
        Else
            Return ""
        End If

    End Function

    Public exceltype As String

    ''' <summary>
    ''' Excel 处理
    ''' </summary>
    ''' <param name="csvflg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function OpenExcel(Optional ByVal csvflg As Boolean = False) As Boolean

        Dim bc As New MsBaobiaoBizLogic
        If Me.rdoOr.Checked Then
            bc.JoinFlg = " or "
        Else
            bc.JoinFlg = " and "
        End If

        '1.検査結果詳細表
        If Me.rdoJCJGMS.Checked Then
            'Dim title As String = "商品名,商品コード,作番,生产实际日,完工数量,生产线,向先,纳期日,检查日期,检查者,治具编号,图片名称,图片链接,分类名称,检查顺序,类别,检查位置,检查项目,基准类型,基准值1,基准值2,基准值3,检查次数,方法,实测值1,实测值2,检查结果,备注,欠品,检查开始时间,检查终了时间,检查时长,检查种类"
            'Dim title As String = "生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,结果,详细"
            ExportExcel(bc.GetJCJGMS(GetWhere()), kmTitle, csvflg)
        End If

        '2.检查结果汇总表
        If Me.rdoCheckResult.Checked Then
            kmTitle = "商品名,商品コード,作番,生产线,检查者,完工数量,生産日期,納品日期,検査完了,默認結果,正在検査,检查合格,检查不合格,欠品"
            ExportExcel(bc.GetCheckResult(GetWhere()), kmTitle, csvflg)
        End If

        '3.未检查一览表
        If Me.rdoWJCYL.Checked Then
            'dgvWhere.DataSource = bc.GetWJCYL(GetWhere()).Tables(0)
            ' Dim title As String = "生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,原因"
            ExportExcel(bc.GetWJCYL(GetWhere()), kmTitle, csvflg)
        End If

        '4.图片 一览表
        If Me.rdoPicture.Checked Then
            ' Dim title As String = "图片名称,图片描述,图片位置"
            ExportExcel(bc.GetPictureInfo(GetWhere()), kmTitle, csvflg)
        End If

        '5.治具 一览表
        If Me.rdoTools.Checked Then
            ' Dim title As String = "治具名称,治具ID,条码,条码有无"
            ExportExcel(bc.GetToolsInfo(GetWhere()), kmTitle, csvflg)
        End If

        '6.检查项目
        If Me.rdoXM.Checked Then
            ' Dim title As String = "商品コード,作番,实测值1,实测值2,生产日期,出荷日期,数量"
            ExportExcel(bc.GetXM(GetWhere()).Tables(0), kmTitle, csvflg)
        End If

        '7.新旧未检查一览表
        If Me.rdoMissCheckListNewAndOld.Checked Then
            ' Dim title As String = "系统, 生产实际日, 商品名, 商品コード, 作番, 完工数量, 生产线, 向先, 纳期日, 原因"
            Dim dt As DataTable = bc.GetMissCheckList(GetWhere())
            ExportExcel(dt, kmTitle, csvflg)
        End If

        '8.新旧検査結果詳細表
        If Me.rdoNewOldCheckResultMs.Checked Then
            ' Dim title As String
            '  title = "系统,生产实际日,商品名,商品コード,作番,完工数量,生产线,向先,纳期日,检查日期,检查开始时间,检查终了时间,检查时长,结果,详细,检查者,备注"
            Dim dt As DataTable = bc.GetNewOldNewOldCheckResultMs(GetWhere())
            ExportExcel(dt, kmTitle, csvflg)
        End If
        If rbCheckMS.Checked Then
            '  Dim title As String
            ' title = "id,商品名,商品コード,作番,生产实际日,完工数量,生产线,向先,纳期日,检查日期,检查者,治具编号,图片名称,图片链接,分类名称,检查顺序,类别,检查位置,检查项目,基准类型,基准值1,基准值2,基准值3,检查次数,方法,值1,值2,值3,值4,值5,值6,检查结果,备注,欠品,检查开始时间,检查终了时间,检查时长,检查种类"

            Dim dt As DataTable = (bc.GetCheckMs(GetWhere()).Tables(0))
            Dim i As Integer = 0
            Dim dicPictures As New Dictionary(Of String, String)
            For i = 0 To dt.Rows.Count - 1
                Dim picId As String = dt.Rows(i).Item("picture_id").ToString
                Dim picName As String
                Dim savePictureName As String = Application.StartupPath & "\PICTURE_BK\"

                If picId <> "" Then
                    picName = picId & ".jpg"

                    If Not dicPictures.ContainsKey(picId) Then
                        dicPictures.Add(picId, "")

                        Dim imageByte() As Byte = DirectCast(bc.GetPictureContent(picId), Byte())

                        Dim sm As IO.MemoryStream = New IO.MemoryStream(imageByte, True)
                        Dim px As New PictureBox
                        px.Image = Image.FromStream(DirectCast(sm, System.IO.Stream))

                        px.Image.Save(savePictureName & "\" & picName, px.Image.RawFormat)
                        sm.Close()

                   
                    End If


                    dt.Rows(i).Item("picture_content") = savePictureName & "\" & picName

                Else
                    dt.Rows(i).Item("picture_content") = ""
                End If
            Next
            dicPictures.Clear()
            ExportExcel(dt, kmTitle, csvflg)


        End If
    End Function

    ''' <summary>
    ''' CSV 处理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExportCSV(ByVal dt As DataTable, ByVal title As String)

        Try
            Dim strFileName As String
            Dim saveFileDialog As SaveFileDialog

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



            If dt.Rows.Count < 1 Then
                MessageBox.Show(M00005I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            Dim savePictureName As String = String.Empty

            If Me.rdoPicture.Checked Then
                If strFileName.IndexOf(".csv") > 0 Then
                    savePictureName = Mid(strFileName, 1, strFileName.LastIndexOf(".csv")) & "\图片Images" & Now.ToString("yyyyMMddHHmmss")
                End If

                '若文件夹不存在则新建文件夹   
                If Not System.IO.Directory.Exists(savePictureName) Then
                    '新建文件夹  
                    System.IO.Directory.CreateDirectory(savePictureName)
                End If

                '写入图片()
                Dim i As Integer
                Dim picName As String = String.Empty
                For i = 0 To dt.Rows.Count - 1
                    If picName <> dt.Rows(i).Item("picture_nm").ToString Then
                        Dim imageByte() As Byte = DirectCast(dt.Rows(i).Item("picture_content"), Byte())
                        Dim sm As MemoryStream = New MemoryStream(imageByte, True)
                        Dim px As New PictureBox
                        px.Image = Image.FromStream(DirectCast(sm, System.IO.Stream))
                        picName = dt.Rows(i).Item("picture_nm").ToString & ".jpg"
                        px.Image.Save(savePictureName & "\" & picName, px.Image.RawFormat)
                        sm.Close()
                        picName = dt.Rows(i).Item("picture_nm").ToString
                    End If
                    dt.Rows(i).Item("picture_path") = savePictureName
                Next
            End If

            Dim sb As New System.Text.StringBuilder
            Dim t As System.IO.StreamWriter = New System.IO.StreamWriter(strFileName, False, System.Text.Encoding.UTF8)



            sb.AppendLine(title)

            If Me.rdoPicture.Checked = True Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    For j As Integer = 0 To dt.Columns.Count - 2
                        'sb.Append(dt.Rows(i).Item(j).ToString & ",")
                        t.Write(dt.Rows(i).Item(j).ToString & ",")
                    Next
                    t.Write(vbNewLine)
                    'sb.Append(vbNewLine)
                Next
            Else
                For i As Integer = 0 To dt.Rows.Count - 1
                    For j As Integer = 0 To dt.Columns.Count - 1
                        'sb.Append(dt.Rows(i).Item(j).ToString & ",")
                        t.Write(dt.Rows(i).Item(j).ToString & ",")
                    Next
                    t.Write(vbNewLine)
                    'sb.Append(vbNewLine)
                Next
            End If
            t.Close()


            'File.WriteAllText(strFileName, sb.ToString, System.Text.Encoding.UTF8)
            MessageBox.Show(M00004I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            System.Diagnostics.Process.Start(strFileName)



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 导出文件处理
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="title"></param>
    ''' <remarks></remarks>
    Private Sub ExportExcel(ByVal dt As DataTable, ByVal title As String, ByVal csvflg As Boolean)

        If csvflg Then
            ExportCSV(dt, title)
            Return
        End If

        Dim strFileName As String
        Dim saveFileDialog As SaveFileDialog

        saveFileDialog = New SaveFileDialog()
        saveFileDialog.Filter = exceltype
        saveFileDialog.FilterIndex = 0
        saveFileDialog.RestoreDirectory = True
        saveFileDialog.Title = "导出文件保存"
        saveFileDialog.ShowDialog()
        strFileName = saveFileDialog.FileName
        If String.IsNullOrEmpty(strFileName) Then
            Return
        End If
        If File.Exists(strFileName) Then
            MsgBox("文件已经存在！！ 请重新设定报表文件")
        End If



        If dt.Rows.Count < 1 Then
            MessageBox.Show(M00005I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim fileSaved As Boolean = False
        Dim savePictureName As String = String.Empty

        If Me.rdoPicture.Checked Then
            If strFileName.IndexOf(".xls") > 0 Then
                savePictureName = Mid(strFileName, 1, strFileName.LastIndexOf(".xls")) & "\图片Images" & Now.ToString("yyyyMMddHHmmss")
            ElseIf strFileName.IndexOf(".xlsx") > 0 Then
                savePictureName = Mid(strFileName, 1, strFileName.LastIndexOf(".xlsx")) & "\图片Images" & Now.ToString("yyyyMMddHHmmss")
            End If

            '若文件夹不存在则新建文件夹   
            If Not System.IO.Directory.Exists(savePictureName) Then
                '新建文件夹  
                System.IO.Directory.CreateDirectory(savePictureName)
            End If
        End If


        Dim xlApp = CreateObject("Excel.Application")
        If xlApp Is Nothing Then
            MessageBox.Show(M00021I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        Dim workbooks = xlApp.Workbooks
        Dim workbook = workbooks.Add(-4167)
        Dim worksheet = workbook.Worksheets(1)

        worksheet.Name = "DATA"

        Try
            Dim ts() As String = title.Split(","c)

            Dim range

            '列索引，行索引，总列数，总行数          
            Dim colIndex As Integer = 0
            Dim rowIndex As Integer = 0
            Dim colCount As Integer = dt.Columns.Count
            Dim rowCount As Integer = dt.Rows.Count
            Dim maxcol As Integer = ts.Length
            If maxcol < colCount Then
                maxcol = colCount
            End If

            If Me.rdoPicture.Checked Then
                '写入图片()
                Dim i As Integer
                Dim picName As String = String.Empty
                For i = 0 To rowCount - 1
                    If picName <> dt.Rows(i).Item("picture_nm").ToString Then
                        Dim imageByte() As Byte = DirectCast(dt.Rows(i).Item("picture_content"), Byte())
                        Dim sm As MemoryStream = New MemoryStream(imageByte, True)
                        Dim px As New PictureBox
                        px.Image = Image.FromStream(DirectCast(sm, System.IO.Stream))
                        picName = dt.Rows(i).Item("picture_nm").ToString & ".jpg"
                        px.Image.Save(savePictureName & "\" & picName, px.Image.RawFormat)
                        sm.Close()
                        picName = dt.Rows(i).Item("picture_nm").ToString
                    End If
                    dt.Rows(i).Item("picture_path") = savePictureName
                Next
            End If

            '创建缓存数据
            Dim objData(rowCount + 1, maxcol) As Object

            '获取具体数据
            For rowIndex = 0 To rowCount
                If rowIndex = 0 Then

                    For i As Integer = 0 To ts.Length - 1
                        objData(rowIndex, i) = ts(i)
                    Next
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

            If Me.rbCheckMS.Checked Then


   

                For i As Integer = 2 To rowCount + 1
                    Try
                        Dim v = CStr(xlApp.Cells(i, 14).Value)
                        worksheet.Hyperlinks.Add(xlApp.Cells(i, 14), CStr(v))
                    Catch ex As Exception

                    End Try
                Next





            End If



            Application.DoEvents()

            workbook.Saved = True

            fileSaved = True

            workbook.SaveCopyAs(strFileName)

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

            If fileSaved AndAlso File.Exists(strFileName) Then
                System.Diagnostics.Process.Start(strFileName)
            End If

            MessageBox.Show(M00004I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception
            Try
                workbooks.Close()
                xlApp.Quit()
                xlApp = Nothing

                fileSaved = False
                '若文件存在
                If System.IO.File.Exists(strFileName) Then
                    '删除文件
                    System.IO.File.Delete(strFileName)
                End If
                If Me.rdoPicture.Checked Then
                    '若文件夹存在
                    If System.IO.Directory.Exists(savePictureName) Then
                        '删除文件夹
                        System.IO.Directory.Delete(savePictureName)
                    End If
                End If
                MessageBox.Show(M00020I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Catch ex2 As Exception

            End Try

            Exit Sub
        End Try

    End Sub

    Public Sub SetWhere(ByVal colstr As String)
        If colstr = "" Then
            dgvWhere.DataSource = Nothing
            Exit Sub
        End If

        Dim arrcol() As String = colstr.Split(","c)
        Dim dt As New DataTable
        For i As Integer = 0 To arrcol.Length - 1
            dt.Columns.Add(arrcol(i))
        Next
        dgvWhere.DataSource = Nothing
        dgvWhere.DataSource = dt
    End Sub

    Public Function GetWhere() As String
        Dim where As String = ""
        If Me.dgvWhere.Rows.Count > 0 Then
            For i As Integer = 0 To Me.dgvWhere.ColumnCount - 1
                If i > 0 Then
                    where = where & ","
                End If
                where = where & GetValue(Me.dgvWhere.Rows(0).Cells(i).Value)
            Next
        End If
        Return where
    End Function

    Public Function GetValue(ByVal v As Object) As String
        If v Is Nothing Then
            Return ""
        Else
            Return v.ToString.Trim
        End If
    End Function


#Region "图片处理"

    ''' <summary>
    ''' 导出图片以及文件处理
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="title"></param>
    ''' <remarks></remarks>
    Private Sub ExportPictureExcel(ByVal dt As DataTable, ByVal title As String, ByVal csvflg As Boolean)


        If csvflg Then
            ExportCSV(dt, title)
            Return
        End If

        Dim strFileName As String
        Dim strFilter As String
        Dim saveFileDialog As SaveFileDialog

        saveFileDialog = New SaveFileDialog()
        saveFileDialog.Filter = exceltype
        saveFileDialog.FilterIndex = 0
        saveFileDialog.RestoreDirectory = True
        saveFileDialog.Title = "导出文件保存"
        saveFileDialog.ShowDialog()
        strFileName = saveFileDialog.FileName
        strFilter = Path.GetDirectoryName(strFileName)
        If String.IsNullOrEmpty(strFileName) Then
            Return
        End If
        If File.Exists(strFileName) Then
            MsgBox("文件已经存在！！ 请重新设定报表文件")
            Exit Sub
        End If

        '取得情报有无判断
        If dt.Rows.Count < 1 Then
            MessageBox.Show(M00005I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim fileSaved As Boolean = False

        Dim xlApp = CreateObject("Excel.Application")
        If xlApp Is Nothing Then
            MessageBox.Show(M00021I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
        Dim workbooks = xlApp.Workbooks
        Dim workbook = workbooks.Add(-4167)
        Dim worksheet = workbook.Worksheets(1)
        worksheet.Name = "DATA"

        '图片文件夹定义
        Dim saveFolderName As String = strFilter & "\Image" & Now.ToString("yyyyMMddHHmmss")

        Try
            '若文件夹不存在则新建文件夹   
            If Not System.IO.Directory.Exists(saveFolderName) Then
                '新建文件夹  
                System.IO.Directory.CreateDirectory(saveFolderName)
            End If

            Dim ts() As String = title.Split(","c)

            Dim range

            '列索引，行索引，总列数，总行数          
            Dim colIndex As Integer = 0
            Dim rowIndex As Integer = 0
            Dim colCount As Integer = dt.Columns.Count
            Dim rowCount As Integer = dt.Rows.Count
            Dim maxcol As Integer = ts.Length
            If maxcol < colCount Then
                maxcol = colCount
            End If

            '写入图片
            Dim i As Integer
            Dim picNm As String = String.Empty
            Dim picName As String = String.Empty
            Dim picContentName As String = String.Empty
            For i = 0 To rowCount - 1
                If picNm <> dt.Rows(i).Item("picture_nm").ToString Then
                    Dim imageByte() As Byte = DirectCast(dt.Rows(i).Item("picture_content"), Byte())
                    Dim sm As MemoryStream = New MemoryStream(imageByte, True)
                    Dim px As New PictureBox
                    px.Image = Image.FromStream(DirectCast(sm, System.IO.Stream))
                    picName = dt.Rows(i).Item("picture_nm").ToString & ".jpg"
                    px.Image.Save(saveFolderName & "\" & picName, px.Image.RawFormat)
                    sm.Close()
                    picNm = dt.Rows(i).Item("picture_nm").ToString
                End If
                dt.Rows(i).Item("picture_path") = saveFolderName & "\" & picName
            Next

            '创建缓存数据
            Dim objData(rowCount + 1, maxcol) As Object

            '获取具体数据
            For rowIndex = 0 To rowCount
                If rowIndex = 0 Then

                    For i = 0 To ts.Length - 1
                        objData(rowIndex, i) = ts(i)
                    Next
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

            workbook.Saved = True

            fileSaved = True

            workbook.SaveCopyAs(strFileName)

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

            If fileSaved AndAlso File.Exists(strFileName) Then
                System.Diagnostics.Process.Start(strFileName)
            End If

            MessageBox.Show(M00004I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception
            Try
                workbooks.Close()
                xlApp.Quit()
                xlApp = Nothing

                fileSaved = False
                '若文件存在
                If System.IO.File.Exists(strFileName) Then
                    '删除文件
                    System.IO.File.Delete(strFileName)
                End If

                '如果文件夹存在
                If System.IO.Directory.Exists(saveFolderName) Then
                    '删除文件夹
                    System.IO.Directory.Delete(saveFolderName)
                End If

                MessageBox.Show(M00020I, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Catch ex2 As Exception

            End Try

            Exit Sub
        End Try

    End Sub

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
    ''' 数据行双击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvList_CellMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvList.CellMouseDoubleClick

        Dim IMGID As String = String.Empty
        If CType(sender, DataGridView).Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString = "双击打开" Then
            '图片取得
            IMGID = CType(sender, DataGridView).Rows(e.RowIndex).Cells(e.ColumnIndex - 1).Value.ToString & ".jpg"

            Dim savePictureName As String = Application.StartupPath & "\PICTURE_BK\" & IMGID


            ShowMaxPicture(Image.FromFile(savePictureName))
        End If

        '图片ID
        ''初始值记录（判断变化用）
        'Dim dtImg As New DataTable
        'Dim IMGID As String = String.Empty
        'If dgvList.CurrentCell.ColumnIndex = 12 AndAlso dgvList.CurrentRow.Cells(12).Value.ToString = "双击打开" Then
        '    '图片取得
        '    IMGID = dgvList.CurrentRow.Cells(11).Value.ToString
        '    '图片表示
        '    dtImg = bc.GetPictureContent(IMGID)
        '    If dtImg.Rows.Count > 0 Then
        '        '大图显示
        '        ShowMaxPicture(GetImageFromByteArray(DirectCast(dtImg.Rows(0).Item("picture_content"), Byte())))
        '    End If

        'End If

    End Sub

    ''' <summary>
    ''' 显示发 大图     
    ''' </summary>
    ''' <param name="picture"></param>
    ''' <remarks></remarks>
    Public Sub ShowMaxPicture(ByVal picture As Image)

        Try
            Dim frmMaxImage As Form
            Dim maxPictureBox As PictureBox
            Dim frmName As String = "frmMaxPicture"

            '如果图 片放大窗口已经 打开 了，那么 先关 ?掉
            CloseMaxImage(Me, EventArgs.Empty)

            frmMaxImage = New Form '新建立一个form窗口
            maxPictureBox = New PictureBox '新建立一个图 片显 示控件
            frmMaxImage.Name = frmName
            With picture
                frmMaxImage.Width = .Width
                frmMaxImage.Height = .Height + 36
                maxPictureBox.Width = .Width
                maxPictureBox.Height = .Height
            End With
            maxPictureBox.Image = picture '设置图 片显 示控件的图 片，来自形参
            frmMaxImage.Controls.Add(maxPictureBox) '将图 片控件加入到form窗口中

            AddHandler frmMaxImage.LostFocus, AddressOf CloseMaxImage 'form失去焦点后关闭大图

            frmMaxImage.Show() '显示form窗口
        Catch ex As Exception

        End Try

    End Sub
    ''' <summary>
    ''' 关闭大图
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CloseMaxImage(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim frmName As String = "frmMaxPicture"
        Try
            For Each myForm As Form In Application.OpenForms

                If myForm.Name = frmName Then
                    myForm.Close()
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub

#End Region




    Private Sub msBaobiao_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Height = 1000
        Me.Width = My.Computer.Screen.Bounds.Width - 10
        dgvWhere.Width = My.Computer.Screen.Bounds.Width - 40
        dgvList.Width = My.Computer.Screen.Bounds.Width - 40
        dgvList.Height = 700
        Me.Location = New Point(1, 1)


        Me.Location = New Point(1, 1)
        Me.Height = My.Computer.Screen.Bounds.Height - 40
        Me.Width = My.Computer.Screen.Bounds.Width - 10
        Me.AutoScroll = True


    End Sub
End Class