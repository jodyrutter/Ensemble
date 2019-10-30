using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using static Android.Views.View;

using Toolbar = Android.Support.V7.Widget.Toolbar;
using Xamarin.Forms;
using Ensemble.Droid;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms.Platform.Android;

namespace Ensemble.Droid
{
    [Activity(Label = "ToForms", Theme = "@style/AppTheme")]
    public class ToXForms : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.toxforms);

            Forms.Init(this, null);

            Android.Support.V4.App.Fragment mainPage = new LandingPage().CreateSupportFragment(this);
            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.fragment_frame_layout, mainPage)
                .Commit();
        }


    }
}