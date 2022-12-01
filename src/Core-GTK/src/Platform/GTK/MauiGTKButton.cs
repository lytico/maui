using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Atk;
using Cairo;
using Gdk;
using GLib;
using Gtk;
using Microsoft.Maui.Hosting;
using static System.Net.Mime.MediaTypeNames;

namespace Microsoft.Maui.Platform.GTK
{
	public class MauiGTKButton : Box
	{
		private enum DrawType {
			LABELONLY,
			LABELANDIMAGE,
			IMAGEONLY
		};

		private double colorMaxValue = 65535;
		private Gdk.Color _defaultBorderColor;
		private Gdk.Color _defaultBackgroundColor;
		private Gtk.Label _localLabel = null!;
		private Gtk.Box _imageAndLabelBox = null!;
		private Gtk.EventBox _eventBox = null!;
		private string _resFileId = null!;
		private int _fontSize = 16;

		public MauiGTKButton() : base(Gtk.Orientation.Horizontal, 0) {
			Initialize(string.Empty, string.Empty);
		}

		public MauiGTKButton(string label, string resFileId) : base(Gtk.Orientation.Horizontal, 0) {
			Initialize(label, resFileId);
		}

		public MauiGTKButton(IView button) : base(Gtk.Orientation.Horizontal, 0)
		{
			if ((button is ITextButton virtualTextButton) && (button is IImageButton virtualImageButton))
			{
				_fontSize = Convert.ToInt32(virtualTextButton.Font.Size);
				if (virtualTextButton.TextColor != null)
				{
					var red = Convert.ToByte(virtualTextButton.TextColor.Red * 255);
					var green = Convert.ToByte(virtualTextButton.TextColor.Green * 255);
					var blue = Convert.ToByte(virtualTextButton.TextColor.Blue * 255);
					FontColor = new Gdk.Color(red, green, blue);
				}
				else if (virtualTextButton.StrokeColor != null)
				{
					var red = Convert.ToByte(virtualTextButton.StrokeColor.Red * 255);
					var green = Convert.ToByte(virtualTextButton.StrokeColor.Green * 255);
					var blue = Convert.ToByte(virtualTextButton.StrokeColor.Blue * 255);
					FontColor = new Gdk.Color(red, green, blue);
				}

				if (virtualImageButton.Source != null)
				{
					var fileImageSource = (IFileImageSource)virtualImageButton.Source;

					if (fileImageSource != null)
					{
						// Console.WriteLine("Image: " + fileImageSource.File);
						if (string.IsNullOrEmpty(virtualTextButton.Text))
						{
							Initialize(string.Empty, fileImageSource.File);
						}
						else
						{
							Initialize(virtualTextButton.Text, fileImageSource.File);
						}
						return;
					}
				}
				Initialize(virtualTextButton.Text, string.Empty);

				return;
			}
			Initialize(string.Empty, string.Empty);
		}

		public Gdk.Color? BorderColor {  get; set; }
		public Gdk.Color? BackgroundColor { get; set; }
		public uint BorderWidthButton { get; set; } = 0;

		private Gtk.Image _image = null!;
		private Gtk.Image _newImage = null!;
		public Gtk.Image Image { 
			get { return _image; }
			set {
				_newImage = value;
				if (_newImage == null)
				{
					_resFileId = null!;
				}
				else
				{
					_resFileId = _newImage.File;
				}
				//_image = value;
				//if (_image == null)
				//{
				//	_resFileId = null!;
				//}
				RecreateContainer();
			}
		}

		public int FontSize {
			get { return _fontSize; }
			set {
				_fontSize = value;
			}
		}

		private string _label = null!;
		public string Label { 
			get { return _label; }
			set {
				_label = value;
				if (_localLabel == null && !string.IsNullOrEmpty(_label))
				{
					RecreateContainer();
				}
				UpdateText(_label);
			}
		}

		public Gdk.Color FontColor { get; set; }
		public string FontFamily { get; set; } = null!;

		public Gtk.Label GetInternalLabel { 
			get
			{
				return _localLabel;
			}
		}

		//bool _offsetXSet;
		private uint _offsetX;
		//private double OffsetX {
		//	get { return _offsetX; }
		//	set {
		//		_offsetX = value;
		//		UpdateOffsetX();
		//	}
		//}

		public event EventHandler Clicked = null!;

		public void SetBackgroundColor(Gdk.Color backgroundColor)
		{
			BackgroundColor = backgroundColor;
		}

