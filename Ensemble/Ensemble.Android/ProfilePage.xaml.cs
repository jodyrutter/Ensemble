using Android.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Auth;
using Ensemble.Droid;

namespace Ensemble
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer

    public partial class ProfilePage : ContentView
    {
        FirebaseAuth auth;
	    public ProfilePage()
        {
            InitializeComponent();
            auth = FirebaseAuth.Instance;
        }

        //Upon click, navigates to Settings page
        [Obsolete]
        async void navSettings(object sender, EventArgs e) {
            var nextPage = new NavigationPage(new Ensemble.Droid.Settings());
            await Navigation.PushModalAsync(nextPage);
        }

        //Upon click, logs user out of account and navigates them to the Log In Screen
        [Obsolete]
        private void LogOut(object sender, EventArgs e)
        {
            
            auth.SignOut();
            //if current user is null, go back to main activity
            if (auth.CurrentUser == null)
            {
                var intent = new Intent(Forms.Context, typeof(MainActivity));
                Forms.Context.StartActivity(intent);
                
            }
        }
    }
}
