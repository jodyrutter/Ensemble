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
using Android.Gms.Tasks;
using Android.Support.Design.Widget;
using System.Collections.Generic;
using Android.Content;

namespace Ensemble.Droid
{
    [Activity(Label = "Ensemble", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Activity, IOnClickListener,IOnCompleteListener
    {
        Button btnLogin;
        EditText input_email, input_pwd;
        TextView btnSignUp, btnForgetPwd;
        RelativeLayout activity_main;
        public static FirebaseApp Myapp;
        FirebaseAuth auth;
        private Context context;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            InitFirebaseAuth();

            btnLogin = FindViewById<Button>(Resource.Id.login_btn_login);
            input_email = FindViewById<EditText>(Resource.Id.login_email);
            input_pwd = FindViewById<EditText>(Resource.Id.login_password);
            btnSignUp = FindViewById<TextView>(Resource.Id.login_btn_sign_up);
            btnForgetPwd = FindViewById<TextView>(Resource.Id.login_btn_forget_password);
            activity_main = FindViewById<RelativeLayout>(Resource.Id.activity_main);

            btnSignUp.SetOnClickListener(this);
            btnLogin.SetOnClickListener(this);
            btnForgetPwd.SetOnClickListener(this);
            /*
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());*/
        }

        private void InitFirebaseAuth()
        {
            FirebaseOptions options = new FirebaseOptions.Builder()
                .SetDatabaseUrl("https://ensemble-65b0c.firebaseio.com/")
                .SetApplicationId("1:800755725006:android:964d63a2924e415158969f")
                .SetApiKey("AIzaSyAJBWfWnAu-3pnw6VzHoQu17bZKghnZJOA")
                .Build();

            Boolean hasBeenInit = false;
            List<FirebaseApp> firebaseApps = FirebaseApp.GetApps(context);
            for (FirebaseApp app : firebaseApps)
            {
                if (app.getName().equals(FirebaseApp.DefaultAppName))
                {
                    hasBeenInit = true;
                    Myapp = app;
                }
            }
            

            if (!hasBeenInit)
            {
                Myapp = FirebaseApp.InitializeApp(this, options);
            }
            /*
            if (Myapp == null)
            {
                Myapp = FirebaseApp.InitializeApp(this, options);
                
            }*/
            auth = FirebaseAuth.GetInstance(Myapp);

        }
        /*public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }*/

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.login_btn_sign_up)
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
        }

        private void LoginUser(string email, string password)
        {
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
        }
    }
}