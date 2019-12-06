using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Firebase.Auth;
using Firebase.Database;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Firebase;
using Java.Util;
using Java.Lang;
using Ensemble.Droid.Helpers;
using FR.Ganfra.Materialspinner;
using Android;

using static Android.Views.View;

namespace Ensemble.Droid
{
    [Activity(Label = "SignUp", Theme = "@style/AppTheme")]
    public class SignUp : AppCompatActivity
    {
        //Initialize Buttons
        Button btnSignup;                                        //Sign up button
        TextView btnLogin;
        
        //Initialize fields for creating account
        EditText input_email;
        EditText input_pwd;                         
        EditText input_username;
        EditText input_age;
        EditText input_bio;
        EditText input_youlink;
        
        RelativeLayout activity_sign_up;                         //the relativeLayout from the xml file
        FirebaseAuth auth;                                       //Firebase auth variable
        MaterialSpinner instrumentSpinner;                       //Spinner for instruments   
        List<string> instruments;                                //String of instruments user to choose from
        User test;                                               //test user to check if username is the same
        string favInstrument;                                    //favInstrument choice of user
        int num;                                                 //int used to parse string into an integer

        TaskCompletionListener tcl = new TaskCompletionListener();
        FirebaseHelper fh = new FirebaseHelper();                           //FirebaseHelper variable


        
        //Set up Spinner from Array defined in Resources.String
        private void SetUpSpinner()
        {
            instruments = new List<string>();

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.instrumentArray, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            instrumentSpinner.Adapter = adapter;
        }

        //If selection is made, favInstrument is selection
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            favInstrument = instrumentSpinner.GetItemAtPosition(e.Position).ToString();
            //Toast.MakeText(this, favInstrument, ToastLength.Long).Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SignUp);

            //Initiate Firebase onto Sign Up page
            auth = FirebaseAuth.GetInstance(MainActivity.app);
            
            ConnectControl();
            SetUpSpinner();
            
            
        }



        //Link initialized variables with controls on Sign Up xml
        void ConnectControl()
        {
            //Connecting views of xml to variables of cs file
            btnSignup = FindViewById<Button>(Resource.Id.signup_btn_register);
            btnLogin = FindViewById<TextView>(Resource.Id.signup_btn_login);
            input_email = FindViewById<EditText>(Resource.Id.signup_email);
            input_pwd = FindViewById<EditText>(Resource.Id.signup_password);
            input_username = FindViewById<EditText>(Resource.Id.signup_username);
            instrumentSpinner = FindViewById<MaterialSpinner>(Resource.Id.instrumentSpinner);
            input_age = FindViewById<EditText>(Resource.Id.signup_age);
            input_bio = FindViewById<EditText>(Resource.Id.signup_bio);
            input_youlink = FindViewById<EditText>(Resource.Id.signup_ylink);
            activity_sign_up = FindViewById<RelativeLayout>(Resource.Id.activity_sign_up);

            //Link button presses to functions
            btnSignup.Click += btnSignup_Click;
            btnLogin.Click += btnLogin_Click;

            //initialize spinner selection to class
            instrumentSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
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
            //if credentials are filled in correctly, register user, else error
            if (ValidateCredentials())
            {
                RegisterUser(input_email.Text, input_pwd.Text);
            }
            else
            {
                Snackbar.Make(activity_sign_up, "Please try again", Snackbar.LengthLong).Show();
            }


        }

