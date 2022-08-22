using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Gtk;
// using GTK.Primitives;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Compatibility.Platform.GTK.Extensions;
using Container = Microsoft.Maui.Controls.Compatibility.Platform.GTK.GtkFormsContainer;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Microsoft.Maui.Controls.Compatibility.Platform.GTK.Renderers
{
	public abstract class AbstractPageRenderer<TWidget, TPage> : Container, IPageControl, IVisualElementRenderer, IEffectControlProvider
		where TWidget : Widget
		where TPage : Page
	{
		private Gdk.Rectangle _lastAllocation;
		private DateTime _lastAllocationTime;
		protected bool _disposed;
		protected bool _appeared;
		protected readonly PropertyChangedEventHandler _propertyChangedHandler;

		protected AbstractPageRenderer()
		{
			VisibleWindow = true;
			_propertyChangedHandler = OnElementPropertyChanged;
		}

		public Controls.Page Control { get; protected set; } = null!;

		public TWidget? Widget { get; protected set; }

		public VisualElement Element { get; protected set; } = null!;

		public TPage? Page => Element as TPage;

		public bool Disposed { get { return _disposed; } }

		public Container Container => this;

		public event EventHandler<VisualElementChangedEventArgs>? ElementChanged;

		protected IElementController? ElementController => Element as IElementController;

		protected IPageController? PageController => Element as IPageController;

		void IEffectControlProvider.RegisterEffect(Effect effect)
		{
			var platformEffect = effect as PlatformEffect;
			//if (platformEffect != null)
			//	platformEffect.SetContainer(Container);

			if (platformEffect == null || Container == null || Control == null || Control.Content == null)
			{
				return;
			}

			platformEffect.Container = Container;
			platformEffect.Control = Control.Content;
		}

		public virtual void SetElement(VisualElement element)
		{
			VisualElement oldElement = Element;
			Element = element;

			OnElementChanged(new VisualElementChangedEventArgs(oldElement, element));

			EffectUtilities.RegisterEffectControlProvider(this, oldElement, element);
		}

		public virtual void SetElementSize(Graphics.Size newSize)
		{
			if (Element == null)
				return;

			var elementSize = new Graphics.Size(Element.Bounds.Width, Element.Bounds.Height);

			if (elementSize == newSize)
				return;

			var bounds = new Graphics.Rect(Element.X, Element.Y, newSize.Width, newSize.Height);

			Element.Layout(bounds);
		}

		public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			return Container.GetDesiredSize(widthConstraint, heightConstraint);
		}

		public override void Destroy()
		{
			base.Destroy();

			if (!_disposed)
			{
				if (_appeared)
				{
					ReadOnlyCollection<Element> children = (ReadOnlyCollection<Element>)((IElementController)Element).Descendants();
					for (var i = 0; i < children.Count; i++)
					{
						var visualChild = children[i] as VisualElement;
						visualChild?.Cleanup();
					}

					if (Page != null)
						Page.SendDisappearing();
				}

				_appeared = false;

				Dispose(true);

				_disposed = true;
			}
		}

		protected override void OnShown()
		{
			base.OnShown();

			if (_appeared || _disposed)
				return;

			UpdateBackgroundColor();
			UpdateBackgroundImage();

			_appeared = true;

			if (PageController != null)
				PageController.SendAppearing();
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);

			var now = DateTime.Now;
			var diff = now.Subtract(_lastAllocationTime);

			if (_lastAllocation != allocation)
			{
				_lastAllocation = allocation;
				_lastAllocationTime = now;
				SetPageSize(_lastAllocation.Width, _lastAllocation.Height); // Check ToolBar for size calculations.
				PageQueueResize();
			}
			else if (diff > TimeSpan.FromMilliseconds(50)) // Prevent infinite resizing loops for very fast layout changes
			{
				SetPageSize(allocation.Width, allocation.Height);
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Element != null)
				{
					Element.PropertyChanged -= OnElementPropertyChanged;
				}

				Platform.SetRenderer(Element, null);

				Control.Destroy();
				Control = null!;
				Element = null!;
			}
		}

		protected virtual void OnElementChanged(VisualElementChangedEventArgs e)
		{
			if (e.OldElement != null)
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					Control = new Controls.Page();
					Add(Control);
				}

				e.NewElement.PropertyChanged += OnElementPropertyChanged;
			}

			UpdateBackgroundImage();

			ElementChanged?.Invoke(this, e);
		}

		protected virtual void UpdateBackgroundColor()
		{
			if (Element != null)
				Control.SetBackgroundColor(Element.BackgroundColor);
		}

		protected virtual void UpdateBackgroundImage()
		{
			if (Page != null)
				Control.SetBackgroundImage(Page.BackgroundImageSource);
		}

		protected virtual void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
				UpdateBackgroundColor();
			else if (e.PropertyName == Microsoft.Maui.Controls.Page.BackgroundImageSourceProperty.PropertyName)
				UpdateBackgroundImage();
		}

		protected virtual void SetPageSize(int width, int height)
		{
			var finalHeight = height;

			if (Page != null &&
				HasAncestorNavigationPage(Page))
				finalHeight -= GtkToolbarConstants.ToolbarHeight; // Subtract the size of the Toolbar.

			var pageContentSize = new Gdk.Rectangle(0, 0, width, finalHeight);
			var newSize = pageContentSize.ToSize();

			SetElementSize(newSize);
		}

		private void PageQueueResize()
		{
			Control?.Content?.QueueResize();
		}

		private bool HasAncestorNavigationPage(TPage page)
		{
			bool hasParentNavigation = false;
			TPage? parent = null;
			TPage current = page;

			if (current != null)
				parent = current.Parent as TPage;

			while (parent != null)
			{
				hasParentNavigation = parent is NavigationPage;

				current = parent;

				if (hasParentNavigation)
					break;

				if (current != null)
					parent = current.Parent as TPage;
			}
			var hasAncestorNavigationPage = hasParentNavigation && NavigationPage.GetHasNavigationBar(current);
			return hasAncestorNavigationPage;
		}
	}
}
