Imports System
Imports System.IO
Imports System.Collections



Public Class Form2
    Public password_flag As Boolean
    Public delete_password_flag As Boolean


    Const Database_file_path As String = "c:\database\"
    Const Image_file_path As String = "c:\database\images\"

    Const Quantity_Level_Warning As Integer = 100



    Private Sub Form2_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        Form1.Close()
        Form3.Close()
        Form4.Close()

    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        password_flag = False

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        'Ask User to choose the image of the product

        OpenFileDialog1.Filter = "JPG|*.jpg|JPEG|*.jpeg|PNG|*.png"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Title = "Select image"

        If (OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Product_Image.ImageLocation = OpenFileDialog1.FileName
        Else
            Product_Image.ImageLocation = ""

        End If


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'code will execute when submit button of insert record is pressed

        If password_flag = False Then
            Form3.Show()
        End If



        If (password_flag = True) Then


            Dim flag As Boolean

            Dim s1 As String
            s1 = System.IO.Path.GetFileName(Product_Image.ImageLocation)

            If StrComp(Product_Name.Text, "") = 0 Then
                MsgBox("Please Enter Product Name")


                'check if the user has chosen any image or not
            ElseIf (StrComp(s1, "") = 0) Then
                MsgBox("Please select an Image of the product")


            ElseIf (StrComp(Product_ID.Text, "") = 0) Then
                MsgBox("Please Enter Product ID")


            ElseIf StrComp(No_Units.Text, "") = 0 Then
                MsgBox("Please Enter the total number of Units")


            ElseIf StrComp(PRICE.Text, "") = 0 Then
                MsgBox("Please Enter Product Price")


            ElseIf StrComp(STACK_NUMBER.Text, "") = 0 Then
                MsgBox("Please Enter Stack Number")


            ElseIf StrComp(Product_Desc.Text, "") = 0 Then
                MsgBox("Please Enter Product Description")

            Else




                Dim Insert_Record As New Record()

                'preparing class body for storing in file
                Insert_Record.product_name = Product_Name.Text
                Insert_Record.product_id = Product_ID.Text
                Insert_Record.number_of_product = Val(No_Units.Text)
                Insert_Record.price = Val(PRICE.Text)
                Insert_Record.stack_number = STACK_NUMBER.Text
                Insert_Record.product_description = Product_Desc.Text
                Insert_Record.product_image = s1



                'check if the database directory exists or not
                If Directory.Exists(Database_file_path) Then
                Else

                    'if directory not exist then create the directory
                    Shell("cmd /c mkdir " + Database_file_path)

                End If



                'check if the image directory exists or not
                If Directory.Exists(Image_file_path) Then
                Else

                    'if directory not exist then create the directory
                    Shell("cmd /c mkdir " + Image_file_path)

                End If


                'check if the file already exist or not
                If File.Exists(Database_file_path + Product_Name.Text + ".xml") Then
                    MsgBox("Product Name " & Product_Name.Text & " alrady exist." + vbNewLine +
                           "You can't add another product with same name." + vbNewLine +
                           "If you want to modify that record," + vbNewLine +
                           "please go to Modify Existing Record Section")
                Else




                    flag = False


                    If File.Exists(Image_file_path + s1) Then
                        MsgBox("Image with same filename already exist in Record." + vbNewLine +
                                        "Please choose another file or Rename the File")
                    Else
                        'copy the image in images file
                        My.Computer.FileSystem.CopyFile(Product_Image.ImageLocation, Image_file_path + s1)

                        'writing Insert_Record object to xml file
                        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(Record))
                        Dim file_writer As New System.IO.StreamWriter(Database_file_path + Product_Name.Text + ".xml")
                        writer.Serialize(file_writer, Insert_Record)
                        file_writer.Close()

                        flag = True

                    End If

                End If                'end if statement of block 2
            End If              'end if statement for block 1


            If flag = True Then
                MsgBox("Added Record Successfully")
                Product_Name.Text = ""
                Product_ID.Text = ""
                No_Units.Text = ""
                PRICE.Text = ""
                STACK_NUMBER.Text = ""
                Product_Desc.Text = ""
                Product_Image.ImageLocation = ""
                password_flag = False
            End If


        End If              'for password check flag


    End Sub




    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        'code executes when search button in display record is pressed

        Dim Temp_file_name As String
        Temp_file_name = Database_file_path + SEARCH_QUERY.Text + ".xml"


        If File.Exists(Temp_file_name) Then

            Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(Record))
            Dim file_read As New System.IO.StreamReader(Temp_file_name)

            Dim temp As Record

            'Load the records in textbox
            temp = CType(reader.Deserialize(file_read), Record)
            PRODUCT_NAME_2.Text = temp.product_name
            PRODUCT_ID_2.Text = temp.product_id
            AVAILABLE.Text = temp.number_of_product

            'changing the color of the shape depending upon quantity available
            If Val(AVAILABLE.Text) < 100 Then
                OvalShape2.FillColor = Color.Red
            Else
                OvalShape2.FillColor = Color.Green
            End If


            PRICE_2.Text = temp.price
            STACK_NUMBER_2.Text = temp.stack_number
            PRODUCT_DESC_2.Text = temp.product_description

            'Load the image in picturebox
            If File.Exists(Image_file_path + temp.product_image) Then
                PictureBox1.ImageLocation = Image_file_path + temp.product_image

            End If


        Else
            MsgBox("Record Not Exsist")

        End If


    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        'Code executed when search button in modify record is pressed

        Dim Temp_Filename As String
        Temp_Filename = Database_file_path + SEARCH_QUERY_2.Text + ".xml"


        'check if the product record exist or not
        If File.Exists(Temp_Filename) Then

            'Enable Modify Button
            Button6.Enabled = True

            'open streamreader to read file
            Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(Record))
            Dim file_read As New System.IO.StreamReader(Temp_Filename)

            'Reading Data from File
            Dim read_record As New Record
            read_record = CType(reader.Deserialize(file_read), Record)

            'Updating the values in the form
            PRODUCT_NAME_3.Text = read_record.product_name
            PRODUCT_ID_3.Text = read_record.product_id
            AVAILABLE_2.Text = read_record.number_of_product
            PRICE_3.Text = read_record.price
            STACK_NUMBER_3.Text = read_record.stack_number
            PRODUCT_DESC_3.Text = read_record.product_description

            'closing the file
            file_read.Close()


            'Checking If the image file exist or not
            If File.Exists(Image_file_path + read_record.product_image) Then
                'Load the image in picture box
                PictureBox2.ImageLocation = Image_file_path + read_record.product_image
            End If

        Else

            MsgBox("Record not Exist")
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        'Ask User to choose the image of the product in modify record portion
        OpenFileDialog2.Filter = "JPG|*.jpg|JPEG|*.jpeg|PNG|*.png"
        OpenFileDialog2.FileName = ""
        OpenFileDialog2.Title = "Select image"

        If (OpenFileDialog2.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            PictureBox2.ImageLocation = OpenFileDialog2.FileName
        Else
            PictureBox2.ImageLocation = ""
        End If

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        'Code executed when modify button is pressed

        Dim img_default As String

        'Read Image name from file
        If File.Exists(Database_file_path + PRODUCT_NAME_3.Text + ".xml") Then

            'Open Streamreader for file reading
            Dim img_file_read As New System.Xml.Serialization.XmlSerializer(GetType(Record))
            Dim img_temp_file As New System.IO.StreamReader(Database_file_path + PRODUCT_NAME_3.Text + ".xml")


            'Read object from file
            Dim temp_data As New Record
            temp_data = CType(img_file_read.Deserialize(img_temp_file), Record)

            'Store the name of the image in temporary variable
            img_default = temp_data.product_image

            'Close the file
            img_temp_file.Close()

        Else

            img_default = ""

        End If




        Dim s1 As String    'Variable for storing image file name

        s1 = System.IO.Path.GetFileName(PictureBox2.ImageLocation)

        If StrComp(s1, "") = 0 Then
            MsgBox("Please Select a Product Image")


        ElseIf StrComp(PRODUCT_ID_3.Text, "") = 0 Then
            MsgBox("Please Enter Product ID")


        ElseIf StrComp(PRICE_3.Text, "") = 0 Then
            MsgBox("Please Enter Product Price")


        ElseIf StrComp(STACK_NUMBER_3.Text, "") = 0 Then
            MsgBox("Please Enter Stack Number")


        ElseIf StrComp(PRODUCT_DESC_3.Text, "") = 0 Then
            MsgBox("Please Enter Product Description")




        Else

            If StrComp(img_default, "") = 0 Then

                'If there is no image stored
                My.Computer.FileSystem.CopyFile(PictureBox2.ImageLocation, Image_file_path + s1)

            Else
                'copy new image

                'Remove old image from directory
                If StrComp(img_default, s1) = 0 Then
                    'do nothing

                Else

                    My.Computer.FileSystem.DeleteFile(Image_file_path + img_default)
                    My.Computer.FileSystem.CopyFile(PictureBox2.ImageLocation, Image_file_path + s1)


                End If
            End If





            'prepare the class for writing in file
            Dim Temp As New Record
            Temp.product_name = PRODUCT_NAME_3.Text
            Temp.product_id = PRODUCT_ID_3.Text
            Temp.number_of_product = Val(AVAILABLE_2.Text)
            Temp.price = Val(PRICE_3.Text)
            Temp.stack_number = STACK_NUMBER_3.Text
            Temp.product_description = PRODUCT_DESC_3.Text
            Temp.product_image = s1



            'Open Streamwriter and serializer for modify file
            Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(Record))
            Dim file_writer As New System.IO.StreamWriter(Database_file_path + Temp.product_name + ".xml")

            'write modified data in file
            writer.Serialize(file_writer, Temp)

            'close the file
            file_writer.Close()

            'Print the success message
            MsgBox("Record Modified Successfully")

            'CLEAR ALL TEXT BOXES
            SEARCH_QUERY_2.Text = ""
            PRODUCT_NAME_3.Text = ""
            PRODUCT_ID_3.Text = ""
            AVAILABLE_2.Text = ""
            PRICE_3.Text = ""
            STACK_NUMBER_3.Text = ""
            PRODUCT_DESC_3.Text = ""
            PictureBox2.ImageLocation = ""

            'Disable Modify Button
            Button6.Enabled = False
        End If


    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        'code executes when search button in delete portion is presses

        'check if the record exist or not
        If File.Exists(Database_file_path + SEARCH_QUERY_3.Text + ".xml") Then

            'Enable delete button
            DELETE_RECORD.Enabled = True

            'Read the record from file
            Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(Record))
            Dim file_read As New System.IO.StreamReader(Database_file_path + SEARCH_QUERY_3.Text + ".xml")

            Dim Temp_record As New Record

            Temp_record = CType(reader.Deserialize(file_read), Record)

            'close the file
            file_read.Close()

            'Update values in respective fields
            PRODUCT_NAME_4.Text = Temp_record.product_name
            PRODUCT_ID_4.Text = Temp_record.product_id
            AVAILABLE_3.Text = Temp_record.number_of_product
            PRICE_4.Text = Temp_record.price
            STACK_NUMBER_4.Text = Temp_record.stack_number
            PRODUCT_DESC_4.Text = Temp_record.product_description


            'Check for product image file
            If File.Exists(Image_file_path + Temp_record.product_image) Then
                PictureBox3.ImageLocation = Image_file_path + Temp_record.product_image
            Else
                PictureBox3.ImageLocation = ""

            End If
        Else
            MsgBox("Record Not Found")

        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start(LinkLabel1.Tag.ToString)
    End Sub

    Private Sub DELETE_RECORD_Click(sender As Object, e As EventArgs) Handles DELETE_RECORD.Click

        'code execute when delete button is pressed

        If delete_password_flag = False Then
            Form4.Show()
        End If


        If delete_password_flag = True Then


            'ask user for file delete confirmation

            If MsgBox("Do you Really Want To Delete This Record ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                If File.Exists(Database_file_path + PRODUCT_NAME_4.Text + ".xml") Then

                    'open record file for image name
                    Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(Record))
                    Dim temp_file As New System.IO.StreamReader(Database_file_path + PRODUCT_NAME_4.Text + ".xml")

                    'Read image File Name
                    Dim Temp_Record As New Record
                    Temp_Record = CType(reader.Deserialize(temp_file), Record)

                    'closing the file
                    temp_file.Close()


                    'Delete main Database file
                    My.Computer.FileSystem.DeleteFile(Database_file_path + PRODUCT_NAME_4.Text + ".xml")

                    'Look for related image file
                    If File.Exists(Image_file_path + Temp_Record.product_image) Then
                        My.Computer.FileSystem.DeleteFile(Image_file_path + Temp_Record.product_image)
                    End If


                    'Clear all output(text) boxes
                    SEARCH_QUERY_3.Text = ""
                    PRODUCT_NAME_4.Text = ""
                    PRODUCT_ID_4.Text = ""
                    AVAILABLE_3.Text = ""
                    PRICE_4.Text = ""
                    STACK_NUMBER_4.Text = ""
                    PRODUCT_DESC_4.Text = ""
                    PictureBox3.ImageLocation = ""

                    'Disable the delete button again
                    DELETE_RECORD.Enabled = False


                    'Show success message
                    MsgBox("Record Deleted Successfully")

                    'disable delete password flag
                    delete_password_flag = False


                End If              'For database file check flag

            End If                   'For message box confirmation flag

        End If                      'For password check flag

    End Sub


    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        'code executed when add in issue portion is pressed

        'check if there is something in the input text boxes or not
        If SEARCH_QUERY_5.TextLength > 0 And QUANTITY.TextLength > 0 Then

            'check if the record exist or not
            If File.Exists(Database_file_path + SEARCH_QUERY_5.Text + ".xml") Then

                'Read the record from file
                Dim file_read As New System.Xml.Serialization.XmlSerializer(GetType(Record))
                Dim stream_read As New System.IO.StreamReader(Database_file_path + SEARCH_QUERY_5.Text + ".xml")


                'Store record in a temporary variable
                Dim Temp_Record As New Record

                Temp_Record = CType(file_read.Deserialize(stream_read), Record)

                'close the file
                stream_read.Close()


                'check for enough quantity available or not
                If Temp_Record.number_of_product < Val(QUANTITY.Text) Then
                    MsgBox("Not sufficient quantity available" + vbNewLine +
                           "Quantity available " & Temp_Record.number_of_product)

                Else

                    'Update Invoice text box with values
                    INVOICE.Text = INVOICE.Text + vbNewLine + Temp_Record.product_name + vbTab + (Val(QUANTITY.Text) * Temp_Record.price).ToString


                    Temp_Record.number_of_product = Temp_Record.number_of_product - Val(QUANTITY.Text)

                    'Modify the record

                    'Open File for writing modified data
                    Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(Record))
                    Dim file_Write As New System.IO.StreamWriter(Database_file_path + SEARCH_QUERY_5.Text + ".xml")

                    'Write modified record In file
                    writer.Serialize(file_Write, Temp_Record)

                    'closing the file
                    file_Write.Close()


                End If              'For Quantity Checking Flag

            Else

                MsgBox("No Data found in Database")

            End If                     'For Record checking flag

        Else
            MsgBox("Please Enter Product Name And Quantity")
        End If



    End Sub
End Class


Public Class Record


    Public product_name As String
    Public product_id As String
    Public number_of_product As Integer
    Public price As Single
    Public stack_number As String
    Public product_image As String
    Public product_description As String

End Class