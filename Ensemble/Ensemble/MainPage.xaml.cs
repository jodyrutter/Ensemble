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

        FirebaseHelper firebaseHelper = new FirebaseHelper();
        List<User> users = new List<User>();
        public MainPage()
        {
            InitializeComponent();
        }
        /**
         * Runs when the sign in button is clicked.
         */

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var allUsers = await firebaseHelper.GetAllUsers();
            users = allUsers;
        }


        void OnSignIn(object sender, EventArgs args)
        {
            string user, pass; //Strings to contain the user's login credentials.
            user = username.Text;
            pass = password.Text;
        }

        private void SignUp(object sender, EventArgs e)
        {

        }

        private void Add_Clicked(object sender, EventArgs e)
        {

        }
    }
}
