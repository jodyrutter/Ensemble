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
        MaterialSpinner part1Spinner;
        MaterialSpinner part2Spinner;
        MaterialSpinner part3Spinner;
        Button submitButton;
        FirebaseHelper fh;
        //List<string> statusList;
        List<string> usernameList;
        User user;
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
            part1Spinner = (MaterialSpinner)view.FindViewById(Resource.Id.statusSpinner1);
            part2Spinner = (MaterialSpinner)view.FindViewById(Resource.Id.statusSpinner2);
            part3Spinner = (MaterialSpinner)view.FindViewById(Resource.Id.statusSpinner3);
            submitButton = (Button)view.FindViewById(Resource.Id.submitButton);
            ler = (LinearLayout)view.FindViewById(Resource.Id.Createlayout);
            SetupPart1Spinner();

            submitButton.Click += CreateARoom;

            return view;
            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void CreateARoom(object sender, EventArgs e)
        {
            if (ValidateEntries())
            {
                String roomname = roomnameText.EditText.Text;
                String participant1 = part1Spinner.SelectedItem.ToString();
                String participant2 = part2Spinner.SelectedItem.ToString();
                String participant3 = part3Spinner.SelectedItem.ToString();

                participants.Add(participant1);

                if (participant2 != "N/A")
                    participants.Add(participant2);

                if (participant3 != "N/A")
                    participants.Add(participant3);

                room = new Room(participants, roomname);

                AddRoomToRealtime(room);
                Snackbar.Make(ler, "Room Created", Snackbar.LengthShort).Show();
                Dismiss();
            }
            else
            {
                Snackbar.Make(ler, "Error", Snackbar.LengthShort).Show();
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
            //there must be a check so that rooms do not have the same name in the Database Child
            else
            {
                roomNamebool = true;
            }

            if (part1Spinner.SelectedItem == null || part2Spinner.SelectedItem == null || part3Spinner.SelectedItem == null)
            {
                participantbool = false;
                Snackbar.Make(ler, "Choose participants or choose N/A", Snackbar.LengthShort).Show();
            }

            else if(part1Spinner.SelectedItem != null && part2Spinner.SelectedItem != null && part3Spinner.SelectedItem != null)
            {
                
                if (part1Spinner.SelectedItem.ToString() == "N/A")
                {
                    Snackbar.Make(ler, "Participant 1 must not be N/A", Snackbar.LengthShort).Show();
                    participantbool = false;
                }
                else
                {
                    participantbool = true;
                }
            }

            return roomNamebool && participantbool;
        }
       
        public async void SetupPart1Spinner()
        {
            userList = new List<User>();
            usernameList = new List<string>();
            participants = new List<string>();
            fh = new FirebaseHelper();
            
            usernameList.Add("N/A");




            user = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            userList = await fh.GetAllUsers();
            foreach (User u in userList)
            {
                /*for (int i = 0; i < user.Yes.Count; i++)
                {
                    if (u.Email != FirebaseAuth.Instance.CurrentUser.Email)
                    {
                        if (u.Email == user.Yes[i])
                        {
                            usernameList.Add(u.Email);
                        }
                    }
                }*/
                
                
                if(u.Email != FirebaseAuth.Instance.CurrentUser.Email)
                    usernameList.Add(u.Email);
            }

            participants.Add(FirebaseAuth.Instance.CurrentUser.Email);


            adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem, usernameList);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            part1Spinner.Adapter = adapter;
            part2Spinner.Adapter = adapter;
            part3Spinner.Adapter = adapter;
        }

        
    }

}