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

namespace Ensemble.Droid
{
    [Activity(Label = "RoomSelection")]
    public class RoomSelection : Activity
    {
        private Button add_chat;
        private ListView rooms;
        List<Room> roomList = new List<Room>();
        private FirebaseHelper fh = new FirebaseHelper();
        private RelativeLayout relative;
        private RoomViewAdapter adapter;
        private Room r = new Room();
        public async void GetRoomsList()
        {
            
            var items = await fh.GetAllRooms();

            foreach (var item in items)
            {
                roomList.Add(item);
            }

        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Rooms);

            add_chat = FindViewById<Button>(Resource.Id.add_chat);
            rooms = FindViewById<ListView>(Resource.Id.list_of_rooms);
            relative = FindViewById<RelativeLayout>(Resource.Id.activity_rooms);
            roomList.Add(r);
            GetRoomsList();
            DisplayRooms();


            add_chat.Click += Add_chat_Click;


        }

        private void Add_chat_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DisplayRooms()
        {
            GetRoomsList();

            if (roomList.Count == 0)
            {
                Snackbar.Make(relative, "There are no conversations. Time to chat with some musicians", Snackbar.LengthShort).Show();

            }
            else
            {
                adapter = new RoomViewAdapter(this, roomList);
                rooms.Adapter = adapter;
            }

           
        }
    }
}