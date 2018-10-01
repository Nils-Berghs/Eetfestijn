using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace be.berghs.nils.eetfestijn.classes
{
    public class PropertyChangedNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