		public void UpdateText(string text)
		{
			this._label = text;
			if (this._localLabel != null)
			{
				var markup = MauiGTKText.GetUpdateText(text, _fontSize, FontColor, FontFamily);
				//var font_Size = (int)(_fontSize * Pango.Scale.PangoScale);
				//var fontColor = "#";
				//// color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0
				//var red = (ushort)(FontColor.Red / colorMaxValue) * 0xFF;
				//var green = (ushort)(FontColor.Green / colorMaxValue) * 0xFF;
				//var blue = (ushort)(FontColor.Blue / colorMaxValue) * 0xFF;
				//fontColor += red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
				//var markup = "<span foreground='";
				//markup += fontColor;
				//markup += "' size='";
				//markup += font_Size.ToString("D");
				//markup += "'>";
				//markup += this.Label;
				//markup += "</span>";
				this._localLabel.Markup = markup;
			}
		}

		private void Initialize(string label, string resFileId) {
			_defaultBackgroundColor = new Gdk.Color(0x56, 0x2F, 0xD9); // #562FD9
			_defaultBorderColor = new Gdk.Color(0x26, 0x0F, 0xC9);
			//_defaultBackgroundColor = new Gdk.Color(0x80, 0x80, 0x80); // #562FD9
			//_defaultBorderColor = new Gdk.Color(0x80, 0x80, 0x80);
			BackgroundColor = _defaultBackgroundColor;
			BorderColor = _defaultBorderColor;
			//_offsetXSet = false;
			//_offsetX = 0;

			FontSize = 24;
			FontColor = new Gdk.Color(0xFF, 0xFF, 0xFF);

			_resFileId = resFileId;
			_label = label;

			HasWindow = false;
			AppPaintable = true;

			//_eventBox = new Gtk.EventBox();

			//// EventWindow
			//_eventBox.Events |= EventMask.ButtonPressMask;
			//_eventBox.ButtonPressEvent += MauiImageButton_ButtonPressEvent;

			////_localLabel = new Gtk.Label(Label);
			////if (!string.IsNullOrEmpty(_resFileId)) {
			////	Image = new Gtk.Image(_resFileId);
			////	_imageEventBox.Add(_image);
			////}
			////_eventBox.Add(_localLabel);
			//PackStart(_eventBox, false, false, 20);
			////PackStart(_imageEventBox, false, false, 0);

			RecreateContainer();
		}

		private void MauiImageButton_ButtonPressEvent(object o, ButtonPressEventArgs args) {
			Clicked?.Invoke(this, args);
			QueueDraw();
		}

		private void CalculateOffset() {
			var testLabel = new Gtk.Label("");

			//var font_Size = (int)(_fontSize * Pango.Scale.PangoScale);
			//var fontColor = "#";
			//// color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0
			//var red = (ushort)(FontColor.Red / colorMaxValue) * 0xFF;
			//var green = (ushort)(FontColor.Green / colorMaxValue) * 0xFF;
			//var blue = (ushort)(FontColor.Blue / colorMaxValue) * 0xFF;
			//fontColor += red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
			//var s1 = "<span foreground='";
			//s1 += fontColor;
			//s1 += "' size='";
			//s1 += font_Size.ToString("D");
			//s1 += "'>hello</span>";

			var markup = MauiGTKText.GetUpdateText("hello", _fontSize, FontColor, FontFamily);

			//var s1 = "<span font-size='";
			//s1 += _fontSize.ToString("D");
			//s1 += "'>hello</span>";
			testLabel.Markup = markup;
			//var s2 = "@@@@@";
			//var s3 = "String pixel";
			// var layout = testLabel.CreatePangoLayout(s1);
			var layout = testLabel.Layout;
			//layout = gtk_widget_create_pango_layout(label, s1);
			layout.GetPixelSize(out int testWidth, out int testHeight);
			////pango_layout_get_pixel_size(layout, &width, NULL);
			//Console.WriteLine(s1 + " pixel size(OnDraw): " + testWidth.ToString("D") + " and height: " + testHeight.ToString("D"));
			////g_print("%s pixel size is: %d/n", s1, width);
			//layout.SetText(s2);
			//layout.GetPixelSize(out int testWidth2, out int testHeight2);
			////pango_layout_set_text(layout, s2, -1);
			////pango_layout_get_pixel_size(layout, &width, NULL);
			//Console.WriteLine(s2 + " pixel size(OnDraw): " + testWidth2.ToString("D") + " and height: " + testHeight2.ToString("D"));
			////g_print("%s pixel size is: %d/n", s2, width);
			//layout.SetText(s3);
			//layout.GetPixelSize(out int testWidth3, out int testHeight3);
			////pango_layout_set_text(layout, s3, -1);
			////pango_layout_get_pixel_size(layout, &width, NULL);
			////g_print("%s pixel size is: %d/n", s3, width);
			//Console.WriteLine(s3 + " pixel size(OnDraw): " + testWidth3.ToString("D") + " and height: " + testHeight3.ToString("D"));
			layout = null;
			testLabel = null;

			_offsetX = Convert.ToUInt32(testHeight / 4);
		}

