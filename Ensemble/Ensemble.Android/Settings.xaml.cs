using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ensemble.Droid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Boolean band;
        public Boolean ind;
        public String vid;
        public SwitchCell bandCell;
        public SwitchCell indCell;
        public TextCell vidCell;
        public Settings()
        {
            InitializeComponent();
            ConnectControl();
        }
        void ConnectControl() {
            bandCell = (SwitchCell)FindByName("group");
            indCell = (SwitchCell)FindByName("individuals");
            vidCell = (TextCell)FindByName("vid");
            //vidCell.IsEnabled = false;
            //views
            //btnSignup = FindViewById<Button>(Resource.Id.signup_btn_register);
            //btnLogin = FindViewById<TextView>(Resource.Id.signup_btn_login);
            //Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);
            //input_email = FindViewById<EditText>(Resource.Id.signup_email);
            //input_pwd = FindViewById<EditText>(Resource.Id.signup_password);
            //input_username = FindViewById<EditText>(Resource.Id.signup_username);
            //input_favInstrument = FindViewById<EditText>(Resource.Id.signup_favInstrument);
            //input_age = FindViewById<EditText>(Resource.Id.signup_age);
            //input_bio = FindViewById<EditText>(Resource.Id.signup_bio);
            //input_youlink = FindViewById<EditText>(Resource.Id.signup_ylink);
            //activity_sign_up = FindViewById<RelativeLayout>(Resource.Id.activity_sign_up);

            ////Link button presses to functions
            //btnSignup.Click += btnSignup_Click;
            //btnLogin.Click += btnLogin_Click;

            //spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            //var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.instrument_array, Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //spinner.Adapter = adapter;

        }
    }
}