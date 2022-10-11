using System;
using System.Windows.Forms;

namespace XamlCTaskRunner
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void OnTestRunnerClicked(object sender, EventArgs e)
		{
			var debugXaml_cTask = new DebugXamlCTask();
			debugXaml_cTask.Execute();
		}
	}
}