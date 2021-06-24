Public Class ParamSQL
    Public Property NomChamp As String
    Public Property ValueChamp As String

    Public Sub New()
        NomChamp = ""
        ValueChamp = ""
    End Sub

    Public Sub New(ByVal Champ As String, Value As String)
        NomChamp = Champ
        ValueChamp = Value
    End Sub

    Public Shared Function ShowMeEveryParam(ByVal ListOfParam As List(Of ParamSQL)) As String
        Dim str As String = ""
        For Each item In ListOfParam
            str &= item.NomChamp & ": " & item.ValueChamp & "; "
        Next
        Return str
    End Function
End Class
