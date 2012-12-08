﻿Imports HoMIDom
Imports HoMIDom.HoMIDom
Imports HoMIDom.HoMIDom.Server
Imports HoMIDom.HoMIDom.Device
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Text
Imports STRGS = Microsoft.VisualBasic.Strings

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
'Imports System.Drawing
Imports System.Data.OleDb

Imports System.IO.Ports

Imports System.Windows.Forms
Imports System.Management
Imports System.Math

Imports WUApiLib.UpdateOperation
Imports WUApiLib.UpdateSessionClass

Imports System.Runtime.InteropServices



'Imports Microsoft.Win32

'- adresse1 =  type d'action : PING, INFO, UPDATE, shutd...
'- adresse 2 = ip (pour le ping), Memoire/CPU/DisqueC/DisqueD/ALL... (pour le systeme),display/install(pour windows update)

' Auteur : Fabien
' Date : 09/09/2012
'-------------------------------------------------------------------------------------
'                                                                      
'-------------------------------------------------------------------------------------

' Driver System
''' <summary>Class Driver System</summary>
''' <remarks>infos system</remarks>
<Serializable()> Public Class Driver_System
    Implements HoMIDom.HoMIDom.IDriver

#Region "Variables génériques"
    '!!!Attention les variables ci-dessous doivent avoir une valeur par défaut obligatoirement
    'aller sur l'adresse http://www.somacon.com/p113.php pour avoir un ID
    Dim _ID As String = "7396FA30-0050-11E2-9BC2-523B6288709B"
    Dim _Nom As String = "System"
    Dim _Enable As String = False
    Dim _Description As String = "Driver System"
    Dim _StartAuto As Boolean = False
    Dim _Protocol As String = "System"
    Dim _IsConnect As Boolean = False
    Dim _IP_TCP As String = "@"
    Dim _Port_TCP As String = "@"
    Dim _IP_UDP As String = "@"
    Dim _Port_UDP As String = "@"
    Dim _Com As String = "@"
    Dim _Refresh As Integer = 0
    Dim _Modele As String = "System"
    Dim _Version As String = My.Application.Info.Version.ToString
    Dim _OsPlatform As String = "3264"
    Dim _Picture As String = ""
    Dim _Server As HoMIDom.HoMIDom.Server
    Dim _DeviceSupport As New ArrayList
    Dim _Device As HoMIDom.HoMIDom.Device
    Dim _Parametres As New ArrayList
    Dim _LabelsDriver As New ArrayList
    Dim _LabelsDevice As New ArrayList
    Dim MyTimer As New Timers.Timer
    Dim _Idsrv As String
    Dim _DeviceCommandPlus As New List(Of HoMIDom.HoMIDom.Device.DeviceCommande)

    'param avancé
    Dim _DEBUG As Boolean = False



#End Region

#Region "Variables Internes"



#End Region

