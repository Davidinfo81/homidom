﻿Imports System.Data
Imports System.IO
Imports System.Collections.ObjectModel

Partial Public Class uLog
    Public Event CloseMe(ByVal MyObject As Object)
    Public ligneLog As New ObservableCollection(Of Dictionary(Of String, Object))
    Dim keys As New List(Of String)
    Dim headers As String() = {"datetime", "typesource", "source", "fonction", "message"}

    Private Sub BtnRefresh_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnRefresh.Click
        RefreshLog()
    End Sub

    Private Sub RefreshLog()
        Try
            'Variables
            Dim _LigneIgnorees As Integer = 0
            Me.Cursor = Cursors.Wait

            If IsConnect = True Then
                Dim TargetFile As StreamWriter
                TargetFile = New StreamWriter("log.txt", False)
                TargetFile.Write(myService.ReturnLog)
                TargetFile.Close()

                Dim tr As TextReader = New StreamReader("log.txt")
                Dim lineCount As Integer = 1

                While tr.Peek() >= 0
                    Try
                        Dim line As String = tr.ReadLine()

                        Dim tmp As String() = line.Trim.Split(vbTab)
                        Dim data As String() = Nothing

                        If tmp.Length < 6 And tmp.Length > 3 Then
                            If tmp(4).Length > 255 Then tmp(4) = Mid(tmp(4), 1, 255)
                            data = tmp
                        Else
                            _LigneIgnorees += 1
                        End If

                        ' creates a dictionary where the column name is the key and
                        '    and the data is the value
                        Dim sensorData As New Dictionary(Of String, Object)

                        If data IsNot Nothing Then
                            For i As Integer = 0 To data.Length - 1
                                sensorData(keys(i)) = data(i)
                            Next
                            ligneLog.Add(sensorData)
                        End If

                        sensorData = Nothing
                        lineCount += 1
                    Catch ex As Exception
                        MessageBox.Show("Erreur lors de la ligne du fichier log: " & ex.ToString, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error)
                    End Try
                End While
            End If

            Try
                If _LigneIgnorees > 0 Then
                    MessageBox.Show(_LigneIgnorees & " ligne(s) du log ne seront pas prises en compte car elles ne respectent pas le format attendu, veuillez consultez le fichier log sur le serveur pour avoir la totalité")
                End If

                DGW.DataContext = ligneLog
            Catch ex As Exception
                MessageBox.Show("Erreur: " & ex.ToString)
            End Try

            'Supression du fichier temporaire
            Me.Cursor = Nothing
        Catch ex As Exception
            MessageBox.Show("Erreur lors de la récuppération du fichier log: " & ex.ToString)
        End Try
    End Sub

    Private Sub CreateGridColumn(ByVal headerText As String)
        Try
            Dim col1 As DataGridTextColumn = New DataGridTextColumn()
            col1.Header = headerText
            col1.Binding = New Binding(String.Format("[{0}]", headerText))
            DGW.Columns.Add(col1)
        Catch ex As Exception
            MessageBox.Show("Erreur: " & ex.ToString)
        End Try
    End Sub

    Public Sub New()
        Try

            ' Cet appel est requis par le Concepteur Windows Form.
            InitializeComponent()

            ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
            For Each h As String In headers
                CreateGridColumn(h)
                keys.Add(h)
            Next

            RefreshLog()
        Catch ex As Exception
            MessageBox.Show("Erreur lors sur la fonction New de uLog: " & ex.ToString)
        End Try
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnClose.Click
        RaiseEvent CloseMe(Me)
    End Sub

    Private Sub DGW_LoadingRow(ByVal sender As Object, ByVal e As System.Windows.Controls.DataGridRowEventArgs) Handles DGW.LoadingRow
        Try
            'Dim RowDataContaxt As System.Data.DataRowView = TryCast(e.Row.DataContext, System.Data.DataRowView)
            Dim RowDataContaxt As Dictionary(Of String, Object) = TryCast(e.Row.DataContext, Dictionary(Of String, Object))
            If RowDataContaxt IsNot Nothing Then
                'If RowDataContaxt.Row.IsNull(1) = False Then
                Select Case RowDataContaxt(keys(1))
                    Case "INFO"
                        'RowDataContaxt.Row.Item(1) = "INFO"
                        e.Row.Background = Brushes.White
                    Case "ACTION"
                        'RowDataContaxt.Row.Item(1) = "ACTION"
                        e.Row.Background = Brushes.White
                    Case "MESSAGE"
                        'RowDataContaxt.Row.Item(1) = "MESSAGE"
                        e.Row.Background = Brushes.White
                    Case "VALEUR CHANGE"
                        'RowDataContaxt.Row.Item(1) = "VALEUR CHANGE"
                        e.Row.Background = Brushes.White
                    Case "VALEUR INCHANGE"
                        'RowDataContaxt.Row.Item(1) = "VALEUR INCHANGE"
                        e.Row.Background = Brushes.White
                    Case "VALEUR INCHANGE PRECISION"
                        'RowDataContaxt.Row.Item(1) = "VALEUR INCHANGE PRECISION"
                        e.Row.Background = Brushes.White
                    Case "VALEUR INCHANGE LASTETAT"
                        'RowDataContaxt.Row.Item(1) = "VALEUR INCHANGE LASTETAT"
                        e.Row.Background = Brushes.White
                    Case "ERREUR"
                        'RowDataContaxt.Row.Item(1) = "ERREUR"
                        e.Row.Background = Brushes.Red
                    Case "ERREUR CRITIQUE"
                        'RowDataContaxt.Row.Item(1) = "ERREUR CRITIQUE"
                        e.Row.Background = Brushes.Red
                    Case "DEBUG"
                        'RowDataContaxt.Row.Item(1) = "DEBUG"
                        e.Row.Background = Brushes.Yellow
                End Select
            End If

        Catch ex As Exception
            MsgBox("Err: " & ex.ToString)
        End Try
    End Sub


End Class
