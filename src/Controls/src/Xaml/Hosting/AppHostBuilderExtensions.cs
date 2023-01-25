using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Maui.Controls.Handlers;
#if !__GTK__
using Microsoft.Maui.Controls.Handlers.Items;
#endif
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Platform;


#if ANDROID
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#elif WINDOWS && __GTK__
using ResourcesProvider = Microsoft.Maui.Controls.Compatibility.Platform.GTK.ResourcesProvider;
using Microsoft.Maui.Controls.Compatibility.Platform.GTK;
#elif WINDOWS && !__GTK__
using ResourcesProvider = Microsoft.Maui.Controls.Compatibility.Platform.UWP.WindowsResourcesProvider;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
#elif IOS || MACCATALYST                   
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
#elif TIZEN
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Tizen;
#endif

namespace Microsoft.Maui.Controls.Hosting
{
	public static partial class AppHostBuilderExtensions
	{
		public static MauiAppBuilder UseMauiApp<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp>(this MauiAppBuilder builder)
			where TApp : class, IApplication
		{
#pragma warning disable RS0030 // Do not used banned APIs - don't want to use a factory method here
			builder.Services.TryAddSingleton<IApplication, TApp>();
#pragma warning restore RS0030
			builder.SetupDefaults();
			return builder;
		}

		public static MauiAppBuilder UseMauiApp<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp>(this MauiAppBuilder builder, Func<IServiceProvider, TApp> implementationFactory)
			where TApp : class, IApplication
		{
			builder.Services.TryAddSingleton<IApplication>(implementationFactory);
			builder.SetupDefaults();
			return builder;
		}

