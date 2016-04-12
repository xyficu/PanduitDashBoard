Imports System.Data
Imports System.Threading
Imports System.Windows.Shapes


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
    Private defaultColor As Brush = New SolidColorBrush(Colors.LightGray)
    Private timeout1Color As Brush = New SolidColorBrush(Colors.Yellow)
    Private timeout2Color As Brush = New SolidColorBrush(Colors.Red)

    'Public Delegate Sub ParameterizedThreadStart(obj() As Double)


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
            labelCurrentPandiutOrder.Content = drv.Item("Panduit_Order").ToString
            labelStatus.Content = drv.Item("Status").ToString
            labelUrgent.Content = drv.Item("Urgent").ToString
            labelSPANumber.Content = drv.Item("SPA_Number").ToString
            labelLastUser.Content = drv.Item("Last_User").ToString

            outDrv = drv

        Catch ex As Exception
            'MessageBox.Show(ex.Message)

        End Try

    End Sub

    ''' <summary>
    ''' 控制控件闪烁的函数。如果t小于时间节点1，控件为绿色不闪烁状态；如果t在时间节点1和节点2之间控件为黄色闪烁状态；
    ''' 如果t大于时间节点2，控件为红色闪烁状态；不涉及控件为浅灰色状态不闪烁
    ''' </summary>
    ''' <param name="t">两个节点之间的时间间隔</param>
    ''' <param name="thread">控制节点的线程</param>
    ''' <param name="limit1">时间节点1</param>
    ''' <param name="limit2">时间节点2</param>
    ''' <returns></returns>
    Private Overloads Function ControllerBlink(t As TimeSpan, thread As Thread, Optional limit1 As Int32 = 30, Optional limit2 As Int32 = 60)

        Dim timegap As Double = Math.Round(t.TotalMinutes, 1)
        Dim limit() As Double = {timegap, CType(limit1, Double), CType(limit2, Double)}

        'If timegap > limit1 Then
        If thread.ThreadState = ThreadState.Unstarted Then
            thread.Start(limit)
        ElseIf thread.ThreadState = ThreadState.Suspended Then
            thread.Resume()
        End If

        'Else
        '    If thread.ThreadState = ThreadState.WaitSleepJoin Then
        '        thread.Suspend()
        '    End If
        'End If
        Return Nothing
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub DealBarReceiveToLogin(obj As Object)
        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleBarReceiveToLogin(AddressOf BarReceiveToLogin), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleBarReceiveToLogin(AddressOf BarReceiveToLogin), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarReceiveToLogin(AddressOf BarReceiveToLogin), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleBarReceiveToLogin(AddressOf BarReceiveToLogin), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarReceiveToLogin(AddressOf BarReceiveToLogin), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleBarReceiveToLogin(args As Brush)
    Private Sub BarReceiveToLogin(color As Brush)
        BarReceiveToEnter.Fill = color
    End Sub


    Private Sub DealDotLogin(obj As Object)
        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleDotLoginColor(AddressOf DotLoginColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleDotLoginColor(AddressOf DotLoginColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotLoginColor(AddressOf DotLoginColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleDotLoginColor(AddressOf DotLoginColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotLoginColor(AddressOf DotLoginColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleDotLoginColor(color As Brush)
    Private Sub DotLoginColor(color As Brush)
        dotLogin.Fill = color
    End Sub


    Private Sub DealDotPrice(obj As Object)
        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleDotPriceColor(AddressOf DotPriceColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleDotPriceColor(AddressOf DotPriceColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotPriceColor(AddressOf DotPriceColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleDotPriceColor(AddressOf DotPriceColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotPriceColor(AddressOf DotPriceColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleDotPriceColor(color As Brush)
    Private Sub DotPriceColor(color As Brush)
        dotPrice.Fill = color
    End Sub


    Private Sub DealDotBook(obj As Object)
        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleDotBookColor(AddressOf DotBookColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleDotBookColor(AddressOf DotBookColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotBookColor(AddressOf DotBookColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleDotBookColor(AddressOf DotBookColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotBookColor(AddressOf DotBookColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleDotBookColor(color As Brush)
    Private Sub DotBookColor(color As Brush)
        dotBook.Fill = color
    End Sub


    Private Sub DealBarLoginToPrice(obj As Object)
        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleBarLoginToPriceColor(AddressOf BarLoginToPriceColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleBarLoginToPriceColor(AddressOf BarLoginToPriceColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarLoginToPriceColor(AddressOf BarLoginToPriceColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleBarLoginToPriceColor(AddressOf BarLoginToPriceColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarLoginToPriceColor(AddressOf BarLoginToPriceColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleBarLoginToPriceColor(color As Brush)
    Private Sub BarLoginToPriceColor(color As Brush)
        barLoginToPrice.Fill = color
    End Sub



    Private Sub DealBarPriceToBook(obj As Object)
        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleBarPriceToBookColor(AddressOf BarPriceToBookColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleBarPriceToBookColor(AddressOf BarPriceToBookColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarPriceToBookColor(AddressOf BarPriceToBookColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleBarPriceToBookColor(AddressOf BarPriceToBookColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarPriceToBookColor(AddressOf BarPriceToBookColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleBarPriceToBookColor(color As Brush)
    Private Sub BarPriceToBookColor(color As Brush)
        barPriceToBook.Fill = color
    End Sub



    Private Sub DealBarLoginToBook(obj As Object)

        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleBarLoginToBookColor(AddressOf BarLoginToBookColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleBarLoginToBookColor(AddressOf BarLoginToBookColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarLoginToBookColor(AddressOf BarLoginToBookColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleBarLoginToBookColor(AddressOf BarLoginToBookColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarLoginToBookColor(AddressOf BarLoginToBookColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleBarLoginToBookColor(color As Brush)
    Private Sub BarLoginToBookColor(color As Brush)
        barLoginToBook.Fill = color
    End Sub



    Private Sub DealBarBookToCredit(obj As Object)

        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleBarBookToCreditColor(AddressOf BarBookToCreditColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleBarBookToCreditColor(AddressOf BarBookToCreditColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarBookToCreditColor(AddressOf BarBookToCreditColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleBarBookToCreditColor(AddressOf BarBookToCreditColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarBookToCreditColor(AddressOf BarBookToCreditColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleBarBookToCreditColor(color As Brush)
    Private Sub BarBookToCreditColor(color As Brush)
        barBookToCredit.Fill = color
    End Sub



    Private Sub DealDotCredit(obj As Object)

        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleDotCreditColor(AddressOf DotCreditColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleDotCreditColor(AddressOf DotCreditColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotCreditColor(AddressOf DotCreditColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleDotCreditColor(AddressOf DotCreditColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotCreditColor(AddressOf DotCreditColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleDotCreditColor(color As Brush)
    Private Sub DotCreditColor(color As Brush)
        dotCredit.Fill = color
    End Sub


    Private Sub DealBarCreditToMFG(obj As Object)

        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleBarCreditToMFGColor(AddressOf BarCreditToMFGColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleBarCreditToMFGColor(AddressOf BarCreditToMFGColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarCreditToMFGColor(AddressOf BarCreditToMFGColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleBarCreditToMFGColor(AddressOf BarCreditToMFGColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarCreditToMFGColor(AddressOf BarCreditToMFGColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleBarCreditToMFGColor(color As Brush)
    Private Sub BarCreditToMFGColor(color As Brush)
        barCreditToMFG.Fill = color
    End Sub



    Private Sub DealDotMFG(obj As Object)

        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleDotMFGColor(AddressOf DotMFGColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleDotMFGColor(AddressOf DotMFGColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotMFGColor(AddressOf DotMFGColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleDotMFGColor(AddressOf DotMFGColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotMFGColor(AddressOf DotMFGColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleDotMFGColor(color As Brush)
    Private Sub DotMFGColor(color As Brush)
        dotMFG.Fill = color
    End Sub



    Private Sub DealBarMFGToPickRelease(obj As Object)

        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleBarMFGToPickReleaseColor(AddressOf BarMFGToPickReleaseColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleBarMFGToPickReleaseColor(AddressOf BarMFGToPickReleaseColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarMFGToPickReleaseColor(AddressOf BarMFGToPickReleaseColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleBarMFGToPickReleaseColor(AddressOf BarMFGToPickReleaseColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarMFGToPickReleaseColor(AddressOf BarMFGToPickReleaseColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleBarMFGToPickReleaseColor(color As Brush)
    Private Sub BarMFGToPickReleaseColor(color As Brush)
        barMFGToPickRelease.Fill = color
    End Sub



    Private Sub DealBarCreditToPickRelease(obj As Object)

        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleBarCreditToPickReleaseColor(AddressOf BarCreditToPickReleaseColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleBarCreditToPickReleaseColor(AddressOf BarCreditToPickReleaseColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarCreditToPickReleaseColor(AddressOf BarCreditToPickReleaseColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleBarCreditToPickReleaseColor(AddressOf BarCreditToPickReleaseColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarCreditToPickReleaseColor(AddressOf BarCreditToPickReleaseColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleBarCreditToPickReleaseColor(color As Brush)
    Private Sub BarCreditToPickReleaseColor(color As Brush)
        barCreditToPickRelease.Fill = color
    End Sub



    Private Sub DealDotPickRelease(obj As Object)

        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleDotPickReleaseColor(AddressOf DotPickReleaseColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleDotPickReleaseColor(AddressOf DotPickReleaseColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotPickReleaseColor(AddressOf DotPickReleaseColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleDotPickReleaseColor(AddressOf DotPickReleaseColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotPickReleaseColor(AddressOf DotPickReleaseColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleDotPickReleaseColor(color As Brush)
    Private Sub DotPickReleaseColor(color As Brush)
        dotPickRelease.Fill = color
    End Sub



    Private Sub DealBarPickReleaseToReadyToPick(obj As Object)

        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleBarPickReleaseToReadyToPickColor(AddressOf BarPickReleaseToReadyToPickColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleBarPickReleaseToReadyToPickColor(AddressOf BarPickReleaseToReadyToPickColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarPickReleaseToReadyToPickColor(AddressOf BarPickReleaseToReadyToPickColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleBarPickReleaseToReadyToPickColor(AddressOf BarPickReleaseToReadyToPickColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarPickReleaseToReadyToPickColor(AddressOf BarPickReleaseToReadyToPickColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleBarPickReleaseToReadyToPickColor(color As Brush)
    Private Sub BarPickReleaseToReadyToPickColor(color As Brush)
        barPickReleaseToReadyToPick.Fill = color
    End Sub



    Private Sub DealDotReadyToPick(obj As Object)

        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleDotReadyToPickColor(AddressOf DotReadyToPickColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleDotReadyToPickColor(AddressOf DotReadyToPickColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotReadyToPickColor(AddressOf DotReadyToPickColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleDotReadyToPickColor(AddressOf DotReadyToPickColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleDotReadyToPickColor(AddressOf DotReadyToPickColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleDotReadyToPickColor(color As Brush)
    Private Sub DotReadyToPickColor(color As Brush)
        dotReadyToPick.Fill = color
    End Sub



    Private Sub DealBarReadyToPickToCustomerPick(obj As Object)

        Dim limit() As Double = CType(obj, Double())
        If CType(limit(0), Double) < CType(limit(1), Double) Then
            '控件绿色
            Dispatcher.Invoke(New DeleBarReadyToPickToCustomerPickColor(AddressOf BarReadyToPickToCustomerPickColor), passedColor)
        ElseIf CType(limit(0), Double) < CType(limit(2), Double) Then
            '控件黄色闪烁
            While True
                Dispatcher.Invoke(New DeleBarReadyToPickToCustomerPickColor(AddressOf BarReadyToPickToCustomerPickColor), timeout1Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarReadyToPickToCustomerPickColor(AddressOf BarReadyToPickToCustomerPickColor), defaultColor)
                Thread.Sleep(200)
            End While
        ElseIf CType(limit(0), Double) >= CType(limit(2), Double) Then
            '控件红色闪烁
            While True
                Dispatcher.Invoke(New DeleBarReadyToPickToCustomerPickColor(AddressOf BarReadyToPickToCustomerPickColor), timeout2Color)
                Thread.Sleep(200)
                Dispatcher.Invoke(New DeleBarReadyToPickToCustomerPickColor(AddressOf BarReadyToPickToCustomerPickColor), defaultColor)
                Thread.Sleep(200)
            End While
        End If
    End Sub
    Delegate Sub DeleBarReadyToPickToCustomerPickColor(color As Brush)
    Private Sub BarReadyToPickToCustomerPickColor(color As Brush)
        barReadyToPickToCustomerPick.Fill = color
    End Sub



    Private Sub TrainStation_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        threadBarReceiveToLogin = New Thread(New ParameterizedThreadStart(AddressOf DealBarReceiveToLogin))
        threadDotLogin = New Thread(New ParameterizedThreadStart(AddressOf DealDotLogin))
        threadDotPrice = New Thread(New ParameterizedThreadStart(AddressOf DealDotPrice))
        threadDotBook = New Thread(New ParameterizedThreadStart(AddressOf DealDotBook))
        threadBarLoginToPrice = New Thread(New ParameterizedThreadStart(AddressOf DealBarLoginToPrice))
        threadBarPriceToBook = New Thread(New ParameterizedThreadStart(AddressOf DealBarPriceToBook))
        threadBarLoginToBook = New Thread(New ParameterizedThreadStart(AddressOf DealBarLoginToBook))
        threadDotBook = New Thread(New ParameterizedThreadStart(AddressOf DealDotBook))
        threadBarBookToCredit = New Thread(New ParameterizedThreadStart(AddressOf DealBarBookToCredit))
        threadDotCredit = New Thread(New ParameterizedThreadStart(AddressOf DealDotCredit))
        threadBarCreditToMFG = New Thread(New ParameterizedThreadStart(AddressOf DealBarCreditToMFG))
        threadDotMFG = New Thread(New ParameterizedThreadStart(AddressOf DealDotMFG))
        threadBarMFGToPickRelease = New Thread(New ParameterizedThreadStart(AddressOf DealBarMFGToPickRelease))
        threadBarCreditToPickRelease = New Thread(New ParameterizedThreadStart(AddressOf DealBarCreditToPickRelease))
        threadDotPickRelease = New Thread(New ParameterizedThreadStart(AddressOf DealDotPickRelease))
        threadBarPickReleaseToReadyToPick = New Thread(New ParameterizedThreadStart(AddressOf DealBarPickReleaseToReadyToPick))
        threadDotReadyToPick = New Thread(New ParameterizedThreadStart(AddressOf DealDotReadyToPick))
        threadBarReadyToPickToCustomerPick = New Thread(New ParameterizedThreadStart(AddressOf DealBarReadyToPickToCustomerPick))


        BarReceiveToEnter.Fill = New SolidColorBrush(Colors.LightGray)
        dotLogin.Fill = New SolidColorBrush(Colors.LightGray)
        dotPrice.Fill = New SolidColorBrush(Colors.LightGray)
        dotBook.Fill = New SolidColorBrush(Colors.LightGray)
        barLoginToPrice.Fill = New SolidColorBrush(Colors.LightGray)
        barPriceToBook.Fill = New SolidColorBrush(Colors.LightGray)
        barLoginToBook.Fill = New SolidColorBrush(Colors.LightGray)
        barBookToCredit.Fill = New SolidColorBrush(Colors.LightGray)
        dotCredit.Fill = New SolidColorBrush(Colors.LightGray)
        barCreditToMFG.Fill = New SolidColorBrush(Colors.LightGray)
        dotMFG.Fill = New SolidColorBrush(Colors.LightGray)
        barMFGToPickRelease.Fill = New SolidColorBrush(Colors.LightGray)
        barCreditToPickRelease.Fill = New SolidColorBrush(Colors.LightGray)
        dotPickRelease.Fill = New SolidColorBrush(Colors.LightGray)
        barPickReleaseToReadyToPick.Fill = New SolidColorBrush(Colors.LightGray)
        dotReadyToPick.Fill = New SolidColorBrush(Colors.LightGray)
        barReadyToPickToCustomerPick.Fill = New SolidColorBrush(Colors.LightGray)

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
