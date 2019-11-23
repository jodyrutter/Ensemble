﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.RecyclerView.Extensions;
using Android.Views;
using Android.Widget;
using Ensemble.Droid.EventListener;
using Firebase.Auth;
using Firebase.Database;
using Java.Util;

namespace Ensemble.Droid
{
    [Activity(Label = "CreateRoom")]
    public class CreateRoom : Activity
    {
        FirebaseHelper fh = new FirebaseHelper();
        ListView lv;
        List<string> users = new List<string>();
        List<User> userList = new List<User>();
        List<string> part = new List<string>();
        User user;
        UserListener UserListener;
        RelativeLayout rel;

        //DatabaseReference reference;
        //FirebaseDatabase db;



        public void RetrieveData()
        {
            UserListener = new UserListener();
            UserListener.Create();
            UserListener.UsersRetrieved += UserListener_UsersRetrieved;
        }

        private void UserListener_UsersRetrieved(object sender, UserListener.UserDataEventArgs e)
        {
            userList = e.users;
            getUserListName();
            
        }
        public void getUserListName()
        {
            foreach (User i in userList)
                users.Add(i.Email);
        }

        private async void GetAllUsers()
        {
            FirebaseHelper vb = new FirebaseHelper();
            var c = await vb.GetAllUsers();
            foreach (var z in c)
            {
                users.Add(z.Email);
            }

            var item = await fh.GetUserwithEmail(FirebaseAuth.Instance.CurrentUser.Email);
            user = item;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.CreateRoom);
            GetAllUsers();
            lv = FindViewById<ListView>(Resource.Id.listView1);
            rel = FindViewById<RelativeLayout>(Resource.Id.real);
            //RetrieveData();
            //GetAllUsers();
            
            ArrayAdapter<string> a = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, users);
            lv.Adapter = a;
            //ListAdapter = a;
            //lv.TextFilterEnabled = true;
            
            /*lv.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
            {
                Toast.MakeText(Application, ((TextView)e.View).Text, ToastLength.Short).Show();
            };*/
            //lv.Adapter = adapter;
        }

      
    }
}