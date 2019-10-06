using System;
using System.Collections.Generic;
using System.Text;


namespace Ensemble
{
    public class User
    {
        //user information has to be finalized
        public string Email { get; set; }
        public string Pwd { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public int Age { get; set; }

        public User()
        {

        }

        public User(string e, string p)
        {
            Email = e;
            Pwd = p;
        }

        public User(string e, string p, string fname, string lname, int age)
        {
            Email = e;
            Pwd = p;
            FName = fname;
            LName = lname;
            Age = age;
        }


    }
}
