Imports System.Data

Public Class TrainStation
    Public Sub New(ByRef dr As DataRowView)

        InitializeComponent()

        Try
            labelStartTime.Content = dr.Item("Login_Order_Time").ToString
            labelFinishTime.Content = dr.Item("Send_To_Pricing_Time").ToString

            Dim loginTime, sendToPriceTime As New DateTime
            loginTime = dr.Item("Login_Order_Time").ToString
            sendToPriceTime = dr.Item("Send_To_Pricing_Time").ToString

            Dim t4 As TimeSpan = sendToPriceTime - loginTime
            labelFinishTime.Content = Math.Round((t4.TotalHours), 3).ToString() + " Hours"


        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
