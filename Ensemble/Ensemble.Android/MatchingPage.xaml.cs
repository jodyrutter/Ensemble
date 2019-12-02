using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ensemble
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
   
    public partial class MatchingPage : ContentView
    {
        public ObservableCollection<User> people { get; set; }
        ListView lView = new ListView()
        {
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.White,
            HasUnevenRows = true,
        };
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        User u = new User();

        public MatchingPage()
        {
            people = new ObservableCollection<User>();

            DataTemplate ListDataTemplate = new DataTemplate(() =>
            {
                SwipeGestureGrid gridData = new SwipeGestureGrid()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    RowDefinitions =
                        {
                            new RowDefinition { },
                        },
                    ColumnDefinitions =
                        {
                            new ColumnDefinition { },
                            new ColumnDefinition { },
                            new ColumnDefinition { },
                        }
                };
                Grid gridBase = new Grid()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = 60,
                    RowDefinitions =
                    {
                        new RowDefinition { },
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(0, GridUnitType.Absolute)},   //Button for Cells here
                        new ColumnDefinition {  },                                                   //Put Cells Data here
                        new ColumnDefinition { Width = new GridLength(0, GridUnitType.Absolute)},   //Button for Cells here
                    },
                };

                Button btnCellDelete = new Button() { Text = "No", BackgroundColor = Color.Red };
                Button btnCellApprove = new Button() { Text = "Yes", BackgroundColor = Color.Green };

                var nameLabel = new Label { FontAttributes = FontAttributes.Bold };
                var insLabel = new Label();
                var imageLabel = new Image();

                nameLabel.SetBinding(Label.TextProperty, "uname");
                insLabel.SetBinding(Label.TextProperty, "FavInstrument");
                imageLabel.SetBinding(Image.SourceProperty, "ProfilePic");

                gridData.Children.Add(nameLabel, 1, 0);
                gridData.Children.Add(insLabel, 2, 0);
                gridData.Children.Add(imageLabel, 0, 0);

                gridBase.Children.Add(btnCellApprove, 0, 0);
                gridBase.Children.Add(gridData, 1, 0);
                gridBase.Children.Add(btnCellDelete, 2, 0);

                gridData.SwipeLeft += GridTemplate_SwipeLeft;
                gridData.SwipeRight += GridTemplate_SwipeRight;
                gridData.Tapped += GridTemplate_Tapped;
                btnCellDelete.Clicked += BtnCellDelete_Clicked;
                btnCellApprove.Clicked += BtnCellApprove_Clicked;

                return new ViewCell
                {
                    View = gridBase,
                    Height = 60,
                };
            });

            GetPossibleMatches();

            //people.Add(new person() { name = "Steve", instruments = "Guitar", image = "" });
            //people.Add(new person() { name = "John", instruments = "Piano", image = "" });
            //people.Add(new person() { name = "Tom", instruments = "Flute", image = "" });
            //people.Add(new person() { name = "Lucas", instruments = "Recorder", image = "" });
            //people.Add(new person() { name = "Tariq", instruments = "Drums", image = "" });
            //people.Add(new person() { name = "Jane", instruments = "Singer", image = "" });

            lView.ItemTemplate = ListDataTemplate;
            lView.ItemsSource = people;
            Content = lView;
        }

        private void BtnCellApprove_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button)
                {
                    var templateGrid = ((Button)sender);
                    //templateGrid.Parent = gridBase
                    //templateGrid.Parent.Parent = cell
                    if (templateGrid.Parent != null && templateGrid.Parent.Parent != null && templateGrid.Parent.Parent.BindingContext != null)
                    {
                        var deletedate = templateGrid.Parent.Parent.BindingContext;
                        User swipedUser = (User)deletedate;
                        people.Remove(swipedUser);
                        u.Yes.Add(swipedUser.Email);
                        updateUser();
                        if(swipedUser.Yes.Contains(u.Email))
                        {
                            var peeps = new List<string>() { u.Email, swipedUser.Email };
                            var room = new Room(peeps, (swipedUser.uname + ", " + u.uname));
                            AddRoomToRealtime(room);
                            App.Current.MainPage.DisplayAlert("It's match!", "Check your chat rooms to begin composing messages!", "Ok");
                        }
                            //start chat room
                        lView.ItemsSource = null;
                        lView.ItemsSource = people;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async void AddRoomToRealtime(Room room)
        {
            await firebaseHelper.CreateRoom(room);
        }

        private async void updateUser()
        {
            await firebaseHelper.UpdateUser(u);
        }

        private void GridTemplate_Tapped(object sender, EventArgs e)
        {
            var tappedUser = (User)(((SwipeGestureGrid)sender).Parent.Parent.BindingContext);
            //string url = "https:" + "//www.youtube.com/embed/" + tappedUser.yLink.Substring(tappedUser.yLink.IndexOf("v=")+2);
            //string html = @"<iframe width=""853"" height=""480"" src=""" + url + @""" frameborder=""0"" allow=""accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture"" allowfullscreen></iframe>";
            WebView webView = new WebView()
            {
                HeightRequest = App.Current.MainPage.Height,
                Source = tappedUser.yLink
            };
            webView.BackgroundColor = Color.Transparent;
            Button button = new Button()
            {
                Text = "Back",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };
            button.Clicked += OnButtonClicked;


            StackLayout stackLayout = new StackLayout() { Children = { webView, button } };

            App.Current.MainPage.Navigation.PushModalAsync(new ContentPage() { Content = stackLayout });
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void GridTemplate_SwipeLeft(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeGestureGrid)
                {
                    var templateGrid = ((SwipeGestureGrid)sender).Parent;
                    if (templateGrid != null && templateGrid is Grid)
                    {
                        var CellTemplateGrid = (Grid)templateGrid;
                        if(CellTemplateGrid.ColumnDefinitions[0].Width.Value == 120)
                            CellTemplateGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Absolute);
                        else
                            CellTemplateGrid.ColumnDefinitions[2].Width = new GridLength(120, GridUnitType.Absolute);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        private async void GetPossibleMatches()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "user.txt");
            string currentEmail = File.ReadAllText(fileName);
            u = await firebaseHelper.GetUserwithEmail(currentEmail);
            if (u.No == null)
                u.No = new List<string>();
            if (u.Yes == null)
                u.Yes = new List<string>();
            await firebaseHelper.UpdateUser(u);
            List<string> exclude = u.No.Concat(u.Yes).ToList();
            var lads = await firebaseHelper.GetAllUsersExceptNoList(exclude, u.Email);
            
            foreach (var p in lads)
            {
                people.Add(p);
            }
        }

        private void GridTemplate_SwipeRight(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeGestureGrid)
                {
                    var templateGrid = ((SwipeGestureGrid)sender).Parent;
                    if (templateGrid != null && templateGrid is Grid)
                    {
                        var CellTemplateGrid = (Grid)templateGrid;
                        if (CellTemplateGrid.ColumnDefinitions[2].Width.Value == 120)
                            CellTemplateGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Absolute);
                        else
                            CellTemplateGrid.ColumnDefinitions[0].Width = new GridLength(120, GridUnitType.Absolute);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void BtnCellDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button)
                {
                    var templateGrid = ((Button)sender);
                    //templateGrid.Parent = gridBase
                    //templateGrid.Parent.Parent = cell
                    if (templateGrid.Parent != null && templateGrid.Parent.Parent != null && templateGrid.Parent.Parent.BindingContext != null)
                    {
                        var deletedate = templateGrid.Parent.Parent.BindingContext;
                        people.Remove((User)deletedate);
                        u.No.Add(((User)deletedate).Email);
                        updateUser();
                        lView.ItemsSource = null;
                        lView.ItemsSource = people;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public class person
        {
            public string name { get; set; }
            public string instruments { get; set; }
            public string image { get; set; }
            public person()
            {

            }
        }
    }
}
