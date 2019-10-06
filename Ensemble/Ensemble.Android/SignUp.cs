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
        TextView btnLogin, btnForgetPass;                        //Login & Forget Password textview buttons
        EditText input_email, input_pwd;                         //Variables to input email and password  
        RelativeLayout activity_sign_up;                         //the xaml file variable for the class
        FirebaseAuth auth;                                       //Firebase auth variable
        string email;                                            //variable for email. will be replaced by User class
        string pwd;                                              //variable for password. will be replaced by user class                    
        TaskCompletionListener tcl = new TaskCompletionListener(); 
        FirebaseHelper fh = new FirebaseHelper();                           //FirebaseHelper variable


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
            btnLogin = FindViewById<TextView>(Resource.Id.signup_btn_login);
            btnForgetPass = FindViewById<TextView>(Resource.Id.signup_btn_forget_password); //Take out later maybe
            input_email = FindViewById<EditText>(Resource.Id.signup_email);
            input_pwd = FindViewById<EditText>(Resource.Id.signup_password);
            activity_sign_up = FindViewById<RelativeLayout>(Resource.Id.activity_sign_up);

            //Link button presses to functions
            btnSignup.Click += btnSignup_Click;
            btnLogin.Click += btnLogin_Click;
            btnForgetPass.Click += btnForgetPass_Click;
        }

        //Go to Main Activity
        private void btnLogin_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }

        //Go to Forget Password Activity
        private void btnForgetPass_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ForgetPassword));
            Finish();
        }

        //If Signup button is pressed
        private void btnSignup_Click(object sender, EventArgs e)
        {
            //link user input to email and pwd variables
            email = input_email.Text; 
            pwd = input_pwd.Text;

            //it is not an email if it does not contain @, so try again
            if (!email.Contains("@"))
            {
                Snackbar.Make(activity_sign_up, "Please enter a valid email", Snackbar.LengthShort).Show();
                return;
            }
            //if password does not contain 8 characters or more, try again
            else if (pwd.Length < 8)
            {
                Snackbar.Make(activity_sign_up, "Please enter a password up to 8 characters", Snackbar.LengthShort).Show();
                return;
            }
            //If email and password valid, register the user
            RegisterUser(email, pwd);
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
            await fh.AddUser(email, pwd);
            
           
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