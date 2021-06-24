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

    Public Shared Function CustomParameter(ByVal Key As String, ByVal Value As Object, ByVal TypeValue As String) As ParamSQL
        Select Case TypeValue
            Case "DATETIME"
                Return New ParamSQL(Key, CType(Value, DateTime).Year & "-" & CType(Value, DateTime).Month & "-" & CType(Value, DateTime).Day & " " & CType(Value, DateTime).ToLongTimeString)
            Case "FLOAT"
                Return New ParamSQL(Key, CType(Value, Decimal).ToString.Replace(",", "."))
            Case Else
                Return New ParamSQL(Key, Value)
        End Select
    End Function
End Class
