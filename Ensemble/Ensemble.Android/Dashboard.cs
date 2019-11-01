using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using static Android.Views.View;
using Ensemble;

namespace Ensemble.Droid
{
    [Activity(Label = "DashBoard", Theme = "@style/AppTheme")]
    public class Dashboard : AppCompatActivity, IOnClickListener, IOnCompleteListener
    {

        //Initialize variables
        TextView txtWelcome;
        EditText input_new_pwd;
        Button btnChangePwd, btnLogOut;
        User u;
        Button btnDelete;
        Button btnProfile;
        Button btnMessages;
        RelativeLayout activity_dashboard;
        FirebaseAuth auth;
        FirebaseHelper fh = new FirebaseHelper();

        //Initialize button presses
        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.dashboard_btn_change_pass)
                ChangePassword(input_new_pwd.Text);
            else if (v.Id == Resource.Id.dashboard_btn_logout)
                LogoutUser();
            else if (v.Id == Resource.Id.dashboard_btn_delete_pass)
                DeleteUser();
            else if (v.Id == Resource.Id.dashboard_btn_go_profile)
                Profile();
            else if (v.Id == Resource.Id.dashboard_btn_go_messages) ;
                //MessagesAsync();
        }

        //Logout user account
        private void LogoutUser()
        {
            auth.SignOut();
            if (auth.CurrentUser == null)
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            }
        }

        private void Profile()
        { 
            //To do later
        }

        private async System.Threading.Tasks.Task GetUser()
        {
            string email = auth.CurrentUser.Email;
            User x = await fh.GetUserwithEmail(email);

           GetUser2(u, x.Email, x.Pwd, x.uname, x.Age, x.ProfilePic, x.FavInstrument, x.ShortBio, x.yLink);


        }

        private void GetUser2(User u, string e, string p, string fname, int age, string profile, string instrument, string shortbio, string ylink)
        { 
            //User tmp = User(e, p, fname, age, profile, instrument, shortbio, ylink);
        }

        /*private async System.Threading.Tasks.Task MessagesAsync()
        {
            string email = auth.CurrentUser.Email;
            User c = await fh.GetUserwithEmail(email);
            

            
        }*/
        private void DeleteUser()
        {
            
            string em = auth.CurrentUser.Email;
            fh.DeleteUser(em);
            auth.SignOut();
            if (auth.CurrentUser == null)
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            }
        }

        //edit so it edits the password on user database
        //might have to move to other page (edit page)
        //Change password of user
        private void ChangePassword(string newpwd)
        {
            FirebaseUser user = auth.CurrentUser;
            user.UpdatePassword(newpwd)
                .AddOnCompleteListener(this);

            //Change password on Realtime database
            UpdatetoRealtime(user.Email, newpwd);
        }

        //Update email and password into Realtime database
        private async void UpdatetoRealtime(string email, string newpwd)
        {
            await fh.UpdateUser(email, newpwd);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DashBoard);
           
            //Init Firebase
            auth = FirebaseAuth.GetInstance(MainActivity.app);
           

            //View
            btnChangePwd = FindViewById<Button>(Resource.Id.dashboard_btn_change_pass);
            btnDelete = FindViewById<Button>(Resource.Id.dashboard_btn_delete_pass);
            btnProfile = FindViewById<Button>(Resource.Id.dashboard_btn_go_profile);
            btnMessages = FindViewById<Button>(Resource.Id.dashboard_btn_go_messages);
            txtWelcome = FindViewById<TextView>(Resource.Id.dashboard_welcome);
            btnLogOut = FindViewById<Button>(Resource.Id.dashboard_btn_logout);
            input_new_pwd = FindViewById<EditText>(Resource.Id.dashboard_newpassword);
            activity_dashboard = FindViewById<RelativeLayout>(Resource.Id.activity_dashboard);

            btnChangePwd.SetOnClickListener(this);
            btnDelete.SetOnClickListener(this);
            btnLogOut.SetOnClickListener(this);
            btnMessages.SetOnClickListener(this);

            //check session
            if (auth != null)
            {
                txtWelcome.Text = "Welcome , " + auth.CurrentUser.Email + "Favorite instrument";
            }
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful == true)
            {
                Snackbar s = Snackbar.Make(activity_dashboard, "Password has been changed!", Snackbar.LengthShort);
                s.Show();
            }
        }
    }
}