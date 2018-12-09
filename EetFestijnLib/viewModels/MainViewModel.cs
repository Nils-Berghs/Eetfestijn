using be.berghs.nils.EetFestijnLib.classes;
using be.berghs.nils.EetFestijnLib.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.viewModels
{
    public class MainViewModel: BaseViewModel
    {
        private bool test = true;
        public MainViewModel(IViewFactory viewFactory):base(viewFactory)
        {
            NewSessionCommand = new Command(() => viewFactory.CreateView(new MenuViewModel(viewFactory)), ()=>test == true );
            OpenSessionCommand = new Command(() =>
            {
                test = !test;
                ((Command)NewSessionCommand).ChangeCanExecute();
            });

        }

        public ICommand NewSessionCommand { get; private set; }

        public ICommand OpenSessionCommand { get; private set; }
    }
}
