using System;

using Xamarin.Forms;

namespace Ensemble.Droid
{
    public class MatchingPage : ContentPage
    {
        public MatchingPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

