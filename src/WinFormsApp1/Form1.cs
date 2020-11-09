using System;
using System.Collections.Generic;
using System.Windows.Forms;

using NonInvasiveKeyboardHookLibrary;

using Wintox.Common;
using Wintox.Lib.Constants;
using Wintox.Lib.LowLevelProcessing;
using Wintox.Lib.Models;

namespace WinFormsApp1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			_processor = new LowLevelProcessor(new ExcludingSettings(null)
			{
				Excluded =
				{
					"Program Manager",
					"TextInputHost.exe",
					"ApplicationFrameHost.exe"
				}
			});

			_manager = new KeyboardHookManager();
			_manager.Start();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			listBox1.Items.Clear();

			_windows = _processor.GetOpenedWindows();

			_windows.ForEach(x => listBox1.Items.Add(x.ExecutablePath + "   " + x.Title));
		}

		private void button2_Click(object sender, EventArgs e)
		{
			_processor.SetTopMode(_windows[listBox1.SelectedIndex], WindowTopMode.TopMost);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			_processor.SetTopMode(_windows[listBox1.SelectedIndex], WindowTopMode.NoTopMost);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			_manager.RegisterHotkey(new[] {NonInvasiveKeyboardHookLibrary.ModifierKeys.Alt}, 0x51,
			                        () =>
			                        {
				                        listBox1.Items.Clear();
				                        var active = _processor.GetActive();
				                        
				                        listBox1.Items.Add(active.Title);
				                        
				                        MessageBox.Show("asdasd");
			                        });
		}

		private          List<OpenedWindow>  _windows;
		private readonly LowLevelProcessor   _processor;
		private          KeyboardHookManager _manager;
	}
}