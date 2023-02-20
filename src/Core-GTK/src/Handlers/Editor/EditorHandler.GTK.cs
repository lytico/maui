using System;
using System.Runtime.InteropServices;

namespace Microsoft.Maui.Handlers
{
	// TODO: NET7 issoto - Change the TPlatformView generic type to MauiAppCompatEditText
	// This type adds support to the SelectionChanged event
	public partial class EditorHandler : ViewHandler<IEditor, ScrolledTextView>
	{
		bool _set;

		// TODO: NET7 issoto - Change the return type to MauiAppCompatEditText
		protected override ScrolledTextView CreatePlatformView(IView editor)
		{
			var editText = new ScrolledTextView();

			Gtk.Widget widget = editText;
			SetMargins(editor, ref widget);
			//editText.SetSingleLine(false);
			//editText.SetHorizontallyScrolling(false);
			_ignoreBufferWhileInputing = false;

			if (editor is IEditor editorView)
			{
				if (editorView.Visibility == Visibility.Visible)
				{
					editText.Show();
				}
			}

			return editText;
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			// TODO: NET7 issoto - Remove the casting once we can set the TPlatformView generic type as MauiAppCompatEditText
			if (!_set && PlatformView is ScrolledTextView editText)
				editText.SelectionReceived += EditText_SelectionReceived;
				// editText.SelectionChanged += OnSelectionChanged;

			_set = true;
		}

		private void EditText_SelectionReceived(object o, Gtk.SelectionReceivedArgs args)
		{
			//var cursorPostion = PlatformView.GetCursorPosition();
			//var selectedTextLength = PlatformView.GetSelectedTextLength();
			//var arg1 = args.SelectionData.Length;
			//var arg2 = args.SelectionData.Target.

			//if (VirtualView.CursorPosition != cursorPostion)
			//	VirtualView.CursorPosition = cursorPostion;

			//if (VirtualView.SelectionLength != selectedTextLength)
			//	VirtualView.SelectionLength = selectedTextLength;
		}

		// TODO: NET7 issoto - Change the platformView type to MauiAppCompatEditText
		protected override void ConnectHandler(ScrolledTextView platformView)
		{
			platformView.TextView.Buffer.Changed += Buffer_Changed;
			//platformView.ViewAttachedToWindow += OnPlatformViewAttachedToWindow;
			//platformView.TextChanged += OnTextChanged;
		}

		private void Buffer_Changed(object sender, EventArgs e)
		{
			if (VirtualView.Handler is IEditorHandler handler)
			{
				var ignore = handler._ignoreBufferWhileInputing;
				if (VirtualView.Text != PlatformView.TextView.Buffer.Text && !ignore)
				{
					VirtualView.Text = PlatformView.TextView.Buffer.Text;
				}
			}
		}

		// TODO: NET7 issoto - Change the platformView type to MauiAppCompatEditText
		protected override void DisconnectHandler(ScrolledTextView platformView)
		{
			//platformView.ViewAttachedToWindow -= OnPlatformViewAttachedToWindow;
			//platformView.TextChanged -= OnTextChanged;
			platformView.TextView.Buffer.Changed -= Buffer_Changed;

			//// TODO: NET7 issoto - Remove the casting once we can set the TPlatformView generic type as MauiAppCompatEditText
			//if (_set && platformView is ScrolledTextView editText)
			//	editText.SelectionChanged -= OnSelectionChanged;

			//_set = false;
		}

		public static void MapBackground(IEditorHandler handler, IEditor editor) { }
		// handler.PlatformView?.UpdateBackground(editor);

