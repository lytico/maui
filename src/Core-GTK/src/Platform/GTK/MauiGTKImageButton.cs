using System;
using System.Reflection.Emit;
using Atk;
using Cairo;
using Gdk;
using GLib;
using Gtk;
using static System.Net.Mime.MediaTypeNames;

namespace Microsoft.Maui.Platform.GTK
{
	public class MauiGTKImageButton : Box
	{
		private Gtk.Box _imageBox = null!;
		private Gtk.EventBox _eventBox = null!;
		private string _resFileId = null!;

		public MauiGTKImageButton() : base(Gtk.Orientation.Horizontal, 0) {
			Initialize(string.Empty);
		}

		public MauiGTKImageButton(string resFileId) : base(Gtk.Orientation.Horizontal, 0) {
			Initialize(resFileId);
		}

		public MauiGTKImageButton(string resFileId, int width, int height) : base(Gtk.Orientation.Horizontal, 0) {
			this.WidthRequest = width;
			this.HeightRequest = height;

			Initialize(resFileId);
		}

		public Gdk.Color? BorderColor {  get; set; }
		public Gdk.Color? BackgroundColor { get; set; }
		public uint BorderWidthButton { get; set; } = 0;

		public string ImageSource {
			get {
				return _resFileId;
			}
			set {
				_resFileId = value;
				_image = new Gtk.Image(_resFileId);
				RecreateContainer();
			}
		}
		private Gtk.Image _image = null!;
		public Gtk.Image Image { 
			get { return _image; }
			set {
				_image = value;
				RecreateContainer();
			}
		}

		public event EventHandler Clicked = null!;

		private void Initialize(string resFileId) {
			_resFileId = resFileId;

			RecreateContainer();
		}

		private void MauiImageButton_ButtonPressEvent(object o, ButtonPressEventArgs args) {
			Clicked?.Invoke(this, args);
			QueueDraw();
		}

		private void RecreateContainer() {
			if (_imageBox != null) {
				if (_imageBox.Children.Length > 0) {
					if (_image != null) {
						_imageBox.RemoveFromContainer(_image);
						_image = null!;
					}
				}

				_eventBox.RemoveFromContainer(_imageBox);
				_imageBox.Dispose();
				_imageBox = null!;
			}

			if (_eventBox != null) {
				this.RemoveFromContainer(_eventBox);
				_eventBox.Dispose();
				_eventBox = null!;
			}

			_eventBox = new Gtk.EventBox();

			// EventWindow
			_eventBox.Events |= EventMask.ButtonPressMask;
			_eventBox.ButtonPressEvent += MauiImageButton_ButtonPressEvent;

			if (!string.IsNullOrEmpty(_resFileId)) {
				// We do have a label and an image.
				PackStart(_eventBox, true, true, 0);
			}

			_imageBox = new Gtk.Box(Gtk.Orientation.Horizontal, 0);
			_eventBox.Add(_imageBox);

			if (!string.IsNullOrEmpty(_resFileId)) {
				// We don't have a label but we do have an image
				PrivateRecreateImageContainer();
			}

			QueueDraw();
		}

		private void PrivateRecreateImageContainer() {
			var pixbuf = new Gdk.Pixbuf(_resFileId);

			if (this.WidthRequest > 0) {
				pixbuf = pixbuf.ScaleSimple(this.WidthRequest, this.HeightRequest, InterpType.Bilinear);
			}

			_image = new Gtk.Image(pixbuf);
			_imageBox.PackStart(_image, true, true, 0);
		}
	}
}
