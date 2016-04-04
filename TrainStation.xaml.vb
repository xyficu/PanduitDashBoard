Imports System.Data
Imports System.Threading



Public Class TrainStation
    Private threadDotLogin As Thread
    Private threadDotPrice As Thread
    Private threadDotBook As Thread
    Private threadBarLoginToPrice As Thread
    Private threadBarPriceToBook As Thread
    Private threadBarLoginToBook As Thread

    Private oriColorDotLogin As Brush
    Private oriColorDotPrice As Brush
    Private oriColorDotBook As Brush
    Private oriColorBarLoginToPrice As Brush
    Private oriColorBarPriceToBook As Brush
    Private oriColorBarLoginToBook As Brush

    Private curDotLogin As Brush
    Private curBarLoginToPrice As Brush
    Private curDotPrice As Brush
    Private curBarPriceToBook As Brush
    Private curDotBook As Brush

    Private outDrv As DataRowView


    Public Sub New(ByRef drv As DataRowView)

        InitializeComponent()


        Try
            Dim loginTime, sendToPriceTime As New DateTime
            loginTime = drv.Item("Login_Order_Time").ToString
            sendToPriceTime = drv.Item("Send_To_Pricing_Time").ToString

            Dim time4 As TimeSpan = sendToPriceTime - loginTime
            labelPriceSubLoginTime.Content = Math.Round((time4.TotalMinutes), 1).ToString() + " Minutes"

            labelCustomerId.Content = drv.Item("Customer_ID").ToString
            labelCustomerOrder.Content = drv.Item("Customer_Order").ToString
            labelPanduitOrder.Content = drv.Item("Panduit_Order").ToString
            labelStatus.Content = drv.Item("Status").ToString
            labelUrgent.Content = drv.Item("Urgent").ToString
            labelSPANumber.Content = drv.Item("SPA_Number").ToString
            labelLastUser.Content = drv.Item("Last_User").ToString

            outDrv = drv



        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Function ControllerBlink(t As TimeSpan, thread As Thread)
        If Math.Round(t.TotalMinutes, 1) > 10 Then
            If thread.ThreadState = ThreadState.Unstarted Then
                thread.Start()
            ElseIf thread.ThreadState = ThreadState.Suspended Then
                thread.Resume()
            End If
        Else
            If thread.ThreadState = ThreadState.WaitSleepJoin Then
                thread.Suspend()
            End If
        End If
        Return Nothing
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub



    Private Sub DealDotLogin()
        While True
            Dispatcher.Invoke(AddressOf DotLoginColor1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf DotLoginColor2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub DotLoginColor1()
        dotLogin.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub DotLoginColor2()
        dotLogin.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealDotPrice()
        While True
            Dispatcher.Invoke(AddressOf DotPriceColor1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf DotPriceColor2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub DotPriceColor1()
        dotPrice.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub DotPriceColor2()
        dotPrice.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealDotBook()
        While True
            Dispatcher.Invoke(AddressOf DotBookColor1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf DotBookColor2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub DotBookColor1()
        dotBook.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub DotBookColor2()
        dotBook.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealBarLoginToPrice()
        While True
            Dispatcher.Invoke(AddressOf BarLoginToPriceColor1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf BarLoginToPriceColor2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub BarLoginToPriceColor1()
        barLoginToPrice.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub BarLoginToPriceColor2()
        barLoginToPrice.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealBarPriceToBook()
        While True
            Dispatcher.Invoke(AddressOf BarPriceToBookColor1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf BarPriceToBookColor2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub BarPriceToBookColor1()
        barPriceToBook.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub BarPriceToBookColor2()
        barPriceToBook.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealBarLoginToBook()
        While True
            Dispatcher.Invoke(AddressOf BarLoginToBookColor1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf BarLoginToBookColor2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub BarLoginToBookColor1()
        barLoginToBook.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub BarLoginToBookColor2()
        barLoginToBook.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub TrainStation_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        threadDotLogin = New Thread(AddressOf DealDotLogin)
        threadDotPrice = New Thread(AddressOf DealDotPrice)
        threadDotBook = New Thread(AddressOf DealDotBook)
        threadBarLoginToPrice = New Thread(AddressOf DealBarLoginToPrice)
        threadBarPriceToBook = New Thread(AddressOf DealBarPriceToBook)
        threadBarLoginToBook = New Thread(AddressOf DealBarLoginToBook)

        oriColorDotLogin = dotLogin.Fill
        oriColorDotPrice = dotPrice.Fill
        oriColorDotBook = dotBook.Fill
        oriColorBarLoginToPrice = barLoginToPrice.Fill
        oriColorBarPriceToBook = barPriceToBook.Fill
        oriColorBarLoginToBook = barLoginToBook.Fill

        Dim t1, t2, t3, t4, t5, t6 As New DateTime
        t1 = outDrv.Item("Login_Order_Time").ToString
        t2 = outDrv.Item("Send_To_Pricing_Time").ToString
        t3 = outDrv.Item("Price_Modify_Time").ToString
        t4 = outDrv.Item("Price_Send_Back_Time").ToString
        t5 = outDrv.Item("Book_Order_Time").ToString
        t6 = t5

        ControllerBlink(t2 - t1, threadDotLogin)
        ControllerBlink(t3 - t2, threadBarLoginToPrice)
        ControllerBlink(t4 - t3, threadDotPrice)
        ControllerBlink(t5 - t4, threadBarPriceToBook)
        ControllerBlink(t6 - t1, threadDotBook)

    End Sub

    Private Sub TrainStation_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        AbortThread(threadDotLogin)
        AbortThread(threadDotPrice)
        AbortThread(threadDotBook)
        AbortThread(threadBarLoginToPrice)
        AbortThread(threadBarPriceToBook)
        AbortThread(threadBarLoginToBook)
    End Sub

    Private Function AbortThread(thread As Thread)
        If thread.ThreadState = ThreadState.Suspended Then
            thread.Resume()
        End If
        thread.Abort()
        Return Nothing
    End Function
End Class
