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
        private Button add_chat;
        private Button refresh;
        private ListView rooms;
        List<Room> roomList = new List<Room>();
        private FirebaseHelper fh = new FirebaseHelper();
        private RelativeLayout relative;
        private RoomViewAdapter adapter;
        
        AddRoom fragment;

       
        /*public async void GetRoomsList()
        {
            roomList.Clear();
            var items = await fh.GetAllUsersRooms(FirebaseAuth.Instance.CurrentUser.Email);
            foreach(var item in items)
                roomList.Add(item);
        }*/
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Rooms);
            DisplayRooms();
            add_chat = FindViewById<Button>(Resource.Id.add_chat);
            rooms = FindViewById<ListView>(Resource.Id.list_of_rooms);
            relative = FindViewById<RelativeLayout>(Resource.Id.activity_rooms);
            refresh = FindViewById<Button>(Resource.Id.refresh_btn);

            add_chat.Click += Add_chat_Click;
            refresh.Click += Refresh_Click;
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            DisplayRooms();
        }

        private void Add_chat_Click(object sender, EventArgs e)
        {
            fragment = new AddRoom();
            var trans = SupportFragmentManager.BeginTransaction();
            fragment.Show(trans, "new room");
            DisplayRooms();
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            DisplayRooms();
        }

        private async void DisplayRooms()
        {
            roomList.Clear();
            var items = await fh.GetAllRooms(FirebaseAuth.Instance.CurrentUser.Email);
            foreach (var item in items)
                roomList.Add(item);
            adapter = new RoomViewAdapter(this, roomList);
            
            rooms.Adapter = adapter;
            rooms.ItemClick += Rooms_ItemClick;
            if (rooms.Adapter.Count == 0)
            {
                Snackbar.Make(relative, "There are no conversations. Time to chat with some musicians", Snackbar.LengthShort).Show();
            }
        }

        private void Rooms_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
           
            Toast.MakeText(this, roomList[Convert.ToInt32(e.Id)].Name, ToastLength.Short).Show();
            var intent = new Intent(this, typeof(Chat));
            intent.PutExtra("Room", roomList[Convert.ToInt32(e.Id)].Name);
            this.StartActivity(intent);
        }

        public void OnCancelled(DatabaseError error)
        {
            
        }
    }
}