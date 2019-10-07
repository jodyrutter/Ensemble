using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace Ensemble
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        //UI is accessable with their real names in this script. For example, the username is just username.
        public MainPage()
        {
            InitializeComponent();
        }
        /**
         * Runs when the sign in button is clicked.
         */
        void OnSignIn(object sender, EventArgs args)
        {
            string user, pass; //Strings to contain the user's login credentials.
            user = username.Text;
            pass = password.Text;
            //TODO if user is a real user
            App.Current.MainPage = new LandingPage();
        }
        void OnSignUp(object sender, EventArgs args)
        {
            Navigation.PushModalAsync(new Page1());
        }
    }
}
