Imports System.IO
Imports System.Windows

'Namespace Common

''' <summary>
''' 配置文件内容取得
''' </summary>
''' <remarks></remarks>
Public Class GetConfig

    Public Shared Function GetConfigVal(ByVal key As String) As String

        Dim strVal As String = String.Empty
        Dim tableName As String = "TABLEB"
        Dim xmlPath As String = Forms.Application.StartupPath & "/Maintenance_Config.txt"
        Dim reader As New Xml.XmlTextReader(xmlPath)

        If File.Exists(xmlPath) = False Then
            Return String.Empty
        End If

        While (reader.Read())
            If (reader.IsStartElement()) Then
                Dim doc As New Xml.XmlDocument
                doc.Load(reader)
                Dim Table As Xml.XmlNode = doc.SelectSingleNode(tableName)
                Try
                    strVal = Table.Item(key).InnerText
                Catch ex As Exception
                    Return String.Empty
                End Try
            End If
        End While

        Return strVal
    End Function

End Class
'End Namespace

