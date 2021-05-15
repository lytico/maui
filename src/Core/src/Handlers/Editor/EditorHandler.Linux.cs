using System;
using Gtk;

namespace Microsoft.Maui.Handlers
{

	public partial class EditorHandler : ViewHandler<IEditor, TextView>
	{

		protected override TextView CreateNativeView()
		{
			return new() { WrapMode = WrapMode.WordChar };
		}

		protected override void ConnectHandler(TextView nativeView)
		{
			nativeView.Buffer.Changed += OnNativeViewChanged;
		}

		protected override void DisconnectHandler(TextView nativeView)
		{
			nativeView.Buffer.Changed -= OnNativeViewChanged;
		}

		protected void OnNativeViewChanged(object sender, EventArgs e)
		{
			if (sender != NativeView?.Buffer || VirtualView == null)
				return;

			var text = NativeView.Buffer.Text;

			if (VirtualView.Text != text)
				VirtualView.Text = text;
		}

		public static void MapText(EditorHandler handler, IEditor editor)
		{
			handler.NativeView?.UpdateText(editor);
		}

		public static void MapFont(EditorHandler handler, IEditor editor)
		{
			handler.MapFont(editor);
		}

		public static void MapIsReadOnly(EditorHandler handler, IEditor editor)
		{
			if (handler.NativeView is { } nativeView)
				nativeView.Editable = !editor.IsReadOnly;
		}

		public static void MapTextColor(EditorHandler handler, IEditor editor)
		{
			handler.NativeView?.UpdateTextColor(editor.TextColor);
		}

		[MissingMapper]
		public static void MapPlaceholder(EditorHandler handler, IEditor editor) { }

		[MissingMapper]
		public static void MapPlaceholderColor(EditorHandler handler, IEditor editor) { }

		[MissingMapper]
		public static void MapCharacterSpacing(EditorHandler handler, IEditor editor) { }

		[MissingMapper]
		public static void MapMaxLength(EditorHandler handler, IEditor editor) { }

		[MissingMapper]
		public static void MapIsTextPredictionEnabled(EditorHandler handler, IEditor editor) { }

	}

}