using Prism.Navigation;
using Prism.Services;

namespace Maui.Controls.Sample.MultiPage.GTK.ViewModels
{
	public class BaseServices
	{
		public BaseServices(
			INavigationService navigationService,
			IPageDialogService pageDialogs,
			IDialogService dialogService,
			IDialogViewRegistry dialogRegistry)
		{
			NavigationService = navigationService;
			PageDialogs = pageDialogs;
			Dialogs = dialogService;
			DialogRegistry = dialogRegistry;
		}

		public INavigationService NavigationService { get; }
		public IPageDialogService PageDialogs { get; }
		public IDialogService Dialogs { get; }
		public IDialogViewRegistry DialogRegistry { get; }
	}
}
