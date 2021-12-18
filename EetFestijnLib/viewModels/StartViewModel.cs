using be.berghs.nils.EetFestijnLib.Helpers;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class StartViewModel: PageViewModel
    {
        public ICommand NewSessionCommand { get; }

        public ICommand OpenSessionCommand { get; }

        public StartViewModel(StackViewModel<PageViewModel> stackViewModel):base(stackViewModel)
        {
            NewSessionCommand = new Command(() => StackViewModel.PushViewModel(new MenuViewModel(StackViewModel)));
            OpenSessionCommand = new Command(() =>
            {
                //test = !test;
                ((Command)NewSessionCommand).ChangeCanExecute();
            });

        }

        
    }
}
