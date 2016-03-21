Imports System.Data


Class MainWindow
    Private trainStation As TrainStation
    Private db As New DBHelper
    Private timer As New Timers.Timer()
    Dim dtUrgent As DataTable
    Dim dtNotBooked As DataTable



    Private Sub PanduitDashboardMain_Loaded(sender As Object, e As RoutedEventArgs) Handles PanduitDashboardMain.Loaded
        FillDataGrid()

    End Sub


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


        'fill Not booked orders
        'dtNotBooked = New DataTable
        'db.GetNotBookedOrders(dtNotBooked)
        'If dtNotBooked IsNot Nothing Then
        '    'dataGridOrders.ItemsSource = dtNotBooked.DefaultView
        '    dataGridOrders.ItemsSource = dtNotBooked.DefaultView
        'End If


        labelOrderCount.Content = db.GetOrderCount.ToString()
        labelPriceRequest.Content = db.GetPriceRequestOrderCount.ToString()
        labelBooked.Content = db.GetBookedOrderCount.ToString()

        Return Nothing
    End Function

    Private Sub buttonRefresh_Click(sender As Object, e As RoutedEventArgs) Handles buttonRefresh.Click

        'StartTimer()
        'Refresh()
        'FillDataGrid()
        RollOrders()


    End Sub

    'Private Sub buttonTotal_Click(sender As Object, e As RoutedEventArgs) Handles buttonTotal.Click
    '    shapeTotal.Fill = New SolidColorBrush(Colors.Red)
    'End Sub


    Private Function RollOrders()

        Dim index As Int32 = dataGridUrgent.SelectedIndex
        dataGridUrgent.SelectedIndex = (index + 1) Mod dataGridUrgent.Items.Count
        dataGridUrgent.Focus()

        Return Nothing

    End Function


    Private Sub StartTimer()
        timer.Interval = 2000
        AddHandler timer.Elapsed, AddressOf RollOrders
        timer.AutoReset = True
        timer.Enabled = True
    End Sub

    Private Sub StopTimer()
        timer.Enabled = False

    End Sub

    Private Function ShowTrainStation()
        Dim dr As DataRowView
        dr = dataGridUrgent.SelectedItem

        labelLoginTime.Content = dr.Item("Login_Order_Time").ToString
        labelPriceRequestTime.Content = dr.Item("Send_To_Pricing_Time").ToString

        Dim loginTime, priceTime As New DateTime
        loginTime = dr.Item("Login_Order_Time").ToString
        priceTime = dr.Item("Send_To_Pricing_Time").ToString

        Dim t4 As TimeSpan = priceTime - loginTime
        labelPriceSubLoginTime.Content = Math.Round((t4.TotalHours), 3).ToString() + " Hours"

        Return Nothing
    End Function


    Private Sub dataGridUrgent_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles dataGridUrgent.SelectionChanged
        'dataGridUrgent
        ShowTrainStation()
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        If dataGridUrgent IsNot Nothing Then
            dataGridUrgent.SelectedIndex = 0
            dataGridUrgent.Focus()
        End If

    End Sub
End Class
