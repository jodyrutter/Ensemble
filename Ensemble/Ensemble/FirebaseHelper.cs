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
        FirebaseClient firebase = new FirebaseClient("https://ensemble-65b0c.firebaseio.com/");

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

        public async Task AddUser(string e, string p)
        {
            await firebase
                .Child("Users")
                .PostAsync(new User() { Email = e, Pwd = p });
        }

        public async Task<User> GetUser(string email)
        {
            var allUsers = await GetAllUsers();
            await firebase
                .Child("Users")
                .OnceAsync<User>();
            return allUsers.Where(a => a.Email == email).FirstOrDefault();
        }

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
