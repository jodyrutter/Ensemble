using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Ensemble
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
            Grid grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                },
            };
            grid.Children.Add(new Label
            {
                Text = "",
                FontSize = 48,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 16,
            }, 0, 0);
            var profileButton = new Button
            {
                Text = "Profile",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = DeviceDisplay.MainDisplayInfo.Width / 4,
                HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 32,
            };
            profileButton.Clicked += OnProfileButtonClicked;
            grid.Children.Add(profileButton, 0, 1);

            var matchButton = new Button
            {
                Text = "Match",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = DeviceDisplay.MainDisplayInfo.Width / 4,
                HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 32,
            };
            matchButton.Clicked += OnMatchButtonClicked;
            grid.Children.Add(matchButton, 0, 2);

            var composeButton = new Button
            {
                Text = "Compose",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = DeviceDisplay.MainDisplayInfo.Width / 4,
                HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 32,
            };
            composeButton.Clicked += OnComposeButtonClicked;
            grid.Children.Add(composeButton, 0, 3);

            //var tPage = new TabbedPage();
            //tPage.Children.Add(new ProfilePage { Title = "Profile" });
            //tPage.Children.Add(new MatchingPage { Title = "Match" });
            //tPage.Children.Add(new ContentPage { Title = "Compose" });

            //Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage.SetIsSwipePagingEnabled(tPage, false);

            //tPage.Title = "Ensemble";
            //App.Current.MainPage = new NavigationPage();
            //Navigation.PushModalAsync(new TabbedPage());
            this.Content = grid;
        }

        private void OnComposeButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ContentPage());
        }

        private void OnMatchButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MatchingPage());
        }

        private void OnProfileButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ProfilePage());
        }

        /*  void OnProfile(object sender, EventArgs args)
          {
              Navigation.PushModalAsync(new Page1());
          }*/
    }
}
