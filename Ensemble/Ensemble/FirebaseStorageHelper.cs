using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;
namespace Ensemble
{
    public class FirebaseStorageHelper
    {
        FirebaseStorage firebaseStorage = new FirebaseStorage("gs://ensemble-65b0c.appspot.com");

        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            var imageUrl = await firebaseStorage
                .Child("Profiles")
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }

        public async Task<string> GetFile(string fileName)
        {
            return await firebaseStorage
                .Child("Profiles")
                .Child(fileName)
                .GetDownloadUrlAsync();
        }

        public async Task DeleteFile(string fileName)
        {
            await firebaseStorage
                .Child("Profiles")
                .Child(fileName)
                .DeleteAsync();
        }
    }
}
