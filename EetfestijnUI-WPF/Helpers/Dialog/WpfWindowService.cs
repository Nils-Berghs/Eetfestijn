﻿using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.ViewModels;

namespace be.berghs.nils.EetFestijn.UI.WPF.Helpers.Dialog
{
    internal class WpfWindowService : IWindowService
    {
        public void ShowWindow(ViewModelBase viewModel)
        {
            if (viewModel is OrderViewModel)
            {
                OrderSummaryWindow orderSummaryWindow = new OrderSummaryWindow();
                orderSummaryWindow.DataContext = viewModel;
                orderSummaryWindow.Show();
            }
        }
    }
}