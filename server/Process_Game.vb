Imports System.Timers
Public Module Process_Game
    '引入變數和常數們
    Dim Max_bullet = Data_Game.Max_bullet
    Dim Max_player = Data_Game.Max_player
    Dim Pi = Data_Game.Pi

    Dim Players = Data_Game.Players
    Dim Bullet = Data_Game.Bullet
    Dim Guns = Data_Game.Guns
    Dim Ammo_Type = Data_Game.Ammo_Type

    '這是個天殺的Timer XDD
    Public GameTimer, GUITimer As System.Timers.Timer
    Public Sub Initialize_Timer() '初始化遊戲時鐘
        GameTimer = New System.Timers.Timer
        AddHandler GameTimer.Elapsed, AddressOf GameTimer_Tick
        GameTimer.Interval = 83
        GameTimer.SynchronizingObject = Terminal
        'GameTimer.Enabled = True

        GUITimer = New System.Timers.Timer
        AddHandler GUITimer.Elapsed, AddressOf GUITimer_Tick 'ameTimer_Tick
        GUITimer.Interval = 100
        GUITimer.SynchronizingObject = Terminal
        GUITimer.Enabled = False
    End Sub
    Public Sub GameTimer_Tick(ByVal sender As System.Object, ByVal e As ElapsedEventArgs)
        Sync_Work()
        Dim i As Integer
        For i = 0 To Max_player
            If Players(i).Enable = True Then
                '移動運算
                If Players(i).Moving Then
                    Players(i).Move()
                End If
                '開槍運算
                If Players(i).Attack_count > 0 Then
                    Players(i).Attack_count += -1
                End If

                If Players(i).Attacking Then
                    If Players(i).Attack_count = 0 Then '冷卻完畢 開槍
                        Create_BulletObj(Players(i))
                        Players(i).Attack_count = Players(i).Gun.Delay
                        'players(i).Attacking = False    '
                    End If
                End If
            End If
        Next

        '飛行子彈運算
        For i = 0 To Max_bullet
            If Bullet(i).Enable Then
                '讓子彈飛~~
                Bullet(i).X = Bullet(i).X + Bullet(i).Speed * Math.Cos(Bullet(i).Angle)
                Bullet(i).Y = Bullet(i).Y + Bullet(i).Speed * Math.Sin(Bullet(i).Angle)
                '飛太遠啦~銷毀子彈
                Bullet(i).Count += 1
                If Bullet(i).Count > 100 Then
                    Bullet(i).Enable = False

                End If
                '命中判定
                Dim j As Integer
                For j = 0 To Max_player
                    If Players(j).Enable Then
                        Dim rang As Single
                        rang = (Players(j).X - Bullet(i).X) ^ 2 + (Players(j).Y - Bullet(i).Y) ^ 2
                        rang = rang ^ 0.5
                        If rang <= Players(j).R Then    '命中玩家
                            Bullet(i).Enable = False
                            Players(j).HP = Players(j).HP - Bullet(i).Damage
                            If Players(j).HP <= 0 Then  '死亡判定
                                Players(j).Enable = False
                            End If
                            Exit For
                        End If
                    End If
                Next
            End If
        Next
    End Sub

    Public Sub Recycle_PlayerObj(ByVal _name As String)    '銷毀指定名稱的玩家物件
        Dim i As Integer
        For i = 0 To Max_player
            If Players(i).Enable And Players(i).Name = _name Then
                Players(i).Enable = False
                Exit For
            End If
        Next
    End Sub
    Public Function Search_PlayerObj(ByVal _Name As String) As Human   '找到指定名稱的player物件並回傳
        Dim i As Integer
        For i = 0 To Max_player
            If Players(i).Enable And Players(i).Name = _Name Then
                Return Players(i)
                Exit For
            End If
        Next
    End Function

    Public Function Search_PlayerObjB(ByVal _Name As String) As Boolean    '判斷指定名稱的玩家物件存不存在
        Search_PlayerObjB = False
        Dim i As Integer
        For i = 0 To Max_player
            If Players(i).Enable And Players(i).Name = _Name Then
                Return True
                Exit For
            End If
        Next
    End Function

    '創造動態物件
    Public Sub Create_PlayerObj(ByVal _name As String)
        Dim i As Integer
        For i = 0 To Max_player
            If Players(i).Enable = False Then
                Players(i).Name = _name
                Players(i).HP = 100
                Players(i).MP = 100
                Players(i).Gun = New Weapon
                Players(i).Gun = Guns(0)        '玩家拿著AK47~*
                Players(i).Attacking = False
                Players(i).Reload_Count = 0
                Players(i).Moving = False
                'Players(i).Running = False
                Players(i).Attack_count = 0
                Players(i).X = Int(Rnd() * 200 - 100)
                Players(i).Y = Int(Rnd() * 200 - 100)
                Players(i).Move_direct = 0
                Players(i).R = 0.3              'M
                Players(i).Angle_W = 0
                Players(i).Enable = True
                Exit For
            End If
        Next
    End Sub
    Sub Create_BulletObj(ByVal _shooter As Human)
        Dim i As Integer
        For i = 0 To Max_bullet
            If Bullet(i).Enable = False Then
                Bullet(i).Owner = _shooter
                Bullet(i).X = _shooter.X + 0.4 * Math.Cos(_shooter.Angle_W)
                Bullet(i).Y = _shooter.Y + 0.4 * Math.Sin(_shooter.Angle_W)
                Bullet(i).Angle = _shooter.Angle_W + _shooter.Gun.Shift * NormSInv(Rnd) '常態分布著彈
                Bullet(i).Speed = _shooter.Gun.Ammo_Type.Speed
                Bullet(i).Count = 0
                Bullet(i).Damage = _shooter.Gun.Ammo_Type.Damage
                Bullet(i).Enable = True
                Exit For        '跳脫回圈
            End If
        Next
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
