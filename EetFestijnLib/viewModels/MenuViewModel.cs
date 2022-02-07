using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class MenuViewModel: PageViewModel
    {
        public ObservableCollection<ProductViewModel> Foods { get; private set; }

        public ObservableCollection<ProductViewModel> Beverages { get; private set; }

        public ObservableCollection<ProductViewModel> Desserts { get; private set; }

        public ICommand MoveItemDownCommand { get; }

        public ICommand MoveItemUpCommand { get; }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get;  }

        internal MenuViewModel(StackViewModel<PageViewModel> stackViewModel) : base(stackViewModel)
        {
            ProductList productList = ReadProductList();
            Foods = new ObservableCollection<ProductViewModel>();// (productList.Foods.Select(p => new ProductViewModel(p)));
            Beverages = new ObservableCollection<ProductViewModel>(productList.Beverages.Select(p => new ProductViewModel(p)));
            Desserts = new ObservableCollection<ProductViewModel>(productList.Desserts.Select(p => new ProductViewModel(p)));
            Foods.CollectionChanged += Products_CollectionChanged;
            Beverages.CollectionChanged += Products_CollectionChanged;
            Desserts.CollectionChanged += Products_CollectionChanged;

            OkCommand = new Command(Confirm, CanConfirm);
            CancelCommand = new Command(Cancel);
            MoveItemDownCommand = new Command<ProductViewModel>(pvm =>MoveItemDown(pvm), pvm => CanMoveItemDown(pvm));
            MoveItemUpCommand = new Command<ProductViewModel>(pvm => MoveItemUp(pvm), pvm => CanMoveItemUp(pvm));

            Foods.Add(new ProductViewModel() { Name = "Zalm", Price = "19" });
            Foods.Add(new ProductViewModel() { Name = "Vlees", Price = "17" });
            Foods.Add(new ProductViewModel() { Name = "Ballekes", Price = "15" });
            Foods.Add(new ProductViewModel() { Name = "Vegi", Price = "14" });

        }

        private bool CanMoveItemDown(ProductViewModel pvm)
        {
            return Foods.IndexOf(pvm) < Foods.Count - 1;
        }

        private void MoveItemDown(ProductViewModel pvm)
        {
            int index = Foods.IndexOf(pvm);
            Foods.Remove(pvm);
            Foods.Insert(index + 1, pvm);
        }

        private bool CanMoveItemUp(ProductViewModel pvm)
        {
            return Foods.IndexOf(pvm) !=0 && !string.IsNullOrWhiteSpace(pvm.Name) && !string.IsNullOrWhiteSpace(pvm.Price);
        }

        private void MoveItemUp(ProductViewModel pvm)
        {
            int index = Foods.IndexOf(pvm);
            Foods.Remove(pvm);
            Foods.Insert(index - 1, pvm);
        }

        private void Products_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if the number of items in our collection is updated, reevaluate the commands
            ((Command)OkCommand).ChangeCanExecute();
            ((Command<ProductViewModel>)MoveItemDownCommand).ChangeCanExecute();
            ((Command<ProductViewModel>)MoveItemUpCommand).ChangeCanExecute();


        }

        private void ProductViewModelInitializedEvent(object sender, EventArgs e)
        {
            if (sender is ProductViewModel pvm)
            {
                pvm.ProductViewModelInitializedEvent -= ProductViewModelInitializedEvent;
                ((Command<ProductViewModel>)MoveItemUpCommand).ChangeCanExecute();
            }
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
            //TODO

            //ProductList productList = new ProductList(Foods, Beverages, Desserts);
            //SaveProductList(productList);
            //StackViewModel.PushViewModel(new OrderViewModel(StackViewModel, productList));
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
