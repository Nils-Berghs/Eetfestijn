using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class StartViewModel: PageViewModel
    {
        public ICommand NewSessionCommand { get; }

        public ICommand OpenSessionCommand { get; }

        public StartViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService):base(stackViewModel, dialogService)
        {
            NewSessionCommand = new Command(() => StackViewModel.PushViewModel(new CreateSessionViewModel(StackViewModel, dialogService)));
            OpenSessionCommand = new Command(() =>
            {
                //test = !test;
                ((Command)NewSessionCommand).ChangeCanExecute();
            });

        }

        
    }
}