#Region "Propriétés génériques"
    Public WriteOnly Property IdSrv As String Implements HoMIDom.HoMIDom.IDriver.IdSrv
        Set(ByVal value As String)
            _Idsrv = value
        End Set
    End Property

    Public Property COM() As String Implements HoMIDom.HoMIDom.IDriver.COM
        Get
            Return _Com
        End Get
        Set(ByVal value As String)
            _Com = value
        End Set
    End Property
    Public ReadOnly Property Description() As String Implements HoMIDom.HoMIDom.IDriver.Description
        Get
            Return _Description
        End Get
    End Property
    Public ReadOnly Property DeviceSupport() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.DeviceSupport
        Get
            Return _DeviceSupport
        End Get
    End Property
    Public Property Parametres() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.Parametres
        Get
            Return _Parametres
        End Get
        Set(ByVal value As System.Collections.ArrayList)
            _Parametres = value
        End Set
    End Property

    Public Property LabelsDriver() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.LabelsDriver
        Get
            Return _LabelsDriver
        End Get
        Set(ByVal value As System.Collections.ArrayList)
            _LabelsDriver = value
        End Set
    End Property
    Public Property LabelsDevice() As System.Collections.ArrayList Implements HoMIDom.HoMIDom.IDriver.LabelsDevice
        Get
            Return _LabelsDevice
        End Get
        Set(ByVal value As System.Collections.ArrayList)
            _LabelsDevice = value
        End Set
    End Property

    Public Event DriverEvent(ByVal DriveName As String, ByVal TypeEvent As String, ByVal Parametre As Object) Implements HoMIDom.HoMIDom.IDriver.DriverEvent

    Public Property Enable() As Boolean Implements HoMIDom.HoMIDom.IDriver.Enable
        Get
            Return _Enable
        End Get
        Set(ByVal value As Boolean)
            _Enable = value
        End Set
    End Property
    Public ReadOnly Property ID() As String Implements HoMIDom.HoMIDom.IDriver.ID
        Get
            Return _ID
        End Get
    End Property

    Public Property IP_TCP() As String Implements HoMIDom.HoMIDom.IDriver.IP_TCP
        Get
            Return _IP_TCP
        End Get
        Set(ByVal value As String)
            _IP_TCP = value
        End Set
    End Property
    Public Property IP_UDP() As String Implements HoMIDom.HoMIDom.IDriver.IP_UDP
        Get
            Return _IP_UDP
        End Get
        Set(ByVal value As String)
            _IP_UDP = value
        End Set
    End Property
    Public ReadOnly Property IsConnect() As Boolean Implements HoMIDom.HoMIDom.IDriver.IsConnect
        Get
            Return _IsConnect
        End Get
    End Property
    Public Property Modele() As String Implements HoMIDom.HoMIDom.IDriver.Modele
        Get
            Return _Modele
        End Get
        Set(ByVal value As String)
            _Modele = value
        End Set
    End Property
    Public ReadOnly Property Nom() As String Implements HoMIDom.HoMIDom.IDriver.Nom
        Get
            Return _Nom
        End Get
    End Property
    Public Property Picture() As String Implements HoMIDom.HoMIDom.IDriver.Picture
        Get
            Return _Picture
        End Get
        Set(ByVal value As String)
            _Picture = value
        End Set
    End Property
    Public Property Port_TCP() As String Implements HoMIDom.HoMIDom.IDriver.Port_TCP
        Get
            Return _Port_TCP
        End Get
        Set(ByVal value As String)
            _Port_TCP = value
        End Set
    End Property
    Public Property Port_UDP() As String Implements HoMIDom.HoMIDom.IDriver.Port_UDP
        Get
            Return _Port_UDP
        End Get
        Set(ByVal value As String)
            _Port_UDP = value
        End Set
    End Property
    Public ReadOnly Property Protocol() As String Implements HoMIDom.HoMIDom.IDriver.Protocol
        Get
            Return _Protocol
        End Get
    End Property
    Public Property Refresh() As Integer Implements HoMIDom.HoMIDom.IDriver.Refresh
        Get
            Return _Refresh
        End Get
        Set(ByVal value As Integer)
            _Refresh = value
        End Set
    End Property

    Public Property Server() As HoMIDom.HoMIDom.Server Implements HoMIDom.HoMIDom.IDriver.Server
        Get
            Return _Server
        End Get
        Set(ByVal value As HoMIDom.HoMIDom.Server)
            _Server = value
        End Set
    End Property
    Public ReadOnly Property Version() As String Implements HoMIDom.HoMIDom.IDriver.Version
        Get
            Return _Version
        End Get
    End Property

    Public ReadOnly Property OsPlatform() As String Implements HoMIDom.HoMIDom.IDriver.OsPlatform
        Get
            Return _OsPlatform
        End Get
    End Property
    Public Property StartAuto() As Boolean Implements HoMIDom.HoMIDom.IDriver.StartAuto
        Get
            Return _StartAuto
        End Get
        Set(ByVal value As Boolean)
            _StartAuto = value
        End Set
    End Property

