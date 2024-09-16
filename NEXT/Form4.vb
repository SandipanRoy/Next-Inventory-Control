Public Class Form4

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If StrComp(PASSWORD_IN.Text, Form3.Admin_Password) = 0 Then
            Form2.delete_password_flag = True

            'Reset Password TextBox
            PASSWORD_IN.Text = ""
            Me.Hide()

            Form2.DELETE_RECORD.PerformClick()

        Else
            MsgBox("Incorrect Password")
        End If
    End Sub
End Class