        //Check the validations of every entry that is entered
        private  bool ValidateCredentials()
        {
            bool emailVal;
            bool passVal;
            bool unameVal;
            bool AgeVal;
            bool ProfileVal = true;
            bool FavVal;
            bool BioVal;
            bool YVal;


            //validation for email, making sure there is a '@'
            if (!input_email.Text.Contains("@"))
            {
                emailVal = false;
                Snackbar.Make(activity_sign_up, "Please enter a valid email", Snackbar.LengthLong).Show();
            }
            else
                emailVal = true;

            //validation for password 
            if (input_pwd.Text.Length < 8)
            {
                passVal = false;
                Snackbar.Make(activity_sign_up, "Please enter a password up to 8 characters", Snackbar.LengthLong).Show();
            }
            else
                passVal = true;

            //validation for username there needs to be some characters for username
            if (input_username.Text.Length == 0)
            {
                unameVal = false;
                Snackbar.Make(activity_sign_up, "Please enter a username", Snackbar.LengthLong).Show();
            }
            else
            {
                //Make sure username doesnt already exist. To do list
                //IsSameUsername();
                //if (test != null)
                //{
                //    unameVal = false;
                //    Snackbar.Make(activity_sign_up, "Username already taken. Try another username", Snackbar.LengthShort).Show();
                //}
                //else
                    unameVal = true;
            }

            //Validation for age
            if (input_age.Text.Length == 0)
            {
                AgeVal = false;
                Snackbar.Make(activity_sign_up, "Please enter your age", Snackbar.LengthLong).Show();
            }
            else
            {
                //entry in input_age is not integer, then error
                if (!int.TryParse(input_age.Text, out num))
                {
                    AgeVal = false;
                    Snackbar.Make(activity_sign_up, "Age entered invalid. Try again", Snackbar.LengthLong).Show();
                }
                else
                    AgeVal = true;
            }
         
            //Validation on Favorite instrument: if there is no selection for favinstrument, then error
            if (instrumentSpinner.SelectedItem == null || favInstrument == null)
            {
                FavVal = false;
                Snackbar.Make(activity_sign_up, "Please enter your Favorite instrument", Snackbar.LengthLong).Show();
            }
            else
                FavVal = true;

            //Validation for Short Bio: need entry for short bio
            if (input_bio.Text.Length == 0)
            {
                BioVal = false;
                Snackbar.Make(activity_sign_up, "Please enter short bio about yourself", Snackbar.LengthLong).Show();
            }
            else
            {
                //if bio is more than 250 words, error because too long
                if (input_bio.Text.Length > 250)
                {
                    BioVal = false;
                    Snackbar.Make(activity_sign_up, "Short Bio too long.", Snackbar.LengthLong).Show();
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
                YVal = true;
            }


            return (emailVal && passVal && unameVal && AgeVal && ProfileVal && FavVal && BioVal && YVal);
        }

        private async void IsSameUsername()
        {
            test = new User();
            test = await fh.GetUserwithUsername(input_username.Text);
        }

        //Create account with FirebaseAuth
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
            Snackbar.Make(activity_sign_up, "User Registration failed", Snackbar.LengthLong).Show();
        }
        
        //Add user credentials to Firebase Realtime database
        private void AddtoRealtime()
        {
            //turn age input into into
            int.TryParse(input_age.Text, out num);
            
            //encrypt password
            string encryptedPwd = CryptoEngine.Encrypt(input_pwd.Text);
            
            HashMap UserInfo = new HashMap();
            UserInfo.Put("Age", num);
            UserInfo.Put("Email", input_email.Text);
            UserInfo.Put("FavInstrument",favInstrument);
            UserInfo.Put("ProfilePic", "Picture");
            UserInfo.Put("Pwd", encryptedPwd);
            UserInfo.Put("ShortBio", input_bio.Text);
            UserInfo.Put("uname", input_username.Text);
            UserInfo.Put("yLink", input_youlink.Text);

            //Push hashmap to database under User child
            DatabaseReference dref = AppDataHelper.GetDatabase().GetReference("Users").Push();
            dref.SetValue(UserInfo);

            //Success message
            Snackbar.Make(activity_sign_up, "Account added to Realtime Database", Snackbar.LengthLong).Show();


        }
        //Upon success, Add to Realtime database & go to Main Activity page
        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            Snackbar.Make(activity_sign_up, "User Registration Success", Snackbar.LengthLong).Show();
            AddtoRealtime();

            StartActivity(typeof(MainActivity));
            Finish();

        }

       
    }
}
