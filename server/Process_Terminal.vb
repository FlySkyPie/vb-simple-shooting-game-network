Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text
Imports System.Timers
Public Module Process_Terminal
    '引入變數和常數們
    Dim Max_bullet = Data_Game.Max_bullet
    Dim Max_player = Data_Game.Max_player
    Dim Pi = Data_Game.Pi
    Dim Players = Data_Game.Players
    Dim Bullet = Data_Game.Bullet
    Dim Guns = Data_Game.Guns
    Dim Ammo_Type = Data_Game.Ammo_Type
    '跟圖形顯示有關的變數
    Public TerminalBox_W, TerminalBox_H As Integer
    Public Logs(22) As String
    Public TerminalBox As PictureBox
    'im SB As PictureBox = Terminal.TerminalBox
    '圖形化追蹤變數
    Public Ratio As Single = 30
    Dim Camera_X As Single      '攝影機座標
    Dim Camera_Y As Single      '攝影機座標
    Public GUI_mod As Boolean = False
    Public Lock_player As Human

    Public Sub GUITimer_Tick(ByVal sender As System.Object, ByVal e As ElapsedEventArgs)
        '繪圖
        Dim GUI As Bitmap
        GUI = New Bitmap(TerminalBox.Width, TerminalBox.Height)
        Dim drawFont As New Font("Arial", 9)
        Dim drawBrush As New SolidBrush(Color.FromArgb(64, 225, 225))
        Dim drawpen As New Pen(drawBrush)
        Dim g As Graphics
        g = Graphics.FromImage(GUI)
        g.Clear(Color.FromArgb(32, 32, 32))
        drawBrush.Color = Color.FromArgb(64, 225, 225)
        '-圖像
        '--定位攝影機
        Camera_X = Lock_player.X
        Camera_Y = Lock_player.Y
        '--攝影機看得到的範圍
        Dim _Y_min, _Y_max, _X_min, _X_max As Single
        _X_min = Camera_X - TerminalBox.Width / 2 / Ratio
        _X_max = Camera_X + TerminalBox.Width / 2 / Ratio
        _Y_min = Camera_Y - TerminalBox.Height / 2 / Ratio
        _Y_max = Camera_Y + TerminalBox.Height / 2 / Ratio
        '---繪製地圖線
        drawpen.Color = Color.FromArgb(16, 64, 70)
        Dim hh, Sx, Sy As Integer
        Sx = TerminalBox.Width / 2
        Sy = TerminalBox.Height / 2
        hh = Int((Fix(Camera_X) - Camera_X) * Ratio)
        Dim tw, th As Integer
        tw = Int(TerminalBox.Width / Ratio / 2)
        th = Int(TerminalBox.Height / Ratio / 2)
        For k = -tw To tw
            Dim pp1 As Point = New Point(Sx + hh + k * Ratio, 0)
            Dim pp2 As Point = New Point(Sx + hh + k * Ratio, TerminalBox.Height)
            g.DrawLine(drawpen, pp1, pp2)
        Next
        hh = Int((Fix(Camera_Y) - Camera_Y) * Ratio)
        For k = -th To th
            Dim pp1 As Point = New Point(0, Sy + hh + k * Ratio)
            Dim pp2 As Point = New Point(TerminalBox.Width, Sy + hh + k * Ratio)
            g.DrawLine(drawpen, pp1, pp2)
        Next
        '---繪製玩家
        drawpen.Color = Color.FromArgb(64, 255, 255)
        Dim i As Integer
        For i = 0 To Max_player
            If Players(i).Enable Then '玩家物件使用中
                If (Players(i).X > _X_min And Players(i).X < _X_max) And (Players(i).Y > _Y_min And Players(i).Y < _Y_max) Then '繪製範圍內 開始繪圖
                    Dim c_x, c_y As Integer
                    c_x = Int(TerminalBox.Width / 2) + (Players(i).X - Camera_X) * Ratio
                    c_y = Int(TerminalBox.Height / 2) + (Players(i).Y - Camera_Y) * Ratio
                    Dim _R As Single = Players(0).R * Ratio
                    g.DrawEllipse(drawpen, c_x - _R, c_y - _R, _R * 2, _R * 2)
                    Dim p1 As Point = New Point(c_x, c_y)
                    Dim p2 As Point = New Point(c_x + Int(_R * 2 * Math.Cos(Players(i).Angle_W)), c_y + Int(_R * 2 * Math.Sin(Players(i).Angle_W)))
                    g.DrawLine(drawpen, p1, p2)
                End If
            End If
        Next
        '---繪製子彈
        For i = 0 To Max_bullet
            If Bullet(i).Enable Then
                If (Bullet(i).X > _X_min And Bullet(i).X < _X_max) And (Bullet(i).Y > _Y_min And Bullet(i).Y < _Y_max) Then '繪製範圍內 開始繪圖
                    Dim c_x, c_y As Integer
                    c_x = Int(TerminalBox.Width / 2) + (Bullet(i).X - Camera_X) * Ratio
                    c_y = Int(TerminalBox.Height / 2) + (Bullet(i).Y - Camera_Y) * Ratio
                    'g.DrawEllipse(drawpen, c_x, c_y, 2, 2)
                    Dim p1 As Point = New Point(c_x, c_y)
                    Dim p2 As Point = New Point(c_x - Int(0.15 * Ratio * Math.Cos(Bullet(i).Angle)), c_y - Int(0.15 * Ratio * Math.Sin(Bullet(i).Angle)))
                    g.DrawLine(drawpen, p1, p2)
                End If
            End If
        Next
        '-文字
        drawFont = New Font("Arial", 9)
        Dim Stmp As String = ""
        Stmp += "Name:" & Players(0).Name & vbNewLine
        Stmp += "HP:" & Players(0).HP & vbNewLine
        Stmp += "MP:" & Players(0).MP & vbNewLine
        Stmp += "R:" & Players(0).R & vbNewLine
        Stmp += "Location:(" & Players(0).X & "," & Players(0).Y & ")" & vbNewLine
        Stmp += "Moving Direcion:" & Players(0).Move_direct & vbNewLine
        Stmp += "body Direcion:" & Players(0).Angle_W & vbNewLine
        Stmp += "attacking:" & Players(0).Attacking & "/" & Players(0).Attack_count & vbNewLine
        Stmp += "Weapon:" & Players(0).Gun.Name & vbNewLine
        g.DrawString(Stmp, drawFont, drawBrush, 1, 1)
        TerminalBox.Image = GUI
    End Sub

    Public Sub Initialize_TerminalBox()
        TerminalBox = New PictureBox
        TerminalBox.BackColor = System.Drawing.SystemColors.Control
        TerminalBox.Location = New System.Drawing.Point(138, 12)
        TerminalBox.Name = "TerminalBox"
        TerminalBox.Size = New System.Drawing.Size(554, 334)
        TerminalBox.TabIndex = 1
        TerminalBox.TabStop = False
        Terminal.Controls.Add(TerminalBox)
    End Sub
    Public Sub Terminal_Command(ByVal _com As String) '終端機執行命令
        Dim _command() As String = _com.Split(" ")
        Select Case _command(0)
            Case "start"
                If _command.Length = 1 Then
                    Send_Msg_To_Log("Server", "Please type in ip.")
                ElseIf _command.Length = 2 Then
                    If Server_Online Then
                        Send_Msg_To_Log("Server", "Error command,Server has been running!")
                    Else
                        server_ip = _command(1)
                        Start_sever()
                    End If
                Else
                    Send_Msg_To_Log("Server", "Error command,please type right command in.")
                End If
            Case "ip"
                Print_IP()
            Case "follow"
                If _command.Length = 3 Then
                    If Search_PlayerObjB(_command(1)) Then
                        Lock_player = Search_PlayerObj(_command(1))
                        Ratio = _command(2)
                        GUI_mod = True
                        GUITimer.Enabled = True
                    Else
                        Send_Msg_To_Log("Server", "This player isn't online.")
                    End If

                ElseIf _command.Length = 2 Then
                    If _command(1) = "off" Then
                        GUITimer.Enabled = False
                        GUI_mod = False
                        Terminal_Flash()
                    End If
                End If
            Case Else
                Send_Msg_To_Log("Server", "Error command,please type right command in.")
        End Select
    End Sub

    Sub Print_IP()
        Dim _str As String = "There are IP that you can use:  "
        Dim ip() As IPAddress = Dns.GetHostEntry(Dns.GetHostName).AddressList
        For Each it As IPAddress In ip
            If it.AddressFamily = AddressFamily.InterNetwork Then
                _str += "[  " & it.ToString & "  ] , "
                'Send_Msg_To_Log("Server", it.ToString)
            End If
        Next
        _str += "[  127.0.0.1  ] "
        Terminal_NewMsg(_str)
    End Sub

    '跟圖形顯示有關的函式
    Public Sub Terminal_NewMsg(ByVal _str As String)    '傳送一則訊息到畫面
        'Terminal.Text = _str
        Dim i As Integer
        For i = 22 To 1 Step -1
            Logs(i) = Logs(i - 1)
        Next
        Logs(0) = _str

        Terminal_Flash() '刷新終端機介面
    End Sub
    Public Sub Terminal_Flash() '刷新終端機介面
        If GUI_mod = False Then
            Dim GUI As Bitmap
            GUI = New Bitmap(TerminalBox_W, TerminalBox_H)
            Dim drawFont As New Font("Arial", 9)
            Dim drawBrush As New SolidBrush(Color.FromArgb(64, 225, 225))
            Dim drawpen As New Pen(drawBrush)
            Dim g As Graphics
            g = Graphics.FromImage(GUI)
            g.Clear(Color.FromArgb(32, 32, 32))
            drawBrush.Color = Color.FromArgb(64, 225, 225)
            Dim _str1 As String = ""

            For i = 22 To 0 Step -1
                _str1 += Logs(i) & vbNewLine
            Next
            g.DrawString(_str1, drawFont, drawBrush, 1, 1)

            TerminalBox.Image = GUI
        End If
        'SB.Refresh()
    End Sub
End Module
