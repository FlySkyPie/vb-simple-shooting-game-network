Module Data_Game
    '遊戲運算需要的全域常數
    Public Const Max_player = 50        '最大玩家數
    Public Const Max_bullet = 100       '最大子彈數
    Public Const Pi = 3.141592653589

    '變數類型(動態)
    Public Players(Max_player) As Human
    Public Bullet(Max_bullet) As Bullet
    '資料類型(靜態)
    Public Guns(99) As Weapon           '槍枝類型
    Public Ammo_Type(99) As Ammo_Type   '彈藥類型
    Public Items(99) As Item            '物品類型

    Public Sub Load_Guns()  '載入槍枝資料 (初始化)
        Dim i As Integer
        For i = 0 To 99
            guns(i) = New Weapon
        Next
        guns(0).Name = "AK-47"              '名稱
        guns(0).Ammo_Type = Ammo_type(0)    '彈藥類型
        guns(0).Ammo = 30                   '(預設)剩餘彈藥
        guns(0).Delay = 2                   '擊發延遲
        guns(0).Shift = Pi / 90             '彈道飄移量 (標準差) '2度
    End Sub
    Public Sub Load_AmmoType()  '載入彈藥資料(初始化)
        Dim i As Integer
        For i = 0 To 99
            Ammo_type(i) = New Ammo_Type
        Next
        Ammo_type(0).Name = "7.62x39"
        Ammo_type(0).Speed = 1.1
        Ammo_type(0).Damage = 11
    End Sub

    Public Sub Initialize_Bullet()  '初始化飛行子陣列
        Dim i As Integer
        For i = 0 To Max_bullet
            Bullet(i) = New Bullet
            Bullet(i).Enable = False
        Next
    End Sub
    Sub Initialize_playerOBJ()      '初始化玩家陣列
        Dim i As Integer
        For i = 0 To Max_player
            Players(i) = New Human
            Players(i).Enable = False
        Next
    End Sub

End Module