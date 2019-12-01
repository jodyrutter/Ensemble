using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Firebase.Auth;
using Firebase.Database;
using static Android.Views.View;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Firebase;
using Java.Util;
using Java.Lang;
using Ensemble.Droid.Helpers;
using FR.Ganfra.Materialspinner;
using Firebase.Storage;
using Android.Graphics;
using Android.Provider;
using System.IO;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Android.Content.PM;
using Android;

namespace Ensemble.Droid
{
    [Activity(Label = "SignUp", Theme = "@style/AppTheme")]
    public class SignUp : AppCompatActivity, IOnProgressListener, IOnSuccessListener, IOnFailureListener
    {
        //Initialize all variables
        Button btnSignup;                                        //Sign up button
        Button addPic;
        ImageView pic;
        Button captureButton;
        Android.Net.Uri filePath;
        const int PICK_IMAGE_REQUEST = 71;
        MediaFile file;
        ProgressDialog progressDialog;
        FirebaseStorageHelper fsh = new FirebaseStorageHelper();
        EditText input_email;
        EditText input_pwd;                         //Variables to input email and password
        EditText input_username;
        //EditText input_favInstrument;
        EditText input_age;
        EditText input_bio;
        EditText input_youlink;
        RelativeLayout activity_sign_up;                         //the xaml file variable for the class
        FirebaseAuth auth;                                       //Firebase auth variable
        MaterialSpinner instrumentSpinner;
        List<string> instruments;
        string favInstrument;
        private byte[] imgArray;
        int num;
        TaskCompletionListener tcl = new TaskCompletionListener();
        FirebaseHelper fh = new FirebaseHelper();                           //FirebaseHelper variable
        
        
        
        List<string> ye = null;
        List<string> nay = null;

        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };
        StorageReference storeRef;

