using be.berghs.nils.EetFestijnLib.Models;
using be.berghs.nils.EetFestijnLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace be.berghs.nils.EetFestijnLib.Helpers.Dialog
{
    public interface IDialogService
    {
        Task ShowDialog(DialogViewModelBase viewModel, string dialogHostIdentifier = null);
        void ShowSaveFileDialog(ExportOptions exportOptions);
    }
}
