Imports Lixil.AvoidMissSystem.DataAccess
Imports Lixil.AvoidMissSystem.Utilities.Consts
Imports Lixil.AvoidMissSystem.Utilities.MsgConst
Imports System.Transactions

Public Class MsMaintenceCheckBizLogic

    Dim da As New MsMaintenceCheckDA
    Dim strMessage As String = String.Empty
    Dim goodsUpdFlg As Boolean = False
    Dim classifyUpdFlg As Boolean = False
    Dim goodsChangeFlg As Boolean = True
    Dim classifyChangeFlg As Boolean = True

    ''' <summary>
    ''' 查询数据取得
    ''' </summary>
    ''' <param name="dicRequirement"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCheckData(ByVal dicRequirement As Dictionary(Of String, String), ByVal pageIdx As Integer) As DataTable
        Dim ds As DataSet
        ds = da.GetCheckData(dicRequirement, pageIdx)
        Return ds.Tables(0)
    End Function

    ''' <summary>
    ''' 区分表名称取得
    ''' </summary>
    ''' <param name="meiKbn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetKbn(ByVal meiKbn As String) As DataTable
        Return da.GetKbn(meiKbn).Tables(0)
    End Function

    ''' <summary>
    ''' 非管理员部门取得
    ''' </summary>
    ''' <param name="strUserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDepartment(ByVal strUserID As String) As DataSet
        Return da.GetDepartment(strUserID)
    End Function

    ''' <summary>
    ''' 检查项目表更新
    ''' </summary>
    ''' <param name="checkId"></param>
    ''' <param name="goodsId"></param>
    ''' <param name="classifyId"></param>
    ''' <param name="updPara"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateMCheck(ByVal PersonalDbTransactionScopeAction As PersonalDataAccess.PersonalDbTransactionScopeAction, _
ByVal checkId As String, _
                                 ByVal goodsId As String, _
                                 ByVal classifyId As String, _
                                 ByVal updPara As Dictionary(Of String, String)) As Integer

        Dim result1 As Integer = 1
        Dim result2 As Integer = 1
        Dim result3 As Integer = 1

        '检查表更新
        If updPara.ContainsKey(TOOLNO_TEXT) OrElse _
            updPara.ContainsKey(TOOLDISP_TEXT) OrElse _
            updPara.ContainsKey(CLASSIFYDISP_TEXT) OrElse _
            updPara.ContainsKey(DEPARTMENT_TEXT) OrElse _
            updPara.ContainsKey(GOODSKIND_TEXT) OrElse _
            updPara.ContainsKey(CHKPOSITION_TEXT) OrElse _
            updPara.ContainsKey(CHKITEM_TEXT) OrElse _
            updPara.ContainsKey(BMTYPE_TEXT) OrElse _
            updPara.ContainsKey(BMVALUE1_TEXT) OrElse _
            updPara.ContainsKey(BMVALUE2_TEXT) OrElse _
            updPara.ContainsKey(BMVALUE3_TEXT) OrElse _
            updPara.ContainsKey(CHKWAY_TEXT) OrElse _
            updPara.ContainsKey(CHKTIMES_TEXT) Then

            result1 = da.UpdateMCheck(PersonalDbTransactionScopeAction, checkId, updPara)
        End If

        '商品表更新
        If updPara.ContainsKey(GOODSNAME_TEXT) Then
            result2 = da.UpdateMGoods(PersonalDbTransactionScopeAction, goodsId, updPara)
        End If
        '分类表更新
        If updPara.ContainsKey(CLASSIFY_TEXT) OrElse _
            updPara.ContainsKey(IMGID_TEXT) OrElse _
            updPara.ContainsKey(DEPARTMENT_TEXT) OrElse _
            updPara.ContainsKey(TOOLNO_TEXT) OrElse _
            updPara.ContainsKey(CLASSIFYDISP_TEXT) Then

            result3 = da.UpdateMClassify(PersonalDbTransactionScopeAction, classifyId, updPara)
        End If

        If result1 = 1 AndAlso result2 = 1 AndAlso result3 = 1 Then
            Return 1
        Else
            Return 0
        End If

    End Function

    ''' <summary>
    ''' 插入处理
    ''' </summary>
    ''' <param name="dicInsert"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertCheckMs(ByVal dicInsert As Dictionary(Of String, String), ByVal singleFlg As Boolean) As Integer
        Dim indexResult As New Result
        'Dim dsDepartment As DataSet
        'Dim dsKind As DataSet
        Dim dsType As DataSet
        Dim result1 As Integer = 1
        Dim result2 As Integer = 1
        Dim result3 As Integer = 1
        Dim strKind As String = String.Empty
        Dim strType As String = String.Empty
        Dim strDepartment As String = String.Empty
        Try
            '批量导入的时候
            If singleFlg = False Then

                '部门存在判断（不存在的话返回错误信息）
                'dsDepartment = da.GetBumenCd("0004", dicInsert.Item(DEPARTMENTNAME_TEXT).ToString)

                Dim dtAllDepartment As DataTable = da.GetBumenAll0004Cd
                Dim drs() As DataRow = dtAllDepartment.Select("mei_cd = '" & dicInsert.Item(DEPARTMENTNAME_TEXT).ToString & "'")

               

                If drs.Length > 0 Then
                    '部门存在
                    strDepartment = drs(0).Item("mei_cd").ToString
                    '种类存在判断

                    Dim dtKind As DataTable = da.GetBumenAll0001Cd()
                    Dim drsKinds() As DataRow = dtKind.Select("mei='" & dicInsert.Item(KINDNAME_TEXT).ToString & "'")

                    'dsKind = da.GetKbnMei("0001", dicInsert.Item(KINDNAME_TEXT).ToString)
                    If drsKinds.Length > 0 Then
                        '种类名称存在的时候
                        strKind = drsKinds(0).Item("mei_cd").ToString
                    Else
                        '种类名称不存在的时候,新的种类插入,并取得种类CD
                        Dim maxKindMeiCd As String = String.Empty
                        '种类CD最大值取得
                        maxKindMeiCd = da.GetMaxKbnCd("0001").Tables(0).Rows(0).Item(0).ToString
                        maxKindMeiCd = CStr(CInt(maxKindMeiCd) + 1)
                        maxKindMeiCd = maxKindMeiCd.PadLeft(3, "0"c)
                        '新种类Cd插入
                        da.InsertKbn("0001", maxKindMeiCd, dicInsert.Item(KINDNAME_TEXT).ToString, _
                                    dicInsert.Item(USER_TEXT).ToString)
                        strKind = maxKindMeiCd
                    End If

                    '类型存在判断
                    dsType = da.GetKbnMei("0002", dicInsert.Item(TYPENAME_TEXT).ToString)
                    If dsType.Tables(0).Rows.Count > 0 Then
                        '类型名称存在的时候
                        strType = dsType.Tables(0).Rows(0).Item("mei_cd").ToString
                    Else
                        '类型名称不存在的时候,新的类型插入,并取得类型CD
                        Dim maxTypeMeiCd As String = String.Empty
                        '类型CD最大值取得
                        maxTypeMeiCd = da.GetMaxKbnCd("0002").Tables(0).Rows(0).Item(0).ToString
                        maxTypeMeiCd = CStr(CInt(maxTypeMeiCd) + 1)
                        maxTypeMeiCd = maxTypeMeiCd.PadLeft(3, "0"c)
                        '新类型Cd插入
                        da.InsertKbn("0002", maxTypeMeiCd, dicInsert.Item(TYPENAME_TEXT).ToString, _
                                    dicInsert.Item(USER_TEXT).ToString)
                        strType = maxTypeMeiCd
                    End If

                Else
                    Return 1 '部门不存在Error'????????????????
                End If
            Else
                strDepartment = dicInsert.Item(DEPARTMENTCD_TEXT).ToString
                strKind = dicInsert.Item(KINDCD_TEXT).ToString
                strType = dicInsert.Item(TYPECD_TEXT).ToString
            End If

            If dicInsert.ContainsKey(CHECKID_TEXT) = False OrElse _
                dicInsert(CHECKID_TEXT) = String.Empty Then
                '检查项目id为空或者新建的时候，执行插入操作
                '采番处理
                indexResult = GetIndex(dicInsert, strKind, strDepartment)

                '采番成功的时候，插入数据库
                If indexResult.Success = True Then
                    Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction

                    '检查表插入
                    result1 = da.InsertCheckMs(PerDbTraneAction, dicInsert, _
                                               indexResult.CheckId, _
                                               indexResult.ClassifyId, _
                                               indexResult.GoodsId, _
                                               strKind, _
                                               strType, _
                                               strDepartment)
                    '分类表处理
                    'If classifyUpdFlg = True Then
                    '    '分类表更新
                    '    '??????????????（不进行更新处理，直接报错）
                    'Else
                    '分类表插入
                    If classifyChangeFlg = True Then
                        result2 = da.InsertClassifyMs(PerDbTraneAction, dicInsert, _
                                                      indexResult.ClassifyId, _
                                                      indexResult.GoodsId, _
                                                      strKind, _
                                                      strType, _
                                                      strDepartment)
                    End If

                    'End If

                    '商品表处理
                    'If goodsUpdFlg = True Then
                    '    '商品表更新
                    '    '????????????????（不进行更新处理，直接报错）
                    'Else
                    '商品表插入
                    If goodsChangeFlg = True Then
                        result3 = da.InsertGoodsMs(PerDbTraneAction, dicInsert, indexResult.GoodsId)
                    End If
                    'End If

                    If result1 = 1 AndAlso result2 = 1 AndAlso result3 = 1 Then
                        PerDbTraneAction.CloseCommit()
                        Return 0 '插入成功
                    Else
                        PerDbTraneAction.CloseRollback()
                        Return 2 '插入失败
                    End If

                Else
                    Return 5 '采番失败
                End If

            Else
                '检查项目id不为空的时候，执行更新操作
                Dim PerDbTraneAction As New PersonalDataAccess.PersonalDbTransactionScopeAction

                '商品表采番处理
                indexResult = GetGoodsIndex(indexResult, dicInsert, strKind, strDepartment)

                '分类表采番处理
                indexResult = GetClassifyIndex(indexResult, dicInsert, strKind, strDepartment, indexResult.GoodsId)

                '分类表插入
                If indexResult.Success = True Then
                    If classifyChangeFlg = True Then
                        result2 = da.InsertClassifyMs(PerDbTraneAction, dicInsert, _
                                                      indexResult.ClassifyId, _
                                                      indexResult.GoodsId, _
                                                      strKind, _
                                                      strType, _
                                                      strDepartment)
                    End If

                    '商品表插入
                    If goodsChangeFlg = True Then
                        result3 = da.InsertGoodsMs(PerDbTraneAction, dicInsert, indexResult.GoodsId)
                    End If
                End If

                '检查表更新
                result1 = da.UpdateCheckMs(PerDbTraneAction, dicInsert, _
                                           indexResult.ClassifyId, _
                                           indexResult.GoodsId, _
                                           strKind, _
                                           strType, _
                                           strDepartment)

                If result1 = 1 AndAlso result2 = 1 AndAlso result3 = 1 Then
                    PerDbTraneAction.CloseCommit()
                    Return 0 '更新成功
                Else
                    PerDbTraneAction.CloseRollback()
                    Return 4 '更新失败
                End If

            End If

        Catch ex As Exception
            Return 3 '插入失败
        End Try
    End Function

    ''' <summary>
    ''' 采番处理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetIndex(ByVal dicInsert As Dictionary(Of String, String), _
                             ByVal strKind As String, _
                             ByVal strDepartment As String) As Result
        Dim bcIndex As New IndexBizLogic
        Dim result As New Result
        Dim strCheckId As String = String.Empty

        '采番处理
        '检查表采番
        strCheckId = bcIndex.GetIndex(TYPEKBN.CHECK)
        result.Success = False
        Select Case strCheckId
            Case DBErro.InserError
                result.Message = M00002D.Replace("{0}", "检查表采番")
                Return result
            Case DBErro.UpdateError
                result.Message = M00003D.Replace("{0}", "检查表采番")
                Return result
            Case DBErro.GetIndexError
                result.Message = M00001D.Replace("{0}", "检查表")
                Return result
            Case DBErro.GetIndexMaxError
                result.Message = M00005D.Replace("{0}", "检查表")
                Return result
        End Select

        '商品表采番
        result = GetGoodsIndex(result, dicInsert, strKind, strDepartment)

        '分类表采番
        If result.Success = True Then
            result = GetClassifyIndex(result, dicInsert, strKind, strDepartment, result.GoodsId)
        Else
            Return result
        End If
        If result.Success = False Then
            Return result
        End If
        result.CheckId = strCheckId
        result.Success = True
        Return result

    End Function

    ''' <summary>
    ''' 商品表采番
    ''' </summary>
    ''' <param name="dicInsert"></param>
    ''' <param name="strKind"></param>
    ''' <param name="strDepartment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetGoodsIndex(ByVal result As Result, _
                                  ByVal dicInsert As Dictionary(Of String, String), _
                                  ByVal strKind As String, _
                                  ByVal strDepartment As String) As Result
        Dim bcIndex As New IndexBizLogic
        Dim dsGoods As DataSet
        Dim strGoodsId As String = String.Empty

        '采番处理

        '商品表采番
        '商品CD存在判断
        dsGoods = da.GetGoodId(dicInsert.Item(GOODSCD_TEXT).ToString)
        If dsGoods.Tables(0).Rows.Count > 0 Then
            '商品CD存在的时候
            '判断商品名称是否变更，变更的情况下返回错误信息，商品CD不登录
            strGoodsId = dsGoods.Tables(0).Rows(0).Item("id").ToString
            result.GoodsId = strGoodsId
            goodsChangeFlg = False
            If dsGoods.Tables(0).Rows(0).Item("goods_name").ToString <> dicInsert.Item(GOODSNAME_TEXT).ToString Then
                result.Message = "商品名不一致"
                result.Success = False
                Return result
            End If

            'goodsUpdFlg = True
        Else
            '商品CD不存在的时候,采番处理
            strGoodsId = bcIndex.GetIndex(TYPEKBN.GOODS)
            goodsChangeFlg = True
            result.Success = False
            Select Case strGoodsId
                Case DBErro.InserError
                    result.Message = M00002D.Replace("{0}", "商品表采番")
                    Return result
                Case DBErro.UpdateError
                    result.Message = M00003D.Replace("{0}", "商品表采番")
                    Return result
                Case DBErro.GetIndexError
                    result.Message = M00001D.Replace("{0}", "商品表")
                    Return result
                Case DBErro.GetIndexMaxError
                    result.Message = M00005D.Replace("{0}", "商品表")
                    Return result
            End Select
            result.GoodsId = strGoodsId
        End If

        result.Success = True

        Return result

    End Function

    ''' <summary>
    ''' 分类表采番处理
    ''' </summary>
    ''' <param name="dicInsert"></param>
    ''' <param name="strKind"></param>
    ''' <param name="strDepartment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClassifyIndex(ByVal result As Result, _
                                     ByVal dicInsert As Dictionary(Of String, String), _
                                     ByVal strKind As String, _
                                     ByVal strDepartment As String, _
                                     ByVal strGoodsId As String) As Result
        Dim bcIndex As New IndexBizLogic
        Dim dsClassify As DataSet
        Dim strClassifyId As String = String.Empty

        '采番处理

        '分类表采番
        '分类数据存在判断
        dsClassify = da.GetClassify(strGoodsId, strKind, _
                                    strDepartment, _
                                    dicInsert.Item(TOOLNO_TEXT).ToString, _
                                    dicInsert.Item(CLASSIFY_TEXT).ToString, _
                                    dicInsert.Item(IMGID_TEXT).ToString)
        If dsClassify.Tables(0).Rows.Count > 0 Then
            '    ''分类数据存在的时候,分类名称、图片ID、分类表示顺有变化时，更新分类表
            '    'If dsClassify.Tables(0).Rows(0).Item("classify_name").ToString <> dicInsert.Item(CLASSIFY_TEXT).ToString OrElse _
            '    '    dsClassify.Tables(0).Rows(0).Item("picture_id").ToString <> dicInsert.Item(IMGID_TEXT).ToString OrElse _
            '    '    dsClassify.Tables(0).Rows(0).Item("disp_no").ToString <> dicInsert.Item(CLASSIFYDISP_TEXT).ToString Then
            '    '    classifyUpdFlg = True
            '    'End If
            '    ''分类数据存在的时候,分类名称、图片ID、分类表示顺有变化时，返回错误
            '    'If dsClassify.Tables(0).Rows(0).Item("classify_name").ToString <> dicInsert.Item(CLASSIFY_TEXT).ToString OrElse _
            '    '    dsClassify.Tables(0).Rows(0).Item("picture_id").ToString <> dicInsert.Item(IMGID_TEXT).ToString OrElse _
            '    '    dsClassify.Tables(0).Rows(0).Item("disp_no").ToString <> dicInsert.Item(CLASSIFYDISP_TEXT).ToString Then
            '    '    Return result
            '    'Else
            '    '    classifyChangeFlg = False
            '    'End If
            '分类数据存在的时候,不更新
            classifyChangeFlg = False
            strClassifyId = dsClassify.Tables(0).Rows(0).Item("id").ToString
        Else
            '分类数据不存在的时候,采番处理
            classifyChangeFlg = True
            strClassifyId = bcIndex.GetIndex(TYPEKBN.CLASSIFY)
            result.Success = False
            Select Case strClassifyId
                Case DBErro.InserError
                    result.Message = M00002D.Replace("{0}", "分类表采番")
                    Return result
                Case DBErro.UpdateError
                    result.Message = M00003D.Replace("{0}", "分类表采番")
                    Return result
                Case DBErro.GetIndexError
                    result.Message = M00001D.Replace("{0}", "分类表")
                    Return result
                Case DBErro.GetIndexMaxError
                    result.Message = M00005D.Replace("{0}", "分类表")
                    Return result
            End Select

        End If

        result.ClassifyId = strClassifyId
        result.Success = True
        Return result

    End Function

    ''' <summary>
    ''' 治具编号存在检查用治具信息取得
    ''' </summary>
    ''' <param name="toolNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTool(ByVal toolNo As String) As DataTable
        Return da.GetTool(toolNo).Tables(0)
    End Function

    ''' <summary>
    ''' 治具ID存在检查用治具信息取得
    ''' </summary>
    ''' <param name="toolId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetToolId(ByVal toolId As String) As DataTable
        Return da.GetToolId(toolId).Tables(0)
    End Function

    ''' <summary>
    ''' 图片ID存在检查用图片信息取得
    ''' </summary>
    ''' <param name="imgId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPicture(ByVal imgId As String) As DataTable
        Return da.GetPicture(imgId).Tables(0)
    End Function

    ''' <summary>
    ''' 图片内容取得
    ''' </summary>
    ''' <param name="imgId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPictureContent(ByVal imgId As String) As DataTable
        Return da.GetPictureContent(imgId).Tables(0)
    End Function

    ''' <summary>
    ''' 商品ID取得
    ''' </summary>
    ''' <param name="goodsCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetGoodId(ByVal goodsCd As String) As DataTable
        Return da.GetGoodId(goodsCd).Tables(0)
    End Function

    ''' <summary>
    ''' 分类情报取得
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMClassify(ByVal id As String) As DataTable
        Return da.GetMClassify(id).Tables(0)
    End Function

    ''' <summary>
    ''' 检查项目删除
    ''' </summary>
    ''' <param name="checkId"></param>
    ''' <param name="classifyId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteMCheck(ByVal PersonalDbTransactionScopeAction As PersonalDataAccess.PersonalDbTransactionScopeAction, _
