Imports System.ComponentModel
Imports System.Threading

'Public Class Window1
'    Private My_Thread As Thread
'    Dim r As New Random
'    Delegate Sub My_Delegate(ByVal x As Integer, ByVal Y As Integer)

'    '第一步，开启线程  
'    Private Sub Window1_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
'        My_Thread = New Thread(AddressOf Deal_Thread)
'        My_Thread.Start()
'    End Sub

'    '第二步，进入线程处理程序  
'    Private Sub Deal_Thread()
'        While (True)
'            Dim x As Integer = r.Next(Me.Width - Button1.Width)
'            Dim y As Integer = r.Next(Me.Height - Button1.Height)
'            Invoke_Thread(x, y) '使用委托  
'            Thread.Sleep(2000)
'        End While

'    End Sub

'    '第三步，给委托传递参数，引发委托  
'    Private Sub Invoke_Thread(ByVal x As Integer, ByVal Y As Integer)
'        Dim hander As New My_Delegate(AddressOf Deal_Delegate)
'        hander.Invoke(x, Y)

'    End Sub

'    '第四步，更新窗体中控件  
'    Private Sub Deal_Delegate(ByVal x As Integer, ByVal Y As Integer)
'        Button1.Content = x.ToString + " " + Y.ToString

'    End Sub

'    '最后别忘记关掉线程  
'    Private Sub Window1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
'        My_Thread.Abort()
'    End Sub
'End Class
