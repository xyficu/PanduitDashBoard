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
        Dim sqlCmdGetUrgent As String = "select * from [SPANewColor] where [Urgent] = True"
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


End Class
