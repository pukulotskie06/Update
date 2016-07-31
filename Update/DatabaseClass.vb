﻿Public Class DatabaseClass
    Private con As New OleDb.OleDbConnection
    Private dataAdapter As OleDb.OleDbDataAdapter
    Public dataSet As New DataSet
    Private dataProvider As String = "Provider=Microsoft.ACE.OLEDB.12.0;"
    Private dataSource As String = "Data Source = " & System.Environment.CurrentDirectory & "\Inventory.accdb"
    'Provider=Microsoft.ACE.OLEDB.12.0;Data Source="C:\Users\pukulot\Documents\Visual Studio 2010\Projects\POS\POS\bin\Debug\inventory.accdb";Persist Security Info=True;User ID=admin

   

    ''End of delete
    ''Fill the combobox depending of the field name

    Public Function Fill(ByVal sql As String) As String

        con.Close()

        con.ConnectionString = dataProvider & dataSource
        con.Open()
        dataAdapter = New OleDb.OleDbDataAdapter("Select Distinct [" & sql & "] from Items", con)
        Dim tempDs As New DataSet
        dataAdapter.Fill(tempDs, "Combobox")
        Dim combo As String = Nothing

        For x As Integer = 0 To tempDs.Tables("Combobox").Rows.Count - 1
                combo &= tempDs.Tables("Combobox").Rows(x).Item(sql) & ";"

        Next

        Return combo
    End Function

    ''Énd of fill here

    ''Fill the datagrid depending on the type
    Public Sub FillDataWithType(ByVal sql As String, ByVal sql2 As String)
        Dim temp As String = "Select [ID],[Product_Name],[Description],[Brand],[Type],[Size],[Unit],[Price] From [Items] Where "
        con.Close()
        con.ConnectionString = dataProvider & dataSource
        con.Open()
        dataSet = New DataSet


        sql = "[" & sql & "] = " & "'" & sql2 & "'"

        'sql = "[" & sql & "] = " & sql2
        dataAdapter = New OleDb.OleDbDataAdapter(temp & sql, con)

        Try
            dataAdapter.Fill(dataSet, "Inventory")
        Catch ex As Exception
            sql = sql.Replace("'" & sql2 & "'", sql2)
            dataAdapter = New OleDb.OleDbDataAdapter(temp & sql, con)
            dataAdapter.Fill(dataSet, "Inventory")
        End Try
    End Sub

    ''end of filling the datagrid


    ''Updating query
    Public Sub DatabaseClass(ByVal sql As String)
        con.Close()
        con.ConnectionString = dataProvider & dataSource
        con.Open()
        dataSet = New DataSet()
        dataAdapter = New OleDb.OleDbDataAdapter(sql, con)
        dataAdapter.Fill(dataSet, "Inventory")

        ''MsgBox(dataSource)

    End Sub


    ''Updating Query
    Public Sub UpdateQuery(ByVal x As Integer, ByVal s As String)
        Dim tableFields() As String = s.Trim.Split(";")
        For y = 0 To tableFields.Length - 1
            '' MsgBox(tableFields(y))
        Next
        con.Close()
        Dim sqlCmd As String = "Update Items SET [Product_Name] = '" & tableFields(0) & "',[Description] = '" & tableFields(1) _
                               & "',[Brand] = '" & tableFields(2) & "',[Type] = '" & tableFields(3) _
                               & "',[Qty] = " & Val(tableFields(4)) & ",[Size] = '" & tableFields(5) _
                               & "',[Unit] = '" & tableFields(6) & "',[Price] = " & Val(tableFields(7)) & " Where ID = ?"
        con.ConnectionString = dataProvider & dataSource
        con.Open()

        Dim cmd As New OleDb.OleDbCommand(sqlCmd, con)

        cmd.Parameters.Add(New OleDb.OleDbParameter("ID", x))
        Try
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            con.Close()
            MsgBox("ID no " & x & " successfully updated!")
        Catch ex As Exception
            MsgBox(ex.ToString)

        End Try

    End Sub

    ''Fill the datagrid depending on the type update
    Public Sub FillDataWithType(ByVal sql As String, ByVal sql2 As String, ByVal sql3 As String)
        Dim temp As String = "Select [ID],[Product_Name],[Description],[Brand],[Type],[Qty],[Size],[Unit],[Price] From [Items] Where "
        con.Close()
        con.ConnectionString = dataProvider & dataSource
        con.Open()
        dataSet = New DataSet


        sql = "[" & sql & "] = " & "'" & sql2 & "'"

        'sql = "[" & sql & "] = " & sql2
        dataAdapter = New OleDb.OleDbDataAdapter(temp & sql, con)

        Try
            dataAdapter.Fill(dataSet, "Inventory")
        Catch ex As Exception
            sql = sql.Replace("'" & sql2 & "'", sql2)
            dataAdapter = New OleDb.OleDbDataAdapter(temp & sql, con)
            dataAdapter.Fill(dataSet, "Inventory")
        End Try
    End Sub

    ''end of filling the datagrid
   
    ''Updating query ends here

End Class
