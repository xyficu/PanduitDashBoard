Imports System.ComponentModel
Imports System.Data
Imports System.Threading
Imports System.Windows.Threading

Class MainWindow
    Private trainStation As TrainStation
    Private db As New DBHelper
    Private timer As New Timers.Timer()
    Dim dtUrgent As DataTable
    Dim dtNotBooked As DataTable
    Private threadAutoScrollDataGrid As Thread

    '声明更新界面委托
    Delegate Function UpdateUIDelegate()
    Dim rollOrderDelegate As New UpdateUIDelegate(AddressOf RollOrders)

    '设定滚动时间
    Private threadSleepTime As Int32 = 1

    '窗体加载时填充datagrid
    Private Sub PanduitDashboardMain_Loaded(sender As Object, e As RoutedEventArgs) Handles PanduitDashboardMain.Loaded
        FillDataGrid()

    End Sub

    '填充datagrid
    Private Function FillDataGrid()

        'fill urgent orders
        dtUrgent = New DataTable
        db.GetUrgentOrders(dtUrgent)
        If dtUrgent IsNot Nothing Then
            'dataGridUrgent.ItemsSource = dtUrgent.DefaultView
            Dim binding = New Binding()
            binding.Source = dtUrgent.DefaultView
            dataGridUrgent.SetBinding(DataGrid.ItemsSourceProperty, binding)
        End If

        labelOrderCount.Content = db.GetOrderCount.ToString()
        labelPriceRequest.Content = db.GetPriceRequestOrderCount.ToString()
        labelBooked.Content = db.GetBookedOrderCount.ToString()

        Return Nothing
    End Function


    Private Sub DealThread()

        While True
            Dispatcher.Invoke(AddressOf RollOrders)
            'Dispatcher.BeginInvoke(DispatcherPriority.Normal, rollOrderDelegate)
            Thread.Sleep(1000 * threadSleepTime)
        End While

    End Sub


    'Private Sub buttonTotal_Click(sender As Object, e As RoutedEventArgs) Handles buttonTotal.Click
    '    shapeTotal.Fill = New SolidColorBrush(Colors.Red)
    'End Sub

    '遍历显示datagrid条目
    Private Function RollOrders()

        Try
            If dataGridUrgent.Items.Count > 0 Then
                Dim index As Int32 = dataGridUrgent.SelectedIndex
                dataGridUrgent.SelectedIndex = (index + 1) Mod dataGridUrgent.Items.Count
                dataGridUrgent.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        Return Nothing

    End Function

    Private Function ShowTrainStation()
        Dim dr As DataRowView
        dr = dataGridUrgent.SelectedItem

        'labelLoginTime.Content = dr.Item("Login_Order_Time").ToString
        'labelPriceRequestTime.Content = dr.Item("Send_To_Pricing_Time").ToString

        Dim loginTime, priceTime As New DateTime
        loginTime = dr.Item("Login_Order_Time").ToString
        priceTime = dr.Item("Send_To_Pricing_Time").ToString

        Dim t4 As TimeSpan = priceTime - loginTime
        labelPriceSubLoginTime.Content = Math.Round((t4.TotalHours), 3).ToString() + " Hours"

        Return Nothing
    End Function


    Private Sub dataGridUrgent_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles dataGridUrgent.SelectionChanged
        'scroll to the selected item
        If dataGridUrgent.Items.Count > 0 Then
            dataGridUrgent.ScrollIntoView(dataGridUrgent.Items(dataGridUrgent.SelectedIndex))
            ShowTrainStation()
        End If

    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        threadAutoScrollDataGrid = New Thread(AddressOf DealThread)

        If dataGridUrgent IsNot Nothing Then
            dataGridUrgent.SelectedIndex = 0
            dataGridUrgent.Focus()
        End If


    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            If threadAutoScrollDataGrid.ThreadState = ThreadState.Suspended Then
                threadAutoScrollDataGrid.Resume()
            End If

            threadAutoScrollDataGrid.Abort()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    '启动自动刷新
    Private Sub buttonAutoScroll_Click(sender As Object, e As RoutedEventArgs) Handles buttonAutoScroll.Click
        Try
            If threadAutoScrollDataGrid.ThreadState = ThreadState.Unstarted Then
                threadAutoScrollDataGrid.Start()
                buttonAutoScroll.Content = "Stop Auto Scroll"

            ElseIf threadAutoScrollDataGrid.ThreadState = ThreadState.Suspended Then
                threadAutoScrollDataGrid.Resume()
                buttonAutoScroll.Content = "Stop Auto Scroll"

            ElseIf threadAutoScrollDataGrid.ThreadState = ThreadState.WaitSleepJoin Then
                threadAutoScrollDataGrid.Suspend()
                buttonAutoScroll.Content = "Start Auto Scroll"

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
End Class
