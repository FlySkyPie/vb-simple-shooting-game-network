<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Terminal
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
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

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請不要使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PlayersList = New System.Windows.Forms.ListBox()
        Me.CommandBox = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'PlayersList
        '
        Me.PlayersList.FormattingEnabled = True
        Me.PlayersList.ItemHeight = 12
        Me.PlayersList.Location = New System.Drawing.Point(12, 12)
        Me.PlayersList.Name = "PlayersList"
        Me.PlayersList.Size = New System.Drawing.Size(120, 364)
        Me.PlayersList.TabIndex = 0
        '
        'CommandBox
        '
        Me.CommandBox.Font = New System.Drawing.Font("新細明體", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.CommandBox.Location = New System.Drawing.Point(138, 352)
        Me.CommandBox.Name = "CommandBox"
        Me.CommandBox.Size = New System.Drawing.Size(452, 23)
        Me.CommandBox.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(597, 353)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(95, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Send"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Terminal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(704, 387)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.CommandBox)
        Me.Controls.Add(Me.PlayersList)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Terminal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ShiangBaDaOnline_Server     by FlySkyPie"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PlayersList As System.Windows.Forms.ListBox
    Friend WithEvents CommandBox As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
