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
        public bool sVocal { get; set; }
        public bool sGuitar { get; set; }
        public bool sDrum { get; set; }
        public bool sBass { get; set; }
        public bool sPiano { get; set; }
        public bool sViolins { get; set; }
        public bool sSynth { get; set; }
        public bool sOther { get; set; }
        public bool lGroup { get; set; }
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
            lGroup = false;
            sVocal = false;
            sGuitar = false;
            sDrum = false;
            sBass = false;
            sPiano = false;
            sViolins = false;
            sSynth = false;
            sOther = false;
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
            lGroup = false;
            sVocal = false;
            sGuitar = false;
            sDrum = false;
            sBass = false;
            sPiano = false;
            sViolins = false;
            sSynth = false;
            sOther = false;

        }
        public User(string e, string p, string fname, int age, string profile, string instrument, string shortbio, string ylink, List<string> yes, List<string> no, bool group, bool vocals, bool guitar, bool drums, bool bass, bool piano, bool violin, bool synth, bool other)
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
            lGroup = group;
            sVocal = vocals;
            sGuitar = guitar;
            sDrum = drums;
            sBass = bass;
            sPiano = piano;
            sViolins = violin;
            sSynth = synth;
            sOther = other;
        }
        public void setUserSettings(string ylink, bool group, bool vocals, bool guitar, bool drums, bool bass, bool piano, bool violin, bool synth, bool other) {
            yLink = ylink;
            lGroup = group;
            sVocal = vocals;
            sGuitar = guitar;
            sDrum = drums;
            sBass = bass;
            sPiano = piano;
            sViolins = violin;
            sSynth = synth;
            sOther = other;
        }


    }
}
