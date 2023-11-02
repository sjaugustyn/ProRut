<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series4 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series5 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series6 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series7 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Title1 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
        Dim Title2 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
        Dim Title3 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Button4 = New System.Windows.Forms.Button()
        Me.DirectoryEntry1 = New System.DirectoryServices.DirectoryEntry()
        Me.DirectoryEntry2 = New System.DirectoryServices.DirectoryEntry()
        Me.DirectoryEntry3 = New System.DirectoryServices.DirectoryEntry()
        Me.DirectoryEntry4 = New System.DirectoryServices.DirectoryEntry()
        Me.FolderBrowserDialog2 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FolderBrowserDialog3 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FolderBrowserDialog4 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Log = New System.Windows.Forms.ListBox()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.FolderBrowserDialog5 = New System.Windows.Forms.FolderBrowserDialog()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.FolderBrowserDialog6 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FolderBrowserDialog7 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FolderBrowserDialog8 = New System.Windows.Forms.FolderBrowserDialog()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog9 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FolderBrowserDialog10 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.TextBox8 = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog2 = New System.Windows.Forms.OpenFileDialog()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ButtonResults = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.bincheckbox = New System.Windows.Forms.CheckBox()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button4
        '
        Me.Button4.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Location = New System.Drawing.Point(12, 304)
        Me.Button4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(228, 100)
        Me.Button4.TabIndex = 10
        Me.Button4.Text = "Run"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Log
        '
        Me.Log.FormattingEnabled = True
        Me.Log.ItemHeight = 16
        Me.Log.Items.AddRange(New Object() {"Log"})
        Me.Log.Location = New System.Drawing.Point(12, 410)
        Me.Log.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Log.Name = "Log"
        Me.Log.Size = New System.Drawing.Size(405, 452)
        Me.Log.TabIndex = 12
        '
        'Chart1
        '
        Me.Chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
        ChartArea1.AxisX.Interval = 10.0R
        ChartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount
        ChartArea1.AxisX.MajorGrid.Interval = 10.0R
        ChartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.DarkGray
        ChartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash
        ChartArea1.AxisX.MajorTickMark.Interval = 2.0R
        ChartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.DarkGray
        ChartArea1.AxisX.Minimum = 0R
        ChartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount
        ChartArea1.AxisY.MajorGrid.Interval = 0.2R
        ChartArea1.AxisY.MajorGrid.IntervalOffset = 0R
        ChartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.DarkGray
        ChartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash
        ChartArea1.AxisY.MajorTickMark.Interval = 0.2R
        ChartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.DarkGray
        ChartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
        ChartArea1.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea1)
        Me.Chart1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Legend1.Alignment = System.Drawing.StringAlignment.Center
        Legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom
        Legend1.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend1)
        Me.Chart1.Location = New System.Drawing.Point(427, 7)
        Me.Chart1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Chart1.Name = "Chart1"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series1.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Series1.Legend = "Legend1"
        Series1.Name = "Leica"
        Series2.ChartArea = "ChartArea1"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series2.Color = System.Drawing.Color.LightSeaGreen
        Series2.Legend = "Legend1"
        Series2.Name = "Leica Sample Spaced"
        Series3.ChartArea = "ChartArea1"
        Series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series3.Legend = "Legend1"
        Series3.Name = "R2R - Leica"
        Series4.ChartArea = "ChartArea1"
        Series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline
        Series4.Color = System.Drawing.Color.Red
        Series4.Legend = "Legend1"
        Series4.Name = "4th Order Polynomial"
        Series5.ChartArea = "ChartArea1"
        Series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series5.Color = System.Drawing.Color.Silver
        Series5.Legend = "Legend1"
        Series5.Name = "Station Profile"
        Series6.ChartArea = "ChartArea1"
        Series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series6.Color = System.Drawing.Color.Black
        Series6.Legend = "Legend1"
        Series6.Name = "Corrected Station Profile"
        Series7.BorderWidth = 2
        Series7.ChartArea = "ChartArea1"
        Series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series7.Color = System.Drawing.Color.DimGray
        Series7.Legend = "Legend1"
        Series7.Name = "Straightedge"
        Me.Chart1.Series.Add(Series1)
        Me.Chart1.Series.Add(Series2)
        Me.Chart1.Series.Add(Series3)
        Me.Chart1.Series.Add(Series4)
        Me.Chart1.Series.Add(Series5)
        Me.Chart1.Series.Add(Series6)
        Me.Chart1.Series.Add(Series7)
        Me.Chart1.Size = New System.Drawing.Size(1212, 855)
        Me.Chart1.TabIndex = 15
        Me.Chart1.Text = "Chart1"
        Title1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Title1.Name = "ProRut Simulated Straightedge"
        Title1.Text = "ProRut Simulated Straightedge"
        Title2.DockedToChartArea = "ChartArea1"
        Title2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left
        Title2.IsDockedInsideChartArea = False
        Title2.Name = "Y Title"
        Title2.Text = "Elevation (in)"
        Title3.DockedToChartArea = "ChartArea1"
        Title3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom
        Title3.IsDockedInsideChartArea = False
        Title3.Name = "X Title"
        Title3.Text = "South to North (ft)"
        Me.Chart1.Titles.Add(Title1)
        Me.Chart1.Titles.Add(Title2)
        Me.Chart1.Titles.Add(Title3)
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(103, 48)
        Me.TextBox3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(315, 22)
        Me.TextBox3.TabIndex = 18
        Me.TextBox3.Text = "10,25,60,75,110,125,160,175,210,225,260,275"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(16, 49)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(75, 20)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "Stations:"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(195, 86)
        Me.TextBox4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(44, 22)
        Me.TextBox4.TabIndex = 20
        Me.TextBox4.Text = "16.0"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(15, 87)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(164, 20)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "Straightedge Length:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(15, 126)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(168, 20)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "S.E. Center Location:"
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(195, 124)
        Me.TextBox5.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(44, 22)
        Me.TextBox5.TabIndex = 23
        Me.TextBox5.Text = "17.1"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(241, 87)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(23, 20)
        Me.Label9.TabIndex = 24
        Me.Label9.Text = "ft."
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(241, 126)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(23, 20)
        Me.Label10.TabIndex = 25
        Me.Label10.Text = "ft."
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(273, 201)
        Me.TextBox6.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(51, 22)
        Me.TextBox6.TabIndex = 26
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(8, 202)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(250, 20)
        Me.Label11.TabIndex = 27
        Me.Label11.Text = "Rut Depth Measurement Range:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(327, 202)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(23, 20)
        Me.Label12.TabIndex = 28
        Me.Label12.Text = "ft."
        '
        'TextBox7
        '
        Me.TextBox7.Location = New System.Drawing.Point(12, 7)
        Me.TextBox7.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(297, 22)
        Me.TextBox7.TabIndex = 29
        Me.TextBox7.Text = "Select Data Directory..."
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(319, 7)
        Me.Button6.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(100, 25)
        Me.Button6.TabIndex = 30
        Me.Button6.Text = "Browse"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'FolderBrowserDialog10
        '
        Me.FolderBrowserDialog10.Description = "Select Data Directory"
        Me.FolderBrowserDialog10.ShowNewFolderButton = False
        Me.FolderBrowserDialog10.Tag = "folder browser dialog"
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(319, 236)
        Me.Button7.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(100, 25)
        Me.Button7.TabIndex = 32
        Me.Button7.Text = "Browse"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'TextBox8
        '
        Me.TextBox8.Location = New System.Drawing.Point(12, 236)
        Me.TextBox8.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.Size = New System.Drawing.Size(297, 22)
        Me.TextBox8.TabIndex = 31
        Me.TextBox8.Text = "Select Leica Reference File..."
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(319, 272)
        Me.Button3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(100, 25)
        Me.Button3.TabIndex = 34
        Me.Button3.Text = "Browse"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(12, 272)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(297, 22)
        Me.TextBox1.TabIndex = 33
        Me.TextBox1.Text = "Select Individual R2R Reference File..."
        '
        'OpenFileDialog2
        '
        Me.OpenFileDialog2.FileName = "OpenFileDialog2"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 167)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 20)
        Me.Label1.TabIndex = 35
        Me.Label1.Text = "Optional"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Window
        Me.Label2.Location = New System.Drawing.Point(1504, 42)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 17)
        Me.Label2.TabIndex = 36
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Window
        Me.Label3.Location = New System.Drawing.Point(1504, 65)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 17)
        Me.Label3.TabIndex = 37
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.Window
        Me.Label4.Location = New System.Drawing.Point(1407, 42)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(88, 17)
        Me.Label4.TabIndex = 38
        Me.Label4.Text = "Max Rut (in):"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.SystemColors.Window
        Me.Label5.Location = New System.Drawing.Point(1407, 65)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(88, 17)
        Me.Label5.TabIndex = 39
        Me.Label5.Text = "Location (ft):"
        '
        'ButtonResults
        '
        Me.ButtonResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonResults.Location = New System.Drawing.Point(245, 304)
        Me.ButtonResults.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ButtonResults.Name = "ButtonResults"
        Me.ButtonResults.Size = New System.Drawing.Size(173, 98)
        Me.ButtonResults.TabIndex = 40
        Me.ButtonResults.Text = "View Results"
        Me.ButtonResults.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.SystemColors.Window
        Me.Label13.Location = New System.Drawing.Point(1245, 65)
        Me.Label13.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(75, 17)
        Me.Label13.TabIndex = 44
        Me.Label13.Text = "Sec. Pass:"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.SystemColors.Window
        Me.Label14.Location = New System.Drawing.Point(1245, 42)
        Me.Label14.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(76, 17)
        Me.Label14.TabIndex = 43
        Me.Label14.Text = "Veh. Pass:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.SystemColors.Window
        Me.Label15.Location = New System.Drawing.Point(1328, 65)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(0, 17)
        Me.Label15.TabIndex = 42
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.SystemColors.Window
        Me.Label16.Location = New System.Drawing.Point(1328, 42)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(0, 17)
        Me.Label16.TabIndex = 41
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.SystemColors.Window
        Me.Label17.Location = New System.Drawing.Point(1245, 18)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(42, 17)
        Me.Label17.TabIndex = 46
        Me.Label17.Text = "Date:"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.BackColor = System.Drawing.SystemColors.Window
        Me.Label18.Location = New System.Drawing.Point(1293, 18)
        Me.Label18.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(0, 17)
        Me.Label18.TabIndex = 45
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.BackColor = System.Drawing.SystemColors.Window
        Me.Label19.Location = New System.Drawing.Point(1407, 18)
        Me.Label19.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(56, 17)
        Me.Label19.TabIndex = 48
        Me.Label19.Text = "Station:"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.BackColor = System.Drawing.SystemColors.Window
        Me.Label20.Location = New System.Drawing.Point(1504, 18)
        Me.Label20.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(0, 17)
        Me.Label20.TabIndex = 47
        '
        'bincheckbox
        '
        Me.bincheckbox.AutoSize = True
        Me.bincheckbox.Location = New System.Drawing.Point(103, 169)
        Me.bincheckbox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.bincheckbox.Name = "bincheckbox"
        Me.bincheckbox.Size = New System.Drawing.Size(205, 21)
        Me.bincheckbox.TabIndex = 49
        Me.bincheckbox.Text = "Read from binary files (.bin)"
        Me.bincheckbox.UseVisualStyleBackColor = True
        '
        'SaveFileDialog1
        '
        '
        'Form1
        '
        Me.AcceptButton = Me.Button4
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlDark
        Me.ClientSize = New System.Drawing.Size(1604, 870)
        Me.Controls.Add(Me.bincheckbox)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.ButtonResults)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.TextBox8)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.TextBox7)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.TextBox6)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.Chart1)
        Me.Controls.Add(Me.Log)
        Me.Controls.Add(Me.Button4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "Form1"
        Me.Text = "ProRut Simulated Straightedge"
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents DirectoryEntry1 As System.DirectoryServices.DirectoryEntry
    Friend WithEvents DirectoryEntry2 As System.DirectoryServices.DirectoryEntry
    Friend WithEvents DirectoryEntry3 As System.DirectoryServices.DirectoryEntry
    Friend WithEvents DirectoryEntry4 As System.DirectoryServices.DirectoryEntry
    Friend WithEvents FolderBrowserDialog2 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents FolderBrowserDialog3 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents FolderBrowserDialog4 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Log As System.Windows.Forms.ListBox
    Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents FolderBrowserDialog5 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents FolderBrowserDialog6 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents FolderBrowserDialog7 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents FolderBrowserDialog8 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog9 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents FolderBrowserDialog10 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialog2 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ButtonResults As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents bincheckbox As System.Windows.Forms.CheckBox
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
End Class
