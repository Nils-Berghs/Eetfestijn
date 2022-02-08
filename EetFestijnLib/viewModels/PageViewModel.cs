using be.berghs.nils.EetFestijnLib.Helpers.Dialog;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public abstract class PageViewModel:ViewModelBase
    {
        public IDialogService DialogService { get; }

        protected StackViewModel<PageViewModel> StackViewModel { get;  }
        
        protected PageViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService)
        {
            DialogService = dialogService;
            StackViewModel = stackViewModel;
        }
    }
}
