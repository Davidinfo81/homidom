﻿Imports System.Data.SQLite
Imports HoMIDom.HoMIDom.Server
Imports STRGS = Microsoft.VisualBasic.Strings
Imports System.IO

Namespace HoMIDom


    ''' <summary>      
    ''' Classe de gestion des base de données sqlite     
    ''' </summary>   
    '''    ''' <remarks></remarks> 
    ''' 
    Public Class Sqlite
        Dim _Server As Server
        Private SQLconnect As New SQLiteConnection()
        Private Shared lock As New Object
        Public bdd_name As String
        Public connecte As Boolean

        ''' <summary>          
        ''' Creation de l'objet
        ''' </summary>
        ''' <param name="basename">Nom de la base de donnée : homidom / medias</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal basename As String, ByVal Server As Server)
            Try
                _Server = server
                If basename <> "" Then
                    bdd_name = basename
                Else
                    bdd_name = ""
                    _Server.Log(server.TypeLog.ERREUR, server.TypeSource.SERVEUR, "SQLlite:New", "Le nom de la BDD est vide")
                End If
                'ajout d'un handler pour capturer les erreurs non catchés
                AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf sqlite_UnhandledExceptionEvent
                connecte = False
            Catch ex As Exception
                _Server.Log(server.TypeLog.ERREUR, server.TypeSource.SERVEUR, "SQLlite:New", "Exception : " & ex.ToString)
            End Try
        End Sub

        ''' <summary>Fourni l'etat d'une connexion à la bdd</summary>
        ''' <returns>True/False</returns> 
        Public Function getconnecte() As Boolean
            Try
                If SQLconnect.State = ConnectionState.Open Then
                    connecte = True
                    Return True
                Else
                    connecte = False
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>          
        ''' Connection à une BDD Sqlite
        ''' </summary>
        ''' <returns>String si OK, String "ERR:..." si erreur</returns>
        ''' <remarks></remarks>          
        Public Function connect() As String
            Try

                'If File.Exists("./bdd/" & bdd_name & ".db") Then
                '    SQLconnect.ConnectionString = "Data Source=./bdd/" & bdd_name & ".db;"
                If File.Exists(My.Application.Info.DirectoryPath & "/bdd/" & bdd_name & ".db") Then
                    SQLconnect.ConnectionString = "Data Source=" & My.Application.Info.DirectoryPath & "/bdd/" & bdd_name & ".db;"
                    SQLconnect.Open()
                    If SQLconnect.State = ConnectionState.Open Then
                        connecte = True
                        Return "Connecté"
                    Else
                        connecte = False
                        Return "ERR: Non Connecté à la base sqlite " & bdd_name
                    End If
                Else
                    Return "ERR: base sqlite " & bdd_name & " non trouvé"
                End If
            Catch ex As Exception
                Return "ERR: Non Connecté à la base sqlite " & bdd_name & " exception: " & ex.Message
            End Try
        End Function

        ''' <summary>
        ''' DeConnection à une BDD Sqlite 
        ''' </summary>       
        ''' <returns>String si OK, String "ERR:..." si erreur</returns>
        ''' <remarks></remarks>          
        ''' 
        Public Function disconnect() As String
            Try
                connecte = False
                SQLconnect.Close()
                Return "Deconnecté"
            Catch ex As Exception
                Return "ERR: Erreur lors de la déconnexion de la base " & bdd_name & " : " & ex.Message
            End Try
        End Function

        ''' <summary>
        ''' Requete sans résultat
        ''' </summary>
        ''' <param name="commande">ex : DELETE FROM contact where Contact_id=10</param>
        ''' <param name="params">Liste es paramètres utilisés dans la query sous forme @parameter0, @parameter1 etc</param>
        ''' <returns>String si OK, String "ERR:..." si erreur</returns>
        ''' <remarks></remarks>
        Public Function nonquery(ByVal commande As String, ByVal ParamArray params() As String) As String
            Try
                'on vérifie si la commande n'est pas vide
                If Not String.IsNullOrEmpty(commande) Then
                    SyncLock lock
                        connect() 'lock pour etre sur de ne pas faire deux operations en meme temps 
                        'on vérifie si on est connecté à la BDD       
                        If SQLconnect.State = ConnectionState.Open Then
                            Dim SQLcommand As SQLiteCommand
                            SQLcommand = SQLconnect.CreateCommand
                            SQLcommand.CommandText = commande
                            If params IsNot Nothing Then
                                For p = 0 To params.Length - 1
                                    SQLcommand.Parameters.Add(New SQLiteParameter("@parameter" + p.ToString(), params(p)))
                                Next
                            End If
                            'SyncLock lock 'lock pour etre sur de ne pas faire deux operations en meme temps 
                            Try
                                SQLcommand.ExecuteNonQuery()
                            Catch ex As Exception
                                Return "ERR: Erreur lors de la nonquery Execute " & commande & "  --> " & ex.ToString
                            End Try
                            'End SyncLock
                            SQLcommand.Dispose()
                            disconnect()
                            Return "Commande éxécutée avec succés : " & commande
                        Else
                            connecte = False
                            Return "ERR: Non connecté à la BDD " & bdd_name
                        End If
                    End SyncLock
                Else
                    Return "ERR: La commande est vide"
                End If
            Catch ex As Exception
                Return "ERR: Erreur lors de la nonquery " & commande & "  --> " & ex.ToString
            End Try
        End Function

        ''' <summary>Requete avec résultat</summary>          
        ''' <param name="commande">ex : SELECT * FROM contact</param>          
        ''' <param name="resultat">Arralist contenant la liste des résultats</param>          
        ''' <returns>String si OK, String "ERR:..." si erreur</returns>          
        ''' <remarks></remarks>          
        Public Function query(ByVal commande As String, ByRef resultat As DataTable, ByVal ParamArray params() As String) As String
           Try
                'on vérifie si la commande n'est pas vide  
                'If commande IsNot Nothing And commande <> "" Then
                If Not String.IsNullOrEmpty(commande) Then
                    'lock pour etre sur de ne pas faire deux operations en meme temps
                    SyncLock lock
                        connect()

                        'on vérifie si on est connecté à la BDD   
                        If SQLconnect.State = ConnectionState.Open Then
                            Dim SQLcommand As SQLiteCommand
                            Dim SQLreader As SQLiteDataReader
                            SQLcommand = SQLconnect.CreateCommand
                            SQLcommand.CommandText = commande

                            If params IsNot Nothing Then
                                For p = 0 To params.Length - 1
                                    SQLcommand.Parameters.Add(New SQLiteParameter("@parameter" + p.ToString(), params(p)))
                                Next
                            End If
                            ''lock pour etre sur de ne pas faire deux operations en meme temps
                            'SyncLock lock

                            Try
                                SQLreader = SQLcommand.ExecuteReader()
                            Catch ex As Exception
                                Return "ERR: Erreur lors de la querysimple ExecuteReader " & commande & "  --> " & ex.ToString
                            End Try
                            'End SyncLock
                            SQLcommand.Dispose()
                            If SQLreader.HasRows = True Then
                                'lecture de la premiere ligne       
                                'SQLreader.Read()

                                Dim x As DataColumn
                                Dim resultattemp As New DataTable
                                For i = 0 To SQLreader.FieldCount - 1 'pour chaque colonne, on va créer la même dans le datable 
                                    x = New DataColumn("col_" & i)
                                    resultattemp.Columns.Add(x)
                                Next
                                x = Nothing

                                'lecture des lignes   
                                Dim j As Integer = 0
                                While SQLreader.Read()
                                    Dim row As DataRow = resultattemp.NewRow() ' on créé une premiere ligne dans le datatble      
                                    For i = 0 To SQLreader.FieldCount - 1
                                        row.Item(i) = SQLreader(i)
                                    Next
                                    resultattemp.Rows.Add(row)
                                    j += 1
                                End While
                                resultat = resultattemp
                                resultattemp = Nothing
                                SQLreader.Close()
                                disconnect()
                                Return "Commande éxécutée avec succés : " & commande
                            Else
                                SQLreader.Close()
                                disconnect()
                                Return "Commande éxécutée avec succés mais pas de résultat : " & commande
                            End If

                        Else
                            connecte = False
                            Return "ERR: Non connecté à la BDD " & bdd_name
                        End If
                    End SyncLock
                Else
                    Return "ERR: La commande est vide"
                End If
            Catch ex As Exception
                Return "ERR: Erreur lors de la query " & commande & " Erreur: " & ex.ToString
            End Try
        End Function

        ''' <summary>Requete avec résultat simple</summary>          
        ''' <param name="commande">ex : SELECT * FROM contact</param>          
        ''' <param name="resultat">Arralist contenant la liste des résultats</param>          
        ''' <returns>String si OK, String "ERR:..." si erreur</returns>          
        ''' <remarks></remarks>          
        Public Function querysimple(ByVal commande As String, ByRef resultat As String, ByVal ParamArray params() As String) As String
            Try
                'on vérifie si la commande n'est pas vide
                'If commande IsNot Nothing And commande <> "" Then
                If Not String.IsNullOrEmpty(commande) Then
                    'lock pour etre sur de ne pas faire deux operations en meme temps
                    SyncLock lock
                        connect()
                        'on vérifie si on est connecté à la BDD   
                        If SQLconnect.State = ConnectionState.Open Then
                            Dim SQLcommand As SQLiteCommand
                            SQLcommand = SQLconnect.CreateCommand
                            SQLcommand.CommandText = commande

                            If params IsNot Nothing Then
                                For p = 0 To params.Length - 1
                                    SQLcommand.Parameters.Add(New SQLiteParameter("@parameter" + p.ToString(), params(p)))
                                Next
                            End If
                            ''lock pour etre sur de ne pas faire deux operations en meme temps
                            'SyncLock lock
                            resultat = SQLcommand.ExecuteScalar
                            Try
                                resultat = SQLcommand.ExecuteScalar
                            Catch ex As Exception
                                Return "ERR: Erreur lors de la querysimple ExecuteScalar " & commande & "  --> " & ex.ToString
                            End Try
                            'End SyncLock
                            SQLcommand.Dispose()
                            disconnect()
                            Return "Commande éxécutée avec succés : " & commande
                        Else
                            connecte = False
                            Return "ERR: Non connecté à la BDD " & bdd_name
                        End If
                    End SyncLock
                Else
                    Return "ERR: La commande est vide"
                End If
            Catch ex As Exception
                Return "ERR: Erreur lors de la querysimple " & commande & " Erreur: " & ex.ToString
            End Try
        End Function

        ''' <summary>Compte le nombre de resultat</summary>          
        ''' <param name="commande">ex : SELECT COUNT(*) FROM table WHERE col1 = 'xxx'</param>          
        ''' <param name="resultat">Nombre de resultat</param>          
        ''' <returns>String si OK, String "ERR:..." si erreur</returns>          
        ''' <remarks></remarks>          
        Public Function count(ByVal commande As String, ByRef resultat As Integer) As String
            Try

                'on vérifie si la commande n'est pas vide
                'If commande IsNot Nothing And commande <> "" Then
                resultat = 0
                If Not String.IsNullOrEmpty(commande) Then
                    connect()
                    'on vérifie si on est connecté à la BDD   
                    If SQLconnect.State = ConnectionState.Open Then
                        Dim SQLcommand As SQLiteCommand
                        SQLcommand = SQLconnect.CreateCommand
                        SQLcommand.CommandText = commande

                        'lock pour etre sur de ne pas faire deux operations en meme temps
                        SyncLock lock
                            resultat = SQLcommand.ExecuteScalar()
                        End SyncLock

                        SQLcommand.Dispose()
                        disconnect()
                        Return "Commande éxécutée avec succés : " & commande
                    Else
                        connecte = False
                        Return "ERR: Non connecté à la BDD " & bdd_name
                    End If
                Else
                    Return "ERR: La commande est vide"
                End If
            Catch ex As Exception
                Return "ERR: Erreur lors de la query Count " & commande & " Erreur: " & ex.ToString
            End Try
        End Function

        Private Sub sqlite_UnhandledExceptionEvent(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
            _Server.Log(TypeLog.ERREUR, TypeSource.SERVEUR, "SQLite UnhandledExceptionEvent", "Exception : " & e.ExceptionObject.ToString())
        End Sub

    End Class
End Namespace
