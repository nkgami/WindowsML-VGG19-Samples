Module Module1

    Sub Main()
        Dim vGG19Test As VGG19Test = New VGG19Test(System.IO.Directory.GetCurrentDirectory() + "\model.onnx")
        Dim imageList As String() = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + "\test")
        For Each str As String In imageList
            Console.WriteLine(str)
            vGG19Test.InferenceImage(str).Wait()
        Next
        Console.ReadLine()
    End Sub
End Module
