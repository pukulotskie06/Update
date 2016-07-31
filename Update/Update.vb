Public Class Update
    ''Author pukulot github id = pukulotskie06

    Public db As New DatabaseClass
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MessageBox.Show("Are you sure You want to update ID No " & dGrid.CurrentRow.Cells("ID").Value.ToString() & "?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = 6 Then
            Dim s As String = txtProdName.Text & ";" & txtDescription.Text & ";" & txtBrandName.Text & ";" & txtType.Text & ";" _
                             & txtQty.Text & ";" & txtSize.Text & ";" & txtUnit.Text & ";" & txtPrice.Text & ";"

            db.UpdateQuery(Val(dGrid.CurrentRow.Cells("ID").Value.ToString()), s)
            Button2.PerformClick()
            'clearTextBoxes()

        End If

    End Sub

    Public Function textBoxContent() As Boolean
        Return True
    End Function

    ''Clear text boxes
    Public Sub clearTextBoxes()
        txtBrandName.Clear()
        txtProdName.Clear()
        txtDescription.Clear()
        txtPrice.Clear()
        txtUnit.Clear()
        txtQty.Clear()
        txtType.Clear()
        txtSize.Clear()
    End Sub
    ''Set Value of the textbox according to their corresponding value
    Public Sub setTextboxValue()
        txtBrandName.Text = dGrid.CurrentRow.Cells("Brand").Value.ToString()
        txtProdName.Text = dGrid.CurrentRow.Cells("Product_Name").Value.ToString()
        txtDescription.Text = dGrid.CurrentRow.Cells("Description").Value.ToString()
        txtPrice.Text = dGrid.CurrentRow.Cells("Price").Value.ToString()
        txtUnit.Text = dGrid.CurrentRow.Cells("Unit").Value.ToString()
        txtQty.Text = dGrid.CurrentRow.Cells("Qty").Value.ToString()
        txtType.Text = dGrid.CurrentRow.Cells("Type").Value.ToString()
        txtSize.Text = dGrid.CurrentRow.Cells("Size").Value.ToString()

    End Sub

    Public FieldNames() As String = {"ID", "Product_Name", "Description", _
                                     "Brand", "Type", "Qty", "Size", _
                                    "Unit", "Price", "Total"}
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load



        For x = 0 To FieldNames.Length - 1
            If x <> 9 Then
                dGrid.Columns.Add(FieldNames(x), FieldNames(x))
                comboFilter.Items.Add(FieldNames(x))
            End If
        Next
        'DataGridView1.Columns("Product_Name").Width = 200
        dGrid.Columns("ID").Width = 60
        dGrid.Columns("Unit").Width = 60
        dGrid.Columns("Qty").Width = 56
        Button2.PerformClick()

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim con As New DatabaseClass
        Dim tempString As String = Nothing
        For x = 0 To FieldNames.Length - 1
            If x <> 9 Then
                tempString &= "[" & FieldNames(x) & "]"
                If x < FieldNames.Length - 2 Then
                    tempString &= ","
                End If
            End If
        Next

        con.DatabaseClass("Select " & tempString & " From Items")


        dGrid.Rows.Clear()
        For x As Integer = 0 To con.dataSet.Tables("Inventory").Rows.Count - 1
            dGrid.Rows.Add()
            For y As Integer = 0 To con.dataSet.Tables("Inventory").Columns.Count - 1
                dGrid.Rows(x).Cells(y).Value = con.dataSet.Tables("Inventory").Rows(x).Item(y)
            Next
        Next

        If chkBoxfilter.Checked = True Then
            comboFilterNames.Text = Nothing
            comboFilter.Text = Nothing
        End If
        setTextboxValue()
    End Sub

    Private Sub chkBoxfilter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBoxfilter.CheckedChanged
        If chkBoxfilter.Checked = True Then
            comboFilterNames.Enabled = True
            comboFilter.Enabled = True
        Else
            comboFilterNames.Enabled = False
            comboFilter.Enabled = False
            comboFilter.Text = ""
            comboFilterNames.Text = ""
            Button2.PerformClick()


        End If
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ''Main.show()
        Me.Close()

    End Sub

    Private Sub comboFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboFilter.SelectedIndexChanged

        If comboFilter.Text <> Nothing Then
            comboFilterNames.Text = ""
            comboFilterNames.Items.Clear()

            Dim s() As String = db.Fill(comboFilter.Text).Split(";")

            For x As Integer = 0 To s.Length - 2
                comboFilterNames.Items.Add(s(x))
            Next
        End If
    End Sub

    Private Sub comboFilterNames_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboFilterNames.SelectedIndexChanged

        If comboFilter.Text = Nothing Or comboFilterNames.Text = Nothing Then

        Else

            db.FillDataWithType(comboFilter.Text, comboFilterNames.Text, "")

            dGrid.Rows.Clear()

            For x As Integer = 0 To db.dataSet.Tables("Inventory").Rows.Count - 1
                dGrid.Rows.Add()
                For y As Integer = 0 To db.dataSet.Tables("Inventory").Columns.Count - 1
                    dGrid.Rows(x).Cells(y).Value = db.dataSet.Tables("Inventory").Rows(x).Item(y)
                Next
            Next


        End If
        setTextboxValue()
    End Sub

    Private Sub diableButtons(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles comboFilterNames.KeyPress, comboFilter.KeyPress
        e.Handled = True
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dGrid.CellClick
        setTextboxValue()
    End Sub


    Dim rowCtr As Integer
    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress

        If Asc(e.KeyChar) = 13 And TextBox1.Text <> Nothing Then
            If rowCtr <= ctr Then

                dGrid.ClearSelection()
                dGrid.Rows(Val(row(rowCtr))).Selected = True
                dGrid.CurrentCell = dGrid.SelectedCells(0)

                rowCtr += 1
                If rowCtr = ctr Then
                    rowCtr = 0
                End If
                setTextboxValue()
            End If
        End If

    End Sub


    Dim ctr As Integer
    Dim row As New Collections.ArrayList
    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp

        If e.KeyCode <> 13 Then
            row.Clear()
            Dim temp As Integer = -1
            ctr = 0
            For x = 0 To dGrid.Rows.Count - 1
                For y = 0 To dGrid.Rows(x).Cells.Count - 1
                    If dGrid.Rows(x).Cells(y).Value.ToString.ToLower.Contains(TextBox1.Text.ToString.ToLower) Then
                        If temp <> x Then
                            ctr += 1
                            temp = x
                            row.Add(x)
                        End If
                    End If
                Next

            Next


            If TextBox1.Text <> Nothing Then
                lblDisplay.Text = ctr & " results found!"
            Else
                lblDisplay.Text = Nothing
            End If
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''Main.Show()
        Me.Close()
    End Sub

    Private Sub dGrid_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dGrid.KeyUp
        setTextboxValue()
    End Sub
End Class
