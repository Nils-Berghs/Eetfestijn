using be.berghs.nils.EetFestijnLib.classes;
using be.berghs.nils.EetFestijnLib.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.viewModels
{
    public class MainViewModel: PageViewModel
    {
        public ICommand NewSessionCommand { get; }

        public ICommand OpenSessionCommand { get; }

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

        
    }
}
