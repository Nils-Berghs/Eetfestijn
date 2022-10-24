using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.ViewModels;

namespace be.berghs.nils.EetFestijn.UI.WPF.Helpers.Dialog
{
    internal class WpfWindowService : IWindowService
    {
        private OrderSummaryWindow? OrderSummaryWindow { get; set; }

        public void ShowWindow(ViewModelBase viewModel)
        {
            if (viewModel is OrderViewModel && OrderSummaryWindow == null)
            {
                OrderSummaryWindow = new OrderSummaryWindow();
                OrderSummaryWindow.DataContext = viewModel;
                OrderSummaryWindow.Show();
            }
        }

        public void CloseWindow(ViewModelBase viewModel)
        {
            if (viewModel is OrderViewModel && OrderSummaryWindow != null)
            {
                OrderSummaryWindow.Close();
                OrderSummaryWindow = null;
            }
        }
    }
}
