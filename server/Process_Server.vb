Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text


Public Module Process_Server
    '引入變數和常數們
    Dim Max_bullet = Data_Game.Max_bullet
    Dim Max_player = Data_Game.Max_player
    Dim Pi = Data_Game.Pi
    Dim Players = Data_Game.Players
    Dim Bullet = Data_Game.Bullet
    Dim Guns = Data_Game.Guns
    Dim Ammo_Type = Data_Game.Ammo_Type
    '連線用變數
    Public Server As TcpListener
    Public Client As Socket
    Public Th_Svr As Thread
    Public Th_Clt As Thread
    Public HT As New Hashtable
    Public server_ip As String      'server's ip
    Public Server_Online As Boolean '伺服器是否上線中

    Dim Syn_count As Integer = 0 'Clien-Server同步時機
    '引入和表單的指標
    Dim LB1 As ListBox = Terminal.PlayersList


    '伺服器運算====================================================================================================
    Sub Start_sever()       '啟動伺服器
        '遊戲設定
        Load_AmmoType() : Send_Msg_To_Log("Server", "Load ammos data....")
        Load_Guns() : Send_Msg_To_Log("Server", "Load guns data....")
        Initialize_Bullet() : Send_Msg_To_Log("Server", "Initialize bullet object....")
        Initialize_playerOBJ() : Send_Msg_To_Log("Server", "Initialize player object....")
        GameTimer.Enabled = True : Send_Msg_To_Log("Server", "Turn timer on....")
        '連線設定
        Th_Svr = New Thread(New ThreadStart(AddressOf serverSub))
        Th_Svr.IsBackground = True
        Th_Svr.Start() : Send_Msg_To_Log("Server", "Starting server proccess....")
        Server_Online = True
        Try                         '除錯用
            If Th_Svr.IsAlive Then Send_Msg_To_Log("Server", "Server start successful!")
            If Th_Svr.IsAlive Then Send_Msg_To_Log("Server", "Server's ip is " & server_ip)
        Catch ex As Exception
            Send_Msg_To_Log("ERROR", "Server start fail!")
        End Try
    End Sub
    Sub serverSub()
        Dim port As Integer
        port = 7761
        Dim EP As New IPEndPoint(IPAddress.Parse(server_ip), port)
        Server = New TcpListener(EP)
        Server.Start(Max_player)
        Do While True
            Client = Server.AcceptSocket
            Th_Clt = New Thread(New ThreadStart(AddressOf Listen))
            Th_Clt.IsBackground = True
            Th_Clt.Start()
        Loop
    End Sub
    Sub Listen()
        Dim Sck As Socket = Client
        Dim Th As Thread = Th_Clt
        Do While True
            'Try    '用來防止Clint端意外關閉
            Dim B(2013) As Byte
            Dim inLen As Integer = Sck.Receive(B)
            Dim msg As String = Encoding.Default.GetString(B, 0, inLen)
            'Send_Msg_To_Log("S", msg)
            Dim t_msg() As String = msg.Split("?")
            For i = 0 To t_msg.Length - 2
                Dim Cmd As String = t_msg(i).Substring(0, 1)
                Dim Str As String = t_msg(i).Substring(1)
                Dim _c() As String = Str.Split("|") 'username|數據
                Select Case Cmd
                    Case "0"    '連線
                        If HT.ContainsKey(_c(0)) = False Then
                            HT.Add(_c(0), Sck)
                            LB1.Items.Add(_c(0))
                            Create_PlayerObj(_c(0))
                            Dim d As String = "c" & Search_PlayerObj(_c(0)).HP & "|" & Search_PlayerObj(_c(0)).MP & "|" & Search_PlayerObj(_c(0)).X & "|" & Search_PlayerObj(_c(0)).Y & "?"
                            SendTo(d, _c(0))    '進行第一次同步
                            Send_Msg_To_Log("Game", _c(0) & " had login" & "(" & Sck.RemoteEndPoint.ToString & ")")
                        End If
                    Case "9"    '離線
                        HT.Remove(Str)
                        LB1.Items.Remove(Str)
                        Recycle_PlayerObj(Str)
                        Send_Msg_To_Log("Game", Str & " had logout")
                        Sck.Close()
                        Th.Abort() '執行緒被關掉 下面的東西不會被執行 吧?
                    Case Else
                        Game_Command(Cmd, _c)
                End Select
            Next
            'Catch er As Exception
            '    Send_Msg_To_Log("E", "Something wrong!")
            'End Try
        Loop
    End Sub

    Public Sub Game_Command(ByVal _cmd As String, ByVal _data() As String) '解析指令
        Select Case _cmd
            Case "m"    '移動
                Search_PlayerObj(_data(0)).Move_direct = _data(1)
                Search_PlayerObj(_data(0)).Moving = True ': Send_Msg_To_Log("G", _c(0) & " are moving.")
            Case "s"    '停止移動
                Search_PlayerObj(_data(0)).Moving = False ': Send_Msg_To_Log("G", _c(0) & " stop moving.")
            Case "a"    '射擊
                'create_bullet(Search_PlayerObj(_c(0)), _c(1))
                If Not Search_PlayerObj(_data(0)).Attacking Then
                    Search_PlayerObj(_data(0)).Attacking = True : Send_Msg_To_Log("Game", _data(0) & " are attacking.")
                End If
            Case "q"    '停止射擊
                If Search_PlayerObj(_data(0)).Attacking Then
                    Search_PlayerObj(_data(0)).Attacking = False : Send_Msg_To_Log("Game", _data(0) & " just stop attacking.")
                End If
            Case "t"    '身體角度
                Players(0).Angle_W = _data(1)
        End Select
    End Sub
    Public Sub SendTo(ByVal str As String, ByVal user As String) '送出指令給Client
        Dim B As Byte() = Encoding.Default.GetBytes(str)
        Dim Sck As Socket = HT(user)
        Sck.Send(B, 0, B.Length, SocketFlags.None)
    End Sub
    Public Sub Sync_Work()  '和Client進行同步
        If Syn_count < 10 Then
            Syn_count += 1
        Else    '開始進行同步
            Syn_count = 0
            For i = 0 To Max_player
                If Players(i).Enable Then
                    Dim _d As String = "Y" & Players(i).X & "|" & Players(i).Y & "?"
                    SendTo(_d, Players(i).Name)
                End If
            Next
        End If
    End Sub
    '報告伺服器消息給終端
    Public Sub Send_Msg_To_Log(ByVal _type As String, ByVal _Str As String)
        Dim _t, send As String
        _t = Now.Year & "/" & Format(Now.Month, "00") & "/" & Format(Now.Day, "00") & vbTab & Format(Now.Hour, "00") & ":" & Format(Now.Minute, "00") & ":" & Format(Now.Second, "00") & "." & Format(Now.Millisecond, "000")
        send = "[" & _type & "]" & vbTab & _t & vbTab & _Str
        Terminal_NewMsg(send)
    End Sub
End Module
