using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Collections.Generic;

namespace Ensemble.Droid
{
    internal class ListViewAdapter : BaseAdapter
    {
        private ChatUsingFirebase mainActivity;
        private List<MessageContent> lstMessage;

        public ListViewAdapter(ChatUsingFirebase c, List<MessageContent> ls)
        {
            mainActivity = c;
            lstMessage = ls;
        }
        public override int Count
        {
            get
            {
                return lstMessage.Count;
            }
        }

        public override Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)mainActivity.BaseContext.GetSystemService(Context.LayoutInflaterService);
            View itemView = inflater.Inflate(Resource.Layout.List_Item, null);

            TextView mess_user, mess_time, mess_content;
            mess_user = itemView.FindViewById<TextView>(Resource.Id.message_user);
            mess_time = itemView.FindViewById<TextView>(Resource.Id.message_time);
            mess_content = itemView.FindViewById<TextView>(Resource.Id.message_text);

            mess_user.Text = lstMessage[position].Email;
            mess_time.Text = lstMessage[position].Time;
            mess_content.Text = lstMessage[position].Message;

            return itemView;
        }
    }
}