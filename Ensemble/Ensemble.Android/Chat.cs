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
    [Activity(Label = "Chat")]
    public class Chat : AppCompatActivity
    {
        private FirebaseHelper fh;
        private string text;
        private Room room;
        private EditText msg;
        private TextView roomname;
        private List<string> ppl;
        private FloatingActionButton fab;
        private RelativeLayout rel;
        private List<MessageContent> chatLog;
        private ListView lstChat;
        
        private async void GetRoom(string name)
        {
            string uemail = FirebaseAuth.Instance.CurrentUser.Email;
            var item = await fh.GetRoom(uemail, name);
            room.Name = item.Name;
            room.ChatLog = item.ChatLog;
            room.lastMsg = item.lastMsg;
            room.participants = item.participants;
            


        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.AppTheme);
            SetContentView(Resource.Layout.Chatty1);

            text = Intent.GetStringExtra("Room") ?? "Data is not available";
            //Toast.MakeText(this, text, ToastLength.Long).Show();
            //room = new Room();
            //GetRoom(text);
            //Toast.MakeText(this, room.Name, ToastLength.Long).Show();

            fab = FindViewById<FloatingActionButton>(Resource.Id.fab1);
            roomname = FindViewById<TextView>(Resource.Id.roomName);
            msg = FindViewById<EditText>(Resource.Id.input_msg);
            rel = FindViewById<RelativeLayout>(Resource.Id.real1);

            roomname.Text = text;

            fab.Click += delegate
            {
                PostMessage();
            };
            DisplayChatMessages();
        }

        private async void PostMessage()
        {
            if (msg.Text.Length == 0)
            {
                return;
            }
            else
            {
                MessageContent temp = new MessageContent();
                temp.Email = FirebaseAuth.Instance.CurrentUser.Email;
                temp.Message = msg.Text;
                temp.Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                chatLog.Add(temp);
                msg.Text = "";

                fh = new FirebaseHelper();

                await fh.UpdateRoom(text, ppl ,chatLog, temp);
                DisplayChatMessages();
            }
        }

        private async void DisplayChatMessages()
        {
            chatLog.Clear();

            room = new Room();
            fh = new FirebaseHelper();

            string uemail = FirebaseAuth.Instance.CurrentUser.Email;
            
            var item = await fh.GetRoom(uemail, text);
            room.Name = item.Name;
            room.ChatLog = item.ChatLog;
            room.lastMsg = item.lastMsg;
            room.participants = item.participants;
            ppl = room.participants;
            chatLog = room.ChatLog;
                        
            ListViewAdapter adapter = new ListViewAdapter(this, chatLog);
            lstChat.Adapter = adapter;
            
        }
    }
}