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

namespace Ensemble.Droid
{
    class RoomViewAdapter : BaseAdapter
    {
        private RoomSelection room;
        private List<User> users;

        public override int Count
        {
            get
            {
                return users.Count;
            }
        }

        public RoomViewAdapter(RoomSelection room, List<User> users)
        {
            this.room = room;
            this.users = users;
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
            LayoutInflater inflater = (LayoutInflater)room.BaseContext.GetSystemService(Context.LayoutInflaterService);
            View itemView = inflater.Inflate(Resource.Layout.List_Rooms, null);

            //Controls added when completed
            /*
             * 
             * 
             * 
             * 
             * 
             * user_name.Text = users[position].Email;
             * user_Profile.Text = users[position].ProfilePic;
             * user_ShortBio.Text = users[position].ShortBio;
             */
             
            return itemView;
            
        }


            //fill in your items
            //holder.Title.Text = "new text here";

            
        }

        //Fill in cound here, currently 0

    }

    class RoomViewAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
