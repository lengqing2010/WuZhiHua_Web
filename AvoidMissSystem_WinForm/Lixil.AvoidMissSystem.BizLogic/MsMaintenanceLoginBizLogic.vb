Imports Lixil.AvoidMissSystem.DataAccess

''' <summary>
''' MS维护登录页面用BizLogic
''' </summary>
''' <remarks></remarks>
Public Class MsMaintenanceLoginBizLogic

    ''' <summary>
    ''' 取得当前登录用户权限信息
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserInfo(ByVal userId As String, ByVal password As String) As DataSet
        Dim loginDA As New MsMaintenanceLoginDA
        Dim dsLoginINfo As New DataSet '用户权限信息结果

        '取得用户权限信息处理
        dsLoginINfo = loginDA.GetUserInfo(userId, password)

        Return dsLoginINfo
    End Function
End Class
