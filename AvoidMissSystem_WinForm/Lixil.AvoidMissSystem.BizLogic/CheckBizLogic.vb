Imports System.Transactions
Imports Lixil.AvoidMissSystem.DataAccess

Public Class CheckBizLogic
    'DA层实例化
    Dim da As New CheckDA

#Region "检查主页面用"


    ''' <summary>
    ''' 分类列表取得
    ''' </summary>
    ''' <param name="strGoodsId"></param>
    ''' <param name="strToolsCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClassifyList(ByVal strGoodsId As String, Optional ByVal strToolsCd As String = "") As DataSet
        If strToolsCd.Equals("") Then
            Return da.GetClassifyList(strGoodsId)
        Else
            Return da.GetClassifyList(strGoodsId, strToolsCd)
        End If

    End Function

    ''' <summary>
    ''' 治具列表的取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckToolsOK(ByVal result_id As String, ByVal tools_id As String) As Integer
        Return da.CheckToolsOK(result_id, tools_id)
    End Function

    ''' <summary>
    ''' 治具列表的取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckToolsHaveData(ByVal result_id As String, ByVal tools_id As String) As Boolean
        Return da.CheckToolsHaveData(result_id, tools_id)
    End Function

    ''' <summary>
    ''' 治具列表的取得
    ''' </summary>
    ''' <param name="strGoodsId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetToolsNoList(ByVal strGoodsId As String) As DataSet
        Return da.GetToolsNoList(strGoodsId)
    End Function

    ''' <summary>
    ''' 同批次的检查结果信息取得
    ''' </summary>
    ''' <param name="strMakeNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLastReultInfo(ByVal strGoodsCode As String, ByVal strMakeNo As String) As DataSet
        Return da.GetLastReultInfo(strGoodsCode, strMakeNo)
    End Function

    ''' <summary>
    ''' 同批次的检查结果信息取得
    ''' </summary>
    ''' <param name="strMakeNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLastReultInfoFromt_check_result(ByVal strGoodsCode As String, ByVal strMakeNo As String) As DataSet
        Return da.GetLastReultInfoFromt_check_result(strGoodsCode, strMakeNo)
    End Function

    ''' <summary>
    ''' 根据商品code取得商品ID
    ''' </summary>
    ''' <param name="strGoodsCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetGoodsIdByGoodsCd(ByVal strGoodsCd As String) As DataSet
        Return da.GetGoodsIdByGoodsCd(strGoodsCd)
    End Function

    ''' <summary>
    ''' 保存结果
    ''' </summary>
    ''' <param name="UpOrInflg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveResult(ByVal UpOrInflg As Integer, ByVal hsSearch As Hashtable) As Integer
        If UpOrInflg = 0 Then
            '插入处理
            Return da.InsertResult(hsSearch)
        ElseIf UpOrInflg = 1 Then
            '更新处理
            Return da.UpdateResult(hsSearch)
        End If
    End Function

    ''' <summary>
    ''' 获得部门信息
    ''' </summary>
    ''' <param name="strGoodsCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDepartmentInfo(ByVal strGoodsCd As String) As DataSet
        Return da.GetDepartmentInfo(strGoodsCd)
    End Function

    ''' <summary>
    ''' 获得同作番检查次数
    ''' </summary>
    ''' <param name="strGoodsId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChkTimesInfo(ByVal strGoodsId As String, ByVal strMakeNo As String) As DataSet
        Return da.GetChkTimesInfo(strGoodsId, strMakeNo)
    End Function
    ''' <summary>
    ''' 继续检查信息获得
    ''' </summary>
    ''' <param name="strGoodsID"></param>
    ''' <param name="strMakeNo"></param>
    ''' <param name="strDateNow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetContinueChkInfo(ByVal strGoodsID As String, ByVal strMakeNo As String, ByVal strDateNow As String) As DataSet
        Return da.GetContinueChkInfo(strGoodsID, strMakeNo, strDateNow)
    End Function

    ''' <summary>
    ''' 该分类下结果取得
    ''' </summary>
    ''' <param name="strGoodsId"></param>
    ''' <param name="strResultId"></param>
    ''' <param name="strClassfyId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChkDetailResult(ByVal strGoodsId As String, ByVal strResultId As String, ByVal strClassfyId As String) As DataSet
        Return da.GetChkDetailResult(strGoodsId, strResultId, strClassfyId)
    End Function
    ''' <summary>
    ''' 用户信息取得
    ''' </summary>
    ''' <param name="strUserCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCheckUser(ByVal strUserCd As String) As Boolean
        Dim dt As DataTable = da.GetCheckUser(strUserCd).Tables("UserMsg")
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region

#Region "检查详细页面用"

    ''' <summary>
    ''' 检查项目详细信息取得
    ''' </summary>
    ''' <param name="hsSearch"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChkDetailInfo(ByVal hsSearch As Hashtable) As DataSet
        Return da.GetChkDetailInfo(hsSearch)
    End Function

    ''' <summary>
    ''' 根据图片ID取得图片二进制
    ''' </summary>
    ''' <param name="strPicId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPicInfoById(ByVal strPicId As String) As DataSet
        Return da.GetPicInfoById(strPicId)
    End Function
    ''' <summary>
    ''' 根据检查ID获取检查结果信息
    ''' </summary>
    ''' <param name="strResultId"></param>
    ''' <param name="strChkId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckDetailInfoHave(ByVal strResultId As String, ByVal strChkId As String, ByVal strDisno As String) As Boolean
        Dim dt As DataTable = da.GetDetailInfoByChkId(strResultId, strChkId, strDisno).Tables("DetaileInfo")
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function ResultDetaileInOrUp(ByVal strType As String, ByVal hsValues As Hashtable) As Integer
        If strType.Equals("INSERT") Then
            '插入处理
            Return da.InsertResultDetaile(hsValues)
        Else
            '更新处理
            Return da.UpdateResultDetail(hsValues)
        End If

    End Function
#End Region
    Public Function Updatet_check_resultQinapinFlg(ByVal id As String, ByVal qianpin_flg As String) As Integer
        Return da.Updatet_check_resultQinapinFlg(id, qianpin_flg)
    End Function

    Public Function GetQianpinInfo(ByVal id As String) As Boolean
        Return da.GetQianpinInfo(id)
    End Function

End Class
