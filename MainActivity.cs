using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase;
using Firebase.Auth;
using static Android.Views.View;
using Android.Support.V7.App;
using Android.Gms.Tasks;
using Android.Support.Design.Widget;
using System.Collections.Generic;
using Android.Content;
using System.Numerics;
using Firebase.Database;

namespace Ensemble.Droid
{
    [Activity(Label = "Ensemble", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AppCompatActivity
    {

        //initialize variables
        Button btnLogin;
        EditText input_email, input_pwd;
        TextView btnSignUp, btnForgetPwd;
        RelativeLayout activity_main;
        FirebaseAuth mAuth;
        
        public static FirebaseApp app;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Initialize application
            SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            
            //Initialize views
            btnLogin = FindViewById<Button>(Resource.Id.login_btn_login);
            input_email = FindViewById<EditText>(Resource.Id.login_email);
            input_pwd = FindViewById<EditText>(Resource.Id.login_password);
            btnSignUp = FindViewById<TextView>(Resource.Id.login_btn_sign_up);
            btnForgetPwd = FindViewById<TextView>(Resource.Id.login_btn_forget_password);
            activity_main = FindViewById<RelativeLayout>(Resource.Id.activity_main);

            //initialize button presses
            btnLogin.Click += LoginButton_Click;
            btnSignUp.Click += BtnSignUp_Click;
            btnForgetPwd.Click += BtnForgetPwd_Click;
            InitFirebaseAuth();
        }
        //Go to Sign Up page
        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SignUp));
        }
        //Go to Forgot Password Page
        private void BtnForgetPwd_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ForgetPassword));
        }
        //Try to log in
        private void LoginButton_Click(object sender, EventArgs e)
        {
            string em, p; 
           
            //link variables to user input
            em = input_email.Text;
            p = input_pwd.Text;

            //email invalid if no @ sign
            if (!em.Contains("@"))
            {
                Snackbar.Make(activity_main, "Please provide valid email", Snackbar.LengthShort).Show();
                return;
            }
            //password invalid if below 8 characters
            else if (p.Length < 8)
            {
                Snackbar.Make(activity_main, "Please provide valid password", Snackbar.LengthShort).Show();
                return;
            }
            //Initialize task completion listener and link to functions
            TaskCompletionListener taskCompletionListener = new TaskCompletionListener();
            taskCompletionListener.Success += TaskCompletionListener_Success;
            taskCompletionListener.Failure += TaskCompletionListener_Failure;

            //try to sign in with email and password
            mAuth.SignInWithEmailAndPassword(em, p)
                .AddOnSuccessListener(taskCompletionListener)
                .AddOnFailureListener(taskCompletionListener);

        }
        
        //Upon failure of logging in, try again
        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(activity_main, "Login Failed", Snackbar.LengthShort).Show();
        }
        
        //Upon success, go to Dashboard
        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            Snackbar.Make(activity_main, "Login Success", Snackbar.LengthShort).Show();
            StartActivity(typeof(ToXForms));
        }
        
        //Initialize Firebase Auth to application
        private void InitFirebaseAuth()
        {
            
            app = FirebaseApp.InitializeApp(this);

            var options = new Firebase.FirebaseOptions.Builder()
                    .SetApplicationId("ensemble-65b0c")
                    .SetApiKey("AIzaSyD25wXdD1WxUQGQD3zkXNkf3X9UYJYaAtE")
                    .SetDatabaseUrl("https://ensemble-65b0c.firebaseio.com")
                    .SetStorageBucket("ensemble-65b0c.appspot.com")
                    .Build();

            if (app == null)
            {
                app = FirebaseApp.InitializeApp(this, options);
            }
            mAuth = FirebaseAuth.GetInstance(app);
            Snackbar.Make(activity_main, "Firebase initialized", Snackbar.LengthShort).Show(); 
        }
    }
}