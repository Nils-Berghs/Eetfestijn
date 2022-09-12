using EetfestijnUI_WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijn.UI.WPF.Helpers.Extensions
{
    public static class FocusExtension
    {
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        public static readonly DependencyProperty IsFocusedProperty =  DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusExtension), new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uie = (UIElement)d;
            if (uie is TextBox txb)
            {
                Debug.WriteLine("FocusExtension: Setting focused for textbox {0} to {1}", txb.Name, e.NewValue);
            }
            else
                Debug.WriteLine("FocusExtension: Setting focused for {0} to {}1", uie.GetType(), e.NewValue);
            if ((bool)e.NewValue) // Don't care about false values.
            {
                var f = FocusManager.GetFocusScope(uie);
                
                
                var result = uie.Focus(); 
                Keyboard.Focus(uie);
                FocusManager.SetFocusedElement(f, uie);
            }
            else
            {

            }
        }


    }
}
