namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public abstract class PageViewModel:ViewModelBase
    {
        protected StackViewModel<PageViewModel> StackViewModel { get;  }
        
        protected PageViewModel(StackViewModel<PageViewModel> stackViewModel)
        {
            StackViewModel = stackViewModel;
        }
    }
}
