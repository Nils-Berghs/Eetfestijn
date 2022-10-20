using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class StartViewModel: PageViewModel
    {
        public ICommand NewSessionCommand { get; }

        public ICommand OpenSessionCommand { get; }

        public ObservableCollection<SessionViewModel> Sessions { get; }

        public StartViewModel(StackViewModel<PageViewModel> stackViewModel, IDialogService dialogService, IWindowService windowService) :base(stackViewModel, dialogService, windowService)
        {
            var sessions = FileSystemHelper.ReadAvailableSessions();
            Sessions = new ObservableCollection<SessionViewModel>(sessions.Select(s => new SessionViewModel(stackViewModel, dialogService, windowService, s)));

            NewSessionCommand = new Command(() => StackViewModel.PushViewModel(new CreateSessionViewModel(StackViewModel, dialogService, windowService)));
            OpenSessionCommand = new Command<SessionViewModel>(s =>
            {
                FileSystemHelper.ReadFullSession(s.Session);
                StackViewModel.PushViewModel(s);
            });

        }

        
    }
}
