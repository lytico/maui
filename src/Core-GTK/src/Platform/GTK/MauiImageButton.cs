using System;
using Cairo;
using Gdk;
using GLib;
using Gtk;
using Microsoft.Maui.Platform.GTK;

namespace Microsoft.Maui.Platform
{
	public sealed class MauiImageButton : MauiView
	{
		//private EventBox _container;
		private MauiGTKBox _imageAndLabelContainer = null!;

		//private Gdk.Color _defaultBorderColor;
		//private Gdk.Color _defaultBackgroundColor;
		//private Gdk.Color? _borderColor;
		//private Gdk.Color? _backgroundColor;

		//private MauiGTKButton _button;
		//private Gtk.Image _image = null!;
		private MauiGTKLabel _label;
		private uint _imageSpacing = 0;
		// private uint _borderWidth = 0;

		public MauiImageButton()
		{
			////_defaultBackgroundColor = Style.Backgrounds[(int)StateType.Normal];
			//_defaultBackgroundColor = new Gdk.Color(0, 0, 0);
			////_defaultBorderColor = Style.BaseColors[(int)StateType.Active];
			//_defaultBorderColor = new Gdk.Color(0, 0, 0);
			//_backgroundColor = _defaultBackgroundColor;
			//_borderColor = _defaultBorderColor;

			//_button = new MauiGTKButton();

			//_button.Relief = ReliefStyle.None;

			//_image = new Gtk.Image();
			_label = new MauiGTKLabel();
			//_container = new EventBox();

			//_button.Add(_container);

			//_container.HasWindow = false;
			//_container.AppPaintable = true;
			//_container.Drawn += new DrawnHandler(MauiImageButton_Drawn);

			this.HasWindow = false;
			this.AppPaintable = true;
			//this.Drawn += new DrawnHandler(MauiImageButton_Drawn);

			// Add(_button);
			//AddChildWidget(_button);

			ButtonPressEvent += MauiImageButton_ButtonPressEvent;

			RecreateContainer();
		}

		private void MauiImageButton_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			Clicked?.Invoke(this, args);
		}

		public event EventHandler Clicked = null!;

		//void Draw(Context cr)
		//{

		//}

		//private void MauiImageButton_Drawn(object o, DrawnArgs args)
		//{
		//	double colorMaxValue = 65535;
		//	if (o is MauiImageButton button)
		//	{
		//		////using (var cr = CairoHelper.Create(GdkWindow))
		//		////{

		//		Cairo.Context cr = args.Cr;

		//		cr.Rectangle(0, 0, Allocation.Width, Allocation.Height);

		//		// Draw BackgroundColor
		//		if (_backgroundColor.HasValue)
		//		{
		//			var color = _backgroundColor.Value;
		//			cr.SetSourceRGBA(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0);
		//			cr.FillPreserve();
		//		}

		//		// Draw BorderColor
		//		if (_borderColor.HasValue)
		//		{
		//			cr.LineWidth = _borderWidth;

		//			var color = _borderColor.Value;
		//			cr.LineWidth = 1;
		//			cr.SetSourceRGB(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue);
		//			// cr.Stroke();
		//			cr.StrokePreserve();
		//		}

		//		cr.GetTarget().Dispose();
		//		cr.Dispose();
		//	}
		//}

		public MauiGTKLabel LabelWidget => _label;

		public Gtk.Image ImageWidget => null!;

		public MauiGTKButton ButtonWidget => null!;

		public uint ImageSpacing
		{
			get
			{
				return _imageSpacing;
			}

			set
			{
				_imageSpacing = value;
				UpdateImageSpacing();
			}
		}

