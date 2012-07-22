﻿Imports System.Windows.Forms
Imports System.Drawing

Public Class WWidgetProperty
    Dim Obj As uWidgetEmpty


    Public Sub New(ByVal Objet As uWidgetEmpty)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Obj = Objet
        ChkShowStatus.IsChecked = Obj.ShowStatus
        ChkShowEtiq.IsChecked = Obj.ShowEtiquette
        TxtEtiq.Text = Obj.Etiquette
        TxtX.Text = Obj.X
        TxtY.Text = Obj.Y
        TxtWidth.Text = Obj.Width
        TxtHeight.Text = Obj.Height
        TxtRotation.Text = Obj.Rotation
        TxtDefStatus.Text = Obj.DefautLabelStatus
        lblColor.Background = Obj.ColorBackGround
        ColorPicker1.SelectedColor = Obj.ColorBackGround
        ImgPicture.Source = ConvertArrayToImage(myService.GetByteFromImage(Obj.Picture))


    End Sub

    Private Sub BtnOk_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnOk.Click
        Obj.ShowStatus = ChkShowStatus.IsChecked
        Obj.ShowEtiquette = ChkShowEtiq.IsChecked
        Obj.Etiquette = TxtEtiq.Text
        Obj.X = TxtX.Text
        Obj.Y = TxtY.Text
        Obj.Width = TxtWidth.Text
        Obj.Height = TxtHeight.Text
        Obj.Rotation = TxtRotation.Text
        Obj.DefautLabelStatus = TxtDefStatus.Text
        Obj.ColorBackGround = ColorPicker1.SelectedColor

        For i As Integer = 0 To _ListElement.Count - 1
            If _ListElement.Item(i).Id = Obj.Id And _ListElement.Item(i).ZoneId = Obj.ZoneId Then
                _ListElement.Item(i) = Obj
                Exit For
            End If
        Next

        DialogResult = True
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnCancel.Click
        DialogResult = False
    End Sub

    Private Function ShowDialog(ByVal color As Nullable(Of System.Windows.Media.Color)) As Nullable(Of System.Windows.Media.Color)
        ' Instancier une boite de dilogue de Winform 
        Dim dialogBox As New System.Windows.Forms.ColorDialog()

        ' Configurer cette boite
        If color.HasValue Then
            dialogBox.Color = System.Drawing.Color.FromArgb(color.Value.A, color.Value.R, color.Value.G, color.Value.B)
        End If

        ' Affichage de la boite de dialogue
        If dialogBox.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            ' Retourner la couleur choisie
            Return System.Windows.Media.Color.FromArgb(dialogBox.Color.A, dialogBox.Color.R, dialogBox.Color.G, dialogBox.Color.B)
        Else
            ' Selection annulée
            Return Nothing
        End If
    End Function


    Private Sub BtnEditAction_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnEditAction.Click
        GrpEditAction.Visibility = Windows.Visibility.Visible
    End Sub

    Private Sub BtnEditVisu_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnEditVisu.Click
        GrpEditVisu.Visibility = Windows.Visibility.Visible
    End Sub
End Class
