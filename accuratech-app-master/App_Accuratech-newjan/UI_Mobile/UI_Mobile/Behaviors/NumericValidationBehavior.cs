using Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace UI_Mobile.Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        
        private static void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            int result;
            bool isValid = int.TryParse(e.NewTextValue, out result);


            ((Entry)sender).TextColor = isValid ? Color.Green : Color.Red;
            ((Entry)sender).Keyboard = isValid ? Keyboard.Numeric : Keyboard.Text;
        }


    }
}
