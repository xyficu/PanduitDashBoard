Imports System.Data
Imports System.Threading



Public Class TrainStation
    Private threadBarReceiveToLogin As Thread
    Private threadDotLogin As Thread
    Private threadBarLoginToPrice As Thread
    Private threadDotPrice As Thread
    Private threadBarPriceToBook As Thread
    Private threadBarLoginToBook As Thread
    Private threadDotBook As Thread
    Private threadBarBookToCredit As Thread
    Private threadDotCredit As Thread
    Private threadBarCreditToMFG As Thread
    Private threadDotMFG As Thread
    Private threadBarMFGToPickRelease As Thread
    Private threadBarCreditToPickRelease As Thread
    Private threadDotPickRelease As Thread
    Private threadBarPickReleaseToReadyToPick As Thread
    Private threadDotReadyToPick As Thread
    Private threadBarReadyToPickToCustomerPick As Thread

    Private oriColorBarReceiveToLogin As Brush
    Private oriColorDotLogin As Brush
    Private oriColorDotPrice As Brush
    Private oriColorDotBook As Brush
    Private oriColorBarLoginToPrice As Brush
    Private oriColorBarPriceToBook As Brush
    Private oriColorBarLoginToBook As Brush
    Private oriColorBarBookToCredit As Brush
    Private oriColorDotCredit As Brush
    Private oriColorBarCreditToMFG As Brush
    Private oriColorDotMFG As Brush
    Private oriColorBarMFGToPickRelease As Brush
    Private oriColorBarCreditToPickRelease As Brush
    Private oriColorDotPickRelease As Brush
    Private oriColorBarPickReleaseToReadyToPick As Brush
    Private oriColorDotReadyToPick As Brush
    Private oriColorBarReadyToPickToCustomerPick As Brush

    Private curDotLogin As Brush
    Private curBarLoginToPrice As Brush
    Private curDotPrice As Brush
    Private curBarPriceToBook As Brush
    Private curDotBook As Brush

    Private passedColor As Brush = New SolidColorBrush(Colors.Green)
    Private failedColor As Brush = New SolidColorBrush(Colors.Gray)
    Private timeout1Color As Brush = New SolidColorBrush(Colors.Yellow)
    Private timeout2Color As Brush = New SolidColorBrush(Colors.Red)


    Private outDrv As DataRowView


    Public Sub New(ByRef drv As DataRowView)

        InitializeComponent()

        Try
            Dim StartTime, EndTime As New DateTime
            StartTime = drv.Item("Receive_Order_Time").ToString
            EndTime = DateTime.Now

            Dim time4 As TimeSpan = EndTime - StartTime
            labelPriceSubLoginTime.Content = Math.Round((time4.TotalHours), 1).ToString() + " Hours"

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

    Private Function ControllerBlink(t As TimeSpan, thread As Thread, limit1 As Int32, Optional limit2 As Int32 = 60)
        '小于limit1
        '大于limit1,小于limit2
        '大于limit2
        If Math.Round(t.TotalMinutes, 1) > limit1 Then
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

    Dim args() As Brush = New Brush() {New SolidColorBrush(Colors.Blue), New SolidColorBrush(Colors.Yellow)}
    Private Sub DealBarReceiveToLogin()
        While True
            Dispatcher.Invoke(New DeleBarReceiveToLogin(AddressOf BarReceiveToLogin), args(0))
            Thread.Sleep(200)
            Dispatcher.Invoke(New DeleBarReceiveToLogin(AddressOf BarReceiveToLogin), args(1))
            Thread.Sleep(200)
        End While
    End Sub

    Delegate Sub DeleBarReceiveToLogin(args As Brush)
    Private Sub BarReceiveToLogin(args As Brush)
        BarReceiveToEnter.Fill = args
    End Sub
    Private Sub BarReceiveToLogin1(args() As Object)

        BarReceiveToEnter.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub BarReceiveToLogin2()
        BarReceiveToEnter.Fill = New SolidColorBrush(Colors.Yellow)
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

    Private Sub DealBarBookToCredit()
        While True
            Dispatcher.Invoke(AddressOf BarBookToCredit1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf BarBookTocredit2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub BarBookToCredit1()
        barBookToCredit.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub BarBookTocredit2()
        barBookToCredit.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealDotCredit()
        While True
            Dispatcher.Invoke(AddressOf DotCredit1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf Dotcredit2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub Dotcredit1()
        dotCredit.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub Dotcredit2()
        dotCredit.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealBarCreditToMFG()
        While True
            Dispatcher.Invoke(AddressOf BarCreditToMFG1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf BarcreditToMFG2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub BarCreditToMFG1()
        barCreditToMFG.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub BarCreditToMFG2()
        barCreditToMFG.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealDotMFG()
        While True
            Dispatcher.Invoke(AddressOf DotMFG1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf DotMFG2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub DotMFG1()
        dotMFG.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub DotMFG2()
        dotMFG.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealBarMFGToPickRelease()
        While True
            Dispatcher.Invoke(AddressOf BarMFGToPickRelease1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf BarMFGToPickRelease2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub BarMFGToPickRelease1()
        barMFGToPickRelease.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub BarMFGToPickRelease2()
        barMFGToPickRelease.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealBarCreditToPickRelease()
        While True
            Dispatcher.Invoke(AddressOf BarCreditToPickRelease1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf BarCreditToPickRelease2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub BarCreditToPickRelease1()
        barCreditToPickRelease.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub BarCreditToPickRelease2()
        barCreditToPickRelease.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealDotPickRelease()
        While True
            Dispatcher.Invoke(AddressOf DotPickRelease1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf DOtPickRelease2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub DotPickRelease1()
        dotPickRelease.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub DotPickRelease2()
        dotPickRelease.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealBarPickReleaseToReadyToPick()
        While True
            Dispatcher.Invoke(AddressOf BarPickReleaseToReadyToPick1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf BarPickReleaseToReadyToPick2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub BarPickReleaseToReadyToPick1()
        barPickReleaseToReadyToPick.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub BarPickReleaseToReadyToPick2()
        barPickReleaseToReadyToPick.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealDotReadyToPick()
        While True
            Dispatcher.Invoke(AddressOf DotReadyToPick1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf DotReadyToPick2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub DotReadyToPick1()
        dotReadyToPick.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub DotReadyToPick2()
        dotReadyToPick.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub DealBarReadyToPickToCustomerPick()
        While True
            Dispatcher.Invoke(AddressOf BarReadyToPickToCustomerPick1)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf BarReadyToPickToCustomerPick2)
            Thread.Sleep(200)
        End While
    End Sub
    Private Sub BarReadyToPickToCustomerPick1()
        barReadyToPickToCustomerPick.Fill = New SolidColorBrush(Colors.Blue)
    End Sub
    Private Sub BarReadyToPickToCustomerPick2()
        barReadyToPickToCustomerPick.Fill = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub TrainStation_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        threadBarReceiveToLogin = New Thread(AddressOf DealBarReceiveToLogin)
        threadDotLogin = New Thread(AddressOf DealDotLogin)
        threadDotPrice = New Thread(AddressOf DealDotPrice)
        threadDotBook = New Thread(AddressOf DealDotBook)
        threadBarLoginToPrice = New Thread(AddressOf DealBarLoginToPrice)
        threadBarPriceToBook = New Thread(AddressOf DealBarPriceToBook)
        threadBarLoginToBook = New Thread(AddressOf DealBarLoginToBook)
        threadDotBook = New Thread(AddressOf DealDotBook)
        threadBarBookToCredit = New Thread(AddressOf DealBarBookToCredit)
        threadDotCredit = New Thread(AddressOf DealDotCredit)
        threadBarCreditToMFG = New Thread(AddressOf DealBarCreditToMFG)
        threadDotMFG = New Thread(AddressOf DealDotMFG)
        threadBarMFGToPickRelease = New Thread(AddressOf DealBarMFGToPickRelease)
        threadBarCreditToPickRelease = New Thread(AddressOf DealBarCreditToPickRelease)
        threadDotPickRelease = New Thread(AddressOf DealDotPickRelease)
        threadBarPickReleaseToReadyToPick = New Thread(AddressOf DealBarPickReleaseToReadyToPick)
        threadDotReadyToPick = New Thread(AddressOf DealDotReadyToPick)
        threadBarReadyToPickToCustomerPick = New Thread(AddressOf DealBarReadyToPickToCustomerPick)

        oriColorBarReceiveToLogin = BarReceiveToEnter.Fill
        oriColorDotLogin = dotLogin.Fill
        oriColorDotPrice = dotPrice.Fill
        oriColorDotBook = dotBook.Fill
        oriColorBarLoginToPrice = barLoginToPrice.Fill
        oriColorBarPriceToBook = barPriceToBook.Fill
        oriColorBarLoginToBook = barLoginToBook.Fill
        oriColorBarBookToCredit = barBookToCredit.Fill
        oriColorDotCredit = dotCredit.Fill
        oriColorBarCreditToMFG = barCreditToMFG.Fill
        oriColorDotMFG = dotMFG.Fill
        oriColorBarMFGToPickRelease = barMFGToPickRelease.Fill
        oriColorBarCreditToPickRelease = barCreditToPickRelease.Fill
        oriColorDotPickRelease = dotPickRelease.Fill
        oriColorBarPickReleaseToReadyToPick = barPickReleaseToReadyToPick.Fill
        oriColorDotReadyToPick = dotReadyToPick.Fill
        oriColorBarReadyToPickToCustomerPick = barReadyToPickToCustomerPick.Fill

        't1 = outDrv.Item("Login_Order_Time").ToString
        't2 = outDrv.Item("Send_To_Pricing_Time").ToString
        't3 = outDrv.Item("Price_Modify_Time").ToString
        't4 = outDrv.Item("Price_Send_Back_Time").ToString
        't5 = outDrv.Item("Book_Order_Time").ToString
        't6 = t5

        Dim dr As DataRowView = outDrv
        Dim t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, tm As New DateTime

        If dr.Item("Tag").ToString = "SPANewColor" Then

            If dr.Item("Status").ToString = "Price Request" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Send_To_Pricing_Time").ToString
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(tm - t2, threadBarLoginToPrice, 240)

            ElseIf dr.Item("Status").ToString = "Price Modified" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Send_To_Pricing_Time").ToString
                t3 = dr.Item("Price_Modify_Time").ToString
                t4 = dr.Item("Price_Send_Back_Time").ToString
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(t3 - t2, threadBarLoginToPrice, 240)
                ControllerBlink(t4 - t3, threadDotPrice, 60)
                ControllerBlink(tm - t4, threadBarPriceToBook, 15)

            ElseIf dr.Item("Status").ToString = "Booked" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Send_To_Pricing_Time").ToString
                t3 = dr.Item("Price_Modify_Time").ToString
                t4 = dr.Item("Price_Send_Back_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time")
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(t3 - t2, threadBarLoginToPrice, 240)
                ControllerBlink(t4 - t3, threadDotPrice, 60)
                ControllerBlink(t5 - t4, threadBarPriceToBook, 15)
                ControllerBlink(t5 - t4, threadDotBook, 15)

                If t6.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t5, threadBarBookToCredit, 60)
                Else
                    ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                    ControllerBlink(tm - t6, threadDotCredit, 1440)
                End If
                '检查Credit
            ElseIf dr.Item("Status").ToString = "Credit" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Send_To_Pricing_Time").ToString
                t3 = dr.Item("Price_Modify_Time").ToString
                t4 = dr.Item("Price_Send_Back_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(t3 - t2, threadBarLoginToPrice, 240)
                ControllerBlink(t4 - t3, threadDotPrice, 60)
                ControllerBlink(t5 - t4, threadBarPriceToBook, 15)
                ControllerBlink(t5 - t4, threadDotBook, 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)


                '检查在工厂的订单
            ElseIf dr.Item("Status").ToString = "Manufacturing" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Send_To_Pricing_Time").ToString
                t3 = dr.Item("Price_Modify_Time").ToString
                t4 = dr.Item("Price_Send_Back_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                t8 = dr.Item("MFG_Start_Time").ToString
                t9 = dr.Item("MFG_Complete_Time").ToString
                t10 = dr.Item("WH_Start_Time")
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(t3 - t2, threadBarLoginToPrice, 240)
                ControllerBlink(t4 - t3, threadDotPrice, 60)
                ControllerBlink(t5 - t4, threadBarPriceToBook, 15)
                ControllerBlink(t5 - t4, threadDotBook, 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)
                ControllerBlink(t8 - t7, threadBarCreditToMFG, 2880)
                ControllerBlink(t9 - t8, threadDotMFG, 2880)

                If t10.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t9, threadBarMFGToPickRelease, 2160)
                Else
                    ControllerBlink(t10 - t9, threadBarMFGToPickRelease, 2160)
                    ControllerBlink(tm - t10, threadDotPickRelease, 20)
                End If

                '仓库Pick Release
            ElseIf dr.Item("Status").ToString = "Warehouse Pick Release" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Send_To_Pricing_Time").ToString
                t3 = dr.Item("Price_Modify_Time").ToString
                t4 = dr.Item("Price_Send_Back_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                t8 = dr.Item("MFG_Start_Time")
                t10 = dr.Item("WH_Start_Time").ToString
                t11 = dr.Item("WH_Complete_Time").ToString
                t12 = dr.Item("Ready_To_Pick_ST")
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(t3 - t2, threadBarLoginToPrice, 240)
                ControllerBlink(t4 - t3, threadDotPrice, 60)
                ControllerBlink(t5 - t4, threadBarPriceToBook, 15)
                ControllerBlink(t5 - t4, threadDotBook, 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)
                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(t10 - t7, threadBarCreditToPickRelease, 2160)
                    ControllerBlink(t11 - t10, threadDotPickRelease, 20)

                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    ControllerBlink(t8 - t7, threadBarCreditToMFG, 2880)
                    ControllerBlink(t9 - t8, threadDotMFG, 2880)
                    ControllerBlink(t10 - t9, threadBarMFGToPickRelease, 2160)
                    ControllerBlink(t11 - t10, threadDotPickRelease, 20)
                End If
                If t12.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t11, threadBarPickReleaseToReadyToPick, 240)
                Else
                    ControllerBlink(t12 - t11, threadBarPickReleaseToReadyToPick, 240)
                    ControllerBlink(tm - t12, threadDotReadyToPick, 2160)
                End If
                '检查是否备货完全
            ElseIf dr.Item("Status").ToString = "Ready For Pick" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Send_To_Pricing_Time").ToString
                t3 = dr.Item("Price_Modify_Time").ToString
                t4 = dr.Item("Price_Send_Back_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                t8 = dr.Item("MFG_Start_Time")
                t10 = dr.Item("WH_Start_Time").ToString
                t11 = dr.Item("WH_Complete_Time").ToString
                t12 = dr.Item("Ready_To_Pick_ST").ToString
                t13 = dr.Item("Ready_To_Pick_CT").ToString
                t14 = dr.Item("Customer_Pick_Time")
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(t3 - t2, threadBarLoginToPrice, 240)
                ControllerBlink(t4 - t3, threadDotPrice, 60)
                ControllerBlink(t5 - t4, threadBarPriceToBook, 15)
                ControllerBlink(t5 - t4, threadDotBook, 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)

                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(t10 - t7, threadBarCreditToPickRelease, 2160)
                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    ControllerBlink(t8 - t7, threadBarCreditToMFG, 2880)
                    ControllerBlink(t9 - t8, threadDotMFG, 2880)
                    ControllerBlink(t10 - t9, threadBarMFGToPickRelease, 2160)
                End If
                ControllerBlink(t11 - t10, threadDotPickRelease, 20)
                ControllerBlink(t12 - t11, threadBarPickReleaseToReadyToPick, 240)
                ControllerBlink(t13 - t12, threadDotReadyToPick, 2160)
                If t14.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t14, threadBarReadyToPickToCustomerPick, 7200)
                Else
                    ControllerBlink(t14 - t13, threadBarReadyToPickToCustomerPick, 7200)
                End If
            End If
            'NormalOrder表格
        ElseIf dr.Item("Tag").ToString = "NormalOrder" Then

            If dr.Item("Status").ToString = "Booked" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time")
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t5 - t1, threadDotLogin, 15)
                ControllerBlink(t5 - t1, threadBarLoginToBook, 15)
                ControllerBlink(t5 - t1, threadDotBook, 15)
                If t6.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t5, threadBarBookToCredit, 60)
                Else
                    ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                    ControllerBlink(tm - t6, threadDotCredit, 1440)
                End If

                '检查Credit
            ElseIf dr.Item("Status").ToString = "Credit" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t5 - t1, threadDotLogin, 15)
                ControllerBlink(t5 - t1, threadBarLoginToBook, 15)
                ControllerBlink(t5 - t1, threadDotBook, 15)
                ControllerBlink(t5 - t4, threadBarPriceToBook, 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)

                '检查在工厂的订单
            ElseIf dr.Item("Status").ToString = "Manufacturing" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                t8 = dr.Item("MFG_Start_Time").ToString
                t9 = dr.Item("MFG_Complete_Time").ToString
                t10 = dr.Item("WH_Start_Time")
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t5 - t1, threadDotLogin, 15)
                ControllerBlink(t5 - t1, threadBarLoginToBook, 15)
                ControllerBlink(t5 - t1, threadDotBook, 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)
                ControllerBlink(t8 - t7, threadBarCreditToMFG, 2880)
                ControllerBlink(t9 - t8, threadDotMFG, 2880)

                If t10.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t9, threadBarMFGToPickRelease, 2160)
                Else
                    ControllerBlink(t10 - t9, threadBarMFGToPickRelease, 2160)
                    ControllerBlink(tm - t10, threadDotPickRelease, 20)
                End If

                '仓库Pick Release
            ElseIf dr.Item("Status").ToString = "Warehouse Pick Release" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                t8 = dr.Item("MFG_Start_Time")
                t10 = dr.Item("WH_Start_Time").ToString
                t11 = dr.Item("WH_Complete_Time").ToString
                t12 = dr.Item("Ready_To_Pick_ST")
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t5 - t1, threadDotLogin, 15)
                ControllerBlink(t5 - t1, threadBarLoginToBook, 15)
                ControllerBlink(t5 - t1, threadDotBook, 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)
                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(t10 - t7, threadBarCreditToPickRelease, 2160)
                    ControllerBlink(t11 - t10, threadDotPickRelease, 20)

                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    ControllerBlink(t8 - t7, threadBarCreditToMFG, 2880)
                    ControllerBlink(t9 - t8, threadDotMFG, 2880)
                    ControllerBlink(t10 - t9, threadBarMFGToPickRelease, 2160)
                    ControllerBlink(t11 - t10, threadDotPickRelease, 20)
                End If
                If t12.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t11, threadBarPickReleaseToReadyToPick, 240)
                Else
                    ControllerBlink(t12 - t11, threadBarPickReleaseToReadyToPick, 240)
                    ControllerBlink(tm - t12, threadDotReadyToPick, 2160)
                End If

                '检查是否备货完全
            ElseIf dr.Item("Status").ToString = "Ready For Pick" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                t8 = dr.Item("MFG_Start_Time")
                t10 = dr.Item("WH_Start_Time").ToString
                t11 = dr.Item("WH_Complete_Time").ToString
                t12 = dr.Item("Ready_To_Pick_ST").ToString
                t13 = dr.Item("Ready_To_Pick_CT").ToString
                t14 = dr.Item("Customer_Pick_Time")
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t5 - t1, threadDotLogin, 15)
                ControllerBlink(t5 - t1, threadBarLoginToBook, 15)
                ControllerBlink(t5 - t1, threadDotBook, 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)

                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(t10 - t7, threadBarCreditToPickRelease, 2160)
                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    ControllerBlink(t8 - t7, threadBarCreditToMFG, 2880)
                    ControllerBlink(t9 - t8, threadDotMFG, 2880)
                    ControllerBlink(t10 - t9, threadBarMFGToPickRelease, 2160)
                End If
                ControllerBlink(t11 - t10, threadDotPickRelease, 20)
                ControllerBlink(t12 - t11, threadBarPickReleaseToReadyToPick, 240)
                ControllerBlink(t13 - t12, threadDotReadyToPick, 2160)
                If t14.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t14, threadBarReadyToPickToCustomerPick, 7200)
                Else
                    ControllerBlink(t14 - t13, threadBarReadyToPickToCustomerPick, 7200)
                End If
            End If

            'SPAExpire表格
        ElseIf dr.Item("Tag").ToString = "SPAExpire" Then
            Dim t21 As New TimeSpan
            If dr.Item("Status").ToString = "Pending" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Pending_Time").ToString
                tm = DateTime.Now()
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(tm - t2, threadBarLoginToBook, 7200)


            ElseIf dr.Item("Status").ToString = "Booked" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Pending_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time")
                tm = DateTime.Now()
                t21 = t2 - t1
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(t5 - t2, threadBarLoginToBook, 7200)
                ControllerBlink(t5 - t1, threadDotBook, Convert.ToInt32(t21.TotalMinutes) + 15)
                If t6.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t5, threadBarBookToCredit, 60)
                Else
                    t6 = dr.Item("Credit_Check_Time").ToString
                    ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                    ControllerBlink(tm - t6, threadDotCredit, 1440)
                End If

                '检查Credit
            ElseIf dr.Item("Status").ToString = "Credit" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Pending_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                tm = DateTime.Now()
                t21 = t2 - t1
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(t5 - t2, threadBarLoginToBook, 7200)
                ControllerBlink(t5 - t1, threadDotBook, Convert.ToInt32(t21.TotalMinutes) + 15)
                ControllerBlink(t5 - t4, threadBarPriceToBook, 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)


                '检查在工厂的订单
            ElseIf dr.Item("Status").ToString = "Manufacturing" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Pending_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                t8 = dr.Item("MFG_Start_Time").ToString
                t9 = dr.Item("MFG_Complete_Time").ToString
                t10 = dr.Item("WH_Start_Time")
                tm = DateTime.Now()
                t21 = t2 - t1
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(t5 - t2, threadBarLoginToBook, 7200)
                ControllerBlink(t5 - t1, threadDotBook, Convert.ToInt32(t21.TotalMinutes) + 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)
                ControllerBlink(t8 - t7, threadBarCreditToMFG, 2880)
                ControllerBlink(t9 - t8, threadDotMFG, 2880)

                If t10.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t9, threadBarMFGToPickRelease, 2160)
                Else
                    ControllerBlink(t10 - t9, threadBarMFGToPickRelease, 2160)
                    ControllerBlink(tm - t10, threadDotPickRelease, 20)
                End If

                '仓库Pick Release
            ElseIf dr.Item("Status").ToString = "Warehouse Pick Release" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Pending_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                t8 = dr.Item("MFG_Start_Time")
                t10 = dr.Item("WH_Start_Time").ToString
                t11 = dr.Item("WH_Complete_Time").ToString
                t12 = dr.Item("Ready_To_Pick_ST")
                tm = DateTime.Now()
                t21 = t2 - t1
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(t5 - t2, threadBarLoginToBook, 7200)
                ControllerBlink(t5 - t1, threadDotBook, Convert.ToInt32(t21.TotalMinutes) + 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)
                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(t10 - t7, threadBarCreditToPickRelease, 2160)
                    ControllerBlink(t11 - t10, threadDotPickRelease, 20)

                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    ControllerBlink(t8 - t7, threadBarCreditToMFG, 2880)
                    ControllerBlink(t9 - t8, threadDotMFG, 2880)
                    ControllerBlink(t10 - t9, threadBarMFGToPickRelease, 2160)
                    ControllerBlink(t11 - t10, threadDotPickRelease, 20)
                End If
                If t12.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t11, threadBarPickReleaseToReadyToPick, 240)
                Else
                    ControllerBlink(t12 - t11, threadBarPickReleaseToReadyToPick, 240)
                    ControllerBlink(tm - t12, threadDotReadyToPick, 2160)
                End If

                '检查是否备货完全
            ElseIf dr.Item("Status").ToString = "Ready For Pick" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Pending_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                t8 = dr.Item("MFG_Start_Time")
                t10 = dr.Item("WH_Start_Time").ToString
                t11 = dr.Item("WH_Complete_Time").ToString
                t12 = dr.Item("Ready_To_Pick_ST").ToString
                t13 = dr.Item("Ready_To_Pick_CT").ToString
                t14 = dr.Item("Customer_Pick_Time")
                tm = DateTime.Now()
                t21 = t2 - t1
                ControllerBlink(t1 - t0, threadBarReceiveToLogin, 15)
                ControllerBlink(t2 - t1, threadDotLogin, 15)
                ControllerBlink(t5 - t2, threadBarLoginToBook, 7200)
                ControllerBlink(t5 - t1, threadDotBook, Convert.ToInt32(t21.TotalMinutes) + 15)
                ControllerBlink(t6 - t5, threadBarBookToCredit, 60)
                ControllerBlink(t7 - t6, threadDotCredit, 1440)

                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(t10 - t7, threadBarCreditToPickRelease, 2160)
                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    ControllerBlink(t8 - t7, threadBarCreditToMFG, 2880)
                    ControllerBlink(t9 - t8, threadDotMFG, 2880)
                    ControllerBlink(t10 - t9, threadBarMFGToPickRelease, 2160)
                End If
                ControllerBlink(t11 - t10, threadDotPickRelease, 20)
                ControllerBlink(t12 - t11, threadBarPickReleaseToReadyToPick, 240)
                ControllerBlink(t13 - t12, threadDotReadyToPick, 2160)
                If t14.ToString("HH:mm:ss") = "00:00:00" Then
                    ControllerBlink(tm - t14, threadBarReadyToPickToCustomerPick, 7200)
                Else
                    ControllerBlink(t14 - t13, threadBarReadyToPickToCustomerPick, 7200)
                End If
            End If
        End If
    End Sub

    Private Sub TrainStation_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        AbortThread(threadBarReceiveToLogin)
        AbortThread(threadDotLogin)
        AbortThread(threadDotPrice)
        AbortThread(threadDotBook)
        AbortThread(threadBarLoginToPrice)
        AbortThread(threadBarPriceToBook)
        AbortThread(threadBarLoginToBook)
        AbortThread(threadBarBookToCredit)
        AbortThread(threadDotCredit)
        AbortThread(threadBarCreditToMFG)
        AbortThread(threadDotMFG)
        AbortThread(threadBarMFGToPickRelease)
        AbortThread(threadBarCreditToPickRelease)
        AbortThread(threadDotPickRelease)
        AbortThread(threadBarPickReleaseToReadyToPick)
        AbortThread(threadDotReadyToPick)
        AbortThread(threadBarReadyToPickToCustomerPick)
    End Sub

    Private Function AbortThread(thread As Thread)
        If thread.ThreadState = ThreadState.Suspended Then
            thread.Resume()
        End If
        thread.Abort()
        Return Nothing
    End Function
End Class
