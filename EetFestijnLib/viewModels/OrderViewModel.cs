using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using be.berghs.nils.EetFestijnLib.interfaces;
using be.berghs.nils.EetFestijnLib.models;

namespace be.berghs.nils.EetFestijnLib.viewModels
{
    public class OrderViewModel : PageViewModel
    {
        public ObservableCollection<OrderItemViewModel> Foods { get;  }

        public ObservableCollection<OrderItemViewModel> Beverages { get; }

        public ObservableCollection<OrderItemViewModel> Desserts { get; }

        public OrderViewModel(IViewFactory viewFactory, ProductList products) : base(viewFactory)
        {
            Foods = new ObservableCollection<OrderItemViewModel>(products.Foods.Select(p => new OrderItemViewModel(p)));
            Beverages = new ObservableCollection<OrderItemViewModel>(products.Beverages.Select(p => new OrderItemViewModel(p)));
            Desserts = new ObservableCollection<OrderItemViewModel>(products.Desserts.Select(p => new OrderItemViewModel(p)));
        }
    }
}
