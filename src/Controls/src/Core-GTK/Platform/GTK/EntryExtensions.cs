using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Platform.GTK;

namespace Microsoft.Maui.Controls.Platform
{
	public static class EntryExtensions
	{
		public static void UpdateText(this Gtk.Entry platformControl, Entry entry)
		{
			if (entry.IsPassword)
			{
				platformControl.InputPurpose = Gtk.InputPurpose.Password;
				platformControl.Visibility = false;
			}

			platformControl.Text = entry.Text;
		}
	}
}
