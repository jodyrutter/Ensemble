using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Xamarin.Forms.Platform.Android;
using Ensemble.Droid;
using Android.Content;

namespace Ensemble
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        
        //Initialize grid for the page
        Grid grid = new Grid
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(60, GridUnitType.Absolute) },
                    new RowDefinition { Height = GridLength.Auto },
                },
            ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                },
            ColumnSpacing = 0,
        };
        
        public LandingPage()
        {
           //initialize Profile Button for page            
            var profileButton = new Button
            {
                Text = "Profile",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 60,
                WidthRequest = 300,
                BackgroundColor = Color.White,
            };
            profileButton.Clicked += OnProfileButtonClicked;
            grid.Children.Add(profileButton, 0, 0);

            //Initialize Match button
            var matchButton = new Button
            {
                Text = "Match",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 60,
                WidthRequest = 300,
                BackgroundColor = Color.White,
            };
            matchButton.Clicked += OnMatchButtonClicked;
            grid.Children.Add(matchButton, 1, 0);

            //Initialize Compose Button
            var composeButton = new Button
            {
                Text = "Compose",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 60,
                WidthRequest = 300,
                BackgroundColor = Color.White,
            };
            composeButton.Clicked += OnComposeButtonClicked;
            grid.Children.Add(composeButton, 2, 0);
            this.Content = grid;
        }

        //When Compose is clicked, go to RoomSelection Activity
        [Obsolete]
        private void OnComposeButtonClicked(object sender, EventArgs e)
        {
            var intent = new Intent(Forms.Context, typeof(RoomSelection));
            Forms.Context.StartActivity(intent);
        }

        //When Match is clicked, go to Matching Page
        private void OnMatchButtonClicked(object sender, EventArgs e)
        {
            if(grid.Children.Count > 3)
                grid.Children.RemoveAt(3);
            var mpage = new MatchingPage();
            grid.Children.Add(mpage, 0, 1);
            Grid.SetColumnSpan(mpage, 3);
        }

        //When Profile is clicked, go to Profile page
        private void OnProfileButtonClicked(object sender, EventArgs e)
        {
            if (grid.Children.Count > 3)
                grid.Children.RemoveAt(3);
            var ppage = new ProfilePage();
            grid.Children.Add(ppage, 0, 1);
            Grid.SetColumnSpan(ppage, 3);
        }
    }
}
