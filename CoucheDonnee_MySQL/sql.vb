Imports System.Configuration
Imports MySql.Data.MySqlClient

Public Class sql

    Private ConnectionString As String

    ''' <summary>
    ''' Déclaration des objets d'acces au serveur SQL
    ''' Il peut y avoir jusqu'a 5 connexion SQL en simultané
    ''' Au cas ou on fait plusieurs acces en base en paralelle.
    ''' </summary>
    Private objConnex As MySqlConnection
    Private objComm As MySqlCommand
    Private objCommQuerry As MySqlCommand
    Private objDR As MySqlDataReader
    Private Nb As Integer

    Private objConnex2 As MySqlConnection
    Private objComm2 As MySqlCommand
    Private objCommQuerry2 As MySqlCommand
    Private objDR2 As MySqlDataReader
    Private Nb2 As Integer

    Private objConnex3 As MySqlConnection
    Private objComm3 As MySqlCommand
    Private objCommQuerry3 As MySqlCommand
    Private objDR3 As MySqlDataReader
    Private Nb3 As Integer

    Private objConnex4 As MySqlConnection
    Private objComm4 As MySqlCommand
    Private objCommQuerry4 As MySqlCommand
    Private objDR4 As MySqlDataReader
    Private Nb4 As Integer

    Private objConnex5 As MySqlConnection
    Private objComm5 As MySqlCommand
    Private objCommQuerry5 As MySqlCommand
    Private objDR5 As MySqlDataReader
    Private Nb5 As Integer


    Public Sub New(p_ConnectionString As String)
        Me.ConnectionString = p_ConnectionString
        Me.objConnex = New MySqlConnection()
        Me.objConnex2 = New MySqlConnection()
        Me.objConnex3 = New MySqlConnection()
        Me.objConnex4 = New MySqlConnection()
        Me.objConnex5 = New MySqlConnection()
    End Sub


#Region "Gestion des objets de connexions"
    ''' <summary>
    ''' Permet de vérifier l'état de la connexion SQL
    ''' Si cettte connexion est fermé, Ouvre la connexion
    ''' </summary>
    Private Sub CheckConnection()
        If Not objConnex.State = Data.ConnectionState.Open Then
            Try
                objConnex = New MySqlConnection(ConnectionString)
                objConnex.Open()
            Catch ex As Exception
                AddErreur("OPENING DB 1", "", ex)
            End Try
        End If
    End Sub

    Private Sub CheckConnection2()
        If Not objConnex2.State = Data.ConnectionState.Open Then
            Try
                objConnex2 = New MySqlConnection(ConnectionString)
                objConnex2.Open()
            Catch ex As Exception
                AddErreur("OPENING DB 2", "", ex)
            End Try
        End If
    End Sub

    Private Sub CheckConnection3()
        If Not objConnex3.State = Data.ConnectionState.Open Then
            Try
                objConnex3 = New MySqlConnection(ConnectionString)
                objConnex3.Open()
            Catch ex As Exception
                AddErreur("OPENING DB 3", "", ex)
            End Try
        End If
    End Sub

    Private Sub CheckConnection4()
        If Not objConnex4.State = Data.ConnectionState.Open Then
            Try
                objConnex4 = New MySqlConnection(ConnectionString)
                objConnex4.Open()
            Catch ex As Exception
                AddErreur("OPENING DB 4", "", ex)
            End Try
        End If
    End Sub

    Private Sub CheckConnection5()
        If Not objConnex5.State = Data.ConnectionState.Open Then
            Try
                objConnex5 = New MySqlConnection(ConnectionString)
                objConnex5.Open()
            Catch ex As Exception
                AddErreur("OPENING DB 5", "", ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Permet de récuperer un objet READER et une connexion libre
    ''' </summary>
    ''' <returns></returns>
    Private Function AssigneFreeDataReader() As Integer
        If objDR Is Nothing Then
            CheckConnection()
            Return 1
        End If
        If objDR2 Is Nothing Then
            CheckConnection2()
            Return 2
        End If
        If objDR3 Is Nothing Then
            CheckConnection3()
            Return 3
        End If
        If objDR4 Is Nothing Then
            CheckConnection4()
            Return 4
        End If
        If objDR5 Is Nothing Then
            CheckConnection5()
            Return 5
        End If

        Return 0
    End Function

    ''' <summary>
    ''' Permet de router vers les bons objets de connexion en fonction du reader assigné.
    ''' N'utilise pas de paramètre
    ''' </summary>
    ''' <param name="Requete"></param>
    ''' <returns></returns>
    Public Function RequeteSELECT_Generic(ByVal Requete As String) As MySqlDataReader
        Dim DataReaderUsed As Integer
        Dim TempDataReader As MySqlDataReader
        DataReaderUsed = AssigneFreeDataReader()
        Select Case DataReaderUsed
            Case 1
                TempDataReader = RequeteSELECT(Requete)
            Case 2
                TempDataReader = RequeteSELECT2(Requete)
            Case 3
                TempDataReader = RequeteSELECT3(Requete)
            Case 4
                TempDataReader = RequeteSELECT4(Requete)
            Case 5
                TempDataReader = RequeteSELECT5(Requete)
            Case Else
                TempDataReader = Nothing
        End Select
        Return TempDataReader
    End Function

    ''' <summary>
    ''' Permet de router vers les bons objets de connexion en fonction du reader assigné.
    ''' Utilise des paramètres
    ''' </summary>
    ''' <param name="Requete"></param>
    ''' <returns></returns>
    Public Function RequeteSELECT_Generic(ByVal Requete As String, ByVal ListeParametre As List(Of ParamSQL)) As MySqlDataReader
        Dim DataReaderUsed As Integer
        Dim TempDataReader As MySqlDataReader
        DataReaderUsed = AssigneFreeDataReader()
        Select Case DataReaderUsed
            Case 1
                TempDataReader = RequeteSELECT(Requete, ListeParametre)
            Case 2
                TempDataReader = RequeteSELECT2(Requete, ListeParametre)
            Case 3
                TempDataReader = RequeteSELECT3(Requete, ListeParametre)
            Case 4
                TempDataReader = RequeteSELECT4(Requete, ListeParametre)
            Case 5
                TempDataReader = RequeteSELECT5(Requete, ListeParametre)
            Case Else
                TempDataReader = Nothing
        End Select
        Return TempDataReader
    End Function

    ''' <summary>
    ''' A la fin d'une lecture de données, on vient systématiquement fermer l'acces aux données et liberer l'objet Reader.
    ''' </summary>
    ''' <param name="Index"></param>
    Public Sub ClearSqlDataLayer(ByVal Index As Integer)
        Select Case Index
            Case 1
                objDR.Close()
                objDR = Nothing
            Case 2
                objDR2.Close()
                objDR2 = Nothing
            Case 3
                objDR3.Close()
                objDR3 = Nothing
            Case 4
                objDR4.Close()
                objDR4 = Nothing
            Case 5
                objDR5.Close()
                objDR5 = Nothing
            Case Else
                AddErreur("ClearSqlDataLayer", "Index called : " & Index, New Exception)
        End Select
    End Sub

    ''' <summary>
    ''' Ferme toutes les connexions courante et marque la fin d'utilisation des données SQL. Evite la surcharge serveur.
    ''' </summary>
    Public Sub CloseSQL()
        If objConnex.State = Data.ConnectionState.Open Then
            objConnex.Close()
        End If
        If objConnex2.State = Data.ConnectionState.Open Then
            objConnex2.Close()
        End If
        If objConnex3.State = Data.ConnectionState.Open Then
            objConnex3.Close()
        End If
        If objConnex4.State = Data.ConnectionState.Open Then
            objConnex4.Close()
        End If
        If objConnex5.State = Data.ConnectionState.Open Then
            objConnex5.Close()
        End If
    End Sub
