using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Platform.GTK;

namespace Microsoft.Maui.Controls.Platform
{
	public static class EntryExtensions
	{
		public static void UpdateText(this EntryWrapper platformControl, Entry entry)
		{
			if (entry.IsPassword)
			{
				platformControl.Entry.InputPurpose = Gtk.InputPurpose.Password;
				platformControl.Entry.Visibility = false;
			}

			platformControl.Entry.Text = entry.Text;
		}
	}
}
