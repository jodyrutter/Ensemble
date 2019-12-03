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

namespace Ensemble.Droid
{
    class RoomViewAdapter : BaseAdapter
    {
        FirebaseHelper fh = new FirebaseHelper();
        private RoomSelection roomSelect;
        private List<Room> rooms;
        User user;

        public override int Count
        {
            get
            {
                return rooms.Count;
            }
        }

        public RoomViewAdapter(RoomSelection roomSelect, List<Room> rooms)
        {
            this.roomSelect = roomSelect;
            this.rooms = rooms;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)roomSelect.BaseContext.GetSystemService(Context.LayoutInflaterService);
            View itemView = inflater.Inflate(Resource.Layout.List_Rooms, null);

            //Controls added when completed
            TextView room_name;
            TextView lastMsg;
            TextView lastMsgTime;
            ImageView profilePic;

            room_name = itemView.FindViewById<TextView>(Resource.Id.room_name);
            lastMsg = itemView.FindViewById<TextView>(Resource.Id.last_msg);
            lastMsgTime = itemView.FindViewById<TextView>(Resource.Id.last_msg_time);
            profilePic = itemView.FindViewById<ImageView>(Resource.Id.imageView1);

            room_name.Text = rooms[position].Name;
            if (rooms[position].lastMsg == null)
                lastMsg.Text = null;
            else
                lastMsg.Text = rooms[position].lastMsg.Message;
            
            if (rooms[position].lastMsg == null)
                lastMsgTime.Text = null;
            else
                lastMsgTime.Text = rooms[position].lastMsg.Time;

            
            return itemView;        
        }
        private async void GetUser()
        {
            var item = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            item = user;
        }
    }
}