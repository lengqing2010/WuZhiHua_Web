Imports System.Text
Imports System.Data.SqlClient
Imports System.Reflection.MethodBase
Imports Itis.ApplicationBlocks.Data.SQLHelper
Imports Itis.ApplicationBlocks.ExceptionManagement.UnTrappedExceptionManager
Imports Lixil.AvoidMissSystem.Utilities.Consts

''' <summary>
''' 権限MS用DA
''' </summary>
''' <remarks></remarks>
Public Class MsMaintenanceAuthorityDA

    ''' <summary>
    ''' 取得用戸信息一覧
    ''' </summary>
    ''' <param name="dicSearch"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserInfoList(ByVal dicSearch As Dictionary(Of String, String)) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name)

        Dim sb As New StringBuilder
        Dim ds As New DataSet
        Dim strSql As String = String.Empty
        Dim paramList As New List(Of SqlParameter)
        Dim deleteFlg As String = dicSearch("deleteFlg")
        Dim loginName As String = dicSearch("loginName")
        Dim accessType1 As String = dicSearch("accessType1")
        Dim accessType2 As String = dicSearch("accessType2")
        Dim accessType3 As String = dicSearch("accessType3")
        Dim accessCd1 As String = dicSearch("accessCd1")
        Dim accessCd2 As String = dicSearch("accessCd2")
        Dim accessCd3 As String = dicSearch("accessCd3")

        sb.AppendLine(" SELECT DISTINCT")
        sb.AppendLine("     U.ID AS id")
        sb.AppendLine(" 	,U.LoginName")
        sb.AppendLine(" 	,U.LoginPassword AS password")
        sb.AppendLine(" 	,U.UserName")
        'sb.AppendLine(" 	,P.update_date")
        'sb.AppendLine(" 	,P.update_user")
        sb.AppendLine(" FROM")
        sb.AppendLine("     TB_User U WITH(READCOMMITTED)")
        sb.AppendLine(" LEFT JOIN")
        sb.AppendLine("     m_permission P WITH(READCOMMITTED)")
        sb.AppendLine(" ON")
        sb.AppendLine("     U.ID = P.user_id")
        sb.AppendLine(" AND")
        sb.AppendLine("     P.delete_flg = @delete_flg")
        sb.AppendLine(" WHERE")
        '用戸
        sb.AppendLine("     (@LoginName = '' OR U.LoginName = @LoginName)")
        '検索条件中設定権限時
        If String.IsNullOrEmpty(accessCd1) = False OrElse String.IsNullOrEmpty(accessCd2) = False OrElse String.IsNullOrEmpty(accessCd3) = False Then
            sb.AppendLine(" AND")
            sb.AppendLine(" 	(")
            '選択管理者権限時
            If String.IsNullOrEmpty(accessCd1) = False Then
                sb.AppendLine(" 	    (P.access_type = @access_type1 AND P.access_cd = @access_cd1)")
                sb.AppendLine(" 	OR")
            End If

            '選択機能権限時
            If String.IsNullOrEmpty(accessCd2) = False Then
                sb.AppendLine(" 	    (P.access_type = @access_type2 AND P.access_cd IN (" & accessCd2 & "))")
                sb.AppendLine(" 	OR")
            End If

            '選択部門権限時
            If String.IsNullOrEmpty(accessCd3) = False Then
                sb.AppendLine(" 	    (P.access_type = @access_type3 AND P.access_cd IN (" & accessCd3 & "))")
                sb.AppendLine(" 	OR")
            End If
            sb.AppendLine(" 	)")

            Dim index As Integer = sb.ToString.LastIndexOf("OR")
            strSql = sb.ToString.Substring(0, index) & sb.ToString.Substring(index + 2)
        Else
            strSql = sb.ToString
        End If

        'パラメータ設定
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, deleteFlg))
        paramList.Add(MakeParam("@LoginName", SqlDbType.NVarChar, 30, loginName))
        paramList.Add(MakeParam("@access_type1", SqlDbType.Char, 1, accessType1))
        paramList.Add(MakeParam("@access_type2", SqlDbType.Char, 1, accessType2))
        paramList.Add(MakeParam("@access_type3", SqlDbType.Char, 1, accessType3))
        paramList.Add(MakeParam("@access_cd1", SqlDbType.Char, 3, accessCd1))

        '検索の実行
        Dim tableName As String = "mauthority"
        FillDataset(DataAccessManager.Connection, CommandType.Text, strSql, ds, tableName, paramList.ToArray)
        Return ds
    End Function

    ''' <summary>
    ''' 取得権限信息一覧
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="deleteFlg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAuthorityList(ByVal userId As String, ByVal deleteFlg As String) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name)

        Dim sb As New StringBuilder
        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT")
        sb.AppendLine(" 	P.access_type")
        sb.AppendLine(" 	,P.access_cd")
        sb.AppendLine(" FROM")
        sb.AppendLine("     m_permission P WITH(READCOMMITTED)")
        sb.AppendLine(" WHERE")
        sb.AppendLine("     P.user_id = @user_id")
        sb.AppendLine(" AND")
        sb.AppendLine("     P.delete_flg = @delete_flg")

        'パラメータ設定
        paramList.Add(MakeParam("@user_id", SqlDbType.BigInt, 18, userId))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, deleteFlg))

        '検索の実行
        Dim tableName As String = "mauthority"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString, ds, tableName, paramList.ToArray)
        Return ds
    End Function

    ''' <summary>
    ''' 判断権限信息是否存在
    ''' </summary>
    ''' <param name="dicAuthority"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsAuthorityExsit(ByVal dicAuthority As Dictionary(Of String, String)) As Boolean
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, dicAuthority)

        Dim sb As New StringBuilder
        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT")
        sb.AppendLine(" 	P.user_id")
        sb.AppendLine(" FROM")
        sb.AppendLine("     m_permission P")
        sb.AppendLine(" WHERE")
        sb.AppendLine("     P.user_id = @user_id")
        sb.AppendLine(" AND")
        sb.AppendLine("     P.access_type = @access_type")
        sb.AppendLine(" AND")
        sb.AppendLine("     P.access_cd = @access_cd")

        'パラメータ設定
        paramList.Add(MakeParam("@user_id", SqlDbType.BigInt, 18, dicAuthority("user_id")))
        paramList.Add(MakeParam("@access_type", SqlDbType.Char, 1, dicAuthority("access_type")))
        paramList.Add(MakeParam("@access_cd", SqlDbType.Char, 3, dicAuthority("access_cd")))

        '検索の実行
        Dim tableName As String = "mauthority"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString, ds, tableName, paramList.ToArray)
        If ds.Tables("mauthority") Is Nothing = False Then
            If ds.Tables("mauthority").Rows.Count > 0 Then '権限信息存在時
                Return True
            Else
                Return False '権限信息不存在時
            End If
        Else
            Return False '権限信息不存在時
        End If
    End Function

    ''' <summary>
    ''' 更新権限信息
    ''' </summary>
    ''' <param name="dicAuthority"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateAuthority(ByVal PersonalDbTransactionScopeAction As PersonalDataAccess.PersonalDbTransactionScopeAction, _
ByVal dicAuthority As Dictionary(Of String, String)) As Integer
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name)

        Dim result As Integer
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" UPDATE  ")
        sb.AppendLine("	    m_permission WITH (UPDLOCK)")
        sb.AppendLine("	SET ")
        sb.AppendLine("	    delete_flg = @delete_flg ")
        sb.AppendLine("	    ,update_user = @update_user ")
        sb.AppendLine("	    ,update_date = @update_date ")
        sb.AppendLine("	WHERE ")
        sb.AppendLine("	    user_id = @user_id ")
        sb.AppendLine("	AND ")
        sb.AppendLine("	    access_type = @access_type ")
        sb.AppendLine("	AND ")
        sb.AppendLine("	    access_cd = @access_cd ")

        'パラメータ設定
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 7, dicAuthority("delete_flg")))
        paramList.Add(MakeParam("@update_user", SqlDbType.VarChar, 30, dicAuthority("user")))
        paramList.Add(MakeParam("@update_date", SqlDbType.DateTime, 28, dicAuthority("sysTime")))
        paramList.Add(MakeParam("@user_id", SqlDbType.VarChar, 30, dicAuthority("user_id")))
        paramList.Add(MakeParam("@access_type", SqlDbType.VarChar, 100, dicAuthority("access_type")))
        paramList.Add(MakeParam("@access_cd", SqlDbType.VarChar, 30, dicAuthority("access_cd")))

        '更新の実行
        result = PersonalDbTransactionScopeAction.ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)
        Return result
    End Function

    ''' <summary>
    ''' 削除権限信息
    ''' </summary>
    ''' <param name="dicAuthority"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteAuthority(ByVal PersonalDbTransactionScopeAction As PersonalDataAccess.PersonalDbTransactionScopeAction, _
ByVal dicAuthority As Dictionary(Of String, String)) As Integer
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, dicAuthority)

        Dim result As Integer
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" UPDATE  ")
        sb.AppendLine("	    m_permission WITH (UPDLOCK)")
        sb.AppendLine("	SET ")
        sb.AppendLine("	    delete_flg = @delete_flg ")
        sb.AppendLine("	    ,update_user = @update_user ")
        sb.AppendLine("	    ,update_date = @update_date ")
        sb.AppendLine("	WHERE ")
        sb.AppendLine("	    user_id = @user_id ")

        'パラメータ設定
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, dicAuthority("delete_flg")))
        paramList.Add(MakeParam("@update_user", SqlDbType.VarChar, 30, dicAuthority("user")))
        paramList.Add(MakeParam("@update_date", SqlDbType.DateTime, 28, dicAuthority("sysTime")))
        paramList.Add(MakeParam("@user_id", SqlDbType.BigInt, 18, dicAuthority("user_id")))

        '更新の実行
        result = PersonalDbTransactionScopeAction.ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

        Return result
    End Function

    ''' <summary>
    ''' 添加権限信息
    ''' </summary>
    ''' <param name="dicAuthority"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertAuthority(ByVal PersonalDbTransactionScopeAction As PersonalDataAccess.PersonalDbTransactionScopeAction, _
ByVal dicAuthority As Dictionary(Of String, String)) As Integer
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, dicAuthority)

        Dim result As Integer
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" INSERT INTO m_permission WITH (UPDLOCK)")
        sb.AppendLine("     (user_id")
        sb.AppendLine("     ,access_type")
        sb.AppendLine("     ,access_cd")
        sb.AppendLine("     ,delete_flg")
        sb.AppendLine("     ,insert_user")
        sb.AppendLine("     ,insert_date")
        sb.AppendLine("     ,update_user")
        sb.AppendLine("     ,update_date)")
        sb.AppendLine("	VALUES ")
        sb.AppendLine("     (@user_id")
        sb.AppendLine("     ,@access_type")
        sb.AppendLine("     ,@access_cd")
        sb.AppendLine("     ,@delete_flg")
        sb.AppendLine("     ,@insert_user")
        sb.AppendLine("     ,@insert_date")
        sb.AppendLine("     ,@update_user")
        sb.AppendLine("     ,@update_date)")

        'パラメータ設定
        paramList.Add(MakeParam("@user_id", SqlDbType.BigInt, 18, dicAuthority("user_id")))
        paramList.Add(MakeParam("@access_type", SqlDbType.Char, 1, dicAuthority("access_type")))
        paramList.Add(MakeParam("@access_cd", SqlDbType.Char, 3, dicAuthority("access_cd")))
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, dicAuthority("delete_flg")))
        paramList.Add(MakeParam("@insert_user", SqlDbType.VarChar, 30, dicAuthority("user")))
        paramList.Add(MakeParam("@insert_date", SqlDbType.DateTime, 28, dicAuthority("sysTime")))
        paramList.Add(MakeParam("@update_user", SqlDbType.VarChar, 30, dicAuthority("user")))
        paramList.Add(MakeParam("@update_date", SqlDbType.DateTime, 28, dicAuthority("sysTime")))
        '挿入の実行
        result = PersonalDbTransactionScopeAction.ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

        Return result
    End Function

    ''' <summary>
    ''' 判断是否存在ID不同，登録名相同的記録
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="loginName" ></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function hasAlreadyExsit(ByVal userId As String, ByVal loginName As String) As Boolean
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, userId, loginName)

        Dim sb As New StringBuilder
        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT")
        sb.AppendLine(" 	U.ID")
        sb.AppendLine(" FROM")
        sb.AppendLine("     TB_User U")
        sb.AppendLine(" WHERE")
        sb.AppendLine("     U.ID <> @user_id")
        sb.AppendLine(" AND")
        sb.AppendLine("     U.LoginName = @LoginName")

        'パラメータ設定
        paramList.Add(MakeParam("@user_id", SqlDbType.BigInt, 18, userId))
        paramList.Add(MakeParam("@LoginName", SqlDbType.NVarChar, 30, loginName))

        '検索の実行
        Dim tableName As String = "TB_User"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString, ds, tableName, paramList.ToArray)
        If ds.Tables("TB_User") Is Nothing = False Then
            If ds.Tables("TB_User").Rows.Count > 0 Then '用戸信息存在時
                Return True
            Else
                Return False '用戸信息不存在時
            End If
        Else
            Return False '用戸信息不存在時
        End If
    End Function

    ''' <summary>
    ''' 判断用戸信息是否存在
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsUserInfoExsit(ByVal userId As String) As Boolean
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, userId)

        Dim sb As New StringBuilder
        Dim ds As New DataSet
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT")
        sb.AppendLine(" 	U.ID")
        sb.AppendLine(" FROM")
        sb.AppendLine("     TB_User U")
        sb.AppendLine(" WHERE")
        sb.AppendLine("     U.ID = @user_id")

        'パラメータ設定
        paramList.Add(MakeParam("@user_id", SqlDbType.BigInt, 18, userId))

        '検索の実行
        Dim tableName As String = "TB_User"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString, ds, tableName, paramList.ToArray)
        If ds.Tables("TB_User") Is Nothing = False Then
            If ds.Tables("TB_User").Rows.Count > 0 Then '用戸信息存在時
                Return True
            Else
                Return False '用戸信息不存在時
            End If
        Else
            Return False '用戸信息不存在時
        End If
    End Function

    ''' <summary>
    ''' 更新用戸信息
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateUserInfo(ByVal PersonalDbTransactionScopeAction As PersonalDataAccess.PersonalDbTransactionScopeAction, _
ByVal userId As String, ByVal password As String) As Integer
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, userId, password)

        Dim result As Integer
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine("	UPDATE TB_User WITH (UPDLOCK) ")
        sb.AppendLine("	SET LoginPassword = @LoginPassword  ")
        sb.AppendLine("	WHERE ID = @id ")

        'パラメータ設定
        paramList.Add(MakeParam("@id", SqlDbType.BigInt, 18, userId))
        paramList.Add(MakeParam("@LoginPassword", SqlDbType.NVarChar, 30, password))

        '更新の実行
        result = PersonalDbTransactionScopeAction.ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

        Return result
    End Function

    ''' <summary>
    ''' 更新用戸信息
    ''' </summary>
    ''' <param name="dicUserInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateUserInfo(ByVal PersonalDbTransactionScopeAction As PersonalDataAccess.PersonalDbTransactionScopeAction, _
