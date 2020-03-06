Imports System
Imports System.Windows.Forms
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Module Process_Client
    '連線變數
    Public T As Socket
    Public Th As Thread
    Public Account, Password As String

    Public Online As Boolean = False
    '連線
    Sub send(ByVal Str As String)
        Try
            Dim B() As Byte = Encoding.Default.GetBytes(Str)
            T.Send(B, 0, B.Length, SocketFlags.None)
        Catch ex As Exception
            T.Close()
            Initialize_Login() : MsgBox("已經和伺服器失去連線!")
            Th.Abort()
        End Try
    End Sub
    Public Sub connect()
        Dim ip As String
        Dim port As Integer
        ip = "111.251.178.178" '"111.251.175.228"''"10.0.2.15" '127.0.0.1" ' "111.251.191.8"     '
        port = 7761
        Try
            Dim EP As IPEndPoint = New IPEndPoint(IPAddress.Parse(ip), port)
            T = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            T.Connect(EP)
            Th = New Thread(New ThreadStart(AddressOf Listen))
            Th.IsBackground = True
            Th.Start()
            '已經成功連上伺服器
            send("0" & Account & "|" & Password & "?")
        Catch ex As Exception
            MsgBox("ERROR for can't connect the server!")
            Exit Sub
        End Try

    End Sub
    Sub Listen()
        Dim ServerEP As IPEndPoint = T.RemoteEndPoint
        Dim B(1023) As Byte
        Dim inLen As Integer
        Dim Msg As String
        Do While True
            ' Try
            inLen = T.ReceiveFrom(B, 0, B.Length, SocketFlags.None, ServerEP)
            Msg = Encoding.Default.GetString(B, 0, inLen)
            'Me.Text = Msg
            Dim t_msg() As String = Msg.Split("?")
            For i = 0 To t_msg.Length - 2
                Dim Cmd = t_msg(i).Substring(0, 1)
                Dim Str = t_msg(i).Substring(1)
                Dim _c() As String = Str.Split("|") 'username|數據
                Select Case Cmd
                    Case "c"    '帳密正確 確認連線
                        Initialize_Game(Account, _c(0), _c(1), _c(2), _c(3))
                    Case "E"    '帳密錯誤
                        send("9" & Account & "?")
                        T.Close()
                        MsgBox("帳號或密碼錯誤!")
                        Th.Abort()
                    Case "S"
                        Create_BulletObj(_c(0), _c(1), _c(2), _c(3), _c(4))
                    Case "Y"    '同步封包
                        If (Players(0).X - _c(0)) ^ 2 + (Players(0).Y - _c(1)) ^ 2 > (0.5) ^ 2 Then
                            Players(0).X = _c(0)
                            Players(0).Y = _c(1)
                        End If
                End Select

            Next
            If Msg.Length > 0 Then

            End If

            'Catch ex As Exception
            '    T.Close()
            '    Login_Initialize() : MsgBox("已經和伺服器失去連線!")
            '    Th.Abort()
            'End Try
        Loop
    End Sub
End Module
