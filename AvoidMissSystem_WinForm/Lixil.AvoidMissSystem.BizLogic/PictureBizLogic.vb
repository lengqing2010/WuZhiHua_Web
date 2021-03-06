Imports Lixil.AvoidMissSystem.DataAccess
Imports System.Transactions
Imports System.Net
Imports System.IO
Imports Lixil.AvoidMissSystem.Utilities.Consts

Public Class PictureBizLogic

    Dim picDA As New PictureDA
    Private ComBizLogic As CommonBizLogic

    ''' <summary>
    ''' 权限取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuthority(ByVal userId As String) As DataSet
        Return picDA.GetAuthority(userId)
    End Function

    ''' <summary>
    ''' 管理员权限管理部门信息取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAdminDepartment() As DataSet
        Return picDA.GetAdminDepartment()
    End Function

    ''' <summary>
    ''' 部门权限部门信息取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserDepartment(ByVal userId As String) As DataSet
        Return picDA.GetUserDepartment(userId)
    End Function

    ''' <summary>
    ''' 图片信息取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPictureData(ByVal goodsCd As String _
                                 , ByVal department As String _
                                 , ByVal picId As String _
                                 , ByVal picNm As String _
                                 , ByVal picName As String) As DataSet
        Return picDA.GetPictureData(goodsCd, department, picId, picNm, picName)
    End Function

    ''' <summary>
    ''' 根据ID取得图片
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPictureById(ByVal picId As String) As DataSet
        Return picDA.GetPictureById(picId)
    End Function

    ''' <summary>
    ''' 图片信息添加
    ''' </summary>
    ''' <param name="hsSearch"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertPicture(ByVal hsSearch As Hashtable) As Boolean

        Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction




        If String.IsNullOrEmpty(hsSearch("goodsId").ToString) = True Then
            '图片做成
            If picDA.InsertPicture(PerDbTraneAction, hsSearch) = 1 Then
                PerDbTraneAction.CloseCommit()
                Return True
            Else
                PerDbTraneAction.CloseRollback()
                Return False
            End If
        Else
            '图片做成，商品更新
            If picDA.InsertPicture(PerDbTraneAction, hsSearch) = 1 AndAlso picDA.UpdateClassify(PerDbTraneAction, hsSearch) >= 1 Then
                PerDbTraneAction.CloseCommit()
                Return True
            Else
                PerDbTraneAction.CloseRollback()
                Return False
            End If
        End If

        Return True
    End Function

    ''' <summary>
    ''' 图片更新
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="strUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdatePicture(ByVal dt As DataTable, ByVal strUser As String, _
                                  ByVal updPara As Dictionary(Of String, String), ByRef updCnt As Integer) As Boolean

        Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction


        Dim i As Integer
        Dim flg As Boolean = False
        Dim hstb As New Hashtable
        For i = 0 To dt.Rows.Count - 1
            hstb.Clear()
            hstb.Add("picId", dt.Rows(i).Item("picture_id").ToString)
            If updPara.ContainsKey(PDEPARTMENT) Then
                hstb.Add("departmentCd", updPara(PDEPARTMENT))
            Else
                hstb.Add("departmentCd", dt.Rows(i).Item("department_cd").ToString)
            End If

            If updPara.ContainsKey(PREMARKS) Then
                hstb.Add("picName", updPara(PREMARKS))
            Else
                hstb.Add("picName", dt.Rows(i).Item("picture_name").ToString)
            End If

            hstb.Add("user", strUser)
            If picDA.UpdatePicture(PerDbTraneAction, hstb) = 1 Then
                flg = True
                updCnt = updCnt + 1
            Else
                flg = False
                Exit For
            End If
        Next
        If flg = True Then
            PerDbTraneAction.CloseCommit()
            Return True
        Else
            PerDbTraneAction.CloseRollback()
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' 图片删除
    ''' </summary>
    ''' <param name="hsSearch"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeletePicture(ByVal hsSearch As Hashtable) As Boolean

        Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction

        If String.IsNullOrEmpty(hsSearch("picId").ToString) = False Then
            '图片删除
            If picDA.DeletePicture(PerDbTraneAction, hsSearch) >= 1 Then
                PerDbTraneAction.CloseCommit()
                Return True
            Else
                PerDbTraneAction.CloseRollback()
                Return False
            End If
        End If

        Return True
    End Function

    ''' <summary>
    ''' 图片导入
    ''' </summary>
    ''' <param name="dtExcel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsOrUpdPicture(ByVal dtExcel As DataTable, ByVal strUser As String, ByVal hstb As Hashtable, _
                                    ByRef afterUpdDt As DataTable) As Boolean


        Dim sysTime As DateTime
        Dim updFlg As Boolean = False


        Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction


        ComBizLogic = New CommonBizLogic
        sysTime = ComBizLogic.GetSystemDate()

        For i As Integer = 0 To dtExcel.Rows.Count - 1
            If picDA.InsOrUpdPicture(PerDbTraneAction, dtExcel.Rows(i), strUser, hstb, sysTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), updFlg) <> 1 Then
                Return False
            Else
                If updFlg = True Then
                    '更新的时候，更新成功的数据保存到更新后的Datatable
                    afterUpdDt.Rows.Add(dtExcel.Rows(i).ItemArray)
                End If
            End If
        Next

        If PerDbTraneAction.result = True Then
            PerDbTraneAction.CloseCommit()
        Else
            PerDbTraneAction.CloseRollback()
        End If


        Return True
    End Function

    ''' <summary>
    ''' 图片导出
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExportPictureData(ByVal goodsId As String _
                                     , ByVal department As String _
                                     , ByVal picId As String _
                                     , ByVal picNm As String _
                                     , ByVal picName As String, ByVal pageIdx As Integer) As DataSet
        Return picDA.ExportPictureData(goodsId, department, picId, picNm, picName, pageIdx)
    End Function

    ''' <summary>
    ''' 商品CD取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetGoodsCdList(ByVal strPictureId As String) As DataSet
        Return picDA.GetGoodsCdList(strPictureId)
    End Function

    ''' <summary>
    ''' 图片ID取得
    ''' </summary>
    ''' <param name="picId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPictureId(ByVal picId As String) As DataTable
        Return picDA.GetPictureId(picId).Tables(0)
    End Function

End Class