ByVal dicUserInfo As Dictionary(Of String, String)) As Integer
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, dicUserInfo)

        Dim result As Integer
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine("	UPDATE TB_User WITH (UPDLOCK) ")
        sb.AppendLine("	SET LoginName = @LoginName  ")
        sb.AppendLine("	    ,LoginPassword = @LoginPassword ")
        sb.AppendLine("	    ,UserName = @UserName ")
        sb.AppendLine("	    ,UserCode = @UserCode ")
        sb.AppendLine("	    ,UserType = @UserType ")
        sb.AppendLine("	WHERE ID = @ID ")

        'パラメータ設定
        paramList.Add(MakeParam("@ID", SqlDbType.BigInt, 18, dicUserInfo("user_id")))
        paramList.Add(MakeParam("@LoginName", SqlDbType.NVarChar, 30, dicUserInfo("LoginName")))
        paramList.Add(MakeParam("@LoginPassword", SqlDbType.NVarChar, 30, dicUserInfo("LoginPassword")))
        paramList.Add(MakeParam("@UserName", SqlDbType.NVarChar, 50, dicUserInfo("UserName")))
        paramList.Add(MakeParam("@UserCode", SqlDbType.NVarChar, 50, dicUserInfo("UserCode")))
        paramList.Add(MakeParam("@UserType", SqlDbType.Int, 10, dicUserInfo("UserType")))

        '更新の実行
        result = PersonalDbTransactionScopeAction.ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

        Return result
    End Function

    ''' <summary>
    ''' 添加用戸信息
    ''' </summary>
    ''' <param name="dicUserInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertUserInfo(ByVal PersonalDbTransactionScopeAction As PersonalDataAccess.PersonalDbTransactionScopeAction, _
