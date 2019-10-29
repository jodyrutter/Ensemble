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
    public class Pager
    {
        public string caption;
        public string Caption { get { return caption; } }
    }

    public class PagerCatalog
    {
        static Pager[] Catalog = {
            new Pager { caption = "Profile"},
            new Pager { caption = "Matching Page"},
            new Pager { caption = "Messages"} ,
        };

        private Pager[] pagers;
        public PagerCatalog() { pagers = Catalog; }

        public Pager this[int i] { get { return Catalog[i]; } }

        public int NumPages { get { return Catalog.Length; } }
    }
}