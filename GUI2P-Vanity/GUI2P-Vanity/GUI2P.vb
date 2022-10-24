Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Net.NetworkInformation
Imports System.Text.RegularExpressions
Imports System.Net.Http

''' <summary>
''' GUI2p Vanity
''' 
''' authored by @anon
''' 2022 | this project is open source under MIT
''' i2pd-tools license below
''' 
''' credit to i2p team for a wonderful subnet
''' & i2pd-tools team for making this extension possible
''' 
''' I2PD-TOOLS BSD 3-Clause
''' 
'''Copyright (c) 2016, 
'''All rights reserved.
'''
'''Redistribution And use In source And binary forms, with Or without
'''modification, are permitted provided that the following conditions are met: 
'''
'''* Redistributions of source code must retain the above copyright notice, this
'''  list of conditions And the following disclaimer.
'''
'''* Redistributions in binary form must reproduce the above copyright notice,
'''  this list Of conditions And the following disclaimer In the documentation
'''  And/Or other materials provided with the distribution.
'''
'''* Neither the name of i2pd-reseed nor the names of its
'''  contributors may be used To endorse Or promote products derived from
'''  this software without specific prior written permission.
'''
'''THIS SOFTWARE Is PROVIDED BY THE COPYRIGHT HOLDERS And CONTRIBUTORS "AS IS"
'''And ANY EXPRESS Or IMPLIED WARRANTIES, INCLUDING, BUT Not LIMITED TO, THE
'''IMPLIED WARRANTIES Of MERCHANTABILITY And FITNESS For A PARTICULAR PURPOSE ARE
'''DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER Or CONTRIBUTORS BE LIABLE
'''For ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, Or CONSEQUENTIAL
'''DAMAGES(INCLUDING, BUT Not LIMITED To, PROCUREMENT OF SUBSTITUTE GOODS Or
'''SERVICES; LOSS Of USE, DATA, Or PROFITS; Or BUSINESS INTERRUPTION) HOWEVER
'''CAUSED And ON ANY THEORY OF LIABILITY, WHETHER In CONTRACT, STRICT LIABILITY,
'''Or TORT (INCLUDING NEGLIGENCE Or OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
'''OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'''
''' 
''' </summary>
Public Class GUI2P

    ''' <summary>
    ''' Loads the form and initializes components
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub GUI2P_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        threads.Text = Environment.ProcessorCount - 1
        CheckForIllegalCrossThreadCalls = False
        runWebServer()
    End Sub

    'Define globals
    Dim p As ProcessStartInfo
    Dim ps As Process
    Dim list As String
    Dim rgx As New Regex("[^a-zA-Z0-9\n\r]")
    Dim scan As System.Threading.Thread
    Dim scanning As Boolean = False
    Dim found As Integer = 0

    ''' <summary>
    ''' Button that starts and stops the thread
    ''' </summary>
    ''' <param name="sender">Parent subroutine caller</param>
    ''' <param name="e">Event arguments</param>
    Private Sub startStop_Click(sender As Object, e As EventArgs) Handles startStop.Click
        ' Checks whether to start or stop the thread
        If startStop.Text = "Start" Then

            ' Check if the i2pd-tools' executables exist
            If File.Exists("vain.exe") AndAlso File.Exists("keyinfo.exe") Then

                ' Check if the format of the thread count and if it's too high
                If IsNumeric(threads.Text) AndAlso CInt(threads.Text) <= Environment.ProcessorCount AndAlso CInt(threads.Text) > 0 Then

                    ' Check if the address list is empty
                    If Not String.IsNullOrEmpty(addressList.Text) Then

                        ' Limit input to the address list
                        addressList.ReadOnly = True

                        ' Sanitize the address list
                        sanitize()

                        logOutput("Scan started!")

                        ' Initialize child processes
                        scan = New System.Threading.Thread(AddressOf scanner)
                        p = New ProcessStartInfo
                        p.FileName = Application.StartupPath + "vain.exe"
                        p.CreateNoWindow = True

                        ' Create the regex expression from the address list
                        list = """("

                        For Each line As String In addressList.Lines
                            list = list + line.ToLower + "|"
                        Next

                        list = list.Substring(0, list.Length - 1) + ").*"""

                        p.Arguments = list & " -r -t " & Int(threads.Text)

                        ' Start child processes
                        ps = Process.Start(p)
                        scanning = True

                        scan.Start()
                        startStop.Text = "Stop"
                    Else
                        ' Empty address list
                        logOutput("ERROR: Address list cannot be empty.")
                    End If
                Else
                    ' Invalid thread count
                    logOutput("ERROR: Invalid number of processor threads. Please enter correct number of threads.")
                End If
            Else
                ' Missing executables
                logOutput("ERROR: Missing I2PD-Tools vain.exe or keyinfo.exe please add missing executable to folder.")
            End If
        Else
            ' Stop the thread and child processes
            ps.Kill()

            scanning = False
            startStop.Text = "Start"
            logOutput("Scan stopped!")

            ' Enable address list input
            addressList.ReadOnly = False

        End If
    End Sub

    ''' <summary>
    ''' Sanitizes address list input
    ''' </summary>
    Public Sub sanitize()

        ' Convert everything to lowercase
        addressList.Text = addressList.Text.ToLower

        ' Convert spaces to new lines
        addressList.Text = addressList.Text.Replace(" ", Environment.NewLine)

        ' Replace illegal characters with empty
        addressList.Text = rgx.Replace(addressList.Text, "")

        'Remove duplicates
        Dim lines As String() = addressList.Lines
        Dim cleaned = lines.Distinct.ToArray
        addressList.Lines = cleaned

        ' Use linq to remove empty lines
        addressList.Lines = addressList.Lines.Where(Function(line) line.Trim() <> String.Empty).ToArray()
        logOutput("Address list sanitized!")

    End Sub

    Public Sub RemoveDuplications(Of T)(ByRef arr() As T)
        Dim filteredList As List(Of T) = New List(Of T)

        For Each element As T In arr

            ' If element is already added in the list, skip the elementi
            If filteredList.Contains(element) Then Continue For

            ' Add element to the list
            filteredList.Add(element)
        Next

        ' Return filtered array
        arr = filteredList.ToArray()
    End Sub


    ''' <summary>
    ''' Thread subroutine - Scans for i2pd exit and moves private.dat to appropriate location
    ''' </summary>
    Public Sub scanner()
        ' Check if the thread is supposed to be running
        While scanning

            ' Check if the process has exited and the thread is about to be stopped
            If ps.HasExited AndAlso startStop.Text = "Stop" Then

                Threading.Thread.Sleep(2500)

                ' Check if the key file exists
                If File.Exists("private.dat") Then

                    ' Sleep the thread to ensure file is completely written
                    Threading.Thread.Sleep(3500)

                    ' Initialize new child process
                    Dim oProcess As New Process()
                    Dim oStartInfo As New ProcessStartInfo("keyinfo.exe", "private.dat")
                    oStartInfo.UseShellExecute = False
                    oStartInfo.RedirectStandardOutput = True
                    oProcess.StartInfo = oStartInfo

                    ' Start the b32 resolver
                    oProcess.Start()

                    Dim address As String
                    Dim sOutput As String

                    ' Use a stream reader to get output from child process
                    Using oStreamReader As System.IO.StreamReader = oProcess.StandardOutput

                        ' Get the address from the child process' output stream
                        address = oStreamReader.ReadToEnd()

                        ' Sanitize the b32 address
                        sOutput = address.Replace(".b32.i2p", "").Trim()

                    End Using

                    If sOutput.Trim = "bad key file format" Then
                        File.Delete("private.dat")
                        logOutput("Deleted bad key.")
                    End If

                    'Check if address is contained in address list
                    'Bug in vain.exe causes false addresses this should fix it
                    Dim positive As Boolean = False
                    For Each line As String In addressList.Lines
                        If sOutput.StartsWith(line) Then
                            positive = True
                            Exit For
                        End If
                    Next

                    If positive = True Then
                        ' Check if directories exist
                        If Not Directory.Exists("eepSites") Then
                            My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "eepSites")
                        End If

                        ' Check if the b32 address folder exists
                        If Not Directory.Exists(sOutput) Then
                            My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "eepSites\" & sOutput)
                        End If

                        ' Move the keyfile to the appropriate folder
                        File.Move("private.dat", "eepSites\" & sOutput & "\private.dat")

                        ' Restart vain and the thread
                        ps = Process.Start(p)

                        logOutput("Found: " & address.Trim)

                        ' Add to found URLs
                        found += 1

                        Text = "GUI2P Vanity - Found: " & found
                    Else
                        File.Delete("private.dat")
                        logOutput("Caught and removed false address.")
                    End If

                Else

                    ' Keyfile doesn't exist, restart child processes
                    ps = Process.Start(p)

                End If

            End If

            ' Sleep the thread to ensure smooth operation
            Threading.Thread.Sleep(1500)

        End While
    End Sub

    ''' <summary>
    ''' Handles outputing to the log
    ''' </summary>
    ''' <param name="value">Log event to be printed</param>
    Public Sub logOutput(ByVal value As String)

        Try
            ' Write text to the log output
            log.Text = log.Text & Date.Now.ToString & " | " & value & vbNewLine

            ' Scroll to latest update
            log.Select(log.Text.Length - 1, 0)
            log.ScrollToCaret()
        Catch ex As Exception
            'Catch random crash caused by UI
        End Try

    End Sub


    ''' <summary>
    ''' Saves the address list
    ''' </summary>
    ''' <param name="sender">Parent subroutine caller</param>
    ''' <param name="e">Event arguments</param>
    Private Sub Save_Click(sender As Object, e As EventArgs) Handles Save.Click

        ' Sanitize the address list
        sanitize()

        ' Set up a new IO writer
        Dim writer As New StreamWriter("filter.ini")

        ' Write the address list to filter.ini
        writer.Write(addressList.Text)

        ' Close the IO writer
        writer.Close()

        logOutput("Filter list saved!")

    End Sub

    ''' <summary>
    ''' Loads the address list
    ''' </summary>
    ''' <param name="sender">Parent subroutine caller</param>
    ''' <param name="e">Event arguments</param>
    Private Sub Load_Click(sender As Object, e As EventArgs) Handles Load.Click

        ' Check if filters.ini exists
        If File.Exists("filter.ini") Then

            ' Set up a new IO reader
            Dim reader As New StreamReader("filter.ini")

            ' Read the address list from filters.ini
            addressList.Text = reader.ReadToEnd()
            reader.Close()

            logOutput("Filter list loaded!")

        Else

            logOutput("ERROR: filter.ini does not exist")

        End If
    End Sub

    ''' <summary>
    ''' Handles app closing and terminates child processes
    ''' </summary>
    ''' <param name="sender">Parent subroutine caller</param>
    ''' <param name="e">Event arguments</param>
    Private Sub GUI2P_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            scanning = False
            ' If PS is instantiated and possibly running
            If ps IsNot Nothing Then

                ' Kill child processes
                ps.Kill()

            End If

        Catch ex As Exception
            'Ignore exceptions
        End Try
    End Sub

    ''' <summary>
    ''' Clears the log
    ''' </summary>
    ''' <param name="sender">Parent subroutine caller</param>
    ''' <param name="e">Event arguments</param>
    Private Sub clear(sender As Object, e As EventArgs) Handles clearLog.Click
        log.Clear()
    End Sub

    ''' <summary>
    ''' Button to cycle through different list sorting methods
    ''' </summary>
    ''' <param name="sender">Parent subroutine caller</param>
    ''' <param name="e">Event arguments</param>
    Private Sub shuffle(sender As Object, e As EventArgs) Handles shuffleButton.Click

        ' Check the state of the order button
        Select Case shuffleButton.Text

            Case "Random"

                ' Alphabetically sort the adddress list
                Dim lines = addressList.Lines
                Array.Sort(lines)
                addressList.Lines = lines
                shuffleButton.Text = "Alphabetical"
                logOutput("Sorted alphabetically")

            Case "Alphabetical"

                ' Sort the address list by length
                Dim sorted = addressList.Lines.OrderBy(Function(x) x.Length).ThenBy(Function(x) x).ToArray()
                addressList.Lines = sorted
                shuffleButton.Text = "Length"
                logOutput("Sorted by length")

            Case "Length"

                ' Randomly shuffle the address list
                Dim rng As New Random
                Dim lines = addressList.Lines
                lines = lines.OrderBy(Function(line) rng.NextDouble()).ToArray()
                addressList.Lines = lines
                shuffleButton.Text = "Random"
                logOutput("Sorted randomly")

        End Select

    End Sub

    'Web Server'

    Dim host_name As String = Dns.GetHostName()

    Dim ip_address As String = Dns.GetHostByName(host_name).AddressList(0).ToString()
    Dim args As String() = {"listen", "http://*:6150"}

    Public Sub runWebServer()
        Dim hostName As String = Environment.MachineName

        If Not HttpListener.IsSupported Then
            logOutput("Unable to start web server. HttpListener class not supported.")
            Return
        End If

        If args(0).ToLower = "listen" Then
            Dim netInterface As String = args(1)
            Dim userNetInterface As String = netInterface.Replace("*", "127.0.0.1")

            Dim listener As New HttpListener
            listener.IgnoreWriteExceptions = True

            'This is where we tell the HttpListener what interface
            'and port to listen on
            listener.Prefixes.Add($"http://*:6150/")

            Try
                'Start listenting for HTTP requests
                listener.Start()
            Catch ex As Exception

                logOutput($"Error trying to start server : {ex.Message}")

                Return
            End Try

            logOutput($"Webserver started on {userNetInterface}")

            'Start the web server on another thread
            Threading.ThreadPool.QueueUserWorkItem(Sub()

                                                       Dim visCount As Integer

                                                       Do
                                                           Dim context = listener.GetContext
                                                           Dim response As HttpListenerResponse = context.Response
                                                           Dim request = context.Request

                                                           Dim requestString = request.RawUrl.ToLower

                                                           Select Case True


                                                                       'Redirect to the home page
                                                               Case requestString = "/"
                                                                   response.Redirect("/home")


                                                               Case requestString = "/home"
                                                                   'Home page
                                                                   '******************************************
                                                                   visCount += 1

                                                                   WriteWebPageResponse(GetHomePage(hostName, visCount), response)

                                                               Case requestString = "/load"

                                                                   Load.PerformClick()

                                                                   response.Redirect("/home")

                                                               Case requestString = "/save"

                                                                   Using reader = New StreamReader(request.InputStream, request.ContentEncoding)
                                                                       addressList.Text = reader.ReadToEnd().Replace("address=", "").Replace("%0D%0A", Environment.NewLine)
                                                                       Save.PerformClick()
                                                                       response.Redirect("/home")
                                                                   End Using


                                                               Case requestString = "/startstop"

                                                                   Using reader = New StreamReader(request.InputStream, request.ContentEncoding)
                                                                       threads.Text = reader.ReadToEnd().Replace("threads=", "")
                                                                       If startStop.Text = "Start" Then

                                                                           If File.Exists("vain.exe") AndAlso File.Exists("keyinfo.exe") Then

                                                                               ' Check if the format of the thread count and if it's too high
                                                                               If IsNumeric(threads.Text) AndAlso CInt(threads.Text) <= Environment.ProcessorCount AndAlso CInt(threads.Text) > 0 Then

                                                                                   ' Check if the address list is empty
                                                                                   If Not String.IsNullOrEmpty(addressList.Text) Then

                                                                                       ' Limit input to the address list
                                                                                       addressList.ReadOnly = True

                                                                                       ' Sanitize the address list
                                                                                       sanitize()

                                                                                       logOutput("Scan started!")

                                                                                       ' Initialize child processes
                                                                                       scan = New System.Threading.Thread(AddressOf scanner)
                                                                                       p = New ProcessStartInfo
                                                                                       p.FileName = Application.StartupPath + "vain.exe"
                                                                                       p.CreateNoWindow = True

                                                                                       ' Create the regex expression from the address list
                                                                                       list = """("

                                                                                       For Each line As String In addressList.Lines
                                                                                           list = list + line.ToLower + "|"
                                                                                       Next

                                                                                       list = list.Substring(0, list.Length - 1) + ").*"""

                                                                                       p.Arguments = list & " -r -t " & Int(threads.Text)

                                                                                       ' Start child processes
                                                                                       ps = Process.Start(p)
                                                                                       scanning = True

                                                                                       scan.Start()
                                                                                       startStop.Text = "Stop"
                                                                                   Else
                                                                                       ' Empty address list
                                                                                       logOutput("ERROR: Address list cannot be empty.")
                                                                                   End If
                                                                               Else
                                                                                   ' Invalid thread count
                                                                                   logOutput("ERROR: Invalid number of processor threads. Please enter correct number of threads.")
                                                                               End If
                                                                           Else
                                                                               ' Missing executables
                                                                               logOutput("ERROR: Missing I2PD-Tools vain.exe or keyinfo.exe please add missing executable to folder.")
                                                                           End If

                                                                       Else

                                                                           ' Stop the thread and child processes
                                                                           ps.Kill()

                                                                           scanning = False
                                                                           startStop.Text = "Start"
                                                                           logOutput("Scan stopped!")

                                                                           ' Enable address list input
                                                                           addressList.ReadOnly = False

                                                                       End If
                                                                       response.Redirect("/home")
                                                                   End Using

                                                               Case requestString = "/order"

                                                                   shuffleButton.PerformClick()
                                                                   response.Redirect("/home")

                                                               Case requestString = "/clear"

                                                                   clearLog.PerformClick()
                                                                   response.Redirect("/home")

                                                               Case requestString.Contains("/download")
                                                                   'This path is used to download a private.dat file
                                                                   '******************************************
                                                                   Dim path As String = request.RawUrl.Split("/download/")(1).Replace("/", "\")
                                                                   Dim data() As Byte = File.ReadAllBytes(path & "\private.dat")
                                                                   WriteDownloadResponse(data, path.Split("eepSites\")(1).Substring(0, 9) & "-private.dat", response)

                                                               Case requestString.Contains("/delete")

                                                                   Dim path As String = request.RawUrl.Split("/delete/")(1).Replace("/", "\")
                                                                   Directory.Delete(path, True)
                                                                   logOutput("Deleted " & path.Split("eepSites\")(1).Substring(0))
                                                                   response.Redirect("/home")

                                                               Case Else
                                                                   '404 Error. This happens when a request is made for a page
                                                                   'or resource from a path that doesn't exist.
                                                                   '******************************************

                                                                   response.StatusCode = HttpStatusCode.NotFound
                                                                   response.Redirect("/home")


                                                           End Select

                                                           'Send reponse to web browser by closing the output stream
                                                           response.OutputStream.Close()
                                                       Loop
                                                   End Sub)

        End If

    End Sub

    Private Sub WriteWebPageResponse(ByVal text As String, ByVal r As HttpListenerResponse)

        Try
            Dim data As Byte() = System.Text.Encoding.UTF8.GetBytes(text)

            r.ContentLength64 = data.Length
            r.OutputStream.Write(data, 0, data.Length)
        Catch ex As Exception
        End Try

    End Sub

    Private Sub WriteDownloadResponse(ByVal file As Byte(), ByVal saveAsFileName As String, ByVal r As HttpListenerResponse)

        'This is how web servers induce browsers to download files from them.
        '*********************************************************
        r.AddHeader("Content-Disposition", $"attachment; filename=""{saveAsFileName}""")
        r.ContentType = "application/octet-stream"
        r.ContentLength64 = file.Length
        r.OutputStream.Write(file, 0, file.Length)
    End Sub

    Private Function GetInfo(ByVal file As String) As String

        Dim reader As New StreamReader(file)
        Return reader.ReadToEnd()

    End Function

    Private Function GetHomePage(ByVal hostName As String, ByVal visCount As Integer) As String
        'This function generates the HTML for the home page.
        '*********************************************************

        Dim sb As New StringBuilder

        sb.Append(My.Resources.Content)

        sb.Replace("THREADMAX", Environment.ProcessorCount.ToString)

        sb.Replace("THREADNUM", threads.Text)

        sb.Replace("RUNNING", startStop.Text)

        sb.Replace("LOGS", log.Text)

        sb.Replace("ADDRESSES", addressList.Text)

        sb.Replace("STATS", Me.Text)

        sb.Replace("ORGANIZE", shuffleButton.Text)

        If startStop.Text = "Start" Then
            sb.Replace("REFRESHTIME", "999999")
        Else
            sb.Replace("REFRESHTIME", "30")
        End If

        If Directory.Exists("eepSites") AndAlso Directory.EnumerateDirectories("eepSites").Count > 0 Then
            Dim dirs As New StringBuilder
            For Each Dir As String In Directory.GetDirectories("eepSites")
                Console.WriteLine(Dir)
                dirs.AppendLine("<a href=" & """" & "/download/eepSites/" & Dir & "/" & """>" & "<div class=" & """found" & """><h3>" & Dir & "</h3><a class=" & """button" & """ href=" & """" & "/delete/eepSites/" & Dir & """" & ">Delete</a></div></a>")
            Next
            sb.Replace("FNDADDY", dirs.ToString.Replace("eepSites\", ""))
        Else
            sb.Replace("FNDADDY", "<h3>No Addresses Found</h3>")
        End If

        Return sb.ToString
    End Function

End Class