		public void SetBackgroundColor(Gdk.Color? color)
		{
			if (color != null)
			{
				//_backgroundColor = color;
				//_button.BackgroundColor = color;
				_label.BackgroundColor = color;
				if (_imageAndLabelContainer != null)
				{
					_imageAndLabelContainer.BackgroundColor = color;
				}
			}

			//	//QueueDraw();
			//	if (_backgroundColor != null)
			//	{
			//		var rgbaColor = new Gdk.RGBA();
			
			//		rgbaColor.Red = _backgroundColor.Value.Red;
			//		rgbaColor.Green = _backgroundColor.Value.Green;
			//		rgbaColor.Blue = _backgroundColor.Value.Blue;
			//		OverrideBackgroundColor(StateFlags.Normal, rgbaColor);
			//		OverrideBackgroundColor(StateFlags.Prelight, rgbaColor);
			//		OverrideBackgroundColor(StateFlags.Active, rgbaColor);
			//		//		ModifyBg(StateType.Normal, _backgroundColor.Value);
			//		//		ModifyBg(StateType.Prelight, _backgroundColor.Value);
			//		//		//ModifyBg(StateType.Selected, _backgroundColor.Value);
			//		//		ModifyBg(StateType.Active, _backgroundColor.Value);
			//		//		//ModifyBg(StateType.Insensitive, _backgroundColor.Value);
			//	}
			//}
		}

		public void ResetBackgroundColor()
		{
			//_backgroundColor = _defaultBackgroundColor;
			//QueueDraw();
		}

		public void SetForegroundColor(Gdk.Color color)
		{
			//_label.ModifyFg(StateType.Normal, color);
			//_label.ModifyFg(StateType.Prelight, color);
			//_label.ModifyFg(StateType.Active, color);
		}

		public void SetBorderWidth(uint width)
		{
			// _borderWidth = width;
			//_button.BorderWidthButton = width;
			_label.BorderWidthLabel = width;
			if (_imageAndLabelContainer != null)
			{
				_imageAndLabelContainer.BorderWidthBox = width;
			}
			//QueueDraw();
		}

		public void SetBorderColor(Gdk.Color? color)
		{
			// _borderColor = color;
			//_button.BorderColor = color;
			_label.BorderColor = color;
			if (_imageAndLabelContainer != null)
			{
				_imageAndLabelContainer.BorderColor = color;
			}
			//QueueDraw();
		}

		public void ResetBorderColor()
		{
			//_borderColor = _defaultBorderColor;
			//QueueDraw();
		}

		public void SetImagePosition(PositionType position)
		{
			//_button.ImagePosition = position;
			//RecreateContainer();
		}

		public override void Destroy()
		{
			base.Destroy();
			_label = null!;
			//_image = null!;
			_imageAndLabelContainer = null!;
			//_container = null!;
		}

		//protected override bool OnDrawn(Context cr)
		//{
		//	double colorMaxValue = 65535;

		//	cr.Rectangle(0, 0, Allocation.Width, Allocation.Height);

		//	// Draw BackgroundColor
		//	if (_backgroundColor.HasValue)
		//	{
		//		var color = _backgroundColor.Value;
		//		// Console.WriteLine("OnDrawn color: " + color.Red.ToString("D") + color.Green.ToString("D") + color.Blue.ToString("D"));
		//		cr.SetSourceRGBA(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0);
		//		cr.FillPreserve();
		//	}

		//	// Draw BorderColor
		//	if (_borderColor.HasValue)
		//	{
		//		var color = _borderColor.Value;
		//		cr.LineWidth = 1;
		//		cr.SetSourceRGB(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue);
		//		// cr.Stroke();
		//		cr.StrokePreserve();
		//	}

		//	//cr.GetTarget().Dispose();
		//	//cr.Dispose();

		//	return base.OnDrawn(cr);
		//}