		public static void MapText(IEditorHandler handler, IEditor editor)
		{
			if (handler.PlatformView is ScrolledTextView platformTextView)
			{
				// platformTextView.UpdateText(editor);
				// handler.PlatformView?.UpdateText(editor);
				if (!handler._ignoreBufferWhileInputing)
				{
					handler._ignoreBufferWhileInputing = true;

					// await System.Threading.Tasks.Task.Delay(500);
					// var hasFocus = platformTextView.TextView.HasFocus;
					// var hasFocus = platformControl.FocusState != UI.Xaml.FocusState.Unfocused;
					// var passwordBox = platformControl as MauiPasswordTextBox;
					// var isPassword = passwordBox?.IsPassword ?? false;
					//var textTransform = editor?.TextTransform ?? TextTransform.None;

					// Setting the text causes the cursor to be reset to position zero.
					// So, let's retain the current cursor position and calculate a new cursor
					// position if the text was modified by a Converter.
					// var oldText = platformControl.TextView.Text ?? string.Empty;
					Gtk.TextIter start, end, newEndIter;
					platformTextView.TextView.Buffer.GetBounds(out start, out end);
					var oldText = platformTextView.TextView.Buffer.GetText(start, end, false);
					if (editor.Text == oldText)
					{
						handler._ignoreBufferWhileInputing = false;
						return;
					}
					//var newText = TextTransformUtilites.GetTransformedText(
					//	inputView?.Text,
					//	isPassword ? TextTransform.None : textTransform
					//	);

					//var newText = TextTransformUtilites.GetTransformedText(
					//	editor?.Text,
					//	textTransform
					//	);

					var newText = string.Empty;
					if (editor != null)
					{
						newText = editor.Text;
					}

					// platformTextView.TextView.Buffer.Text = newText;

					// Re-calculate the cursor offset position if the text was modified by a Converter.
					// but if the text is being set by code, let's just move the cursor to the end.
					//var cursorOffset = newText.Length - oldText.Length;
					// int cursorPosition = hasFocus ? platformControl.GetCursorPosition(cursorOffset) : newText.Length;
					// platformTextView.TextView.Buffer.Insert(start, newText);

					//DispatchData data = new DispatchData();
					//data.buffer = platformTextView.TextView.Buffer;
					//data.output_str = newText;

					GLib.Idle.Add(() =>
					{
						System.Threading.Thread.CurrentThread.IsBackground = true;

						//Gdk.Threads.AddIdle(1, display_status_textbuffer, data);
						if (!string.IsNullOrEmpty(newText))
						{
							platformTextView.TextView.Buffer.Text = newText;
						}

						platformTextView.TextView.Buffer.GetBounds(out start, out newEndIter);

						////if (oldText != newText && passwordBox is not null)
						////	passwordBox.Password = newText;
						////else if (oldText != newText)
						////	platformControl.Text = newText;

						////// platformControl.Select(cursorPosition, 0);
						platformTextView.TextView.ScrollToIter(newEndIter, 0, false, 0, 0);
						handler._ignoreBufferWhileInputing = false;

						return false;
					});
				}
			}
		}

		//private static void display_status_textbuffer(DispatchData data)
		//{

		//}

		//struct DispatchData
		//{
		//	public Gtk.TextBuffer buffer;
		//	public string output_str;
		//};

		public static void MapTextColor(IEditorHandler handler, IEditor editor) { }
		// handler.PlatformView?.UpdateTextColor(editor);

		public static void MapPlaceholder(IEditorHandler handler, IEditor editor) { }
		// handler.PlatformView?.UpdatePlaceholder(editor);

		public static void MapPlaceholderColor(IEditorHandler handler, IEditor editor)
		{
			//if (handler is EditorHandler platformHandler)
			//	handler.PlatformView?.UpdatePlaceholderColor(editor);
		}

		public static void MapCharacterSpacing(IEditorHandler handler, IEditor editor) { }
		// handler.PlatformView?.UpdateCharacterSpacing(editor);

		public static void MapMaxLength(IEditorHandler handler, IEditor editor) { }
		// handler.PlatformView?.UpdateMaxLength(editor);

		public static void MapIsReadOnly(IEditorHandler handler, IEditor editor) { }
		// handler.PlatformView?.UpdateIsReadOnly(editor);

		public static void MapIsTextPredictionEnabled(IEditorHandler handler, IEditor editor) { }
		// handler.PlatformView?.UpdateIsTextPredictionEnabled(editor);

		public static void MapFont(IEditorHandler handler, IEditor editor) { }
		// handler.PlatformView?.UpdateFont(editor, handler.GetRequiredService<IFontManager>());

		public static void MapHorizontalTextAlignment(IEditorHandler handler, IEditor editor) { }
		// handler.PlatformView?.UpdateHorizontalTextAlignment(editor);

		public static void MapVerticalTextAlignment(IEditorHandler handler, IEditor editor) { }
		// handler.PlatformView?.UpdateVerticalTextAlignment(editor);

		public static void MapKeyboard(IEditorHandler handler, IEditor editor) { }
		// handler.PlatformView?.UpdateKeyboard(editor);

		public static void MapCursorPosition(IEditorHandler handler, ITextInput editor) { }
		// handler.PlatformView?.UpdateCursorPosition(editor);

		public static void MapSelectionLength(IEditorHandler handler, ITextInput editor) { }
		// handler.PlatformView?.UpdateSelectionLength(editor);

		//void OnPlatformViewAttachedToWindow(object? sender, ViewAttachedToWindowEventArgs e)
		//{
		//	if (PlatformView.IsAlive() && PlatformView.Enabled)
		//	{
		//		// https://issuetracker.google.com/issues/37095917
		//		PlatformView.Enabled = false;
		//		PlatformView.Enabled = true;
		//	}
		//}

		//void OnTextChanged(object? sender, Android.Text.TextChangedEventArgs e) =>
		//	VirtualView?.UpdateText(e);

		//private void OnSelectionChanged(object? sender, EventArgs e)
		//{
		//	var cursorPostion = PlatformView.GetCursorPosition();
		//	var selectedTextLength = PlatformView.GetSelectedTextLength();

		//	if (VirtualView.CursorPosition != cursorPostion)
		//		VirtualView.CursorPosition = cursorPostion;

		//	if (VirtualView.SelectionLength != selectedTextLength)
		//		VirtualView.SelectionLength = selectedTextLength;
		//}
	}
}