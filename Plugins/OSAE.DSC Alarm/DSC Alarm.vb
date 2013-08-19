Imports System.Timers
Imports System.IO.Ports
Imports System.Threading.Thread
Imports OSAE
Imports System.Net.Sockets
Imports System.Text

Public Class DSCAlarm
    Inherits OSAEPluginBase
    Const IPPort As Integer = 4025
    Shared logging As Logging = logging.GetLogger("DSC Alarm")
    Shared pName As String
    Shared COMPort As String
    Shared BaudRate As Integer
    Shared ControllerPort As SerialPort
    Shared UpdateTimer As Timer

    Private Message As String
    Dim ReceivedMessage As String
    Dim ReceiveTime As DateTime
    Dim SendString As String
    Shared Data As String

    'Shared PartitionStatus(), PartitionStatusLast() As String
    Shared ZoneArray(32) As String
    Shared CommDown As Boolean
    Shared LoginNeeded As Boolean
    Shared Password As String
    Shared client_socket As Socket
    Shared EthernetCom As Boolean
    Shared IPAddress As String
    Shared ShutdownNow As Boolean
    Shared MaxPartition As Integer = 1
    'Shared PartitionName() As String
    Public Class DSCPartition
        Public Name As String
        Public State, Status As String
    End Class
    Public Shared Partition(8) As DSCPartition

    Public Class DSCZone
        Public Name As String
        Public State As String
    End Class

    Public Shared Zone(64) As DSCZone


    Enum Command
        Poll = 0
        StatusRequest = 1
        LabelsRequest = 2
        NetworkLogin = 5
        DumpZoneTimers = 8
        CommandOutputControl = 10 'SetTimeDate
        CommandOutputcontrol2 = 20
        PartitionArmControlAway = 30
        PartitionArmControlStay = 31
        PartitionArmControlNoEntryDelay = 32
        PartitionArmControl = 33
        PartitionDisarmControl = 40
        TimeSampControl = 55
        TimeDateBroadcastControl = 56
        TemperatureBroadcastControl = 57
        VirtualKeypadControl = 58
        TriggerPanicAlarm = 60
        KeyPressed = 70
        KeysString = 71
        EnterUserCodeProgramming = 72
        EnterUserProgramming = 73
        KeepAlive = 74
        BaudRateChange = 80
        GetTemperatureSetPoint = 95
        TemperatureChange = 96
        SaveTemperatureSetting = 97

        CodeSend = 200

        CommandAcknowledge = 500
        CommandError = 501
        SystemError = 502
        LoginInteraction = 505
        KeypadLEDState = 510
        KeypadLEDFlashState = 511
        TimeDateBroadcast = 550
        RingDetected = 560
        IndoorTemperatureBroadcast = 561
        OutdoorTemperatureBroadcast = 562
        ThermostatSetPoints = 563
        BroadcastLabels = 570
        BaudRateSet = 580

        ZoneAlarm = 601
        ZoneAlarmRestore = 602
        ZoneTamper = 603
        ZoneTamperRestore = 604
        ZoneFault = 605
        ZoneFaultRestore = 606
        ZoneOpen = 609
        ZoneRestored = 610
        EnvisalinkZoneTimerDump = 615
        DuressAlarm = 620
        FireKeyAlarm = 621
        FireKeyRestored = 622
        AuxiliaryKeyAlarm = 623
        AuxiliaryKeyRestored = 624
        PanicKeyAlarm = 625
        PanicKeyRestored = 626
        AuxiliaryInputAlarm = 631
        AuxiliaryInputAlarmRestored = 632
        PartitionReady = 650
        PartitionNotReady = 651
        PartitionArmed = 652
        PartitionReadyForceArming = 653
        PartitionInAlarm = 654
        PartitionDisarmed = 655
        ExitDelayInProgress = 656
        EntryDelayInProgress = 657
        KeypadLockout = 658
        KeypadBlanking = 659
        CommandOutputInProgress = 660
        ChimeEnabled = 663
        ChimeDisabled = 664
        InvalidAccessCode = 670
        FunctionNotAvailable = 671
        FailedToArm = 672
        PartitionBusy = 673
        SystemArmingInProgress = 674
        SystemInInstallerMode = 680

        UserClosing = 700
        SpecialClosing = 701
        PartialClosing = 702
        UserOpening = 750
        SpecialOpening = 751

        PanelBatteryTrouble = 800
        PanelBatteryTroubleRestore = 801
        PanelACTrouble = 802
        PanelACRestore = 803
        SystemBellTrouble = 806
        SystemBellTroubleRestore = 807
        TLMLine1Trouble = 810
        TLMLine1TroubleRestore = 811
        TLMLine2Trouble = 812
        TLMLine2TroubleRestore = 813
        FTCTrouble = 814
        BufferNearFull = 816
        GeneralDeviceLowBattery = 821
        GeneralDeviceLowBatteryRestore = 822
        WirelessKeyLowBatteryTrouble = 825
        WirelessKeyLowBatteryTroubleRestore = 826
        HandheldKeypadLowBatteryTrouble = 827
        HandheldKeypadLowBatteryTroubleRestore = 828
        GeneralSystemTamper = 829
        GeneralSystemTamperRestore = 830
        HomeAutomationTrouble = 831
        HomeAutomationTroubleRestore = 832
        TroubleLEDOn = 840
        TroubleLEDOff = 841
        FireTroubleAlarm = 842
        FireTroubleAlarmRestored = 843
        VerboseTroubleStatus = 849
        KeybusFault = 896
        KeybusFaultRestore = 897

        CodeRequired = 900
        LCDUpdate = 901
        LCDCursor = 902
        LEDStatus = 903
        BeepStatus = 904
        ToneStatus = 905
        BuzzerStatus = 906
        DoorChimeStatus = 907
        SoftwareVersion = 908
        CommandOutputPressed = 912
        MasterCodeRequired = 921
        InstallersCodeRequired = 922
    End Enum

    Public Overrides Sub RunInterface(ByVal pluginName As String)
        Dim ZoneString As String
        Dim ParseResult As Boolean
        Dim PartitionCollection As OSAEObjectCollection
        Dim UpdateTime As Integer
        Dim PartitionNum As Integer
        Try
            logging.AddToLog("Initializing plugin: " & pluginName, True)
            pName = pluginName

            If Boolean.TryParse(OSAEObjectPropertyManager.GetObjectPropertyValue(pName, "Ethernet").Value, ParseResult) Then
                EthernetCom = ParseResult
            Else
                EthernetCom = False
            End If
            Password = Left(OSAEObjectPropertyManager.GetObjectPropertyValue(pName, "Password").Value, 6)

            'Verify Partition 1 object exists
            If OSAEObjectManager.GetObjectsByType("DSC ALARM PARTITION").Count = 0 Then
                logging.AddToLog("Creating DSC Alarm Partion 1", True)
                OSAEObjectManager.ObjectAdd("DSC Alarm Partition", "DSC Alarm Partion", "DSC ALARM PARTITION", "1", "", True)
            End If

            For Each OSAEObject In OSAEObjectManager.GetObjectsByType("DSC ALARM PARTITION")
                PartitionNum = CInt(OSAEObject.Address)
                logging.AddToLog("Initializing Partion: " & OSAEObject.Name, True)
                If 1 <= PartitionNum <= 8 Then
                    Partition(PartitionNum) = New DSCPartition
                    Partition(PartitionNum).Name = OSAEObject.Name
                    Partition(PartitionNum).State = OSAEObject.State.Value
                    If PartitionNum > MaxPartition Then
                        MaxPartition = PartitionNum
                    End If
                Else
                    logging.AddToLog("Partition " & OSAEObject.Name & " has invalid address " & OSAEObject.Address, True)
                End If
            Next

            For Each OSAEObject In OSAEObjectManager.GetObjectsByType("DSC ALARM ZONE")
                Zone(Integer.Parse(OSAEObject.Address)).Name = OSAEObject.Name
                Zone(Integer.Parse(OSAEObject.Address)).State = OSAEObject.State.Value
            Next

            logging.AddToLog("Finished loading settings", True)

            If EthernetCom Then
                IPAddress = OSAEObjectManager.GetObjectByName(pName).Address
                InitSocket()

            Else
                COMPort = "COM" + OSAEObjectPropertyManager.GetObjectPropertyValue(pluginName, "Port").Value
                BaudRate = CInt(OSAEObjectPropertyManager.GetObjectPropertyValue(pluginName, "Baud").Value)
                ControllerPort = New SerialPort(COMPort)
                logging.AddToLog("Port is set to " & COMPort & " and baud to " & BaudRate.ToString, True)
                ControllerPort.BaudRate = BaudRate
                ControllerPort.Parity = Parity.None
                ControllerPort.DataBits = 8
                ControllerPort.StopBits = 1
                ControllerPort.NewLine = vbCr & vbLf
                ControllerPort.ReadTimeout = 1
                ControllerPort.Open()
                AddHandler ControllerPort.DataReceived, New SerialDataReceivedEventHandler(AddressOf UpdateReceived)
            End If

            SendString = CommandToString(Command.StatusRequest)
            SendCommand(SendString)

            'Testing purposes only
            'ParseMessage("65211FF") 'Partition Armed
            'ParseMessage("656100") 'Partition Exit delay
            'ParseMessage("61000229") 'Zone 2 Closed
            'ParseMessage("60900228") 'Zone 2 Open

            UpdateTime = CInt(OSAEObjectPropertyManager.GetObjectPropertyValue(pluginName, "UpdateTime").Value)
            UpdateTimer = New Timers.Timer(UpdateTime * 1000)
            AddHandler UpdateTimer.Elapsed, New ElapsedEventHandler(AddressOf TimerHandler)
            UpdateTimer.Enabled = True

        Catch ex As Exception
            logging.AddToLog("Error setting up plugin: " & ex.Message, True)
        End Try
    End Sub

    Public Overrides Sub ProcessCommand(method As OSAEMethod)
        Try

        Catch ex As Exception
            logging.AddToLog("Error Processing Command " & ex.Message, True)
        End Try

    End Sub

    Public Overrides Sub Shutdown()

        ShutdownNow = True

        Try
            logging.AddToLog("Shutting down plugin", True)
            If EthernetCom Then

                If client_socket.Connected Then
                    SendCommand(CommandToString(Command.KeepAlive) & "1")
                    Sleep(750)

                    client_socket.Shutdown(SocketShutdown.Both)
                    client_socket.Close()
                End If

            Else
                If ControllerPort.IsOpen Then
                    ControllerPort.Close()
                End If
            End If

            UpdateTimer.Dispose()

            logging.AddToLog("Finished shutting down plugin", True)

        Catch ex As Exception
            logging.AddToLog("Error shutting down: " & ex.Message, True)
        End Try
    End Sub

    Protected Sub UpdateReceived(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)
        Try
            logging.AddToLog("Running serial port event handler", False)
            ProcessReceived()
        Catch ex As Exception
            logging.AddToLog("Error in UpdateReceived " & ex.Message, True)
        End Try
    End Sub

    Protected Sub TimerHandler(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        Try
            logging.AddToLog("Timer Update", False)
            If CommDown Then
                InitSocket()
            End If
            SendString = CommandToString(Command.StatusRequest)
            SendCommand(SendString)
        Catch ex As Exception
            logging.AddToLog("Error in TimerHandler " & ex.Message, True)
        End Try
    End Sub

    Protected Shared Sub SendCommand(message As String)

        message &= CalcChecksum(message) & vbCrLf
        If EthernetCom Then
            Try
                client_socket.Send(Encoding.ASCII.GetBytes(message))

            Catch ex As Exception
                logging.AddToLog("Error in socket sending command " & ex.Message, True)
                CommDown = True
            End Try
        Else
            Try
                ControllerPort.Write(message)
            Catch ex As Exception
                logging.AddToLog("Error in com port sending command " & ex.Message, True)
            End Try

        End If
        logging.AddToLog("Sending message:" & message.TrimEnd & "| Message Lenght= " & message.Length.ToString, False)
    End Sub
    Sub InitSocket()
        Try
            If Not client_socket Is Nothing Then
                client_socket.Dispose()
            End If
            client_socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            client_socket.Connect(IPAddress, IPPort)

            logging.AddToLog("Connection made", False)

            SetupReceive(client_socket)
            logging.AddToLog("Receive routine complete", False)
            CommDown = False
            Threading.Thread.Sleep(500)
            If LoginNeeded Then
                SendCommand(CommandToString(Command.NetworkLogin) & Password)
                LoginNeeded = False
            End If


        Catch ex As Exception
            logging.AddToLog("Error init socket " & ex.Message, True)
            CommDown = True
        End Try
    End Sub

    Protected Sub ProcessReceived()
        Dim Message As String
        Dim MyIndex As Integer
        Try
            Message = ControllerPort.ReadExisting()
            logging.AddToLog("Received: " & Message.TrimEnd, False)

            If Message.Length > 0 Then
                ReceivedMessage += Message
                While True
                    MyIndex = ReceivedMessage.IndexOf(vbCr & vbLf)
                    If MyIndex > -1 Then
                        ReceiveTime = Now()
                        ParseMessage(Left(ReceivedMessage, MyIndex))
                        ReceivedMessage = Mid(ReceivedMessage, MyIndex + 3)
                    Else
                        Exit While
                    End If
                End While
            End If

        Catch ex As Exception
            logging.AddToLog("Error receiving on com port:" & ex.Message, True)
        End Try
    End Sub

    Protected Shared Sub ParseMessage(ByVal message As String)
        Dim RecCommand, LEDNum, LEDStatusNum, LoginResponse As Integer
        Dim LEDStatus, LED, Zone As String
        Dim SoftwareVersion As String
        Dim MessageParts() As String
        Dim FirstValue As Integer
        Dim PartitionState, PartitionStatus As String
        Dim ZoneInt As Integer

        MessageParts = message.Split(ControlChars.CrLf.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
        logging.AddToLog("Processing message:" & message, False)

        Try
            For Each mymessage As String In MessageParts

                logging.AddToLog("Processing string:" & mymessage, False)
                RecCommand = CInt(mymessage.Substring(0, 3))

                'If Not Command.IsDefined(GetType(Command), RecCommand) Then
                'logging.AddToLog("Unrecognized Command:" & mymessage, True)
                'Else

                'FirstValue is the first byte of the message after the command
                'It will often be the parttion but can also be other responses from the alarm

                If mymessage.Length >= 4 Then
                    FirstValue = CInt(mymessage.Substring(3, 1))
                Else
                    FirstValue = 0
                End If

                Select Case RecCommand
                    Case Command.CommandAcknowledge
                        logging.AddToLog("Command Acknowleged by Alarm", False)

                    Case Command.LoginInteraction
                        LoginResponse = CInt(mymessage.Substring(3, 1))
                        Select Case LoginResponse
                            Case 0
                                logging.AddToLog("Password provided was incorrect", False)
                            Case 1
                                logging.AddToLog("Password Correct, session established", False)
                            Case 2
                                logging.AddToLog("Time out. You did not send a password within 10 seconds", False)
                            Case 3
                                logging.AddToLog("Request for password, sent after socket setup", False)
                                LoginNeeded = True
                        End Select

                        'Partion State
                    Case Command.PartitionReady
                        PartitionState = "Ready"
                        SetPartitionState(PartitionState, FirstValue)
                    Case Command.PartitionNotReady
                        PartitionState = "NotReady"
                        SetPartitionState(PartitionState, FirstValue)
                    Case Command.PartitionArmed
                        PartitionState = "Armed"
                        SetPartitionState(PartitionState, FirstValue)
                    Case Command.PartitionReadyForceArming
                        PartitionState = "ReadyForceArming"
                        SetPartitionState(PartitionState, FirstValue)
                    Case Command.PartitionInAlarm
                        PartitionState = "InAlarm"
                        SetPartitionState(PartitionState, FirstValue)
                    Case Command.PartitionDisarmed
                        PartitionState = "Disarmed"
                        SetPartitionState(PartitionState, FirstValue)
                    Case Command.ExitDelayInProgress
                        PartitionState = "ExitDelayInProgress"
                        SetPartitionState(PartitionState, FirstValue)
                    Case Command.EntryDelayInProgress
                        PartitionState = "EntryDelayInProgress"
                        SetPartitionState(PartitionState, FirstValue)
                    Case Command.KeypadLockout
                        PartitionState = "KeypadLockout"
                        SetPartitionState(PartitionState, FirstValue)
                    Case Command.KeypadBlanking
                        PartitionStatus = "KeypadBlanking"
                        'SetPartitionStatus(PartitionStatus, FirstValue)
                    Case Command.CommandOutputInProgress
                        PartitionStatus = "CommandOutputInProgress"
                        'SetPartitionStatus(PartitionStatus)
                    Case Command.InvalidAccessCode
                        PartitionStatus = "InvalidAccessCode"
                        'SetPartitionStatus(PartitionStatus)
                    Case Command.FunctionNotAvailable
                        PartitionStatus = "FunctionNotAvailable"
                        'SetPartitionStatus(PartitionStatus)
                    Case Command.FailedToArm
                        PartitionStatus = "FailedToArm"
                        'SetPartitionStatus(PartitionStatus)
                    Case Command.PartitionBusy
                        PartitionStatus = "Busy"
                        'SetPartitionStatus(PartitionStatus)
                    Case Command.CodeRequired
                        PartitionStatus = "CodeRequired"
                        'SetPartitionStatus(PartitionStatus)

                        'Trouble LED Commands
                    Case Command.TroubleLEDOn
                        If FirstValue <= MaxPartition Then
                            OSAEObjectPropertyManager.ObjectPropertySet(Partition(FirstValue).Name, "TroubleLED", "On", pName)
                        End If
                    Case Command.TroubleLEDOff
                        If FirstValue <= MaxPartition Then
                            OSAEObjectPropertyManager.ObjectPropertySet(Partition(FirstValue).Name, "TroubleLED", "Off", pName)
                        End If

                        'Keybus
                    Case Command.KeybusFault
                        OSAEObjectPropertyManager.ObjectPropertySet(pName, "Keybus", "Fault", pName)
                    Case Command.KeybusFaultRestore
                        OSAEObjectPropertyManager.ObjectPropertySet(pName, "Keybus", "Restore", pName)

                        'LCDUpdate
                    Case Command.LCDUpdate
                        'Do nothing

                        'LED Status
                    Case Command.LEDStatus
                        LEDNum = CInt(mymessage.Substring(3, 1))
                        Select Case LEDNum
                            Case 1
                                LED = "Ready"
                            Case 2
                                LED = "Armed"
                            Case 3
                                LED = "Memory"
                            Case 4
                                LED = "Bypass"
                            Case 5
                                LED = "Trouble"
                            Case 6
                                LED = "Program"
                            Case 7
                                LED = "Fire"
                            Case 8
                                LED = "Backlight"
                            Case 9
                                LED = "AC"
                        End Select

                        LEDStatusNum = CInt(mymessage.Substring(4, 1))
                        Select Case LEDStatusNum
                            Case 0
                                LEDStatus = "Off"
                            Case 1
                                LEDStatus = "On"
                            Case 2
                                LEDStatus = "Flashing"
                        End Select

                        OSAEObjectPropertyManager.ObjectPropertySet(Partition(1).Name, "LED" & LED, LEDStatus, pName)

                    Case Command.KeypadLEDState
                        LEDNum = CInt(mymessage.Substring(3, 2))
                        Select Case LEDNum
                            Case LEDNum And 1
                                LED = "Ready"
                            Case LEDNum And 2
                                LED = "Armed"
                            Case LEDNum And 4
                                LED = "Memory"
                            Case LEDNum And 8
                                LED = "Bypass"
                            Case LEDNum And 16
                                LED = "Trouble"
                            Case LEDNum And 32
                                LED = "Program"
                            Case LEDNum And 64
                                LED = "Fire"
                            Case LEDNum And 128
                                LED = "Backlight"
                        End Select

                        OSAEObjectPropertyManager.ObjectPropertySet(Partition(1).Name, "LED" & LED, LEDStatus, pName)

                    Case Command.KeypadLEDFlashState
                        LEDNum = CInt(mymessage.Substring(3, 2))
                        Select Case LEDNum
                            Case LEDNum And 1
                                LED = "Ready"
                            Case LEDNum And 2
                                LED = "Armed"
                            Case LEDNum And 4
                                LED = "Memory"
                            Case LEDNum And 8
                                LED = "Bypass"
                            Case LEDNum And 16
                                LED = "Trouble"
                            Case LEDNum And 32
                                LED = "Program"
                            Case LEDNum And 64
                                LED = "Fire"
                            Case LEDNum And 128
                                LED = "Backlight"
                        End Select

                        OSAEObjectPropertyManager.ObjectPropertySet(Partition(1).Name, "LED" & LED, LEDStatus, pName)

                    Case Command.ZoneOpen, Command.ZoneRestored
                        Zone = mymessage.Substring(4, 2)
                        ZoneInt = CInt(Zone)
                        If ZoneInt >= 1 And ZoneInt <= 32 Then
                            If RecCommand = Command.ZoneOpen Then
                                If ZoneArray(ZoneInt) <> "Open" Then
                                    logging.AddToLog("Zone " & Zone & " Open", True)
                                    OSAEObjectPropertyManager.ObjectPropertySet(Partition(1).Name, "Zone" & Zone, "Open", pName)
                                    logging.EventLogAdd(Partition(1).Name, "Zone" & Zone & " Open", Zone, "Open")
                                    ZoneArray(ZoneInt) = "Open"
                                End If
                            Else
                                If ZoneArray(ZoneInt) <> "Closed" Then
                                    logging.AddToLog("Zone " & Zone & " Close", True)
                                    OSAEObjectPropertyManager.ObjectPropertySet(Partition(1).Name, "Zone" & Zone, "Closed", pName)
                                    logging.EventLogAdd(Partition(1).Name, "Zone" & Zone & " Closed", Zone, "Closed")
                                    ZoneArray(ZoneInt) = "Closed"
                                End If
                            End If
                        End If

                    Case Command.SoftwareVersion
                        SoftwareVersion = mymessage.Substring(3, 6)
                        OSAEObjectPropertyManager.ObjectPropertySet(pName, "SoftwareVersion", SoftwareVersion, pName)
                    Case Command.VerboseTroubleStatus
                        logging.AddToLog("Verbose Trouble Status", True)
                    Case Else
                        logging.AddToLog("Command not configured for message:" & mymessage, True)
                End Select
                'End If

            Next

        Catch ex As Exception
            logging.AddToLog("Error parsing message:" & ex.Message, True)
        End Try
    End Sub

    Shared Sub SetPartitionState(PartitionState As String, PartitionNumber As Integer)

        Try

            If PartitionNumber <= MaxPartition And PartitionState <> Partition(PartitionNumber).State Then
                logging.AddToLog("Partition " & PartitionNumber.ToString & " State changed to " & PartitionState, True)
                OSAEObjectStateManager.ObjectStateSet(Partition(PartitionNumber).Name, PartitionState, pName)
                logging.EventLogAdd(Partition(PartitionNumber).Name, "Partition " & PartitionState, PartitionState)
                Partition(PartitionNumber).State = PartitionState
            End If
        Catch ex As Exception
            logging.AddToLog("Error updating partition State: " & ex.Message, True)
        End Try

    End Sub

    Shared Function CalcChecksum(ByVal SendString As String) As String

        Try
            Dim CheckSum As Integer

            For Each Character In SendString.ToArray
                CheckSum += Convert.ToByte(Character)
            Next

            CheckSum = CheckSum And &HFF
            Return (CheckSum >> 4).ToString("X") & (CheckSum And &HF).ToString("X")

        Catch ex As Exception
            logging.AddToLog("Error in CalcChecksum " & ex.Message, True)
            Return "00"
        End Try
    End Function

    Shared Function CommandToString(ByVal MyCommand As Command) As String
        Return Int(MyCommand).ToString("D3")
    End Function
    Private Shared Sub SetupReceive(ByVal client As Socket)
        Try
            ' Create the state object.
            Dim state As New StateObject
            state.workSocket = client

            ' Begin receiving the data from the remote device.
            client.BeginReceive(state.buffer, 0, state.BufferSize, 0, _
                New AsyncCallback(AddressOf ReceiveCallback), state)
        Catch ex As Exception
            logging.AddToLog("Error setting up receive buffer " & ex.Message, True)
        End Try
    End Sub 'Receive

    Private Shared Sub ReceiveCallback(ByVal ar As IAsyncResult)
        logging.AddToLog("Starting Receive Callback", False)

        'ReceiveThead = Thread.CurrentThread
        Try
            ' Retrieve the state object and the client socket 
            ' from the asynchronous state object.
            Dim state As StateObject = CType(ar.AsyncState, StateObject)
            Dim client As Socket = state.workSocket

            ' Read data from the remote device.
            Dim bytesRead As Integer = client.EndReceive(ar)

            'logging.AddToLog("Received: " & Encoding.ASCII.GetString(state.buffer, 0, _
            '        bytesRead).TrimEnd(vbCr) & " :Length " & bytesRead.ToString, True)

            'If bytesRead > 0 Then
            ' There might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead))

            'If bytesRead = state.buffer.Length And client.Available > 0 Then
            '  Get the rest of the data.
            'client.BeginReceive(state.buffer, 0, state.BufferSize, 0, _
            '   New AsyncCallback(AddressOf ReceiveCallback), state)
            'Else
            ' All the data has arrived; put it in response.
            'If state.sb.Length > 0 Then
            Data = state.sb.ToString()
            ParseMessage(Data)
            'End If
            If Not ShutdownNow Then
                SetupReceive(client_socket)
            End If

            'End If

        Catch ex As Exception
            logging.AddToLog("Error receiving data " & ex.Message, True)
            CommDown = True
        End Try
    End Sub 'ReceiveCallback
End Class

Public Class StateObject
    ' Client socket.
    Public workSocket As Socket = Nothing
    ' Size of receive buffer.
    Public BufferSize As Integer = 256
    ' Receive buffer.
    Public buffer(BufferSize) As Byte
    ' Received data string.
    Public sb As New StringBuilder
End Class 'StateObject

