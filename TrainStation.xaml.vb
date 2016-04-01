Imports System.Data

Public Class TrainStation
    Public Sub New(ByRef drv As DataRowView)

        InitializeComponent()

        Try
            Dim loginTime, sendToPriceTime As New DateTime
            loginTime = drv.Item("Login_Order_Time").ToString
            sendToPriceTime = drv.Item("Send_To_Pricing_Time").ToString

            Dim t4 As TimeSpan = sendToPriceTime - loginTime
            labelPriceSubLoginTime.Content = Math.Round((t4.TotalMinutes), 1).ToString() + " Minutes"

            labelCustomerId.Content = drv.Item("Customer_ID").ToString
            labelCustomerOrder.Content = drv.Item("Customer_Order").ToString
            labelPanduitOrder.Content = drv.Item("Panduit_Order").ToString
            labelStatus.Content = drv.Item("Status").ToString
            labelUrgent.Content = drv.Item("Urgent").ToString
            labelSPANumber.Content = drv.Item("SPA_Number").ToString
            labelLastUser.Content = drv.Item("Last_User").ToString

        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
