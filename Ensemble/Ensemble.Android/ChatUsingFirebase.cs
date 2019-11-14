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
    [Activity(Label = "ChatUsingFirebase")]
    public class ChatUsingFirebase : AppCompatActivity, IValueEventListener
    {
        private FirebaseHelper fh = new FirebaseHelper();

        private List<MessageContent> lstmessages;
        private User myuser;
        private User recipient;
        private ListView lstChat;
        private EditText edtChat;
        private FloatingActionButton fab;
        private RelativeLayout cuf;

        public int MyResultCode = 1;

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public async void GetUserFromAuth()
        {
            //User(string e, string p, string fname, int age, string profile, string instrument, string shortbio, string ylink, List<string> yes, List<string> no)
            var i = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            myuser = i;
        }

        public async void GetRecipientFromRealtime()
        {
            var i = await fh.GetUserwithEmail("Testbunny@test.com");
            recipient = i;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            

            SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Messages);

            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            edtChat = FindViewById<EditText>(Resource.Id.input);
            lstChat = FindViewById<ListView>(Resource.Id.list_of_messages);
            cuf = FindViewById<RelativeLayout>(Resource.Id.activity_messages);

            GetUserFromAuth();
            fab.Click += delegate
            {
                PostMessage();
            };


            DisplayChatMessages();

        }

        private async void PostMessage()
        {
            await fh.AddToChat(myuser, recipient, edtChat.Text);
            edtChat.Text = "";
            DisplayChatMessages();
        }

        public void OnCancelled(DatabaseError error)
        { 
        
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            DisplayChatMessages();
        }

        private async void DisplayChatMessages()
        {
            lstmessages.Clear();

            var items = await fh.GetChatSingle(myuser, recipient);
            foreach (var item in items)
                lstmessages.Add(item);
            ListViewAdapter adapter = new ListViewAdapter(this, lstmessages);
            lstChat.Adapter = adapter;
        }
    }
}