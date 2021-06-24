Imports MySql.Data

Public Class GenFunc
    Public Shared Function AffecteData(ByVal NomCol As String, ByVal Reader As MySqlClient.MySqlDataReader, ByVal TypeCible As String)
        If Not Reader(NomCol) Is Nothing AndAlso Not TypeOf (Reader(NomCol)) Is DBNull AndAlso Not String.IsNullOrWhiteSpace(Reader(NomCol)) Then
            Return Reader(NomCol)
        Else
            If TypeCible = "string" Then
                Return ""
            ElseIf TypeCible = "number" Then
                Return 0
            ElseIf TypeCible = "date" Then
                Return New Date(1900, 1, 1)
            ElseIf TypeCible = "bool" Then
                Return False
            Else
                Return ""
            End If
        End If
    End Function

    Public Shared Function Between_Decimal(ByVal Nb As Decimal, ByVal NbMin As Decimal, ByVal NbMax As Decimal) As Boolean
        If Nb >= NbMin AndAlso Nb <= NbMax Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function Jsonify(ByVal obj As Object) As String
        Try
            Return Newtonsoft.Json.JsonConvert.SerializeObject(obj).ToString
        Catch ex As Exception
            Return "Impossible de transformer l'objet au format JSON : " & ex.Message
        End Try
    End Function
End Class
