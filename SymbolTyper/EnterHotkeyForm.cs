namespace SymbolTyper;

using System;
using System.Windows.Forms;

public partial class EnterHotkeyForm : Form
{
	public EnterHotkeyForm()
	{
		InitializeComponent();
	}

	private void SubmitButton_Click(object sender, EventArgs e)
	{
		HotkeyManager.UpdateHotkey(HotkeyTextBox.Text);
		Close();
	}

	public Control GetHotkeyTextbox() => HotkeyTextBox;
}
