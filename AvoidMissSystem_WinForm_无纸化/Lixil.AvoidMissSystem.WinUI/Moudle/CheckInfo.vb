Imports Microsoft.VisualBasic
Imports Lixil.AvoidMissSystem.Utilities
''' <summary>
''' 储存检查临时信息
''' </summary>
''' <remarks></remarks>
Public Class CheckInfo
#Region "プロパティ"

    Public _classifyCd As String
    ''' <summary>
    ''' 分类code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ClassifyCd() As String
        Get
            Return _classifyCd
        End Get
        Set(ByVal value As String)
            _classifyCd = value
        End Set
    End Property

    Public _classifyName As String
    ''' <summary>
    ''' 分类名称
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ClassifyName() As String
        Get
            Return _classifyName
        End Get
        Set(ByVal value As String)
            _classifyName = value
        End Set
    End Property

    Public _pictureCode As String
    ''' <summary>
    ''' 图片id
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PictureCode() As String
        Get
            Return _pictureCode
        End Get
        Set(ByVal value As String)
            _pictureCode = value
        End Set
    End Property

    Public _checkResult As String
    ''' <summary>
    ''' 检查结果
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CheckResult() As String
        Get
            Return _checkResult
        End Get
        Set(ByVal value As String)
            _checkResult = value
        End Set
    End Property

    Public _toolsID As String
    ''' <summary>
    ''' 治具ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ToolsID() As String
        Get
            Return _toolsID
        End Get
        Set(ByVal value As String)
            _toolsID = value
        End Set
    End Property

    Public _kindCd As String
    ''' <summary>
    ''' 种类ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property KindCd() As String
        Get
            Return _kindCd
        End Get
        Set(ByVal value As String)
            _kindCd = value
        End Set
    End Property

#End Region
End Class
