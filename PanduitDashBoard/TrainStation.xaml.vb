Imports System.Data

Public Class TrainStation
    Public Sub New(ByRef drv As DataRowView)

        InitializeComponent()

        Try
            labelLoginTime.Content = drv.Item("Login_Order_Time").ToString
            labelPriceRequestTime.Content = drv.Item("Send_To_Pricing_Time").ToString

            Dim loginTime, sendToPriceTime As New DateTime
            loginTime = drv.Item("Login_Order_Time").ToString
            sendToPriceTime = drv.Item("Send_To_Pricing_Time").ToString

            Dim t4 As TimeSpan = sendToPriceTime - loginTime
            labelPriceSubLoginTime.Content = Math.Round((t4.TotalHours), 3).ToString() + " Hours"

            labelId.Content = drv.Item("ID").ToString
            labelCustomerId.Content = drv.Item("Customer_ID").ToString
            labelCustomerOrder.Content = drv.Item("Customer_Order").ToString
            labelPanduitOrder.Content = drv.Item("Panduit_Order").ToString
            labelStatus.Content = drv.Item("Status").ToString
            labelUrgent.Content = drv.Item("Urgent").ToString

        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
