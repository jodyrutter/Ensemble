using System.Collections.Generic;


namespace Ensemble
{
    public class User
    {
        //user information has to be finalized
        public string Email { get; set; }
        public string Pwd { get; set; }   //may get rid of later
        public string uname { get; set; }
        //public string FName { get; set; }
        //public string LName { get; set; }
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

        public User(string e, string p)
        {
            Email = e;
            Pwd = p;
        }
        public User(int age, string instrument, string shortbio, string ylink)
        {
            Age = age;
            FavInstrument = instrument;
            ShortBio = shortbio;
            yLink = ylink;
            Yes = null;
            No = null;
        }
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
