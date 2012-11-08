﻿Imports System.Reflection

Partial Public Class uInternet
    Dim _Url As String 'Url internet de la page à afficher 

    Private Sub uInternet_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        x.Navigate(New Uri(_Url))
        x.Height = Me.Height - 30
    End Sub

    Public Sub New(ByVal Url As String)
        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        _Url = Url
        'AddHandler x.Navigating, New NavigatingCancelEventHandler(AddressOf wb_Navigating)
    End Sub

    Private Sub Image1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Image1.MouseDown
        x.GoBack()
    End Sub

    Private Sub Image2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Image2.MouseDown
        x.GoForward()
    End Sub

    'Private Sub wb_Navigating(ByVal sender As Object, ByVal e As System.Windows.Navigation.NavigationEventArgs)
    '    Dim wb As Controls.WebBrowser = sender
    '    SuppressScriptErrors(wb, True)
    'End Sub

    Sub SuppressScriptErrors(ByVal wb As Controls.WebBrowser, ByVal Hide As Boolean)
        Dim fi As FieldInfo = GetType(Controls.WebBrowser).GetField("_axIWebBrowser2", BindingFlags.NonPublic)
        If fi IsNot Nothing Then
            Dim browser As Controls.WebBrowser = fi.GetValue(wb)
            If browser IsNot Nothing Then
                browser.[GetType]().InvokeMember("Silent", BindingFlags.SetProperty, Nothing, browser, New Object() {Hide})
            End If
        End If

    End Sub

    Private Sub x_Navigated(ByVal sender As Object, ByVal e As System.Windows.Navigation.NavigationEventArgs) Handles x.Navigated
        SuppressScriptErrors(x, True)
    End Sub

    Private Sub x_Navigating(ByVal sender As Object, ByVal e As System.Windows.Navigation.NavigatingCancelEventArgs) Handles x.Navigating
        SuppressScriptErrors(x, True)

    End Sub
End Class
