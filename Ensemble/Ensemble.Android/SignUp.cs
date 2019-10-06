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
        Button btnSignup;
        TextView btnLogin, btnForgetPass;
        EditText input_email, input_pwd;
        RelativeLayout activity_sign_up;
        FirebaseAuth auth;
        string email;
        string pwd;
        FirebaseDatabase db;
        TaskCompletionListener tcl = new TaskCompletionListener();
        FirebaseHelper fh = new FirebaseHelper();

        ISharedPreferences preferences = Application.Context.GetSharedPreferences("userinfo", FileCreationMode.Private);
        ISharedPreferencesEditor editor;

        /*public void OnClick(View v)
        {
            if (v.Id == Resource.Id.signup_btn_login)
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            }
            //most likely take out
            else if (v.Id == Resource.Id.signup_btn_forget_password)
            {
                StartActivity(new Intent(this, typeof(ForgetPassword)));
                Finish();
            }
            //Edit this to create variable class, add to user database and more
            else if (v.Id == Resource.Id.signup_btn_register)
            {
                SignUpUser(input_email.Text, input_pwd.Text);
               
            }
        }

        private void SignUpUser(string email, string password)
        {
            auth.CreateUserWithEmailAndPassword(email, password).AddOnCompleteListener(this, this);
        }*/
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SignUp);

            //Init Firebase
            //InitFirebase();
            auth = FirebaseAuth.GetInstance(MainActivity.app);
            db = FirebaseDatabase.GetInstance(MainActivity.app);
            ConnectControl();
        }

       /* private void InitFirebase()
        {

            var app = FirebaseApp.InitializeApp(this);

            var options = new FirebaseOptions.Builder()
                    .SetApplicationId("ensemble-65b0c")
                    .SetApiKey("AIzaSyD25wXdD1WxUQGQD3zkXNkf3X9UYJYaAtE")
                    .SetDatabaseUrl("https://ensemble-65b0c.firebaseio.com")
                    .SetStorageBucket("ensemble-65b0c.appspot.com")
                    .Build();

            var instance = FirebaseAuth.GetInstance(app);
            if (instance == null)
            {
                instance = new FirebaseAuth(app);
            }

            if (app == null)
            {
                /*var options = new FirebaseOptions.Builder()
                    .SetApplicationId("ensemble-65b0c")
                    .SetApiKey("AIzaSyD25wXdD1WxUQGQD3zkXNkf3X9UYJYaAtE")
                    .SetDatabaseUrl("https://ensemble-65b0c.firebaseio.com")
                    .SetStorageBucket("ensemble-65b0c.appspot.com")
                    .Build();
                    
                app = FirebaseApp.InitializeApp(this, options, "Ensemble");
                //mAuth = FirebaseAuth.Instance;
            }

            auth = instance;


            //mAuth = FirebaseAuth.GetInstance(app);
            //Snackbar.Make(activity_sign_up, "Firebase initialized", Snackbar.LengthShort).Show();



        }*/


        void ConnectControl()
        {
            //views
            btnSignup = FindViewById<Button>(Resource.Id.signup_btn_register);
            btnLogin = FindViewById<TextView>(Resource.Id.signup_btn_login);
            btnForgetPass = FindViewById<TextView>(Resource.Id.signup_btn_forget_password); //Take out later
            input_email = FindViewById<EditText>(Resource.Id.signup_email);
            input_pwd = FindViewById<EditText>(Resource.Id.signup_password);
            activity_sign_up = FindViewById<RelativeLayout>(Resource.Id.activity_sign_up);

            btnSignup.Click += btnSignup_Click;
            btnLogin.Click += btnLogin_Click;
            btnForgetPass.Click += btnForgetPass_Click;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }

        private void btnForgetPass_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ForgetPassword));
            Finish();
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            email = input_email.Text;
            pwd = input_pwd.Text;

            if (!email.Contains("@"))
            {
                Snackbar.Make(activity_sign_up, "Please enter a valid email", Snackbar.LengthShort).Show();
                return;
            }
            else if (pwd.Length < 8)
            {
                Snackbar.Make(activity_sign_up, "Please enter a password up to 8 characters", Snackbar.LengthShort).Show();
                return;
            }
            RegisterUser(email, pwd);
        }

        void RegisterUser(string email, string pass)
        {
            tcl.Success += TaskCompletionListener_Success;
            tcl.Failure += TaskCompletionListener_Failure;

            auth.CreateUserWithEmailAndPassword(email, pass)
                .AddOnSuccessListener(tcl)
                .AddOnFailureListener(tcl);

        }

        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(activity_sign_up, "User Registration failed", Snackbar.LengthShort).Show();
        }
        private async void AddtoRealtime()
        {
            await fh.AddUser(email, pwd);
            
            //await DisplayAlert("Success", "Person Added Successfully", "OK");
        }
        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            Snackbar.Make(activity_sign_up, "User Registration Success", Snackbar.LengthShort).Show();
            AddtoRealtime();
            

            /*HashMap userMap = new HashMap();
            userMap.Put("email", email);
            userMap.Put("password", pwd);


            DatabaseReference userRef = db.GetReference("users/" + auth.CurrentUser.Uid);
            userRef.SetValue(userMap);
            */
            StartActivity(typeof(MainActivity));
            Finish();
            //Database work integration
        }
        /*
        void SaveToSharedPreference()
        {

            editor = preferences.Edit();

            editor.PutString("email", email);
            editor.PutString("password", pwd);
            

            editor.Apply();

        }

        void RetriveData()
        {
            string email = preferences.GetString("email", "");
        }
        */

        /* public void OnComplete(Task task)
         {
             if (task.IsSuccessful == true)
             {
                 Snackbar sn = Snackbar.Make(activity_sign_up, "Register Successfully ", Snackbar.LengthShort);
                 sn.Show();
             }
             else
             {
                 Snackbar sn = Snackbar.Make(activity_sign_up, "Register Failed! ", Snackbar.LengthShort);
                 sn.Show();
             }
         }*/
    }
}