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
        //Initialize variables
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
        
        //Get User from database based off of FirebaseAuth.CurrentUser and set it as u
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

            //Get room name from previous activity
            text = Intent.GetStringExtra("Room") ?? "Data is not available";
            Toast.MakeText(this, text, ToastLength.Long).Show();

            //Set Views to variables
            rel = FindViewById<RelativeLayout>(Resource.Id.real1);
            RoomName = FindViewById<TextView>(Resource.Id.roomName);
            refresh = FindViewById<Button>(Resource.Id.refresh_btn_chat);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab1);
            msg = FindViewById<EditText>(Resource.Id.input_msg);
            listMsg = FindViewById<ListView>(Resource.Id.list_msg);

            testChatLog = new List<MessageContent>();
            fh = new FirebaseHelper();

            //Set name of room
            RoomName.Text = text;
            
            //Get User and Room information
            GetUser();
            GetRoom();

            //Set Button clicks to buttons
            fab.Click += Fab_Click;
            refresh.Click += Refresh_Click;

            //Show the messages
            DisplayChatMessages();
        }

        //Upon refresh click, show the messages
        private void Refresh_Click(object sender, EventArgs e)
        {
            DisplayChatMessages();
        }

        //Upon fab button click, post message
        private void Fab_Click(object sender, EventArgs e)
        {
            PostMessage();
        }

        //Create message and post to Firebase Database
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
        //Get room from Firebase Database based on name of room and user's email
        private async void GetRoom()
        {
            r = new Room();
            var item = await fh.GetRoom(FirebaseAuth.Instance.CurrentUser.Email, text);
            r = item;
        }

        //Show the messages in the Firebase Database
        private async void DisplayChatMessages()
        {
            testChatLog.Clear();
            var item = await fh.GetRoom(FirebaseAuth.Instance.CurrentUser.Email, text);
            int index = 0;
            foreach (var i in item.ChatLog)
                testChatLog.Add(item.ChatLog[index++]);

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