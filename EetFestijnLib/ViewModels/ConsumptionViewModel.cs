using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class ConsumptionViewModel : ViewModelBase
    {
        public ConsumptionCategoryViewModel Foods { get; }

        public ConsumptionCategoryViewModel Beverages { get; }

        public ConsumptionCategoryViewModel Desserts { get; }


        public ConsumptionViewModel(Session session)
        {
            Foods = new ConsumptionCategoryViewModel(session.ProductList.Foods, "Eten");
            Beverages = new ConsumptionCategoryViewModel(session.ProductList.Beverages, "Drank");
            Desserts = new ConsumptionCategoryViewModel(session.ProductList.Desserts, "Dessert");
        }

        internal void AddOrder(Order order)
        {
            Foods.AddOrder(order.Foods);
            Beverages.AddOrder(order.Beverages);
            Desserts.AddOrder(order.Desserts);
        }
    }
}
