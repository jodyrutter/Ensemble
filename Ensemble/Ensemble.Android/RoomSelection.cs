using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Firebase.Auth;
using Firebase.Database;

namespace Ensemble.Droid
{
    [Activity(Label = "RoomSelection")]
    public class RoomSelection : AppCompatActivity, IValueEventListener
    {
        //initialize variables
        private Button add_chat; //create room button
        private Button refresh; //refresh list button
        private ListView rooms; //ListView
        List<Room> roomList = new List<Room>(); //list of rooms
        private FirebaseHelper fh = new FirebaseHelper();
        private RelativeLayout relative;
        private RoomViewAdapter adapter;
        
        AddRoom fragment; //fragment for creating room
    
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Create application
            SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Rooms);

            DisplayRooms(); //Show rooms from Database

            //Connect views
            add_chat = FindViewById<Button>(Resource.Id.add_chat);
            rooms = FindViewById<ListView>(Resource.Id.list_of_rooms);
            relative = FindViewById<RelativeLayout>(Resource.Id.activity_rooms);
            refresh = FindViewById<Button>(Resource.Id.refresh_btn);

            //Add click methods to buttons
            add_chat.Click += Add_chat_Click;
            refresh.Click += Refresh_Click;
        }

        //Upon pressing refresh button, show the rooms
        private void Refresh_Click(object sender, EventArgs e)
        {
            DisplayRooms();
        }

        //Upon pressing add chat button, create fragment to give prompt to create room
        private void Add_chat_Click(object sender, EventArgs e)
        {
            fragment = new AddRoom();
            var trans = SupportFragmentManager.BeginTransaction();
            fragment.Show(trans, "new room");
            DisplayRooms();
        }
        
        //Show rooms if any data changes
        public void OnDataChange(DataSnapshot snapshot)
        {
            DisplayRooms();
        }

        //Show the rooms
        private async void DisplayRooms()
        {
            //clear the roomList List to avoid repetition
            roomList.Clear();

            //Obtain rooms from Firebase Realtime Database (all rooms are under user's username)
            var items = await fh.GetAllRooms(FirebaseAuth.Instance.CurrentUser.Email);
            foreach (var item in items)
                roomList.Add(item);
            
            //show the rooms
            adapter = new RoomViewAdapter(this, roomList);
            
            //Go to method upon clicking specific room
            rooms.Adapter = adapter;
            rooms.ItemClick += Rooms_ItemClick;

            //If roomList is 0, display message
            if (rooms.Adapter.Count == 0)
            {
                Snackbar.Make(relative, "There are no conversations. Time to chat with some musicians", Snackbar.LengthShort).Show();
            }
        }

        //Upon clicking room, go to Chat Activity with the name of the room
        private void Rooms_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(Chat));
            intent.PutExtra("Room", roomList[Convert.ToInt32(e.Id)].Name);
            this.StartActivity(intent);
        }

        //Upon Cancelled
        public void OnCancelled(DatabaseError error)
        {
            
        }
    }
}