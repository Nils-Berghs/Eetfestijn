using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using be.berghs.nils.EetFestijnLib.interfaces;
using be.berghs.nils.EetFestijnLib.models;

namespace be.berghs.nils.EetFestijnLib.viewModels
{
    public class OrderViewModel : BaseViewModel
    {
        public ObservableCollection<Product> Foods { get; private set; }

        public ObservableCollection<Product> Beverages { get; private set; }

        public ObservableCollection<Product> Desserts { get; private set; }

        public OrderViewModel(IViewFactory viewFactory, ProductList products) : base(viewFactory)
        {
            Foods = new ObservableCollection<Product>(products.Foods);
            Beverages = new ObservableCollection<Product>(products.Beverages);
            Desserts = new ObservableCollection<Product>(products.Desserts);
        }
    }
}