		public static IMauiHandlersCollection AddMauiControlsHandlers(this IMauiHandlersCollection handlersCollection)
		{
#if !__GTK__
			handlersCollection.AddHandler<CollectionView, CollectionViewHandler>();
			handlersCollection.AddHandler<CarouselView, CarouselViewHandler>();
#endif
			handlersCollection.AddHandler<Application, ApplicationHandler>();
			handlersCollection.AddHandler<ActivityIndicator, ActivityIndicatorHandler>();
#if !__GTK__
			handlersCollection.AddHandler<BoxView, ShapeViewHandler>();
#endif
			handlersCollection.AddHandler<Button, ButtonHandler>();
			handlersCollection.AddHandler<CheckBox, CheckBoxHandler>();
			handlersCollection.AddHandler<DatePicker, DatePickerHandler>();
			handlersCollection.AddHandler<Editor, EditorHandler>();
			handlersCollection.AddHandler<Entry, EntryHandler>();
			handlersCollection.AddHandler<GraphicsView, GraphicsViewHandler>();
			handlersCollection.AddHandler<Image, ImageHandler>();
			handlersCollection.AddHandler<Label, LabelHandler>();
#if __GTK__
			handlersCollection.AddHandler<VerticalStackLayout, VerticalStackLayoutHandler>();
			handlersCollection.AddHandler<HorizontalStackLayout, HorizontalStackLayoutHandler>();
#endif
			handlersCollection.AddHandler<Layout, LayoutHandler>();
			handlersCollection.AddHandler<Picker, PickerHandler>();
			handlersCollection.AddHandler<ProgressBar, ProgressBarHandler>();
			handlersCollection.AddHandler<ScrollView, ScrollViewHandler>();
			handlersCollection.AddHandler<SearchBar, SearchBarHandler>();
			handlersCollection.AddHandler<Slider, SliderHandler>();
			handlersCollection.AddHandler<Stepper, StepperHandler>();
			handlersCollection.AddHandler<Switch, SwitchHandler>();
			handlersCollection.AddHandler<TimePicker, TimePickerHandler>();
			handlersCollection.AddHandler<Page, PageHandler>();
#if !__GTK__
			handlersCollection.AddHandler<WebView, WebViewHandler>();
#endif
			handlersCollection.AddHandler<Border, BorderHandler>();
			handlersCollection.AddHandler<IContentView, ContentViewHandler>();
#if !__GTK__
			handlersCollection.AddHandler<Shapes.Ellipse, ShapeViewHandler>();
			handlersCollection.AddHandler<Shapes.Line, LineHandler>();
			handlersCollection.AddHandler<Shapes.Path, PathHandler>();
			handlersCollection.AddHandler<Shapes.Polygon, PolygonHandler>();
			handlersCollection.AddHandler<Shapes.Polyline, PolylineHandler>();
			handlersCollection.AddHandler<Shapes.Rectangle, RectangleHandler>();
			handlersCollection.AddHandler<Shapes.RoundRectangle, RoundRectangleHandler>();
#endif
			handlersCollection.AddHandler<Window, WindowHandler>();
			handlersCollection.AddHandler<ImageButton, ImageButtonHandler>();
			handlersCollection.AddHandler<IndicatorView, IndicatorViewHandler>();
			handlersCollection.AddHandler<RadioButton, RadioButtonHandler>();
#if !__GTK__
			handlersCollection.AddHandler<RefreshView, RefreshViewHandler>();
			handlersCollection.AddHandler<SwipeItem, SwipeItemMenuItemHandler>();
			handlersCollection.AddHandler<SwipeView, SwipeViewHandler>();
#endif

#pragma warning disable CA1416 //  'MenuBarHandler', MenuFlyoutSubItemHandler, MenuFlyoutSubItemHandler, MenuBarItemHandler is only supported on: 'ios' 13.0 and later
			handlersCollection.AddHandler<MenuBar, MenuBarHandler>();
#if !__GTK__
			handlersCollection.AddHandler<MenuFlyoutSubItem, MenuFlyoutSubItemHandler>();
			handlersCollection.AddHandler<MenuFlyoutSeparator, MenuFlyoutSeparatorHandler>();
			handlersCollection.AddHandler<MenuFlyoutItem, MenuFlyoutItemHandler>();
#endif
			handlersCollection.AddHandler<MenuBarItem, MenuBarItemHandler>();
#pragma warning restore CA1416

#if WINDOWS || ANDROID || IOS || MACCATALYST || TIZEN
#if !__GTK__
			handlersCollection.AddHandler(typeof(ListView), typeof(Handlers.Compatibility.ListViewRenderer));
#endif
#if !TIZEN
			handlersCollection.AddHandler(typeof(Cell), typeof(Handlers.Compatibility.CellRenderer));
			handlersCollection.AddHandler(typeof(ImageCell), typeof(Handlers.Compatibility.ImageCellRenderer));
			handlersCollection.AddHandler(typeof(EntryCell), typeof(Handlers.Compatibility.EntryCellRenderer));
			handlersCollection.AddHandler(typeof(TextCell), typeof(Handlers.Compatibility.TextCellRenderer));
			handlersCollection.AddHandler(typeof(ViewCell), typeof(Handlers.Compatibility.ViewCellRenderer));
			handlersCollection.AddHandler(typeof(SwitchCell), typeof(Handlers.Compatibility.SwitchCellRenderer));
#endif
#if !__GTK__
			handlersCollection.AddHandler(typeof(TableView), typeof(Handlers.Compatibility.TableViewRenderer));
#endif
			handlersCollection.AddHandler(typeof(Frame), typeof(Handlers.Compatibility.FrameRenderer));
#endif

#if (WINDOWS && !__GTK__) || MACCATALYST
			handlersCollection.AddHandler(typeof(MenuFlyout), typeof(MenuFlyoutHandler));
#endif

#if IOS || MACCATALYST
			handlersCollection.AddHandler(typeof(NavigationPage), typeof(Handlers.Compatibility.NavigationRenderer));
			handlersCollection.AddHandler(typeof(TabbedPage), typeof(Handlers.Compatibility.TabbedRenderer));
			handlersCollection.AddHandler(typeof(FlyoutPage), typeof(Handlers.Compatibility.PhoneFlyoutPageRenderer));
#endif

#if ANDROID || IOS || MACCATALYST || TIZEN
			handlersCollection.AddHandler<SwipeItemView, SwipeItemViewHandler>();
#if ANDROID || IOS || MACCATALYST
			handlersCollection.AddHandler<Shell, ShellRenderer>();
#else
			handlersCollection.AddHandler<Shell, ShellHandler>();
			handlersCollection.AddHandler<ShellItem, ShellItemHandler>();
			handlersCollection.AddHandler<ShellSection, ShellSectionHandler>();
#endif
#endif
#if WINDOWS || ANDROID || TIZEN
			handlersCollection.AddHandler<NavigationPage, NavigationViewHandler>();
#if !__GTK__
			handlersCollection.AddHandler<Toolbar, ToolbarHandler>();
			handlersCollection.AddHandler<FlyoutPage, FlyoutViewHandler>();
#endif
			handlersCollection.AddHandler<TabbedPage, TabbedViewHandler>();
#endif

#if WINDOWS
#if !__GTK__
			handlersCollection.AddHandler<ShellItem, ShellItemHandler>();
			handlersCollection.AddHandler<ShellSection, ShellSectionHandler>();
			handlersCollection.AddHandler<ShellContent, ShellContentHandler>();
			handlersCollection.AddHandler<Shell, ShellHandler>();
#endif
#endif
			return handlersCollection;
		}

