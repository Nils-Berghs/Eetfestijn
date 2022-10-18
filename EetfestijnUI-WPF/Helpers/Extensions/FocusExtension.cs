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
            if ((bool)e.NewValue) // Don't care about false values.
            {
                var uie = (UIElement)d;
                var f = FocusManager.GetFocusScope(uie);
                
                uie.Focus(); 
                Keyboard.Focus(uie);
                FocusManager.SetFocusedElement(f, uie);
            }
        }


    }
}
