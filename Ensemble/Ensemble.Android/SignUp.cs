using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Firebase.Auth;
using static Android.Views.View;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;

namespace Ensemble.Droid
{
    [Activity(Label = "SignUp", Theme = "@style/AppTheme")]
    public class SignUp : Activity, IOnClickListener, IOnCompleteListener
    {
        Button btnSignup;
        TextView btnLogin, btnForgetPass;
        EditText input_email, input_pwd;
        RelativeLayout activity_sign_up;
        FirebaseAuth auth;

        public void OnClick(View v)
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
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SignUp);

            //Init Firebase
            auth = FirebaseAuth.GetInstance(MainActivity.Myapp);

            //views
            btnSignup = FindViewById<Button>(Resource.Id.signup_btn_register);
            btnLogin = FindViewById<Button>(Resource.Id.signup_btn_login);
            btnForgetPass = FindViewById<Button>(Resource.Id.signup_btn_forget_password); //Take out later
            input_email = FindViewById<EditText>(Resource.Id.signup_email);
            input_pwd = FindViewById<EditText>(Resource.Id.signup_password);
            activity_sign_up = FindViewById<RelativeLayout>(Resource.Id.activity_sign_up);

            btnLogin.SetOnClickListener(this);
            btnSignup.SetOnClickListener(this);
            btnForgetPass.SetOnClickListener(this);
        }

        public void OnComplete(Task task)
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
        }
    }
}