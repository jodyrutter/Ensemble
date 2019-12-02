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
        SwitchCell groupCell;
        EntryCell youtubeURL;
        TextCell delete;
        TextCell labAcc;
        TextCell partInst;
        SwitchCell vocals;
        SwitchCell guitars;
        SwitchCell drum;
        SwitchCell basses;
        SwitchCell pianos;
        SwitchCell violins;
        SwitchCell synths;
        SwitchCell others;
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
            settings.group = groupCell.On;
            var i = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            i.setUserSettings(settings.youtube,settings.group,settings.vocal,settings.guitar,settings.drums,settings.bass,settings.piano,settings.violin,settings.synth,settings.other);
            await fh.UpdateUserSettings(i);
            //String a = youtubeURL.Text;
            //Not implemented yet.
        }
        async void ConnectControl() {
            var i = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            settings.group = i.lGroup;
            settings.youtube = i.yLink;
            settings.vocal = i.sVocal;
            settings.guitar = i.sGuitar;
            settings.drums = i.sDrum;
            settings.bass = i.sBass;
            settings.piano = i.sPiano;
            settings.violin = i.sViolins;
            settings.synth = i.sSynth;
            settings.other = i.sOther;
            TableView table;
            labAcc = new TextCell
            {
                Text = "Account Prefereces",
            };
            groupCell = new SwitchCell {
                Text = "I am in a group",
                On = i.lGroup,
            };
            groupCell.OnChanged += (object sender, ToggledEventArgs e) =>
            {
                groupCellChange(sender, e);
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
            partInst = new TextCell {
                Text = "Preferred Instrument of Partner"
            };
            vocals = new SwitchCell
            {
                Text = "Vocals",
                On = settings.vocal,
            };
            vocals.OnChanged += (object sender, ToggledEventArgs e) =>
            {
                vocalChange(sender, e);
            };
            guitars = new SwitchCell
            {
                Text = "Guitar",
                On = settings.guitar,
            };
            guitars.OnChanged += (object sender, ToggledEventArgs e) =>
            {
                guitarChange(sender, e);
            };
            drum = new SwitchCell
            {
                Text = "Drums",
                On = settings.drums,
            };
            drum.OnChanged += (object sender, ToggledEventArgs e) =>
            {
                drumChange(sender, e);
            };
            basses = new SwitchCell
            {
                Text = "Bass",
                On = settings.bass,
            };
            basses.OnChanged += (object sender, ToggledEventArgs e) =>
            {
                bassChange(sender, e);
            };
            pianos = new SwitchCell
            {
                Text = "Piano",
                On = settings.piano,
            };
            pianos.OnChanged += (object sender, ToggledEventArgs e) =>
            {
                pianoChange(sender, e);
            };
            violins = new SwitchCell
            {
                Text = "Violin",
                On = settings.violin,
            };
            violins.OnChanged += (object sender, ToggledEventArgs e) =>
            {
                violinChange(sender, e);
            };
            synths = new SwitchCell
            {
                Text = "Synth",
                On = settings.synth,
            };
            synths.OnChanged += (object sender, ToggledEventArgs e) =>
            {
                synthChange(sender, e);
            };
            others = new SwitchCell
            {
                Text = "Other",
                On = settings.other,
            };
            others.OnChanged += (object sender, ToggledEventArgs e) =>
            {
                otherChange(sender, e);
            };
            table = new TableView {
                Root = new TableRoot
                {
                    new TableSection{
                        labAcc,
                        groupCell,
                        youtubeURL,
                        //Removed delete do to issues.
                        partInst,
                        vocals,
                        guitars,
                        drum,
                        basses,
                        pianos,
                        violins,
                        synths,
                        others,
                    }
                }
            };
            //table.VerticalOptions = LayoutOptions.FillAndExpand;
            MainLayout.Children.Add(table);
        }
        private void groupCellChange(object sender, ToggledEventArgs e) {
            settings.group = e.Value;
        }
        private void vocalChange(object sender, ToggledEventArgs e)
        {
            settings.vocal = e.Value;
        }
        private void guitarChange(object sender, ToggledEventArgs e)
        {
            settings.guitar = e.Value;
        }
        private void drumChange(object sender, ToggledEventArgs e)
        {
            settings.drums = e.Value;
        }
        private void bassChange(object sender, ToggledEventArgs e)
        {
            settings.bass = e.Value;
        }
        private void pianoChange(object sender, ToggledEventArgs e)
        {
            settings.piano = e.Value;
        }
        private void violinChange(object sender, ToggledEventArgs e)
        {
            settings.violin = e.Value;
        }
        private void synthChange(object sender, ToggledEventArgs e)
        {
            settings.synth = e.Value;
        }
        private void otherChange(object sender, ToggledEventArgs e)
        {
            settings.other = e.Value;
        }
        protected override async void OnDisappearing() {
            base.OnDisappearing();
            saveSettings();
        }
    }
}