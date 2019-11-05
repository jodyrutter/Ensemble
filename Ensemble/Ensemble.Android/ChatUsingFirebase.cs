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
//using Firebase.Database.Query;
using Firebase.Xamarin.Database;

namespace Ensemble.Droid
{
    [Activity(Label = "ChatUsingFirebase")]
    public class ChatUsingFirebase : AppCompatActivity, IValueEventListener
    {
        private FirebaseClient fbc;
        private List<MessageContent> lstMessage = new List<MessageContent>();

        private ListView lstChat;
        private EditText editChat;
        private FloatingActionButton fab;

        public int MyResultCode = 1;
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }     

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MessagesChat);
            fbc = new FirebaseClient("https://ensemble-65b0c.firebaseio.com/");
            FirebaseDatabase.Instance.GetReference("MESSAGES").AddValueEventListener(this);

            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            editChat = FindViewById<EditText>(Resource.Id.input);
            lstChat = FindViewById<ListView>(Resource.Id.list_of_messages);

            fab.Click += delegate { PostMessage(); };

            if (FirebaseAuth.Instance.CurrentUser == null)
                StartActivityForResult(new Android.Content.Intent(this, typeof(MainActivity)), MyResultCode);
            else
            {
                Toast.MakeText(this, "Welcome" + FirebaseAuth.Instance.CurrentUser.Email, ToastLength.Short).Show();
                DisplayChatMessage();
            }
        }

        private async void PostMessage()
        {
            string email = FirebaseAuth.Instance.CurrentUser.Email;
            var items = await fbc.Child("MESSAGES").PostAsync(new MessageContent(email, editChat.Text));
            editChat.Text = "";

        }

        public void OnCancelled(DatabaseError error)
        { 
        
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            DisplayChatMessage();
        }

        private async void DisplayChatMessage()
        {
            lstMessage.Clear();

            var items = await fbc.Child("MESSAGES").OnceAsync<MessageContent>();
            foreach (var item in items)
            {
                lstMessage.Add(item.Object);
            }
            ListViewAdapter adapter = new ListViewAdapter(this, lstMessage);
            lstChat.Adapter = adapter;
        }
    }
}