ByVal checkId As String, _
                                 ByVal classifyId As String, _
                                 ByVal strUserId As String) As Integer

        Dim result1 As Integer = 1
        Dim result2 As Integer = 1

        '检查表更新
        result1 = da.DeleteMCheck(PersonalDbTransactionScopeAction, checkId, strUserId)

        '分类表更新s
        result2 = da.DeleteMClassify(PersonalDbTransactionScopeAction, classifyId, strUserId)

        If result1 = 1 AndAlso result2 >= 1 Then
            Return 1
        Else
            Return 0
        End If

    End Function

    ''' <summary>
    ''' 检查项目Id存在判断
    ''' </summary>
    ''' <param name="checkId"></param>
    ''' <returns>true:存在 false:不存在</returns>
    ''' <remarks></remarks>
    Public Function GetCheckId(ByVal checkId As String) As DataTable
        Return da.GetCheckId(checkId).Tables(0)
    End Function

    Public Class Result

        Dim _message As String
        Public Property Message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                _message = value
            End Set
        End Property

        Dim _success As Boolean
        Public Property Success() As Boolean
            Get
                Return _success
            End Get
            Set(ByVal value As Boolean)
                _success = value
            End Set
        End Property

        Dim _checkId As String
        Public Property CheckId() As String
            Get
                Return _checkId
            End Get
            Set(ByVal value As String)
                _checkId = value
            End Set
        End Property

        Dim _classifyId As String
        Public Property ClassifyId() As String
            Get
                Return _classifyId
            End Get
            Set(ByVal value As String)
                _classifyId = value
            End Set
        End Property

        Dim _goodsId As String
        Public Property GoodsId() As String
            Get
                Return _goodsId
            End Get
            Set(ByVal value As String)
                _goodsId = value
            End Set
        End Property

    End Class
End Class

