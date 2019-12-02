using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ensemble.Droid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public static string SettingsScreenTitle = "Settings";
        private FirebaseHelper fh;
        SettingsA settings;
        EntryCell youtubeURL;
        TextCell delete;
        TextCell labAcc;
        public Settings()
        {
            InitializeComponent();
            settings = new SettingsA();
            fh = new FirebaseHelper();
            ConnectControl();
            Title = SettingsScreenTitle;
        }
        async private void saveSettings() {
            settings.youtube = youtubeURL.Text;
            var i = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            i.yLink = settings.youtube;
            await fh.UpdateUser(i);
            //String a = youtubeURL.Text;
            //Not implemented yet.
        }
        async void ConnectControl() {
            var i = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            settings.youtube = i.yLink;
            TableView table;
            labAcc = new TextCell
            {
                Text = "Account Prefereces",
            };
            youtubeURL = new EntryCell {
                Placeholder = "YouTube video of performance",
                Text = settings.youtube,
            };
            delete = new TextCell
            {
                Text = "Delete Account",
            };
            delete.Tapped += async (object sender, EventArgs e) =>
            {
                await fh.DeleteUser(i.Email);
            };
            table = new TableView {
                Root = new TableRoot
                {
                    new TableSection{
                        labAcc,
                        youtubeURL,
                        //Removed delete do to issues.
                    }
                }
            };
            table.VerticalOptions = LayoutOptions.FillAndExpand;
            MainLayout.Children.Add(table);
        }
        protected override async void OnDisappearing() {
            base.OnDisappearing();
            saveSettings();
        }
    }
}