using Gdk;
using Gtk;
using System;
using System.Maui.Platform.GTK.Extensions;

namespace System.Maui.Platform.GTK.Controls
{
	public class Page : Table
	{
		private Gdk.Rectangle _lastAllocation = Gdk.Rectangle.Zero;
		private GtkFormsContainer _headerContainer;
		private GtkFormsContainer _contentContainerWrapper;
		private Fixed _contentContainer;
		private HBox _toolbar;
		private GtkFormsContainer _content;
		private ImageControl _image;
		private Gdk.Color _defaultBackgroundColor;

		public HBox Toolbar
		{
			get
			{
				return _toolbar;
			}
			set
			{
				if (_toolbar != value)
				{
					RefreshToolbar(value);
				}
			}
		}

		public GtkFormsContainer Content
		{
			get
			{
				return _content;
			}
			set
			{
				if (_content != value)
				{
					RefreshContent(value);
				}
			}
		}

		public Page() : base(1, 1, true)
		{
			BuildPage();
		}

		public void SetToolbarColor(Color backgroundColor)
		{
			_headerContainer.SetBackgroundColor(backgroundColor);
		}

		public void SetBackgroundColor(Color backgroundColor)
		{
			_contentContainerWrapper.SetBackgroundColor(backgroundColor);
		}

		public async void SetBackgroundImage(ImageSource imageSource)
		{
			_image.Pixbuf = await imageSource.GetNativeImageAsync();
		}

		protected override void Dispose(bool disposing)
		{

			if (_contentContainerWrapper != null)
			{
				_contentContainerWrapper.SizeAllocated -= OnContentContainerWrapperSizeAllocated;
				_contentContainerWrapper = null;
			}
			_contentContainer = null;
			_image = null;
			_toolbar = null;
			_content = null;
			_headerContainer = null;
			base.Dispose(disposing);
		}

		private void BuildPage()
		{
			_defaultBackgroundColor = this.BackgroundColor(StateType.Normal);

			_toolbar = new HBox();
			_content = new GtkFormsContainer();

			var root = new VBox(false, 0);

			_headerContainer = new GtkFormsContainer();
			root.PackStart(_headerContainer, false, false, 0);

			_image = new ImageControl();
			_image.Aspect = ImageAspect.Fill;

			_contentContainerWrapper = new GtkFormsContainer();
			_contentContainerWrapper.SizeAllocated += OnContentContainerWrapperSizeAllocated;
			_contentContainer = new Fixed();
			_contentContainer.Add(_image);
			_contentContainerWrapper.Add(_contentContainer);

			root.PackStart(_contentContainerWrapper, true, true, 0); // Should fill all available space

			Attach(root, 0, 1, 0, 1);

			ShowAll();
		}

		private void RefreshToolbar(HBox newToolbar)
		{
			_toolbar.Dispose();
			_toolbar = newToolbar;
			_headerContainer.Add(_toolbar);
			_toolbar.ShowAll();
		}

		private void RefreshContent(GtkFormsContainer newContent)
		{
			_content.Dispose();
			_content = newContent;
			_contentContainer.Add(_content);
			_content.ShowAll();
		}

		private void OnContentContainerWrapperSizeAllocated(object o, SizeAllocatedArgs args)
		{
			_image.SetSizeRequest(args.Allocation.Width, args.Allocation.Height);
		}
	}
}
