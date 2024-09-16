Public Class Form3
    Public Const Admin_Password As String = "admin@123"

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If StrComp(Admin_Password, TextBox1.Text) = 0 Then
            Form2.password_flag = True
            Form2.Show()

            'Reset password text box
            TextBox1.Text = ""
            Me.Hide()
            Form2.Button2.PerformClick()
        Else
            MsgBox("Incorrect Password")
        End If
    End Sub
End Class