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
using Firebase.Xamarin.Database;


namespace Ensemble.Droid
{
    [Activity(Label = "ChatUsingFirebase")]
    public class ChatUsingFirebase : AppCompatActivity, IValueEventListener
    {
        private FirebaseClient firebase;
        private List<MessageContent> lstmessages;
        private ListView lstChat;
        private EditText edtChat;
        private FloatingActionButton fab;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Messages);

            firebase = new FirebaseClient("https://ensemble-65b0c.firebaseio.com/");
            FirebaseDatabase.Instance.GetReference("chats").AddValueEventListener(this);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            edtChat = FindViewById<EditText>(Resource.Id.input);
            lstChat = FindViewById<ListView>(Resource.Id.list_of_messages);

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

            var items = await firebase.Child("chats")
                .OnceAsync<MessageContent>();
            foreach (var item in items)
                lstmessages.Add(item.Object);
            ListViewAdapter adapter = new ListViewAdapter(this, lstmessages);
            //ListViewAdapter adapter = new ListViewAdapter(this, lstmessages);
            //lstChat.Adapter = adapter;
        }
    }
}