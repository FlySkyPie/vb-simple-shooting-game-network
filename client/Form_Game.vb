Public Class Form_Game
    'GUI運算
    '-鍵盤
    Private Sub ScreenBox_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) 'Handles ScreenBox.MouseMove
        If Online Then '連線中
            Mouse_X = e.X
            Mouse_Y = e.Y
            Mouse_Angle = Math.Atan2(e.Y - ScreenBox.Height / 2, e.X - ScreenBox.Width / 2)
            Mouse_Rang = (e.X - ScreenBox.Width / 2) ^ 2 + (e.Y - ScreenBox.Height / 2) ^ 2
            Mouse_Rang = Mouse_Rang ^ 0.5
            Players(0).Angle_W = Mouse_Angle
        End If
    End Sub

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        e.Handled = True
        If e.KeyValue = Keys.W Then key_up = True
        If e.KeyValue = Keys.A Then key_left = True
        If e.KeyValue = Keys.D Then key_right = True
        If e.KeyValue = Keys.S Then key_down = True
    End Sub

    Private Sub Form1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        e.Handled = True
        If e.KeyValue = Keys.W Then key_up = False
        If e.KeyValue = Keys.A Then key_left = False
        If e.KeyValue = Keys.D Then key_right = False
        If e.KeyValue = Keys.S Then key_down = False
    End Sub
    '-滑鼠
    Private Sub ScreenBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) ' Handles ScreenBox.MouseDown
        If Online Then
            Players(0).Attacking = True : send("a" & Account & "?")
        End If

    End Sub
    Private Sub ScreenBox_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) ' Handles ScreenBox.MouseUp
        If Online Then
            Players(0).Attacking = False : send("q" & Account & "?")
        End If
    End Sub




    '載入控件
    Private Sub Form_Game_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        '載入圖形化控件
        '載入文字
        Label_A = New Label
        Label_A.AutoSize = True
        Label_A.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Label_A.Location = New System.Drawing.Point(15, 18)
        'Label_A.Name = "Label1"
        Label_A.Size = New System.Drawing.Size(44, 16)
        Label_A.TabIndex = 1
        Label_A.Text = "帳號:"

        Label_P = New Label
        Label_P.AutoSize = True
        Label_P.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Label_P.Location = New System.Drawing.Point(15, 50)
        'Label_P.Name = "Label2"
        Label_P.Size = New System.Drawing.Size(44, 16)
        Label_P.TabIndex = 3
        Label_P.Text = "密碼:"
        '載入文字方塊
        AccountBox = New TextBox
        AccountBox.Location = New System.Drawing.Point(65, 16)
        AccountBox.Name = "TextBox_A"
        AccountBox.Size = New System.Drawing.Size(193, 22)
        AccountBox.TabIndex = 0

        PasswordBox = New TextBox
        PasswordBox.Location = New System.Drawing.Point(65, 48)
        PasswordBox.Name = "TextBox_P"
        PasswordBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        PasswordBox.ReadOnly = True
        PasswordBox.Size = New System.Drawing.Size(193, 22)
        PasswordBox.TabIndex = 2
        PasswordBox.Text = "123"

        '載入按鈕
        Button_Login = New Button
        Button_Login.Location = New System.Drawing.Point(264, 16)
        Button_Login.Name = "Button1"
        Button_Login.Size = New System.Drawing.Size(83, 54)
        Button_Login.TabIndex = 4
        Button_Login.Text = "登入"
        Button_Login.UseVisualStyleBackColor = True
        AddHandler Button_Login.Click, AddressOf Button_Click

        '載入LoginBox
        LoginBox = New System.Windows.Forms.GroupBox()
        LoginBox.Controls.Add(AccountBox)
        LoginBox.Controls.Add(PasswordBox)
        LoginBox.Controls.Add(Button_Login)
        LoginBox.Controls.Add(Label_A)
        LoginBox.Controls.Add(Label_P)
        LoginBox.Location = New Point(142, 199)
        ' LoginBox.Name = "LoginBox"
        LoginBox.Size = New Size(357, 83)
        LoginBox.TabIndex = 6
        LoginBox.TabStop = False
        LoginBox.Visible = True

        Me.Controls.Add(LoginBox)
        LoginBox.ResumeLayout(False)
        LoginBox.PerformLayout()

        '載入screenboxx
        ScreenBox = New PictureBox
        ScreenBox.BackColor = Color.FromArgb(32, 32, 32)
        ScreenBox.Location = New System.Drawing.Point(0, 0)
        ScreenBox.Name = "TerminalBox"
        ScreenBox.Size = New System.Drawing.Size(640, 480)
        ScreenBox.TabIndex = 1
        ScreenBox.TabStop = False
        AddHandler ScreenBox.MouseMove, AddressOf ScreenBox_MouseMove
        AddHandler ScreenBox.MouseDown, AddressOf ScreenBox_MouseDown
        AddHandler ScreenBox.MouseUp, AddressOf ScreenBox_MouseUp
        Me.Controls.Add(ScreenBox)
    End Sub

    '送出密碼
    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles Button_Login.Click
        If AccountBox.Text = "" Then
            MsgBox("請輸入帳號!!", MsgBoxStyle.OkOnly, "錯誤")
        ElseIf PasswordBox.Text = "" Then
            MsgBox("請輸入密碼!!", MsgBoxStyle.OkOnly, "錯誤")
        Else
            Account = AccountBox.Text
            Password = PasswordBox.Text
            connect()
        End If
    End Sub
    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'e.Cancel = True
        If Online Then    '連線中
            Timer_Game.Enabled = False
            Timer_Graphic.Enabled = False
            send("9" & Account & "?")
            'T.Close()
            'Th.Abort()
            Application.ExitThread()
            Me.Dispose()
            End
        End If
        'Me.Dispose()
    End Sub
End Class