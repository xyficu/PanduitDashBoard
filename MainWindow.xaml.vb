Imports System.Data
Imports System.Threading
Imports System.Windows.Threading


Class MainWindow
    Private trainStation As TrainStation
    Private db As New DBHelper
    Private timer As New Timers.Timer()
    Dim dtUrgent, dtBreach As DataTable
    Dim dtNotBooked As DataTable
    Private threadAutoScrollDataGrid As Thread
    Private threadLabelBreachedBlink As Thread
    Private threadLabelUrgentBlink As Thread
    Private threadUpdateDB As Thread

    Private oriLabelBreachedColor As Brush
    Private oriLabelUrgentColor As Brush

    Private timerUpdateDB As Timer

    '设置是否需要更新数据库
    Private needUpdateDB As Boolean = True
    '设定数据库更新时间
    Private updateDBInterval As Integer = 10
    '设定滚动时间
    Private rollOverInterval As Integer = 1

    '窗体加载时填充datagrid
    Private Sub PanduitDashboardMain_Loaded(sender As Object, e As RoutedEventArgs) Handles PanduitDashboardMain.Loaded
        FillDataGrid()

    End Sub

    '填充datagrid
    Private Function FillDataGrid()

        Try
            'fill urgent orders
            dtUrgent = New DataTable
            dtBreach = New DataTable
            db.GetUrgentOrders(dtUrgent)


            If dtUrgent IsNot Nothing Then

                Dim binding = New Binding()
                binding.Source = dtUrgent.DefaultView
                dataGridUrgent.SetBinding(DataGrid.ItemsSourceProperty, binding)

                '隐藏不需要的列
                dataGridUrgent.Columns(0).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(1).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(2).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(6).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(7).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(8).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(10).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(11).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(12).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(13).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(14).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(15).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(16).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(17).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(18).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(19).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(20).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(21).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(22).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(23).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(24).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(25).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(26).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(27).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(28).Visibility = Visibility.Hidden
                dataGridUrgent.Columns(29).Visibility = Visibility.Hidden
            End If
            db.GetBreachedOrders(dtBreach)
            If dtBreach IsNot Nothing Then

                Dim binding = New Binding()
                binding.Source = dtBreach.DefaultView
                dataGridBreach.SetBinding(DataGrid.ItemsSourceProperty, binding)

                '隐藏不需要的列
                dataGridBreach.Columns(0).Visibility = Visibility.Hidden
                dataGridBreach.Columns(1).Visibility = Visibility.Hidden
                dataGridBreach.Columns(2).Visibility = Visibility.Hidden
                dataGridBreach.Columns(6).Visibility = Visibility.Hidden
                dataGridBreach.Columns(7).Visibility = Visibility.Hidden
                dataGridBreach.Columns(8).Visibility = Visibility.Hidden
                dataGridBreach.Columns(10).Visibility = Visibility.Hidden
                dataGridBreach.Columns(11).Visibility = Visibility.Hidden
                dataGridBreach.Columns(12).Visibility = Visibility.Hidden
                dataGridBreach.Columns(13).Visibility = Visibility.Hidden
                dataGridBreach.Columns(14).Visibility = Visibility.Hidden
                dataGridBreach.Columns(15).Visibility = Visibility.Hidden
                dataGridBreach.Columns(16).Visibility = Visibility.Hidden
                dataGridBreach.Columns(17).Visibility = Visibility.Hidden
                dataGridBreach.Columns(18).Visibility = Visibility.Hidden
                dataGridBreach.Columns(19).Visibility = Visibility.Hidden
                dataGridBreach.Columns(20).Visibility = Visibility.Hidden
                dataGridBreach.Columns(21).Visibility = Visibility.Hidden
                dataGridBreach.Columns(22).Visibility = Visibility.Hidden
                dataGridBreach.Columns(23).Visibility = Visibility.Hidden
                dataGridBreach.Columns(24).Visibility = Visibility.Hidden
                dataGridBreach.Columns(25).Visibility = Visibility.Hidden
                dataGridBreach.Columns(26).Visibility = Visibility.Hidden
                dataGridBreach.Columns(27).Visibility = Visibility.Hidden
                dataGridBreach.Columns(28).Visibility = Visibility.Hidden
                dataGridBreach.Columns(29).Visibility = Visibility.Hidden
                'Dim row As DataGridRow = dataGridTimeout.ItemContainerGenerator.ContainerFromIndex(0)
                'row.Background = New SolidColorBrush(Colors.Blue)

            End If

            GetTotalStatistics()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try


        Return Nothing
    End Function


    Private Sub DealThread()
        While True

            Dispatcher.Invoke(AddressOf RollOrdersBreach)
            Dispatcher.Invoke(AddressOf RollOrdersUrgent)
            Thread.Sleep(1000 * rollOverInterval)

        End While
    End Sub

    '延迟1秒方法
    Private Sub WaitSeconds(ByVal s As Integer)
        Dim tmpNow As Date = Now
        While Now.Subtract(tmpNow).Seconds < s
            tmpNow.AddSeconds(1)
        End While
    End Sub


    '滚动显示datagrid breach条目
    Private Function RollOrdersBreach()

        Try
            If dataGridBreach.Items.Count > 0 Then
                Dim index As Int32 = dataGridBreach.SelectedIndex
                dataGridBreach.SelectedIndex = (index + 1) Mod dataGridBreach.Items.Count
                dataGridBreach.Focus()
                CheckCurrentOrderTimeBreached()
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.ToString)
        End Try

        Return Nothing

    End Function

    '滚动显示datagrid urgent条目
    Private Function RollOrdersUrgent()

        Try
            If dataGridUrgent.Items.Count > 0 Then
                Dim index As Int32 = dataGridUrgent.SelectedIndex
                dataGridUrgent.SelectedIndex = (index + 1) Mod dataGridUrgent.Items.Count
                dataGridUrgent.Focus()
                CheckCurrentOrderTimeUrgent()
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.ToString)
        End Try

        Return Nothing

    End Function

    Private Function UpdateTrainStationBreached(ByRef lasttime As DateTime, ByVal limit As Int32)
        Dim dr As DataRowView
        'Dim dt As New DataTable
        'db.GetBreachedOrders(dt)
        dr = dtBreach.DefaultView.Item(dataGridBreach.SelectedIndex)
        Dim loginTime As New DateTime
        loginTime = dr.Item("Receive_Order_Time")
        Dim t4 As TimeSpan = lasttime - loginTime
        labelPriceSubLoginTimeBreached.Content = Math.Round((t4.TotalHours), 1).ToString() + " Hours"
        '变回原色
        labelPriceSubLoginTimeBreached.Background = oriLabelBreachedColor
        '如果t4超时label就变色，如果不超时就变回原色
        If Math.Round((t4.TotalHours), 1) > limit Then
            If threadLabelBreachedBlink.ThreadState = ThreadState.Unstarted Then
                threadLabelBreachedBlink.Start()
            ElseIf threadLabelBreachedBlink.ThreadState = ThreadState.Suspended Then
                threadLabelBreachedBlink.Resume()
            End If
        Else
            If threadLabelBreachedBlink.ThreadState = ThreadState.WaitSleepJoin Then
                threadLabelBreachedBlink.Suspend()
            End If
        End If


        Return Nothing
    End Function
    'Label变色
    Private Function UpdateTrainStationUrgent(ByRef lasttime As DateTime, ByVal limit As Int32)
        Dim dr As DataRowView
        Dim dt As New DataTable
        'db.GetUrgentOrders(dt)
        dr = dtUrgent.DefaultView.Item(dataGridUrgent.SelectedIndex)

        Dim loginTime, priceTime As New DateTime
        loginTime = dr.Item("Receive_Order_Time")

        Dim t4 As TimeSpan = lasttime - loginTime
        labelPriceSubLoginTimeUrgent.Content = Math.Round((t4.TotalHours), 1).ToString() + " Hours"

        '变回原色
        labelPriceSubLoginTimeUrgent.Background = oriLabelUrgentColor
        '如果t4超时label就变色，如果不超时就停止变色
        If Math.Round((t4.TotalHours), 1) > limit Then
            If threadLabelUrgentBlink.ThreadState = ThreadState.Unstarted Then
                threadLabelUrgentBlink.Start()
            ElseIf threadLabelUrgentBlink.ThreadState = ThreadState.Suspended Then
                threadLabelUrgentBlink.Resume()
            End If
        Else
            If threadLabelUrgentBlink.ThreadState = ThreadState.WaitSleepJoin Then
                threadLabelUrgentBlink.Suspend()
            End If
        End If


        Return Nothing
    End Function


    Private Sub dataGridUrgent_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles dataGridUrgent.SelectionChanged
        'scroll to the selected item
        dotStarturgent.Fill = New SolidColorBrush(Colors.Green)
        BarReceiveToEnterUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        dotLoginUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        barLoginToBookUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        barLoginToPriceUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        dotPriceUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        barPriceToBookUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        dotBookUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        BarBookToCreditUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        dotCreditUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        BarCreditToPickReleaseUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        BarCreditToMFGUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        dotMFGUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        BarMFGToPickReleaseUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        dotPickReleaseUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        BarPickReleaseToReadyToPickUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        dotReadyToPickUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        BarReadyToPickToCustomerPickUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        dotCustomerPickUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        barEndUrgent.Fill = New SolidColorBrush(Colors.LightGray)
        If dataGridUrgent.Items.Count > 0 Then
            dataGridUrgent.ScrollIntoView(dataGridUrgent.Items(dataGridUrgent.SelectedIndex))
            UpdateTrainStationUrgent(DateTime.Now(), 2400)
            CheckCurrentOrderTimeUrgent()
        End If

    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        threadAutoScrollDataGrid = New Thread(AddressOf DealThread)
        threadLabelBreachedBlink = New Thread(AddressOf DealBreachedBlink)
        threadLabelUrgentBlink = New Thread(AddressOf DealUrgentBlink)
        'threadUpdateDB = New Thread(AddressOf DealUpdateDatagridContent)

        oriLabelBreachedColor = labelPriceSubLoginTimeBreached.Background
        oriLabelUrgentColor = labelPriceSubLoginTimeUrgent.Background

        If dataGridUrgent IsNot Nothing Then
            dataGridUrgent.SelectedIndex = 0
            dataGridUrgent.Focus()

            dataGridBreach.SelectedIndex = 0

        End If

        If needUpdateDB = True Then
            timerUpdateDB = New Timer(AddressOf DealUpdateDatagridContent, Nothing, 1000 * updateDBInterval, 1000 * updateDBInterval)
        End If

    End Sub

    'start auto-scroll
    Private Sub buttonAutoScroll_Click(sender As Object, e As RoutedEventArgs) Handles buttonAutoScroll.Click
        Try
            If threadAutoScrollDataGrid.ThreadState = ThreadState.Unstarted Then
                threadAutoScrollDataGrid.Start()
                buttonAutoScroll.Content = "Stop Auto-Scroll"

            ElseIf threadAutoScrollDataGrid.ThreadState = ThreadState.Suspended Then
                threadAutoScrollDataGrid.Resume()
                buttonAutoScroll.Content = "Stop Auto-Scroll"

            ElseIf threadAutoScrollDataGrid.ThreadState = ThreadState.WaitSleepJoin Then
                threadAutoScrollDataGrid.Suspend()
                buttonAutoScroll.Content = "Start Auto-Scroll"

            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)

        End Try


    End Sub

    Private Function UpdateDatagridContent()
        Try
            Dim curIndexUrgent As Int32 = dataGridUrgent.SelectedIndex
            Dim curIndexBreached As Int32 = dataGridBreach.SelectedIndex
            dtUrgent.Rows.Clear()
            db.GetUrgentOrders(dtUrgent)
            dataGridUrgent.SelectedIndex = curIndexUrgent
            dtBreach.Rows.Clear()
            db.GetBreachedOrders(dtBreach)
            dataGridBreach.SelectedIndex = curIndexBreached
        Catch ex As Exception
            'MessageBox.Show(ex.ToString)
        End Try
        Return Nothing
    End Function

    Private Sub DealUpdateDatagridContent(ByVal state As Object)
        Thread.Sleep(1000 * updateDBInterval)
        Dispatcher.Invoke(AddressOf UpdateDatagridContent)

    End Sub

    Private Sub dataGridUrgent_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles dataGridUrgent.MouseUp
        Dim drv As DataRowView
        drv = dataGridUrgent.SelectedItem
        trainStation = New TrainStation(selectedDrUrgent)
        trainStation.ShowDialog()
    End Sub

    'check order, change color when timeout
    Private selectedDrBreached As DataRowView
    Private Function CheckCurrentOrderTimeBreached()

        If dataGridBreach.SelectedIndex >= 0 Then
            selectedDrBreached = dtBreach.DefaultView.Item(dataGridBreach.SelectedIndex)
            ChangeColor(selectedDrBreached, "breach")
            labelCurrentPandiutOrderBreached.Content = selectedDrBreached.Item("Panduit_Order").ToString
        End If

        Return Nothing
    End Function

    'check order, change color when timeout
    Private selectedDrUrgent As DataRowView
    Private Function CheckCurrentOrderTimeUrgent()

        If dataGridUrgent.SelectedIndex >= 0 Then
            selectedDrUrgent = dtUrgent.DefaultView.Item(dataGridUrgent.SelectedIndex)
            ChangeColor(selectedDrUrgent, "urgent")
            labelCurrentPandiutOrderUrgent.Content = selectedDrUrgent.Item("Panduit_Order").ToString
        End If

        Return Nothing
    End Function

    'change color depend on dot time
    Private Function CheckColor(t1 As DateTime, t2 As DateTime, yellowtime As Int32, redtime As Int32)
        Dim t As TimeSpan = t2 - t1

        Dim f As Double = t.TotalMinutes
        If f <= 0 Then
            Return New SolidColorBrush(Colors.Gray)
        ElseIf f > 0 And f <= yellowtime Then
            Return New SolidColorBrush(Colors.Green)
        ElseIf f > yellowtime And f <= redtime Then
            Return New SolidColorBrush(Colors.Yellow)
        ElseIf f > redtime Then
            Return New SolidColorBrush(Colors.Red)
        End If

        Return New SolidColorBrush(Colors.White)
    End Function

    Private Function GetTotalStatistics()
        Dim totalCount As Int32 = 0
        Dim totalFailedCount As Int32 = 0
        Dim prictCount As Int32 = 0
        Dim priceFailedCount As Int32 = 0
        Dim bookCount As Int32 = 0
        Dim bookFailedCount As Int32 = 0
        db.GetTotalOrderFailCount(totalCount, totalFailedCount,
                                  prictCount, priceFailedCount,
                                  bookCount, bookFailedCount)

        labelTotalOrderBreachSum.Content = totalFailedCount.ToString()
        labelTotalOrderSum.Content = totalCount.ToString()
        labelTotalPriceSum.Content = priceFailedCount.ToString()
        labelTotalPriceBreachSum.Content = prictCount.ToString()
        labelTotalBookBreachSum.Content = bookFailedCount.ToString()
        labelTotalBookSum.Content = bookCount.ToString()


        Return Nothing
    End Function

    Private Sub dataGridTimeout_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles dataGridBreach.MouseUp

        trainStation = New TrainStation(selectedDrBreached)
        trainStation.ShowDialog()

    End Sub

    Private Sub MainWindow_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Thread.Sleep(1000 * rollOverInterval)
        AbortThread(threadAutoScrollDataGrid)

    End Sub

    Private Sub AbortThread(thread As Thread)
        Try

            If threadAutoScrollDataGrid.ThreadState = ThreadState.Suspended Then
                thread.Resume()
            End If
            threadAutoScrollDataGrid.Abort()

            If threadLabelBreachedBlink.ThreadState = ThreadState.Suspended Then
                thread.Resume()
            End If
            threadLabelBreachedBlink.Abort()

            If threadLabelUrgentBlink.ThreadState = ThreadState.Suspended Then
                thread.Resume()
            End If
            threadLabelUrgentBlink.Abort()

        Catch ex As Exception
            'MessageBox.Show(ex.ToString)  threadUpdateDB

        End Try
    End Sub

    Private Sub dataGridBreach_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles dataGridBreach.SelectionChanged
        'scroll to the selected item
        If dataGridBreach.Items.Count > 0 Then
            dotStartBreach.Fill = New SolidColorBrush(Colors.Green)
            BarReceiveToEnterBreached.Fill = New SolidColorBrush(Colors.LightGray)
            dotLoginBreached.Fill = New SolidColorBrush(Colors.LightGray)
            barLoginToBookBreached.Fill = New SolidColorBrush(Colors.LightGray)
            barLoginToPriceBreached.Fill = New SolidColorBrush(Colors.LightGray)
            dotPriceBreached.Fill = New SolidColorBrush(Colors.LightGray)
            barPriceToBookBreached.Fill = New SolidColorBrush(Colors.LightGray)
            dotBookBreached.Fill = New SolidColorBrush(Colors.LightGray)
            barBookToCreditBreached.Fill = New SolidColorBrush(Colors.LightGray)
            dotCreditBreached.Fill = New SolidColorBrush(Colors.LightGray)
            barCreditToPickReleaseBreached.Fill = New SolidColorBrush(Colors.LightGray)
            barCreditToMFGBreached.Fill = New SolidColorBrush(Colors.LightGray)
            dotMFGBreached.Fill = New SolidColorBrush(Colors.LightGray)
            barMFGToPickReleaseBreached.Fill = New SolidColorBrush(Colors.LightGray)
            dotPickReleaseBreached.Fill = New SolidColorBrush(Colors.LightGray)
            barPickReleaseToReadyToPickBreached.Fill = New SolidColorBrush(Colors.LightGray)
            dotReadyToPickBreached.Fill = New SolidColorBrush(Colors.LightGray)
            barReadyToPickToCustomerPickBreached.Fill = New SolidColorBrush(Colors.LightGray)
            dotCustomerPickBreached.Fill = New SolidColorBrush(Colors.LightGray)
            barEndBreached.Fill = New SolidColorBrush(Colors.LightGray)
            Try
                dataGridBreach.ScrollIntoView(dataGridBreach.Items(dataGridBreach.SelectedIndex))
                UpdateTrainStationBreached(DateTime.Now(), 2400)
                CheckCurrentOrderTimeBreached()
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try
        End If
    End Sub

    Private Sub DealBreachedBlink()
        While True
            Dispatcher.Invoke(AddressOf labelBreachedBlue)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf labelBreachedYellow)
            Thread.Sleep(200)
        End While


    End Sub

    Private Sub DealUrgentBlink()
        While True
            Dispatcher.Invoke(AddressOf labelUrgentBlue)
            Thread.Sleep(200)
            Dispatcher.Invoke(AddressOf labelUrgentYellow)
            Thread.Sleep(200)
        End While


    End Sub

    Private Sub labelBreachedBlue()
        labelPriceSubLoginTimeBreached.Background = New SolidColorBrush(Colors.LightBlue)
    End Sub
    Private Sub labelBreachedYellow()
        labelPriceSubLoginTimeBreached.Background = New SolidColorBrush(Colors.Yellow)
    End Sub

    Private Sub labelUrgentBlue()
        labelPriceSubLoginTimeUrgent.Background = New SolidColorBrush(Colors.LightBlue)
    End Sub
    Private Sub labelUrgentYellow()
        labelPriceSubLoginTimeUrgent.Background = New SolidColorBrush(Colors.Yellow)
    End Sub


    Public Function ChangeColor(ByRef dr As DataRowView, ByVal category As String)
        Dim t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, tm As New DateTime

        If dr.Item("Tag").ToString = "SPANewColor" Then

            If dr.Item("Status").ToString = "Price Request" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Send_To_Pricing_Time").ToString
                tm = DateTime.Now()
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarLoginToPrice(t2, tm, category)

            ElseIf dr.Item("Status").ToString = "Price Modified" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Send_To_Pricing_Time").ToString
                t3 = dr.Item("Price_Modify_Time").ToString
                t4 = dr.Item("Price_Send_Back_Time").ToString
                tm = DateTime.Now()
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarLoginToPrice(t2, t3, category)
                DotPrice(t3, t4, category)
                BarPriceToBook(t4, tm, category)

            ElseIf dr.Item("Status").ToString = "Booked" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Send_To_Pricing_Time").ToString
                t3 = dr.Item("Price_Modify_Time").ToString
                t4 = dr.Item("Price_Send_Back_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time")
                tm = DateTime.Now()
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarLoginToPrice(t2, t3, category)
                DotPrice(t3, t4, category)
                BarPriceToBook(t4, t5, category)
                DotBook(t4, t5, category)
                If t6.ToString("HH:mm:ss") = "00:00:00" Then
                    BarBookToCredit(t5, tm, category)
                Else
                    BarBookToCredit(t5, t6, category)
                    DotCredit(t6, tm, category)
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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarLoginToPrice(t2, t3, category)
                DotPrice(t3, t4, category)
                BarPriceToBook(t4, t5, category)
                DotBook(t4, t5, category)
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)


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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarLoginToPrice(t2, t3, category)
                DotPrice(t3, t4, category)
                BarPriceToBook(t4, t5, category)
                DotBook(t4, t5, category)
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)
                BarCreditToMFG(t7, t8, category)
                DotMFG(t8, t9, category)
                If t10.ToString("HH:mm:ss") = "00:00:00" Then
                    BarMFGToWHSEPick(t9, tm, category)
                Else
                    BarMFGToWHSEPick(t9, t10, category)
                    DotWHSEPick(t10, tm, category)
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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarLoginToPrice(t2, t3, category)
                DotPrice(t3, t4, category)
                BarPriceToBook(t4, t5, category)
                DotBook(t4, t5, category)
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)
                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    BarCreditToWHSEPick(t7, t10, category)
                    DotWHSEPick(t10, t11, category)
                    If t12.ToString("HH:mm:ss") = "00:00:00" Then
                        BarPickReleaseToReadyToPick(t11, tm, category)
                    Else
                        BarPickReleaseToReadyToPick(t11, t12, category)
                        DotWHSEPick(t12, tm, category)
                    End If
                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    BarCreditToMFG(t7, t8, category)
                    DotMFG(t8, t9, category)
                    BarMFGToWHSEPick(t9, t10, category)
                    DotWHSEPick(t10, t11, category)
                    If t12.ToString("HH:mm:ss") = "00:00:00" Then
                        BarPickReleaseToReadyToPick(t11, tm, category)
                    Else
                        t12 = dr.Item("Ready_To_Pick_ST").ToString
                        BarPickReleaseToReadyToPick(t11, t12, category)
                        DotWHSEPick(t12, tm, category)
                    End If
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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarLoginToPrice(t2, t3, category)
                DotPrice(t3, t4, category)
                BarPriceToBook(t4, t5, category)
                DotBook(t4, t5, category)
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)
                DotWHSEPick(t10, t11, category)
                BarPickReleaseToReadyToPick(t11, t12, category)
                DotReadyToPick(t12, t13, category)
                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    BarCreditToWHSEPick(t7, t10, category)
                    If t14.ToString("HH:mm:ss") = "00:00:00" Then
                        BarReadyToPickToCustomerPick(t13, tm, category)
                    Else
                        t14 = dr.Item("Customer_Pick_Time").ToString
                        BarReadyToPickToCustomerPick(t13, t14, category)
                    End If
                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    BarCreditToMFG(t7, t8, category)
                    DotMFG(t8, t9, category)
                    BarMFGToWHSEPick(t9, t10, category)

                    If t14.ToString("HH:mm:ss") = "00:00:00" Then
                        BarReadyToPickToCustomerPick(t13, tm, category)
                    Else
                        t14 = dr.Item("Customer_Pick_Time").ToString
                        BarReadyToPickToCustomerPick(t13, t14, category)
                    End If
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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t5, category)
                BarOrderLoginToOrderBook(t1, t5, category)
                DotBook(t1, t5, category)
                If t6.ToString("HH:mm:ss") = "00:00:00" Then
                    BarBookToCredit(t5, tm, category)
                Else
                    t6 = dr.Item("Credit_Check_Time").ToString
                    BarBookToCredit(t5, t6, category)
                    DotCredit(t6, tm, category)
                End If

                '检查Credit
            ElseIf dr.Item("Status").ToString = "Credit" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time").ToString
                t7 = dr.Item("Credit_Complete_Time").ToString
                tm = DateTime.Now()
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t5, category)
                BarOrderLoginToOrderBook(t1, t5, category)
                DotBook(t1, t5, category)
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)

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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t5, category)
                BarOrderLoginToOrderBook(t1, t5, category)
                DotBook(t1, t5, category)
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)
                BarCreditToMFG(t7, t8, category)
                DotMFG(t8, t9, category)
                If t10.ToString("HH:mm:ss") = "00:00:00" Then
                    BarMFGToWHSEPick(t9, tm, category)
                Else
                    t10 = dr.Item("WH_Start_Time").ToString
                    BarMFGToWHSEPick(t9, t10, category)
                    DotWHSEPick(t10, tm, category)
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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t5, category)
                BarOrderLoginToOrderBook(t1, t5, category)
                DotBook(t1, t5, category)
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)
                DotWHSEPick(t10, t11, category)
                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    BarCreditToWHSEPick(t7, t10, category)
                    If t12.ToString("HH:mm:ss") = "00:00:00" Then
                        BarPickReleaseToReadyToPick(t11, tm, category)
                    Else
                        t12 = dr.Item("Ready_To_Pick_ST").ToString
                        BarPickReleaseToReadyToPick(t11, t12, category)
                        DotReadyToPick(t12, tm, category)
                    End If

                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    BarCreditToMFG(t7, t8, category)
                    DotMFG(t8, t9, category)
                    BarMFGToWHSEPick(t9, t10, category)

                    If t12.ToString("HH:mm:ss") = "00:00:00" Then
                        BarPickReleaseToReadyToPick(t11, tm, category)
                    Else
                        t12 = dr.Item("Ready_To_Pick_ST").ToString
                        BarPickReleaseToReadyToPick(t11, t12, category)
                        DotReadyToPick(t12, tm, category)
                    End If
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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t5, category)
                BarOrderLoginToOrderBook(t1, t5, category)
                DotBook(t1, t5, category)
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)
                DotWHSEPick(t10, t11, category)
                BarPickReleaseToReadyToPick(t11, t12, category)
                DotReadyToPick(t12, t13, category)
                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    BarCreditToWHSEPick(t7, t10, category)
                    If t14.ToString("HH:mm:ss") = "00:00:00" Then
                        BarReadyToPickToCustomerPick(t13, tm, category)
                    Else
                        t14 = dr.Item("Customer_Pick_Time").ToString
                        BarReadyToPickToCustomerPick(t13, t14, category)
                    End If
                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    BarCreditToMFG(t7, t8, category)
                    DotMFG(t8, t9, category)
                    BarMFGToWHSEPick(t9, t10, category)
                    If t14.ToString("HH:mm:ss") = "00:00:00" Then
                        BarReadyToPickToCustomerPick(t13, tm, category)
                    Else
                        t14 = dr.Item("Customer_Pick_Time").ToString
                        BarReadyToPickToCustomerPick(t13, t14, category)
                    End If
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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarBookPending(t2, tm, category)

            ElseIf dr.Item("Status").ToString = "Booked" Then
                t0 = dr.Item("Receive_Order_Time").ToString
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Pending_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = dr.Item("Credit_Check_Time")
                tm = DateTime.Now()
                t21 = t2 - t1
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarBookPending(t2, t5, category)


                DotBookPending(t1, t5, category, Convert.ToInt32(t21.TotalMinutes))
                If t6.ToString("HH:mm:ss") = "00:00:00" Then
                    BarBookToCredit(t5, tm, category)
                Else
                    t6 = dr.Item("Credit_Check_Time").ToString
                    BarBookToCredit(t5, t6, category)
                    DotCredit(t6, tm, category)
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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarBookPending(t2, t5, category)
                DotBookPending(t1, t5, category, Convert.ToInt32(t21.TotalMinutes))
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)


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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarBookPending(t2, t5, category)
                DotBookPending(t1, t5, category, Convert.ToInt32(t21.TotalMinutes))
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)
                BarCreditToMFG(t7, t8, category)
                DotMFG(t8, t9, category)
                If t10.ToString("HH:mm:ss") = "00:00:00" Then
                    BarMFGToWHSEPick(t9, tm, category)
                Else
                    t10 = dr.Item("WH_Start_Time").ToString
                    BarMFGToWHSEPick(t9, t10, category)
                    DotWHSEPick(t10, tm, category)
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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarBookPending(t2, t5, category)
                DotBookPending(t1, t5, category, Convert.ToInt32(t21.TotalMinutes))
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)
                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    BarCreditToWHSEPick(t7, t10, category)
                    DotWHSEPick(t10, t11, category)
                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    BarCreditToMFG(t7, t8, category)
                    DotMFG(t8, t9, category)
                    BarMFGToWHSEPick(t9, t10, category)
                End If

                If t12.ToString("HH:mm:ss") = "00:00:00" Then
                    BarPickReleaseToReadyToPick(t11, tm, category)
                Else
                    t12 = dr.Item("Ready_To_Pick_ST").ToString
                    BarPickReleaseToReadyToPick(t11, t12, category)
                    DotReadyToPick(t12, tm, category)
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
                BarOrderReceiveToOrderEnter(t0, t1, category)
                DotOrderLogin(t1, t2, category)
                BarBookPending(t2, t5, category)
                DotBookPending(t1, t5, category, Convert.ToInt32(t21.TotalMinutes))
                BarBookToCredit(t5, t6, category)
                DotCredit(t6, t7, category)
                If t8.ToString("HH:mm:ss") = "00:00:00" Then
                    BarCreditToWHSEPick(t7, t10, category)
                    DotWHSEPick(t10, t11, category)
                Else
                    t8 = dr.Item("MFG_Start_Time").ToString
                    t9 = dr.Item("MFG_Complete_Time").ToString
                    BarCreditToMFG(t7, t8, category)
                    DotMFG(t8, t9, category)
                    BarMFGToWHSEPick(t9, t10, category)
                End If
                BarPickReleaseToReadyToPick(t11, t12, category)
                DotReadyToPick(t12, t13, category)
                If t14.ToString("HH:mm:ss") = "00:00:00" Then
                    BarReadyToPickToCustomerPick(t13, tm, category)
                Else
                    t14 = dr.Item("Customer_Pick_Time").ToString
                    BarReadyToPickToCustomerPick(t13, t14, category)
                End If
            End If
        End If
        Return Nothing
    End Function

    Private Function BarOrderReceiveToOrderEnter(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            BarReceiveToEnterBreached.Fill = CheckColor(t1, t2, 10, 15)
            Return Nothing
        ElseIf category = "urgent" Then
            BarReceiveToEnterUrgent.Fill = CheckColor(t1, t2, 10, 15)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function DotOrderLogin(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            dotLoginBreached.Fill = CheckColor(t1, t2, 10, 15)
            Return Nothing
        ElseIf category = "urgent" Then
            dotLoginUrgent.Fill = CheckColor(t1, t2, 10, 15)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function BarOrderLoginToOrderBook(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            barLoginToBookBreached.Fill = CheckColor(t1, t2, 10, 15)
            Return Nothing
        ElseIf category = "urgent" Then
            barLoginToBookUrgent.Fill = CheckColor(t1, t2, 10, 15)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function BarLoginToPrice(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            barLoginToPriceBreached.Fill = CheckColor(t1, t2, 240, 360)
            Return Nothing
        ElseIf category = "urgent" Then
            barLoginToPriceUrgent.Fill = CheckColor(t1, t2, 240, 360)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function DotPrice(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            dotPriceBreached.Fill = CheckColor(t1, t2, 30, 60)
            Return Nothing
        ElseIf category = "urgent" Then
            dotPriceUrgent.Fill = CheckColor(t1, t2, 30, 60)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function BarPriceToBook(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            barPriceToBookBreached.Fill = CheckColor(t1, t2, 10, 15)
            Return Nothing
        ElseIf category = "urgent" Then
            barPriceToBookUrgent.Fill = CheckColor(t1, t2, 10, 15)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function DotBook(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            dotBookBreached.Fill = CheckColor(t1, t2, 10, 15)
            Return Nothing
        ElseIf category = "urgent" Then
            dotBookUrgent.Fill = CheckColor(t1, t2, 10, 15)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function BarBookToCredit(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            barBookToCreditBreached.Fill = CheckColor(t1, t2, 30, 60)
            Return Nothing
        ElseIf category = "urgent" Then
            BarBookToCreditUrgent.Fill = CheckColor(t1, t2, 30, 60)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function DotCredit(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            dotCreditBreached.Fill = CheckColor(t1, t2, 480, 1440)
            Return Nothing
        ElseIf category = "urgent" Then
            dotCreditUrgent.Fill = CheckColor(t1, t2, 480, 1440)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function BarCreditToWHSEPick(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            barCreditToPickReleaseBreached.Fill = CheckColor(t1, t2, 1440, 2160)
            Return Nothing
        ElseIf category = "urgent" Then
            BarCreditToPickReleaseUrgent.Fill = CheckColor(t1, t2, 1440, 2160)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function
    Private Function BarCreditToMFG(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            barCreditToMFGBreached.Fill = CheckColor(t1, t2, 1440, 2880)
            Return Nothing
        ElseIf category = "urgent" Then
            BarCreditToMFGUrgent.Fill = CheckColor(t1, t2, 1440, 2880)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function DotMFG(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            dotMFGBreached.Fill = CheckColor(t1, t2, 1440, 2880)
            Return Nothing
        ElseIf category = "urgent" Then
            dotMFGUrgent.Fill = CheckColor(t1, t2, 1440, 2880)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function


    Private Function BarMFGToWHSEPick(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            barMFGToPickReleaseBreached.Fill = CheckColor(t1, t2, 1440, 2160)
            Return Nothing
        ElseIf category = "urgent" Then
            BarMFGToPickReleaseUrgent.Fill = CheckColor(t1, t2, 1440, 2160)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function DotWHSEPick(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            dotPickReleaseBreached.Fill = CheckColor(t1, t2, 10, 20)
            Return Nothing
        ElseIf category = "urgent" Then
            dotPickReleaseUrgent.Fill = CheckColor(t1, t2, 10, 20)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function BarPickReleaseToReadyToPick(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            barPickReleaseToReadyToPickBreached.Fill = CheckColor(t1, t2, 120, 240)
            Return Nothing
        ElseIf category = "urgent" Then
            BarPickReleaseToReadyToPickUrgent.Fill = CheckColor(t1, t2, 120, 240)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function DotReadyToPick(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            dotReadyToPickBreached.Fill = CheckColor(t1, t2, 1440, 2160)
            Return Nothing
        ElseIf category = "urgent" Then
            dotReadyToPickUrgent.Fill = CheckColor(t1, t2, 1440, 2160)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function

    Private Function BarReadyToPickToCustomerPick(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            barReadyToPickToCustomerPickBreached.Fill = CheckColor(t1, t2, 4320, 7200)
            Return Nothing
        ElseIf category = "urgent" Then
            BarReadyToPickToCustomerPickUrgent.Fill = CheckColor(t1, t2, 4320, 7200)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function
    Private Function BarBookPending(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String)
        If category = "breach" Then
            barLoginToBookBreached.Fill = CheckColor(t1, t2, 4320, 7200)
            Return Nothing
        ElseIf category = "urgent" Then
            barLoginToBookUrgent.Fill = CheckColor(t1, t2, 4320, 7200)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function
    Private Function DotBookPending(ByVal t1 As DateTime, ByVal t2 As DateTime, ByVal category As String, ByVal pendingtime As Int32)
        If category = "breach" Then
            dotBookBreached.Fill = CheckColor(t1, t2, pendingtime + 10, pendingtime + 15)
            Return Nothing
        ElseIf category = "urgent" Then
            dotBookUrgent.Fill = CheckColor(t1, t2, pendingtime + 10, pendingtime + 15)
            Return Nothing
        Else
            Return Nothing
        End If
    End Function
End Class



