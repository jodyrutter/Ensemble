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
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
        }
        void OnSubmit(object sender, EventArgs args)
        {
            string user, pass; //Strings to contain the user's login credentials.
            user = username.Text;
            pass = password.Text;
        }
    }
}