using be.berghs.nils.EetFestijnLib.classes;
using be.berghs.nils.EetFestijnLib.interfaces;
using be.berghs.nils.EetFestijnLib.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.viewModels
{
    public class MenuViewModel: BaseViewModel
    {
        public ObservableCollection<Product> Foods { get; set; }

        public ObservableCollection<Product> Beverages { get; set; }

        public ObservableCollection<Product> Desserts { get; set; }

        public ICommand OkCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        internal MenuViewModel(IViewFactory viewFactory): base(viewFactory)
        {
            Foods = new ObservableCollection<Product>();
            Foods.CollectionChanged += Products_CollectionChanged;
            Beverages = new ObservableCollection<Product>();
            Beverages.CollectionChanged += Products_CollectionChanged;
            Desserts = new ObservableCollection<Product>();
            Desserts.CollectionChanged += Products_CollectionChanged;

            OkCommand = new Command(Confirm, CanConfirm);
            CancelCommand = new Command(Cancel);
        }

        private void Products_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if the number of items in our collection is updated, reevaluate the change can execute
            ((Command)OkCommand).ChangeCanExecute();
        }

        private void Cancel()
        {
            ViewFactory.PopView();
        }

        private bool CanConfirm()
        {
            return Foods.Count > 0 || Beverages.Count > 0 || Desserts.Count > 0;
        }

        private void Confirm()
        {
            //todo load next viewmodel
        }
    }
}
