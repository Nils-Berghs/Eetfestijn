using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class ConsumptionCategoryViewModel
    {
        public IEnumerable<ConsumptionItemViewModel> Items => ItemDictionary.Values;

        private Dictionary<string, ConsumptionItemViewModel> ItemDictionary { get; }

        public string ProductType { get; }

        public ConsumptionCategoryViewModel(IEnumerable<Product> products, string productType)
        {
            ProductType = productType;
            ItemDictionary = new Dictionary<string, ConsumptionItemViewModel>();
            foreach (Product product in products)
                ItemDictionary.Add(product.Name, new ConsumptionItemViewModel(product));
        }

        internal void AddOrder(IEnumerable<OrderItem> items)
        {
            foreach(var item in items)
            {
                if (item.Count > 0)
                    ItemDictionary[item.Product.Name].Consumption += item.Count;
            }
        }
    }
}
