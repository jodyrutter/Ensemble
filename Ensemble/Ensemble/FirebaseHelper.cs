using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace Ensemble
{
    
    class FirebaseHelper
    {
        FirebaseClient firebase = new FirebaseClient("https://ensemble-65b0c.firebaseio.com/");

        public async Task<List<User>> GetAllUsers()
        {
            return (await firebase
                .Child("User")
                .OnceAsync<User>()).Select(item => new User
                {
                    email = item.Object.email,
                    pwd = item.Object.pwd
                }).ToList();
        }

        protected async Task AddUser(string e, string p)
        {
            await firebase
                .Child("Users")
                .PostAsync(new User() { email = e, pwd = p });
        }

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
        }
    }
}
