Public Class Terminal
    Private Sub Terminal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Randomize()

        CheckForIllegalCrossThreadCalls = False
        Initialize_Timer() '初始化遊戲時鐘
        Initialize_TerminalBox() '初始化Log顯示器
        TerminalBox_W = TerminalBox.Width
        TerminalBox_H = TerminalBox.Height
    End Sub


    '這兩個都是送出指令
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Terminal_Command(CommandBox.Text)
        CommandBox.Text = ""
    End Sub
    Private Sub CommnadBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CommandBox.KeyDown
        If e.KeyValue = Keys.Enter Then
            Terminal_Command(CommandBox.Text)
            CommandBox.Text = ""
        End If
    End Sub
End Class
