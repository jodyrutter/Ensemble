using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ensemble;
using Android.Content;

namespace Ensemble.Droid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        //initialize variables 
        public static string SettingsScreenTitle = "Settings";
        private FirebaseHelper fh;
        SettingsA settings;
        EntryCell age;
        EntryCell favInstrument;
        EntryCell bio;
        EntryCell youtubeURL;
        TextCell save;
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
        //Allows user to save changes made to account
        async private void saveSettings() {
            settings.youtube = youtubeURL.Text;
            try
            {
                settings.age = Int32.Parse(age.Text);
            }
            catch (FormatException) {
                //Age not an int
            }
            settings.bio = bio.Text;
            settings.favInst = favInstrument.Text;
            var i = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            i.yLink = settings.youtube;
            i.ShortBio = settings.bio;
            i.Age = settings.age;
            i.FavInstrument = settings.favInst;
            await fh.UpdateUser(i);

        }
        async void ConnectControl() {
            var i = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            settings.youtube = i.yLink;
            settings.age = i.Age;
            settings.bio = i.ShortBio;
            settings.favInst = i.FavInstrument;
            TableView table;
            labAcc = new TextCell
            {
                Text = "Account Preferences",
            };
            youtubeURL = new EntryCell {
                Placeholder = "YouTube video of performance",
                Text = settings.youtube,
            };
            age = new EntryCell
            {
                Placeholder = "Age",
                Text = settings.age.ToString(),
            };
            bio = new EntryCell
            {
                Placeholder = "Enter short bio",
                Text = settings.bio,
            };
            favInstrument = new EntryCell
            {
                Placeholder = "Enter favorite instrument",
                Text = settings.favInst,
            };
            delete = new TextCell
            {
                Text = "Delete Account",
            };
            delete.Tapped += async (object sender, EventArgs e) =>
            {
                await fh.DeleteUser(i.Email);
            };
            save = new TextCell {
                Text = "Save",
            };
            save.Tapped += (object sender, EventArgs e) =>
            {
                Save_Settings(sender, e);
            };
            table = new TableView {
                Root = new TableRoot
                {
                    new TableSection{
                        labAcc,
                        youtubeURL,
                        age,
                        bio,
                        favInstrument,
                        save,
                    }
                }
            };
            table.VerticalOptions = LayoutOptions.FillAndExpand;
            MainLayout.Children.Add(table);
        }
        //save changes upon closing application
        protected override async void OnDisappearing() {
            base.OnDisappearing();
            saveSettings();
        }
        //save changes upon pressing save button
        private void Save_Settings(object sender, EventArgs e)
        {
            saveSettings();
        }
    }
}