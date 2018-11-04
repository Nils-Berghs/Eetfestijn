using be.berghs.nils.eetfestijn.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace be.berghs.nils.eetfestijn.windows
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {

        public StartUpResult StartUpResult { get; set; }

        public StartupWindow()
        {
            InitializeComponent();
        }

        private void NewSessie_Click(object sender, RoutedEventArgs e)
        {
            DefineMenu menuWindow = new DefineMenu();
            this.Hide();
            menuWindow.ShowDialog();
            StartUpResult = StartUpResult.Start;
            this.Close();
        }

        private void ContinueSessie_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult dr = ofd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                App.mOrderList.ReadFromXml(ofd.FileName);
                StartUpResult = StartUpResult.Continue;
                this.Close();
            }
            
            
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            StartUpResult = StartUpResult.Exit;
            this.Close();
        }
    }

    
}
