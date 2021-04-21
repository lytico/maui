using System;

namespace Microsoft.Maui.Controls.Compatibility.Platform.Gtk
{

	public interface IVisualElementRenderer : IRegisterable, IDisposable
	{
		FrameworkElement ContainerElement { get; }

		VisualElement Element { get; }

		event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint);

		void SetElement(VisualElement element);

		global::Gtk.Widget GetNativeElement();
	}

}