using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace Ensemble
{
    
    public class FirebaseHelper
    {
        //makes sure it links to Ensemble Firebase Realtime database
        FirebaseClient firebase = new FirebaseClient("https://ensemble-65b0c.firebaseio.com/");

        //Get all users from Realtime Database
        public async Task<List<User>> GetAllUsers()
        {
            return (await firebase
                .Child("Users")
                .OnceAsync<User>()).Select(item => new User
                {
                    Email = item.Object.Email,
                    Pwd = item.Object.Pwd
                }).ToList();
        }
        //Add 1 user to Realtime Database
        public async Task AddUser(string e, string p)
        {
            await firebase
                .Child("Users")
                .PostAsync(new User() { Email = e, Pwd = p });
        }
        //add a user with credentials into Realtime Database
        public async Task AddUser(string e, string p, string fname, string lname, int age, string ppic, string favInstrument, string ylink, string bio)
        {
            await firebase
                .Child("Users")
                .PostAsync(new User(e, p, fname, lname, age, ppic, favInstrument, bio, ylink));
        }
        //Get user from Realtime Database based on email
        public async Task<User> GetUser(string email)
        {
            var allUsers = await GetAllUsers();
            await firebase
                .Child("Users")
                .OnceAsync<User>();
            return allUsers.Where(a => a.Email == email).FirstOrDefault();
        }
        //Find user based on name
        public async Task<User> GetUser(string Fname, string lName)
        {
            var allUsers = await GetAllUsers();
            await firebase
                .Child("Users")
                .OnceAsync<User>();
            return allUsers.Where(a => a.FName == Fname).Where(a => a.LName == lName).FirstOrDefault();
        }
        
        //update user information on Realtime Database
        public async Task UpdateUser(string e, string p)
        {
            var toUpdateUser = (await firebase
                .Child("Users")
                .OnceAsync<User>
                ()).Where(a => a.Object.Email == e).FirstOrDefault();

            await firebase
                .Child("Users")
                .Child(toUpdateUser.Key)
                .PutAsync(new User() { Email = e, Pwd = p });
        }

        //Delete user from Realtime Database based on email
        public async Task DeleteUser(string e)
        {
            var toDelete = (await firebase
                .Child("Users")
                .OnceAsync<User>
                ()).Where(a => a.Object.Email == e).FirstOrDefault();

            await firebase.Child("Users").Child(toDelete.Key).DeleteAsync();
        }
    }
}
