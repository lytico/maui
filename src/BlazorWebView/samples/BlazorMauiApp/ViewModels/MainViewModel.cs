using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Maui.Controls.Sample.Models;
using Maui.Controls.Sample.Pages;
using Maui.Controls.Sample.Services;
using Maui.Controls.Sample.ViewModels.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Dispatching;

namespace Maui.Controls.Sample.ViewModels
{
	public class MainViewModel : BaseGalleryViewModel
	{
		readonly IConfiguration _configuration;
		readonly ITextService _textService;
		readonly IDispatcher _dispatcher;

		public MainViewModel(IConfiguration configuration, ITextService textService, IDispatcher dispatcher)
		{
			_configuration = configuration;
			_textService = textService;
			_dispatcher = dispatcher;
			Debug.WriteLine($"Value from config: {_configuration["MyKey"]}");
			Debug.WriteLine($"Value from TextService: {_textService.GetText()}");

			Task.Run(() =>
			{
				Debug.WriteLine($"This is on the thread pool! (dispatchRequired={_dispatcher.IsDispatchRequired}; id={Environment.CurrentManagedThreadId})");

				_dispatcher.Dispatch(() =>
				{
					Debug.WriteLine($"This is on the main thread! (dispatchRequired={_dispatcher.IsDispatchRequired}; id={Environment.CurrentManagedThreadId})");
				});
			});
		}

		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
#if NET6_0_OR_GREATER
			new SectionModel(typeof(BlazorPage), "Blazor",
				"The BlazorWebView control allow to easily embed Razor components into native UI."),
#endif

		};
	}
}