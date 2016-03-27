Imports System.ComponentModel
Imports System.Data
Imports System.Threading
Imports System.Windows.Threading

Class MainWindow
    Private trainStation As TrainStation
    Private db As New DBHelper
    Private timer As New Timers.Timer()
    Dim dtUrgent, dtTimeout As DataTable
    Dim dtNotBooked As DataTable
    Private threadAutoScrollDataGrid As Thread

    '声明更新界面委托
    Delegate Function UpdateUIDelegate()
    Dim rollOrderDelegate As New UpdateUIDelegate(AddressOf RollOrders)

    '设定滚动时间
    Private threadSleepTime As Int32 = 3

    '窗体加载时填充datagrid
    Private Sub PanduitDashboardMain_Loaded(sender As Object, e As RoutedEventArgs) Handles PanduitDashboardMain.Loaded
        FillDataGrid()

    End Sub

    '检查是否超时
    Private Function CheckTimeout(t1 As DateTime, t2 As DateTime)
        Dim t As TimeSpan = t2 - t1
        If t.TotalMinutes > 10 Then
            Return True
        End If
        Return False

    End Function

    Private Function GetTimeoutDataTable(ByRef dt As DataTable)
        '只获取超时的Order，移除非超时的order
        For Each dr As DataRowView In dt.DefaultView
            Dim t1, t2, t3, t4, t5, t6 As New DateTime
            t1 = dr.Item("Login_Order_Time").ToString
            t2 = dr.Item("Send_To_Pricing_Time").ToString
            t3 = dr.Item("Price_Modify_Time").ToString
            t4 = dr.Item("Price_Send_Back_Time").ToString
            t5 = dr.Item("Book_Order_Time").ToString
            t6 = t5

            If CheckTimeout(t1, t2) = True Then
                Continue For
            ElseIf CheckTimeout(t2, t3) = True Then
                Continue For
            ElseIf CheckTimeout(t3, t4) = True Then
                Continue For
            ElseIf CheckTimeout(t4, t5) = True Then
                Continue For
            ElseIf CheckTimeout(t1, t6) = True Then
                Continue For
            Else dt.Rows.Remove(dr.Row)
            End If

        Next

        Return Nothing
    End Function

    '填充datagrid
    Private Function FillDataGrid()

        'fill urgent orders
        dtUrgent = New DataTable
        dtTimeout = New DataTable
        db.GetUrgentOrders(dtUrgent)
        db.GetUrgentOrders(dtTimeout)

        If dtUrgent IsNot Nothing Then
            'dataGridUrgent.ItemsSource = dtUrgent.DefaultView

            Dim binding = New Binding()
            binding.Source = dtUrgent.DefaultView
            dataGridUrgent.SetBinding(DataGrid.ItemsSourceProperty, binding)

            dataGridUrgent.Columns(0).Visibility = Visibility.Collapsed
            dataGridUrgent.Columns(5).Visibility = Visibility.Collapsed
            dataGridUrgent.Columns(6).Visibility = Visibility.Collapsed
            dataGridUrgent.Columns(9).Visibility = Visibility.Collapsed
            dataGridUrgent.Columns(10).Visibility = Visibility.Collapsed
            dataGridUrgent.Columns(11).Visibility = Visibility.Collapsed
        End If

        If dtTimeout IsNot Nothing Then
            '只获取超时的数据
            GetTimeoutDataTable(dtTimeout)

            Dim binding = New Binding()
            binding.Source = dtTimeout.DefaultView
            dataGridTimeout.SetBinding(DataGrid.ItemsSourceProperty, binding)

            dataGridTimeout.Columns(0).Visibility = Visibility.Hidden
            dataGridTimeout.Columns(5).Visibility = Visibility.Hidden
            dataGridTimeout.Columns(6).Visibility = Visibility.Hidden
            dataGridTimeout.Columns(9).Visibility = Visibility.Hidden
            dataGridTimeout.Columns(10).Visibility = Visibility.Hidden
            dataGridTimeout.Columns(11).Visibility = Visibility.Hidden

            '根据紧急程度标注颜色
            For Each dr As DataRowView In dataGridTimeout.Items
                Dim t1, t2, t3, t4, t5, t6 As New DateTime
                t1 = dr.Item("Login_Order_Time").ToString
                t2 = dr.Item("Send_To_Pricing_Time").ToString
                t3 = dr.Item("Price_Modify_Time").ToString
                t4 = dr.Item("Price_Send_Back_Time").ToString
                t5 = dr.Item("Book_Order_Time").ToString
                t6 = t5

            Next

            'Dim row As DataGridRow = dataGridTimeout.ItemContainerGenerator.ContainerFromIndex(0)
            'row.Background = New SolidColorBrush(Colors.Blue)

        End If

        GetTotalStatistics()
        'labelOrderCount.Content = db.GetOrderCount.ToString()
        'labelPriceRequest.Content = db.GetPriceRequestOrderCount.ToString()
        'labelBooked.Content = db.GetBookedOrderCount.ToString()

        Return Nothing
    End Function


    Private Sub DealThread()

        While True
            Dispatcher.Invoke(AddressOf RollOrders)

            'Dispatcher.BeginInvoke(DispatcherPriority.Normal, rollOrderDelegate)
            Thread.Sleep(1000 * threadSleepTime)
        End While

    End Sub

    '遍历显示datagrid条目
    Private Function RollOrders()

        Try
            If dataGridUrgent.Items.Count > 0 Then
                Dim index As Int32 = dataGridUrgent.SelectedIndex
                dataGridUrgent.SelectedIndex = (index + 1) Mod dataGridUrgent.Items.Count
                dataGridUrgent.Focus()
                CheckCurrentOrderTime()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        Return Nothing

    End Function

    Private Function ShowTrainStation()
        Dim dr As DataRowView
        Dim dt As New DataTable
        db.GetUrgentOrders(dt)
        dr = dt.DefaultView.Item(dataGridUrgent.SelectedIndex)

        Dim loginTime, priceTime As New DateTime
        loginTime = dr.Item("Login_Order_Time").ToString
        priceTime = dr.Item("Send_To_Pricing_Time").ToString

        Dim t4 As TimeSpan = priceTime - loginTime
        labelPriceSubLoginTime.Content = Math.Round((t4.TotalMinutes), 1).ToString() + " Minutes"


        Return Nothing
    End Function


    Private Sub dataGridUrgent_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles dataGridUrgent.SelectionChanged
        'scroll to the selected item
        If dataGridUrgent.Items.Count > 0 Then
            dataGridUrgent.ScrollIntoView(dataGridUrgent.Items(dataGridUrgent.SelectedIndex))
            ShowTrainStation()
            CheckCurrentOrderTime()

        End If

    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        threadAutoScrollDataGrid = New Thread(AddressOf DealThread)

        If dataGridUrgent IsNot Nothing Then
            dataGridUrgent.SelectedIndex = 0
            dataGridUrgent.Focus()

            dataGridTimeout.SelectedIndex = 0

            'Dim style As New Style
            'style.Setters.Add(New Setter(HorizontalAlignmentProperty, HorizontalAlignment.Center))
            'dataGridUrgent.HorizontalContentAlignment = HorizontalAlignment.Right
            'For Each col As DataGridColumn In dataGridUrgent.Columns
            '    col.CellStyle = style
            'Next

            'labelPriceSubLoginTime.HorizontalContentAlignment = HorizontalAlignment.Left

        End If


    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            'Thread.Sleep(1000 * threadSleepTime)
            If threadAutoScrollDataGrid.ThreadState = ThreadState.Suspended Then
                threadAutoScrollDataGrid.Resume()

            End If
            'threadAutoScrollDataGrid.Interrupt()
            threadAutoScrollDataGrid.Abort()
        Catch ex As Exception
            'MessageBox.Show(ex.ToString)
        End Try

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

    Private Sub dataGridUrgent_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles dataGridUrgent.MouseUp
        Dim drv As DataRowView
        drv = dataGridUrgent.SelectedItem
        trainStation = New TrainStation(drv)
        trainStation.ShowDialog()
    End Sub

    'check order, change color when timeout
    Private Function CheckCurrentOrderTime()
        Dim dr As DataRowView
        Dim dt As New DataTable
        db.GetUrgentOrders(dt)
        If dataGridUrgent.SelectedIndex > 0 Then
            dr = dt.DefaultView.Item(dataGridUrgent.SelectedIndex)

            Dim t1, t2, t3, t4, t5, t6 As New DateTime
            t1 = dr.Item("Login_Order_Time").ToString
            t2 = dr.Item("Send_To_Pricing_Time").ToString
            t3 = dr.Item("Price_Modify_Time").ToString
            t4 = dr.Item("Price_Send_Back_Time").ToString
            t5 = dr.Item("Book_Order_Time").ToString
            t6 = t5

            dotLogin.Fill = CheckColor(t1, t2)
            barLoginToPrice.Fill = CheckColor(t2, t3)
            dotPrice.Fill = CheckColor(t3, t4)
            barPriceToBook.Fill = CheckColor(t4, t5)
            dotBook.Fill = CheckColor(t1, t6)

            labelCurrentPandiutOrder.Content = dr.Item("Panduit_Order").ToString
        End If

        Return Nothing
    End Function

    'change color depend on dot time
    Private Function CheckColor(t1 As DateTime, t2 As DateTime)
        Dim t As TimeSpan = t2 - t1

        Dim f As Double = t.TotalMinutes
        If f <= 0 Then
            Return New SolidColorBrush(Colors.Gray)
        ElseIf f > 0 And f <= 10 Then
            Return New SolidColorBrush(Colors.Green)
        ElseIf f > 10 And f <= 15 Then
            Return New SolidColorBrush(Colors.Yellow)
        ElseIf f > 15 Then
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


        labelOrderCount.Content = totalFailedCount.ToString + "/" + totalCount.ToString
        labelPriceRequest.Content = priceFailedCount.ToString + "/" + prictCount.ToString
        labelBooked.Content = bookFailedCount.ToString + "/" + bookCount.ToString

        'show total order count
        labelTotalRunningOrderCount.Content = totalCount.ToString

        'show timeout order count
        labelTotalTimeoutOrderCount.Content = totalFailedCount.ToString

    End Function

    Private Sub dataGridTimeout_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles dataGridTimeout.MouseUp
        Dim drv As DataRowView
        drv = dataGridTimeout.SelectedItem
        trainStation = New TrainStation(drv)
        trainStation.ShowDialog()
    End Sub
End Class



