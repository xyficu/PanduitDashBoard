Imports System.Data

Public Class DBHelper
    Private m_DBConStr As String
    Private m_DBConnIns As OleDb.OleDbConnection

    'constructor
    Public Sub New()
        m_DBConStr = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source = " + AppDomain.CurrentDomain.BaseDirectory + "\Track.accdb;"
        'm_DBConStr = "C:\Order.accdb;;"
        m_DBConnIns = New OleDb.OleDbConnection
        m_DBConnIns.ConnectionString = m_DBConStr

    End Sub

    'destructor
    Protected Overrides Sub Finalize()
        MyBase.Finalize()

    End Sub

    'get db table
    Public Function GetOrders(ByRef dt As DataTable, ByVal sqlCmd As String)
        Dim errMsg As String

        Try
            m_DBConnIns.Open()
            Dim dataAdapter As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(sqlCmd, m_DBConnIns)
            dataAdapter.Fill(dt)
            m_DBConnIns.Close()
            Return Nothing

        Catch ex As Exception
            m_DBConnIns.Close()
            errMsg = ex.ToString()
            dt.Clear()
            Return Nothing
        End Try
    End Function

    'get Not booked orders order by time
    Public Function GetNotBookedOrders(ByRef dt As DataTable)
        Dim sqlCmdGetBooked As String = "select * from [SPANewColor] where [Status] <> 'Booked' order by [Send_To_Pricing_Time] "
        GetOrders(dt, sqlCmdGetBooked)
        Return Nothing
    End Function

    'get urgent orders
    Public Function GetUrgentOrders(ByRef dt As DataTable)
        Dim sqlCmdGetUrgent As String = "select * from [SPANewColor] where [Urgent] = 'Yes'"
        GetOrders(dt, sqlCmdGetUrgent)
        Return Nothing
    End Function

    'get order count
    Public Function GetOrderCount()
        Dim sqlCmd As String = "select * from [SPANewColor]"
        Dim dt As New DataTable
        GetOrders(dt, sqlCmd)
        Return dt.Rows.Count

    End Function

    'get booked order count
    Public Function GetBookedOrderCount()
        Dim sqlCmd As String = "select * from [SPANewColor] where [Status] = 'Booked'"
        Dim dt As New DataTable
        GetOrders(dt, sqlCmd)
        Return dt.Rows.Count

    End Function

    'get price request order count
    Public Function GetPriceRequestOrderCount()
        Dim sqlCmd As String = "select * from [SPANewColor] where [Status] = 'Price Request'"
        Dim dt As New DataTable
        GetOrders(dt, sqlCmd)
        Return dt.Rows.Count

    End Function

    Public Function GetTotalOrderFailCount(ByRef _totalCount, ByRef _totalFailedCount,
                                           ByRef _totalPriceCount, ByRef _priceFailedCount,
                                           ByRef _totalBookCount, ByRef _bookFailedCount)
        Dim totalCount As Int32 = 0
        Dim totalFailedCount As Int32 = 0
        Dim totalPriceCount As Int32 = 0
        Dim priceFailedCount As Int32 = 0
        Dim totalBookCount As Int32 = 0
        Dim bookFailedCount As Int32 = 0

        'Calculate SPA 
        Dim sqlCmd As String = "select * from [SPANewColor]"
        Dim dt As New DataTable
        GetOrders(dt, sqlCmd)
        totalCount += dt.Rows.Count
        totalPriceCount += dt.Select("[Status] = 'Price Request'").Count
        totalBookCount += dt.Select("[Status] = 'Booked'").Count

        Dim t1SPA, t2SPA, t3SPA, t4SPA As DateTime
        For Each row As DataRow In dt.Rows
            t1SPA = row.Item("Login_Order_Time").ToString
            t2SPA = row.Item("Send_To_Pricing_Time").ToString
            t3SPA = row.Item("Price_Modify_Time").ToString
            t4SPA = row.Item("Price_Send_Back_Time").ToString
            Dim t21 As TimeSpan = t2SPA - t1SPA
            Dim t43 As TimeSpan = t4SPA - t3SPA
            If t21.TotalMinutes > 15 Then
                totalFailedCount += 1
            End If
            If t43.TotalMinutes > 15 Then
                priceFailedCount += 1
            End If

        Next

        'Calculate Normal
        sqlCmd = "select * from [NormalOrder]"
        dt.Clear()
        GetOrders(dt, sqlCmd)
        totalCount += dt.Rows.Count
        totalPriceCount += dt.Select("[Status] = 'Price Request'").Count
        totalBookCount += dt.Select("[Status] = 'Booked'").Count

        Dim t1Normal, t6Normal As DateTime
        For Each row As DataRow In dt.Rows
            t1Normal = row.Item("Login_Order_Time").ToString
            t6Normal = row.Item("Book_Order_Time").ToString
            Dim t61 As TimeSpan = t6Normal - t1Normal
            If t61.TotalMinutes > 15 Then
                totalFailedCount += 1
            End If
        Next

        _totalCount = totalCount
        _totalFailedCount = totalFailedCount
        _totalPriceCount = totalPriceCount
        _priceFailedCount = priceFailedCount
        _totalBookCount = totalBookCount
        _bookFailedCount = bookFailedCount

        Return Nothing
    End Function


End Class
