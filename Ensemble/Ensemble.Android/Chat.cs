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
    [Activity(Label = "Chat")]
    public class Chat : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Chatty1);

            string text = Intent.GetStringExtra("Room") ?? "Data is not available";
            Toast.MakeText(this, text, ToastLength.Long).Show();
            
        }
    }
}