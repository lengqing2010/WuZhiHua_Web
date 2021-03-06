Imports Lixil.AvoidMissSystem.DataAccess
Imports Lixil.AvoidMissSystem.Utilities
Imports Lixil.AvoidMissSystem.Utilities.Consts

Public Class MsBaobiaoBizLogic
    Dim da As New MsBaobiaoDA
    Public JoinFlg As String = " or "




    ''' <summary>
    ''' 未检查一览表
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWJCYL(ByVal where As String) As DataTable
        da.JoinFlg = JoinFlg
        Return da.GetWJCYL(where)
    End Function


    ''' <summary>
    ''' 未检查一览表 新 旧
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMissCheckList(ByVal where As String) As DataTable
        da.JoinFlg = JoinFlg
        Return da.GetMissCheckList(where)
    End Function

    ''' <summary>
    ''' 検査結果詳細情報
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNewOldNewOldCheckResultMs(ByVal where As String) As DataTable
        da.JoinFlg = JoinFlg
        Return da.GetNewOldNewOldCheckResultMs(where)
    End Function

    ''' <summary>
    ''' 検査結果詳細情報
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetJCJGMS(ByVal where As String) As DataTable
        da.JoinFlg = JoinFlg
        Return da.GetJCJGMS(where)
    End Function


    Public Function GetCheckMs(ByVal where As String) As DataSet
        da.JoinFlg = JoinFlg
        Return da.GetCheckMs(where)
    End Function


    ''' <summary>
    ''' 检查项目
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetXM(ByVal where As String) As DataSet
        da.JoinFlg = JoinFlg
        Return da.GetXM(where)
    End Function

    ''' <summary>
    ''' 治具情報
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetToolsInfo(ByVal where As String) As DataTable
        da.JoinFlg = JoinFlg
        Return da.GetToolsInfo(where)
    End Function

    ''' <summary>
    ''' 检查结果汇总表取得
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCheckResult(ByVal where As String) As DataTable
        da.JoinFlg = JoinFlg
        Return da.GetCheckResult(where)
    End Function

    ''' <summary>
    ''' 图片情報
    ''' </summary>
    ''' <param name="where"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPictureInfo(ByVal where As String) As DataTable
        da.JoinFlg = JoinFlg
        Return da.GetPictureInfo(where)
    End Function


    Public Function GetPictureContent(ByVal id As String) As Object
        Return da.GetPictureContent(id)
    End Function
End Class
