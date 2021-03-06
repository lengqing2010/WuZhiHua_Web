
Imports System.Runtime.InteropServices.Marshal
Imports System.IO
Imports System.Windows.Forms
Imports System.Data

''' <summary>
''' Log文件Excel导出处理
''' </summary>
''' <remarks></remarks>
Public Class LogExport

    ''' <summary>
    ''' Log文件Excel导出处理
    ''' </summary>
    ''' <param name="kbn">机能区分</param>
    ''' <param name="dtError">错误信息Dataset</param>
    ''' <param name="dtUpdBefore">更新前Dataset</param>
    ''' <param name="dtUpdAfter">更新后Dataset</param>
    ''' <param name="updDate">操作时间</param>
    ''' <returns>导出文件存放的路径</returns>
    ''' <remarks></remarks>
    Public Shared Function LogExport(ByVal kbn As String, _
                              ByVal dtError As DataTable, _
                              ByVal dtUpdBefore As DataTable, _
                              ByVal dtUpdAfter As DataTable, _
                              ByVal updDate As DateTime) As String

        Dim strFileName As String
        Dim strFilePath As String
        Dim strDBFilePath As String = String.Empty
        'Dim xlBook 
        'Dim delSheet1 
        'Dim delSheet2 
        'Dim delSheet3 
        'Dim xlSheetError 
        'Dim xlSheetUpdBefore 
        'Dim xlSheetUpdAfter 
        'Dim xlApp
        'Dim IntRowIndex As Integer = 1
        'Dim IntColIndex As Integer = 0
        'Dim dtCol As DataColumn
        'Dim dtRow As DataRow
        Dim folderName As String = String.Empty

        Try
            '文件名设定
            strFileName = kbn & String.Format("{0:yyyyMMddHHmmss}", updDate)
            '文件夹名称设定
            folderName = Format(updDate, "yyyyMMdd")
            '文件夹存在判定
            If System.IO.Directory.Exists(GetConfig.GetConfigVal("FileExportPath") & "\" & folderName) = False Then
                '文件夹不存在的时候，新建一个文件夹???????
                My.Computer.FileSystem.CreateDirectory(GetConfig.GetConfigVal("FileExportPath") & "\" & folderName)
            End If

            '文件保存路径
            'strFilePath = GetConfig.GetConfigVal("FileExportPath") & "\" & folderName & "\" & strFileName & ".xls"
            strFilePath = GetConfig.GetConfigVal("FileExportPath") & "\" & folderName & "\"

            'xlApp = CreateObject("Excel.Application")
            'xlApp.Visible = False
            'xlBook = xlApp.Workbooks.Add()

            'xlBook.Worksheets(1).Delete()

            '导出数据设定
            If dtUpdBefore.Rows.Count > 0 Then
                ''更新前信息sheet
                'xlSheetUpdBefore = xlBook.Worksheets.Add()
                'xlSheetUpdBefore.Name = "更新前信息"
                'xlSheetUpdBefore.Activate()
                ''xlSheetUpdBefore.Range("A1").Resize(rowCount + 1, colCount).Value = dtUpdBefore
                ''***将所得到的表的列名,赋值给单元格***  
                'For Each dtCol In dtUpdBefore.Columns
                '    IntColIndex = IntColIndex + 1
                '    xlSheetUpdBefore.Cells(1, IntColIndex) = dtCol.ColumnName
                'Next
                ''-------------------------------------  
                ''****将得到的表所有行,赋值给单元格*****  s
                'For Each dtRow In dtUpdBefore.Rows
                '    IntRowIndex = IntRowIndex + 1
                '    IntColIndex = 0
                '    For Each dtCol In dtUpdBefore.Columns
                '        IntColIndex = IntColIndex + 1
                '        'xlSheetUpdBefore.Cells.NumberFormatLocal = "@"
                '        'xlSheetUpdBefore.Cells(IntRowIndex, IntColIndex).value = dtRow(dtCol.ColumnName).ToString
                '    Next
                'Next

                dtUpdBefore.WriteXml(strFilePath & strFileName & "BFUPD" & ".xml")

                strDBFilePath = strDBFilePath & strFileName & "BFUPD" & ".xml" & " "
            End If
            'IntRowIndex = 1
            'IntColIndex = 0


            If dtUpdAfter.Rows.Count > 0 Then
                ''更新后信息sheet
                'xlSheetUpdAfter = xlBook.Worksheets.Add()
                'xlSheetUpdAfter.Name = "更新后信息"
                'xlSheetUpdAfter.Activate()
                ''xlSheetUpdAfter.Range("A1").Resize(rowCount + 1, colCount).Value = dtUpdAfter
                ''***将所得到的表的列名,赋值给单元格***  
                'For Each dtCol In dtUpdAfter.Columns
                '    IntColIndex = IntColIndex + 1
                '    xlSheetUpdAfter.Cells(1, IntColIndex) = dtCol.ColumnName
                'Next
                ''-------------------------------------  
                ''****将得到的表所有行,赋值给单元格*****  
                'For Each dtRow In dtUpdAfter.Rows
                '    IntRowIndex = IntRowIndex + 1
                '    IntColIndex = 0
                '    For Each dtCol In dtUpdAfter.Columns
                '        IntColIndex = IntColIndex + 1
                '        xlSheetUpdAfter.Cells.NumberFormatLocal = "@"
                '        xlSheetUpdAfter.Cells(IntRowIndex, IntColIndex) = dtRow(dtCol.ColumnName).ToString
                '    Next
                'Next

                dtUpdAfter.WriteXml(strFilePath & strFileName & "AFUPD" & ".xml")

                strDBFilePath = strDBFilePath & strFileName & "AFUPD" & ".xml" & " "
            End If
            'IntRowIndex = 1
            'IntColIndex = 0

            If dtError.Rows.Count > 0 Then
                ''错误信息sheet
                'xlSheetError = xlBook.Worksheets.Add()
                'xlSheetError.Name = "错误信息"
                'xlSheetError.Activate()
                ''***将所得到的表的列名,赋值给单元格***  
                'For Each dtCol In dtError.Columns
                '    IntColIndex = IntColIndex + 1
                '    xlSheetError.Cells(1, IntColIndex) = dtCol.ColumnName
                'Next
                ''-------------------------------------  
                ''****将得到的表所有行,赋值给单元格*****  
                'For Each dtRow In dtError.Rows
                '    IntRowIndex = IntRowIndex + 1
                '    IntColIndex = 0
                '    For Each dtCol In dtError.Columns
                '        IntColIndex = IntColIndex + 1
                '        xlSheetError.Cells.NumberFormatLocal = "@"
                '        xlSheetError.Cells(IntRowIndex, IntColIndex) = dtRow(dtCol.ColumnName).ToString
                '    Next
                'Next

                dtError.WriteXml(strFilePath & strFileName & "ERROR" & ".xml")

                strDBFilePath = strDBFilePath & strFileName & "ERROR" & ".xml" & " "
            End If

            If strDBFilePath <> String.Empty Then
                strDBFilePath = strFilePath & strDBFilePath.TrimEnd(" ")
            End If
            'IntRowIndex = 1
            'IntColIndex = 0

            'delSheet1 = CType(xlBook.Worksheets("sheet1"), Excel.Worksheet)
            'delSheet1.Delete()
            'delSheet2 = CType(xlBook.Worksheets("sheet2"), Excel.Worksheet)
            'delSheet2.Delete()
            'delSheet3 = CType(xlBook.Worksheets("sheet3"), Excel.Worksheet)
            'delSheet3.Delete()

            'If xlBook.Worksheets.Count > 0 Then
            '    xlBook.SaveAs(strFilePath)
            'Else
            '    strFilePath = String.Empty  '没有错误数据和更新数据，未导出Excel
            'End If

            'xlBook.Close()
            'xlApp.Quit()
            'MessageBox.Show("批量导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Throw ex
        Finally
            'If Not xlSheetError Is Nothing Then
            '    ReleaseComObject(xlSheetError)
            'End If
            'If Not xlSheetUpdBefore Is Nothing Then
            '    ReleaseComObject(xlSheetUpdBefore)
            'End If
            'If Not xlSheetUpdAfter Is Nothing Then
            '    ReleaseComObject(xlSheetUpdAfter)
            'End If
            'If Not xlBook Is Nothing Then
            '    ReleaseComObject(xlBook)
            'End If
            'If Not xlApp Is Nothing Then
            '    ReleaseComObject(xlApp)
            'End If
        End Try
        Return strDBFilePath
    End Function

    ''' <summary>
    ''' 获取全部sheet的名称
    ''' </summary>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAllSheetName(ByVal strFilePath As String) As String()

        Dim conn As OleDb.OleDbConnection = New OleDb.OleDbConnection(GetOleDbConn(strFilePath))
        conn.Open()
        Dim sheetNames(conn.GetSchema("Tables").Rows.Count - 1) As String
        For i As Integer = 0 To conn.GetSchema("Tables").Rows.Count - 1
            sheetNames(i) = conn.GetSchema("Tables").Rows(i)("TABLE_NAME").ToString
        Next
        conn.Close()
        Return sheetNames
    End Function


    Public Shared Function GetOleDbConn(ByVal strFilePath As String) As String

        If GetExcelVersion() <= "11.0" Then
            GetOleDbConn = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;IMEX=1""", strFilePath)
        Else
            GetOleDbConn = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES""", strFilePath)
            'GetOleDbConn = String.Format("Provider=Microsoft.ACE.OLEDB.16.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES""", strFilePath)
            'GetOleDbConn = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;IMEX=1""", strFilePath)

        End If

    End Function

    Public Shared ExcelVersion As String = ""
    Public Shared Function GetExcelVersion() As String
        If ExcelVersion = "" Then
            Dim xlApp = CreateObject("Excel.Application")
            ExcelVersion = xlApp.Version
            xlApp.Quit()
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)
            xlApp = Nothing
            Return ExcelVersion
        End If

        Return ExcelVersion

    End Function



End Class
