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
using Firebase;
namespace Ensemble.Droid
{
    [Activity(Label = "Chat")]
    public class Chat : AppCompatActivity, IValueEventListener
    {
        String text;
        RelativeLayout rel;
        TextView RoomName;
        Button refresh;
        FloatingActionButton fab;
        EditText msg;
        ListView listMsg;
        FirebaseHelper fh;
        List<MessageContent> testChatLog;
        ListViewAdapter lva;
        User u;
        Room r;
        private async void GetUser()
        {
            u = new User();
            var item = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            u = item;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.AppTheme);
            SetContentView(Resource.Layout.Chatty1);

            text = Intent.GetStringExtra("Room") ?? "Data is not available";
            Toast.MakeText(this, text, ToastLength.Long).Show();

            rel = FindViewById<RelativeLayout>(Resource.Id.real1);
            RoomName = FindViewById<TextView>(Resource.Id.roomName);
            refresh = FindViewById<Button>(Resource.Id.refresh_btn_chat);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab1);
            msg = FindViewById<EditText>(Resource.Id.input_msg);
            listMsg = FindViewById<ListView>(Resource.Id.list_msg);

            testChatLog = new List<MessageContent>();
            fh = new FirebaseHelper();

            RoomName.Text = text;
            GetUser();
            GetRoom();
            fab.Click += Fab_Click;
            refresh.Click += Refresh_Click;
            DisplayChatMessages();
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            DisplayChatMessages();
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            PostMessage();
        }

        private async void PostMessage()
        {
            if (msg.Text.Length == 0)
                return;
            else
            {
                MessageContent temp = new MessageContent(u.Email, msg.Text);

                testChatLog.Add(temp);
                await fh.UpdateRoom(r.Name, r.participants, testChatLog, temp);

                msg.Text = "";

                DisplayChatMessages();
            }
            
        }
        private async void GetRoom()
        {
            r = new Room();
            var item = await fh.GetRoom(FirebaseAuth.Instance.CurrentUser.Email, text);
            r = item;
        }
        private async void DisplayChatMessages()
        {
            testChatLog.Clear();
            var item = await fh.GetRoom(FirebaseAuth.Instance.CurrentUser.Email, text);
            int index = 0;
            foreach (var i in item.ChatLog)
                testChatLog.Add(item.ChatLog[index++]);
            //testChatLog.Add(new MessageContent("", "Test"));

            lva = new ListViewAdapter(this, testChatLog);
            listMsg.Adapter = lva;
            
        }

        public void OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            throw new NotImplementedException();
        }
    }
}