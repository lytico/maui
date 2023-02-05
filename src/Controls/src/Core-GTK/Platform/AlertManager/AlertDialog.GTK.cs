using Gdk;
using Gtk;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Platform
{
	public sealed class AlertDialog : MessageDialog
	{
		public bool RunAlertDialog(Gtk.Window parent_window, AlertArguments arguments)
		{
			//GtkDialogFlags flags = GTK_DIALOG_DESTROY_WITH_PARENT;
			var flags = Gtk.DialogFlags.DestroyWithParent;
			//dialog = gtk_message_dialog_new(parent_window,
			//								 flags,
			//								 GTK_MESSAGE_ERROR,
			//								 GTK_BUTTONS_CLOSE,
			//								 "Error reading “%s”: %s",
			//filename,
			//								 g_strerror(errno));
			int result = (int)Gtk.ResponseType.None;
			if (string.IsNullOrEmpty(arguments.Accept))
			{
				var dialog = new Gtk.MessageDialog(null, flags, MessageType.Other, ButtonsType.Ok, false, arguments.Message);
				//gtk_dialog_run(GTK_DIALOG(dialog));
				result = dialog.Run();
				//gtk_widget_destroy(dialog);
				dialog.Destroy();
			}
			else
			{
				var useCancel = arguments.Cancel.ToLower().Equals("cancel");
				if (arguments.Accept.ToLower().Equals("ok"))
				{
					if (useCancel)
					{
						var dialog = new Gtk.MessageDialog(null, flags, MessageType.Other, ButtonsType.OkCancel, false, arguments.Message);
						//gtk_dialog_run(GTK_DIALOG(dialog));
						result = dialog.Run();
						//gtk_widget_destroy(dialog);
						dialog.Destroy();
					}
					else
					{
						var dialog = new Gtk.MessageDialog(null, flags, MessageType.Other, ButtonsType.Ok, false, arguments.Message);
						//gtk_dialog_run(GTK_DIALOG(dialog));
						result = dialog.Run();
						//gtk_widget_destroy(dialog);
						dialog.Destroy();
					}
				}
				else if (arguments.Accept.ToLower().Equals("yes"))
				{
					var dialog = new Gtk.MessageDialog(parent_window, flags, MessageType.Other, ButtonsType.YesNo, false, arguments.Message);
					//gtk_dialog_run(GTK_DIALOG(dialog));
					result = dialog.Run();
					//gtk_widget_destroy(dialog);
					dialog.Destroy();
				}
			}


			if (!string.IsNullOrEmpty(arguments.Accept))
			{
				if (arguments.Accept.ToLower().Equals("ok"))
				{
					if (result.Equals((int)Gtk.ResponseType.Ok))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else if(arguments.Accept.ToLower().Equals("yes"))
				{
					if (result.Equals((int)Gtk.ResponseType.Yes))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}

			return false;
		}

		//protected override void OnApplyTemplate()
		//{
		//	base.OnApplyTemplate();

		//	// The child template name is derived from the default style
		//	// https://msdn.microsoft.com/en-us/library/windows/apps/mt299120.aspx
		//	var scrollName = "ContentScrollViewer";

		//	if (GetTemplateChild(scrollName) is ScrollViewer contentScrollViewer)
		//		contentScrollViewer.VerticalScrollBarVisibility = VerticalScrollBarVisibility;
		//}
	}
}