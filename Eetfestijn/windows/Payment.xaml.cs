using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using be.berghs.nils.eetfestijn.classes;
using System.ComponentModel;
using System.Globalization;

namespace be.berghs.nils.eetfestijn.windows
{
    /// <summary>
    /// Interaction logic for Betaling.xaml
    /// </summary>
    public partial class Payment : Window, INotifyPropertyChanged
    {
        Order mOrder;

        public string VolgNummer
        {
            get
            {
                if (mOrder.BestellingId == 0)
                    return "";
                return mOrder.BestellingId + "";
            }
            set
            {
                try
                {
                    int nr = int.Parse(value);
                    if (mOrder.BestellingId != nr)
                    {
                        mOrder.BestellingId = nr;
                        OnPropertyChanged("PaymentCompleted");
                    }
                }
                catch (Exception)
                {
                    OnPropertyChanged("VolgNummer");
                }

            }
        }

        public string Total
        {
            get
            {
                return mOrder.TotalPrice+ "€";
            }
        }

        public string WaardeBonCount
        {
            get
            {
                if (mOrder.WaardeBonCount == 0)
                    return "";
                return mOrder.WaardeBonCount + "";
            }
            set
            {
                try
                {
                    int nr = int.Parse(value);
                    if (mOrder.WaardeBonCount != nr)
                    {
                        mOrder.WaardeBonCount = nr;
                        UpdateTeBetalen();
                        OnPropertyChanged("PaymentCompleted");
                        OnPropertyChanged("WaardeBonCount");
                    }
                }
                catch (Exception)
                {
                    mOrder.WaardeBonCount = 0;
                    OnPropertyChanged("WaardeBonCount");
                    UpdateTeBetalen();
                }

            }
        }

        private bool _MobilePay;
        public bool MobilePay
        {
            get { return _MobilePay; }
            set 
            { 
                if (value != _MobilePay)
                {
                    _MobilePay = value;
                    OnPropertyChanged("PaymentCompleted");
                }
            }
        }

        public bool CashPay
        {
            get { return !MobilePay; }
            set { MobilePay = !value; }
        }

        private decimal mTeBetalen = 0;
        public string TeBetalen
        {
            get
            {
                /*if (mTeBetalen <= 0)
                    return "";*/
                return mTeBetalen + "€";
            }
        }

        private decimal mBetaald = 0;
        public string Betaald
        {
            get
            {
                if (mBetaald == 0)
                    return "";
                return mBetaald + "";
            }
            set
            {
                try
                {
                    string tmp = value.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    decimal val = decimal.Parse(tmp);
                    if (mBetaald == val && tmp== value)
                        return;
                    mBetaald = val;
                }
                catch (Exception)
                {
                    mBetaald = 0;
                }
                OnPropertyChanged("Betaald");
                UpdateTerug();
                OnPropertyChanged("PaymentCompleted");
            }
        }

        private decimal mTerug = 0;
        public string Terug
        {
            get
            {
                if (mTerug <= 0)
                    return "";
                return mTerug + "€";
            }
            
        }

        public bool tIsJust
        {
            get;
            set;
        }

        public Visibility tIsJustVisibility
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Terug))
                    return Visibility.Visible;
                return Visibility.Hidden;
            }
        }

        public bool PaymentCompleted
        {
            get
            {
                if (MobilePay == true)
                    return true;
                if (/*mOrder.BestellingId > 0 &&*/ mTerug >= 0 && mBetaald >= mTeBetalen)
                    return true;
                return false;
            }
        }

        public Payment(Order order)
        {
            mOrder = order;
            InitializeComponent();
            DataContext = this;
            UpdateTeBetalen();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }

        private void UpdateTeBetalen()
        {
            decimal teBetalen = mOrder.GetTeBetalen();
            if (mTeBetalen != teBetalen)
            {
                mTeBetalen = teBetalen;
                OnPropertyChanged("TeBetalen");
                UpdateTerug(); //maybe the terug value also needs to be recalculated
            }
        }

        private void UpdateTerug()
        {
            decimal terug = mBetaald - mOrder.GetTeBetalen();
            if (mTerug != terug)
            {
                mTerug = terug;
                OnPropertyChanged("Terug");
                OnPropertyChanged("tIsJustVisibility");
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            btnOK.Focus();
            

            if (rdbMobile.IsChecked == true)
            {
                mOrder.Betaald = mOrder.GetTeBetalen();
                mOrder.MobilePay = true;
            }
            else
            {
                if (tIsJust)
                    mOrder.Betaald = mBetaald;
                else
                    mOrder.Betaald = mOrder.GetTeBetalen();
                mOrder.MobilePay = false;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txbVolgNr.Focus();

        }
    }
}