		private void RecreateContainer() {
			if (_imageAndLabelBox != null) {
				if (_image != null) {
					_imageAndLabelBox.RemoveFromContainer(_image);
					_image.Dispose();
					_image = null!;
					// _image = _newImage;
				}
				if (_localLabel != null) {
					_imageAndLabelBox.RemoveFromContainer(_localLabel);
					_localLabel.Dispose();
					_localLabel = null!;
				}
				_eventBox.RemoveFromContainer(_imageAndLabelBox);
				_imageAndLabelBox.Dispose();
				_imageAndLabelBox = null!;
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

			CalculateOffset();

			if (string.IsNullOrEmpty(_label) && !string.IsNullOrEmpty(_resFileId)) {
				// We don't have a label but we do have an image
				PackStart(_eventBox, false, false, 0);
			} else if (!string.IsNullOrEmpty(_label)) {  //} && string.IsNullOrEmpty(_resFileId)) {
														 // We do have a label and no image.
														 //var pangoLayout = _localLabel.Layout;
														 //pangoLayout.GetPixelSize(out int pangoPixelWidth, out int pangoPixelHeight);
														 //_localLabel.GetLayoutOffsets(out int labelX, out int labelY);

				//var localOffsetXDouble = pangoPixelHeight / 3;
				//var localOffsetX = Convert.ToUInt32(localOffsetXDouble);
				//_offsetX = Convert.ToUInt32(textHeight / 3);
				//_eventBox.SetSizeRequest(500, 45);
				PackStart(_eventBox, false, false, _offsetX);
			//} else if (!string.IsNullOrEmpty(Label) && !string.IsNullOrEmpty(_resFileId)) {
			//	// We do have a label and an image.
			//	PackStart(_eventBox, false, false, _offsetX);
			}

			_imageAndLabelBox = new Gtk.Box(Gtk.Orientation.Horizontal, 0);
			_eventBox.Add(_imageAndLabelBox);

			if (string.IsNullOrEmpty(_label) && !string.IsNullOrEmpty(_resFileId)) {
				// We don't have a label but we do have an image
				PrivateRecreateImageContainer();
			} else if (!string.IsNullOrEmpty(_label) && string.IsNullOrEmpty(_resFileId)) {
				// We do have a label and no image.
				PrivateRecreateLabelContainer(false);
			} else if (!string.IsNullOrEmpty(Label) && !string.IsNullOrEmpty(_resFileId)) {
				// We do have a label and an image.
				PrivateRecreateImageLabelContainer();
			}

			UpdateText(this.Label);
		}

		private void PrivateRecreateImageLabelContainer() {
			PrivateRecreateImageContainer();
			PrivateRecreateLabelContainer(true);
		}

		private void PrivateRecreateImageContainer() {
			_image = new Gtk.Image(_resFileId);
			_imageAndLabelBox.PackStart(_image, false, false, 0);
		}

		private void PrivateRecreateLabelContainer(bool useOffset) {
			if (_localLabel == null) {
				_localLabel = new Gtk.Label(Label);
			}

			if (_localLabel != null) {
				//_localLabel.SetSizeRequest(250, 38);
				if (useOffset) {
					_imageAndLabelBox.PackStart(_localLabel, false, false, _offsetX);
				} else {
					_imageAndLabelBox.PackStart(_localLabel, false, false, 0);
				}
			}
		}

		//private void UpdateOffsetX() {
		//	if (!_offsetXSet) {
		//		if (!string.IsNullOrEmpty(_label) && string.IsNullOrEmpty(_resFileId)) {
		//			// We do have a label and no image.
		//			RecreateContainer();
		//		}
		//	}
		//}

		protected override bool OnDrawn(Context cr)
		{
			DrawType drawType = DrawType.LABELONLY;
			if (string.IsNullOrEmpty(this.Label)) {
				if (!string.IsNullOrEmpty(_resFileId)) {
					drawType = DrawType.IMAGEONLY;
				}
			}

			if (!string.IsNullOrEmpty(Label) && !string.IsNullOrEmpty(_resFileId)) {
				// We do have a label and an image.
				drawType = DrawType.LABELANDIMAGE;
			}

			int pangoPixelWidth = 0;
			int pangoPixelHeight = 0;
			int pangoImagePixelWidth = 0;
			int pangoImagePixelHeight = 0;
			int labelX = 0;
			int labelY = 0;

			if (_image != null && _image.Pixbuf != null) {
				pangoImagePixelWidth = _image.Pixbuf.Width;
				pangoImagePixelHeight = _image.Pixbuf.Height;
				//labelX = 0;
				//labelY = 0;
			}

			if (drawType == DrawType.IMAGEONLY) {
				if (pangoImagePixelHeight >= (_offsetX * 4)) {
					return base.OnDrawn(cr);
				}
			}

			//if (_localLabel == null) {
			//	if (_image == null) {
			//		return base.OnDrawn(cr);
			//	} else {
			//		pangoPixelWidth = _image.Pixbuf.Width;
			//		pangoPixelHeight = _image.Pixbuf.Height;
			//		labelX = 0;
			//		labelY = 0;
			//	}
			//} else {

			if (drawType != DrawType.IMAGEONLY && _localLabel != null) {
				var pangoLayout = _localLabel.Layout;
				pangoLayout.GetPixelSize(out pangoPixelWidth, out pangoPixelHeight);
				_localLabel.GetLayoutOffsets(out labelX, out labelY);
			}

			// labelX = labelX - pangoImagePixelWidth;
			//if (drawType == DrawType.LABELANDIMAGE) {
			//	pangoPixelWidth = pangoPixelWidth + pangoImagePixelWidth;
			//}

			//}

			//double offsetX = pangoPixelHeight / 3;

			//if (!_offsetXSet) {
			//	_offsetXSet = true;
			//	return base.OnDrawn(cr);
			//}

			// double offsetY = offsetX + (pangoPixelHeight / 8);

			// BorderWidth = Convert.ToUInt32(offsetY);

			double x = labelX - _offsetX;
			if (drawType == DrawType.LABELANDIMAGE) {
				x = labelX - _offsetX - pangoImagePixelWidth;
			}

			if (x < 0) {
				x = 0;
			}
			double y = labelY;

			if (y < 0)
			{
				y = 0;
			}

			// double width = pangoPixelWidth + (2 * offsetX);
			double width = pangoPixelWidth + (2 * _offsetX);

			if (drawType == DrawType.LABELANDIMAGE) {
				width = pangoPixelWidth + (4 * _offsetX) + pangoImagePixelWidth;
			} else if (drawType == DrawType.IMAGEONLY) {
				width = pangoImagePixelWidth;
			}

			//if (drawType == DrawType.LABELONLY) {
			//	width = pangoPixelWidth + (2 * _offsetX);
			//}

			// double height = pangoPixelHeight + (2 * offsetY);
			double height = pangoPixelHeight;
			if (drawType == DrawType.IMAGEONLY) {
				height = pangoImagePixelHeight;
			}

			//if (drawType == DrawType.LABELANDIMAGE) {
			//	width = pangoPixelWidth + (2 * offsetX);
			//}

			double radius = 5.0;
			double degrees = Math.PI / 180.0;

			cr.NewSubPath();
			cr.Arc(x + width - radius, y + radius, radius, -90 * degrees, 0 * degrees);
			cr.Arc(x + width - radius, y + height - radius, radius, 0 * degrees, 90 * degrees);
			cr.Arc(x + radius, y + height - radius, radius, 90 * degrees, 180 * degrees);
			cr.Arc(x + radius, y + radius, radius, 180 * degrees, 270 * degrees);
			cr.ClosePath();

			// Draw BackgroundColor
			if (BackgroundColor.HasValue)
			{
				var color = BackgroundColor.Value;
				cr.SetSourceRGBA(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0);
				cr.FillPreserve();
			}

			// Draw BorderColor
			if (BorderColor.HasValue && BorderWidthButton > 0)
			{
				var color = BorderColor.Value;
				cr.LineWidth = BorderWidthButton;
				cr.SetSourceRGB(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue);
				cr.StrokePreserve();
			}

			return base.OnDrawn(cr);
		}
	}
}
