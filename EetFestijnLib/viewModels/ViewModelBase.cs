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
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Helper method to implement property with backing private field.
        /// This method will set the field an rais a property changed event if the new value differs from the current
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        /// <returns>True if the property has been set</returns>
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
