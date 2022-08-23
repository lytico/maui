using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.GTK;
using Microsoft.Maui.Controls.Compatibility.Platform.GTK.Cells;
using Microsoft.Maui.Controls.Compatibility.Platform.GTK.Renderers;

[assembly: ExportImageSourceHandler(typeof(FileImageSource), typeof(FileImageSourceHandler))]
[assembly: ExportImageSourceHandler(typeof(StreamImageSource), typeof(StreamImagesourceHandler))]
// [assembly: ExportImageSourceHandler(typeof(UriImageSource), typeof(UriImageSourceHandler))]
// [assembly: ExportImageSourceHandler(typeof(FontImageSource), typeof(FontImageSourceHandler))]

// [assembly: Microsoft.Maui.Controls.Dependency(typeof(ResourcesProvider))]
// [assembly: Dependency(typeof(GtkSerializer))]

[assembly: ExportRenderer(typeof(ActivityIndicator), typeof(ActivityIndicatorRenderer))]
[assembly: ExportRenderer(typeof(BoxView), typeof(BoxViewRenderer))]
[assembly: ExportRenderer(typeof(Microsoft.Maui.Controls.Button), typeof(Microsoft.Maui.Controls.Compatibility.Platform.GTK.Renderers.ButtonRenderer))]
//[assembly: ExportRenderer(typeof(CarouselPage), typeof(CarouselPageRenderer))]
[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(EditorRenderer))]
[assembly: ExportRenderer(typeof(Entry), typeof(EntryRenderer))]
[assembly: ExportRenderer(typeof(Frame), typeof(FrameRenderer))]
[assembly: ExportRenderer(typeof(Microsoft.Maui.Controls.Image), typeof(ImageRenderer))]
[assembly: ExportRenderer(typeof(Microsoft.Maui.Controls.Label), typeof(LabelRenderer))]
//[assembly: ExportRenderer(typeof(Layout), typeof(LayoutRenderer))]
[assembly: ExportRenderer(typeof(Microsoft.Maui.Controls.ListView), typeof(ListViewRenderer))]
#pragma warning disable CS0618 // Type or member is obsolete
//[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(MasterDetailPageRenderer))]
#pragma warning restore CS0618 // Type or member is obsolete
[assembly: ExportRenderer(typeof(FlyoutPage), typeof(FlyoutPageRenderer))]
[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]
//[assembly: ExportRenderer(typeof(OpenGLView), typeof(OpenGLViewRenderer))]
[assembly: ExportRenderer(typeof(Page), typeof(PageRenderer))]
[assembly: ExportRenderer(typeof(Picker), typeof(PickerRenderer))]
[assembly: ExportRenderer(typeof(Microsoft.Maui.Controls.ProgressBar), typeof(Microsoft.Maui.Controls.Compatibility.Platform.GTK.Renderers.ProgressBarRenderer))]
[assembly: ExportRenderer(typeof(ScrollView), typeof(ScrollViewRenderer))]
[assembly: ExportRenderer(typeof(SearchBar), typeof(SearchBarRenderer))]
[assembly: ExportRenderer(typeof(Slider), typeof(SliderRenderer))]
[assembly: ExportRenderer(typeof(Stepper), typeof(StepperRenderer))]
[assembly: ExportRenderer(typeof(Switch), typeof(SwitchRenderer))]
[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
[assembly: ExportRenderer(typeof(TableView), typeof(TableViewRenderer))]
[assembly: ExportRenderer(typeof(TimePicker), typeof(TimePickerRenderer))]
[assembly: ExportRenderer(typeof(WebView), typeof(WebViewRenderer))]

[assembly: ExportCell(typeof(Cell), typeof(CellRenderer))]
//[assembly: ExportCell(typeof(Microsoft.Maui.Controls.Compatibility.EntryCell), typeof(EntryCellRenderer))]
//[assembly: ExportCell(typeof(Microsoft.Maui.Controls.Compatibility.TextCell), typeof(TextCellRenderer))]
//[assembly: ExportCell(typeof(Microsoft.Maui.Controls.Compatibility.ImageCell), typeof(ImageCellRenderer))]
//[assembly: ExportCell(typeof(Microsoft.Maui.Controls.Compatibility.SwitchCell), typeof(SwitchCellRenderer))]
//[assembly: ExportCell(typeof(Microsoft.Maui.Controls.Compatibility.ViewCell), typeof(ViewCellRenderer))]