		protected override bool OnDrawn(Context cr)
		{
			double colorMaxValue = 65535;

			cr.Rectangle(5, 5, Allocation.Width - 10, Allocation.Height - 10);

			// Draw BackgroundColor
			if (_label.BackgroundColor.HasValue)
			{
				var color = _label.BackgroundColor.Value;
				cr.SetSourceRGBA(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0);
				cr.FillPreserve();
			}

			// Draw BorderColor
			if (_label.BorderColor.HasValue)
			{
				cr.LineWidth = _label.BorderWidthLabel;
				double[] dashed1 = { 1.0 };
				// int len1 = dashed1.Length;

				var color = _label.BorderColor.Value;
				cr.SetDash(dashed1, 0);
				cr.SetSourceRGB(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue);
				//cr.LineWidth = 1.0;
				//BorderWidth = 1;
				cr.StrokePreserve();
			}

			return base.OnDrawn(cr);
			// return bReturn;
			// return true;
		}

		//protected override bool OnExposeEvent(EventExpose evnt)
		//{
		//	double colorMaxValue = 65535;

		//	using (var cr = CairoHelper.Create(GdkWindow))
		//	{
		//		cr.Rectangle(Allocation.Left, Allocation.Top, Allocation.Width, Allocation.Height);

		//		// Draw BackgroundColor
		//		if (_backgroundColor.HasValue)
		//		{
		//			var color = _backgroundColor.Value;
		//			cr.SetSourceRGBA(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0);
		//			cr.FillPreserve();
		//		}

		//		// Draw BorderColor
		//		if (_borderColor.HasValue)
		//		{
		//			cr.LineWidth = _borderWidth;

		//			var color = _borderColor.Value;
		//			cr.SetSourceRGB(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue);
		//			cr.Stroke();
		//		}
		//	}

		//	return base.OnExposeEvent(evnt);
		//}

		private void RecreateContainer()
		{
			if (_imageAndLabelContainer != null)
			{
				//_imageAndLabelContainer.RemoveFromContainer(_image);
				_imageAndLabelContainer.RemoveFromContainer(_label);
				this.RemoveFromContainer(_imageAndLabelContainer);
				//_button.RemoveFromContainer(_imageAndLabelContainer);
				// _container.RemoveFromContainer(_imageAndLabelContainer);
				_imageAndLabelContainer = null!;
			}

			_imageAndLabelContainer = new MauiGTKBox(Gtk.Orientation.Horizontal, 0);
			_imageAndLabelContainer.PackStart(_label, false, false, 0);

			//switch (_button.ImagePosition)
			//{
			//	case PositionType.Left:
			//		_imageAndLabelContainer = new MauiGTKBox(Gtk.Orientation.Horizontal, 0);
			//		//_imageAndLabelContainer.PackStart(_image, false, false, _imageSpacing);
			//		_imageAndLabelContainer.PackStart(_label, false, false, 0);
			//		break;
			//	case PositionType.Top:
			//		_imageAndLabelContainer = new MauiGTKBox(Gtk.Orientation.Vertical, 0);
			//		//_imageAndLabelContainer.PackStart(_image, false, false, _imageSpacing);
			//		_imageAndLabelContainer.PackStart(_label, false, false, 0);
			//		break;
			//	case PositionType.Right:
			//		_imageAndLabelContainer = new MauiGTKBox(Gtk.Orientation.Horizontal, 0);
			//		_imageAndLabelContainer.PackStart(_label, false, false, 0);
			//		//_imageAndLabelContainer.PackStart(_image, false, false, _imageSpacing);
			//		break;
			//	case PositionType.Bottom:
			//		_imageAndLabelContainer = new MauiGTKBox(Gtk.Orientation.Vertical, 0);
			//		_imageAndLabelContainer.PackStart(_label, false, false, 0);
			//		//_imageAndLabelContainer.PackStart(_image, false, false, _imageSpacing);
			//		break;
			//}

			if (_imageAndLabelContainer != null)
			{
				this.Add(_imageAndLabelContainer);
				this.ShowAll();
			}
		}

		private void UpdateImageSpacing()
		{
			//_imageAndLabelContainer.SetChildPacking(_image, false, false, _imageSpacing, PackType.Start);
		}
	}
}
