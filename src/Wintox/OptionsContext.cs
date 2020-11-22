using System.Windows.Forms;

namespace Wintox
{
	public partial class OptionsContext : Form
	{
		public OptionsContext()
		{
			InitializeComponent();
		}

		private void OptionsContext_FormClosing(object sender, FormClosingEventArgs e)
		{
			Visible  = false;
			e.Cancel = true;
		}
	}
}