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
        dtNotBooked = New DataTable
        db.GetNotBookedOrders(dtNotBooked)
        If dtNotBooked IsNot Nothing Then
            'dataGridOrders.ItemsSource = dtNotBooked.DefaultView
            dataGridOrders.ItemsSource = dtNotBooked.DefaultView
        End If


        labelOrderCount.Content = db.GetOrderCount.ToString()
        labelPriceRequest.Content = db.GetPriceRequestOrderCount.ToString()
        labelBooked.Content = db.GetBookedOrderCount.ToString()

        Return Nothing
    End Function

    Private Sub buttonRefresh_Click(sender As Object, e As RoutedEventArgs) Handles buttonRefresh.Click

        'StartTimer()
        'Refresh()
        FillDataGrid()



    End Sub

    'Private Sub buttonTotal_Click(sender As Object, e As RoutedEventArgs) Handles buttonTotal.Click
    '    shapeTotal.Fill = New SolidColorBrush(Colors.Red)
    'End Sub

    Private Sub dataGridUrgent_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles dataGridUrgent.MouseUp
        Dim dataRow As DataRowView
        dataRow = dataGridUrgent.SelectedItem
        trainStation = New TrainStation(dataRow)
        trainStation.ShowDialog()
    End Sub



    Private Function Refresh()

        'db.GetUrgentOrders(dtUrgent)
        'If dtUrgent.Rows.Count = 0 Then
        '    dtUrgent.Clear()
        'End If


        'db.GetNotBookedOrders(dtNotBooked)
        'If dtNotBooked Is Nothing Then
        '    dtNotBooked.Clear()
        'End If

        Return Nothing

    End Function


    Private Sub StartTimer()
        timer.Interval = 2000
        AddHandler timer.Elapsed, AddressOf Refresh
        timer.AutoReset = False
        timer.Enabled = True
    End Sub

    Private Sub StopTimer()
        timer.Enabled = False

    End Sub
End Class
