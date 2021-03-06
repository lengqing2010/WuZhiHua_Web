Imports System.Transactions
Imports Lixil.AvoidMissSystem.DataAccess
Imports Lixil.AvoidMissSystem.Utilities

''' <summary>
''' 権限MS用BizLogic
''' </summary>
''' <remarks></remarks>
Public Class MsMaintenanceAuthorityBizLogic
    Dim authorityDa As New MsMaintenanceAuthorityDA
    Private ComBizLogic As New CommonBizLogic

    ''' <summary>
    ''' 取得用戸信息一覧
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserInfoList(ByVal dicSearch As Dictionary(Of String, String)) As DataSet
        Return authorityDa.GetUserInfoList(dicSearch)
    End Function

    ''' <summary>
    ''' 取得権限信息一覧
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuthorityList(ByVal userId As String, ByVal deleteFlg As String) As DataSet
        Return authorityDa.GetAuthorityList(userId, deleteFlg)
    End Function

    ''' <summary>
    '''  更新用戸信息
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateUserInfo(ByVal userId As String, ByVal password As String) As Boolean
        Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction

        Try
            If authorityDa.UpdateUserInfo(PerDbTraneAction, userId, password) <> 1 Then
                PerDbTraneAction.CloseRollback()
                Return False
            End If
            PerDbTraneAction.CloseCommit()
            Return True
        Catch ex As Exception
            PerDbTraneAction.CloseRollback()
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' 判断権限信息是否存在
    ''' </summary>
    ''' <param name="dicAuthority"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsAuthorityExsit(ByVal dicAuthority As Dictionary(Of String, String)) As Boolean
        Return authorityDa.IsAuthorityExsit(dicAuthority)
    End Function

    ''' <summary>
    ''' 権限信息添加処理
    ''' </summary>
    ''' <param name="dicAuthority"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertAuthority(ByVal dicAuthority As Dictionary(Of String, String)) As Boolean

        Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction


        Try
            If authorityDa.InsertAuthority(PerDbTraneAction, dicAuthority) <> 1 Then
                PerDbTraneAction.CloseRollback()
                Return False
            End If
            PerDbTraneAction.CloseCommit()
            Return True
        Catch ex As Exception
            PerDbTraneAction.CloseRollback()
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' 権限信息更新処理
    ''' </summary>
    ''' <param name="dicAuthority"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateAuthority(ByVal dicAuthority As Dictionary(Of String, String)) As Boolean

        Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction

        Try
            If authorityDa.UpdateAuthority(PerDbTraneAction, dicAuthority) <> 1 Then
                PerDbTraneAction.CloseRollback()
                Return False
            End If
            PerDbTraneAction.CloseCommit()
            Return True
        Catch ex As Exception
            PerDbTraneAction.CloseRollback()
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' 権限信息削除処理
    ''' </summary>
    ''' <param name="dicAuthority"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteAuthority(ByVal dicAuthority As Dictionary(Of String, String)) As Boolean
        Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction

        Try
            If authorityDa.DeleteAuthority(PerDbTraneAction, dicAuthority) < 0 Then
                PerDbTraneAction.CloseRollback()
                Return False
            End If
            PerDbTraneAction.CloseCommit()
            Return True
        Catch ex As Exception
            PerDbTraneAction.CloseRollback()
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' 批量導入
    ''' </summary>
    ''' <param name="dtExcel"></param>
    ''' <param name="strUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DoImport(ByVal dtExcel As DataTable, ByVal strUser As String, ByVal dtError As DataTable, _
                             ByRef afterUpdDt As DataTable) As Boolean
        Dim sysTime As DateTime
        Dim strUserId As String
        Dim strLoginName As String
        Dim dicUserInfo As Dictionary(Of String, String)
        Dim dicAuthority As Dictionary(Of String, String)
        Dim strAccessType As String = String.Empty
        Dim strAccessCd As String = String.Empty
        Dim drError As DataRow
        Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction

        Try
            '取得系統時間
            sysTime = ComBizLogic.GetSystemDate()

            For i As Integer = 0 To dtExcel.Rows.Count - 1

                dicUserInfo = New Dictionary(Of String, String)
                dicAuthority = New Dictionary(Of String, String)

                With dtExcel.Rows(i)
                    '用户id
                    strUserId = Convert.ToString(.Item("用户id"))
                    '登录名
                    strLoginName = Convert.ToString(.Item("登录名"))
                    '作成用戸信息
                    dicUserInfo.Add("user_id", strUserId)
                    dicUserInfo.Add("LoginName", strLoginName)
                    dicUserInfo.Add("LoginPassword", Convert.ToString(.Item("密码")))
                    dicUserInfo.Add("UserName", Convert.ToString(.Item("用户名")))
                    dicUserInfo.Add("UserCode", Convert.ToString(.Item("用户代码")))
                    dicUserInfo.Add("UserType", Convert.ToString(.Item("用户类型")))

                    '作成权限信息
                    strAccessType = Convert.ToString(.Item("权限类型"))
                    strAccessCd = Convert.ToString(.Item("权限区分"))
                    '当前记录有权限信息时，作成权限信息
                    If String.IsNullOrEmpty(strAccessType) = False AndAlso String.IsNullOrEmpty(strAccessCd) = False Then
                        dicAuthority.Add("user_id", strUserId)
                        dicAuthority.Add("access_type", strAccessType)
                        dicAuthority.Add("access_cd", strAccessCd)
                        dicAuthority.Add("delete_flg", Convert.ToString(.Item("删除区分")))
                        dicAuthority.Add("user", strUser)
                        dicAuthority.Add("sysTime", sysTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                    End If

                End With

                '登录名重复存在的时候,当前记录存入错误数据表，继续下一条记录的操作
                If String.IsNullOrEmpty(strLoginName) = False AndAlso _
                    authorityDa.hasAlreadyExsit(strUserId, strLoginName) Then

                    drError = dtError.NewRow
                    drError("用户id") = strUserId
                    drError("登录名") = strLoginName
                    drError("密码") = dtExcel.Rows(i)("密码")
                    drError("用户名") = dtExcel.Rows(i)("用户名")
                    drError("用户代码") = dtExcel.Rows(i)("用户代码")
                    drError("用户类型") = dtExcel.Rows(i)("用户类型")
                    drError("权限类型") = dtExcel.Rows(i)("权限类型")
                    drError("权限区分") = dtExcel.Rows(i)("权限区分")
                    drError("删除区分") = dtExcel.Rows(i)("删除区分")
                    drError("错误信息") = MsgConst.M00050I
                    dtError.Rows.Add(drError)
                    Continue For
                End If

                '導入用戸信息
                If authorityDa.IsUserInfoExsit(strUserId) Then
                    '更新用户信息
                    If authorityDa.UpdateUserInfo(PerDbTraneAction, dicUserInfo) <> 1 Then
                        PerDbTraneAction.CloseRollback()
                        Return False
                    Else
                        '更新的时候，更新成功的数据保存到更新后的Datatable
                        afterUpdDt.Rows.Add(dtExcel.Rows(i).ItemArray)
                    End If
                Else
                    '添加用户信息
                    If authorityDa.InsertUserInfo(PerDbTraneAction, dicUserInfo) <> 1 Then
                        PerDbTraneAction.CloseRollback()
                        Return False
                    End If
                End If

                '当前记录有权限信息时，更新权限信息
                If String.IsNullOrEmpty(strAccessType) = False AndAlso String.IsNullOrEmpty(strAccessCd) = False Then
                    '導入权限信息
                    If authorityDa.IsAuthorityExsit(dicAuthority) Then
                        '更新权限信息
                        If authorityDa.UpdateAuthority(PerDbTraneAction, dicAuthority) <> 1 Then
                            PerDbTraneAction.CloseRollback()
                            Return False
                        End If
                    Else
                        '添加权限信息
                        If authorityDa.InsertAuthority(PerDbTraneAction, dicAuthority) <> 1 Then
                            PerDbTraneAction.CloseRollback()
                            Return False
                        End If
                    End If
                End If
            Next
            PerDbTraneAction.CloseCommit()
            Return True
        Catch ex As Exception
            PerDbTraneAction.CloseRollback()
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' 取得導出データ一覧
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetExportData(ByVal dicSearch As Dictionary(Of String, String)) As DataSet
        Return authorityDa.GetExportData(dicSearch)
    End Function

    ''' <summary>
    ''' 更新前用户权限信息取得
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="accessType"></param>
    ''' <param name="accessCd"></param>
    ''' <param name="importFlg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserId(ByVal userId As String, _
                              ByVal accessType As String, _
                              ByVal accessCd As String, _
                              ByVal importFlg As Boolean) As DataTable

        Return authorityDa.GetUserId(userId, accessType, accessCd, importFlg).Tables(0)

    End Function
End Class
