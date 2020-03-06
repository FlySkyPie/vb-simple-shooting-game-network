'動態物件
Public Class Human
    Public Enable As Boolean
    '遊戲運算
    Public Name As String           '名字
    Public HP As Integer            '血量
    Public MP As Integer            '體力
    Public Gun As Weapon            '武器
    'Public Item(50) As Item

    '行為
    Public Attacking As Boolean     '是否攻擊
    Public Attack_count As Integer  '攻擊延遲計時器
    Public Reload_Count As Integer  '填裝延遲計時器
    Public Moving As Boolean        '是否移動
    'Public Running As Boolean 

    '物理運算   '注意:物理運算距離單位統一使用m(公尺)
    Public X As Single              '所在座標x
    Public Y As Single              '所在座標y
    Public Move_direct As Single    '動量角度
    Public R As Single              '半徑
    Public Angle_W As Double        '視角
    Public Sub Move()
        Dim speed As Single = 0.035 'M per 10m Sec(M/10mS)  '考慮系統運算作出的妥協
        X = X + speed * Math.Cos(Move_direct)
        Y = Y + speed * Math.Sin(Move_direct)
    End Sub
End Class
Public Class Bullet '子彈-飛行物件
    Public Enable As Boolean
    Public Owner As Human           '發射者
    Public X As Single              '
    Public Y As Single
    Public Angle As Double
    Public Speed As Single
    Public Count As Integer         '計時器 (用來決定飛行距離)
    Public Damage As Integer
    Public Sub Move(ByVal _d As Double)
        X = X + Speed * Math.Cos(Angle)
        Y = Y + Speed * Math.Sin(Angle)
    End Sub

End Class

'靜態物件 通常用來當做資料、設定讀取
Public Class Weapon
    Public Name As String           '名稱
    Public Ammo_Type As Ammo_Type   '彈藥類型
    Public Ammo As Integer          '剩餘彈藥
    Public Delay As Integer         '擊發延遲
    Public Shift As Single          '彈道飄移量 (標準差)
    Public ReloadTime As Integer    '填裝時間
End Class
Public Class Ammo_Type
    Public Name As String           '彈藥名稱
    Public Speed As Single          '飛行速度
    Public Damage As Integer        '對人傷害
End Class
Public Class Item
    Public Name As String           '物品名稱
    Public Type As String           '物品類型 (先用字串存,以後需要優化再改成編號)
    Public Value As Integer         '物品值
    Public Explanation As String    '物品的說明文字
End Class