ByVal dicUserInfo As Dictionary(Of String, String)) As Integer
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name, dicUserInfo)

        Dim result As Integer
        Dim sb As New StringBuilder
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" INSERT INTO TB_User WITH (UPDLOCK)")
        sb.AppendLine("     (ID")
        sb.AppendLine("     ,LoginName")
        sb.AppendLine("     ,LoginPassword")
        sb.AppendLine("     ,UserName")
        sb.AppendLine("     ,UserCode")
        sb.AppendLine("     ,UserType)")
        sb.AppendLine("	VALUES ")
        sb.AppendLine("     (@ID")
        sb.AppendLine("     ,@LoginName")
        sb.AppendLine("     ,@LoginPassword")
        sb.AppendLine("     ,@UserName")
        sb.AppendLine("     ,@UserCode")
        sb.AppendLine("     ,@UserType)")

        'パラメータ設定
        paramList.Add(MakeParam("@ID", SqlDbType.BigInt, 18, dicUserInfo("user_id")))
        paramList.Add(MakeParam("@LoginName", SqlDbType.NVarChar, 30, dicUserInfo("LoginName")))
        paramList.Add(MakeParam("@LoginPassword", SqlDbType.NVarChar, 30, dicUserInfo("LoginPassword")))
        paramList.Add(MakeParam("@UserName", SqlDbType.NVarChar, 50, dicUserInfo("UserName")))
        paramList.Add(MakeParam("@UserCode", SqlDbType.NVarChar, 50, dicUserInfo("UserCode")))
        paramList.Add(MakeParam("@UserType", SqlDbType.Int, 10, dicUserInfo("UserType")))
        '挿入の実行
        result = PersonalDbTransactionScopeAction.ExecuteNonQuery(DataAccessManager.Connection, CommandType.Text, sb.ToString(), paramList.ToArray)

        Return result
    End Function

    ''' <summary>
    ''' 取得導出データ一覧
    ''' </summary>
    ''' <param name="dicSearch"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetExportData(ByVal dicSearch As Dictionary(Of String, String)) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name)

        Dim sb As New StringBuilder
        Dim ds As New DataSet
        Dim strSql As String = String.Empty
        Dim paramList As New List(Of SqlParameter)
        Dim deleteFlg As String = dicSearch("deleteFlg")
        Dim loginName As String = dicSearch("loginName")
        Dim accessType1 As String = dicSearch("accessType1")
        Dim accessType2 As String = dicSearch("accessType2")
        Dim accessType3 As String = dicSearch("accessType3")
        Dim accessCd1 As String = dicSearch("accessCd1")
        Dim accessCd2 As String = dicSearch("accessCd2")
        Dim accessCd3 As String = dicSearch("accessCd3")

        sb.AppendLine(" SELECT")
        sb.AppendLine("     U.ID AS id")
        sb.AppendLine(" 	,U.LoginName")
        sb.AppendLine(" 	,U.LoginPassword AS password")
        sb.AppendLine(" 	,U.UserName")
        sb.AppendLine(" 	,U.UserCode")
        sb.AppendLine(" 	,U.UserType")
        sb.AppendLine(" 	,P.access_type")
        sb.AppendLine(" 	,P.access_cd")
        sb.AppendLine(" 	,P.delete_flg")
        sb.AppendLine(" FROM")
        sb.AppendLine("     TB_User U WITH(READCOMMITTED)")
        sb.AppendLine(" LEFT JOIN")
        sb.AppendLine("     m_permission P WITH(READCOMMITTED)")
        sb.AppendLine(" ON")
        sb.AppendLine("     U.ID = P.user_id")
        sb.AppendLine(" AND")
        sb.AppendLine("     P.delete_flg = @delete_flg")
        sb.AppendLine(" WHERE")
        '用戸
        sb.AppendLine("     (@LoginName = '' OR U.LoginName = @LoginName)")
        '検索条件中設定権限時
        If String.IsNullOrEmpty(accessCd1) = False OrElse String.IsNullOrEmpty(accessCd2) = False OrElse String.IsNullOrEmpty(accessCd3) = False Then
            sb.AppendLine(" AND")
            sb.AppendLine(" 	(")
            '選択管理者権限時
            If String.IsNullOrEmpty(accessCd1) = False Then
                sb.AppendLine(" 	    (P.access_type = @access_type1 AND P.access_cd = @access_cd1)")
                sb.AppendLine(" 	OR")
            End If

            '選択機能権限時
            If String.IsNullOrEmpty(accessCd2) = False Then
                sb.AppendLine(" 	    (P.access_type = @access_type2 AND P.access_cd IN (" & accessCd2 & "))")
                sb.AppendLine(" 	OR")
            End If

            '選択部門権限時
            If String.IsNullOrEmpty(accessCd3) = False Then
                sb.AppendLine(" 	    (P.access_type = @access_type3 AND P.access_cd IN (" & accessCd3 & "))")
                sb.AppendLine(" 	OR")
            End If
            sb.AppendLine(" 	)")

            Dim index As Integer = sb.ToString.LastIndexOf("OR")
            strSql = sb.ToString.Substring(0, index) & sb.ToString.Substring(index + 2)
        Else
            strSql = sb.ToString
        End If

        'パラメータ設定
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, deleteFlg))
        paramList.Add(MakeParam("@LoginName", SqlDbType.NVarChar, 30, loginName))
        paramList.Add(MakeParam("@access_type1", SqlDbType.Char, 1, accessType1))
        paramList.Add(MakeParam("@access_type2", SqlDbType.Char, 1, accessType2))
        paramList.Add(MakeParam("@access_type3", SqlDbType.Char, 1, accessType3))
        paramList.Add(MakeParam("@access_cd1", SqlDbType.Char, 3, accessCd1))

        '検索の実行
        Dim tableName As String = "mauthority"
        FillDataset(DataAccessManager.Connection, CommandType.Text, strSql, ds, tableName, paramList.ToArray)
        Return ds
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
                              ByVal importFlg As Boolean) As DataSet
        AddMethodEntrance(MyClass.GetType.FullName & "." & GetCurrentMethod.Name)

        Dim sb As New StringBuilder
        Dim ds As New DataSet
        Dim strSql As String = String.Empty
        Dim paramList As New List(Of SqlParameter)

        sb.AppendLine(" SELECT DISTINCT")
        sb.AppendLine("     U.ID AS id")
        sb.AppendLine(" 	,U.LoginName")
        sb.AppendLine(" 	,U.LoginPassword AS password")
        sb.AppendLine(" 	,U.UserName")
        sb.AppendLine(" 	,U.UserCode")
        sb.AppendLine(" 	,U.UserType")
        sb.AppendLine(" 	,P.access_type")
        sb.AppendLine(" 	,P.access_cd")
        sb.AppendLine(" 	,P.delete_flg")
        'sb.AppendLine(" 	,P.update_date")
        'sb.AppendLine(" 	,P.update_user")
        sb.AppendLine(" FROM")
        sb.AppendLine("     TB_User U WITH(READCOMMITTED)")
        sb.AppendLine(" LEFT JOIN")
        sb.AppendLine("     m_permission P WITH(READCOMMITTED)")
        sb.AppendLine(" ON")
        sb.AppendLine("     U.ID = P.user_id")
        If importFlg = True Then
            sb.AppendLine(" AND P.access_type =  @access_type ")
            sb.AppendLine(" AND P.access_cd = @access_cd ")
        End If
        sb.AppendLine(" AND")
        sb.AppendLine("     P.delete_flg = @delete_flg")
        sb.AppendLine(" WHERE")
        sb.AppendLine("     U.ID = @ID")

        'パラメータ設定
        paramList.Add(MakeParam("@delete_flg", SqlDbType.Char, 1, UNDELETED))
        paramList.Add(MakeParam("@ID", SqlDbType.BigInt, 18, userId))
        If importFlg = True Then
            paramList.Add(MakeParam("@access_type", SqlDbType.Char, 1, accessType))
            paramList.Add(MakeParam("@access_cd", SqlDbType.Char, 3, accessCd))
        End If

        '検索の実行
        Dim tableName As String = "mauthority"
        FillDataset(DataAccessManager.Connection, CommandType.Text, sb.ToString(), ds, tableName, paramList.ToArray)
        Return ds
    End Function
End Class