#End Region
#Region "SQL Reader"
    ''' <summary>
    ''' Méthode de lecture sans parametre, exemple :
    ''' SELECT * FROM MISSIONS
    ''' </summary>
    ''' <param name="requete"></param>
    ''' <returns></returns>
    Private Function RequeteSELECT(ByVal requete As String) As MySqlDataReader
        Try
            Try
                objDR.Close()
            Catch ex As Exception
            End Try
            objComm = New MySqlCommand(requete, objConnex)
            objDR = objComm.ExecuteReader
            objComm.Dispose()
        Catch ex As Exception
            AddErreur("RequeteSELECT", requete, ex)
        End Try
        Return objDR
    End Function

    ''' <summary>
    ''' Méthode de lecture avec parametre, exemple :
    ''' SELECT * FROM MISSIONS WHERE ID=@IDMission
    ''' </summary>
    ''' <param name="requete"></param>
    ''' <returns></returns>
    Private Function RequeteSELECT(ByVal requete As String, ByVal ListeParametre As List(Of ParamSQL)) As MySqlDataReader
        Try
            Try
                objDR.Close()
            Catch ex As Exception
            End Try
            objComm = New MySqlCommand(requete, objConnex)
            For Each Param In ListeParametre
                objComm.Parameters.AddWithValue("@" & Param.NomChamp, Param.ValueChamp)
            Next
            objDR = objComm.ExecuteReader
            objComm.Dispose()
        Catch ex As Exception
            AddErreur("RequeteSELECT", requete & " || " & ParamSQL.ShowMeEveryParam(ListeParametre), ex)
        End Try
        Return objDR
    End Function

    Private Function RequeteSELECT2(ByVal requete As String) As MySqlDataReader
        Try
            Try
                objDR2.Close()
            Catch ex As Exception
            End Try
            objComm2 = New MySqlCommand(requete, objConnex2)
            objDR2 = objComm2.ExecuteReader
            objComm2.Dispose()
        Catch ex As Exception
            AddErreur("RequeteSELECT2", requete, ex)
        End Try
        Return objDR2
    End Function

    Private Function RequeteSELECT2(ByVal requete As String, ByVal ListeParametre As List(Of ParamSQL)) As MySqlDataReader
        Try
            Try
                objDR2.Close()
            Catch ex As Exception
            End Try
            objComm2 = New MySqlCommand(requete, objConnex2)
            For Each Param In ListeParametre
                objComm2.Parameters.AddWithValue("@" & Param.NomChamp, Param.ValueChamp)
            Next
            objDR2 = objComm2.ExecuteReader
            objComm2.Dispose()
        Catch ex As Exception
            AddErreur("RequeteSELECT2", requete & " || " & ParamSQL.ShowMeEveryParam(ListeParametre), ex)
        End Try
        Return objDR2
    End Function

    Private Function RequeteSELECT3(ByVal requete As String) As MySqlDataReader
        Try
            Try
                objDR3.Close()
            Catch ex As Exception
            End Try
            objComm3 = New MySqlCommand(requete, objConnex3)
            objDR3 = objComm3.ExecuteReader
            objComm3.Dispose()
        Catch ex As Exception
            AddErreur("RequeteSELECT3", requete, ex)
        End Try
        Return objDR3
    End Function

    Private Function RequeteSELECT4(ByVal requete As String) As MySqlDataReader
        Try
            Try
                objDR4.Close()
            Catch ex As Exception
            End Try
            objComm4 = New MySqlCommand(requete, objConnex4)
            objDR4 = objComm4.ExecuteReader
            objComm4.Dispose()
        Catch ex As Exception
            AddErreur("RequeteSELECT4", requete, ex)
        End Try
        Return objDR4
    End Function

    Private Function RequeteSELECT3(ByVal requete As String, ByVal ListeParametre As List(Of ParamSQL)) As MySqlDataReader
        Try
            Try
                objDR3.Close()
            Catch ex As Exception
            End Try
            objComm3 = New MySqlCommand(requete, objConnex3)
            For Each Param In ListeParametre
                objComm3.Parameters.AddWithValue("@" & Param.NomChamp, Param.ValueChamp)
            Next
            objDR3 = objComm3.ExecuteReader
            objComm3.Dispose()
        Catch ex As Exception
            AddErreur("RequeteSELECT3", requete & " || " & ParamSQL.ShowMeEveryParam(ListeParametre), ex)
        End Try
        Return objDR3
    End Function
    Private Function RequeteSELECT4(ByVal requete As String, ByVal ListeParametre As List(Of ParamSQL)) As MySqlDataReader
        Try
            Try
                objDR4.Close()
            Catch ex As Exception
            End Try
            objComm4 = New MySqlCommand(requete, objConnex4)
            For Each Param In ListeParametre
                objComm4.Parameters.AddWithValue("@" & Param.NomChamp, Param.ValueChamp)
            Next
            objDR4 = objComm4.ExecuteReader
            objComm4.Dispose()
        Catch ex As Exception
            AddErreur("RequeteSELECT4", requete & " || " & ParamSQL.ShowMeEveryParam(ListeParametre), ex)
        End Try
        Return objDR4
    End Function
    Private Function RequeteSELECT5(ByVal requete As String) As MySqlDataReader
        Try
            Try
                objDR5.Close()
            Catch ex As Exception
            End Try
            objComm5 = New MySqlCommand(requete, objConnex5)
            objDR5 = objComm5.ExecuteReader
            objComm5.Dispose()
        Catch ex As Exception
            AddErreur("RequeteSELECT5", requete, ex)
        End Try
        Return objDR5
    End Function
    Private Function RequeteSELECT5(ByVal requete As String, ByVal ListeParametre As List(Of ParamSQL)) As MySqlDataReader
        Try
            Try
                objDR5.Close()
            Catch ex As Exception
            End Try
            objComm5 = New MySqlCommand(requete, objConnex5)
            For Each Param In ListeParametre
                objComm5.Parameters.AddWithValue("@" & Param.NomChamp, Param.ValueChamp)
            Next
            objDR5 = objComm5.ExecuteReader
            objComm5.Dispose()
        Catch ex As Exception
            AddErreur("RequeteSELECT5", requete & " || " & ParamSQL.ShowMeEveryParam(ListeParametre), ex)
        End Try
        Return objDR5
    End Function
