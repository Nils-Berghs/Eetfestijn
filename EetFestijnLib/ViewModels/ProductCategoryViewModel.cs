using be.berghs.nils.EetFestijnLib.Helpers;
using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class ProductCategoryViewModel:ViewModelBase
    {
        IDialogService DialogService { get; }

        public ObservableCollection<ProductViewModel> Products { get;  }

        public Command<ProductViewModel> MoveItemDownCommand { get; }

        public Command<ProductViewModel> MoveItemUpCommand { get; }

        public ICommand DeleteItemCommand { get; }

        public ICommand EditItemCommand { get; }

        public ICommand AddProductCommand { get; }

        public string DialogIdentifier { get; }

        public string Title { get; }

        internal ProductCategoryViewModel(IEnumerable<Product> products, IDialogService dialogService, string title)
        {
            DialogService = dialogService;
            Title = title;
            DialogIdentifier = title+ "DialogId";

            Products = new ObservableCollection<ProductViewModel>(products.Select(p => new ProductViewModel(p)));
            Products.CollectionChanged += ProductCollectionChanged;

            MoveItemDownCommand = new Command<ProductViewModel>(pvm => MoveItemDown(pvm), pvm => CanMoveItemDown(pvm));
            MoveItemUpCommand = new Command<ProductViewModel>(pvm => MoveItemUp(pvm), pvm => CanMoveItemUp(pvm));
            DeleteItemCommand = new Command<ProductViewModel>(pvm => DeleteItem(pvm));
            EditItemCommand = new Command<ProductViewModel>(async pvm => await EditItem(pvm));
            AddProductCommand = new Command(AddProduct);

        }

        internal IEnumerable<Product> GetProducts()
        {
            return Products.Select(p => p.Product);
        }

        private async void AddProduct()
        {
            var viewModel = new EditProductViewModel();
            await DialogService.ShowDialog(viewModel, DialogIdentifier);
            if (viewModel.IsConfirmed)
                Products.Add(new ProductViewModel(viewModel.Product));
        }

        private bool CanMoveItemDown(ProductViewModel pvm)
        {
            return Products.IndexOf(pvm) < Products.Count - 1;
        }

        private void MoveItemDown(ProductViewModel pvm)
        {
            int index = Products.IndexOf(pvm);
            Products.Remove(pvm);
            Products.Insert(index + 1, pvm);
        }

        private bool CanMoveItemUp(ProductViewModel pvm)
        {
            return Products.IndexOf(pvm) != 0 && !string.IsNullOrWhiteSpace(pvm.Name) && !string.IsNullOrWhiteSpace(pvm.Price);
        }

        private void MoveItemUp(ProductViewModel pvm)
        {
            int index = Products.IndexOf(pvm);
            Products.Remove(pvm);
            Products.Insert(index - 1, pvm);
        }

        private async Task EditItem(ProductViewModel pvm)
        {
            var viewModel = new EditProductViewModel(pvm.Product);
            await DialogService.ShowDialog(viewModel, DialogIdentifier);
            if (viewModel.IsConfirmed)
                pvm.RaisePropertyChanged();
        }

        private void DeleteItem(ProductViewModel pvm)
        {
            Products.Remove(pvm);
        }

        private void ProductCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if the number of items in our collection is updated, reevaluate the commands
            MoveItemDownCommand.ChangeCanExecute();
            MoveItemUpCommand.ChangeCanExecute();


        }
    }
}
