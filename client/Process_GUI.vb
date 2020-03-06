Imports System.Timers
Module Process_GUI
    '遊戲變數
    Dim Max_bullet = Data_Game.Max_bullet
    Dim Max_player = Data_Game.Max_player
    Dim players() = Data_Game.Players
    Dim guns = Data_Game.Guns
    Dim bullet = Data_Game.Bullet

    Public ScreenBox As PictureBox
    Public LoginBox As GroupBox
    Public AccountBox, PasswordBox As TextBox
    Public Button_Login As Button
    Public Label_A, Label_P As Label

    '圖形化用戶介面變數
    Public Tq As Date = Now
    Public fps As Single
    Public Mouse_X As Single
    Public Mouse_Y As Single
    Public Mouse_Angle As Double
    Public Mouse_Rang As Single
    'Dim GUI As Bitmap
    'Dim Camera_X As Single      '攝影機座標
    'Dim Camera_Y As Single      '攝影機座標
    Public Ratio As Single = 30
    Public key_up As Boolean
    Public key_right As Boolean
    Public key_down As Boolean
    Public key_left As Boolean

    Dim Camera_X As Single      '攝影機座標
    Dim Camera_Y As Single      '攝影機座標
    Dim DrawTime As Integer = 0
    Public Sub Timer_Graphic_Tick(ByVal sender As System.Object, ByVal e As ElapsedEventArgs)

        If DrawTime >= 10 Then
            If (Now.Minute > Tq.Minute) Then
                fps = 1 / (60 + Now.Second + Now.Millisecond / 1000 - Tq.Second - Tq.Millisecond / 1000)
            Else
                fps = 1 / (Now.Second + Now.Millisecond / 1000 - Tq.Second - Tq.Millisecond / 1000)
            End If
            fps = Int(fps * 10)
            Tq = Now
            DrawTime = 0
        Else
            DrawTime += 1
        End If
        '繪圖
        Dim GUI As Bitmap
        GUI = New Bitmap(ScreenBox.Width, ScreenBox.Height)
        Dim drawFont As New Font("Arial", 9)
        Dim drawBrush As New SolidBrush(Color.FromArgb(64, 225, 225))
        Dim drawpen As New Pen(drawBrush)
        Dim g As Graphics
        g = Graphics.FromImage(GUI)
        g.Clear(Color.FromArgb(32, 32, 32))
        drawBrush.Color = Color.FromArgb(64, 225, 225)
        '-圖像
        '--定位攝影機
        Camera_X = players(0).X + Mouse_Rang / Ratio / 2 * Math.Cos(players(0).Angle_W)
        Camera_Y = players(0).Y + Mouse_Rang / Ratio / 2 * Math.Sin(players(0).Angle_W)
        '--攝影機看得到的範圍
        Dim _Y_min, _Y_max, _X_min, _X_max As Single
        _X_min = Camera_X - ScreenBox.Width / 2 / Ratio
        _X_max = Camera_X + ScreenBox.Width / 2 / Ratio
        _Y_min = Camera_Y - ScreenBox.Height / 2 / Ratio
        _Y_max = Camera_Y + ScreenBox.Height / 2 / Ratio
        '---繪製地圖線
        drawpen.Color = Color.FromArgb(16, 64, 70)
        Dim hh, Sx, Sy As Integer
        Sx = ScreenBox.Width / 2
        Sy = ScreenBox.Height / 2
        hh = Int((Fix(Camera_X) - Camera_X) * Ratio)
        Dim tw, th As Integer
        tw = Int(ScreenBox.Width / Ratio / 2)
        th = Int(ScreenBox.Height / Ratio / 2)
        For k = -tw To tw
            Dim pp1 As Point = New Point(Sx + hh + k * Ratio, 0)
            Dim pp2 As Point = New Point(Sx + hh + k * Ratio, ScreenBox.Height)
            g.DrawLine(drawpen, pp1, pp2)
        Next
        hh = Int((Fix(Camera_Y) - Camera_Y) * Ratio)
        For k = -th To th
            Dim pp1 As Point = New Point(0, Sy + hh + k * Ratio)
            Dim pp2 As Point = New Point(ScreenBox.Width, Sy + hh + k * Ratio)
            g.DrawLine(drawpen, pp1, pp2)
        Next
        '---繪製玩家
        drawpen.Color = Color.FromArgb(64, 255, 255)
        Dim i As Integer
        For i = 0 To Max_player
            If players(i).Enable Then '玩家物件使用中
                If (players(i).X > _X_min And players(i).X < _X_max) And (players(i).Y > _Y_min And players(i).Y < _Y_max) Then '繪製範圍內 開始繪圖
                    Dim c_x, c_y As Integer
                    c_x = Int(ScreenBox.Width / 2) + (players(i).X - Camera_X) * Ratio
                    c_y = Int(ScreenBox.Height / 2) + (players(i).Y - Camera_Y) * Ratio
                    Dim _R As Single = players(0).R * Ratio
                    g.DrawEllipse(drawpen, c_x - _R, c_y - _R, _R * 2, _R * 2)
                    Dim p1 As Point = New Point(c_x, c_y)
                    Dim p2 As Point = New Point(c_x + Int(_R * 2 * Math.Cos(players(i).Angle_W)), c_y + Int(_R * 2 * Math.Sin(players(i).Angle_W)))
                    g.DrawLine(drawpen, p1, p2)
                End If
            End If
        Next
        '---繪製子彈
        For i = 0 To Max_bullet
            If bullet(i).Enable Then
                If (bullet(i).X > _X_min And bullet(i).X < _X_max) And (bullet(i).Y > _Y_min And bullet(i).Y < _Y_max) Then '繪製範圍內 開始繪圖
                    Dim c_x, c_y As Integer
                    c_x = Int(ScreenBox.Width / 2) + (bullet(i).X - Camera_X) * Ratio
                    c_y = Int(ScreenBox.Height / 2) + (bullet(i).Y - Camera_Y) * Ratio
                    'g.DrawEllipse(drawpen, c_x, c_y, 2, 2)
                    Dim p1 As Point = New Point(c_x, c_y)
                    Dim p2 As Point = New Point(c_x - Int(0.15 * Ratio * Math.Cos(bullet(i).Angle)), c_y - Int(0.15 * Ratio * Math.Sin(bullet(i).Angle)))
                    g.DrawLine(drawpen, p1, p2)
                End If
            End If
        Next
        '-文字
        drawFont = New Font("Arial", 9)
        Dim Stmp As String
        Stmp = "=====GUI=====" & vbNewLine
        Stmp += "FPS: " & fps & vbNewLine
        Stmp += "(" & Mouse_X & "," & Mouse_Y & "," & Mouse_Angle & ")" & vbNewLine
        Stmp += key_up & key_right & key_down & key_left & vbNewLine
        Stmp += "Name:" & players(0).Name & vbNewLine
        Stmp += "HP:" & players(0).HP & vbNewLine
        Stmp += "MP:" & players(0).MP & vbNewLine
        Stmp += "R:" & players(0).R & vbNewLine
        Stmp += "Location:(" & players(0).X & "," & players(0).Y & ")" & vbNewLine
        Stmp += "Moving Direcion:" & players(0).Move_direct & vbNewLine
        Stmp += "body Direcion:" & players(0).Angle_W & vbNewLine
        Stmp += "attacking:" & players(0).Attacking & "/" & players(0).Attack_count & vbNewLine
        Stmp += "Weapon:" & players(0).Gun.Name & vbNewLine
        g.DrawString(Stmp, drawFont, drawBrush, 1, 1)
        ScreenBox.Image = GUI
        'Else
        'DrawTime += 1
        'End If



    End Sub
End Module
