Imports System.IO
Public Class JournalEvent
    Public Shared Function AddEvent(ByVal Ligne As String) As Boolean
        Dim PathPrint As String = My.Application.Info.DirectoryPath & "\"
        Dim FileName As String = "Event.txt"
        Dim FullFileName As String = PathPrint & "\" & FileName
        Using sw As New StreamWriter(FullFileName, True)
            sw.AutoFlush = False
            sw.WriteLine(Now.ToShortDateString & " - " & Now.ToShortTimeString & " : " & Ligne)
            sw.Close()
        End Using
        Return False
    End Function
End Class
