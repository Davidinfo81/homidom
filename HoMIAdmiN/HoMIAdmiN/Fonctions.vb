﻿Imports System.IO
Imports System.Net
Imports System.Net.Sockets

Module Fonctions
    Public Function ConvertArrayToImage(ByVal value As Object, Optional ByVal Taille As Integer = 0) As Object
        Try
            Dim ImgSource As BitmapImage = Nothing
            Dim array As Byte() = TryCast(value, Byte())

            If array IsNot Nothing Then
                ImgSource = New BitmapImage()
                If Taille > 0 Then
                    ImgSource.DecodePixelHeight = Taille
                    ImgSource.DecodePixelWidth = Taille
                End If
                ImgSource.BeginInit()
                ImgSource.CacheOption = BitmapCacheOption.OnLoad
                ImgSource.CreateOptions = BitmapCreateOptions.DelayCreation
                ImgSource.StreamSource = New MemoryStream(array)
                array = Nothing
                ImgSource.EndInit()
                If ImgSource.CanFreeze Then ImgSource.Freeze()
            End If

            Return ImgSource
            ImgSource = Nothing
        Catch ex As Exception
            MessageBox.Show("ERREUR Sub ConvertArrayToImage: " & ex.Message, "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error)
            Return Nothing
        End Try
    End Function

    Public Function UrlIsValid(ByVal Host As String) As Boolean
        Try
            Dim bValid As Boolean = False
            Try
                Dim url As New Uri(Host)
                Dim Request As HttpWebRequest = CType(HttpWebRequest.Create(url), System.Net.HttpWebRequest)
                Dim response As Net.HttpWebResponse = CType(Request.GetResponse(), Net.HttpWebResponse)
                bValid = True
            Catch ex As Exception
                bValid = False
            End Try
            Return bValid
        Catch ex As Exception
            MessageBox.Show("ERREUR Sub UrlIsValid: " & ex.Message, "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error)
            Return False
        End Try
    End Function

    Public Sub SaveRealTime()
        Try
            If IsConnect And My.Settings.SaveRealTime Then
                Dim retour As String = myService.SaveConfig(IdSrv)
                If retour = "0" Then FlagChange = False
            End If
        Catch ex As Exception
            MessageBox.Show("ERREUR Sub SaveRealTime: " & ex.Message, "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Public Function ImageFromUri(ByVal Uri As String) As Windows.Media.Imaging.BitmapImage
        Try
            Dim bmpImage As New BitmapImage()

            bmpImage.BeginInit()
            bmpImage.CacheOption = BitmapCacheOption.OnLoad
            bmpImage.CreateOptions = BitmapCreateOptions.DelayCreation
            bmpImage.UriSource = New Uri(Uri, UriKind.Absolute)
            bmpImage.EndInit()
            If bmpImage.CanFreeze Then bmpImage.Freeze()
            Return bmpImage

            bmpImage = Nothing
        Catch ex As Exception
            MessageBox.Show("ERREUR ImageFromUri: " & ex.Message, "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error)
            Return Nothing
        End Try
    End Function
End Module