        private void SetUpSpinner()
        {
            instruments = new List<string>();
            
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.instrumentArray, Android.Resource.Layout.SimpleSpinnerItem);
            
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            instrumentSpinner.Adapter = adapter;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //Spinner spinner = (Spinner)sender;
            favInstrument = instrumentSpinner.GetItemAtPosition(e.Position).ToString();
            Toast.MakeText(this, favInstrument, ToastLength.Long).Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SignUp);

            //Initiate Firebase onto Sign Up page
            auth = FirebaseAuth.GetInstance(MainActivity.app);
            
            //db = FirebaseDatabase.GetInstance(MainActivity.app);
            ConnectControl();
            SetUpSpinner();
            
        }



        //Link initialized variables with controls on Sign Up xml
        void ConnectControl()
        {
            //views
            btnSignup = FindViewById<Button>(Resource.Id.signup_btn_register);
            addPic = FindViewById<Button>(Resource.Id.pickPic);
            captureButton = FindViewById<Button>(Resource.Id.snapPic);
            pic = FindViewById<ImageView>(Resource.Id.icon);
            input_email = FindViewById<EditText>(Resource.Id.signup_email);
            input_pwd = FindViewById<EditText>(Resource.Id.signup_password);
            input_username = FindViewById<EditText>(Resource.Id.signup_username);
            //input_favInstrument = FindViewById<EditText>(Resource.Id.signup_favInstrument);
            instrumentSpinner = FindViewById<MaterialSpinner>(Resource.Id.instrumentSpinner);
            input_age = FindViewById<EditText>(Resource.Id.signup_age);
            input_bio = FindViewById<EditText>(Resource.Id.signup_bio);
            input_youlink = FindViewById<EditText>(Resource.Id.signup_ylink);
            activity_sign_up = FindViewById<RelativeLayout>(Resource.Id.activity_sign_up);

            //Link button presses to functions
            btnSignup.Click += btnSignup_Click;
            addPic.Click += AddPic_Click;
            captureButton.Click += CaptureButton_Click;
            instrumentSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            RequestPermissions(permissionGroup, 0);

        }

        private void CaptureButton_Click(object sender, EventArgs e)
        {
            TakePhoto();
        }

        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name = "myimage.jpg",
                Directory = "Gallery"
            });

            if (file == null)
                return;

            //Convert file to byte array and set resulting bitmap to imageview
            imgArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imgArray, 0, imgArray.Length);
            pic.SetImageBitmap(bitmap);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void AddPic_Click(object sender, EventArgs e)
        {
            UploadImage();
            /*Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), PICK_IMAGE_REQUEST);
        */}

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == PICK_IMAGE_REQUEST && resultCode == Result.Ok
                && data != null &&
                data.Data != null)
            {
                filePath = data.Data;
                try
                {
                    Bitmap bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, filePath);
                    pic.SetImageBitmap(bitmap);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void UploadToStorage()
        {
            if (imgArray != null)
            {
                storeRef = FirebaseStorage.Instance.GetReference("Images");
                storeRef.PutBytes(imgArray)
                    .AddOnSuccessListener(this)
                    .AddOnFailureListener(this);
            }
        }


       async void UploadImage()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "Upload not supported on this device", ToastLength.Short).Show();
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                CompressionQuality = 40
            });

            imgArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap b = BitmapFactory.DecodeByteArray(imgArray, 0, imgArray.Length);
            pic.SetImageBitmap(b);
        }

        public void OnProgress(Java.Lang.Object snapshot)
        { 
            var taskSnapShot = (UploadTask.TaskSnapshot)snapshot;
            double progress = (100.0 * taskSnapShot.BytesTransferred / taskSnapShot.TotalByteCount);
            progressDialog.SetMessage("Uploaded " + (int)progress + " %");
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            //progressDialog.Dismiss();
            Toast.MakeText(this, "Uploaded Successful", ToastLength.Short).Show();
        }
        public void OnFailure(Java.Lang.Exception e)
        {
            //progressDialog.Dismiss();
            Toast.MakeText(this, "" + e.Message, ToastLength.Short).Show();
        }

        //Go to Main Activity
        private void btnLogin_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }


        //If Signup button is pressed
        private void btnSignup_Click(object sender, EventArgs e)
        {
            if (ValidateCredentials())
            {
                RegisterUser(input_email.Text, input_pwd.Text);
            }
            else
            {
                Snackbar.Make(activity_sign_up, "Please try again", Snackbar.LengthShort).Show();
            }


        }

        private bool ValidateCredentials()
        {
            bool emailVal;
            bool passVal;
            bool unameVal;
            bool AgeVal;
            bool ProfileVal = true;
            bool FavVal;
            bool BioVal;
            bool YVal;


            //validation for email
            if (!input_email.Text.Contains("@"))
            {
                emailVal = false;
                Snackbar.Make(activity_sign_up, "Please enter a valid email", Snackbar.LengthShort).Show();
            }
            else
                emailVal = true;

            //validation for password
            if (input_pwd.Text.Length < 8)
            {
                passVal = false;
                Snackbar.Make(activity_sign_up, "Please enter a password up to 8 characters", Snackbar.LengthShort).Show();
            }
            else
                passVal = true;

            //validation for username
            if (input_username.Text.Length == 0)
            {
                unameVal = false;
                Snackbar.Make(activity_sign_up, "Please enter a username", Snackbar.LengthShort).Show();
            }
            else
            {
                //Make sure username doesnt already exist. To do list
                /*if (fh.GetUserwithUsername(input_username.Text) != null)
                {
                    unameVal = false;
                    Snackbar.Make(activity_sign_up, "Username already taken. Try another username", Snackbar.LengthShort).Show();
                }
                else*/
                unameVal = true;
            }

            //Validation for age
            if (input_age.Text.Length == 0)
            {
                AgeVal = false;
                Snackbar.Make(activity_sign_up, "Please enter your age", Snackbar.LengthShort).Show();
            }
            else
            {
                if (!int.TryParse(input_age.Text, out num))
                {
                    AgeVal = false;
                    Snackbar.Make(activity_sign_up, "Age entered invalid. Try again", Snackbar.LengthShort).Show();
                }
                else
                    AgeVal = true;
            }

            //Validation for profile pic
            //Will be put in later

            //Validation on Favorite instrument
            if (instrumentSpinner.SelectedItem == null || favInstrument == null)
            {
                FavVal = false;
                Snackbar.Make(activity_sign_up, "Please enter your Favorite instrument", Snackbar.LengthShort).Show();
            }
            else
                FavVal = true;

            //Validation for Short Bio
            if (input_bio.Text.Length == 0)
            {
                BioVal = false;
                Snackbar.Make(activity_sign_up, "Please enter short bio about yourself", Snackbar.LengthShort).Show();
            }
            else
            {
                if (input_bio.Text.Length > 250)
                {
                    BioVal = false;
                    Snackbar.Make(activity_sign_up, "Short Bio too long.", Snackbar.LengthShort).Show();
                }
                else
                    BioVal = true;
            }

            //Validation for Youtube Link
            if (input_youlink.Text.Length == 0)
            {
                YVal = false;
                Snackbar.Make(activity_sign_up, "Youtube link not there.", Snackbar.LengthShort).Show();
            }
            else
            {
                //Will put in validation of youtube link later
                YVal = true;
            }


            return (emailVal && passVal && unameVal && AgeVal && ProfileVal && FavVal && BioVal && YVal);
        }

        void RegisterUser(string email, string pass)
        {
            //link the task Completion listener to functions
            tcl.Success += TaskCompletionListener_Success;
            tcl.Failure += TaskCompletionListener_Failure;

            //create the users using Firebase Auth and go to 1 of the two functions depending on success or failure
            auth.CreateUserWithEmailAndPassword(email, pass)
                .AddOnSuccessListener(tcl)
                .AddOnFailureListener(tcl);

        }

        //Upon failure, try again
        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(activity_sign_up, "User Registration failed", Snackbar.LengthShort).Show();
        }
        //Add user to Realtime database
        private void AddtoRealtime()
        {
            int.TryParse(input_age.Text, out num);
            string encryptedPwd = CryptoEngine.Encrypt(input_pwd.Text);
            //await fh.AddUser(input_email.Text, input_pwd.Text, input_username.Text, num, "Picture", input_favInstrument.Text, input_bio.Text, input_youlink.Text, ye, nay);

            HashMap UserInfo = new HashMap();
            UserInfo.Put("Age", num);
            UserInfo.Put("Email", input_email.Text);
            UserInfo.Put("FavInstrument",favInstrument);
            UserInfo.Put("ProfilePic", "Picture");
            UserInfo.Put("Pwd", encryptedPwd);
            UserInfo.Put("ShortBio", input_bio.Text);
            UserInfo.Put("uname", input_username.Text);
            UserInfo.Put("yLink", input_youlink.Text);
            //UserInfo.Put("Yes", num);
            //UserInfo.Put("No", num);

            DatabaseReference dref = AppDataHelper.GetDatabase().GetReference("Users").Push();
            dref.SetValue(UserInfo);

            Snackbar.Make(activity_sign_up, "Account added to Realtime Database", Snackbar.LengthShort).Show();


        }
        //Upon success, Add to Realtime database & go to Main Activity page
        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            Snackbar.Make(activity_sign_up, "User Registration Success", Snackbar.LengthShort).Show();
            AddtoRealtime();

            StartActivity(typeof(MainActivity));
            Finish();

        }

       
    }
}