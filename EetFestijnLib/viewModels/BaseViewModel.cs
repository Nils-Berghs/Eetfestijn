using be.berghs.nils.EetFestijnLib.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.viewModels
{
    /// <summary>
    /// The base class for a viewmodel, it contains the INotifyPropertyChanged implementation.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected IViewFactory ViewFactory { get; private set; }
        
        protected BaseViewModel(IViewFactory viewFactory)
        {
            ViewFactory = viewFactory;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            InvokePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            InvokePropertyChanged(propertyName);
        }
        private void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
