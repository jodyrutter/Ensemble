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
using Firebase;
using Firebase.Database;

namespace Ensemble.Droid.Helpers
{
    public static class AppDataHelper
    {
        public static FirebaseDatabase GetDatabase()
        {
            var app = FirebaseApp.InitializeApp(Application.Context);
            FirebaseDatabase db;

            if (app == null)
            {
                var options = new Firebase.FirebaseOptions.Builder()
                    .SetApplicationId("ensemble-65b0c")
                    .SetApiKey("AIzaSyD25wXdD1WxUQGQD3zkXNkf3X9UYJYaAtE")
                    .SetDatabaseUrl("https://ensemble-65b0c.firebaseio.com")
                    .SetStorageBucket("ensemble-65b0c.appspot.com")
                    .Build();
                app = FirebaseApp.InitializeApp(Application.Context, options);
                db = FirebaseDatabase.GetInstance(app);
                return db;
            }
            else
            {
                db = FirebaseDatabase.GetInstance(app);
            }
            return db;
        }
    }
}