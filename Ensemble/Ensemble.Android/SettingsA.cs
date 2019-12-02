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
        public bool vocal { get; set; }
        public bool guitar { get; set; }
        public bool drums { get; set; }
        public bool bass { get; set; }
        public bool piano { get; set; }
        public bool violin { get; set; }
        public bool synth { get; set; }
        public bool other { get; set; }
        public SettingsA() {
        }
    }
}