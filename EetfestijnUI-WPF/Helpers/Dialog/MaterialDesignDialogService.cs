using be.berghs.nils.EetFestijnLib.Helpers.Dialog;
using be.berghs.nils.EetFestijnLib.Models;
using be.berghs.nils.EetFestijnLib.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijn.UI.WPF.Helpers.Dialog
{
    internal class MaterialDesignDialogService : IDialogService
    {
        public async Task ShowDialog(DialogViewModelBase viewModel, string dialogHostIdentifier)
        {
            _ = await DialogHost.Show(viewModel, dialogHostIdentifier, (object sender, DialogOpenedEventArgs e) =>
            {
                void OnClose(object _, EventArgs args)
                {
                    viewModel.Close -= OnClose;
                    e.Session.Close();
                    // this call clears the session content, this avoids that behaviours defined in xaml are applied twice
                    // when the same dialog is shown multiple times in succession (e.g. on the confirm with badge screen)
                    e.Session.UpdateContent(null);
                }
                viewModel.Close += OnClose;
            });
        }

        public void ShowSaveFileDialog(ExportOptions exportOptions)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = exportOptions.Filter;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.AddExtension= true;

            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                exportOptions.IsConfirmed = true;
                exportOptions.FileName = saveFileDialog.FileName;
            }

        }

    }
}
