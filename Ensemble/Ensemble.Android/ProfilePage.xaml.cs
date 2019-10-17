using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ensemble
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer

    public partial class ProfilePage : ContentPage
    {
        protected Profile profile = new Profile();
        public string name { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string instr { get; set; }


        async void OnDisplayAlertQuestionButtonClicked(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Save?", "Would you like to save your data?", "Yes", "No");
            Console.WriteLine("Save data: " + response);
            profile.Age = age;
            profile.Gender = gender;
            profile.Name = name;
            profile.Instr = instr;
            profile.printto();
        }

        void OnNameTextChanged(object sender, TextChangedEventArgs e)
        {
            string oldText = e.OldTextValue;
            string newText = e.NewTextValue;
            name = newText;

        }
        /*   void OnNameCompleted(object sender, EventArgs e)
		   {
			   /*string text = ((Editor)sender).Text;
			   profile.name = text;

			   name = ((Editor)sender).Text;
		   }*/
        /*  void OnEmailCompleted(object sender, EventArgs e)
		  {
			  //string text = ((Editor)sender).Text;
			  /*profile.email= ;
			  profile.printto();
		  }*/

        void OnAgeTextChanged(object sender, TextChangedEventArgs e)
        {
            var oldText = e.OldTextValue;
            var newText = e.NewTextValue;
            age = newText;
        }
        /* void OnAgeCompleted(object sender, EventArgs e)
		 {
			 var text = ((Editor)sender).Text;
		 }*/

        void OnGenderTextChanged(object sender, TextChangedEventArgs e)
        {
            string oldText = e.OldTextValue;
            string newText = e.NewTextValue;
            gender = newText;

        }
        /*   void OnGenederCompleted(object sender, EventArgs e)
		   {
			   string text = ((Editor)sender).Text;
		   }*/

        void OnInstrumentTextChanged(object sender, TextChangedEventArgs e)
        {
            string oldText = e.OldTextValue;
            string newText = e.NewTextValue;
            instr = newText;
        }
        /*    void OnInstrumentCompleted(object sender, EventArgs e)
			{
				string text = ((Editor)sender).Text;
			}*/
    }
}