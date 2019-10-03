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
//using Firebase.Database;
//using Firebase.FirebaseOptions;

namespace Ensemble.Droid
{
    [Activity(Label = "Ensemble", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AppCompatActivity
    {
        Button btnLogin;
        EditText input_email, input_pwd;
        TextView btnSignUp, btnForgetPwd;
        RelativeLayout activity_main;
        
        FirebaseAuth auth;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            

            btnLogin = FindViewById<Button>(Resource.Id.login_btn_login);
            input_email = FindViewById<EditText>(Resource.Id.login_email);
            input_pwd = FindViewById<EditText>(Resource.Id.login_password);
            btnSignUp = FindViewById<TextView>(Resource.Id.login_btn_sign_up);
            btnForgetPwd = FindViewById<TextView>(Resource.Id.login_btn_forget_password);

            activity_main = FindViewById<RelativeLayout>(Resource.Id.activity_main);

            btnLogin.Click += LoginButton_Click;
            btnSignUp.Click += BtnSignUp_Click;
            btnForgetPwd.Click += BtnForgetPwd_Click;
            InitFirebaseAuth();


            //btnSignUp.SetOnClickListener(this);
            //btnLogin.SetOnClickListener(this);
            //btnForgetPwd.SetOnClickListener(this);
            /*
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());*/
        }

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SignUp));
        }

        private void BtnForgetPwd_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ForgetPassword));
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string em, p;

            em = input_email.Text;
            p = input_pwd.Text;

            if (!em.Contains("@"))
            {
                Snackbar.Make(activity_main, "Please provide valid email", Snackbar.LengthShort).Show();
                return;
            }

            else if (p.Length < 6)
            {
                Snackbar.Make(activity_main, "Please provide valid password", Snackbar.LengthShort).Show();
                return;
            }
            TaskCompletionListener taskCompletionListener = new TaskCompletionListener();
            taskCompletionListener.Success += TaskCompletionListener_Success;
            taskCompletionListener.Failure += TaskCompletionListener_Failure;

            auth.SignInWithEmailAndPassword(em, p)
                .AddOnSuccessListener(taskCompletionListener)
                .AddOnFailureListener(taskCompletionListener);

        }

        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(activity_main, "Login Failed", Snackbar.LengthShort).Show();
        }

        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            Snackbar.Make(activity_main, "Login Success", Snackbar.LengthShort).Show();
            StartActivity(typeof(Dashboard));
        }


        private void InitFirebaseAuth()
        {
            
            var app = FirebaseApp.InitializeApp(this);
            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetApplicationId("ensemble-65b0c")
                    .SetApiKey("AIzaSyD25wXdD1WxUQGQD3zkXNkf3X9UYJYaAtE")
                    .Build();

                app = FirebaseApp.InitializeApp(this, options);
                auth = FirebaseAuth.Instance;
            }
            else
            {
                auth = FirebaseAuth.Instance;
            }
            

        }
        /*public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }*/

       /* public void OnClick(View v)
        {
            if (v.Id == Resource.Id.login_btn_forget_password)
            {
                StartActivity(new Android.Content.Intent(this, typeof(ForgetPassword)));
                Finish();
            }

            else if (v.Id == Resource.Id.login_btn_sign_up)
            {
                StartActivity(new Android.Content.Intent(this, typeof(SignUp)));
                Finish();
            }
            
            else if (v.Id == Resource.Id.login_btn_login)
            {
                LoginUser(input_email.Text, input_pwd.Text);
            }
        }*/

        

        /*private void LoginUser(string email, string password)
        {
            if (!email.Contains("@"))
            {
                Snackbar.Make(activity_main, "Please provide valid email", Snackbar.LengthShort).Show();
                return;
            }

            else if (password.Length < 6)
            {
                Snackbar.Make(activity_main, "Please provide valid password", Snackbar.LengthShort).Show();
                return;
            }

            auth.SignInWithEmailAndPassword(email, password).AddOnCompleteListener(this);
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                StartActivity(new Android.Content.Intent(this, typeof(Dashboard)));
                Finish();
            }
            else
            {
                Snackbar sn = Snackbar.Make(activity_main, "Login Failed", Snackbar.LengthShort);
                sn.Show();
            }
        }*/

      
    }
}