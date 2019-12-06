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

        //get adapter's room count
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
            //Create the view
            LayoutInflater inflater = (LayoutInflater)roomSelect.BaseContext.GetSystemService(Context.LayoutInflaterService);
            View itemView = inflater.Inflate(Resource.Layout.List_Rooms, null);

            //Controls for each room in the ViewAdapter 
            TextView room_name;
            TextView lastMsg;
            TextView lastMsgTime;
            ImageView profilePic;

            //Connect controls in xml with variables in cs 
            room_name = itemView.FindViewById<TextView>(Resource.Id.room_name);
            lastMsg = itemView.FindViewById<TextView>(Resource.Id.last_msg);
            lastMsgTime = itemView.FindViewById<TextView>(Resource.Id.last_msg_time);
            profilePic = itemView.FindViewById<ImageView>(Resource.Id.imageView1);

            //Room name is the name of the room
            room_name.Text = rooms[position].Name;
            
            //If there is no last message of the room, keep last message blank else put the last message of the room's chat
            if (rooms[position].lastMsg == null)
                lastMsg.Text = null;
            else
                lastMsg.Text = rooms[position].lastMsg.Message;

            //If there is no last message of the room, keep last message's time blank else put the last message's time of the room's chat
            if (rooms[position].lastMsg == null)
                lastMsgTime.Text = null;
            else
                lastMsgTime.Text = rooms[position].lastMsg.Time;

            
            return itemView;        
        }
        //obtain user's information
        private async void GetUser()
        {
            var item = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            item = user;
        }
    }
}