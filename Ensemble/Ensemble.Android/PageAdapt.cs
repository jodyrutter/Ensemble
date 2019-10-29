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
using Java.Lang;
namespace Ensemble.Droid
{
    
    
    class PageAdapt : PagerAdapter
    {
        Context context;
        public PagerCatalog pageCat;

        public PageAdapt(Context context, PagerCatalog pc)
        {
            this.context = context;
            this.pageCat = pc;
        }

        public override int Count
        {
            get { return pageCat.NumPages; }
        }

        public override Java.Lang.Object InstantiateItem(View container, int position)
        {
            var imageView = new ImageView(context);
            imageView.SetImageResource(Resource.Drawable.ensemble);

            var viewPager = container.JavaCast<ViewPager>();
            viewPager.AddView(imageView);
            return imageView;
        }
        public override void DestroyItem(View container, int position, Java.Lang.Object view)
        {
            var viewPager = container.JavaCast<ViewPager>();
            viewPager.RemoveView(view as View);
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view == @object;
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(pageCat[position].caption);
        }
    }
}