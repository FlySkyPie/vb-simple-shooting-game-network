Imports System.Timers
Module Process_Game
    '遊戲變數
    Dim Max_bullet = Data_Game.Max_bullet
    Dim Max_player = Data_Game.Max_player
    Dim players() = Data_Game.Players
    Dim guns = Data_Game.Guns
    Dim bullet = Data_Game.Bullet




    '這是個天殺的Timer XDD
    Public Timer_Game As System.Timers.Timer
    Public Timer_Graphic As System.Timers.Timer  '繪圖時鐘
    Public Sub Initialize_Timer()
        '初始化遊戲時鐘
        Timer_Game = New System.Timers.Timer
        AddHandler Timer_Game.Elapsed, AddressOf Timer_Game_Tick
        Timer_Game.Interval = 83
        Timer_Game.SynchronizingObject = Form_Game
        Timer_Game.Enabled = False
        '初始化繪圖時鐘
        Timer_Graphic = New System.Timers.Timer
        AddHandler Timer_Graphic.Elapsed, AddressOf Timer_Graphic_Tick
        Timer_Graphic.Interval = 100
        Timer_Graphic.SynchronizingObject = Form_Game
        Timer_Graphic.Enabled = False
    End Sub


    Public Sub Timer_Game_Tick(ByVal sender As System.Object, ByVal e As ElapsedEventArgs)
        send("t" & Account & "|" & players(0).Angle_W & "?")
        players(0).Moving = (key_up Or key_down Or key_right Or key_left)
        If players(0).Moving Then
            Dim _move_x, _move_y As Integer
            _move_x = 0 : _move_y = 0
            If key_up Then _move_y = _move_y - 1
            If key_down Then _move_y = _move_y + 1
            If key_right Then _move_x = _move_x + 1
            If key_left Then _move_x = _move_x - 1
            players(0).Move_direct = Math.Atan2(_move_y, _move_x)
            send("m" & Account & "|" & players(0).Move_direct & "?")
        Else
            send("s" & Account & "?")
        End If

        '遊戲運算
        '除了玩家以外的"人偶"運算
        For i = 1 To Max_player
            If players(i).Enable = True Then
                '移動運算
                If players(i).Moving Then
                    players(i).Move()
                End If
            End If
        Next
        '玩家的運算
        If players(0).Enable = True Then
            '移動運算
            If players(0).Moving Then
                players(0).Move()
            End If
            '開槍運算
            If players(0).Attack_count > 0 Then
                players(0).Attack_count += -1
            End If
            If players(0).Attacking Then
                If players(0).Attack_count = 0 Then '冷卻完畢 開槍
                    'create_bullet(players(0))
                    U_attack(players(0))
                    players(0).Attack_count = players(0).Gun.delay
                End If
            End If
        End If
        '飛行子彈運算
        For i = 0 To Max_bullet
            If bullet(i).Enable Then
                '讓子彈飛~~
                bullet(i).X = bullet(i).X + bullet(i).Speed * Math.Cos(bullet(i).Angle)
                bullet(i).Y = bullet(i).Y + bullet(i).Speed * Math.Sin(bullet(i).Angle)
                '飛太遠啦~銷毀子彈
                bullet(i).count += 1
                If bullet(i).count > 100 Then
                    bullet(i).Enable = False
                End If
                '命中判定
                Dim j As Integer
                For j = 0 To Max_player
                    If players(j).Enable Then
                        Dim rang As Single
                        rang = (players(j).X - bullet(i).X) ^ 2 + (players(j).Y - bullet(i).Y) ^ 2
                        rang = rang ^ 0.5
                        If rang <= players(j).R Then    '命中玩家
                            bullet(i).Enable = False
                            players(j).HP = players(j).HP - bullet(i).damage
                            If players(j).HP <= 0 Then  '死亡判定
                                players(j).Enable = False
                            End If
                            Exit For
                        End If
                    End If
                Next
            End If
        Next
    End Sub
    'Public Sub Initialize_ScreenBox()

    'End Sub

    Public Sub Initialize_Game(ByVal _name As String, ByVal _HP As Integer, ByVal _MP As Integer, ByVal _X As Single, ByVal _Y As Integer)
        '初始化Timer
        Initialize_Timer()
        '初始化ScreenBox
        'Initialize_ScreenBox()
        '載入彈藥種類
        Load_AmmoType()
        '槍種陣列初始化
        Load_Guns()
        '飛行子彈陣列初始化
        Initialize_Bullet()
        '玩家陣列初始化
        Initialize_playerOBJ()
        '設置玩家

        players(0).Name = _name
        players(0).HP = _HP
        players(0).MP = _MP
        players(0).X = _X
        players(0).Y = _Y
        players(0).Gun = New Weapon
        players(0).Gun = guns(0)    '玩家拿著AK47~*
        players(0).Attacking = False
        players(0).Reload_Count = 0
        players(0).Moving = False
        'Players(i).Running = False
        players(0).Attack_count = 0
        players(0).Move_direct = 0
        players(0).R = 0.3              'M
        players(0).Angle_W = 0
        players(0).Enable = True

        'Dim _GB As GroupBox = Form_Game.LoginBox
        LoginBox.Visible = False
        'Form_Game.Visible = True
        Timer_Game.Enabled = True
        Timer_Graphic.Enabled = True
        'MsgBox("遊戲啟動完成")
        Online = True
    End Sub
    Public Sub Initialize_Login()
        Timer_Game.Enabled = False
        Timer_Graphic.Enabled = False
        Form_Game.Visible = False
        'Dim _GB As GroupBox = Form_Game.LoginBox
        LoginBox.Visible = True
        Online = False
    End Sub
    Sub Create_BulletObj(ByVal _owner As String, ByVal _X As Single, ByVal _Y As Single, ByVal _angle As Double, ByVal _gun As String)
        Dim i As Integer
        For i = 0 To Max_bullet
            If bullet(i).Enable = False Then
                bullet(i).Owner = _owner
                bullet(i).X = _X
                bullet(i).Y = _Y
                bullet(i).Angle = _angle
                bullet(i).Speed = Search_Gun(_gun).Ammo_Type.Speed
                bullet(i).Count = 0
                bullet(i).Damage = Search_Gun(_gun).Ammo_Type.Damage
                bullet(i).Enable = True
                Exit For        '跳脫回圈
            End If
        Next
    End Sub
    Public Sub U_attack(ByVal _player As Human)
        Dim _X = _player.X + 0.4 * Math.Cos(_player.Angle_W)
        Dim _Y = _player.Y + 0.4 * Math.Sin(_player.Angle_W)
        Dim _Angle = _player.Angle_W + _player.Gun.Shift * NormSInv(Rnd) '常態分布著彈
        Create_BulletObj(_player.Name, _X, _Y, _Angle, _player.Gun.Name)
        'Dim _d As String = "a" & account & "|" & _Angle & "?"
        'send(_d)
    End Sub
    Public Function NormSInv(ByVal p As Double) As Double   '符合常態分布的亂數
        Const a1 = -39.6968302866538, a2 = 220.946098424521, a3 = -275.928510446969
        Const a4 = 138.357751867269, a5 = -30.6647980661472, a6 = 2.50662827745924
        Const b1 = -54.4760987982241, b2 = 161.585836858041, b3 = -155.698979859887
        Const b4 = 66.8013118877197, b5 = -13.2806815528857, c1 = -0.00778489400243029
        Const c2 = -0.322396458041136, c3 = -2.40075827716184, c4 = -2.54973253934373
        Const c5 = 4.37466414146497, c6 = 2.93816398269878, d1 = 0.00778469570904146
        Const d2 = 0.32246712907004, d3 = 2.445134137143, d4 = 3.75440866190742
        Const p_low = 0.02425, p_high = 1 - p_low
        Dim q As Double, r As Double
        If p < 0 Or p > 1 Then
            Err.Raise(vbObjectError, , "NormSInv: Argument out of range.")
        ElseIf p < p_low Then
            q = (-2 * Math.Log(p)) ^ 0.5
            NormSInv = (((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) / _
               ((((d1 * q + d2) * q + d3) * q + d4) * q + 1)
        ElseIf p <= p_high Then
            q = p - 0.5 : r = q * q
            NormSInv = (((((a1 * r + a2) * r + a3) * r + a4) * r + a5) * r + a6) * q / _
               (((((b1 * r + b2) * r + b3) * r + b4) * r + b5) * r + 1)
        Else
            q = (-2 * Math.Log(1 - p)) ^ 0.5
            NormSInv = -(((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) / _
               ((((d1 * q + d2) * q + d3) * q + d4) * q + 1)
        End If
    End Function
End Module
