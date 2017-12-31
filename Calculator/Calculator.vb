
Imports System
Imports System.IO
Imports System.Xml

Public Class frmCalc

    Dim connection As OleDb.OleDbConnection


    Sub ConnectToDatabase()

        connection = New OleDb.OleDbConnection("Provider=Microstoft.ACE.OLEDB.12.0; " &
                                               "Data Source=Calculator.accdb;")

        connection.Open()

    End Sub

    Sub DisconnectFromDatabase()

        connection.Close()

    End Sub

    Sub ViewCalculations()

        Dim q As String = "SELECT * FROM Calculator"

        Dim cmd As New OleDb.OleDbCommand(q, connection)
        Dim reader As OleDb.OleDbDataReader = cmd.ExecuteReader

        Console.Clear()
        Console.WriteLine("{0,-5} {1, -20}", "ID", "Equation")

        While reader.Read

            Console.WriteLine("{0,-5} {1, -20}",
                              reader("ID"),
                              reader("Equation"))

        End While

        reader.Close()

    End Sub

    Dim class1 As New MathOp
    Dim class2 As New MathOp2

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        class1.Num1 = txtNum1.Text
        class1.Num2 = txtNum2.Text
        class1.Result = class1.Num1 + class1.Num2

        txtResult.Text = class1.Result

    End Sub

    Private Sub btnSub_Click(sender As Object, e As EventArgs) Handles btnSub.Click
        class1.Num1 = txtNum1.Text
        class1.Num2 = txtNum2.Text
        class1.Result = class1.Num1 - class1.Num2

        txtResult.Text = class1.Result

    End Sub

    Private Sub btnDiv_Click(sender As Object, e As EventArgs) Handles btnDiv.Click
        class2.Num1 = txtNum1.Text
        class2.Num2 = txtNum2.Text
        class2.Result = class2.Num1 / class2.Num2

        Try
            txtResult.Text = class2.Result

        Catch ex As Exception
            MessageBox.Show("You cannot divide by Zero")

        End Try

    End Sub

    Private Sub btnMult_Click(sender As Object, e As EventArgs) Handles btnMult.Click
        class2.Num1 = txtNum1.Text
        class2.Num2 = txtNum2.Text
        class2.Result = class2.Num1 * class2.Num2

        txtResult.Text = class2.Result

    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        ListBox1.Items.Clear()

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        ListBox1.Items.Add(txtResult.Text)

    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click

        ListBox1.Items.Remove(ListBox1.SelectedIndex())

    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        Dim index As Integer = ListBox1.SelectedIndex
        ListBox1.Items.RemoveAt(index)

        ListBox1.Items.Insert(index, txtResult.Text)

    End Sub

    Private Sub ListBox1_Click(sender As Object, e As EventArgs) Handles ListBox1.Click

        txtResult.Text = ListBox1.SelectedItem.ToString()

    End Sub

    Private Sub btnSaveData_Click(sender As Object, e As EventArgs) Handles btnSaveData.Click

        Dim Equation As String

        Equation = ListBox1.Text

        Dim com As System.Data.OleDb.OleDbCommand

        Try
            ConnectToDatabase()
            MsgBox("Access succesfully connected")
            com = New System.Data.OleDb.OleDbCommand("INSERT INTO account(Equation) VALUES(" & Equation & ")")
            com.ExecuteReader(CommandBehavior.CloseConnection)
            DisconnectFromDatabase()
            Return
        Catch ex As Exception
            DisconnectFromDatabase()
            MsgBox(ex.ToString)
            Return
        End Try



    End Sub

    Private Sub btnRetrieve_Click(sender As Object, e As EventArgs) Handles btnRetrieve.Click

        ViewCalculations()

    End Sub

    Private Sub btnSXml_Click(sender As Object, e As EventArgs) Handles btnSXml.Click

        Dim settings As New XmlWriterSettings()
        settings.Indent = True

        Dim XmlWrt As XmlWriter = XmlWriter.Create("Equation.xml", settings)

        With XmlWrt

            .WriteStartDocument()

            .WriteComment("XML Database.")

            .WriteStartElement("Equation")
            .WriteString(ListBox1.Text.ToString())
            .WriteEndElement()

            .WriteEndElement()

            .WriteEndDocument()
            .Close()

        End With

        MessageBox.Show("XML file saved.")

    End Sub

    Private Sub btnRXml_Click(sender As Object, e As EventArgs) Handles btnRXml.Click

        Dim reader As New XmlTextReader("D:\Kohl's School\ENTD461\Assignments\Calculator\Calculator\bin\Debug")

        While reader.Read()

            Select Case reader.NodeType

                Case XmlNodeType.Element
                    ListBox1.Items.Add("<" + reader.Name & ">")
                    Exit Select
                Case XmlNodeType.Text
                    ListBox1.Items.Add(reader.Value)
                    Exit Select
                Case XmlNodeType.EndElement
                    ListBox1.Items.Add("")
                    Exit Select

            End Select

        End While

    End Sub
End Class
