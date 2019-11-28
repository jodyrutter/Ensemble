using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Database;
namespace Ensemble.Droid
{
    [Activity(Label = "Chat")]
    public class Chat : AppCompatActivity
    {
        FirebaseHelper fh = new FirebaseHelper();

        Room room;
        private EditText msg;
        private TextView roomname;
        private FloatingActionButton fab;
        private RelativeLayout rel;
        private async void GetRoom(string name)
        {
            string uemail = FirebaseAuth.Instance.CurrentUser.Email;
            var item = await fh.GetRoom(uemail, name);
            room.Name = item.Name;
            room.ChatLog = item.ChatLog;
            room.lastMsg = item.lastMsg;
            room.participants = item.participants;
            


        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.AppTheme);
            SetContentView(Resource.Layout.Chatty1);

            string text = Intent.GetStringExtra("Room") ?? "Data is not available";
            //Toast.MakeText(this, text, ToastLength.Long).Show();
            room = new Room();
            GetRoom(text);
            Toast.MakeText(this, room.Name, ToastLength.Long).Show();

            fab = FindViewById<FloatingActionButton>(Resource.Id.fab1);
            roomname = FindViewById<TextView>(Resource.Id.roomName);
            msg = FindViewById<EditText>(Resource.Id.input_msg);
            rel = FindViewById<RelativeLayout>(Resource.Id.real1);

            roomname.Text = text;
        }
    }
}