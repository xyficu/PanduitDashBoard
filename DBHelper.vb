Imports System.Data

Public Class DBHelper
    Private m_DBConStr As String
    Private m_DBConnIns As OleDb.OleDbConnection

    'constructor
    Public Sub New()
        m_DBConStr = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source = " + AppDomain.CurrentDomain.BaseDirectory + "\Track.accdb;"
        'm_DBConStr = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source = " + "C:\Track.accdb;"
        'm_DBConStr = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = \\pweng\PWshares\departments\AP Process Improvement\Database\SecureDatabase\Track.accdb;"
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

    '获得所有表格里未完成的订单
    Public Function GetNotCompleteOrders(ByRef dt As DataTable)
        Dim sqlCmdGetOrderUnfinish As String = "select * from [OrderTable] where [Customer_Pick] = false order by [Receive_Order_Time] "
        GetOrders(dt, sqlCmdGetOrderUnfinish)
        Return Nothing
    End Function

    '获得所有表格里未完成的Urgent订单
    Public Function GetUrgentOrders(ByRef dt As DataTable)
        Dim sqlCmdGetOrderUrgent As String = "select * from [OrderTable] where [Urgent] = 'Yes'and [Customer_Pick] = false order by [Receive_Order_Time] "

        GetOrders(dt, sqlCmdGetOrderUrgent)

        Return Nothing
    End Function

    'get breached orders
    Public Function GetBreachedOrders(ByRef dt As DataTable)
        GetNotCompleteOrders(dt)
        '保留超时的Order，移除非超时的order
        For Each dr As DataRowView In dt.DefaultView
            Dim t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, tm As New DateTime
            'SPANewColor表格
            If dr.Item("Tag").ToString = "SPANewColor" Then
                If dr.Item("Status").ToString = "Price Request" Then
                    t0 = dr.Item("Receive_Order_Time").ToString
                    t1 = dr.Item("Login_Order_Time").ToString
                    t2 = dr.Item("Send_To_Pricing_Time").ToString
                    tm = DateTime.Now()
                    If CheckTimeout(t0, t1, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t1, t2, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t2, tm, 360) = True Then
                        Continue For
                    Else
                        dt.Rows.Remove(dr.Row)
                    End If
                ElseIf dr.Item("Status").ToString = "Price Modified" Then
                    t0 = dr.Item("Receive_Order_Time").ToString
                    t1 = dr.Item("Login_Order_Time").ToString
                    t2 = dr.Item("Send_To_Pricing_Time").ToString
                    t3 = dr.Item("Price_Modify_Time").ToString
                    t4 = dr.Item("Price_Send_Back_Time").ToString
                    t0 = dr.Item("Receive_Order_Time").ToString
                    If CheckTimeout(t0, t1, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t1, t2, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t2, t3, 360) = True Then
                        Continue For
                    ElseIf CheckTimeout(t3, t4, 60) = True Then
                        Continue For
                    ElseIf CheckTimeout(t4, tm, 15) = True Then
                        Continue For
                    Else
                        dt.Rows.Remove(dr.Row)
                    End If
                ElseIf dr.Item("Status").ToString = "Booked" Then
                    t0 = dr.Item("Receive_Order_Time").ToString
                    t1 = dr.Item("Login_Order_Time").ToString
                    t2 = dr.Item("Send_To_Pricing_Time").ToString
                    t3 = dr.Item("Price_Modify_Time").ToString
                    t4 = dr.Item("Price_Send_Back_Time").ToString
                    t5 = dr.Item("Book_Order_Time").ToString
                    t6 = dr.Item("Credit_Check_Time")
                    tm = DateTime.Now()
                    If CheckTimeout(t0, t1, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t1, t2, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t2, t3, 360) = True Then
                        Continue For
                    ElseIf CheckTimeout(t3, t4, 60) = True Then
                        Continue For
                    ElseIf CheckTimeout(t4, t5, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t5, tm, 60) = True Then
                        Continue For
                    Else
                        dt.Rows.Remove(dr.Row)
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

                    If CheckTimeout(t0, t1, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t1, t2, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t2, t3, 360) = True Then
                        Continue For
                    ElseIf CheckTimeout(t3, t4, 60) = True Then
                        Continue For
                    ElseIf CheckTimeout(t4, t5, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t5, t6, 60) = True Then
                        Continue For
                    ElseIf CheckTimeout(t6, t7, 60) = True Then
                        Continue For
                    Else dt.Rows.Remove(dr.Row)
                    End If
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
                    t9 = dr.Item("MFG_Complete_Time")
                    tm = DateTime.Now()
                    If CheckTimeout(t0, t1, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t1, t2, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t2, t3, 360) = True Then
                        Continue For
                    ElseIf CheckTimeout(t3, t4, 60) = True Then
                        Continue For
                    ElseIf CheckTimeout(t4, t5, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t5, t6, 60) = True Then
                        Continue For
                    ElseIf CheckTimeout(t6, t7, 1440) = True Then
                        Continue For
                    ElseIf CheckTimeout(t7, t8, 2840) = True Then
                        Continue For
                    ElseIf t9.ToString("HH:mm:ss") <> "00:00:00" Then
                        t9 = dr.Item("MFG_Complete_Time").ToString
                        If CheckTimeout(t8, t9, 2840) = True And CheckTimeout(t9, tm, 2160) Then
                            Continue For
                        End If
                        'ElseIf t9.ToString("HH:mm:ss") = "00:00:00" Then
                        '    If CheckTimeout(t8, tm, 2840) = True Then
                        '        Continue For
                        '    End If
                    Else dt.Rows.Remove(dr.Row)
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
                    If t8.ToString("HH:mm:ss") = "00:00:00" Then
                        If CheckTimeout(t0, t1, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t1, t2, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t2, t3, 360) = True Then
                            Continue For
                        ElseIf CheckTimeout(t3, t4, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t4, t5, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t5, t6, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t6, t7, 1440) = True Then
                            Continue For
                        ElseIf CheckTimeout(t7, t10, 2160) = True Then
                            Continue For
                        ElseIf CheckTimeout(t10, t11, 20) = True Then
                            Continue For
                        ElseIf t12.ToString("HH:mm:ss") <> "00:00:00" Then
                            t12 = dr.Item("Ready_To_Pick_ST").ToString
                            If CheckTimeout(t11, t12, 240) = True Then
                                Continue For
                            End If
                        ElseIf t12.ToString("HH:mm:ss") = "00:00:00" Then
                            If CheckTimeout(t11, tm, 240) = True Then
                                Continue For
                            End If
                        Else dt.Rows.Remove(dr.Row)
                        End If
                    Else

                        t8 = dr.Item("MFG_Start_Time").ToString
                        t9 = dr.Item("MFG_Complete_Time").ToString

                        If CheckTimeout(t0, t1, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t1, t2, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t2, t3, 360) = True Then
                            Continue For
                        ElseIf CheckTimeout(t3, t4, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t4, t5, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t5, t6, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t6, t7, 1440) = True Then
                            Continue For
                        ElseIf CheckTimeout(t7, t8, 2840) = True Then
                            Continue For
                        ElseIf CheckTimeout(t8, t9, 2840) = True Then
                            Continue For
                        ElseIf CheckTimeout(t9, t10, 2160) = True Then
                            Continue For
                        ElseIf CheckTimeout(t10, t11, 20) = True Then
                            Continue For
                        ElseIf t12.ToString("HH:mm:ss") <> "00:00:00" Then
                            t12 = dr.Item("Ready_To_Pick_ST").ToString
                            If CheckTimeout(t11, t12, 240) = True Then
                                Continue For
                            End If
                        ElseIf t12.ToString("HH:mm:ss") = "00:00:00" Then
                            If CheckTimeout(t11, tm, 240) = True Then
                                Continue For
                            End If
                        Else dt.Rows.Remove(dr.Row)
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
                    If t8.ToString("HH:mm:ss") = "00:00:00" Then
                        If CheckTimeout(t0, t1, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t1, t2, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t2, t3, 360) = True Then
                            Continue For
                        ElseIf CheckTimeout(t3, t4, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t4, t5, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t5, t6, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t6, t7, 1440) = True Then
                            Continue For
                        ElseIf CheckTimeout(t7, t10, 2160) = True Then
                            Continue For
                        ElseIf CheckTimeout(t10, t11, 20) = True Then
                            Continue For
                        ElseIf CheckTimeout(t11, t12, 240) = True Then
                            Continue For
                        ElseIf CheckTimeout(t12, t13, 2160) = True Then
                            Continue For
                        ElseIf t14.ToString("HH:mm:ss") = "00:00:00" Then
                            If CheckTimeout(t13, tm, 7200) = True Then
                                Continue For
                            End If
                        ElseIf t14.ToString("HH:mm:ss") <> "00:00:00" Then
                            t14 = dr.Item("Customer_Pick_Time").ToString
                            If CheckTimeout(t13, t14, 7200) = True Then
                                Continue For
                            End If
                        Else dt.Rows.Remove(dr.Row)
                        End If
                    Else

                        t8 = dr.Item("MFG_Start_Time").ToString
                        t9 = dr.Item("MFG_Complete_Time").ToString

                        If CheckTimeout(t0, t1, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t1, t2, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t2, t3, 360) = True Then
                            Continue For
                        ElseIf CheckTimeout(t3, t4, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t4, t5, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t5, t6, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t6, t7, 1440) = True Then
                            Continue For
                        ElseIf CheckTimeout(t7, t8, 2840) = True Then
                            Continue For
                        ElseIf CheckTimeout(t8, t9, 2840) = True Then
                            Continue For
                        ElseIf CheckTimeout(t9, t10, 2160) = True Then
                            Continue For
                        ElseIf CheckTimeout(t10, t11, 20) = True Then
                            Continue For
                        ElseIf CheckTimeout(t11, t12, 240) = True Then
                            Continue For
                        ElseIf CheckTimeout(t12, t13, 2160) = True Then
                            Continue For
                        ElseIf t14.ToString("HH:mm:ss") = "00:00:00" Then
                            If CheckTimeout(t13, tm, 7200) = True Then
                                Continue For
                            End If
                        ElseIf t14.ToString("HH:mm:ss") <> "00:00:00" Then
                            t14 = dr.Item("Customer_Pick_Time").ToString
                            If CheckTimeout(t13, t14, 7200) = True Then
                                Continue For
                            End If
                        Else dt.Rows.Remove(dr.Row)
                        End If
                    End If
                End If

                '普通订单
            ElseIf dr.Item("Tag").ToString = "NormalOrder" Then


                If dr.Item("Status").ToString = "Booked" Then
                    t0 = dr.Item("Receive_Order_Time").ToString
                    t1 = dr.Item("Login_Order_Time").ToString
                    t5 = dr.Item("Book_Order_Time").ToString
                    t6 = dr.Item("Credit_Check_Time")
                    tm = DateTime.Now()
                    If CheckTimeout(t0, t1, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t1, t5, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t5, tm, 60) = True Then
                        Continue For
                    Else
                        dt.Rows.Remove(dr.Row)
                    End If

                    '检查Credit
                ElseIf dr.Item("Status").ToString = "Credit" Then
                    t0 = dr.Item("Receive_Order_Time").ToString
                    t1 = dr.Item("Login_Order_Time").ToString
                    t5 = dr.Item("Book_Order_Time").ToString
                    t6 = dr.Item("Credit_Check_Time").ToString
                    t7 = dr.Item("Credit_Complete_Time").ToString
                    If CheckTimeout(t0, t1, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t1, t5, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t5, t6, 60) = True Then
                        Continue For
                    ElseIf CheckTimeout(t6, t7, 1440) = True Then
                        Continue For
                    Else dt.Rows.Remove(dr.Row)
                    End If
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
                    If CheckTimeout(t0, t1, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t1, t5, 15) = True Then
                        Continue For
                    ElseIf CheckTimeout(t5, t6, 60) = True Then
                        Continue For
                    ElseIf CheckTimeout(t6, t7, 1440) = True Then
                        Continue For
                    ElseIf CheckTimeout(t7, t8, 2840) = True Then
                        Continue For
                    ElseIf t10.ToString("HH:mm:ss") <> "00:00:00" Then
                        t10 = dr.Item("WH_Start_Time").ToString
                        If CheckTimeout(t9, t10, 2160) = True Then
                            Continue For
                        End If
                    ElseIf t10.ToString("HH:mm:ss") = "00:00:00" Then
                        If CheckTimeout(t9, tm, 2160) = True Then
                            Continue For
                        End If
                    Else dt.Rows.Remove(dr.Row)
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
                    If t8.ToString("HH:mm:ss") = "00:00:00" Then
                        If CheckTimeout(t0, t1, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t1, t5, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t5, t6, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t6, t7, 1440) = True Then
                            Continue For
                        ElseIf CheckTimeout(t7, t10, 2160) = True Then
                            Continue For
                        ElseIf CheckTimeout(t10, t11, 20) = True Then
                            Continue For
                        ElseIf t12.ToString("HH:mm:ss") <> "00:00:00" Then
                            t12 = dr.Item("Ready_To_Pick_ST").ToString
                            If CheckTimeout(t11, t12, 240) = True Then
                                Continue For
                            End If
                        ElseIf t12.ToString("HH:mm:ss") = "00:00:00" Then
                            If CheckTimeout(t11, tm, 240) = True Then
                                Continue For
                            End If
                        Else dt.Rows.Remove(dr.Row)
                        End If
                    Else
                        t8 = dr.Item("MFG_Start_Time").ToString
                        t9 = dr.Item("MFG_Complete_Time").ToString
                        If CheckTimeout(t0, t1, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t1, t5, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t5, t6, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t6, t7, 1440) = True Then
                            Continue For
                        ElseIf CheckTimeout(t7, t8, 2840) = True Then
                            Continue For
                        ElseIf CheckTimeout(t8, t9, 2840) = True Then
                            Continue For
                        ElseIf CheckTimeout(t9, t10, 2160) = True Then
                            Continue For
                        ElseIf CheckTimeout(t10, t11, 20) = True Then
                            Continue For
                        ElseIf t12.ToString("HH:mm:ss") <> "00:00:00" Then
                            t12 = dr.Item("Ready_To_Pick_ST").ToString
                            If CheckTimeout(t11, t12, 240) = True Then
                                Continue For
                            End If
                        ElseIf t12.ToString("HH:mm:ss") = "00:00:00" Then
                            If CheckTimeout(t11, tm, 240) = True Then
                                Continue For
                            End If
                        Else dt.Rows.Remove(dr.Row)
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
                    If t8.ToString("HH:mm:ss") = "00:00:00" Then
                        If CheckTimeout(t0, t1, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t1, t5, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t5, t6, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t6, t7, 1440) = True Then
                            Continue For
                        ElseIf CheckTimeout(t7, t10, 2160) = True Then
                            Continue For
                        ElseIf CheckTimeout(t10, t11, 20) = True Then
                            Continue For
                        ElseIf CheckTimeout(t11, t12, 240) = True Then
                            Continue For
                        ElseIf CheckTimeout(t12, t13, 2160) = True Then
                            Continue For
                        ElseIf t14.ToString("HH:mm:ss") <> "00:00:00" Then
                            t14 = dr.Item("Customer_Pick_Time").ToString
                            If CheckTimeout(t13, t14, 7200) = True Then
                                Continue For
                            End If
                        ElseIf t14.ToString("HH:mm:ss") = "00:00:00" Then
                            If CheckTimeout(t13, t14, 7200) = True Then
                                Continue For
                            End If
                        Else dt.Rows.Remove(dr.Row)
                        End If
                    Else
                        t8 = dr.Item("MFG_Start_Time").ToString
                        t9 = dr.Item("MFG_Complete_Time").ToString

                        If CheckTimeout(t0, t1, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t1, t5, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t5, t6, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t6, t7, 1440) = True Then
                            Continue For
                        ElseIf CheckTimeout(t7, t8, 2840) = True Then
                            Continue For
                        ElseIf CheckTimeout(t8, t9, 2840) = True Then
                            Continue For
                        ElseIf CheckTimeout(t9, t10, 2160) = True Then
                            Continue For
                        ElseIf CheckTimeout(t10, t11, 20) = True Then
                            Continue For
                        ElseIf CheckTimeout(t11, t12, 240) = True Then
                            Continue For
                        ElseIf CheckTimeout(t12, t13, 2160) = True Then
                            Continue For
                        ElseIf t14.ToString("HH:mm:ss") <> "00:00:00" Then
                            t14 = dr.Item("Customer_Pick_Time").ToString
                            If CheckTimeout(t13, t14, 7200) = True Then
                                Continue For
                            End If
                        ElseIf t14.ToString("HH:mm:ss") = "00:00:00" Then
                            If CheckTimeout(t13, t14, 7200) = True Then
                                Continue For
                            End If
                        Else dt.Rows.Remove(dr.Row)
                        End If
                    End If
                End If


                'SPAExpire表格
            ElseIf dr.Item("Tag").ToString = "SPAExpire" Then
                'Dim t21 As New TimeSpan
                If dr.Item("Status").ToString = "Pending" Then
                    t0 = dr.Item("Receive_Order_Time").ToString
                    t1 = dr.Item("Login_Order_Time").ToString
                    't2 = dr.Item("Pending_Time").ToString
                    tm = DateTime.Now()
                    If CheckTimeout(t0, t1, 15) = True Then
                        Continue For
                        'ElseIf CheckTimeout(t1, t2, 15) = True Then
                        '    Continue For
                    Else
                        dt.Rows.Remove(dr.Row)
                    End If

                ElseIf dr.Item("Status").ToString = "Booked" Then
                    t0 = dr.Item("Receive_Order_Time").ToString
                        t1 = dr.Item("Login_Order_Time").ToString
                        t5 = dr.Item("Book_Order_Time").ToString
                        t6 = dr.Item("Credit_Check_Time")
                        tm = DateTime.Now()
                        If CheckTimeout(t0, t1, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t1, t5, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t5, tm, 60) = True Then
                            Continue For
                        Else
                            dt.Rows.Remove(dr.Row)
                        End If

                        '检查Credit
                    ElseIf dr.Item("Status").ToString = "Credit" Then
                        t0 = dr.Item("Receive_Order_Time").ToString
                        t1 = dr.Item("Login_Order_Time").ToString
                        t5 = dr.Item("Book_Order_Time").ToString
                        t6 = dr.Item("Credit_Check_Time").ToString
                        t7 = dr.Item("Credit_Complete_Time").ToString
                        If CheckTimeout(t0, t1, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t1, t5, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t5, t6, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t6, t7, 1440) = True Then
                            Continue For
                        Else dt.Rows.Remove(dr.Row)
                        End If
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
                        If CheckTimeout(t0, t1, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t1, t5, 15) = True Then
                            Continue For
                        ElseIf CheckTimeout(t5, t6, 60) = True Then
                            Continue For
                        ElseIf CheckTimeout(t6, t7, 1440) = True Then
                            Continue For
                        ElseIf CheckTimeout(t7, t8, 2840) = True Then
                            Continue For
                        ElseIf t10.ToString("HH:mm:ss") <> "00:00:00" Then
                            t10 = dr.Item("WH_Start_Time").ToString
                            If CheckTimeout(t9, t10, 2160) = True Then
                                Continue For
                            End If
                        ElseIf t10.ToString("HH:mm:ss") = "00:00:00" Then
                            If CheckTimeout(t9, tm, 2160) = True Then
                                Continue For
                            End If
                        Else dt.Rows.Remove(dr.Row)
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
                        If t8.ToString("HH:mm:ss") = "00:00:00" Then
                            If CheckTimeout(t0, t1, 15) = True Then
                                Continue For
                            ElseIf CheckTimeout(t1, t5, 15) = True Then
                                Continue For
                            ElseIf CheckTimeout(t5, t6, 60) = True Then
                                Continue For
                            ElseIf CheckTimeout(t6, t7, 1440) = True Then
                                Continue For
                            ElseIf CheckTimeout(t7, t10, 2160) = True Then
                                Continue For
                            ElseIf CheckTimeout(t10, t11, 20) = True Then
                                Continue For
                            ElseIf t12.ToString("HH:mm:ss") <> "00:00:00" Then
                                t12 = dr.Item("Ready_To_Pick_ST").ToString
                                If CheckTimeout(t11, t12, 240) = True Then
                                    Continue For
                                End If
                            ElseIf t12.ToString("HH:mm:ss") = "00:00:00" Then
                                If CheckTimeout(t11, tm, 240) = True Then
                                    Continue For
                                End If
                            Else dt.Rows.Remove(dr.Row)
                            End If
                        Else
                            t8 = dr.Item("MFG_Start_Time").ToString
                            t9 = dr.Item("MFG_Complete_Time").ToString
                            If CheckTimeout(t0, t1, 15) = True Then
                                Continue For
                            ElseIf CheckTimeout(t1, t5, 15) = True Then
                                Continue For
                            ElseIf CheckTimeout(t5, t6, 60) = True Then
                                Continue For
                            ElseIf CheckTimeout(t6, t7, 1440) = True Then
                                Continue For
                            ElseIf CheckTimeout(t7, t8, 2840) = True Then
                                Continue For
                            ElseIf CheckTimeout(t8, t9, 2840) = True Then
                                Continue For
                            ElseIf CheckTimeout(t9, t10, 2160) = True Then
                                Continue For
                            ElseIf CheckTimeout(t10, t11, 20) = True Then
                                Continue For
                            ElseIf t12.ToString("HH:mm:ss") <> "00:00:00" Then
                                t12 = dr.Item("Ready_To_Pick_ST").ToString
                                If CheckTimeout(t11, t12, 240) = True Then
                                    Continue For
                                End If
                            ElseIf t12.ToString("HH:mm:ss") = "00:00:00" Then
                                If CheckTimeout(t11, tm, 240) = True Then
                                    Continue For
                                End If
                            Else dt.Rows.Remove(dr.Row)
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
                        If t8.ToString("HH:mm:ss") = "00:00:00" Then
                            If CheckTimeout(t0, t1, 15) = True Then
                                Continue For
                            ElseIf CheckTimeout(t1, t5, 15) = True Then
                                Continue For
                            ElseIf CheckTimeout(t5, t6, 60) = True Then
                                Continue For
                            ElseIf CheckTimeout(t6, t7, 1440) = True Then
                                Continue For
                            ElseIf CheckTimeout(t7, t10, 2160) = True Then
                                Continue For
                            ElseIf CheckTimeout(t10, t11, 20) = True Then
                                Continue For
                            ElseIf CheckTimeout(t11, t12, 240) = True Then
                                Continue For
                            ElseIf CheckTimeout(t12, t13, 2160) = True Then
                                Continue For
                            ElseIf t14.ToString("HH:mm:ss") <> "00:00:00" Then
                                t14 = dr.Item("Customer_Pick_Time").ToString
                                If CheckTimeout(t13, t14, 7200) = True Then
                                    Continue For
                                End If
                            ElseIf t14.ToString("HH:mm:ss") = "00:00:00" Then
                                If CheckTimeout(t13, t14, 7200) = True Then
                                    Continue For
                                End If
                            Else dt.Rows.Remove(dr.Row)
                            End If
                        Else
                            t8 = dr.Item("MFG_Start_Time").ToString
                            t9 = dr.Item("MFG_Complete_Time").ToString

                            If CheckTimeout(t0, t1, 15) = True Then
                                Continue For
                            ElseIf CheckTimeout(t1, t5, 15) = True Then
                                Continue For
                            ElseIf CheckTimeout(t5, t6, 60) = True Then
                                Continue For
                            ElseIf CheckTimeout(t6, t7, 1440) = True Then
                                Continue For
                            ElseIf CheckTimeout(t7, t8, 2840) = True Then
                                Continue For
                            ElseIf CheckTimeout(t8, t9, 2840) = True Then
                                Continue For
                            ElseIf CheckTimeout(t9, t10, 2160) = True Then
                                Continue For
                            ElseIf CheckTimeout(t10, t11, 20) = True Then
                                Continue For
                            ElseIf CheckTimeout(t11, t12, 240) = True Then
                                Continue For
                            ElseIf CheckTimeout(t12, t13, 2160) = True Then
                                Continue For
                            ElseIf t14.ToString("HH:mm:ss") <> "00:00:00" Then
                                t14 = dr.Item("Customer_Pick_Time").ToString
                                If CheckTimeout(t13, t14, 7200) = True Then
                                    Continue For
                                End If
                            ElseIf t14.ToString("HH:mm:ss") = "00:00:00" Then
                                If CheckTimeout(t13, t14, 7200) = True Then
                                    Continue For
                                End If
                            Else dt.Rows.Remove(dr.Row)
                            End If
                        End If
                    End If
                End If

        Next

        Return Nothing
    End Function

    '检查是否超时
    Private Function CheckTimeout(t1 As DateTime, t2 As DateTime, timelimit As Int32)
        Dim t As TimeSpan = t2 - t1
        If t.TotalMinutes > timelimit Then
            Return True
        End If
        Return False

    End Function

    'get order count
    Public Function GetOrderCount()
        Dim sqlCmd As String = "Select * from [SPANewColor]"
        Dim dt As New DataTable
        GetOrders(dt, sqlCmd)
        Return dt.Rows.Count

    End Function

    'get booked order count
    Public Function GetBookedOrderCount()
        Dim sqlCmd As String = "Select * from [SPANewColor] where [Status] = 'Booked'"
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
