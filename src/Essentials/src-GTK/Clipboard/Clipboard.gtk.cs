#nullable enable
using System;
using System.Threading.Tasks;

namespace Microsoft.Maui.ApplicationModel.DataTransfer
{
	partial class ClipboardImplementation : IClipboard
	{
		public Task SetTextAsync(string? text)
		{
			var clipboard = Gtk.Clipboard.Get(null);
			if (clipboard != null)
			{
				clipboard.Text = text;
			}
			return Task.CompletedTask;
		}

		public bool HasText
			=> true;

		public Task<string?> GetTextAsync()
		{
			return Task.FromResult<string?>(null);
		}

		void StartClipboardListeners()
		{
		}

		void StopClipboardListeners()
		{
		}

		public void ClipboardChangedEventListener(object? sender, object val) => OnClipboardContentChanged();
	}
}
