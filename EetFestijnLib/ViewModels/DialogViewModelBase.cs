using be.berghs.nils.EetFestijnLib.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class DialogViewModelBase: ViewModelBase
    {
        public event EventHandler Close;

        /// <summary>
        /// Indicates if the dialog shown for this viewmodel was confirmed
        /// </summary>
        public bool IsConfirmed { get; protected set; }

        /// <summary>
        /// The command to link to an 'OK' button
        /// </summary>
        public Command OkCommand { get; }

        /// <summary>
        /// The command to link to a 'Cancel' button
        /// </summary>
        public Command CancelCommand { get; }

        public DialogViewModelBase()
        {
            OkCommand = new Command(async () => await ConfirmAndClose(), CanConfirm);
            CancelCommand = new Command(RaisCloseEvent, CanCancel);
        }

        private async Task ConfirmAndClose()
        {
            if (!await PreviewConfirm())
                return;

            IsConfirmed = true;
            await Confirm();
            RaisCloseEvent();
        }

        protected void RaisCloseEvent()
        {
            Close?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Previews the confirm and allows child implementations to cancel the confirmation.
        /// </summary>
        /// <returns>True to continue, false to cancel</returns>
        protected virtual Task<bool> PreviewConfirm()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Child implementations can override this method for custom actions on confirm.
        /// The base implementation just returns a completed task.
        /// </summary>
        protected virtual Task Confirm()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Child implementations must override this method to disable the OK commmand
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanConfirm()
        {
            return true;
        }

        /// <summary>
        /// Child implementations must override this method to disable the Cancel Command
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanCancel()
        {
            return true;
        }
    }
}
