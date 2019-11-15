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
                    Pwd = item.Object.Pwd,
                    uname = item.Object.uname,
                    Age = item.Object.Age,
                    ProfilePic = item.Object.ProfilePic,
                    FavInstrument = item.Object.FavInstrument,
                    yLink = item.Object.yLink,
                    ShortBio = item.Object.ShortBio,
                    Yes = item.Object.Yes,
                    No = item.Object.No
                }).ToList();
        }


        //Get all users from Realtime Database except specific email
        public async Task<List<User>> GetAllUsersExcept(string email)
        {
            var AllUsers = await GetAllUsers();
            await firebase
                .Child("Users")
                .OnceAsync<User>();
            return AllUsers.Where(a => a.Email != email).ToList();
        }
        //Get all users from Realtime Database except in user's no list
        public async Task<List<User>> GetAllUsersExceptNoList(List<string> no)
        {
            var AllUsers = await GetAllUsers();

            await firebase
                .Child("Users")
                .OnceAsync<User>();

            for (int i = 0; i < AllUsers.Count; i++)
            {
                for (int j = 0; j < no.Count; j++)
                {
                    if (AllUsers[i].Email == no[j])
                    {
                        AllUsers.RemoveAt(i);
                    }
                }
            }
            return AllUsers;
        }

        //Add 1 user to Realtime Database
        public async Task AddUser(string e, string p)
        {
            await firebase
                .Child("Users")
                .PostAsync(new User() { Email = e, Pwd = p });
        }
        //add a user with credentials into Realtime Database
        //public User(string e, string p, string fname, int age, string profile, string instrument, string shortbio, string ylink, List<string> yes, List<string> no)
        public async Task AddUser(string e, string p, string fname, int age, string ppic, string favInstrument, string bio, string ylink, List<string> yes, List<string> no)
        {
            await firebase
                .Child("Users")
                .PostAsync(new User(e, p, fname, age, ppic, favInstrument, bio, ylink, yes, no));
        }
        //Get user from Realtime Database based on email
        public async Task<User> GetUserwithEmail(string email)
        {
            var allUsers = await GetAllUsers();
            await firebase
                .Child("Users")
                .OnceAsync<User>();
            return allUsers.Where(a => a.Email == email).FirstOrDefault();
        }
        //Find user based on name
        public async Task<User> GetUserwithUsername(string Fname)
        {
            var allUsers = await GetAllUsers();
            await firebase
                .Child("Users")
                .OnceAsync<User>();
            return allUsers.Where(a => a.uname == Fname).FirstOrDefault();
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


        public async Task CreateRoom(List<string> ppl, MessageContent lastMsg, string name, List<MessageContent> messages)
        {
            await firebase
                .Child("Messaging")
                .PostAsync(new Room(ppl, lastMsg, name, messages));
        }

        public async Task UpdateRoom(String name, List<String> ppl, List<MessageContent> chat, MessageContent lastMsg)
        {
            var toUpdateRoom = (await firebase
                .Child("Messaging")
                .OnceAsync<Room>())
                .Where(a => a.Object.Name == name).FirstOrDefault();

            await firebase
                .Child("Messaging")
                .Child(toUpdateRoom.Key)
                .PutAsync(new Room(ppl, lastMsg, name, chat) { });
        }

        public async Task<List<Room>> GetAllRooms()
        {
            return (await firebase
                .Child("Messaging")
                .OnceAsync<Room>()).Select(item => new Room
                {
                    Name = item.Object.Name,
                    participants = item.Object.participants,
                    lastMsg = item.Object.lastMsg,
                    ChatLog = item.Object.ChatLog
                }).ToList();
        }

        public async Task<Room> GetRoom(String name)
        {
            var allRooms = await GetAllRooms();
            await firebase
                .Child("Messaging")
                .OnceAsync<Room>();
            return allRooms.Where(a => a.Name == name).FirstOrDefault();
        }
        
        //need function to delete older chat msgs if 50 msgs or more (delete 49)
    }
}
