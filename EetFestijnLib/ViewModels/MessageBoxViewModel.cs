using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.ViewModels
{
    public class MessageBoxViewModel:DialogViewModelBase
    {
        public string Message { get; }

        public bool HasCancel { get; }

        public MessageBoxViewModel(string message, bool hasCancel = false)
        {
            Message = message;
            HasCancel = hasCancel;
        }
    }
}
