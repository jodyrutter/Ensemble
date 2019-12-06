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
using Firebase.Auth;

namespace Ensemble.Droid
{
    class SettingsA
    {
        public bool group { get; set; }
        public string youtube { get; set; }
        public int age { get; set; }
        public string favInst { get; set; }
        public string bio { get; set; }
        public SettingsA() {
        }
    }
}