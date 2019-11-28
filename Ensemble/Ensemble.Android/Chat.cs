using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Database;
namespace Ensemble.Droid
{
    [Activity(Label = "Chat")]
    public class Chat : Activity
    {
        FirebaseHelper fh = new FirebaseHelper();

        Room room;
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
            SetContentView(Resource.Layout.Chatty1);

            string text = Intent.GetStringExtra("Room") ?? "Data is not available";
            //Toast.MakeText(this, text, ToastLength.Long).Show();
            room = new Room();
            GetRoom(text);
            Toast.MakeText(this, room.Name, ToastLength.Long).Show();

        }
    }
}