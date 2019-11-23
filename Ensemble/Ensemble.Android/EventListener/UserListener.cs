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
using Ensemble.Droid.Helpers;
using Firebase.Database;

namespace Ensemble.Droid.EventListener
{
    public class UserListener : Java.Lang.Object, IValueEventListener
    {
        List<User> userList = new List<User>();
        public event EventHandler<UserDataEventArgs> UsersRetrieved;
        public class UserDataEventArgs : EventArgs
        { 
            public List<User> users { get; set; }
        }
        public void OnCancelled(DatabaseError error)
        {
            
        }

        public void Create()
        {
            DatabaseReference dRef = AppDataHelper.GetDatabase().GetReference("Users");
            dRef.AddValueEventListener(this);
        }
        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Value != null)
            {
                var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                userList.Clear();
                foreach (DataSnapshot data in child)
                {
                    User user = new User();
                    user.Email = data.Child("Email").Value.ToString();
                    user.Pwd = data.Child("Pwd").Value.ToString();
                    user.uname = data.Child("uname").Value.ToString();
                    user.Age = Convert.ToInt32(data.Child("Age").Value.ToString());
                    user.ProfilePic = data.Child("ProfilePic").Value.ToString();
                    user.FavInstrument = data.Child("FavInstrument").Value.ToString();
                    user.ShortBio = data.Child("ShortBio").Value.ToString();
                    user.yLink = data.Child("yLink").Value.ToString();
                    //user.Yes = data.Child("ProfilePic").;
                    //user.No = ;
                    userList.Add(user);
                }
                UsersRetrieved.Invoke(this, new UserDataEventArgs { users = userList });
            }
        }
    }
}