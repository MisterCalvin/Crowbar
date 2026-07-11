Public Module ThemeManager

	Private ReadOnly DarkBackColor As Color = Color.FromArgb(30, 30, 30)
	Private ReadOnly DarkPanelColor As Color = Color.FromArgb(34, 34, 34)
	Private ReadOnly DarkInputColor As Color = Color.FromArgb(42, 42, 42)
	Private ReadOnly DarkButtonColor As Color = Color.FromArgb(50, 50, 50)
	Private ReadOnly DarkBorderColor As Color = Color.FromArgb(72, 72, 72)
	Private ReadOnly DarkTextColor As Color = Color.FromArgb(232, 232, 232)
	Private ReadOnly DarkMutedTextColor As Color = Color.FromArgb(180, 180, 180)
	Private ReadOnly DarkSelectionColor As Color = Color.FromArgb(62, 92, 138)
	Private ReadOnly DarkLinkColor As Color = Color.FromArgb(130, 175, 255)

	Public Sub ApplyTheme(ByVal root As Control, ByVal darkModeIsEnabled As Boolean)
		If root Is Nothing Then
			Return
		End If

		ApplyWindowTheme(root, darkModeIsEnabled)
		ApplyControlTheme(root, darkModeIsEnabled)
	End Sub

	Private Sub ApplyWindowTheme(ByVal control As Control, ByVal darkModeIsEnabled As Boolean)
		Dim form As Form = TryCast(control, Form)
		If form Is Nothing OrElse Not form.IsHandleCreated Then
			Return
		End If

		Try
			Dim enabled As Integer = If(darkModeIsEnabled, 1, 0)
			If DwmSetWindowAttribute(form.Handle, 20, enabled, 4) <> 0 Then
				DwmSetWindowAttribute(form.Handle, 19, enabled, 4)
			End If
		Catch ex As Exception
			' Older Windows versions simply keep the normal title bar.
		End Try
	End Sub

	Private Sub ApplyControlTheme(ByVal control As Control, ByVal darkModeIsEnabled As Boolean)
		Dim backColor As Color = If(darkModeIsEnabled, DarkBackColor, SystemColors.Control)
		Dim panelColor As Color = If(darkModeIsEnabled, DarkPanelColor, SystemColors.Control)
		Dim inputColor As Color = If(darkModeIsEnabled, DarkInputColor, SystemColors.Window)
		Dim textColor As Color = If(darkModeIsEnabled, DarkTextColor, SystemColors.ControlText)
		Dim inputTextColor As Color = If(darkModeIsEnabled, DarkTextColor, SystemColors.WindowText)

		If TypeOf control Is TextBoxBase OrElse TypeOf control Is ListBox Then
			control.BackColor = inputColor
			control.ForeColor = inputTextColor
		ElseIf TypeOf control Is ComboBox Then
			ApplyComboBoxTheme(CType(control, ComboBox), darkModeIsEnabled)
		ElseIf TypeOf control Is Button Then
			Dim button As Button = CType(control, Button)
			button.UseVisualStyleBackColor = Not darkModeIsEnabled
			button.FlatStyle = If(darkModeIsEnabled, FlatStyle.Flat, FlatStyle.Standard)
			button.BackColor = If(darkModeIsEnabled, DarkButtonColor, SystemColors.Control)
			button.ForeColor = textColor
			If darkModeIsEnabled Then
				button.FlatAppearance.BorderColor = DarkBorderColor
				button.FlatAppearance.MouseOverBackColor = Color.FromArgb(62, 62, 62)
				button.FlatAppearance.MouseDownBackColor = Color.FromArgb(72, 72, 72)
			End If
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
		ElseIf TypeOf control Is GroupBox Then
			control.BackColor = backColor
			control.ForeColor = textColor
		ElseIf TypeOf control Is Panel OrElse TypeOf control Is SplitContainer Then
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
		ElseIf TypeOf control Is LinkLabel Then
			ApplyLinkLabelTheme(CType(control, LinkLabel), darkModeIsEnabled)
		End If

		If control.ContextMenuStrip IsNot Nothing Then
			ApplyToolStripTheme(control.ContextMenuStrip, darkModeIsEnabled)
		End If

		For Each child As Control In control.Controls
			ApplyControlTheme(child, darkModeIsEnabled)
		Next
	End Sub

	Private Sub ApplyComboBoxTheme(ByVal comboBox As ComboBox, ByVal darkModeIsEnabled As Boolean)
		comboBox.BackColor = If(darkModeIsEnabled, DarkInputColor, SystemColors.Window)
		comboBox.ForeColor = If(darkModeIsEnabled, DarkTextColor, SystemColors.WindowText)

		RemoveHandler comboBox.DrawItem, AddressOf ComboBox_DrawItem
		If darkModeIsEnabled Then
			comboBox.FlatStyle = FlatStyle.Flat
			comboBox.DrawMode = DrawMode.OwnerDrawFixed
			AddHandler comboBox.DrawItem, AddressOf ComboBox_DrawItem
		Else
			comboBox.FlatStyle = FlatStyle.Standard
			comboBox.DrawMode = DrawMode.Normal
		End If
	End Sub

	Private Sub ApplyDataGridViewTheme(ByVal grid As DataGridView, ByVal darkModeIsEnabled As Boolean)
		If darkModeIsEnabled Then
			grid.BackgroundColor = DarkInputColor
			grid.GridColor = DarkBorderColor
			grid.BorderStyle = BorderStyle.FixedSingle
			grid.DefaultCellStyle.BackColor = DarkInputColor
			grid.DefaultCellStyle.ForeColor = DarkTextColor
			grid.DefaultCellStyle.SelectionBackColor = DarkSelectionColor
			grid.DefaultCellStyle.SelectionForeColor = Color.White
			grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(37, 37, 37)
			grid.AlternatingRowsDefaultCellStyle.ForeColor = DarkTextColor
			grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45)
			grid.ColumnHeadersDefaultCellStyle.ForeColor = DarkTextColor
			grid.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45)
			grid.RowHeadersDefaultCellStyle.ForeColor = DarkTextColor
			grid.EnableHeadersVisualStyles = False

			For Each column As DataGridViewColumn In grid.Columns
				column.DefaultCellStyle.BackColor = DarkInputColor
				column.DefaultCellStyle.ForeColor = DarkTextColor
				column.DefaultCellStyle.SelectionBackColor = DarkSelectionColor
				column.DefaultCellStyle.SelectionForeColor = Color.White
			Next
		Else
			grid.BackgroundColor = SystemColors.AppWorkspace
			grid.GridColor = SystemColors.ControlDark
			grid.BorderStyle = BorderStyle.FixedSingle
			grid.DefaultCellStyle.BackColor = SystemColors.Window
			grid.DefaultCellStyle.ForeColor = SystemColors.ControlText
			grid.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight
			grid.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText
			grid.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Window
			grid.AlternatingRowsDefaultCellStyle.ForeColor = SystemColors.ControlText
			grid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control
			grid.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText
			grid.RowHeadersDefaultCellStyle.BackColor = SystemColors.Control
			grid.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText
			grid.EnableHeadersVisualStyles = True
		End If
	End Sub

	Private Sub ApplyLinkLabelTheme(ByVal linkLabel As LinkLabel, ByVal darkModeIsEnabled As Boolean)
		If darkModeIsEnabled Then
			linkLabel.BackColor = DarkBackColor
			linkLabel.ForeColor = DarkTextColor
			linkLabel.LinkColor = DarkLinkColor
			linkLabel.ActiveLinkColor = Color.FromArgb(170, 205, 255)
			linkLabel.VisitedLinkColor = Color.FromArgb(180, 155, 225)
		Else
			linkLabel.BackColor = SystemColors.Control
			linkLabel.ForeColor = SystemColors.ControlText
			linkLabel.LinkColor = SystemColors.HotTrack
			linkLabel.ActiveLinkColor = Color.Red
			linkLabel.VisitedLinkColor = Color.Purple
		End If
	End Sub

	Private Sub ApplyToolStripTheme(ByVal toolStrip As ToolStrip, ByVal darkModeIsEnabled As Boolean)
		Dim renderer As ToolStripRenderer = If(darkModeIsEnabled, CType(New DarkToolStripRenderer(), ToolStripRenderer), CType(New ToolStripProfessionalRenderer(), ToolStripRenderer))
		ToolStripManager.Renderer = renderer

		If darkModeIsEnabled Then
			toolStrip.BackColor = DarkPanelColor
			toolStrip.ForeColor = DarkTextColor
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
		item.BackColor = If(darkModeIsEnabled, DarkPanelColor, SystemColors.Control)
		item.ForeColor = If(darkModeIsEnabled, DarkTextColor, SystemColors.ControlText)

		If TypeOf item Is ToolStripControlHost Then
			Dim host As ToolStripControlHost = CType(item, ToolStripControlHost)
			If host.Control IsNot Nothing Then
				ApplyControlTheme(host.Control, darkModeIsEnabled)
			End If
		End If

		If TypeOf item Is ToolStripDropDownItem Then
			Dim dropDownItem As ToolStripDropDownItem = CType(item, ToolStripDropDownItem)
			If dropDownItem.DropDown IsNot Nothing Then
				ApplyToolStripTheme(dropDownItem.DropDown, darkModeIsEnabled)
			End If
		End If
	End Sub

	Private Sub ComboBox_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs)
		If e.Index < 0 Then
			Return
		End If

		Dim comboBox As ComboBox = CType(sender, ComboBox)
		Dim selected As Boolean = (e.State And DrawItemState.Selected) = DrawItemState.Selected
		Using backgroundBrush As New SolidBrush(If(selected, DarkSelectionColor, DarkInputColor))
			e.Graphics.FillRectangle(backgroundBrush, e.Bounds)
		End Using

		Dim text As String = comboBox.GetItemText(comboBox.Items(e.Index))
		Using textBrush As New SolidBrush(DarkTextColor)
			e.Graphics.DrawString(text, e.Font, textBrush, e.Bounds)
		End Using
	End Sub

	Private Class DarkToolStripRenderer
		Inherits ToolStripProfessionalRenderer

		Public Sub New()
			MyBase.New(New DarkColorTable())
		End Sub

		Protected Overrides Sub OnRenderSeparator(ByVal e As ToolStripSeparatorRenderEventArgs)
			Using pen As New Pen(DarkBorderColor)
				e.Graphics.DrawLine(pen, 4, e.Item.Height \ 2, e.Item.Width - 4, e.Item.Height \ 2)
			End Using
		End Sub

		Protected Overrides Sub OnRenderItemCheck(ByVal e As ToolStripItemImageRenderEventArgs)
			Using brush As New SolidBrush(DarkSelectionColor)
				e.Graphics.FillRectangle(brush, e.ImageRectangle)
			End Using
			MyBase.OnRenderItemCheck(e)
		End Sub
	End Class

	Private Class DarkColorTable
		Inherits ProfessionalColorTable

		Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
			Get
				Return DarkPanelColor
			End Get
		End Property

		Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
			Get
				Return DarkPanelColor
			End Get
		End Property

		Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
			Get
				Return DarkPanelColor
			End Get
		End Property

		Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
			Get
				Return DarkPanelColor
			End Get
		End Property

		Public Overrides ReadOnly Property MenuItemSelected As Color
			Get
				Return Color.FromArgb(52, 72, 104)
			End Get
		End Property

		Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
			Get
				Return Color.FromArgb(52, 72, 104)
			End Get
		End Property

		Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
			Get
				Return Color.FromArgb(52, 72, 104)
			End Get
		End Property

		Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
			Get
				Return DarkInputColor
			End Get
		End Property

		Public Overrides ReadOnly Property MenuItemPressedGradientMiddle As Color
			Get
				Return DarkInputColor
			End Get
		End Property

		Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
			Get
				Return DarkInputColor
			End Get
		End Property

		Public Overrides ReadOnly Property MenuItemBorder As Color
			Get
				Return DarkBorderColor
			End Get
		End Property

		Public Overrides ReadOnly Property MenuBorder As Color
			Get
				Return DarkBorderColor
			End Get
		End Property

		Public Overrides ReadOnly Property ToolStripBorder As Color
			Get
				Return DarkBorderColor
			End Get
		End Property
	End Class

	<System.Runtime.InteropServices.DllImport("dwmapi.dll")>
	Private Function DwmSetWindowAttribute(ByVal hwnd As IntPtr, ByVal attribute As Integer, ByRef attributeValue As Integer, ByVal attributeSize As Integer) As Integer
	End Function

End Module
