#nullable enable

using System;
using System.Reflection.Emit;
using Gtk;
using Microsoft.Maui.Controls.Platform;

namespace Microsoft.Maui.Controls
{
	public partial class Element
	{
		public void PopulateNativeElement(object nativeElement, IMauiContext context)
		{
			if (nativeElement is MauiGTKWindow winUIWindow)
			{
				System.Diagnostics.Debug.WriteLine("nativeElement is MauiGTKWindow");
				foreach (var child in AllChildren)
				{
					System.Diagnostics.Debug.WriteLine("Child of type: " + child.GetType().Name);
					if (child is ContentPage childPage)
					{
						System.Diagnostics.Debug.WriteLine("child is ContentPage");
						// We already have MauiGTKWindow (Gtk.Window) in nativeElement
						// Set the title
						winUIWindow.Title = childPage.Title;
						foreach (var innerChild in child.AllChildren)
						{
							System.Diagnostics.Debug.WriteLine("innerChild of type: " + innerChild.GetType().Name);
							if (innerChild is ScrollView)
							{
								// ScrollView is not yet defined for Maui.GTK so do nothing.
								System.Diagnostics.Debug.WriteLine("innerChild is ScrollView");
								foreach (var scrollContent in innerChild.AllChildren)
								{
									System.Diagnostics.Debug.WriteLine("scrollContent of type: " + scrollContent.GetType().Name);
									if (scrollContent is VerticalStackLayout vertLayout)
									{
										// Add a VBox as a vertical stack here
										var vbox = new Gtk.VBox(true, (int)vertLayout.Spacing);
										System.Diagnostics.Debug.WriteLine("scrollContent is VerticalStackLayout");
										foreach (var viewItem in scrollContent.AllChildren)
										{
											System.Diagnostics.Debug.WriteLine("viewItem of type: " + viewItem.GetType().Name);
											if (viewItem is Label viewLabel)
											{
												var label = new Gtk.Label();
												label.Text = viewLabel.Text;
												vbox.PackStart(label, true, true, (uint)vertLayout.Padding.Top);
											}
											else if (viewItem is Button viewButton)
											{
												var fileName = viewButton.ImageSource.ToString();
												// 12345
												// File: 
												var usableFileName = fileName?.Substring(6);
												Gtk.Image image = new Gtk.Image(usableFileName);
												var button = new Gtk.Button();
												button.Label = viewButton.Text;
												button.Image = image;

												HBox buttonBox = new HBox();
												VBox buttonSpacer = new VBox();
												buttonSpacer.SetSizeRequest((int)viewButton.WidthRequest, (int)viewButton.HeightRequest);
												buttonBox.PackStart(buttonSpacer, false, false, 3);
												buttonBox.PackStart(button, false, false, 3);

												vbox.PackStart(buttonBox, true, true, (uint)vertLayout.Padding.Top);
											}
										}
										winUIWindow.Add(vbox);
									}
								}
							}
						}
					}
				}
			}

			// Above produced Debug output:
			//
			// nativeElement is MauiGTKWindow
			// Child of type: MainPage
			// child is ContentPage
			// innerChild of type: ScrollView
			// innerChild is ScrollView
			// scrollContent of type: VerticalStackLayout
			// scrollContent is VerticalStackLayout
			// viewItem of type: Label
			// viewItem of type: Label
			// viewItem of type: Button
		}

		public static void MapAutomationPropertiesIsInAccessibleTree(IElementHandler handler, Element element)
		{
			Platform.AutomationPropertiesProvider.SetImportantForAccessibility(
				handler.PlatformView as Gtk.EventBox, element);
		}

		public static void MapAutomationPropertiesExcludedWithChildren(IElementHandler handler, Element element)
		{
			Platform.AutomationPropertiesProvider.SetImportantForAccessibility(
				handler.PlatformView as Gtk.EventBox, element);
		}
	}
}
