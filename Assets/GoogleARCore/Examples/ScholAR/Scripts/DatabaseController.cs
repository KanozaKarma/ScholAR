using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

namespace GoogleARCore.Examples.ScholAR
{
    public class DatabaseController : MonoBehaviour
    {
        public Button SignUpButton;
        public Button RegisterButton;

        public Button LogInButton;
        public Button ConfirmLogInButton;

        public Button CancelButton;
        public Button QuitButton;

        public InputField LogInUsername;
        public InputField LogInPassword;

        public InputField SignUpUsername;
        public InputField SignUpPassword;
        public InputField ConfirmPassword;
        public InputField SignUpEmail;

        public GameObject MainMenu;
        public GameObject SignUpMenu;
        public GameObject LogInMenu;

        private bool m_IsQuitting = false;

        public void Update()
        {
            _UpdateApplicationLifecycle();

            SignUpButton.onClick.AddListener(SignUpHandler);
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://scholar-ac37c.firebaseio.com/");
            LogInButton.onClick.AddListener(LogInHandler);
            QuitButton.onClick.AddListener(_DoQuit);
        }

        public void TakeInfo()
        {
            LogInButton.onClick.AddListener(LogInHandler);
        }

        public void SignUpHandler()
        {
            MainMenu.SetActive(false);
            LogInMenu.SetActive(false);
            SignUpMenu.SetActive(true);

            CancelButton.onClick.AddListener(Cancel);
            RegisterButton.onClick.AddListener(CheckSignUp);

        }

        private void LogInHandler()
        {
            MainMenu.SetActive(false);
            LogInMenu.SetActive(true);
            SignUpMenu.SetActive(false);

            CancelButton.onClick.AddListener(Cancel);
            ConfirmLogInButton.onClick.AddListener(CheckLogIn);
        }

        public void CheckSignUp()
        {
            string Username = SignUpUsername.text;
            string Password = SignUpPassword.text;
            string confirmPassword = ConfirmPassword.text;
            string email = SignUpEmail.text;
            string userid = "";

            if (Username.Equals(null))
            {
                _ShowAndroidToastMessage("Please Enter a Username");
                SignUpHandler();
            }

            if (Password.Equals(null))
            {
                _ShowAndroidToastMessage("Please Enter a Password");
                SignUpHandler();
            }

            if (email.Equals(null))
            {
                _ShowAndroidToastMessage("Please Enter an Email");
                SignUpHandler();
            }

            if(!email.Contains("@"))
            {
                _ShowAndroidToastMessage("Please enter a valid email");
                SignUpHandler();
            }

            if (!Password.Equals(confirmPassword))
            {
                _ShowAndroidToastMessage("The passwords do not match");
                Debug.Log("The passwords do not match");
            }
            else
            {
                User user = new User(userid, Username, Password, email);
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://scholar-ac37c.firebaseio.com/");
                //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

                WriteNewUser(user);
            }
        }

        public void CheckLogIn()
        {
            bool isLoggedIn = false;
            string Username = LogInUsername.text;
            string Password = LogInPassword.text;
            string key;
            //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            //DatabaseReference data = reference
            FirebaseDatabase database = FirebaseDatabase.GetInstance("https://scholar-ac37c.firebaseio.com");
            DatabaseReference reference = database.GetReference("Users");
            DatabaseReference achievement = database.GetReference("Achievements");
            Query query = reference.OrderByChild("Username").EqualTo(Username.ToString());
            //query.ValueChanged()
            //if (Username.Equals(reference.OrderByChild(Username)) && Password)#

            FirebaseDatabase.DefaultInstance.GetReference("Users").OrderByChild(Username).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    //_ShowAndroidToastMessage("Incorrect Log-in information");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (Password.Equals(snapshot.Child("Password").GetRawJsonValue()))
                    {
                        isLoggedIn = true;
                        key = snapshot.Child("userId").GetRawJsonValue();

                        System.IO.File.WriteAllText(@"userdetails.txt", key);
                        //string achievementNo = achievement.Child("Achievements").
                    }
                    LogInMenu.SetActive(false);
                    MainMenu.SetActive(true);
                }

            });
        }

        private void WriteNewUser(User user)
        {
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://scholar-ac37c.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

            string key = reference.Child("Users").Push().Key;
            user.SetUserId(key);
            string json = JsonUtility.ToJson(user);
            reference.Child("Users").Child(key).SetRawJsonValueAsync(json);

            _ShowAndroidToastMessage("Sign Up successful! Please log-in with your new details");
            SignUpMenu.SetActive(false);
            MainMenu.SetActive(true);
            Update();
        }

        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting. Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        private void Cancel()
        {
            MainMenu.SetActive(true);
            LogInMenu.SetActive(false);
            SignUpMenu.SetActive(false);
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
