Public Module ThemeManager

	Public Sub ApplyTheme(ByVal root As Control, ByVal darkModeIsEnabled As Boolean)
		If root Is Nothing Then
			Return
		End If

		ApplyControlTheme(root, darkModeIsEnabled)
	End Sub

	Private Sub ApplyControlTheme(ByVal control As Control, ByVal darkModeIsEnabled As Boolean)
		Dim backColor As Color = If(darkModeIsEnabled, Color.FromArgb(32, 32, 32), SystemColors.Control)
		Dim panelColor As Color = If(darkModeIsEnabled, Color.FromArgb(40, 40, 40), SystemColors.Control)
		Dim inputColor As Color = If(darkModeIsEnabled, Color.FromArgb(24, 24, 24), SystemColors.Window)
		Dim textColor As Color = If(darkModeIsEnabled, Color.FromArgb(235, 235, 235), SystemColors.ControlText)
		Dim inputTextColor As Color = If(darkModeIsEnabled, Color.FromArgb(245, 245, 245), SystemColors.WindowText)

		If TypeOf control Is TextBoxBase OrElse TypeOf control Is ComboBox OrElse TypeOf control Is ListBox Then
			control.BackColor = inputColor
			control.ForeColor = inputTextColor
		ElseIf TypeOf control Is Button Then
			Dim button As Button = CType(control, Button)
			button.UseVisualStyleBackColor = Not darkModeIsEnabled
			button.BackColor = If(darkModeIsEnabled, Color.FromArgb(55, 55, 55), SystemColors.Control)
			button.ForeColor = textColor
		ElseIf TypeOf control Is CheckBox Then
			Dim checkBox As CheckBox = CType(control, CheckBox)
			checkBox.UseVisualStyleBackColor = Not darkModeIsEnabled
			checkBox.BackColor = backColor
			checkBox.ForeColor = textColor
		ElseIf TypeOf control Is RadioButton Then
			Dim radioButton As RadioButton = CType(control, RadioButton)
			radioButton.UseVisualStyleBackColor = Not darkModeIsEnabled
			radioButton.BackColor = backColor
			radioButton.ForeColor = textColor
		ElseIf TypeOf control Is TabPage Then
			control.BackColor = backColor
			control.ForeColor = textColor
		ElseIf TypeOf control Is Panel OrElse TypeOf control Is GroupBox Then
			control.BackColor = panelColor
			control.ForeColor = textColor
		Else
			control.BackColor = backColor
			control.ForeColor = textColor
		End If

		If TypeOf control Is DataGridView Then
			ApplyDataGridViewTheme(CType(control, DataGridView), darkModeIsEnabled)
		ElseIf TypeOf control Is ListView Then
			Dim listView As ListView = CType(control, ListView)
			listView.BackColor = inputColor
			listView.ForeColor = inputTextColor
		ElseIf TypeOf control Is TreeView Then
			Dim treeView As TreeView = CType(control, TreeView)
			treeView.BackColor = inputColor
			treeView.ForeColor = inputTextColor
		ElseIf TypeOf control Is ToolStrip Then
			ApplyToolStripTheme(CType(control, ToolStrip), darkModeIsEnabled)
		End If

		If control.ContextMenuStrip IsNot Nothing Then
			ApplyToolStripTheme(control.ContextMenuStrip, darkModeIsEnabled)
		End If

		For Each child As Control In control.Controls
			ApplyControlTheme(child, darkModeIsEnabled)
		Next
	End Sub

	Private Sub ApplyDataGridViewTheme(ByVal grid As DataGridView, ByVal darkModeIsEnabled As Boolean)
		If darkModeIsEnabled Then
			grid.BackgroundColor = Color.FromArgb(24, 24, 24)
			grid.GridColor = Color.FromArgb(70, 70, 70)
			grid.DefaultCellStyle.BackColor = Color.FromArgb(24, 24, 24)
			grid.DefaultCellStyle.ForeColor = Color.FromArgb(245, 245, 245)
			grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(75, 95, 130)
			grid.DefaultCellStyle.SelectionForeColor = Color.White
			grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30)
			grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(48, 48, 48)
			grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(235, 235, 235)
			grid.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(48, 48, 48)
			grid.RowHeadersDefaultCellStyle.ForeColor = Color.FromArgb(235, 235, 235)
			grid.EnableHeadersVisualStyles = False
		Else
			grid.BackgroundColor = SystemColors.AppWorkspace
			grid.GridColor = SystemColors.ControlDark
			grid.DefaultCellStyle.BackColor = SystemColors.Window
			grid.DefaultCellStyle.ForeColor = SystemColors.ControlText
			grid.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight
			grid.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText
			grid.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Window
			grid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control
			grid.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText
			grid.RowHeadersDefaultCellStyle.BackColor = SystemColors.Control
			grid.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText
			grid.EnableHeadersVisualStyles = True
		End If
	End Sub

	Private Sub ApplyToolStripTheme(ByVal toolStrip As ToolStrip, ByVal darkModeIsEnabled As Boolean)
		If darkModeIsEnabled Then
			toolStrip.BackColor = Color.FromArgb(40, 40, 40)
			toolStrip.ForeColor = Color.FromArgb(235, 235, 235)
			toolStrip.Renderer = New DarkToolStripRenderer()
		Else
			toolStrip.BackColor = SystemColors.Control
			toolStrip.ForeColor = SystemColors.ControlText
			toolStrip.Renderer = New ToolStripProfessionalRenderer()
		End If

		For Each item As ToolStripItem In toolStrip.Items
			ApplyToolStripItemTheme(item, darkModeIsEnabled)
		Next
	End Sub

	Private Sub ApplyToolStripItemTheme(ByVal item As ToolStripItem, ByVal darkModeIsEnabled As Boolean)
		item.BackColor = If(darkModeIsEnabled, Color.FromArgb(40, 40, 40), SystemColors.Control)
		item.ForeColor = If(darkModeIsEnabled, Color.FromArgb(235, 235, 235), SystemColors.ControlText)

		If TypeOf item Is ToolStripDropDownItem Then
			Dim dropDownItem As ToolStripDropDownItem = CType(item, ToolStripDropDownItem)
			If dropDownItem.DropDown IsNot Nothing Then
				ApplyToolStripTheme(dropDownItem.DropDown, darkModeIsEnabled)
			End If
		End If
	End Sub

	Private Class DarkToolStripRenderer
		Inherits ToolStripProfessionalRenderer

		Public Sub New()
			MyBase.New(New DarkColorTable())
		End Sub
	End Class

	Private Class DarkColorTable
		Inherits ProfessionalColorTable

		Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
			Get
				Return Color.FromArgb(40, 40, 40)
			End Get
		End Property

		Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
			Get
				Return Color.FromArgb(40, 40, 40)
			End Get
		End Property

		Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
			Get
				Return Color.FromArgb(40, 40, 40)
			End Get
		End Property

		Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
			Get
				Return Color.FromArgb(40, 40, 40)
			End Get
		End Property

		Public Overrides ReadOnly Property MenuItemSelected As Color
			Get
				Return Color.FromArgb(62, 72, 90)
			End Get
		End Property

		Public Overrides ReadOnly Property MenuItemBorder As Color
			Get
				Return Color.FromArgb(85, 95, 115)
			End Get
		End Property

		Public Overrides ReadOnly Property MenuBorder As Color
			Get
				Return Color.FromArgb(70, 70, 70)
			End Get
		End Property
	End Class

End Module
