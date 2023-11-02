Imports System
Imports System.IO
Imports System.Collections
Imports Microsoft.VisualBasic
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Linq
Imports System.Object
Imports System.Threading

' ProRut Simulated Straightedge Software
' Stephen Augustyn
' CSRA
' March 31, 2016

Public Class Form1

    ' declare global variables
    Dim staname As String 'station filename
    Dim binX As New ArrayList 'results of binary file processing
    Dim binZ As New ArrayList 'results of binary file processing
    Dim csvfile As String 'results full filename, new after each subsequent program run
    Dim csvfilename As String = "\MaxRutDepths" 'results filename
    Dim csvfileext As String = ".csv" 'results filename extension

    Public Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        'timestamp
        Dim time1 As DateTime
        time1 = Now

        'clear necessary variables
        staname = ""

        'set wait cursor
        Cursor = Cursors.WaitCursor

        ' internal settings
        Dim sectionLimit As Integer = 1000 ' limit of number of rows of section pass correction file
        Dim leicalimit As Integer = 20000 ' limit of number of rows of leica reference file
        Dim samplespacing As Single = 0.082021 ' (ft) used to fit Leica to correct sample spacing

        ' variables
        Dim secdiff13 As Integer = 0 ' differences between vehicle pass and section pass for sections 1-3 and 4-6
        Dim secdiff46 As Integer = 0

        ' check user inputs are valid
        If IsNumeric(TextBox4.Text) Then
        Else
            MsgBox("Invalid Straightedge Length Input")
            Cursor = Cursors.Default
            Exit Sub
        End If
        If IsNumeric(TextBox5.Text) Or TextBox5.Text = "" Then
        Else
            MsgBox("Invalid Center Location Input")
            Cursor = Cursors.Default
            Exit Sub
        End If
        If TextBox5.Text = "" Then
            TextBox5.Text = 17.1
            MsgBox("Center Location blank, set to default of 17.1 ft.")
        End If
        If IsNumeric(TextBox6.Text) Or TextBox6.Text = "" Then
        Else
            MsgBox("Invalid Range Input")
            Cursor = Cursors.Default
            Exit Sub
        End If
        'check data directory
        If Not Directory.Exists(TextBox7.Text) Then
            MsgBox("Invalid Data Directory")
            Cursor = Cursors.Default
            Exit Sub
        End If

        ' read user inputs
        Dim stationstring As String = TextBox3.Text
        Dim stations As Array = Split(stationstring, ",")
        Dim selength As Single = TextBox4.Text
        Dim centerloc As Single = TextBox5.Text
        Dim range As Single
        Dim datadirectory As String = TextBox7.Text
        Dim leicafile As String = TextBox8.Text

        ' check if Leica file is specefied, and if it exists
        If leicafile <> "" And leicafile <> "Select Leica Reference File..." Then
            If File.Exists(leicafile) Then
                leicafile = TextBox8.Text
            Else
                MsgBox("Leica Reference File does not exist.")
                Cursor = Cursors.Default
                Exit Sub
            End If
        Else
            leicafile = My.Application.Info.DirectoryPath & "\Leica_Default.csv"
        End If

        ' fit leica to sample spacing
        Dim R2Rx(0 To 784) As Single
        For i = 0 To 783
            R2Rx(i + 1) = R2Rx(i) + samplespacing
        Next
        PrintValues(R2Rx)

        'find straightedge centerpoint
        Dim secenter As Integer
        Dim closestdif As Single = 999
        For i = 0 To 784
            If R2Rx(i) - centerloc >= 0 Then
                If R2Rx(i) - centerloc < closestdif Then
                    secenter = i
                    closestdif = R2Rx(i) - centerloc
                End If
            End If
        Next

        ' calculate other straightedge points
        Dim secenterl As Integer = secenter - 1
        Dim leftend As Integer
        Dim rightend As Integer
        leftend = secenter - Math.Round((selength / 2) / samplespacing)
        rightend = secenterl + Math.Round((selength / 2) / samplespacing)

        ' default points for 16 ft straightedge at center location 17.1 ft are:
        'Dim leftend As Integer = 111
        'Dim secenterl As Integer = 208
        'Dim secenter As Integer = 209
        'Dim rightend As Integer = 306

        ' note starting straightedge position values
        Console.WriteLine("Starting straightedge position: " & leftend & ", " & secenterl & ", " & secenter & ", " & rightend)

        ' move straightedge if too far left and/or too long
        If leftend < 0 Then
            secenterl = secenterl - leftend
            secenter = secenter - leftend
            rightend = rightend - leftend
            leftend = leftend - leftend
            If rightend > 784 Then
                secenterl = (784 / 2) - 1
                secenter = (784 / 2)
                rightend = 784
                MsgBox("Straightedge longer than profile, shrunk to fit")
            Else
                MsgBox("Straightedge out of range, shifted right")
            End If
        End If

        ' move straightedge if too far right and/or too long
        If rightend > 784 Then
            leftend = leftend - (rightend - 784)
            secenterl = secenterl - (rightend - 784)
            secenter = secenter - (rightend - 784)
            rightend = rightend - (rightend - 784)
            If leftend < 0 Then
                secenterl = (784 / 2) - 1
                secenter = (784 / 2)
                leftend = 0
                MsgBox("Straightedge longer than profile, shrunk to fit")
            Else
                MsgBox("Straightedge out of range, shifted left")
            End If
        End If

        ' note new straightedge position values
        Console.WriteLine("Final straightedge position: " & leftend & ", " & secenterl & ", " & secenter & ", " & rightend)

        'set ranges to measure rut depth
        Dim leftrange As Integer = leftend ' range to measure for max rut depth
        Dim rightrange As Integer = rightend ' range to measure for max rut depth
        If TextBox6.Text = "" Then
            leftrange = leftend
            rightrange = rightend
        Else
            range = TextBox6.Text
            leftrange = secenter - Math.Round((range / 2) / samplespacing)
            rightrange = secenterl + Math.Round((range / 2) / samplespacing)
        End If

        ' check if range is more than straighedge length
        If leftrange < leftend Or rightrange > rightend Then
            leftrange = leftend
            rightrange = rightend
            MsgBox("Range greater than Straightedge Length, set to Straightedge Length")
        End If

        ' note range of rut depth measurement
        Console.WriteLine("Range left and right: " & leftrange & ", " & rightrange)

        ' open csv file for writing and record header
        'Dim csvfile As String = My.Application.Info.DirectoryPath & "\MaxRutDepths.csv"

        csvfile = datadirectory & csvfilename & csvfileext
        For i = 1 To 999999
            If File.Exists(csvfile) Then
                csvfile = datadirectory & csvfilename & i & csvfileext
            Else
                Exit For
            End If
        Next
        Dim outfileheader As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvfile, False)
        outfileheader.WriteLine("Date, Vehicle Pass, Section Pass, Station, Max Rut Depth (in), Max Rut Depth Location (ft), " & "Corrected Profile (in) - Sample Spacing (ft): " & samplespacing & ",,,,," & "Straightedge Length (ft): ," & selength & ", Center Location(ft): ," & centerloc)
        outfileheader.Close()

        ' open Vehicle Pass - Section Pass 1 through 3 correction
        Dim MyReader13 As New Microsoft.VisualBasic.
        FileIO.TextFieldParser(
          My.Application.Info.DirectoryPath & "\sectionpass1-3.txt")

        MyReader13.TextFieldType = FileIO.FieldType.Delimited
        MyReader13.SetDelimiters(Chr(9)) ' Chr(9) is tab

        Dim currentRow13 As String()
        Dim section13(sectionLimit, 1) As Double
        Dim Row13 As Integer = 0
        Dim Column13 As Integer = 0

        While Not MyReader13.EndOfData
            currentRow13 = MyReader13.ReadFields()
            Dim currentField As String
            For Each currentField In currentRow13
                section13(Row13, Column13) = currentField
                Column13 += 1
            Next
            Row13 += 1
            Column13 = 0
        End While

        ' open Vehicle Pass - Section Pass 4 through 6 correction
        Dim MyReader46 As New Microsoft.VisualBasic.
        FileIO.TextFieldParser(
          My.Application.Info.DirectoryPath & "\sectionpass4-6.txt")

        MyReader46.TextFieldType = FileIO.FieldType.Delimited
        MyReader46.SetDelimiters(Chr(9)) ' Chr(9) is tab

        Dim currentRow46 As String()
        Dim section46(sectionLimit, 1) As Double
        Dim Row46 As Integer = 0
        Dim Column46 As Integer = 0

        While Not MyReader46.EndOfData
            currentRow46 = MyReader46.ReadFields()
            Dim currentField As String
            For Each currentField In currentRow46
                section46(Row46, Column46) = currentField
                Column46 += 1
            Next
            Row46 += 1
            Column46 = 0
        End While

        ' read leica full file
        Dim MyReaderL As New Microsoft.VisualBasic.
        FileIO.TextFieldParser(leicafile)

        MyReaderL.TextFieldType = FileIO.FieldType.Delimited
        MyReaderL.SetDelimiters(",")

        Dim currentRowL As String()
        Dim LeicaL(leicalimit, 2) As Single
        Dim RowL As Integer = 0
        Dim ColumnL As Integer = 0
        Dim LengthL As Integer

        While Not MyReaderL.EndOfData
            currentRowL = MyReaderL.ReadFields()
            Dim currentField As String
            For Each currentField In currentRowL
                If IsNumeric(currentField) Then
                    LeicaL(RowL, ColumnL) = currentField
                    ColumnL += 1
                End If
            Next
            RowL += 1
            ColumnL = 0
        End While
        LengthL = RowL - 1

        ' zero leica northing
        Dim LeicaNZ(LengthL) As Single
        For i = 1 To LengthL
            LeicaNZ(i) = LeicaL(i, 0) - LeicaL(1, 0)
        Next

        ' zero leica elevation and convert to inches
        Dim LeicaEZ(LengthL) As Single
        For i = 1 To LengthL
            LeicaEZ(i) = (LeicaL(i, 2) - LeicaL(1, 2)) * 12
        Next

        ' automatically lookup Leica values at R2R spacing
        Dim LeicaEZS(0 To 784) As Single
        For i = 0 To 784 ' R2R samples
            Dim LookupDif As Single = 999
            Dim LookupLoc As Integer = 0
            For j = 0 To LengthL - 1 ' Leica samples
                If Math.Abs(R2Rx(i) - LeicaNZ(j)) < LookupDif Then
                    LookupDif = Math.Abs(R2Rx(i) - LeicaNZ(j))
                    LookupLoc = j
                End If
            Next
            LeicaEZS(i) = LeicaEZ(LookupLoc)
        Next

        'end Leica

        ' decide wether to list .bin or .txt files based on binary file checkbox
        Dim refsuffix As String
        If bincheckbox.Checked = True Then
            refsuffix = "*ef.bin"
        Else
            refsuffix = "*ef.txt"
        End If

        'Get list of Folders
        For Each Dir As String In Directory.GetDirectories(datadirectory)
            Console.WriteLine(Dir)
            Dim Date3 As String
            Date3 = Mid(Dir, Dir.LastIndexOf("\") + 2, 8)

            'Get list of bin subfolders
            For Each bin As String In Directory.GetDirectories(Dir)
                Console.WriteLine(bin)

                'Get list of Ref files (.bin or .txt)
                For Each Refname As String In Directory.GetFiles(bin, refsuffix)

                    ' changes refname for individual files if selected, and checks that it exists
                    If TextBox1.Text <> "" And TextBox1.Text <> "Select Individual R2R Reference File..." Then
                        If File.Exists(TextBox1.Text) Then
                            Refname = TextBox1.Text
                        Else
                            Cursor = Cursors.Default
                            MsgBox("Individual R2R Reference File does not exist.")
                            Exit Sub
                        End If
                    End If

                    Console.WriteLine(Refname)

                    'record date and vehicle pass for each ref file
                    Dim Date1 As String
                    Dim Date2 As String
                    Date1 = Path.GetFileName(Refname)
                    Date2 = Mid(Date1, 1, 8)
                    Log.Items.Add("Reading File: " & Date1)

                    ' record vehicle pass
                    Dim spot1 As Integer
                    Dim spot2 As Integer
                    Dim Vpass As String
                    spot1 = Date1.IndexOf("_") + 2
                    spot2 = Date1.LastIndexOf("_") - 9
                    Vpass = Mid(Date1, spot1, spot2)

                    ' process Ref bin file if Read from binary files checkbox is selected
                    Dim RefX As New ArrayList
                    Dim RefZ As New ArrayList
                    Dim bincount As Integer = 0
                    If bincheckbox.Checked = True Then
                        If firstbinread = True Then Call StartBin(Refname) : firstbinread = False
                        Call StartBin(Refname)
                        If LDecimate < 1 Then Cursor = Cursors.Default : Exit Sub
                        For Each [object] In binX 'write .bin conversion results to RefX, RefZ variables
                            RefX.Add(binX(bincount))
                            RefZ.Add(binZ(bincount))
                            bincount += 1
                        Next
                    Else
                        'Read Reference File straight from .txt
                        Dim MyReader As New Microsoft.VisualBasic.
                        FileIO.TextFieldParser(Refname)
                        MyReader.TextFieldType = FileIO.FieldType.FixedWidth
                        MyReader.SetFieldWidths(8, 8)
                        Dim currentRow As String()
                        Dim Ref(849, 1) As Double
                        Dim Row As Integer = 0
                        Dim Column As Integer = 0

                        While Not MyReader.EndOfData
                            currentRow = MyReader.ReadFields()
                            Dim currentField As String
                            For Each currentField In currentRow
                                Ref(Row, Column) = currentField
                                Column += 1
                            Next
                            RefX.Add(Ref(Row, 0))
                            RefZ.Add(Ref(Row, 1))
                            Row += 1
                            Column = 0
                        End While
                    End If


                    ' shorten longer Reference files
                    If RefX.Count > 785 Then
                        RefX.RemoveRange(785, RefX.Count - 785)
                        RefZ.RemoveRange(785, RefZ.Count - 785)
                    End If


                    ' lengthens shorter Reference files
                    If RefX.Count < 785 Then
                        For i = RefX.Count To 784
                            RefX.Add(RefX(RefX.Count - 1) + RefX(1))
                            RefZ.Add(RefZ(RefZ.Count - 1))
                        Next
                    End If


                    ' subtract leica from Ref
                    Dim RLDifM As New ArrayList
                    Dim RowSubM As Integer = 0
                    For Each [object] In RefZ
                        RLDifM.Add(RefZ(RowSubM) - LeicaEZS(RowSubM))
                        RowSubM += 1
                    Next

                    ' create 4th order polynomial regression
                    ' first create X matrix and Y matrix
                    Dim xmat(0 To 784, 0 To 4) As Double
                    Dim ymat(0 To 784) As Double
                    For i = 0 To 784
                        xmat(i, 0) = 1
                        xmat(i, 1) = (i + 1)
                        xmat(i, 2) = (i + 1) ^ 2
                        xmat(i, 3) = (i + 1) ^ 3
                        xmat(i, 4) = (i + 1) ^ 4
                        ymat(i) = RLDifM(i)
                    Next

                    ' transpose X matrix
                    Dim xmatt(0 To 4, 0 To 784) As Double
                    For i = 0 To 784
                        xmatt(0, i) = 1
                        xmatt(1, i) = (i + 1)
                        xmatt(2, i) = (i + 1) ^ 2
                        xmatt(3, i) = (i + 1) ^ 3
                        xmatt(4, i) = (i + 1) ^ 4
                    Next

                    ' multiply X-transpose and Y matrices
                    Dim xmattmy(0 To 4, 0) As Double
                    For k = 0 To 4
                        For j = 0 To 0
                            Dim xmattmysum As Double = 0
                            For i = 0 To 784
                                xmattmysum += (xmatt(k, i) * ymat(i))
                            Next
                            xmattmy(k, j) = xmattmysum
                        Next
                    Next

                    ' create x matrix transposed multiplied by x matrix, with inverse of that taken
                    Dim xmattminv(0 To 4, 0 To 4) As Double
                    xmattminv = {{0.032338970074262, -0.000494033870397686, 0.00000220128836085853, -0.00000000373653415677065, 0.00000000000214059881673501},
                    {-0.000494033870397686, 0.0000100232011100144, -0.0000000501770824801453, 0.0000000000907902556261324, -0.0000000000000541579461651725},
                    {0.00000220128836085853, -0.0000000501770824801453, 0.000000000267818723882022, -0.000000000000504669785936706, 0.000000000000000309604250176496},
                    {-0.00000000373653415677065, 0.0000000000907902556261324, -0.000000000000504669785936706, 0.000000000000000978047388889411, -6.12470765345122E-19},
                    {0.00000000000214059881673501, -0.0000000000000541579461651725, 0.000000000000000309604250176496, -6.12470765345122E-19, 3.89612446148358E-22}}

                    'multiply inverse by product of x-transpose and Y matrix to create polynomial coefficients
                    Dim coeff(0 To 4, 0) As Double
                    For k = 0 To 4
                        For j = 0 To 0
                            Dim coeffsum As Double = 0
                            For i = 0 To 4
                                coeffsum += (xmattminv(k, i) * xmattmy(i, j))
                            Next
                            coeff(k, j) = coeffsum
                        Next
                    Next

                    ' create 4th Order Polynomial curve
                    Dim curve As New ArrayList
                    For i = 0 To 784
                        curve.Add(coeff(0, 0) + (coeff(1, 0) * xmat(i, 1)) + (coeff(2, 0) * (xmat(i, 1) ^ 2)) + (coeff(3, 0) * (xmat(i, 1) ^ 3)) + (coeff(4, 0) * (xmat(i, 1) ^ 4)))
                    Next

                    ' plot leica, difference, and 4th order polynomial
                    Me.Chart1.Series("Leica").Points.DataBindXY(LeicaNZ, LeicaEZ)
                    Me.Chart1.Series("Leica Sample Spaced").Points.DataBindXY(RefX, LeicaEZS)
                    Me.Chart1.Series("R2R - Leica").Points.DataBindXY(RefX, RLDifM)
                    Me.Chart1.Series("4th Order Polynomial").Points.DataBindXY(RefX, curve)

                    'Create file string for each station of Ref
                    For Each station As String In stations

                        ' remove spaces from station
                        station = station.Trim
                        Log.Items.Add(station) 'add station number to log

                        ' check if station is numberic
                        If IsNumeric(station) Then
                        Else
                            MsgBox("Invalid Stations Input")
                            Cursor = Cursors.Default
                            Exit Sub
                        End If

                        ' lookup section pass from vehicle pass for each station 
                        Dim Spass As Integer = 0 'section pass

                        'for sections 1-3
                        If station < 160 Then
                            For i = 0 To sectionLimit - 1
                                If Vpass = section13(i, 0) Then
                                    Spass = section13(i, 1)
                                    secdiff13 = Vpass - Spass
                                End If
                            Next
                            If Spass = 0 Then
                                Spass = Vpass - secdiff13 ' looks up section pass if run in batch mode
                                If secdiff13 = 0 Then ' looks up section pass if not run in batch mode
                                    Dim vehdiff13 As Integer = 9999999
                                    For i = 0 To sectionLimit - 1
                                        If Vpass - section13(i, 0) < vehdiff13 And Vpass - section13(i, 0) >= 0 Then
                                            vehdiff13 = Vpass - section13(i, 0)
                                            secdiff13 = section13(i, 0) - section13(i, 1)
                                            Spass = Vpass - secdiff13
                                        End If
                                    Next
                                End If
                            End If
                        End If

                        'for sections 4-6
                        If station >= 160 Then
                            For i = 0 To sectionLimit - 1
                                If Vpass = section46(i, 0) Then
                                    Spass = section46(i, 1)
                                    secdiff46 = Vpass - Spass
                                End If
                            Next
                            If Spass = 0 Then
                                Spass = Vpass - secdiff46 ' looks up section pass if run in batch mode
                                If secdiff46 = 0 Then ' looks up section pass if not run in batch mode
                                    Dim vehdiff46 As Integer = 9999999
                                    For i = 0 To sectionLimit - 1
                                        If Vpass - section46(i, 0) < vehdiff46 And Vpass - section46(i, 0) >= 0 Then
                                            vehdiff46 = Vpass - section46(i, 0)
                                            secdiff46 = section46(i, 0) - section46(i, 1)
                                            Spass = Vpass - secdiff46
                                        End If
                                    Next
                                End If
                            End If
                        End If

                        ' create station filename 
                        staname = Refname.Replace("Ref", station)
                        staname = staname.Replace("ref", station)

                        'if station name does not exist, try folder date instead of Ref file date
                        If Not My.Computer.FileSystem.FileExists(staname) Then
                            staname = staname.Replace(Date2, Date3)
                        End If

                        ' check if it exists now before processing
                        If My.Computer.FileSystem.FileExists(staname) Then

                            ' process Station bin file if Read from binary files checkbox is selected
                            Dim StaX As New ArrayList
                            Dim StaZ As New ArrayList
                            If bincheckbox.Checked = True Then
                                Call StartBin(staname)
                                If LDecimate < 1 Then Cursor = Cursors.Default : Exit Sub
                                bincount = 0
                                For Each [object] In binX 'write .bin conversion results to StaX, StaZ variables
                                    StaX.Add(binX(bincount))
                                    StaZ.Add(binZ(bincount))
                                    bincount += 1
                                Next

                                'check if file exists 2017
                                If My.Computer.FileSystem.FileExists(staname) Then
                                    MsgBox("File found.")
                                Else
                                    MsgBox("File not found.")
                                End If

                                'create text file if it doesn't exist 2017
                                'Dim fs As FileStream = File.Create(staname)

                            Else
                                ' Read each station Profile straight from .txt
                                Dim StaReader As New Microsoft.VisualBasic.
                                FileIO.TextFieldParser(staname)
                                StaReader.TextFieldType = FileIO.FieldType.FixedWidth
                                StaReader.SetFieldWidths(8, 8)
                                Dim currentRowSta As String()
                                Dim Sta(849, 1) As Double
                                Dim RowSta As Integer = 0
                                Dim ColumnSta As Integer = 0

                                While Not StaReader.EndOfData
                                    currentRowSta = StaReader.ReadFields()
                                    Dim currentFieldSta As String
                                    For Each currentFieldSta In currentRowSta
                                        Sta(RowSta, ColumnSta) = currentFieldSta
                                        ColumnSta += 1
                                    Next
                                    StaX.Add(Sta(RowSta, 0))
                                    StaZ.Add(Sta(RowSta, 1))
                                    RowSta += 1
                                    ColumnSta = 0
                                End While
                            End If

                            ' shorten longer station files 
                            If StaX.Count > 785 Then
                                StaX.RemoveRange(785, StaX.Count - 785)
                                StaZ.RemoveRange(785, StaZ.Count - 785)
                            End If

                            ' lengthens shorter station files
                            If StaX.Count < 785 Then
                                For i = StaX.Count To 784
                                    StaX.Add(StaX(StaX.Count - 1) + StaX(1))
                                    StaZ.Add(StaZ(StaZ.Count - 1))
                                Next
                            End If

                            ' plot each station profile
                            Me.Chart1.Series("Station Profile").Points.DataBindXY(StaX, StaZ)

                            'subtract beam curvature from station profile to form corrected profile
                            Dim StaCor As New ArrayList
                            Dim RowCor As Integer = 0
                            Dim StaCorString As String = ""
                            For Each [object] In StaZ
                                StaCor.Add(StaZ(RowCor) - curve(RowCor))
                                StaCorString += Format(Math.Round(StaCor(RowCor), 4), "##.0000") & ","
                                RowCor += 1
                            Next

                            ' plot corrected station profile
                            Me.Chart1.Series("Corrected Station Profile").Points.DataBindXY(StaX, StaCor)

                            ' find max on left side of straightedge
                            Dim maxlocl As Integer = leftend
                            For i = leftend To secenterl
                                If StaCor(i) > StaCor(maxlocl) Then
                                    maxlocl = i
                                End If
                            Next

                            ' find max on right side of straightedge
                            Dim maxlocr As Integer = secenter
                            For i = secenter To rightend
                                If StaCor(i) > StaCor(maxlocr) Then
                                    maxlocr = i
                                End If
                            Next

                            ' find slope between these two points
                            Dim slope As Double
                            slope = (StaCor(maxlocr) - StaCor(maxlocl)) / (StaX(maxlocr) - StaX(maxlocl))

                            ' build straightedge based on slope
                            Dim stedz(0 To 784) As Double
                            stedz(leftend) = StaCor(maxlocl) - (slope * (StaX(maxlocl) - StaX(leftend)))
                            For i = leftend + 1 To rightend
                                stedz(i) = stedz(leftend) + (slope * (StaX(i) - StaX(leftend)))
                            Next

                            ' do loop to place straightedge at rest
                            Dim atRest As Boolean = False
                            Dim difhalfL(0 To 784) As Double
                            Dim difhalfR(0 To 784) As Double
                            Dim maxdifL As Single
                            Dim maxdifR As Single
                            Do Until atRest

                                ' find best new point on left half of straightedge
                                maxlocl = leftend
                                For i = leftend To secenterl
                                    difhalfL(i) = StaCor(i) - stedz(i)
                                    If difhalfL(i) > difhalfL(maxlocl) Then
                                        maxlocl = i
                                        maxdifL = difhalfL(i)
                                    End If
                                Next

                                ' find new slope
                                slope = (StaCor(maxlocr) - StaCor(maxlocl)) / (StaX(maxlocr) - StaX(maxlocl))

                                ' build straightedge based on slope
                                stedz(leftend) = StaCor(maxlocl) - (slope * (StaX(maxlocl) - StaX(leftend)))
                                For i = leftend + 1 To rightend
                                    stedz(i) = stedz(leftend) + (slope * (StaX(i) - StaX(leftend)))
                                Next


                                ' find best new point on right half of straightedge
                                maxlocr = secenter
                                For i = secenter To rightend
                                    difhalfR(i) = StaCor(i) - stedz(i)
                                    If difhalfR(i) > difhalfR(maxlocr) Then
                                        maxlocr = i
                                        maxdifR = difhalfR(i)
                                    End If
                                Next

                                ' find new slope
                                slope = (StaCor(maxlocr) - StaCor(maxlocl)) / (StaX(maxlocr) - StaX(maxlocl))

                                ' build straightedge based on slope
                                stedz(leftend) = StaCor(maxlocl) - (slope * (StaX(maxlocl) - StaX(leftend)))
                                For i = leftend + 1 To rightend
                                    stedz(i) = stedz(leftend) + (slope * (StaX(i) - StaX(leftend)))
                                Next

                                ' checks if straightedge sits on top
                                If maxdifL <= 0.0005 Then
                                    If maxdifR <= 0.0005 Then
                                        atRest = True
                                    End If
                                End If
                            Loop

                            'trim straightedge zeros just for plotting 
                            Dim stedztrim As New ArrayList
                            Dim stedxtrim As New ArrayList
                            For i = leftend To rightend
                                stedztrim.Add(stedz(i))
                                stedxtrim.Add(StaX(i))
                            Next

                            ' plot straightedge
                            Me.Chart1.Series("Straightedge").Points.DataBindXY(stedxtrim, stedztrim)

                            ' 2017-10-23 save chart as image file

                            'Dim imagefilename As String = datadirectory + "\" + Vpass + "_" + station + ".jpg"

                            'Chart1.SaveImage(imagefilename,
                            'System.Drawing.Imaging.ImageFormat.Jpeg)

                            ' end 2017-10-23 save chart as image

                            ' subtract profile from straightedge to find max rut depth
                            Dim MaxRut As Single = 0
                                Dim MaxRutLoc As Single
                                For i = leftrange To rightrange
                                    If stedz(i) - StaCor(i) > MaxRut Then
                                        MaxRut = stedz(i) - StaCor(i)
                                        MaxRutLoc = i
                                    End If
                                Next
                                Console.WriteLine(MaxRut)
                                Console.WriteLine(StaX(MaxRutLoc))

                                ' write to graph labels
                                Label16.Text = Vpass
                                Label15.Text = Spass
                                Label18.Text = Date2.Insert(6, "-").Insert(4, "-")
                                Label20.Text = station
                                Label2.Text = Math.Round(MaxRut, 3).ToString("#0.000")
                                Label3.Text = Format(Math.Round(StaX(MaxRutLoc), 2), "#0.00")

                                ' write results to csv 
                                Dim outfile As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvfile, True)
                                outfile.WriteLine(Date2.Insert(6, "-").Insert(4, "-") & "," & Vpass & "," & Spass & "," & station & "," & Math.Round(MaxRut, 4).ToString("#0.0000") & "," & Format(Math.Round(StaX(MaxRutLoc), 2), "#0.00") & "," & StaCorString)
                                outfile.Close()

                            Else
                                Log.Items.Add("Station " & station & " file D.N.E.") ' station does not exists
                        End If ' if station file exists

                    Next 'End list of station numbers

                    'return cursor to default from wait if individual ref file selected
                    If TextBox1.Text <> "" And TextBox1.Text <> "Select Individual R2R Reference File..." Then
                        Log.TopIndex = Log.Items.Count - 1 'scroll to bottom of Log
                        Cursor = Cursors.Default
                        Dim timelapse As Object
                        timelapse = Now - time1
                        'Log.Items.Add(timelapse.totalseconds.ToString)
                        Exit Sub
                    End If

                    Me.Refresh() ' refresh plot for batch processing

                Next 'End list of Ref files
            Next 'End list of bin subfolders
        Next 'End list of folders

        ' warn user if no data found
        If staname = "" Then
            MsgBox("No data found, check Data Directory.")
        End If

        'scroll to bottom of Log
        Log.TopIndex = Log.Items.Count - 1

        ' end wait cursor
        Cursor = Cursors.Default

    End Sub

    'Sub to print values to output for debugging
    Public Shared Sub PrintValues(myList As IEnumerable)
        Dim obj As [Object]
        For Each obj In myList
            Console.Write("   {0}", obj)
        Next obj
        Console.WriteLine()
    End Sub 'PrintValues

    ' subs for selecting data directory
    Public Sub FolderBrowserDialog10_HelpRequest(sender As Object, e As EventArgs) Handles FolderBrowserDialog10.HelpRequest
    End Sub

    Public Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If (FolderBrowserDialog10.ShowDialog() = DialogResult.OK) Then
            TextBox7.Text = FolderBrowserDialog10.SelectedPath
        End If
    End Sub

    'subs for selecting leica file
    Public Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        OpenFileDialog1.Filter = "Leica Reference Files|*.txt;*.csv"
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            TextBox8.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Public Sub OpenFileDialog1_FileOk(sender As Object, e As ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
    End Sub

    ' subs for individual R2R reference files
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If bincheckbox.Checked = True Then
            OpenFileDialog2.Filter = ".bin Reference Files|*f.bin"
        Else
            OpenFileDialog2.Filter = ".txt Reference Files|*f.txt"
            'OpenFileDialog2.InitialDirectory = TextBox7.Text
        End If
        If OpenFileDialog2.ShowDialog() = DialogResult.OK Then
            TextBox1.Text = OpenFileDialog2.FileName
        End If
    End Sub

    Private Sub OpenFileDialog2_FileOk(sender As Object, e As ComponentModel.CancelEventArgs) Handles OpenFileDialog2.FileOk
    End Sub

    Private Sub Log2_SelectedIndexChanged(sender As Object, e As EventArgs)
    End Sub

    'subs to view results
    Private Sub ButtonResults_Click(sender As Object, e As EventArgs) Handles ButtonResults.Click
        Try
            Process.Start(csvfile)
            Exit Try
        Catch
            MsgBox("Run Program First")
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Public Sub bincheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles bincheckbox.CheckedChanged
        If bincheckbox.Checked = True Then
            TextBox1.Text = TextBox1.Text.Replace(".txt", ".bin")
        ElseIf bincheckbox.Checked = False Then
            TextBox1.Text = TextBox1.Text.Replace(".bin", ".txt")
        End If
    End Sub


    ' start .bin processing subs, originally from Profile32 for R2R Profiler
    ' translated from VB6 to VB.net by Stephen Augustyn, CSRA, March 28, 2016

    ' global variables
    Public MinLeftElev As Double, MaxLeftElev As Double
    Public MinRightElev As Double, MaxRightElev As Double
    Public AllChannels() As Single, NChans As Long
    Public NOverflows() As Long
    ' Public PlotChanNo(0 To StartNPics - 1) As Long - moved down
    Public ProcessingAll As Boolean, FileNameOnly$
    Public WritingDirDocs As Boolean
    Public DatFileName$, WorkingDir$, AppPath$, DatPathFileName$, FileExt$
    Public DatFileDateTime As Object  ' GFH 10/15/02.
    Public FNo As Integer, FLength As Long
    Public LSamples As Long, LDecimate As Long
    Public LStartSamples As Long, LEndSamples As Long

    Public Const SelcomCal As Single = 7.874 / 4096         ' inches
    Public Const VtoFCal As Double = 32.16 * 12.0! / 32270.0!    ' in/s per pulse
    'Public Const DeltaT As Double = 1 / 400                ' seconds
    'Public Const DeltaT As Double = 1 / 20                ' seconds
    Public DeltaT As Double = 0.01, SampleRate As Double
    'Public Const VtoFCal As Double = 32.16 * 12! / (32270 * 1.08844)    ' in/s per pulse
    'Public Const DeltaT As Double = 1.08844 / 8000                ' seconds
    ' Decimate to 1 sample/25 mm, vehicle speed = 40 ft/s, cutoff at 1/2 of Nyquist.
    ' See ReadandProcessBinFile for new definitions.
    Public IDecimate As Long ' = 10
    Public DatronCal As Double ' = 2.5 / 25.4 / 12 * 0.944        ' feet
    Public DeltaD As Double ' = DatronCal * IDecimate       ' feet
    Public DatronCalNominal As Double ' feet
    Public DatronCalFactor As Double
    ' Public Const DeltaD As Double = 26.17 / 25.4 / 12          ' feet
    'Public Const DecimateFilterFreq As Double = 12 * 40 / 4 ' Hz
    Public SpeedMax As Double           ' Set in Sub SmoothSpeed().
    Public DecimateFilterFreq As Double ' Set in Sub ZProfileVsDistance().
    ' = SpeedMax / (DatronCal * IDecimate * 4), 1/2 Nyquist frequency.
    ' = 187.3 Hz at SpeedMax = 58 ft/s
    Public Const FilterForward As Boolean = False           ' Only filter forwards.
    Public Const FilterBothWays As Boolean = True
    Public Const SpeedtoInt As Single = 100
    Public Const ZHiPasstoInt As Single = 1000

    Public Const GetArray As Boolean = True, PutArray As Boolean = False

    Public Status() As Byte, Datron() As Byte
    Public Selcom() As Single, SelcomAv As Double
    Public BVtoF() As Byte
    Public ZDotDbl() As Double, ZDot() As Single
    Public ZDotS() As Single, ZDotSum As Double
    Public ZVehicle() As Single  ' Vehicle Z, not smoothed.
    Public VtoFav As Double, VtoFavBy2 As Double
    Public Speed() As Single, PaveVelocity() As Single
    Public ZProTime() As Single, ZProDist() As Single
    Public ZHiPass() As Single
    Public IntArray() As Integer, SingleArray() As Single

    Public Const SpeedName$ = "Speed", SelcomName$ = "Selcom"
    Public Const ZDotName$ = "ZDot", ZVehicleName$ = "ZVehicle"
    Public Const ZProTimeName$ = "ZProTime", ZProDistName$ = "ZProDist"
    Public Const ZHiPassName$ = "ZHiPass", StatusName$ = "Status"
    Public Const VtoFName$ = "V/F"

    Public ReadFileCompleted As Boolean, ZVehicleCompleted As Boolean
    Public ZProfileDistCompleted As Boolean, ZProfileTimeCompleted As Boolean
    Public Zooming As Boolean

    Public VehicleZDot0 As Double, VehicleZDDot0 As Double
    Public VehicleTheta As Double, VehicleStiff As Double

    Public Const StartNPics As Integer = 3
    Public PlotChanNo(0 To StartNPics - 1) As Long
    Public FilterFreq(0 To StartNPics - 1) As Double
    Public StartIPic As Integer, StartPicDelta(StartNPics) As Single
    Public StartPicX(StartNPics) As Single, StartPicY(StartNPics) As Single
    Public StartPicOXAxis(StartNPics) As Single, StartPicOYAxis(StartNPics) As Single

    Public StartPicDDX(StartNPics) As Double, StartPicDDY(StartNPics) As Double
    Public StartPicVarName(StartNPics) As String
    Public Const optVariableN As Integer = 7

    Public Const ZoomLevelsN As Integer = 20
    Public ZoomLevel(0 To StartNPics - 1) As Integer
    Public ZoomLStart(0 To StartNPics - 1, 0 To ZoomLevelsN) As Long
    Public ZoomLEnd(0 To StartNPics - 1, 0 To ZoomLevelsN) As Long

    ' Public variables cannot be passed reliably in Subs.
    Public BTemp As Byte, ITemp As Integer, LTemp As Long
    Public STemp As Single, DTemp As Double
    Public NTime As Object, Ret As Object, S$

    Public ZeroSP As Boolean ' 3/2/16 '3/9/16 added as global variable
    Public NonZero As Integer ' ""
    Public file_name As String ' added 3/11/16
    Public bytes ' added 3/11/16
    Public firstbinread As Boolean = True ' added 3/29/2016 'determines if first time bin file is read

    Public Const Connstring = "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=CC5;Data Source=LIN-08988;"        ' Workstation ID=EGG2095 '----BING Linwood SQL

    Public sqlText As String, sqlText2 As String
    '''Public NewCn As New ADODB.Connection, rs As New ADODB.Recordset, rs2 As New ADODB.Recordset
    '''Public newcn2 As New ADODB.Connection


    Public Sub StartBin(binfilename As String)
        Static Started As Boolean, Booltemp As Boolean

        NTime = Now

        Call EraseAllArrays()

        ReadFileCompleted = False : ZVehicleCompleted = False
        ZProfileDistCompleted = False : ZProfileTimeCompleted = False

        Call ReadandProcessBinFile(binfilename)

    End Sub

    Public Sub ReadandProcessBinFile(binfilename As String)

        Dim I As Long, J As Long, L As Long, FLength As Long
        Dim ISkip As Integer, JP1 As Long, FWDBinFNo As Integer
        Dim LF As Long, BTemp As Byte, S As String, FilterFreqDecimate As Double ' removed $ added as string 3/10/16
        Dim Start As Single, Slope As Single, II As Integer, LocalExt As String
        Dim Invalid As Long, NInvalid As Long
        Dim SelcomLast As Single, GlitchThresh As Single
        Dim MidSample As Single, NGlitches As Long, SelcomRev() As Single
        Dim IChan As Long, Var As Object
        Dim SingleFile As Boolean
        Dim FileT1() As Single, FileT2() As Single, FileT3() As Single
        Dim LS1 As Long, LS2 As Long, LS3 As Long
        '  Dim IDecimate As Long
        '  Dim DatronCalNominal As Double, DatronCal As Long, DatronCalFactor As Long

        NChans = 4
        ReDim NOverflows(0 To 63)

        SingleFile = True

        NTime = Now

        IDecimate = 1  ' One sample per 2.5 mm, approximately one per 0.1 inch.
        DatronCalNominal = 2.5 / 25.4 / 12        ' Nominal calibration in feet per Datron pulse.
        DatronCalFactor = 0.2042
        DatronCal = DatronCalNominal * DatronCalFactor  ' Final Calibration coefficient.
        DeltaD = DatronCal * IDecimate                  ' Decimated sample spacing.
        '  lblDatronCalFactor.Caption = Format(DatronCalFactor, "0.0######")

        Call ReadandProcessDatFile(binfilename)

        Call ZProfileVsDistance()
        LSamples = LDecimate
        DeltaT = DeltaD

        If LDecimate < 1 Then Exit Sub 'steve added for shorter .bin files

        S$ = "Reading " & DatFileName & ", length = " & Format(FLength, "#,###,##0") & " bytes"
        '''steve Call WriteSbar(S$, "T", "", "")
        S$ = S$ & " elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"
        '    Debug.Print S$

        LStartSamples = 1 : LEndSamples = LSamples

        Dim AllChannels(0 To NChans - 1, 0 To LSamples - 1) 'steve removed Re, changed to 0 for only columns (second one)

        For L = 1 To LSamples
            AllChannels(1, L - 1) = ZProDist(L - 1)
            '    AllChannels(1, L-1) = ZProDist(LSamples - L + 1)
        Next L

        For L = 1 To LSamples
            AllChannels(2 - 1, L - 1) = AllChannels(1, L - 1) '-1 ? check
        Next L

        DeltaT = DeltaD ' 0.001428 '0.25 / 0.3048  ' feet

        SampleRate = 1 / DeltaT

        '''stevefrmStart.txtSampleRate.Text = Format(SampleRate, "0.0######")
        '''steveCall frmStart.txtSampleRate_KeyPress(Asc(vbCr))

        For I = 0 To 2  ' picVariable.UBound
            '''steve   frmStart.txtChanNo(I) = Format(I, "0")
            '''steve   frmStart.txtFilterFreq(I).Text = frmStart.txtSampleRate.Text
        Next I

        For L = 1 To LSamples
            AllChannels(0, L - 1) = AllChannels(2, L - 1)
        Next L

        For L = 1 To LSamples
            '    AllChannels(1, L) = AllChannels(0, LSamples - L + 1)
            AllChannels(1, L - 1) = Speed(L - 1) '-1 block
            AllChannels(3, L - 1) = AllChannels(0, LSamples - L + 1 - 1)
        Next L

        For L = 1 To -LSamples
            AllChannels(0, L - 1) = AllChannels(3, L - 1) '-1
        Next L

        MinLeftElev = -100 : MinRightElev = -100
        MaxLeftElev = 100 : MaxRightElev = 100
        For L = 1 To LSamples / 2
            If AllChannels(0, L - 1) < MaxLeftElev Then MaxLeftElev = AllChannels(0, L - 1)
            If AllChannels(0, L - 1) > MinLeftElev Then MinLeftElev = AllChannels(0, L - 1)
        Next L
        MaxLeftElev = -MaxLeftElev
        MinLeftElev = -MinLeftElev

        For L = LSamples / 2 + 1 To LSamples
            If AllChannels(0, L - 1) < MaxRightElev Then MaxRightElev = AllChannels(0, L - 1)
            If AllChannels(0, L - 1) > MinRightElev Then MinRightElev = AllChannels(0, L - 1)
        Next L
        MaxRightElev = -MaxRightElev
        MinRightElev = -MinRightElev

        '    Debug.Print LSamples; (LSamples - 1) * DeltaT

        ReDim Speed(0 To LSamples - 1) '-1 block
        ReDim Selcom(0 To LSamples - 1)
        ReDim ZDotS(0 To LSamples - 1)
        For I = 0 To 2 'StartNPics - 1
            If FilterFreq(I) < 0.01 Then
                FilterFreq(I) = 1 / DeltaT
                '''steve               frmStart.txtFilterFreq(I).Text = Format(FilterFreq(I), "0.0")
            End If
            '    frmStart.txtChanNo(I).Text = Format(I, "0")
            '''steve           Call frmStart.txtChanNo_KeyPress(CInt(I), Asc(vbCr))
        Next I

        ReadFileCompleted = True
        '''steve      Call WriteSbar(FileExt$ & " file processed", "T", "", "")
        ProcessingAll = False

    End Sub

    Public Sub ReadandProcessDatFile(binfilename As String) ' taken from steve's version

        Dim I As Long, J As Long, L As Long, FLength As Long
        Dim ISkip As Integer, EndSkip As Integer, ProDatFNo As Integer
        Dim JP2 As Long
        Dim LF As Long, BTemp As Byte
        Dim Slope As Single, II As Integer
        Dim Invalid As Long, NInvalid As Long
        Dim SelcomLast As Single, GlitchThresh As Single
        Dim MidSample As Single, NGlitches As Long
        Dim FirstInvalid As Long, K As Long, NBadStatus As Long
        Dim AX As Integer, Datronlast As Integer
        ''Dim VtoFav, LStartSamples, LEndSamples As Integer ' added 3/2/16
        ''Dim S As String

        FirstInvalid = 0
        ISkip = 256
        EndSkip = 0 '5461

        ProDatFNo = FreeFile()

        ' bin file reading - custom name input

        file_name = binfilename
        DatFileDateTime = FileDateTime(file_name)
        bytes = My.Computer.FileSystem.ReadAllBytes(file_name)
        'Dim LSamples As Double
        'Dim AllChannels(0 To 3, 0 To LSamples)
        'For Each name As String In bytes
        'Me.Log.Items.Add(name)
        'Next

        FLength = FileLen(file_name)
        LF = FLength - (EndSkip * 3)

        If LF - ISkip * 3 < 1 Then
            Console.WriteLine("Record too short.", "T", "", "")
            Exit Sub
        End If

        ReDim BVtoF(0 To LF - 1)     ' reads bytes to BVtoF ' had to change from "1 To LF"
        For I = 0 To LF - 1
            BVtoF(I) = bytes(I)
        Next

        'Get #ProDatFNo, , BVtoF()
        'Close(ProDatFNo)

        L = ISkip * 3
        LF = LF - L
        For I = 1 To -LF
            BVtoF(I - 1) = BVtoF(I - 1 + L) ' -1 added because BVtoF has to start at 0 'check based on -LF situation
        Next I

        LSamples = LF \ 4
        LStartSamples = 1 : LEndSamples = LSamples
        L = LSamples

        ReDim Status(0 To L - 1), Datron(0 To L - 1) ' changed from 1 to L for each 3/2/16

        NBadStatus = 0
        VtoFav = 0
        J = -3
        For I = 1 To L
            J = J + 4
            JP2 = J + 2
            BTemp = BVtoF(J - 1)
            Status(I - 1) = BTemp And 7 ' Change this to 1 or 3 to give spare inputs.
            '    Status(I-1) = 0 'BTemp And 128
            If Status(I - 1) <> 0 Then NBadStatus = NBadStatus + 1 'bad status if last 3 bits in 1, 5, 9th byte is on
            '   Must have 112 for FAA system. GFH 09/13/01. **************
            '    BTemp = BTemp And 8
            AX = -1
            If (BTemp And 8) = 0 Then AX = 0 ' if 5th bit is off
            Datron(I - 1) = 0
            If AX - Datronlast = -1 Then Datron(I - 1) = 255 ' if 5th bit is on and last time it was off
            Datronlast = AX
            '    If I Mod 64 = 0 Then Datron(I - 1) = 255 Else Datron(I - 1) = 0
            '    Datron(J - 1) = BTemp And 8 '32    ' Was 112 before NYC added event marker. 112 for FAA box.
            ' Datron marker moved to only the 32 bit.
        Next


        S = "Number of raw data points = " & Format(L, "#,##0") & " " & vbCrLf
        S = S & "Number of bad Selcom Status points = " & Format(NBadStatus, "#,##0") & vbCrLf
        '  S = S & "Percentage of bad Selcom Status points = " & Format(100 * CDbl(NBadStatus) / CDbl(L), "0.0")
        'MsgBox(S)

        '  Debug.Print "ReDim Selcom; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"

        ' variable check, can be removed
        ''For I = 1000000 To 1001000
        ''Me.Log.Items.Add(Status(I))
        ''Me.Log2.Items.Add(BVtoF(I))
        ''Next

        'datron check, can be removed
        ''Dim Datronsum As Double = 0
        ''For I = 0 To L - 1
        ''Datronsum += Datron(I)
        ''Next
        ''Me.Log.Items.Add(Datronsum)

        ''Dim ITemp As Integer ' added 3/2/16
        ''Dim LTemp As Integer ' added 3/2/16
        ''Dim SelcomAv As Double ' added 3/2/16
        ''Dim ProcessingAll As Boolean ' added 3/2/16
        ''Dim SelcomCal As Single = 7.874 / 4096         ' inches
        ReDim Selcom(0 To L - 1) ' changed from 1 to L
        '  Debug.Print "Process file data; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"
        K = 0
        J = -3
        '  If BVtoF(3 - 1) > 32 Then BVtoF(3 - 1) = 20 ' GFH 1/15/00. Simplifies test below.
        For I = 1 To L
            J = J + 4
            JP2 = J + 2
            '    Debug.Print I; Status(I - 1)
            If Status(I - 1) = 0 Then
                ITemp = (BVtoF(J - 1) And 240) / 16 + BVtoF(JP2 - 1) * 16 * 1.95
                Selcom(I - 1) = ITemp * SelcomCal
                If I > 330000 And I <= 330064 Then
                    '''Debug.Print I; ITemp; Format(Selcom(I), "0.000000")

                End If
                If Invalid <> 0 Then  ' Interpolate invalid samples.
                    LTemp = I - Invalid - 1
                    If LTemp <= 0 Then
                        'Data from 1st to (I-1)th are invalid, because Status(1: invalid)=0
                        For II = 1 To Invalid
                            Selcom(II - 1) = Selcom(I - 1)
                        Next II
                        FirstInvalid = Invalid
                    Else
                        Slope = (Selcom(I - 1) - Selcom(LTemp - 1)) / (Invalid + 1)
                        For II = 1 To Invalid
                            Selcom(LTemp + II - 1) = Selcom(LTemp - 1) + Slope * II
                        Next II
                    End If
                    Invalid = 0
                End If

            Else

                Invalid = Invalid + 1
                NInvalid = NInvalid + 1

            End If

            '   Get VtoF count for accelerometer if present.
            '    BTemp = BVtoF(J + 2)
            '    If 32 < BTemp Then BTemp = BVtoF(I - 1) ' GFH 1/15/00. See start of loop.
            '    VtoFav = VtoFav + CDbl(BTemp)
            '    BVtoF(I) = BTemp ' This is the VtoF count for one sample.

        Next I

        Debug.Print("(BVtoF(J) And 240) =" & (BVtoF(J) And 240))


        For I = 1 To L
            If I Mod 6000 = 0 Then
                Debug.Print(I & Int(Selcom(I - 1) / SelcomCal) & Format(Selcom(I - 1), "0.000000"))
            End If
        Next I

        ' Debug.Print "The number of samples with VtoF>23= "; K; " VtoFav = "; VtoFav / L
        '  Exit Sub
        '  Erase Status
        ReDim Preserve BVtoF(0 To L - 1) ' check

        'delete first Invalid data if FirstInvalid>0
        If FirstInvalid > 0 Then
            For I = FirstInvalid + 1 To L
                J = I - FirstInvalid
                BVtoF(J - 1) = BVtoF(I - 1)
                Selcom(J - 1) = Selcom(I - 1)
                Datron(J - 1) = Datron(I - 1)
            Next I
            L = L - FirstInvalid
            LSamples = L
            LEndSamples = LSamples
        End If
        ReDim Preserve Datron(0 To L - 1), Selcom(0 To L - 1), BVtoF(0 To L - 1)
        ' Remove single sample glitches.
        GlitchThresh = 0.25
        SelcomLast = Selcom(2 - 1)
        For I = 1 To L - 1
            If Math.Abs(Selcom(I - 1) - SelcomLast) > GlitchThresh Then
                MidSample = (SelcomLast + Selcom(I + 1 - 1)) / 2
                If Math.Abs(Selcom(I - 1) - MidSample) > GlitchThresh Then
                    Selcom(I - 1) = MidSample
                    NGlitches = NGlitches + 1
                End If
            End If
            SelcomLast = Selcom(I - 1)
        Next I
        ' Debug.Print "NGlitches = "; NGlitches
        Selcom(L - 1) = Selcom(L - 1 - 1)

        SelcomAv = 0
        For I = 1 To L
            SelcomAv = Selcom(I - 1) + SelcomAv
        Next I

        SelcomAv = SelcomAv / L
        ' zero selcom to its average
        For I = 1 To L
            Selcom(I - 1) = Selcom(I - 1) - SelcomAv
        Next I

        ProcessingAll = False
        '  Debug.Print "Plot Selcom; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"
        If Not ProcessingAll Then
            '    Call PlotPicFirstTime(1, SelcomName$)
            '    frmStart!picVariable(1).Refresh
        End If
        '  Debug.Print "End Plot Selcom; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"

        '''Call WriteSbar("Processing speed", "T", "", "")
        ReDim Speed(0 To L - 1) 'changed from 1 to L 
        '  Debug.Print "StartSmoothSpeed; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"

        Call SmoothSpeed()

        'Debug.Print "Speed = "; Speed(20)
        '  NonZero = 1
        If ZeroSP = True Then
            If FLength - (EndSkip + ISkip) * 3 < 1 Then
                S$ = file_name & " processed failure because FLength - (EndSkip + ISkip) * 3 < 1"
            Else
                S$ = file_name & " processed failure because speed = 0."
            End If
            Debug.Print(S$)
            ' Exit Sub
        End If

        ' Debug.Print "End SmoothSpeed; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"
        ' S$ = "Finished .dat file processing; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"  '& vbCrLf  '12-27-2011
        'steve S$ = "Finished .bin file processing; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"  '& vbCrLf '12-27-2011
        '  S$ = S$ & " NInvalid = " & Format(NInvalid, "0") & " NGlitches = " & Format(NGlitches, "0")
        'MsgBox(S$)
        '  Debug.Print "FirstInvalid= "; FirstInvalid

        ReadFileCompleted = True

        ' Call WriteSbar(".dat file processed", "T", "", "")  '12-27-2011
        'MsgBox(".bin file processed" & "T" & "")  '12-27-2011 ' changed 3/2/16
        ProcessingAll = False

        'Me.Log.Items.Add(Datron(0))



    End Sub

    Public Sub SmoothSpeed()

        ' Sub SmoothSpeed()

        ' Assume Datron and Speed open.

        Dim SpeedFilterFreq As Double, I As Long, L As Long
        Dim Trailer() As Byte, DatronCount As Long
        Dim II As Long, III As Long, TotalDatronCount As Long
        Dim AVN As Integer, AVNH As Integer, SpeedCal As Single
        Dim SumSpeed As Double
        Dim S$ '2/25/00 D.M. ' changed to $ 3/10/16

        'Dim DatronCal As Double ' = 2.5 / 25.4 / 12 * 0.944        ' feet ' added 3/2/16 ' removed 3/9/16
        'Dim DeltaT As Double = 1 / 20                ' seconds

        SpeedFilterFreq = 0.5
        AVN = 129
        AVNH = AVN \ 2
        SpeedCal = DatronCal / ((AVN - 1) * DeltaT)  ' ft/s
        '  Debug.Print "AVN = "; AVN; AVNH; DeltaT
        L = LSamples
        ReDim Trailer(0 To L + AVN - L - 1) ' every Trailer set has to add - L - 1 '3/2/16

        For I = 1 To AVN : Trailer(L + I - L - 1) = Datron(L - I - 1) : Next I

        TotalDatronCount = 0
        For I = 1 To L
            If Datron(I - 1) > 1 Then TotalDatronCount = TotalDatronCount + 1
        Next I
        '  Debug.Print "Total Datron counts = "; TotalDatronCount

        DatronCount = 0
        For I = 1 To AVN
            If Datron(I - 1) > 1 Then DatronCount = DatronCount + 1
        Next I

        Speed(AVNH + 1 - 1) = DatronCount * SpeedCal

        LTemp = AVNH + 1
        For I = 1 To AVNH
            Speed(I - 1) = Speed(LTemp - 1)
        Next I

        For I = AVNH + 2 To L - AVNH
            '    DatronCount = 0
            '    III = I - AVNH - 1
            '    For II = 1 To AVN
            '      If Datron(II + III - 1) > 0 Then DatronCount = DatronCount + 1
            '    Next II
            If Datron(I - AVNH - 1 - 1) > 1 Then DatronCount = DatronCount - 1
            If Datron(I + AVNH - 1) > 1 Then DatronCount = DatronCount + 1
            Speed(I - 1) = DatronCount * SpeedCal
        Next I

        For I = L - AVNH + 1 To L
            If Datron(I - AVNH - 1 - 1) > 1 Then DatronCount = DatronCount - 1
            If Trailer(I + AVNH - L - 1) > 1 Then DatronCount = DatronCount + 1
            Speed(I - 1) = DatronCount * SpeedCal
        Next I

        Erase Trailer

        'Dim NTime As Object 'removed 3/9/16 after added

        S$ = "Raw speed computed; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds" ' changed to $ 3/10/16
        '  Debug.Print S$
        If Not ProcessingAll Then
            '    Call PlotPicFirstTime(2, SpeedName$)
            '    frmStart!picVariable(2).Refresh
        End If
        'user can find speed=0 in picture(0)
        ' moved ZeroSP declatation to global 3/9/16
        If TotalDatronCount <= 0 Then
            '   NonZero = 0
            '   Exit Sub
            ZeroSP = True
        Else
            '   NonZero = 1
            ZeroSP = False
        End If
        'Dim NonZero As Integer ' 3/2/16 moved to global variables
        NonZero = 1
        '  Call Filter(SpeedFilterFreq, DeltaT, Speed(), L, True)
        '  Call FilterTan4(SpeedFilterFreq, DeltaT, Speed(1), L, -1)

        '  Call CorrectSpeedCity(SpeedFilterFreq) ' Interpolate dropouts for measured street cutting speed=0 segments

        Call CorrectSpeed(SpeedFilterFreq) ' Interpolate dropouts 
        '  If ZeroSP = True Then Exit Sub

        'Dim SpeedName As String
        'Dim SpeedMax As Double ' 3/2/16 removed 3/9/16
        L = LSamples
        SpeedMax = 0
        For I = 1 To L
            If Speed(I - 1) > SpeedMax Then SpeedMax = Speed(I - 1)
        Next I

        S$ = "Speed filtered; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds" ' changed to $ 3/10/16
        '  Debug.Print S$ ' changed to $ 3/10/16
        If Not ProcessingAll Then
            ' Call PlotPic(0, 0, L * DeltaT, DeltaT, False, Speed(), SpeedName$)
            ' frmStart!picVariable(0).Refresh
            '*****
            ' Call PlotPic(2, 0, L * DeltaT, DeltaT, True, Speed(), SpeedName$)
            '2/25/00 D.M.
            ReDim SingleArray(L) ' check, removed -1 because length is same
            For I = 1 To L
                SingleArray(I - 1) = Speed(I - 1) '* FttoM
            Next I
            S$ = SpeedName$ '& " (m/s)"  GFH 3/12/00 ' changed to $ 3/10/16
            '    Call PlotPicFirstTime(1, SpeedName$)  ' GFH 3/12/00
            '    Call PlotPicFirstTime(2, DatronName$) ' GFH 9/26/01
            '    Call PlotPic(2, 0, L * DeltaT, DeltaT, True, SingleArray(), S$, " (m/s)")
            '*** D.M.

            ''' frmStart!picVariable(1).Refresh() 'check, left out if not necessary
            '   Call PlotPic(1, 0, L * DeltaT, DeltaT, True, Selcom(), SelcomName$)
            '    frmStart!picVariable(1).Refresh


            '****
        End If

    End Sub
    ' end sub smoothspeed


    Public Sub CorrectSpeed(SpeedFilterFreq As Double)  'added ByRef 3/14/16

        Dim I As Long, II As Long, L As Long, LTemp As Long
        Dim LStart As Long, LEnd As Long
        Dim Slope As Single, Ip1 As Integer
        Dim Invalid As Long, NInvalid As Long
        Dim DoCorrection As Boolean
        Dim K As Long

        'not active, doesnt matter
        DoCorrection = True 'False  'True
        If Not DoCorrection Then
            '        Call FilterTan4(SpeedFilterFreq, DeltaT, Speed(1), LSamples, -1)
            Call Filter(SpeedFilterFreq, DeltaT, Speed, LSamples, True) 'removed parenthesis speed '''steve 
            Exit Sub
        End If

        L = LSamples
        For I = 1 To L
            If Speed(I - 1) > 0 Then '-1
                LStart = I
                Exit For
            End If
        Next I

        For I = L To 1 Step -1
            If Speed(I - 1) > 0 Then '-1
                LEnd = I
                Exit For
            End If
        Next I
        '  Debug.Print "Start End = "; LStart; LEnd; SpeedFilterFreq
        If LEnd - LStart < 500 Then Exit Sub

        ReDim ZVehicle(0 To L - 1) ' 0 to -1

        For I = 1 To L
            ZVehicle(I - 1) = Speed(I - 1) '-1
        Next I

        '  Call FilterTan4(SpeedFilterFreq, DeltaT, ZVehicle(1), L, -1)
        Call Filter(SpeedFilterFreq, DeltaT, ZVehicle, L, -1) ' check, consider removing parenthesis 'removed parenthesis '''steve 

        For I = LStart To LEnd
            If Not ((Speed(I - 1) = 0) Or (Math.Abs(Speed(I - 1) - ZVehicle(I - 1)) > 5)) Then '-1
                If Invalid <> 0 Then  ' Interpolate invalid samples.
                    Ip1 = 250
                    Invalid = Invalid + Ip1 * 2
                    LTemp = I - Invalid - 1 + Ip1
                    If LTemp < 1 Then LTemp = 1

                    If I + Ip1 <= L Then
                        Slope = (Speed(I + Ip1 - 1) - Speed(LTemp - 1)) / (Invalid + 1) '-1
                    Else
                        Invalid = Invalid - (I + Ip1 - L)
                        Slope = (Speed(L - 1) - Speed(LTemp - 1)) / (Invalid + 1) '-1
                    End If
                    '       Debug.Print "Invalid = "; I; Invalid; ZVehicle(I); ZVehicle(LTemp); Speed(I)
                    For II = 1 To Invalid
                        '          Debug.Print "Invalid = "; LTemp + II; Speed(LTemp + II); ZVehicle(LTemp) + Slope * II
                        Speed(LTemp + II - 1) = Speed(LTemp - 1) + Slope * II '-1
                    Next II
                    Invalid = 0
                End If
            Else
                Invalid = Invalid + 1
                NInvalid = NInvalid + 1
            End If
        Next I

        '  Call FilterTan4(SpeedFilterFreq, DeltaT, Speed(1), L, -1)
        Call Filter(SpeedFilterFreq, DeltaT, Speed, L, -1) ' check 'consider removing () like previously called 'removed parenthesis '''steve

        ' Debug.Print "Speed Invalid - "; NInvalid

        Erase ZVehicle

    End Sub
    'end sub CorrectSpeed


    'start bas file variables


    'Attribute VB_Name = "Filters"
    'Option Explicit ' copied block above

    Dim B01 As Double, B02 As Double
    Dim A11 As Double, A12 As Double, A21 As Double, A22 As Double
    Public TanFiltFreq As Double

    '  Declare Sub FilterTan4 Lib "DigFilters.dll" Alias "_FILTERTAN4@20" (FilterFreq As Double, Delta As Double, FF As Single, N As Long, Backwards As Integer)
    '  Declare Sub ShiftByteArray Lib "DigFilters.dll" Alias "_SHIFTBYTEARRAY@12" (ByteArray As Byte, L As Long, LShift As Long)
    '  Declare Sub Integrate Lib "DigFilters.dll" Alias "_INTEGRATE@24" (BVtoF As Byte, ZDotDbl As Double, LStart As Long, L As Long, ZDotSum As Double, VtoFCal As Double)
    '  Declare Sub RegressionF Lib "DigFilters.dll" Alias "_REGRESSIONF@40" (Speed As Single, ZDotS As Single, ZDotDbl As Double, LStart As Long, L As Long, DeltaT As Double, Theta As Double, CbyD As Double, V0 As Double, A0 As Double)
    '  Declare Sub MaxMinArraySingle Lib "DigFilters.dll" Alias "_MAXMINARRAYSINGLE@24" (LStart As Long, LEnd As Long, AArray As Single, ArrayLength As Long, YMax As Single, YMin As Single)

    Public Sub Filter(FilterFreq As Double, Delta As Double, ByRef FF() As Single, N As Long, Backwards As Integer)  'complete 3/9/16  'added ByRef 3/14/16

        ''Dim W As Single ' steve added 3/14/16
        Dim FFLeadIn() As Single, FFTrailer() As Single, I As Long, Started As Integer
        Dim NyQuistFreq As Double, NEnds As Long

        NyQuistFreq = 1 / (Delta * 2)
        NEnds = 10 / (FilterFreq * Delta)
        If NEnds < 100 Then NEnds = 100
        If NEnds >= N Then NEnds = N / 2
        '  Debug.Print "NEnds = "; NEnds

        ' Get filter coefficients for TAN2Filt
        If FilterFreq > NyQuistFreq Then FilterFreq = NyQuistFreq
        Call SetTanFilt(FilterFreq, Delta)

        ' Setup lead-in segment.
        ReDim FFLeadIn(0 To NEnds - 1) ' changed 'check maybe ' all FFLeadIn needs to add +NEnds - 1 ' checked 3/11/16
        ''ReDim FFTrailer(0 To NEnds - 1) ''temp steve 'added by steve 3/11/16
        STemp = FF(1 - 1) * 2 '-1
        For I = 1 To NEnds : FFLeadIn(1 - I + NEnds - 1) = STemp - FF(I + 1 - 1) : Next I ' NEnds - 1

        If Backwards Then
            ReDim FFTrailer(0 To NEnds - 1)
            STemp = FF(N - 1) * 2
            For I = 1 To NEnds : FFTrailer(I - 1) = STemp - FF(N - I - 1) : Next I
        End If

        ' Filter forwards.

        'checking here 3/14/16
        Started = False
        For I = -NEnds + 1 To 0
            Call TAN2FILT(FFLeadIn(I + NEnds - 1), Started) '+NEnds - 1 
        Next I

        Started = True
        For I = 1 To N
            Call TAN2FILT(FF(I - 1), Started)
        Next I

        Erase FFLeadIn

        If Backwards Then

            For I = N + 1 To N + NEnds
                Call TAN2FILT(FFTrailer(I - N - 1), Started)
            Next I

            '   Filter backwards.
            Started = False 'True           ' Works best using old forward values.

            For I = N + NEnds To N + 1 Step -1
                Call TAN2FILT(FFTrailer(I - N - 1), Started) 'stays zero for speed, OK on both
            Next I

            Started = True
            For I = N To 1 Step -1
                Call TAN2FILT(FF(I - 1), Started) ' zeros here too for speed , not for decimatefilterfreq
            Next I

            'looks like all good up to here 'steve   '3/23/16 as well
            Erase FFTrailer

        End If

    End Sub
    ' end sub filter


    Public Sub SetTanFilt(FilterFreq As Double, Delta As Double) 'added ByRef 3/14/16

        Dim AL1(0 To 3) As Double, AL2(0 To 3) As Double, BL0(0 To 3) As Double '1 to 4
        Dim BL1(0 To 3) As Double, BL2(0 To 3) As Double, HF(0 To 3) As Double '1 to 4
        Dim FB As Double, FREQ As Double, FH As Double
        Dim BZERO As Double, ABZ As Double, PHS As Double
        Dim I As Integer, MM As Integer

        MM = 4

        Call LPTB(MM, Delta, FilterFreq, AL1, AL2, BZERO) 'removed parenthesis AL1 AL2  
        For I = 1 To MM / 2
            BL0(I - 1) = BZERO : BL1(I - 1) = 2.0# * BZERO : BL2(I - 1) = BZERO '-1  'used ByRef for BZERO 'steve
            '        Debug.Print "I,AL1,AL2,BL0,BL1,BL2= "; AL1(I); AL2(I); BL0(I); BL1(I); BL2(I)
        Next I

        B01 = BL0(1 - 1) : B02 = BL0(2 - 1) '-1
        A11 = AL1(1 - 1) : A12 = AL1(2 - 1) : A21 = AL2(1 - 1) : A22 = AL2(2 - 1) '-1
        '    Debug.Print "Filter Coeffs. = "; B01; B02; A11; A12; A21; A22; BZERO

        GoTo SKIPCHECK ' this block is skipped 'steve
        ''For I = 1 To 15
        ''FREQ = I * 2
        ''Call TTRAN(AL1(), AL2(), BL0(), BL1(), BL2(), MM, Delta, FREQ, ABZ, PHS)
        ''Debug.Print "FREQ, ABZ= "; FREQ; ABZ
        ''HF(I) = ABZ
        ''Next I

SKIPCHECK:

    End Sub ' settanfilt


    Public Sub LPTB(MM As Integer, T As Double, B As Double, ByRef A1() As Double, ByRef A2() As Double, ByRef BZERO As Double) 'added ByRefs 3/14/16

        Dim I As Integer, M As Integer, M1 As Integer
        Dim ANG As Double, FACT As Double
        Dim Factor As Double, FFN As Double, SECTOR As Double
        Dim AM As Double, BM As Double, AMS As Double
        Dim DEN As Double, WEDGE As Double, Temp As Double, DTemp As Double

        M = MM
        ANG = 3.14159265359 * B * T
        FACT = Math.Sin(ANG) / Math.Cos(ANG)
        M1 = M - M / 2
        Factor = 1
        FFN = M
        SECTOR = 3.14159265359 / FFN
        WEDGE = SECTOR / 2.0#
        For I = 1 To M1
            FFN = I - 1
            ANG = FFN * SECTOR + WEDGE
            AM = FACT * Math.Sin(ANG)
            BM = FACT * Math.Cos(ANG)
            AMS = AM * AM
            DEN = (1.0# + BM) ^ 2 + AMS
            A1(I - 1) = -2.0# * ((1.0# - BM * BM) - AMS) / DEN
            A2(I - 1) = ((1.0# - BM) ^ 2 + AMS) / DEN
            DTemp = (1 + A1(I - 1) + A2(I - 1)) / 4 '-1
            Factor = Factor * DTemp
            'Debug.Print "A1, A2, Factor = "; I; A1(I-1); A2(I-1); DTemp; Factor
        Next I
        Temp = M1
        BZERO = Factor ^ (1 / Temp)
    End Sub ' LPTB

    Sub TAN2FILT(ByRef W As Single, ByRef Running As Integer)  'added ByRef 3/14/16 'removed static and added below 3/15/16

        Static Dim WM1 As Double, WM2 As Double ' made static 3/15/16
        Static Dim Y1 As Double, Y2 As Double
        Static Dim Y1M1 As Double, Y1M2 As Double
        Static Dim Y2M1 As Double, Y2M2 As Double
        Static Dim I As Integer

        If Not Running Then
            Y1M2 = W : Y1M1 = W : Y2M2 = W : Y2M1 = W
            WM2 = W : WM1 = W
            Running = True
        End If

        '  Debug.Print B01; B02; A11; A12; A21; A22
        '  Debug.Print W; Y1; Y2; Y1M2; Y1M1; Y2M2; Y2M1; WM2; WM1
        Y1 = B01 * (W + 2.0! * WM1 + WM2) - A11 * Y1M1 - A21 * Y1M2
        Y2 = B02 * (Y1 + 2.0! * Y1M1 + Y1M2) - A12 * Y2M1 - A22 * Y2M2
        Y1M2 = Y1M1 : Y1M1 = Y1
        Y2M2 = Y2M1 : Y2M1 = Y2
        WM2 = WM1 : WM1 = W
        W = Y2
    End Sub

    Public Sub ZProfileVsDistance()

        Dim L As Long, LStart As Long, LEnd As Long, IDMax As Long, IDTotal As Long
        Dim I As Long, ID As Long, IDD As Long, IDp1 As Long, Ip1 As Long
        Dim IT As Long, ITLast As Long, LDelete As Long
        Dim STemp As Single, ZProAv As Double, ZProAvSmooth As Double, SpeedCutoff As Single
        Dim Zoffset As Double, Zslope As Double, DeltaXout As Double
        Dim ZProDistT() As Single, ZProDistTsmooth() As Single, IntArrayT() As Integer
        Dim ZTemp As Double, XFraction As Double
        Dim BeamCalDateTime(0 To 9) As Date  ' GFH 10/15/02. 'steve changed to 0 to 9, 3/11/16
        Dim SmoothEndsFreq As Double, SmoothFilterFreq As Double
        Dim VarTemp As Object, LDecimate1 As Long
        Dim IDStartDelete As Long, IDecimatePlot As Long

        ''Call WriteSbar("Decimating ZProTime", "T", "", "")

        ' GFH 2/6/00.
        ' DeltaD must be reset each time this subroutine is executed because
        ' it is set to 2.5 mm for plotting, etc, after interpolation. See below.
        DeltaD = DatronCal * IDecimate

        LStart = LStartSamples : LEnd = LEndSamples
        If LEnd > LSamples Then LEnd = LSamples

        ' Find number of Datron samples and limit to IDMax for length of the beam.
        IDMax = 99999 ' 18000 '12539 ' Injun modified for 21.8ft at 0.00167487 ft sample spacing - 07/29/05. '01-05-2012
        IDMax = IDMax ' To allow for decimation to 2.5 mm spacing.
        IDStartDelete = 260 '132  '  0 '1792 ' Places the start point 3 inches from the edge of the slab  '01-09-2012  ''steve changed to 260 for full profile 3/28/16
        ' at 0.00167487 ft sample spacing.
        IDMax = IDMax + IDStartDelete ' 14 ft 6 inches at 0.00167487 ft sample spacing
        ' plus the amount to be deleted at the start.
        ID = 0
        For I = LStart To LEnd
            If Datron(I - 1) > 0 Then ID = ID + 1 '-1
            If ID = IDMax Then
                LEnd = I
            End If
        Next I
        IDTotal = ID
        If IDMax > IDTotal Then
            IDMax = IDTotal
        End If
        ' Debug.Print "IDMax = "; IDMax

        L = LEnd - LStart + 1

        ReDim ZProDistT(0 To L - 1), ZHiPass(0 To Selcom.Length - 1) 'steve changed to Selcom.Length for the fix 3/24/16
        ReDim ZProTime(0 To Selcom.Length - 1) '' steve added line 3/24/16 
        For I = 0 To Selcom.Length - 1 '' steve added line 3/24/16 
            ZProTime(I) = Selcom(I)  ' GFH 09/13/01. For transverse. No accelerometer.'removed parenthesis ()   () '''steve
            ZHiPass(I) = Selcom(I)  ' GFH 09/25/02. For transverse. To smooth around the end points.'removed parenthesis '''steve
        Next

        S$ = "Filtering ZproTime; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"
        '  Debug.Print S$
        ''Call WriteSbar("Filtering ZProTime", "T", "", "")

        SpeedCutoff = SpeedMax / 2 ' Speedmax is the maximum speed in the record.
        DecimateFilterFreq = SpeedMax / (DatronCal * IDecimate * 4) '/ 125
        If DecimateFilterFreq > 0 Then  ' GFH 09/13/01. If datron pulses.
            Call Filter(DecimateFilterFreq, DeltaT, ZProTime, L, True) 'removed parenthesis '''steve
            'ZProTime = ZProTime
        End If

        ' Filter profile to smooth around the end points to minimize jitter in the zero offset.
        SmoothEndsFreq = DecimateFilterFreq / 64
        If DecimateFilterFreq > 0 Then  ' GFH 09/13/01. If datron pulses.
            Call Filter(SmoothEndsFreq, DeltaT, ZHiPass, L, True) 'removed parenthesis '''steve 
        End If

        S$ = "Decimating ZProTime; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"
        ' Debug.Print S$
        ''Call WriteSbar("Decimating ZProTime", "T", "", "")

        ReDim ZProDistT(0 To L - 1), IntArrayT(0 To L - 1) '-1
        ReDim ZProDistTsmooth(0 To L - 1) '-1
        '  STemp = DecimateFilterFreq * 4 / 12   ' ft/s
        '  Debug.Print "Cutoff Speed = "; SpeedCutoff; IDecimate ' STemp; IDecimate
        IT = LStart - 1 : ITLast = IT
        ID = 0 : IDD = 0
        ZProAv = 0 : ZProAvSmooth = 0 : LTemp = 0
        For I = 1 To L
            IT = IT + 1
            ZProAv = ZProAv + ZProTime(I - 1) '-1
            ZProAvSmooth = ZProAvSmooth + ZHiPass(I - 1)
            If Datron(IT - 1) > 0 Then '-1
                IDD = IDD + 1
                LTemp = LTemp + 1
            End If
            If IDD = IDecimate Then
                ID = ID + 1
                IDD = 0
                If Speed(IT - 1) > SpeedCutoff Then '-1
                    '       Set equal to the closest ZProTime sample. The filter will
                    '       antialias properly above speedcutoff. GFH Comments added 2/6/00
                    ZProDistT(ID - 1) = ZProTime(I - 1) '-1
                    ZProDistTsmooth(ID - 1) = ZHiPass(I - 1) '-1
                Else
                    '       Filter will not antialias properly. So take the average
                    '       of ZProTime over the period from ITLast to IT. GFH Comments added 2/6/00
                    ZProDistT(ID - 1) = ZProAv / (IT - ITLast) '-1
                    ZProDistTsmooth(ID - 1) = ZProAvSmooth / (IT - ITLast) '-1
                End If
                ZProAv = 0 : ZProAvSmooth = 0 : ITLast = IT
                IntArrayT(ID - 1) = Speed(IT - 1) * SpeedtoInt    ' Save speed as an integer. '-1
            End If
        Next I

        LDecimate = ID - IDStartDelete
        If LDecimate < 1 Then MsgBox("Error: .bin file too short") : Exit Sub 'steve added for error on small files including 20150622(3)_5544_Ref.bin

        ' Sample spacing still at DatronCal value.
        ReDim ZProDist(0 To LDecimate - 1), IntArray(0 To LDecimate - 1) '-1 and this block
        ReDim ZHiPass(0 To LDecimate - 1)
        For I = 1 To LDecimate
            ZProDist(I - 1) = ZProDistT(I + IDStartDelete - 1)
            ZHiPass(I - 1) = ZProDistTsmooth(I + IDStartDelete - 1)
            IntArray(I - 1) = IntArrayT(I + IDStartDelete - 1)
        Next I

        ' This block modified to select reference file by GFH on 10/15/02. ''block commented for not needing correct for curvature function
        ReDim ZProDistT(0 To LDecimate - 1) '-1
        '  WriteProName$ = "yes"
        '  If WriteProName$ = "yes" Then
        ''If frmStart.chkCorrectforCurvature.value = vbChecked Then
        ''FNo = FreeFile()
        '' BeamCalDateTime(0) = "10/15/2002 3:03:20 PM"
        '' BeamCalDateTime(1) = "05/01/2003 0:00:00 PM"
        ''  BeamCalDateTime(2) = "04/01/2004 0:00:00 PM" ' GFH 12/21/04.
        '' BeamCalDateTime(3) = "06/15/2005 0:00:00 PM" ' GFH 06/20/05.
        '' If DatFileDateTime < BeamCalDateTime(0) Then
        ''WriteProName$ = App.Path & "\ProfileOffsets1.pro"
        '' ElseIf DatFileDateTime < BeamCalDateTime(1) Then
        '' WriteProName$ = App.Path & "\ProfileOffsets2.pro"
        ''  ElseIf DatFileDateTime < BeamCalDateTime(2) Then
        ''  WriteProName$ = App.Path & "\ProfileOffsets3.pro"
        ''  ElseIf DatFileDateTime < BeamCalDateTime(3) Then
        ''  WriteProName$ = App.Path & "\ProfileOffsets4.pro" ' Created with 20-1130.dat and Filter = 1000.
        ''  Else
        ''  WriteProName$ = App.Path & "\ProfileOffsets5.pro" ' Created with 20-1130.dat and Filter = 1000.
        ''  End If
        ''  If Dir(WriteProName$) <> "" Then
        ''I = FileLen(WriteProName$) / 4
        '' ReDim ZProDistT(1 To I)
        '' Open WriteProName$ For Binary As FNo
        '' Get FNo, , ZProDistT()
        '' DoEvents()
        '' If LDecimate > I Then LDecimate = I
        ''Close(FNo)
        '' End If
        '' End If

        VarTemp = 121.92 'set manually from program, steve 'frmStart.txtFilterNo.Text
        If IsNumeric(VarTemp) Then ZTemp = VarTemp Else ZTemp = 121.9
        '  ZTemp from the textbox = cutoff frequency in cy/ft. Default = 121.92 cy/ft = 12 * 25.4 / 2.5mm
        SmoothFilterFreq = ZTemp
        Call Filter(SmoothFilterFreq, DeltaD, ZProDist, LDecimate, True) 'removed parenthesis '''steve 

        ' Remove reference profile offsets first.
        For I = 1 To LDecimate '-1 block
            ZProDist(I - 1) = ZProDist(I - 1) - ZProDistT(I - 1)
            ZHiPass(I - 1) = ZHiPass(I - 1) - ZProDistT(I - 1)
        Next I

        '  Zoffset = ZProDist(1-1)
        '  Zslope = (ZProDist(LDecimate-1) - Zoffset) / (LDecimate - 1)
        Zoffset = ZHiPass(1 - 1)
        Zslope = (ZHiPass(LDecimate - 1) - Zoffset) / (LDecimate - 1)
        For I = 1 To LDecimate
            ZProDist(I - 1) = ZProDist(I - 1) - Zoffset '' - Zslope * (I - 1)  ''steve took out slope for full profile 3/28/16
            ZHiPass(I - 1) = ZHiPass(I - 1) - Zoffset ''- Zslope * (I - 1) ''steve took out slope for full profile 3/28/16
        Next I

        'Call WriteSbar("Saving files", "T", "", "")

        ' ***************************************************************************
        ' If a file is opened and written to, the length will not change if there is
        ' less new data than the original length of the file. This can lead to
        ' incorrect operation if the file is read by another program (ProView in this
        ' case) and calculates the number of data points from the length of the file.
        ' Therefore, always kill a file if it exists before writing to it.
        '****************************************************************************

        ''If False Then ' Write file of profile offsets to be subtracted from the measured profiles. 'check
        '                 Corrects for the curvature in the profiler beam.
        ''FNo = FreeFile()
        '' WriteProName$ = App.Path & "\ProfileOffsets5.pro"

        '    If False Then ' Read and average a previous file with the current file.
        '      IT = FileLen(WriteProName$) / 4
        '      ReDim ZProDistT(1 To IT)
        '      Open WriteProName$ For Binary As FNo
        '      Get FNo, , ZProDistT()
        '      Close (FNo)
        '      If LDecimate > IT Then LDecimate = IT
        '      For I = 1 To LDecimate
        '        ZProDist(I) = (ZProDist(I) + ZProDistT(I)) / 2
        '      Next I
        '    End If
        ''  If Dir(WriteProName$) <> "" Then Kill(WriteProName$) ' GFH 2-27-00
        ''  Open WriteProName$ For Binary As FNo
        '' Put(FNo, , ZProDist())
        '' Close(FNo)

        ''End If

        For I = 1 To LDecimate
            ZProDistT(I - 1) = ZProDist(I - 1)
            IntArrayT(I - 1) = IntArray(I - 1)
        Next I


        If 1 = 1 Then '' set to yes for 25mm spacing frmStart.chkmmSpacing.value = vbChecked Or frmStart.chk25mmSpacing.value = vbChecked Then

            IDecimatePlot = 1
            If 1 = 1 Then IDecimatePlot = 10 '' frmStart.chk25mmSpacing.value = vbChecked

            If SmoothFilterFreq > 1 / (4 * CDbl(IDecimatePlot) * DeltaD / DatronCalFactor) Then
                SmoothFilterFreq = 1 / (4 * CDbl(IDecimatePlot) * DeltaD / DatronCalFactor)
                Call Filter(SmoothFilterFreq, DeltaD, ZProDistT, LDecimate, True) 'removed parenthesis '''steve
            End If

            '   Interpolate to a specified sample spacing of 2.5 mm from 0.5105 mm.
            LDecimate1 = LDecimate
            LDecimate = Int(CDbl(LDecimate) * DatronCalFactor / CDbl(IDecimatePlot)) + 1
            ReDim ZProDist(0 To LDecimate - 1), IntArray(0 To LDecimate - 1) ', ZProDistT(1 To 17685) '-1
            ZProDist(1 - 1) = ZProDistT(1 - 1) '-1 block
            IntArray(1 - 1) = IntArrayT(1 - 1)
            For I = 1 To LDecimate - 1

                ZTemp = (I * IDecimatePlot) / DatronCalFactor
                ID = Int(ZTemp)  ' Index for DatronCal sample spacing.
                ' The sample to be interpolated lies between DatronCal * (ID to ID+1).
                Ip1 = I + 1
                ' IDp1 = ID + 1

                XFraction = ZTemp - ID

                ZProDist(Ip1 - 1) = ZProDistT(ID - 1) + (ZProDistT(ID + 1 - 1) - ZProDistT(ID - 1)) * XFraction '-1 block
                IntArray(Ip1 - 1) = IntArrayT(ID - 1) + (IntArrayT(ID + 1 - 1) - IntArrayT(ID - 1)) * XFraction

                'ZProDist(Ip1) = ZProDistT(IDp1) + (ZProDistT(ID + 2) - ZProDistT(IDp1)) * XFraction
                'IntArray(Ip1) = IntArrayT(IDp1) + (IntArrayT(ID + 2) - IntArrayT(IDp1)) * XFraction
            Next I

            DeltaD = (DeltaD * CDbl(IDecimatePlot)) / DatronCalFactor ' = 2.5 mm.

        End If

        Zoffset = ZProDist(1 - 1) '-1
        Zslope = (ZProDist(LDecimate - 1) - Zoffset) / (LDecimate - 1) '-1
        For I = 1 To LDecimate
            ZProDist(I - 1) = ZProDist(I - 1) - Zoffset ''- Zslope * (I - 1) '-1 ''steve took out slope for full profile 3/28/16
        Next I
        ' Write profile at 2.5 mm spacing to a text file. GFH 10-14-05.
        Dim ProfileRecord$   '-----------12-19-2011
        Dim cmd1 As Odbc.OdbcCommand, strSQL1 As String, strSQL2 As String, strSQL As String  '----12-27-2011 '' changed from ADODB.Command

        ''Dim txtfile As String = My.Application.Info.DirectoryPath & "\test.txt" 'steve
        ''Dim outfile As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(txtfile, True) 'steve write text file

        'clear binary processing results for new results
        binX.Clear()
        binZ.Clear()

        If 1 = 1 Then 'frmStart.chkWriteTextFile.value = vbChecked Then
            ''If Dir(DatFileName$ & ".txt") <> "" Then Kill(DatFileName$ & ".txt") 'check for datfilename$
            FNo = FreeFile()
            '''steve Open DatFileName$ & ".txt" For Output As FNo
            For I = 1 To LDecimate
                'S$ = LPad(8, Format(DeltaD * (I - 1), "0.0000")) & LPad(8, Format(ZProDist(I), "0.000")) '12-27-2011
                S$ = (Space(2) & Format(DeltaD * (I - 1), "0.0000")) & (Space(3) & Format(ZProDist(I - 1), "0.000")) '12-27-2011 'translated 3/10/16 SJA
                'Log.Items.Add(S$) 'steve
                binX.Add(Math.Round(DeltaD * (I - 1), 4))
                binZ.Add(Math.Round(ZProDist(I - 1), 3))
                ''outfile.WriteLine(S$) 'steve
                'MsgBox(S$)
                ' S$ = Format(ZProDist(I), "0.000") '12-27-2011
                ' If I = 1 Then '12-27-2011
                ' ProfileRecord = Trim(S$) '12-27-2011
                '  Else: ProfileRecord = Trim(ProfileRecord) & "," & Trim(S$) '12-27-2011
                ' End If '12-27-2011

                '''stevePrint #FNo, S$  '12-27-2011
            Next I
            'Print #FNo, ProfileRecord '12-27-2011

            '''temp Close(FNo)
        End If

        ''outfile.Close() 'steve

        GoTo SkipSQL
        '---------------------SQL-----12-27-2011
        Dim Sdatfiledatetime$, DatPosition$, SFilename$, LFilename$, RunNumber$
        Dim LngPos1$, LngPos2$

        Sdatfiledatetime$ = Format$(DatFileDateTime, "ddmmmhhnn")

        LngPos1 = InStr(FileNameOnly$, "_")
        LngPos2 = InStr(FileNameOnly$, ".")
        '''steveSFilename$ = Left$(FileNameOnly, LngPos1 - 1)

        'SFilename$ = FileNameOnly
        'RunNumber$ = 9966

        If WritingDirDocs = True Then
            RunNumber$ = Mid$(FileNameOnly, LngPos1 + 1, Len(WorkingDir$) - LngPos1 - 1)

        Else
            RunNumber$ = Mid$(FileNameOnly, LngPos1 + 1, LngPos2 - LngPos1 - 1)
        End If

        '-------------------------------------------------------------------
        'If Mid$(WorkingDir$, Len(WorkingDir$) - 9, 3) = "Ext" Then
        '   DatPosition$ = Mid$(WorkingDir$, Len(WorkingDir$) - 14, 4)
        'Else
        '   DatPosition$ = Mid$(WorkingDir$, Len(WorkingDir$) - 4, 4)
        'End If
        'LFilename$ = DatPosition$ & SFilename$ & "-" & Sdatfiledatetime$
        '-------------------------------------------------------------------------
        LFilename$ = SFilename$ & "-" & Sdatfiledatetime$

        LFilename$ = SFilename$ & "-" & Sdatfiledatetime$
        strSQL1 = "Insert into profile (Filename,date_time,repetition,records) values("
        For I = 1 To LDecimate
            S$ = Format(ZProDist(I), "0.000")
            If I = 1 Then
                ProfileRecord = Trim(S$)
            Else : ProfileRecord = Trim(ProfileRecord) & "," & Trim(S$)
            End If

        Next I

        '''steveIf NewCn.State = adStateClosed Then
        '''steveNewCn.ConnectionString = Connstring
        '''steveNewCn.ConnectionTimeout = 0
        '''steveNewCn.CommandTimeout = 0
        '''steveNewCn.Open()
        '''steveNewCn.CursorLocation = adUseClient
        '''steveEnd If

        '''stevecmd1 = New ADODB.Command
        '''stevecmd1.ActiveConnection = NewCn

        strSQL2 = "'" & LFilename$ & "','" & DatFileDateTime & "'," & "'" & RunNumber$ & "'," & "'" & Trim(ProfileRecord$) & "')"
        strSQL = strSQL1 & strSQL2
        cmd1.CommandText = strSQL
        cmd1.CommandTimeout = 0

        '  Debug.Print "strsq= "; strSQL


        '''steveIf NewCn.State = 0 Then
        '''steveNewCn.BeginTrans()
        '''steveEnd If
        '''steveDoEvents()
        '''stevecmd1.Execute()
        '''steveDoEvents()
        '------------------------------------------------------------------------------------------------------------


SkipSQL:

        S$ = "Plotting decimated profile; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"
        '  Debug.Print S$
        '''steveCall WriteSbar("Plotting ZProDist", "T", "", "")

        '''steveCall WriteSbar("ZProDist finished", "T", "", "")
        S$ = "Finished ZproDist; elapsed time = " & Format$(DateDiff("s", NTime, Now), "0") & " seconds"
        ' Debug.Print S$

        '  S = "Number of raw data points = " & Format(L, "#,##0") & " " & vbCrLf
        '  S = S & "Number of bad Selcom Status points = " & Format(NBadStatus, "#,##0") & vbCrLf
        ' First two lines written in Sub ReadandProcessDatFile().
        '''steveS = frmStart.txtStatusMessage.Text
        S = S & "Number of data points at " & Format(DeltaD * DatronCalFactor * 12, "0.0000")
        S = S & " in spacing = " & Format(LDecimate1, "#,##0") & " " & vbCrLf
        S = S & "Number of output data points at " & Format(DeltaD * 12, "0.0000")
        S = S & " in spacing = " & Format(LDecimate, "#,##0") & " "
        '''stevefrmStart.txtStatusMessage.Text = S

    End Sub

    Public Sub EraseAllArrays()

        Erase Status, Datron, Selcom, BVtoF, ZDotDbl, ZDot, ZDotS
        Erase ZVehicle, Speed, ZProTime, ZProDist, ZHiPass
        Erase IntArray, SingleArray

    End Sub

    'end .bin processing subs

End Class

''Option Explicit On
''Attribute VB_Name = "Filters"
