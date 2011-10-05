﻿Imports System.IO

Public Class WConfig
    Dim Frm As Window1
    Dim FlagNewMnu As Integer = -1
    Dim MyListMnu As New List(Of uCtrlImgMnu)

    Private Sub BtnOk_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnOk.Click
        Frm.ShowSoleil = ChkSoleil.IsChecked
        Frm.ShowTemperature = ChkTemp.IsChecked
        Frm.Friction = SliderFriction.Value
        Frm.ImageBackGround = TxtImgBack.Text
        Frm.SpeedTouch = SliderSpeed.Value
        'Frm.EnableServiceTV = ChkServiceTV.IsChecked
        Frm.IP = TxtIP.Text
        Frm.PortSOAP = TxtPort.Text
        Frm.ListMnu = MyListMnu
        'RaiseEvent CloseControl(Me.Uid)
        DialogResult = True
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnCancel.Click
        DialogResult = False
    End Sub

    Public Sub New(ByVal FrmMere As Window1)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Frm = FrmMere
        ChkSoleil.IsChecked = Frm.ShowSoleil
        ChkTemp.IsChecked = Frm.ShowTemperature
        SliderFriction.Value = Frm.Friction
        LblFriction.Content = Frm.Friction
        TxtImgBack.Text = Frm.ImageBackGround
        SliderSpeed.Value = Frm.SpeedTouch
        LblSpeed.Content = Frm.SpeedTouch
        'ChkServiceTV.IsChecked =F My.Settings.EnableServiceTV
        TxtIP.Text = Frm.IP
        TxtPort.Text = Frm.PortSOAP

        For i As Integer = 0 To Frm.ListMnu.Count - 1
            Dim x As uCtrlImgMnu = Frm.ListMnu.Item(i)
            ListMnu.Items.Add(x.Text)
        Next
        MyListMnu = Frm.ListMnu

        If myService IsNot Nothing Then
            For i As Integer = 0 To myService.GetAllZones.Count - 1
                Dim x As New MenuItem
                x.Header = myService.GetAllZones.Item(i).Name
                x.Uid = myService.GetAllZones.Item(i).ID
                AddHandler x.Click, AddressOf MenuZone_Click
                MnuAddZone.Items.Add(x)
            Next
        End If
    End Sub

    Private Sub SliderFriction_ValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Double)) Handles SliderFriction.ValueChanged
        If LblFriction IsNot Nothing Then
            LblFriction.Content = SliderFriction.Value
        End If
    End Sub

    Private Sub BtnFileImgBack_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnFileImgBack.Click
        Dim dlg As New Microsoft.Win32.OpenFileDialog()
        dlg.Filter = "jpeg (*.jpg) |*.jpg;*.jpeg|(*.png) |*.png|(*.*) |*.*"

        If dlg.ShowDialog() = True Then
            TxtImgBack.Text = dlg.FileName
        End If
    End Sub

    Private Sub SliderSpeed_ValueChanged(ByVal sender As Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of Double)) Handles SliderSpeed.ValueChanged
        If LblSpeed IsNot Nothing Then
            LblSpeed.Content = SliderSpeed.Value
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Frm = Nothing
        MyBase.Finalize()
    End Sub

    Private Sub ListMnu_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ListMnu.SelectionChanged
        If ListMnu.SelectedIndex < 0 Then Exit Sub

        If ListMnu.SelectedIndex >= 0 Then
            BtnUP.IsEnabled = True
            BtnDown.IsEnabled = True
        End If
        If ListMnu.SelectedIndex = 0 Then
            BtnUP.IsEnabled = False
        End If
        If ListMnu.SelectedIndex = ListMnu.Items.Count - 1 Then
            BtnDown.IsEnabled = False
        End If

        If MyListMnu.Item(ListMnu.SelectedIndex).Defaut = True Then
            ListMnu.ContextMenu.Items(1).isenabled = False
            TxtName.Text = MyListMnu.Item(ListMnu.SelectedIndex).Text
            LblType.Content = MyListMnu.Item(ListMnu.SelectedIndex).Type.ToString
            TxtImage.Text = MyListMnu.Item(ListMnu.SelectedIndex).Icon
            StkProperty.Visibility = Windows.Visibility.Visible
            StkImage.Visibility = Windows.Visibility.Visible
            If TxtImage.Text <> "" Then
                If File.Exists(TxtImage.Text) Then
                    Dim bmpImage As New BitmapImage()
                    bmpImage.BeginInit()
                    bmpImage.UriSource = New Uri(TxtImage.Text, UriKind.Absolute)
                    bmpImage.EndInit()
                    ImgMnu.Source = bmpImage
                End If
            End If

        Else
            ListMnu.ContextMenu.Items(1).isenabled = True
            TxtName.Text = MyListMnu.Item(ListMnu.SelectedIndex).Text
            LblType.Content = MyListMnu.Item(ListMnu.SelectedIndex).Type.ToString
            If Frm.ListMnu.Item(ListMnu.SelectedIndex).Type <> uCtrlImgMnu.TypeOfMnu.Zone Then
                StkImage.Visibility = Windows.Visibility.Visible
            Else
                ImgMnu.Source = ConvertArrayToImage(myService.GetByteFromImage(myService.ReturnZoneByID(MyListMnu.Item(ListMnu.SelectedIndex).Parametres(0)).Icon))
                StkImage.Visibility = Windows.Visibility.Hidden
            End If
            TxtImage.Text = MyListMnu.Item(ListMnu.SelectedIndex).Icon
            If Frm.ListMnu.Item(ListMnu.SelectedIndex).Type = uCtrlImgMnu.TypeOfMnu.Internet Then
                LblParam.Content = "Url: "
                TxtParam.Text = MyListMnu.Item(ListMnu.SelectedIndex).Parametres.Item(0)
                StkParam.Visibility = Windows.Visibility.Visible
            Else
                StkParam.Visibility = Windows.Visibility.Hidden
            End If

            StkProperty.Visibility = Windows.Visibility.Visible
            End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Dim dlg As New Microsoft.Win32.OpenFileDialog()
        dlg.Filter = "jpeg (*.jpg) |*.jpg;*.jpeg|(*.png) |*.png|(*.*) |*.*"

        If dlg.ShowDialog() = True Then
            TxtImage.Text = dlg.FileName
            If TxtImage.Text <> "" Then
                If File.Exists(TxtImage.Text) Then
                    Dim bmpImage As New BitmapImage()
                    bmpImage.BeginInit()
                    bmpImage.UriSource = New Uri(TxtImage.Text, UriKind.Absolute)
                    bmpImage.EndInit()
                    ImgMnu.Source = bmpImage
                End If
            End If
        End If
    End Sub

    Private Sub BtnOkMnu_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnOkMnu.Click
        If TxtName.Text = "" Then
            MessageBox.Show("Veuillez saisir le nom du menu!", "Erreur")
            Exit Sub
        End If
        Select Case FlagNewMnu
            Case -1
                If ListMnu.SelectedIndex < 0 Then Exit Sub
                MyListMnu.Item(ListMnu.SelectedIndex).Text = TxtName.Text
                MyListMnu.Item(ListMnu.SelectedIndex).Icon = TxtImage.Text
                If MyListMnu.Item(ListMnu.SelectedIndex).Type = uCtrlImgMnu.TypeOfMnu.Internet Then
                    MyListMnu.Item(ListMnu.SelectedIndex).Parametres.Item(0) = TxtParam.Text
                End If
            Case 1 'New Internet
                If TxtParam.Text = "" Then
                    MessageBox.Show("Veuillez saisir l'url internet!", "Erreur")
                    Exit Sub
                End If
                Dim x As New uCtrlImgMnu
                x.Text = TxtName.Text
                x.Type = uCtrlImgMnu.TypeOfMnu.Internet
                x.Icon = TxtImage.Text
                x.Parametres.Add(TxtParam.Text)
                MyListMnu.Add(x)
                ListMnu.Items.Add(TxtName.Text)
            Case 2 'New Meteo
                Dim x As New uCtrlImgMnu
                x.Text = TxtName.Text
                x.Type = uCtrlImgMnu.TypeOfMnu.Meteo
                x.Icon = TxtImage.Text
                MyListMnu.Add(x)
                ListMnu.Items.Add(TxtName.Text)
            Case 3 'New zone
                Dim x As New uCtrlImgMnu
                x.Text = TxtName.Text
                x.Type = uCtrlImgMnu.TypeOfMnu.Zone
                x.Icon = TxtImage.Text
                x.Parametres.Add(TxtParam.Text)
                MyListMnu.Add(x)
                ListMnu.Items.Add(TxtName.Text)
        End Select
        StkProperty.Visibility = Windows.Visibility.Hidden
    End Sub

    Private Sub BtnCancelMnu_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnCancelMnu.Click
        StkProperty.Visibility = Windows.Visibility.Hidden
    End Sub

    Private Sub MnuAddInternet_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MnuAddInternet.Click
        TxtName.Text = ""
        LblType.Content = uCtrlImgMnu.TypeOfMnu.Internet.ToString
        TxtImage.Text = ""
        LblParam.Content = "Url: "
        TxtParam.Text = ""
        StkImage.Visibility = Windows.Visibility.Visible
        StkParam.Visibility = Windows.Visibility.Visible
        StkProperty.Visibility = Windows.Visibility.Visible
        FlagNewMnu = 1
    End Sub

    Private Sub MnuAddMeteo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MnuAddMeteo.Click
        For i As Integer = 0 To MyListMnu.Count - 1
            If MyListMnu.Item(i).Type = uCtrlImgMnu.TypeOfMnu.Meteo Then
                MessageBox.Show("Vous ne pouvez plus ajouter de menu Meteo car il en existe déjà un ! ")
                Exit Sub
            End If
        Next

        TxtName.Text = ""
        LblType.Content = uCtrlImgMnu.TypeOfMnu.Meteo.ToString
        TxtImage.Text = ""
        StkImage.Visibility = Windows.Visibility.Visible
        StkParam.Visibility = Windows.Visibility.Hidden
        StkProperty.Visibility = Windows.Visibility.Visible
        FlagNewMnu = 2
    End Sub

    'Supprimer un menu
    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItem1.Click
        If ListMnu.SelectedIndex < 0 Then Exit Sub
        MyListMnu.RemoveAt(ListMnu.SelectedIndex)
        ListMnu.Items.RemoveAt(ListMnu.SelectedIndex)
        StkProperty.Visibility = Windows.Visibility.Hidden
    End Sub

    Private Sub MenuZone_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        TxtName.Text = sender.Header
        LblType.Content = uCtrlImgMnu.TypeOfMnu.Zone.ToString
        ImgMnu.Source = ConvertArrayToImage(myService.GetByteFromImage(myService.ReturnZoneByID(sender.uid).Icon))
        TxtParam.Text = sender.uid
        StkImage.Visibility = Windows.Visibility.Hidden
        StkParam.Visibility = Windows.Visibility.Hidden
        StkProperty.Visibility = Windows.Visibility.Visible
        FlagNewMnu = 3
    End Sub

    Private Sub BtnUP_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnUP.Click
        If ListMnu.SelectedIndex < 1 Then Exit Sub
        Dim tmp1 As uCtrlImgMnu
        Dim tmp2 As uCtrlImgMnu

        tmp1 = MyListMnu.Item(ListMnu.SelectedIndex)
        tmp2 = MyListMnu.Item(ListMnu.SelectedIndex - 1)

        MyListMnu.Item(ListMnu.SelectedIndex) = tmp2
        MyListMnu.Item(ListMnu.SelectedIndex - 1) = tmp1

        ListMnu.Items.Clear()
        For i As Integer = 0 To MyListMnu.Count - 1
            ListMnu.Items.Add(MyListMnu.Item(i).Text)
        Next

        If ListMnu.SelectedIndex - 1 = 0 Then BtnUP.IsEnabled = False
    End Sub

    Private Sub BtnDown_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnDown.Click
        If ListMnu.SelectedIndex < 0 Then Exit Sub
        If ListMnu.SelectedIndex + 1 = ListMnu.Items.Count Then Exit Sub
        Dim tmp1 As uCtrlImgMnu
        Dim tmp2 As uCtrlImgMnu

        tmp1 = MyListMnu.Item(ListMnu.SelectedIndex)
        tmp2 = MyListMnu.Item(ListMnu.SelectedIndex + 1)

        MyListMnu.Item(ListMnu.SelectedIndex) = tmp2
        MyListMnu.Item(ListMnu.SelectedIndex + 1) = tmp1

        ListMnu.Items.Clear()
        For i As Integer = 0 To MyListMnu.Count - 1
            ListMnu.Items.Add(MyListMnu.Item(i).Text)
        Next

        If ListMnu.SelectedIndex + 1 = ListMnu.Items.Count Then BtnDown.IsEnabled = False

    End Sub
End Class
