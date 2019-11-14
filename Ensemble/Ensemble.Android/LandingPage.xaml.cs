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
        FirebaseHelper fh = new FirebaseHelper();
        User profile;
        List<User> users = new List<User>();
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

        //Function used to get users from firebase Realtime database
        /*
        public async void GetUserList()
        {
            var items = await fh.GetAllUsers();

            foreach (var item in items)
            {
                users.Add(item);
            }    
        }*/

        [Obsolete]
        private void OnComposeButtonClicked(object sender, EventArgs e)
        {
            //Uncomment if you want page to show out of format
            //Navigation.PushModalAsync(new Page1(FirebaseAuth.Instance.CurrentUser.Email));

            var intent = new Intent(Forms.Context, typeof(ChatUsingFirebase));
            Forms.Context.StartActivity(intent);
            
            
            //if (grid.Children.Count > 3)
              //  grid.Children.RemoveAt(3);
            //var cpage = new Message1(FirebaseAuth.Instance.CurrentUser.Email);
            //grid.Children.Add(cpage, 0, 1);
            //Grid.SetColumnSpan(cpage, 3);
            
            
        }

        private void OnMatchButtonClicked(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new MatchingPage());
            if(grid.Children.Count > 3)
                grid.Children.RemoveAt(3);
            var mpage = new MatchingPage();
            grid.Children.Add(mpage, 0, 1);
            Grid.SetColumnSpan(mpage, 3);
        }

        private void OnProfileButtonClicked(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new ProfilePage());
            if (grid.Children.Count > 3)
                grid.Children.RemoveAt(3);
            var ppage = new ProfilePage();
            grid.Children.Add(ppage, 0, 1);
            Grid.SetColumnSpan(ppage, 3);

        }

        /*  void OnProfile(object sender, EventArgs args)
          {
              Navigation.PushModalAsync(new Page1());
          }*/
    }
}