#End Region
#Region "Fonctions génériques"
    ''' <summary>
    ''' Retourne la liste des Commandes avancées
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCommandPlus() As List(Of DeviceCommande)
        Return _DeviceCommandPlus
    End Function

    ''' <summary>Execute une commande avancée</summary>
    ''' <param name="MyDevice">Objet représentant le Device </param>
    ''' <param name="Command">Nom de la commande avancée à éxécuter</param>
    ''' <param name="Param">tableau de paramétres</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function ExecuteCommand(ByVal MyDevice As Object, ByVal Command As String, Optional ByVal Param() As Object = Nothing) As Boolean
        Dim retour As Boolean = False
        Try
            If MyDevice IsNot Nothing Then

                ' Pas de commande demandée donc erreur
                If Command = "" Then
                    Return False
                Else
                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, Me.Nom & "ExecuteCommand", Command & " - " & Param(0))
                    Write(MyDevice, Command, Param(0), Param(1))
                    Return True
                End If
            Else
                Return False
            End If



        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & "ExecuteCommand", "exception : " & ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Permet de vérifier si un champ est valide
    ''' </summary>
    ''' <param name="Champ"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function VerifChamp(ByVal Champ As String, ByVal Value As Object) As String Implements HoMIDom.HoMIDom.IDriver.VerifChamp
        Try

            Dim retour As String = "0"
            Select Case UCase(Champ)
                Case "ADRESSE1" ' Action 
                    If Value IsNot Nothing Then
                        'PING, INFO, WUAU
                        If (Value <> "PING" And Value <> "INFO" And Value <> "WUAU") Then
                            retour = "Veuillez saisir un fonctionnalité disponible : PING, INFO ou WUAU."
                        End If
                    End If

                Case "ADRESSE2" 'Parametre 
                    If Value IsNot Nothing Then
                        If Value = "" Or Value = " " Then
                            'PING <IP>,
                            'INFO Memory/CPU/Battery/DisqueC/DisqueD/ALL 
                            'WUAU Display / Update
                            retour = "Veuillez saisir un <parametre> compatible avec votre Action : PING <IP> ou <Hostmane> ou <DomainName>, INFO MEMORY/CPU/BATTERY" & CheckHDD() & " ou WUAU DISPLAY/UPDATE."
                        End If
                    End If
            End Select


            Return retour
        Catch ex As Exception
            Return "Une erreur est apparue lors de la vérification du champ " & Champ & ": " & ex.ToString
        End Try
    End Function

    ''' <summary>Démarrer le du driver</summary>
    ''' <remarks></remarks>
    Public Sub Start() Implements HoMIDom.HoMIDom.IDriver.Start
        Dim retour As String = ""

        Try
            'récupération des paramétres avancés
            _DEBUG = _Parametres.Item(0).Valeur.ToString.ToUpper
            Try

            Catch ex As Exception
                _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " Start", "Erreur dans les paramétres avancés. utilisation des valeur par défaut" & ex.Message)
            End Try

            _IsConnect = True
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, Me.Nom & " Start", "Driver " & Me.Nom & " démarré")


            'My.Computer.FileSystem.Drives.Count
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " Start", ex.Message)
        End Try
    End Sub

    ''' <summary>Arrêter le du driver</summary>
    ''' <remarks></remarks>
    Public Sub [Stop]() Implements HoMIDom.HoMIDom.IDriver.Stop
        Try
            _IsConnect = False
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, Me.Nom & " Stop", "Driver " & Me.Nom & " arrêté")
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " Stop", ex.Message)
        End Try
    End Sub

    ''' <summary>Re-Démarrer le du driver</summary>
    ''' <remarks></remarks>
    Public Sub Restart() Implements HoMIDom.HoMIDom.IDriver.Restart
        [Stop]()
        Start()
    End Sub

    ''' <summary>Intérroger un device</summary>
    ''' <param name="Objet">Objet représetant le device à interroger</param>
    ''' <remarks>pas utilisé</remarks>
    Public Sub Read(ByVal Objet As Object) Implements HoMIDom.HoMIDom.IDriver.Read

        '- adresse1 =  type d'action : PING, INFO, UPDATE...
        '- adresse 2 = ip (pour le ping), INFO Memory/CPU/Battery/DisqueC/DisqueD/ALL ... (pour le systeme),affiche/afficheetinstall(pour windows update)
        'ensuite dans la fonction write suivant les deux champs, tu fais les bonnes actions.
        Dim paramCommand As String = ""
        Try
            If _Enable = False Then
                _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & "Read", "Erreur: Impossible de traiter la commande car le driver n'est pas activé (Enable)")
                Exit Sub
            End If

            If _IsConnect = False Then
                _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & "Read", "Erreur: Impossible de traiter la commande car le driver n'est pas connecté")
                Exit Sub
            End If
            If Left(Objet.adresse2.ToString.ToUpper, 3) = "HDD" Then
                If _DEBUG Then _Server.Log(TypeLog.DEBUG, TypeSource.DRIVER, "Read", "Action: " & Objet.adresse1.ToString & ", Paramètre: " & Objet.adresse2.ToString & ", Composant: " & Objet.Name)
                paramCommand = Right(Objet.adresse2.ToString, (Len(Objet.adresse2.ToString) - 3))
                'Objet.adresse2 = "HDD"
                If _DEBUG Then _Server.Log(TypeLog.DEBUG, TypeSource.DRIVER, "Read", "Action: " & Objet.adresse2.ToString & ", paramCommand: " & paramCommand)

            End If


            Select Case Objet.adresse1.ToString.ToUpper

                Case "PING"
                    If My.Computer.Network.Ping(Objet.adresse2.ToString) Then
                        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Ping " & Objet.adresse2.ToString & " OK")
                    Else
                        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Ping " & Objet.adresse2.ToString & " KO")
                    End If


                Case "INFO"
                    Select Case Objet.adresse2.ToString.ToUpper
                        Case "MEMORY"
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "TotalPhysicalMemory:  " & System.Math.Round((My.Computer.Info.TotalPhysicalMemory) / (1024 * 1024), 2).ToString & " Mo")
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "AvailablePhysicalMemory:  " & System.Math.Round((My.Computer.Info.AvailablePhysicalMemory) / (1024 * 1024), 2).ToString & " Mo")
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "TotalVirtualMemory:  " & System.Math.Round((My.Computer.Info.TotalVirtualMemory) / (1024 * 1024), 2).ToString & " Mo")
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "AvailableVirtualMemory:  " & System.Math.Round((My.Computer.Info.AvailableVirtualMemory) / (1024 * 1024), 2).ToString & " Mo")

                        Case "CPU"
                            ' http://www.a1vbcode.com/snippet-4491.asp
                            _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "System", "CPU information non implémenté")
                        Case "BATTERY"
                            Dim psBattery As PowerStatus = SystemInformation.PowerStatus
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "BatteryChargeStatus : " & psBattery.BatteryChargeStatus)
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "BatteryChargeFullLifetime : " & psBattery.BatteryFullLifetime)
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "BatteryLifePercent : " & psBattery.BatteryLifePercent * 100 & "%")
                            If psBattery.BatteryLifeRemaining <> "-1" Then
                                _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "BatteryLifeRemaining : " & psBattery.BatteryLifeRemaining / 60 & " Min")
                            Else
                                _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "BatteryLifeRemaining : " & "N/A")
                            End If
                            Select Case psBattery.PowerLineStatus
                                Case "0"
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "PowerLineStatus : " & "battery")
                                Case "1"
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "PowerLineStatus : " & "Secteur")
                                Case Else
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "PowerLineStatus : " & "unknow")
                            End Select
                            psBattery = Nothing

                        Case Else
                            If Left(Objet.adresse2.ToString, 3).ToString.ToUpper = "HDD" Then

                                If _DEBUG Then _Server.Log(TypeLog.DEBUG, TypeSource.DRIVER, "READ", "Nbdisk:  " & My.Computer.FileSystem.Drives.Count)
                                Dim drv As New DriveInfo(paramCommand)
                                'Dim allDrives() As DriveInfo = DriveInfo.GetDrives()
                                'Dim d As DriveInfo
                                If drv.IsReady Then
                                    If _DEBUG Then _Server.Log(TypeLog.DEBUG, TypeSource.DRIVER, "READ", "disque selectionné :  " & paramCommand)
                                    ' CDRom, Fixed, Unknown, Network, NoRootDirectory, Ram, Removable, or Unknown. 
                                    If _DEBUG Then _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  File type: ", drv.DriveType)
                                    If _DEBUG Then _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Volume label: ", drv.VolumeLabel)
                                    If _DEBUG Then _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  File system: ", drv.DriveFormat)
                                    '_Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Available space to current user:", (drv.AvailableFreeSpace / (1024 ^ 2) & " Mo"))
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Total available space:          ", CType(drv.TotalFreeSpace / (1024 ^ 2), Integer) & " Mo")
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Total size of drive:            ", CType(drv.TotalSize / (1024 ^ 2), Integer) & " Mo")

                                Else
                                    '_Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Available space to current user: ", "0 bytes")
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Total available space:          0 bytes", "0 bytes")
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Total size of drive:            0 bytes ", "0 bytes")
                                End If
                                drv = Nothing
                            Else
                                _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "System", "information non implémenté")
                            End If
                    End Select

                Case "WUAU"
                    Select Case Objet.adresse2.ToString.ToUpper
                        Case "DISPLAY"
                            CheckForUpdates()
                        Case "INSTALL"
                            InstallUpdates()

                        Case Else
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Parametre non valide")

                    End Select
                Case Else
                        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Action non valide")
            End Select



        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " Read", ex.Message)
        End Try

    End Sub

    ''' <summary>Commander un device</summary>
    ''' <param name="Objet">Objet représetant le device à interroger</param>
    ''' <param name="Command">La commande à passer</param>
    ''' <param name="Parametre1">Action</param>
    ''' <param name="Parametre2">Parametre</param>
    Public Sub Write(ByVal Objet As Object, ByVal Command As String, Optional ByVal Parametre1 As Object = Nothing, Optional ByVal Parametre2 As Object = Nothing) Implements HoMIDom.HoMIDom.IDriver.Write

        '- command =  type d'action : PING, INFO, UPDATE...
        '- Parametre1 = ip (pour le ping), INFO Memory/CPU/Battery/DisqueC/DisqueD/ALL ... (pour le systeme),affiche/afficheetinstall(pour windows update)
        'ensuite dans la fonction write suivant les deux champs, tu fais les bonnes actions.
        Dim paramCommand As String = ""
        Try
            If _Enable = False Then
                _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & "WRITE", "Erreur: Impossible de traiter la commande car le driver n'est pas activé (Enable)")
                Exit Sub
            End If

            If _IsConnect = False Then
                _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & "WRITE", "Erreur: Impossible de traiter la commande car le driver n'est pas connecté à la carte")
                Exit Sub
            End If

            If _DEBUG Then _Server.Log(TypeLog.DEBUG, TypeSource.DRIVER, "WRITE", "Commande: " & Command & ", Action: " & Parametre1 & ", Composant: " & Objet.Name)

            If Left(Parametre1.ToString, 3) = "HDD" Then
                If _DEBUG Then _Server.Log(TypeLog.DEBUG, TypeSource.DRIVER, "WRITE", "Commande: " & Command & ", Action: " & Parametre1 & ", Composant: " & Objet.Name)
                paramCommand = Right(Parametre1, (Len(Parametre1) - 3))

                If _DEBUG Then _Server.Log(TypeLog.DEBUG, TypeSource.DRIVER, "WRITE", "Commande: " & Command & ", paramCommand: " & paramCommand)

            End If

            Select Case UCase(Command)

                Case "PING"
                    If My.Computer.Network.Ping(Parametre1.ToString.ToUpper) Then
                        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Ping " & Parametre1.ToString & " OK")
                    Else
                        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Ping " & Parametre1.ToString & " KO")
                    End If
                Case "INFO"
                    Select Case Parametre1.ToString.ToUpper
                        Case "MEMORY"
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "TotalPhysicalMemory:  " & System.Math.Round((My.Computer.Info.TotalPhysicalMemory) / (1024 * 1024), 2).ToString & " Mo")
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "AvailablePhysicalMemory:  " & System.Math.Round((My.Computer.Info.AvailablePhysicalMemory) / (1024 * 1024), 2).ToString & " Mo")
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "TotalVirtualMemory:  " & System.Math.Round((My.Computer.Info.TotalVirtualMemory) / (1024 * 1024), 2).ToString & " Mo")
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "AvailableVirtualMemory:  " & System.Math.Round((My.Computer.Info.AvailableVirtualMemory) / (1024 * 1024), 2).ToString & " Mo")

                        Case "CPU"
                            _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "System", "CPU information non implémenté")

                        Case "BATTERY"
                            Dim psBattery As PowerStatus = SystemInformation.PowerStatus
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "BatteryChargeStatus : " & psBattery.BatteryChargeStatus)
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "BatteryChargeFullLifetime : " & psBattery.BatteryFullLifetime)
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "BatteryLifePercent : " & psBattery.BatteryLifePercent * 100 & "%")
                            If psBattery.BatteryLifeRemaining <> "-1" Then
                                _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "BatteryLifeRemaining : " & psBattery.BatteryLifeRemaining / 60 & " Min")
                            Else
                                _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "BatteryLifeRemaining : " & "N/A")
                            End If
                            Select Case psBattery.PowerLineStatus
                                Case "0"
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "PowerLineStatus : " & "battery")
                                Case "1"
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "PowerLineStatus : " & "Secteur")
                                Case Else
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "PowerLineStatus : " & "unknow")
                            End Select
                            psBattery = Nothing

                      Case Else

                            If Left(Parametre1.ToString, 3).ToString.ToUpper = "HDD" Then

                                If _DEBUG Then _Server.Log(TypeLog.DEBUG, TypeSource.DRIVER, "READ", "Nbdisk:  " & My.Computer.FileSystem.Drives.Count)
                                Dim drv As New DriveInfo(paramCommand)
                                'Dim allDrives() As DriveInfo = DriveInfo.GetDrives()
                                'Dim d As DriveInfo
                                If drv.IsReady Then
                                    If _DEBUG Then _Server.Log(TypeLog.DEBUG, TypeSource.DRIVER, "READ", "disque selectionné :  " & paramCommand)
                                    ' CDRom, Fixed, Unknown, Network, NoRootDirectory, Ram, Removable, or Unknown. 
                                    If _DEBUG Then _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  File type: ", drv.DriveType)
                                    If _DEBUG Then _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Volume label: ", drv.VolumeLabel)
                                    If _DEBUG Then _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  File system: ", drv.DriveFormat)
                                    ' _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Available space to current user:", (drv.AvailableFreeSpace / (1024 ^ 2) & " Mo"))
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Total available space:          ", CType(drv.TotalFreeSpace / (1024 ^ 2), Integer) & " Mo")
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Total size of drive:            ", CType(drv.TotalSize / (1024 ^ 2), Integer) & " Mo")

                                Else
                                    ' _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Available space to current user: ", "0 bytes")
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Total available space:          0 bytes", "0 bytes")
                                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  Total size of drive:            0 bytes ", "0 bytes")
                                End If
                                drv = Nothing
                            Else
                                _Server.Log(Server.TypeLog.INFO, Server.TypeSource.DRIVER, "System", "information non implémenté")
                            End If
                         
                    End Select

                Case "WUAU"
                    Select Case Parametre1.ToString.ToUpper
                        Case "DISPLAY"
                            CheckForUpdates()
                        Case "INSTALL"
                            InstallUpdates()
                        Case Else
                            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Parametre non valide")
                    End Select
                Case Else
                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Action non valide")
            End Select



        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " Write", ex.Message)
        End Try

    End Sub

    ''' <summary>Fonction lancée lors de la suppression d'un device</summary>
    ''' <param name="DeviceId">Objet représetant le device à interroger</param>
    ''' <remarks></remarks>
    Public Sub DeleteDevice(ByVal DeviceId As String) Implements HoMIDom.HoMIDom.IDriver.DeleteDevice
        Try

        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "System DeleteDevice", ex.Message)
        End Try
    End Sub

    ''' <summary>Fonction lancée lors de l'ajout d'un device</summary>
    ''' <param name="DeviceId">Objet représetant le device à interroger</param>
    ''' <remarks></remarks>
    Public Sub NewDevice(ByVal DeviceId As String) Implements HoMIDom.HoMIDom.IDriver.NewDevice
        Try

        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "System NewDevice", ex.Message)
        End Try
    End Sub

    ''' <summary>ajout des commandes avancées pour les devices</summary>
    ''' <param name="nom">Nom de la commande avancée</param>
    ''' <param name="description">Description qui sera affichée dans l'admin</param>
    ''' <param name="nbparam">Nombre de parametres attendus</param>
    ''' <remarks></remarks>
    Private Sub Add_DeviceCommande(ByVal Nom As String, ByVal Description As String, ByVal NbParam As Integer)
        Try
            Dim x As New DeviceCommande
            x.NameCommand = Nom
            x.DescriptionCommand = Description
            x.CountParam = NbParam
            _DeviceCommandPlus.Add(x)
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " add_devicecommande", "Exception : " & ex.Message)
        End Try
    End Sub

    ''' <summary>ajout Libellé pour le Driver</summary>
    ''' <param name="nom">Nom du champ : HELP</param>
    ''' <param name="labelchamp">Nom à afficher : Aide</param>
    ''' <param name="tooltip">Tooltip à afficher au dessus du champs dans l'admin</param>
    ''' <remarks></remarks>
    Private Sub Add_LibelleDriver(ByVal Nom As String, ByVal Labelchamp As String, ByVal Tooltip As String, Optional ByVal Parametre As String = "")
        Try
            Dim y0 As New HoMIDom.HoMIDom.Driver.cLabels
            y0.LabelChamp = Labelchamp
            y0.NomChamp = UCase(Nom)
            y0.Tooltip = Tooltip
            y0.Parametre = Parametre
            _LabelsDriver.Add(y0)
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " add_devicecommande", "Exception : " & ex.Message)
        End Try
    End Sub

    ''' <summary>Ajout Libellé pour les Devices</summary>
    ''' <param name="nom">Nom du champ : HELP</param>
    ''' <param name="labelchamp">Nom à afficher : Aide, si = "@" alors le champ ne sera pas affiché</param>
    ''' <param name="tooltip">Tooltip à afficher au dessus du champs dans l'admin</param>
    ''' <remarks></remarks>
    Private Sub Add_LibelleDevice(ByVal Nom As String, ByVal Labelchamp As String, ByVal Tooltip As String, Optional ByVal Parametre As String = "")
        Try
            Dim ld0 As New HoMIDom.HoMIDom.Driver.cLabels
            ld0.LabelChamp = Labelchamp
            ld0.NomChamp = UCase(Nom)
            ld0.Tooltip = Tooltip
            ld0.Parametre = Parametre
            _LabelsDevice.Add(ld0)
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " add_devicecommande", "Exception : " & ex.Message)
        End Try
    End Sub

    ''' <summary>ajout de parametre avancés</summary>
    ''' <param name="nom">Nom du parametre (sans espace)</param>
    ''' <param name="description">Description du parametre</param>
    ''' <param name="valeur">Sa valeur</param>
    ''' <remarks></remarks>
    Private Sub Add_ParamAvance(ByVal nom As String, ByVal description As String, ByVal valeur As Object)
        Try
            Dim x As New HoMIDom.HoMIDom.Driver.Parametre
            x.Nom = nom
            x.Description = description
            x.Valeur = valeur
            _Parametres.Add(x)
        Catch ex As Exception
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, Me.Nom & " add_devicecommande", "Exception : " & ex.Message)
        End Try
    End Sub

    ''' <summary>Creation d'un objet de type</summary>
    ''' <remarks></remarks>
    Public Sub New()
        Try
            _Version = Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString

            'Liste des devices compatibles
            _DeviceSupport.Add(ListeDevices.GENERIQUESTRING)
            _DeviceSupport.Add(ListeDevices.GENERIQUEVALUE)

            'ajout des commandes avancées pour les devices
            'add_devicecommande("COMMANDE", "DESCRIPTION", nbparametre)
            Add_DeviceCommande("PING", "Ping <IP>,<Hostname>,<DomainName>", 1)
            Add_DeviceCommande("INFO", "MEMORY/CPU/BATTERY/HDD", 1)
            Add_DeviceCommande("WUAU", "DISPLAY/INSTALL", 1)


            'ajout des commandes avancées pour les devices
            Add_ParamAvance("Debug", "Activer le Debug complet (True/False)", True)

            'Libellé Driver
            Add_LibelleDriver("HELP", "Aide...", "Pas d'aide actuellement...")

            'Libellé Device

            '- adresse1=  type d'action : PING, INFO, UPDATE...
            Add_LibelleDevice("ADRESSE1", "Type d'action", "PING, INFO, WUAU")
            '- adresse2 = ip (pour le ping), Memoire/CPU/DisqueC/DisqueD/ALL... (pour le systeme),affiche/afficheetinstall(pour windows update)
            Add_LibelleDevice("ADRESSE2", "Paramètre", "Ping: <IP>,<Hostname>,<DomainName>,INFO :MEMORY/CPU/BATTERY" & CheckHDD() & ",WUA : display/install")

            Add_LibelleDevice("SOLO", "@", "")
            Add_LibelleDevice("MODELE", "@", "")

            Add_LibelleDevice("REFRESH", "Refresh", "")
            Add_LibelleDevice("LASTCHANGEDUREE", "@", "")
            ' Add_LibelleDevice("CommTimeout", "LastChange Durée", "")

        Catch ex As Exception
            ' WriteLog("ERR: New Exception : " & ex.Message)
            _Server.Log(TypeLog.ERREUR, TypeSource.DRIVER, "System New", ex.Message)
        End Try
    End Sub

    ''' <summary>Si refresh >0 gestion du timer</summary>
    ''' <remarks>PAS UTILISE CAR IL FAUT LANCER UN TIMER QUI LANCE/ARRETE CETTE FONCTION dans Start/Stop</remarks>
    Private Sub TimerTick()

    End Sub

