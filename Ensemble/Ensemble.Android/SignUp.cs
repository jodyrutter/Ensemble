using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Firebase.Auth;
using Firebase.Database;
using static Android.Views.View;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Firebase;
using Java.Util;
using Java.Lang;

//using Firebase.Database;

namespace Ensemble.Droid
{
    [Activity(Label = "SignUp", Theme = "@style/AppTheme")]
    public class SignUp : AppCompatActivity
    {
        //Initialize all variables
        Button btnSignup;                                        //Sign up button
        //TextView btnLogin;                       //Login & Forget Password textview buttons
        EditText input_email;
        EditText input_pwd;                         //Variables to input email and password
        EditText input_username;
        EditText input_favInstrument;
        EditText input_age;
        EditText input_bio;
        EditText input_youlink;
        RelativeLayout activity_sign_up;                         //the xaml file variable for the class
        FirebaseAuth auth;                                       //Firebase auth variable
        int num;
        TaskCompletionListener tcl = new TaskCompletionListener();
        FirebaseHelper fh = new FirebaseHelper();                           //FirebaseHelper variable
        List<string> ye = null;
        List<string> nay = null;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SignUp);

            //Initiate Firebase onto Sign Up page
            auth = FirebaseAuth.GetInstance(MainActivity.app);
            //db = FirebaseDatabase.GetInstance(MainActivity.app);
            ConnectControl();
        }



        //Link initialized variables with controls on Sign Up xml
        void ConnectControl()
        {
            //views
            btnSignup = FindViewById<Button>(Resource.Id.signup_btn_register);
            //btnLogin = FindViewById<TextView>(Resource.Id.signup_btn_login);
            //Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);
            input_email = FindViewById<EditText>(Resource.Id.signup_email);
            input_pwd = FindViewById<EditText>(Resource.Id.signup_password);
            input_username = FindViewById<EditText>(Resource.Id.signup_username);
            input_favInstrument = FindViewById<EditText>(Resource.Id.signup_favInstrument);
            input_age = FindViewById<EditText>(Resource.Id.signup_age);
            input_bio = FindViewById<EditText>(Resource.Id.signup_bio);
            input_youlink = FindViewById<EditText>(Resource.Id.signup_ylink);
            activity_sign_up = FindViewById<RelativeLayout>(Resource.Id.activity_sign_up);

            //Link button presses to functions
            btnSignup.Click += btnSignup_Click;
            //btnLogin.Click += btnLogin_Click;

            //spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            //var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.instrument_array, Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //spinner.Adapter = adapter;

        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner s = (Spinner)sender;
            string toast = string.Format("The Fav Instrument is {0}", s.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

        //Go to Main Activity
        private void btnLogin_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }


        //If Signup button is pressed
        private void btnSignup_Click(object sender, EventArgs e)
        {
            if (ValidateCredentials())
            {
                RegisterUser(input_email.Text, input_pwd.Text);
            }
            else
            {
                Snackbar.Make(activity_sign_up, "Please try again", Snackbar.LengthShort).Show();
            }


        }

        private bool ValidateCredentials()
        {
            bool emailVal;
            bool passVal;
            bool unameVal;
            bool AgeVal;
            bool ProfileVal = true;
            bool FavVal;
            bool BioVal;
            bool YVal;


            //validation for email
            if (!input_email.Text.Contains("@"))
            {
                emailVal = false;
                Snackbar.Make(activity_sign_up, "Please enter a valid email", Snackbar.LengthShort).Show();
            }
            else
                emailVal = true;

            //validation for password
            if (input_pwd.Text.Length < 8)
            {
                passVal = false;
                Snackbar.Make(activity_sign_up, "Please enter a password up to 8 characters", Snackbar.LengthShort).Show();
            }
            else
                passVal = true;

            //validation for username
            if (input_username.Text.Length == 0)
            {
                unameVal = false;
                Snackbar.Make(activity_sign_up, "Please enter a username", Snackbar.LengthShort).Show();
            }
            else
            {
                //Make sure username doesnt already exist. To do list
                /*if (fh.GetUserwithUsername(input_username.Text) != null)
                {
                    unameVal = false;
                    Snackbar.Make(activity_sign_up, "Username already taken. Try another username", Snackbar.LengthShort).Show();
                }
                else*/
                unameVal = true;
            }

            //Validation for age
            if (input_age.Text.Length == 0)
            {
                AgeVal = false;
                Snackbar.Make(activity_sign_up, "Please enter your age", Snackbar.LengthShort).Show();
            }
            else
            {
                if (!int.TryParse(input_age.Text, out num))
                {
                    AgeVal = false;
                    Snackbar.Make(activity_sign_up, "Age entered invalid. Try again", Snackbar.LengthShort).Show();
                }
                else
                    AgeVal = true;
            }

            //Validation for profile pic
            //Will be put in later

            //Validation on Favorite instrument
            if (input_favInstrument.Text.Length == 0)
            {
                FavVal = false;
                Snackbar.Make(activity_sign_up, "Please enter your Favorite instrument", Snackbar.LengthShort).Show();
            }
            else
                FavVal = true;

            //Validation for Short Bio
            if (input_bio.Text.Length == 0)
            {
                BioVal = false;
                Snackbar.Make(activity_sign_up, "Please enter short bio about yourself", Snackbar.LengthShort).Show();
            }
            else
            {
                if (input_bio.Text.Length > 250)
                {
                    BioVal = false;
                    Snackbar.Make(activity_sign_up, "Short Bio too long.", Snackbar.LengthShort).Show();
                }
                else
                    BioVal = true;
            }

            //Validation for Youtube Link
            if (input_youlink.Text.Length == 0)
            {
                YVal = false;
                Snackbar.Make(activity_sign_up, "Youtube link not there.", Snackbar.LengthShort).Show();
            }
            else
            {
                //Will put in validation of youtube link later
                YVal = true;
            }


            return (emailVal && passVal && unameVal && AgeVal && ProfileVal && FavVal && BioVal && YVal);
        }

        void RegisterUser(string email, string pass)
        {
            //link the task Completion listener to functions
            tcl.Success += TaskCompletionListener_Success;
            tcl.Failure += TaskCompletionListener_Failure;

            //create the users using Firebase Auth and go to 1 of the two functions depending on success or failure
            auth.CreateUserWithEmailAndPassword(email, pass)
                .AddOnSuccessListener(tcl)
                .AddOnFailureListener(tcl);

        }

        //Upon failure, try again
        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(activity_sign_up, "User Registration failed", Snackbar.LengthShort).Show();
        }
        //Add user to Realtime database
        private async void AddtoRealtime()
        {
            int.TryParse(input_age.Text, out num);

            await fh.AddUser(input_email.Text, input_pwd.Text, input_username.Text, num, "Picture", input_favInstrument.Text, input_bio.Text, input_youlink.Text, ye, nay);
            Snackbar.Make(activity_sign_up, "Account added to Realtime Database", Snackbar.LengthShort).Show();


        }
        //Upon success, Add to Realtime database & go to Main Activity page
        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            Snackbar.Make(activity_sign_up, "User Registration Success", Snackbar.LengthShort).Show();
            AddtoRealtime();

            StartActivity(typeof(MainActivity));
            Finish();

        }
    }
}