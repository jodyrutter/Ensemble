using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Java.Util;

namespace Ensemble.Droid
{
    [Activity(Label = "CreateRoom")]
    public class CreateRoom : AppCompatActivity
    {
        FirebaseHelper fh = new FirebaseHelper();
        Spinner sp;
        //DatabaseReference reference;
        //FirebaseDatabase db;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CreateRoom);

            sp = FindViewById<Spinner>(Resource.Id.spinner1);

            var reference = FirebaseDatabase.Instance.Reference;
            var db = FirebaseDatabase.Instance;
            if (reference != null)
            {
                reference.Child("Users").AddValueEventListener(this);
            }

            /*sp.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner1_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array., Android.Resource.Layout.SimpleSpinnerDropDownItem);
            sp.Adapter = adapter;*/
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            List<string> usernames = RetrieveUsers(snapshot);
            UpdateSpinner(usernames);
        }

        private List<string> RetrieveUsers(DataSnapshot snapshot)
        {
            List<string> users = new List<string>();
            var children = snapshot.Children.ToEnumerable<DataSnapshot>();
            HashMap map;

            foreach (var s in children)
            {
                map = (HashMap)s.Value;
                if (map.ContainsKey("Email"))
                {
                    users.Add(map.Get("Email").ToString());
                }
            }
            return users;
        }

        /*private void spinner1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("The person to contact is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }*/

        private void UpdateSpinner(List<string> p)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.support_simple_spinner_dropdown_item, p);
        }
    }
}