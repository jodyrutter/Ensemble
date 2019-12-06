using System.Collections.Generic;


namespace Ensemble
{
    public class User
    {
        //variables for the user class
        public string Email { get; set; }
        public string Pwd { get; set; } 
        public string uname { get; set; }
        public int Age { get; set; }
        public string ProfilePic { get; set; }
        public string FavInstrument { get; set; }
        public string ShortBio { get; set; }
        public string yLink { get; set; }
        public List<string> Yes { get; set; }
        public List<string> No { get; set; }

        public User()
        {

        }

        //initialize user with specific credentials
        public User(string e, string p, string fname, int age, string profile, string instrument, string shortbio, string ylink, List<string> yes, List<string> no)
        {
            Email = e;
            Pwd = p;
            uname = fname;
            Age = age;
            ProfilePic = profile;
            FavInstrument = instrument;
            ShortBio = shortbio;
            yLink = ylink;
            Yes = yes;
            No = no;

        }


    }
}
