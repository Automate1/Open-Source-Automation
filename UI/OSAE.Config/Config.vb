#Const RemoteServer = False

Option Strict Off
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
'Imports System.ServiceProcess

Public Module MyGlobal

    Public CN As MySqlConnection
    'Public OSAEApi As OSAE
    Public ServerName As String = "localhost"
    'Public myService As ServiceController = New ServiceController()

End Module


Public Class Config
    'Public WithEvents Config As Form

    Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'CheckService()
    End Sub

    Public Sub DB_Connection()
        CN = New MySqlConnection
        CN.ConnectionString = "server=" & ServerName & ";Port=" & Common.DBPort & ";Database=" & Common.DBName & ";Password=" & Common.DBPassword & ";use procedure bodies=false;Persist Security Info=True;User ID=" & Common.DBUsername

        Try
            CN.Open()
            CN.Close()
        Catch myerror As MySqlException
            MessageBox.Show("Error Connecting to Database: " & myerror.Message)
        End Try
    End Sub


    Private Sub Objects_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Objects.Click

        frmObjects.Show()
        frmObjects.WindowState = FormWindowState.Normal
        frmObjects.BringToFront()

    End Sub

    Private Sub ObjectTypes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ObjectTypes.Click

        frmObjectTypes.Show()
        frmObjectTypes.WindowState = FormWindowState.Normal
        frmObjectTypes.BringToFront()

    End Sub



    Private Sub Setup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        frmSetup.Show()
        frmSetup.WindowState = FormWindowState.Normal
        frmSetup.BringToFront()

    End Sub

 
    Private Sub CheckService()
        'myService.MachineName = ServerName
        'myService.ServiceName = "OSAE"

        'Dim svcStatus As String = myService.Status.ToString()
        'If svcStatus = "Running" Then
        'lblService.Text = "RUNNING"
        'btnService.Content = "Stop"
        'ElseIf svcStatus = "Stopped" Then
        'lblService.Text = "STOPPED"
        'btnService.Content = "Start"
        'End If
    End Sub

    Private Sub butExport_Click(sender As Object, e As EventArgs) Handles butExport.Click
        Dim CMD As New MySqlCommand, sName As String, sLabel As String
        Dim myReader As MySql.Data.MySqlClient.MySqlDataReader
        frmSQLBox.Show()
        frmSQLBox.txtScript.Text = ""
        CMD.Connection = CN
        CMD.CommandText = "SELECT history_timestamp, object_name, property_name, property_value FROM osae_v_object_property_history"
        'CMD.Parameters.AddWithValue("?ObjectName", dgvObjects("object_name", dgvObjects.CurrentCell.RowIndex).Value)
        Try
            If CN.State = ConnectionState.Closed Then CN.Open()
            myReader = CMD.ExecuteReader()
            While myReader.Read
                sName = myReader.Item("property_name")
                sLabel = "" & myReader.Item("property_value")
                frmSQLBox.txtScript.Text &= "INSERT INTO osae_object_property_history (object_property_id,property_value) VALUES(NEW.object_property_id,NEW.property_value);" ');" & vbCrLf
            End While
            CN.Close()
        Catch myerror As MySqlException
            MessageBox.Show("Error in Write_Script: " & myerror.Message)
            If CN.State <> ConnectionState.Closed Then CN.Close()
        End Try

    End Sub
End Class
