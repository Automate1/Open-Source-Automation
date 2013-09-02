Public Class frmSetup
    Private WithEvents frmSetup As Form

     Private Sub frmSetup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtboxServerName.Text = MyGlobal.ServerName

    End Sub

    Private Sub txtProperty_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtboxServerName.TextChanged

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        MyGlobal.ServerName = txtboxServerName.Text

    End Sub
End Class