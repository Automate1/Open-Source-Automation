<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Config))
        Me.Objects = New System.Windows.Forms.Button()
        Me.ObjectTypes = New System.Windows.Forms.Button()
        Me.butExport = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Objects
        '
        Me.Objects.Location = New System.Drawing.Point(37, 86)
        Me.Objects.Margin = New System.Windows.Forms.Padding(4)
        Me.Objects.Name = "Objects"
        Me.Objects.Size = New System.Drawing.Size(121, 28)
        Me.Objects.TabIndex = 0
        Me.Objects.Text = "Objects"
        Me.Objects.UseVisualStyleBackColor = True
        '
        'ObjectTypes
        '
        Me.ObjectTypes.Location = New System.Drawing.Point(37, 24)
        Me.ObjectTypes.Margin = New System.Windows.Forms.Padding(4)
        Me.ObjectTypes.Name = "ObjectTypes"
        Me.ObjectTypes.Size = New System.Drawing.Size(124, 28)
        Me.ObjectTypes.TabIndex = 1
        Me.ObjectTypes.Text = "Object Types"
        Me.ObjectTypes.UseVisualStyleBackColor = True
        '
        'butExport
        '
        Me.butExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.butExport.Enabled = False
        Me.butExport.Location = New System.Drawing.Point(37, 145)
        Me.butExport.Margin = New System.Windows.Forms.Padding(4)
        Me.butExport.Name = "butExport"
        Me.butExport.Size = New System.Drawing.Size(121, 32)
        Me.butExport.TabIndex = 65
        Me.butExport.Text = "Export History"
        Me.butExport.UseVisualStyleBackColor = True
        '
        'Config
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(203, 228)
        Me.Controls.Add(Me.butExport)
        Me.Controls.Add(Me.ObjectTypes)
        Me.Controls.Add(Me.Objects)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Config"
        Me.Text = "OSAE Config"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Objects As System.Windows.Forms.Button
    Friend WithEvents ObjectTypes As System.Windows.Forms.Button
    Friend WithEvents butExport As System.Windows.Forms.Button

End Class
