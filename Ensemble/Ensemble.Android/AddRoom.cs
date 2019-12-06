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
        //initialize variables of the class
        TextInputLayout roomnameText;
        MaterialSpinner part1Spinner;
        MaterialSpinner part2Spinner;
        MaterialSpinner part3Spinner;
        Button submitButton;
        FirebaseHelper fh;
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
        //upon creation of fragment
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //Initialize Controls for fragment
            View view = inflater.Inflate(Resource.Layout.CreateRoom2, container, false);
            roomnameText = (TextInputLayout)view.FindViewById(Resource.Id.roomnameText);
            part1Spinner = (MaterialSpinner)view.FindViewById(Resource.Id.statusSpinner1);
            part2Spinner = (MaterialSpinner)view.FindViewById(Resource.Id.statusSpinner2);
            part3Spinner = (MaterialSpinner)view.FindViewById(Resource.Id.statusSpinner3);
            submitButton = (Button)view.FindViewById(Resource.Id.submitButton);
            ler = (LinearLayout)view.FindViewById(Resource.Id.Createlayout);
            
            //Set up participant spinner
            SetupPartSpinners();

            submitButton.Click += CreateARoom;

            return view;
        }

        //Create chat room
        private async void CreateARoom(object sender, EventArgs e)
        {
            //If all entries are validated
            if (ValidateEntries())
            {
                //Initialize roomname and participants
                String roomname = roomnameText.EditText.Text;
                String participant1 = part1Spinner.SelectedItem.ToString();
                String participant2 = part2Spinner.SelectedItem.ToString();
                String participant3 = part3Spinner.SelectedItem.ToString();

                //Add first participant
                participants.Add(participant1);
                
                //Add second participant if not N/A
                if (participant2 != "N/A")
                    participants.Add(participant2);

                //Add third participant if not N/A
                if (participant3 != "N/A")
                    participants.Add(participant3);

                //Final Validation check. If everyone has not checked yes on everyone in the chat, the room cannot be created thus failure.
                //All people in chat must swipe yes on everyone in 
                for (int i = 0; i < participants.Count; i++)
                {
                    for (int j = 0; j < participants.Count; j++)
                    {
                        if (i == j)
                        {

                        }
                        else
                        {
                           var check = await fh.ValidateRoom(participants[i], participants[j]);

                            if (check == false)
                            {
                                Snackbar.Make(ler, "Error! Participant has not swiped yes on another user. Room creation failure", Snackbar.LengthLong).Show();
                                return;
                            }
                        }
                    }
                }

                //Create room with name and List of participants
                room = new Room(participants, roomname);

                //Add room to realtime database
                AddRoomToRealtime(room);
                Snackbar.Make(ler, "Room Created", Snackbar.LengthShort).Show();
                Dismiss(); //Close fragment 
            }
            else
            {
                Snackbar.Make(ler, "Error", Snackbar.LengthShort).Show();
            }
        }
        //Add room to firebase realtime
        private async void AddRoomToRealtime(Room room)
        {
            await fh.CreateRoom(room);
        }

        //Validation of the  entries
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

            //if the participantSpinners do not have a selection, throw error
            if (part1Spinner.SelectedItem == null || part2Spinner.SelectedItem == null || part3Spinner.SelectedItem == null)
            {
                participantbool = false;
                Snackbar.Make(ler, "Choose participants or choose N/A", Snackbar.LengthShort).Show();
            }

            
            else if(part1Spinner.SelectedItem != null && part2Spinner.SelectedItem != null && part3Spinner.SelectedItem != null)
            {
                //participant 1 spinner cannot be N/A, or there will be error message thrown
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
       
        //Set up the spinners
        public async void SetupPartSpinners()
        {
            //initialize variables
            userList = new List<User>();
            usernameList = new List<string>();
            participants = new List<string>();
            fh = new FirebaseHelper();
            
            //The first string should be N/A so none of the above is an option
            usernameList.Add("N/A");

            //get user information and list of users from Firebase Realtime Database
            user = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            userList = await fh.GetAllUsers();
            foreach (User u in userList)
            {
                for (int i = 0; i < user.Yes.Count; i++)
                {
                    if (u.Email != FirebaseAuth.Instance.CurrentUser.Email)
                    {
                        //Obtain the emails from the user's Yes list to show on usernameList
                        if (u.Email == user.Yes[i])
                        {
                            usernameList.Add(u.Email);
                        }
                    }
                }
            }

            //Add user to participants as first person since user is creating room
            participants.Add(FirebaseAuth.Instance.CurrentUser.Email);

            //show userList using this adapter
            adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem, usernameList);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            //connect all three spinners to that adapter
            part1Spinner.Adapter = adapter;
            part2Spinner.Adapter = adapter;
            part3Spinner.Adapter = adapter;
        }   
    }
}