#End Region

#Region "Fonctions internes"
    'Insérer ci-dessous les fonctions propres au driver et nom communes (ex: start)

    Public Sub CheckForUpdates()
 ' Info de l'agent
        Dim oAgentInfo = CreateObject("Microsoft.Update.AgentInfo")
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "%system32%\wuapi.dll version: " & oAgentInfo.GetInfo("ProductVersionString"))
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "WUA version : " & oAgentInfo.GetInfo("ApiMajorVersion") & "." & oAgentInfo.GetInfo("ApiMinorVersion"))


        '  http://msdn.microsoft.com/en-gb/library/windows/desktop/aa387102(v=vs.85).aspx
        ' This function checks for Windows Updates and returns a integer value containing the number of updates   ' found back to the calling routine

        Dim UpdateSession = New WUApiLib.UpdateSession
        'Dim updateSession = CreateObject("Microsoft.Update.Session")
        UpdateSession.ClientApplicationID = "Homidom Automate Sytem Update"

        Dim updateSearcher = UpdateSession.CreateUpdateSearcher()
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Searching for updates...")

        Dim searchResult = updateSearcher.Search("IsInstalled=0 and Type='Software'")
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "List of applicable items on the machine:")
        For I = 0 To searchResult.Updates.Count - 1
            Dim update = searchResult.Updates.Item(I)
            'WScript.Echo(I + 1 & "> " & update.Title)
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", update.Title)
        Next
        If searchResult.Updates.Count = 0 Then
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "There are no applicable updates.")
            Exit Sub
        End If


    End Sub
    Public Sub InstallUpdates()

        ' Info de l'agent
        Dim oAgentInfo = CreateObject("Microsoft.Update.AgentInfo")
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "%system32%/wuapi.dll version: " & oAgentInfo.GetInfo("ProductVersionString"))
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "WUA version : " & oAgentInfo.GetInfo("ApiMajorVersion") & "." & oAgentInfo.GetInfo("ApiMinorVersion"))


        '  http://msdn.microsoft.com/en-gb/library/windows/desktop/aa387102(v=vs.85).aspx
        ' This function checks for Windows Updates and returns a integer value containing the number of updates   ' found back to the calling routine

        Dim CheckForUpdates = 0
        Dim UpdateSession = New WUApiLib.UpdateSession
        'Dim updateSession = CreateObject("Microsoft.Update.Session")
        UpdateSession.ClientApplicationID = "Homidom automate Sytem update"

        Dim updateSearcher = UpdateSession.CreateUpdateSearcher()
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Searching for updates...")

        Dim searchResult = updateSearcher.Search("IsInstalled=0 and Type='Software'")
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "List of applicable items on the machine:")
        For I = 0 To searchResult.Updates.Count - 1
            Dim update = searchResult.Updates.Item(I)
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", update.Title)
        Next
        If searchResult.Updates.Count = 0 Then
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "There are no applicable updates.")
            Exit Sub
        End If
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Creating collection of updates to download:")

        'Dim updatesToDownload = CreateObject("Microsoft.Update.UpdateColl")
        Dim updatesToDownload = New WUApiLib.UpdateCollection

        For I = 0 To searchResult.Updates.Count - 1
            Dim update = searchResult.Updates.Item(I)
            Dim addThisUpdate = False
            If update.InstallationBehavior.CanRequestUserInput = True Then
                _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", I + 1 & ">  skipping: " & update.Title & " because it requires user input")
            Else
                If update.EulaAccepted = False Then
                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "> note: " & update.Title & " has a license agreement that must be accepted:")
                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", update.EulaText)
                    _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Do you accept this license agreement? (Y/N) ....Y autoupdate Accpete pour vous")
                    ' strInput = WScript.StdIn.Readline
                    '  WScript.Echo()
                    'If (strInput = "Y" Or strInput = "y") Then
                    update.AcceptEula()
                    addThisUpdate = True
                    'Else
                    ' WScript.Echo(I + 1 & "> skipping: " & update.Title & _
                    ' " because the license agreement was declined")
                    'End If
                Else
                    addThisUpdate = True
                End If
            End If
            If addThisUpdate = True Then
                _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", I + 1 & "> adding: " & update.Title)
                updatesToDownload.Add(update)
            End If
        Next

        If updatesToDownload.Count = 0 Then
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "All applicable updates were skipped.")
            Exit Sub
        End If

        Dim downloader As New WUApiLib.UpdateDownloader

        For a As Integer = 0 To updatesToDownload.Count - 1

            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Downloading updates..." & searchResult.Updates.Item(a).Title)

            'If UpdatesTitle.ToUpper = patch.Title.ToUpper Then
            downloader = UpdateSession.CreateUpdateDownloader()
            downloader.Updates = updatesToDownload
            downloader.Download()

            downloader = Nothing
            updatesToDownload = Nothing

            ' End If
        Next
       
        'Dim updatesToDownload = New WUApiLib.UpdateCollection

        Dim updatesToInstall = CreateObject("Microsoft.Update.UpdateColl")
        'Dim updatesToInstall = New WUApiLib.UpdateCollection ' a remplacer plus tard...a voir
        Dim rebootMayBeRequired = False

        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Successfully downloaded updates:")

        For i = 0 To searchResult.Updates.Count - 1
            Dim update = searchResult.Updates.Item(i)
            If update.IsDownloaded = True Then
                _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", i + 1 & "> " & update.Title)
                updatesToInstall.Add(update)
                If update.InstallationBehavior.RebootBehavior > 0 Then
                    rebootMayBeRequired = True
                End If
            End If
        Next

        If updatesToInstall.Count = 0 Then
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "No updates were successfully downloaded.")
            Exit Sub
        End If

        If rebootMayBeRequired = True Then
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "These updates may require a reboot.")
        End If

        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Would you like to install updates now? (Y/N)....update seront validés")
        'strInput = WScript.StdIn.Readline
        ' If (strInput = "Y" Or strInput = "y") Then
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Installing updates...")
        Dim installer = UpdateSession.CreateUpdateInstaller()
        installer.Updates = updatesToInstall
        Dim installationResult = installer.Install()

        'Output results of install
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Installation Result: " & installationResult.ResultCode)
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Reboot Required: " & installationResult.RebootRequired & vbCrLf)
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Listing of updates installed " & "and individual installation results:")

        For i = 0 To updatesToInstall.Count - 1
            _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", i + 1 & "> " & updatesToInstall.Item(i).Title & ": " & installationResult.GetUpdateResult(i).ResultCode)
        Next
       

    End Sub

    Public Sub systemdown()
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "System will be shutdown ")
        System.Diagnostics.Process.Start("ShutDown", "/s")
    End Sub

    Public Sub systemreboot()
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "System will be shutdown ")
        System.Diagnostics.Process.Start("ShutDown", "/r")


        '  C:\>shutdown
        'Usage: shutdown [-i | -l | -s | -r | -a] [-f] [-m \\computername] [-t xx] [-c "comment("] [-d up:xx:yy]")
        '
        'No args                 Display this message (same as -?)
        ' -i                      Display GUI interface, must be the first option
        ' -l                      Log off (cannot be used with -m option)
        '   -s                      Shutdown the computer
        '     -r                      Shutdown and restart the computer
        '     -a                      Abort a system shutdown
        '     -m \\computername Remote computer to shutdown/restart/abort
        '     -t xx                   Set timeout for shutdown to xx seconds
        '     -c "comment"            Shutdown comment (maximum of 127 characters)
        '    -f                      Forces running applications to close without warning()
        '   -d [u][p]:xx:yy         The reason code for the shutdown
        '                        u is the user code
        '                       p is a planned shutdown code
        '                       xx is the major reason code (positive integer less than 256)
        '                       yy is the minor reason code (positive integer less than 65536)
    End Sub

    Public Sub myprocess()
        ' http://msdn.microsoft.com/en-us/library/system.diagnostics.process.workingset64.aspx
        ' http://msdn.microsoft.com/en-us/library/system.diagnostics.process.virtualmemorysize64.aspx

        Dim myprocess As System.Diagnostics.Process = System.Diagnostics.Process.GetCurrentProcess()

        Dim totalBytesOfMemoryUsed As Long = myprocess.WorkingSet64
        Dim totalBytesOfvirtualMemoryUsed As Long = myprocess.VirtualMemorySize64


        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Pid : " & myprocess.Id.ToString & " physical memory usage ( WorkingSet64) : " & ResizeKb(totalBytesOfMemoryUsed))
        _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "System", "Pid : " & myprocess.Id.ToString & " virtual memory usage ( VirtualMemorySize64) : " & ResizeKb(totalBytesOfvirtualMemoryUsed))

        myprocess = Nothing
        totalBytesOfMemoryUsed = Nothing

    End Sub



    Private Function ResizeKb(ByVal b As Double) As String

        ResizeKb = ""
        If b > 0 Then
            Dim bSize(8) As String, i As Integer
            bSize(0) = "Bytes"
            bSize(1) = "KB" 'Kilobytes
            bSize(2) = "MB" 'Megabytes
            bSize(3) = "GB" 'Gigabytes
            bSize(4) = "TB" 'Terabytes
            bSize(5) = "PB" 'Petabytes
            bSize(6) = "EB" 'Exabytes
            bSize(7) = "ZB" 'Zettabytes
            bSize(8) = "YB" 'Yottabytes
            For i = UBound(bSize) To 0 Step -1
                If b >= (1024 ^ i) Then
                    ResizeKb = ThreeNonZeroDigits(b / (1024 ^ i)) & " " & bSize(i)
                    Exit For
                End If
            Next
        Else
            ResizeKb = "0 Bytes"
        End If

    End Function

    Private Function ThreeNonZeroDigits(ByVal value As Double) As Double

        If value >= 100 Then
            ThreeNonZeroDigits = FormatNumber(value)
        ElseIf value >= 10 Then
            ThreeNonZeroDigits = FormatNumber(value, 1)
        Else
            ThreeNonZeroDigits = FormatNumber(value, 2)
        End If
    End Function

    'recupere la liste des volumes diponible ( affichage dans tooltip )
    Private Function CheckHDD() As String
        CheckHDD = ""
        Dim d As DriveInfo
        Dim infohdd As String = ""
        For hddcpt As Integer = 0 To (My.Computer.FileSystem.Drives.Count - 1)
            ' CDRom, Fixed, Unknown, Network, NoRootDirectory, Ram, Removable, or Unknown. 
            ' _Server.Log(TypeLog.INFO, TypeSource.DRIVER, "  File type: {0}", d.DriveType)
            'CheckHDD(hddcpt)[0] = 
            '            CheckHDD = CheckHDD & "/HDD" & hddcpt & "(" & My.Computer.FileSystem.Drives(hddcpt).RootDirectory.ToString & ")"
            CheckHDD = CheckHDD & "/HDD" & My.Computer.FileSystem.Drives(hddcpt).RootDirectory.ToString

        Next
        d = Nothing

    End Function

#End Region


End Class