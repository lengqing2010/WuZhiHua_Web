''' <summary>
''' �X�e�[�g���Ǘ�
''' �A�v���P�[�V���������҂͂��̃N���X�𒼐ڗ��p���邱�Ƃ͂���܂���
''' </summary>
''' <remarks></remarks>
Public Class StateManager
    ' �V���O���g���ŕێ�
    Private _state As Hashtable = New Hashtable()

    Public ReadOnly Property State() As Hashtable
        Get
            Return _state
        End Get
    End Property

    Private Sub New()
    End Sub

    Private Shared soloInstance As New StateManager

    Public Shared ReadOnly Property Instance() As StateManager
        Get
            Return soloInstance
        End Get
    End Property
End Class
