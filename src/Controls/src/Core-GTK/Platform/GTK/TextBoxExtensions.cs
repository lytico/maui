#nullable enable
using GLib;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Platform;

namespace Microsoft.Maui.Controls.Platform
{
	internal static class TextBoxExtensions
	{
		public static void UpdateText(this ScrolledTextView platformControl, IEditor editor)
		{
			var hasFocus = platformControl.TextView.HasFocus;
			// var hasFocus = platformControl.FocusState != UI.Xaml.FocusState.Unfocused;
			// var passwordBox = platformControl as MauiPasswordTextBox;
			// var isPassword = passwordBox?.IsPassword ?? false;
			//var textTransform = editor?.TextTransform ?? TextTransform.None;

			// Setting the text causes the cursor to be reset to position zero.
			// So, let's retain the current cursor position and calculate a new cursor
			// position if the text was modified by a Converter.
			// var oldText = platformControl.TextView.Text ?? string.Empty;
			Gtk.TextIter start, end, newEndIter;
			platformControl.TextView.Buffer.GetBounds(out start, out end);
			var oldText = platformControl.TextView.Buffer.GetText(start, end, false);
			//var newText = TextTransformUtilites.GetTransformedText(
			//	inputView?.Text,
			//	isPassword ? TextTransform.None : textTransform
			//	);

			//var newText = TextTransformUtilites.GetTransformedText(
			//	editor?.Text,
			//	textTransform
			//	);

			var newText = editor?.Text;

			// Re-calculate the cursor offset position if the text was modified by a Converter.
			// but if the text is being set by code, let's just move the cursor to the end.
			//var cursorOffset = newText.Length - oldText.Length;
			// int cursorPosition = hasFocus ? platformControl.GetCursorPosition(cursorOffset) : newText.Length;
			//platformControl.TextView.Buffer.Insert(end, newText);
			platformControl.TextView.Buffer.Text = newText;

			platformControl.TextView.Buffer.GetBounds(out start, out newEndIter);

			//if (oldText != newText && passwordBox is not null)
			//	passwordBox.Password = newText;
			//else if (oldText != newText)
			//	platformControl.Text = newText;

			// platformControl.Select(cursorPosition, 0);
			platformControl.TextView.ScrollToIter(newEndIter, 0, false, 0, 0);
		}
	}

}
