Option Strict Off
Option Explicit On

Imports System.Threading.Thread
Imports System.Timers
Imports System.Net
Imports System.Net.Sockets
Imports System.DateTime
Imports System.Text
Imports System.Text.RegularExpressions

Public Class APC_UPS
    Inherits OSAEPluginBase
    Private Shared logging As Logging = logging.GetLogger("APC UPS")
    Private UpdateTimer As Timer
    Private pName As String

    Private SocketInitNeeded As Boolean
    Private UpdateInterval As Integer
    Dim client_socket As New Socket(AddressFamily.InterNetwork, _
    SocketType.Stream, ProtocolType.Tcp)
    Dim UPSValues As New List(Of String)(New String() {"STATUS", "LINEV", "LOADPCT", "BCHARGE", "TIMELEFT", "BATTV", "LASTXFER"})
    Dim UPSVariablesDic As New Dictionary(Of String, String) 'From _
    '{{"STATUS", ""}, {"LINEV", ""}, {"LOADPCT", ""}, {"BCHARGE", ""}, {"TIMELEFT", ""}, {"BATTV", ""}, {"LASTXFER", ""}}
    Dim Data As String
    Dim CurrentDateTime As DateTime
    Dim msg As Byte()
    Dim ReceiveMessage(1492) As Byte
    Dim MyMessage, ValueString As String
    Dim STATUS, LINEV, LOADPCT, BCHARGE, TIMELEFT, BATTV, LASTXFER As String


    Public Overrides Sub RunInterface(ByVal pluginName As String)
        Dim Host As String
        Dim Port As Integer


        Try
            pName = pluginName
            logging.AddToLog("Initializing plugin: " & pName, True)
            'ComputerName = OSAEApi.ComputerName

            For Each UPSVariable In UPSValues
                UPSVariablesDic.Add(UPSVariable, OSAEObjectPropertyManager.GetObjectPropertyValue(pName, UPSVariable).Value)
            Next

            'Threading.Thread.Sleep(1000)
            Host = OSAEObjectPropertyManager.GetObjectPropertyValue(pName, "Host").Value
            Port = OSAEObjectPropertyManager.GetObjectPropertyValue(pName, "Port").Value
            logging.AddToLog("Connecting to " & Host & ":" & Port.ToString, True)
            client_socket.Connect(Host, Port)
            client_socket.ReceiveTimeout = 2000

            Try
                UpdateInterval = 100 * Int(OSAEObjectPropertyManager.GetObjectPropertyValue(pName, "Update Interval").Value)
            Catch
                UpdateInterval = 30000
            End Try

            logging.AddToLog("Update Interval set to: " & (UpdateInterval / 1000).ToString & " secs", True)

            UpdateTimer = New Timer(UpdateInterval)
            AddHandler UpdateTimer.Elapsed, New ElapsedEventHandler(AddressOf TimerHandler)
            UpdateTimer.Enabled = True
            GetUPSStatus()

        Catch ex As Exception
            logging.AddToLog("Error setting up plugin: " & ex.Message, True)
        End Try

    End Sub

    Public Overrides Sub ProcessCommand(method As OSAEMethod)
        Try
            If method.MethodName = "PRINTMESSAGE" Then
                logging.AddToLog("Print message received from UPS", True)
                GetUPSStatus()
                logging.AddToLog("Recieved message " & MyMessage, True)
            ElseIf method.MethodName = "UPSEVENT" Then
                logging.AddToLog("Received UPS event " & method.Parameter1, True)
                logging.EventLogAdd(pName, method.Parameter1)
            End If
        Catch ex As Exception
            logging.AddToLog("Error Processing Command - " & ex.Message, True)
        End Try

    End Sub


    Public Overrides Sub Shutdown()

        UpdateTimer.Stop()
        client_socket.Shutdown(SocketShutdown.Both)
        client_socket.Close()

        logging.AddToLog("Shutting down plugin", True)
    End Sub

    Public Sub SocketInit()
        logging.AddToLog("Re-initializing socket", True)
        SocketInitNeeded = False
        Try
            client_socket.Shutdown(SocketShutdown.Both)
            client_socket.Close()
            client_socket = New Socket(AddressFamily.InterNetwork, _
                SocketType.Stream, ProtocolType.Tcp)
            client_socket.Connect("localhost", 3551)
            client_socket.ReceiveTimeout = 2000
        Catch ex As Exception
            logging.AddToLog("Error re-initializing socket: " & ex.Message, True)
        End Try
    End Sub

    Protected Sub TimerHandler(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        'logging.AddToLog("Timer Update", False)
        GetUPSStatus()
    End Sub

    Protected Sub GetUPSStatus()
        Dim bytesRec As Integer
        Dim ReturnValue As Integer
        Dim Found As Integer
        Dim MatchObj As Match

        If SocketInitNeeded Then
            SocketInit()
        End If
        Try

            'ReturnValue = client_socket.Send(Encoding.ASCII.GetBytes("status"), 6, 0)
            '0x00, 0x06, 0x73, 0x74, 0x61, 0x74, 0x75, 0x73

            ReturnValue = client_socket.Send({&H0, &H6, &H73, &H74, &H61, &H74, &H75, &H73}, 8, 0)
            logging.AddToLog("Sent message, returned: " & ReturnValue.ToString, False)

        Catch ex As Exception
            SocketInitNeeded = True
            logging.AddToLog("Error in send: " & ex.Message, True)
        End Try

        MyMessage = ""
        Try
            While True
                bytesRec = client_socket.Receive(ReceiveMessage)
                MyMessage += Encoding.ASCII.GetString(ReceiveMessage, 0, bytesRec)
                logging.AddToLog("Recieved message of length " & bytesRec.ToString, False)
                If ReceiveMessage(bytesRec - 1) = &H0 And ReceiveMessage(bytesRec - 2) = &H0 Then
                    Exit While
                End If
            End While

            For Each UPSVariable In UPSVariablesDic.Keys.ToList()
                'STATUS   : ONLINE
                MatchObj = Regex.Match(MyMessage, UPSVariable & "\s+:\s+\S+")
                If MatchObj.Success Then
                    Found = InStrRev(MatchObj.Value, " ")
                    ValueString = Mid(MatchObj.Value, Found + 1)

                    If UPSVariablesDic(UPSVariable) <> ValueString Then
                        logging.AddToLog("New: " & UPSVariable & " " & ValueString, False)
                        OSAEObjectPropertyManager.ObjectPropertySet(pName, UPSVariable, ValueString, pName)
                        UPSVariablesDic(UPSVariable) = ValueString
                    End If
                End If

            Next

        Catch ex As Exception
            SocketInitNeeded = True
            logging.AddToLog("Error in receive: " & ex.Message, True)
        End Try

    End Sub
End Class

