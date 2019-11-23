using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using FR.Ganfra.Materialspinner;

namespace Ensemble.Droid
{
    public class AddRoom : Android.Support.V4.App.DialogFragment
    {
        TextInputLayout roomnameText;
        //TextInputLayout ParticipantText;
        //TextInputLayout SetText;
        MaterialSpinner partSpinner;
        Button submitButton;
        FirebaseHelper fh;
        //List<string> statusList;
        List<string> usernameList;
        List<User> userList;
        List<string> participants;
        ArrayAdapter<string> adapter;
        Room room;
        LinearLayout ler;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.CreateRoom2, container, false);
            roomnameText = (TextInputLayout)view.FindViewById(Resource.Id.roomnameText);
            //ParticipantText = (TextInputLayout)view.FindViewById(Resource.Id.ParticipantnameText);
            //SetText = (TextInputLayout)view.FindViewById(Resource.Id.SetText);
            partSpinner = (MaterialSpinner)view.FindViewById(Resource.Id.statusSpinner);
            submitButton = (Button)view.FindViewById(Resource.Id.submitButton);
            ler = (LinearLayout)view.FindViewById(Resource.Id.Createlayout);
            SetupStatusPinner();

            submitButton.Click += CreateARoom;

            return view;
            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void CreateARoom(object sender, EventArgs e)
        {
            if (ValidateEntries())
            {
                String roomname = roomnameText.EditText.Text;
                String participant = partSpinner.SelectedItem.ToString();

                participants.Add(participant);

                room = new Room(participants, roomname);

                AddRoomToRealtime(room);
                Snackbar.Make(ler, "Room Created", Snackbar.LengthShort).Show();
                Dismiss();
            }
            else
            {
                //string toast =  string.Format("Enter all entries");
                Snackbar.Make(ler, "Enter all entries", Snackbar.LengthShort).Show();
                //Snackbar.Make(activity_sign_up, "Please try again", Snackbar.LengthShort).Show();
            }
        }

        private async void AddRoomToRealtime(Room room)
        {
            await fh.CreateRoom(room);
        }

        public bool ValidateEntries()
        {
            string roomname = roomnameText.EditText.Text;
            

            bool roomNamebool = true ;
            bool participantbool = true;

            //if no room name error throw
            if (roomname.Length == 0)
            {
                roomNamebool = false;
            }
            else
            {
                roomNamebool = true;
            }

            if (partSpinner.SelectedItem == null)
            {
                participantbool = false;
            }

            else
            {
                participantbool = true;
            }

            return roomNamebool && participantbool;
        }
       
        public async void SetupStatusPinner()
        {
            //statusList = new List<string>();
            userList = new List<User>();
            usernameList = new List<string>();
            participants = new List<string>();
            fh = new FirebaseHelper();
            
            /*
            statusList.Add("Graduated");
            statusList.Add("Undergraduate");
            statusList.Add("Dropped Out");
            statusList.Add("Failed");
            */
            userList = await fh.GetAllUsers();
            foreach (User u in userList)
            {
                if(u.Email != FirebaseAuth.Instance.CurrentUser.Email)
                    usernameList.Add(u.Email);
            }

            participants.Add(FirebaseAuth.Instance.CurrentUser.Email);


            adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem, usernameList);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            partSpinner.Adapter = adapter;
        }
    }
}