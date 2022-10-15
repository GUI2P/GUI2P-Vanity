<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class GUI2P
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GUI2P))
        Me.addressList = New System.Windows.Forms.RichTextBox()
        Me.threads = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.startStop = New System.Windows.Forms.Button()
        Me.log = New System.Windows.Forms.RichTextBox()
        Me.Save = New System.Windows.Forms.Button()
        Me.Load = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.clearLog = New System.Windows.Forms.Button()
        Me.shuffleButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'addressList
        '
        Me.addressList.BackColor = System.Drawing.Color.Black
        Me.addressList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.addressList.ForeColor = System.Drawing.Color.Plum
        Me.addressList.Location = New System.Drawing.Point(12, 33)
        Me.addressList.Name = "addressList"
        Me.addressList.Size = New System.Drawing.Size(567, 130)
        Me.addressList.TabIndex = 0
        Me.addressList.Text = ""
        '
        'threads
        '
        Me.threads.BackColor = System.Drawing.Color.Black
        Me.threads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.threads.ForeColor = System.Drawing.Color.Plum
        Me.threads.Location = New System.Drawing.Point(66, 169)
        Me.threads.Name = "threads"
        Me.threads.Size = New System.Drawing.Size(59, 23)
        Me.threads.TabIndex = 1
        Me.threads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Plum
        Me.Label1.Location = New System.Drawing.Point(12, 172)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 15)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Threads"
        '
        'startStop
        '
        Me.startStop.FlatAppearance.BorderColor = System.Drawing.Color.Plum
        Me.startStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Thistle
        Me.startStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumOrchid
        Me.startStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.startStop.ForeColor = System.Drawing.Color.Plum
        Me.startStop.Location = New System.Drawing.Point(504, 169)
        Me.startStop.Name = "startStop"
        Me.startStop.Size = New System.Drawing.Size(75, 23)
        Me.startStop.TabIndex = 3
        Me.startStop.Text = "Start"
        Me.startStop.UseVisualStyleBackColor = True
        '
        'log
        '
        Me.log.BackColor = System.Drawing.Color.Black
        Me.log.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.log.ForeColor = System.Drawing.Color.Plum
        Me.log.Location = New System.Drawing.Point(12, 198)
        Me.log.Name = "log"
        Me.log.ReadOnly = True
        Me.log.Size = New System.Drawing.Size(567, 164)
        Me.log.TabIndex = 4
        Me.log.Text = ""
        '
        'Save
        '
        Me.Save.FlatAppearance.BorderColor = System.Drawing.Color.Plum
        Me.Save.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Thistle
        Me.Save.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumOrchid
        Me.Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Save.ForeColor = System.Drawing.Color.Plum
        Me.Save.Location = New System.Drawing.Point(131, 169)
        Me.Save.Name = "Save"
        Me.Save.Size = New System.Drawing.Size(75, 23)
        Me.Save.TabIndex = 5
        Me.Save.Text = "Save"
        Me.Save.UseVisualStyleBackColor = True
        '
        'Load
        '
        Me.Load.FlatAppearance.BorderColor = System.Drawing.Color.Plum
        Me.Load.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Thistle
        Me.Load.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumOrchid
        Me.Load.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Load.ForeColor = System.Drawing.Color.Plum
        Me.Load.Location = New System.Drawing.Point(212, 169)
        Me.Load.Name = "Load"
        Me.Load.Size = New System.Drawing.Size(75, 23)
        Me.Load.TabIndex = 6
        Me.Load.Text = "Load"
        Me.Load.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Plum
        Me.Label2.Location = New System.Drawing.Point(12, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(234, 15)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Enter desired I2P text separated by new line"
        '
        'clearLog
        '
        Me.clearLog.FlatAppearance.BorderColor = System.Drawing.Color.Plum
        Me.clearLog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Thistle
        Me.clearLog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumOrchid
        Me.clearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.clearLog.ForeColor = System.Drawing.Color.Plum
        Me.clearLog.Location = New System.Drawing.Point(423, 169)
        Me.clearLog.Name = "clearLog"
        Me.clearLog.Size = New System.Drawing.Size(75, 23)
        Me.clearLog.TabIndex = 8
        Me.clearLog.Text = "Clear Log"
        Me.clearLog.UseVisualStyleBackColor = True
        '
        'shuffleButton
        '
        Me.shuffleButton.FlatAppearance.BorderColor = System.Drawing.Color.Plum
        Me.shuffleButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Thistle
        Me.shuffleButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumOrchid
        Me.shuffleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.shuffleButton.ForeColor = System.Drawing.Color.Plum
        Me.shuffleButton.Location = New System.Drawing.Point(293, 169)
        Me.shuffleButton.Name = "shuffleButton"
        Me.shuffleButton.Size = New System.Drawing.Size(124, 23)
        Me.shuffleButton.TabIndex = 9
        Me.shuffleButton.Text = "Random"
        Me.shuffleButton.UseVisualStyleBackColor = True
        '
        'GUI2P
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(591, 374)
        Me.Controls.Add(Me.shuffleButton)
        Me.Controls.Add(Me.clearLog)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Load)
        Me.Controls.Add(Me.Save)
        Me.Controls.Add(Me.log)
        Me.Controls.Add(Me.startStop)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.threads)
        Me.Controls.Add(Me.addressList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "GUI2P"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GUI2P Vanity"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents addressList As RichTextBox
    Friend WithEvents threads As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents startStop As Button
    Friend WithEvents log As RichTextBox
    Friend WithEvents Save As Button
    Friend WithEvents Load As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents clearLog As Button
    Friend WithEvents shuffleButton As Button
End Class
