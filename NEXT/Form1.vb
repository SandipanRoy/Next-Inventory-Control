Public Class Form1
    Dim timer_tick As Integer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Form2.password_flag = False
        Form2.delete_password_flag = False


        Timer1.Start()

        timer_tick = 0

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If (timer_tick > 5) Then
            Form2.Show()
            Me.Hide()

        Else
            timer_tick = timer_tick + 1

        End If
    End Sub


End Class