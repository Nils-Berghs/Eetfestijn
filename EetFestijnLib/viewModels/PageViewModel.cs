using be.berghs.nils.EetFestijnLib.Helpers.Dialog;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public abstract class PageViewModel:ViewModelBase
    {
        public IDialogService DialogService { get; }

        public IWindowService WindowService { get; }

        protected StackViewModel<PageViewModel> StackViewModel { get;  }
        
        protected PageViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService, IWindowService windowService)
        {
            DialogService = dialogService;
            StackViewModel = stackViewModel;
            WindowService = windowService;
        }
    }
}
