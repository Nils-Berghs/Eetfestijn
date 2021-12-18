using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class MenuViewModel: PageViewModel
    {
        public ObservableCollection<Product> Foods { get; private set; }

        public ObservableCollection<Product> Beverages { get; private set; }

        public ObservableCollection<Product> Desserts { get; private set; }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get;  }

        internal MenuViewModel(StackViewModel<PageViewModel> stackViewModel) : base(stackViewModel)
        {
            ProductList productList = ReadProductList();
            Foods = new ObservableCollection<Product>(productList.Foods);
            Beverages = new ObservableCollection<Product>(productList.Beverages);
            Desserts = new ObservableCollection<Product>(productList.Desserts);
            Foods.CollectionChanged += Products_CollectionChanged;
            Beverages.CollectionChanged += Products_CollectionChanged;
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
            StackViewModel.PopViewModel();
        }

        private bool CanConfirm()
        {
            return Foods.Count > 0 || Beverages.Count > 0 || Desserts.Count > 0;
        }

        private void Confirm()
        {
            ProductList productList = new ProductList(Foods, Beverages, Desserts);
            SaveProductList(productList);
            StackViewModel.PushViewModel(new OrderViewModel(StackViewModel, productList));
        }

        /// <summary>
        /// Saves the product list to appData
        /// </summary>
        private void SaveProductList(ProductList productList)
        {
            string path = GetTempMenuPath();
            FileInfo fileInfo = new FileInfo(path);
            Directory.CreateDirectory(fileInfo.DirectoryName);

            File.WriteAllText(path, JsonConvert.SerializeObject(productList, Formatting.Indented));

        }

        /// <summary>
        /// This function reads a product list from a temporary file from AppData
        /// </summary>
        private ProductList ReadProductList()
        {
            string path = GetTempMenuPath();
            try
            {
                if (File.Exists(path))
                {
                    return JsonConvert.DeserializeObject<ProductList>(File.ReadAllText(path));
                }
                else
                {
                    return new ProductList();
                }
            }
            catch
            {
                return new ProductList();
            }
        }

        /// <summary>
        /// Gets the path to temporarily save the menu.
        /// This is AppData/Local/Eetfestijn
        /// </summary>
        /// <returns></returns>
        private string GetTempMenuPath()
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(path, "EetFestijn", "Menu.json");
        }
    }
}
