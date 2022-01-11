Namespace Common
    ''' <summary>
    ''' 重写CheckedListBox（选中的时候没有背景色）
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ColorCodedCheckedListBox
        Inherits CheckedListBox
        'Color.Orange为颜色，你可修改
        Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
            Dim e2 As DrawItemEventArgs = New DrawItemEventArgs(e.Graphics, e.Font, New Rectangle(e.Bounds.Location, e.Bounds.Size), e.Index, CType(((e.State & DrawItemState.Focus) Is IIf(CBool(DrawItemState.Focus), DrawItemState.Focus, DrawItemState.None)), DrawItemState), Color.Black, Me.BackColor)
            MyBase.OnDrawItem(e2)
        End Sub
    End Class
End Namespace