#End Region
#Region "ExecuteNonQuery"
    ''' <summary>
    ''' Permet de jouer une requete INSERT / DELETE / UPDATE sans parametres
    ''' Attends en paramètre une requete classique. On evitera de rentrer des variables dans ces requetes. 
    ''' Si besoin d'integrer des variables dans la requete il faut utiliser la méthode AVEC paramètres.
    ''' </summary>
    ''' <param name="requete"></param>
    Public Sub RequeteINSERTDELETE(ByVal requete As String)
        Try
            CheckConnection()

            Try
                objDR.Close()
            Catch ex As Exception
            End Try
            objCommQuerry = New MySqlCommand(requete, objConnex)
            Nb = objCommQuerry.ExecuteNonQuery

        Catch ex As Exception
            AddErreur("RequeteINSERTDELETE", requete, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Permet de jouer une requete INSERT / DELETE / UPDATE avec parametres
    ''' Attends en paramètre une requete classique + une liste de parametre de type ParamSQL
    ''' </summary>
    ''' <param name="requete"></param>
    ''' <param name="ListParam"></param>
    ''' <returns></returns>
    Public Function RequeteINSERTDELETE(ByVal requete As String, ByVal ListParam As List(Of ParamSQL)) As Integer
        Try
            CheckConnection()

            Try
                objDR.Close()
            Catch ex As Exception
            End Try
            objCommQuerry = New MySqlCommand(requete, objConnex)
            For Each Param In ListParam
                objCommQuerry.Parameters.AddWithValue("@" & Param.NomChamp, Param.ValueChamp)
            Next
            Return objCommQuerry.ExecuteNonQuery
        Catch ex As Exception
            AddErreur("RequeteINSERTDELETE", requete & " || " & ParamSQL.ShowMeEveryParam(ListParam), ex)
            Return 0
        End Try
    End Function
#End Region
#Region "Erreur"
    ''' <summary>
    ''' Permet de formalisé une erreur d'execution et d'ecrire une trace de l'erreur dans la table ERREUR
    ''' Attention à bien avoir la table erreur présente. Si besoin jouer le script SQL de création de table Erreur
    ''' </summary>
    ''' <param name="FunctionCalled"></param>
    ''' <param name="Parameters"></param>
    ''' <param name="Exc"></param>
    Public Sub AddErreur(ByVal FunctionCalled As String, ByVal Parameters As String, ByVal Exc As Exception)
        Try
            Dim LstParamSQL As New List(Of ParamSQL)
            Dim requete As String

            LstParamSQL.Add(ParamSQL.CustomParameter("DateErr", Now, "DATETIME"))
            LstParamSQL.Add(New ParamSQL("Func", FunctionCalled))
            LstParamSQL.Add(New ParamSQL("Params", Parameters))
            LstParamSQL.Add(New ParamSQL("Trace", Exc.StackTrace))
            LstParamSQL.Add(New ParamSQL("ErrMsg", Exc.Message))

            'Ajouter une facon d'identifier l'utilisateur si besoin.
            'LstParamSQL.Add(New ParamSQL("LoginUser", p_User))

            requete = "INSERT INTO Erreur(DateErreur, FunctionCalled, ParametersCalled, StackTrace, Message)"
            requete &= " VALUES(@DateErr, @Func, @Params, @Trace, @ErrMsg)"
            RequeteINSERTDELETE(requete, LstParamSQL)
        Catch ex As Exception
        End Try
    End Sub
#End Region
#Region "Champs MAX"
    ''' <summary>
    ''' Permet de récupérer un champs Max d'une table
    ''' On evitera un maximum de déterminer les ID par cette méthode
    ''' On préferera rendre l'identifiant de la tableau auto incrément
    ''' </summary>
    ''' <param name="Champs"></param>
    ''' <param name="Table"></param>
    ''' <returns></returns>
    Public Function GetIdMax(ByVal Champs As String, ByVal Table As String) As Integer
        Dim requete As String
        Dim IdMax As Integer = 0
        Dim ObjDR As MySqlDataReader
        Try
            requete = "SELECT MAX(" & Champs & ") FROM " & Table
            ObjDR = RequeteSELECT_Generic(requete)
            ObjDR.Read()
            IdMax = ObjDR.Item(0) + 1
        Catch ex As Exception
            IdMax = 1
        End Try
        Return IdMax
    End Function
#End Region

End Class
