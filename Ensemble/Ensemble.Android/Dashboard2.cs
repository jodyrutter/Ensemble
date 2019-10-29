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
using Android.Support.V4.View;
using Android.Support.V4.App;
namespace Ensemble.Droid
{
    public class Dashboard2 : FragmentActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Dashboard2);
            ViewPager vp = FindViewById<ViewPager>(Resource.Id.viewpager);
            PagerCatalog pageCards = new PagerCatalog();
        }
    }
}