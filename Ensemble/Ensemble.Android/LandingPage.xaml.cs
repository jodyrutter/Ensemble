using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ensemble
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : TabbedPage
    {
        public LandingPage()
        {
            this.Children.Add(new ProfilePage { Title = "Profile" });
            this.Children.Add(new MatchingPage { Title = "Match" });
            this.Children.Add(new ContentPage { Title = "Compose" });

            Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage.SetIsSwipePagingEnabled(this, false);

            this.Title = "Ensemble";
        }
      /*  void OnProfile(object sender, EventArgs args)
        {
            Navigation.PushModalAsync(new Page1());
        }*/
    }
}