		static MauiAppBuilder SetupDefaults(this MauiAppBuilder builder)
		{
#if WINDOWS || ANDROID || IOS || MACCATALYST || TIZEN || __GTK__
			// initialize compatibility DependencyService
			DependencyService.SetToInitialized();
			DependencyService.Register<Xaml.ResourcesLoader>();
			DependencyService.Register<Xaml.ValueConverterProvider>();
#if !__GTK__
			DependencyService.Register<PlatformSizeService>();
#endif

#pragma warning disable CS0612, CA1416 // Type or member is obsolete, 'ResourcesProvider' is unsupported on: 'iOS' 14.0 and later
			DependencyService.Register<ResourcesProvider>();
#if !__GTK__
			DependencyService.Register<FontNamedSizeService>();
#endif
#pragma warning restore CS0612, CA1416 // Type or member is obsolete
#endif

			builder.ConfigureImageSourceHandlers();
			builder
				.ConfigureMauiHandlers(handlers =>
				{
					handlers.AddMauiControlsHandlers();
				});

#if WINDOWS || __GTK__
			builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IMauiInitializeService, MauiControlsInitializer>());
#endif
			builder.RemapForControls();

			return builder;
		}

		class MauiControlsInitializer : IMauiInitializeService
		{
			public void Initialize(IServiceProvider services)
			{
#if WINDOWS && !__GTK__
				var dispatcher =
					services.GetService<IDispatcher>() ??
					MauiWinUIApplication.Current.Services.GetRequiredService<IDispatcher>();

				dispatcher
					.DispatchIfRequired(() =>
					{
						var dictionaries = UI.Xaml.Application.Current?.Resources?.MergedDictionaries;
						if (dictionaries != null)
						{
							// Microsoft.Maui.Controls
							UI.Xaml.Application.Current?.Resources?.AddLibraryResources("MicrosoftMauiControlsIncluded", "ms-appx:///Microsoft.Maui.Controls/Platform/Windows/Styles/Resources.xbf");
						}
					});
#endif
			}
		}


		internal static MauiAppBuilder ConfigureImageSourceHandlers(this MauiAppBuilder builder)
		{
			builder.ConfigureImageSources(services =>
			{
				services.AddService<FileImageSource>(svcs => new FileImageSourceService(svcs.CreateLogger<FileImageSourceService>()));
				services.AddService<FontImageSource>(svcs => new FontImageSourceService(svcs.GetRequiredService<IFontManager>(), svcs.CreateLogger<FontImageSourceService>()));
				services.AddService<StreamImageSource>(svcs => new StreamImageSourceService(svcs.CreateLogger<StreamImageSourceService>()));
				services.AddService<UriImageSource>(svcs => new UriImageSourceService(svcs.CreateLogger<UriImageSourceService>()));
			});

			return builder;
		}

		internal static MauiAppBuilder RemapForControls(this MauiAppBuilder builder)
		{
			// Update the mappings for IView/View to work specifically for Controls
			Application.RemapForControls();
			VisualElement.RemapForControls();
			Label.RemapForControls();
			Button.RemapForControls();
			CheckBox.RemapForControls();
			DatePicker.RemapForControls();
			RadioButton.RemapForControls();
#if !__GTK__
			FlyoutPage.RemapForControls();
			Toolbar.RemapForControls();
#endif
			Window.RemapForControls();
			Editor.RemapForControls();
			Entry.RemapForControls();
			Picker.RemapForControls();
			SearchBar.RemapForControls();
			TabbedPage.RemapForControls();
			TimePicker.RemapForControls();
			Layout.RemapForControls();
			ScrollView.RemapForControls();
			RefreshView.RemapForControls();
			Shape.RemapForControls();
			WebView.RemapForControls();

			return builder;
		}
	}
}