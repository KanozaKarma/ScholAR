﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

namespace GoogleARCore.Examples.HelloAR
{
    public class DatabaseController : MonoBehaviour
    {
        public Button SignUpButton;
        public Button RegisterButton;
        public Button LogInButton;
        public Button ConfirmLogInButton;
        //public Text Username;
        //public Text Password;
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

        //FirebaseApp.
        //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        public void Update()
        {
            _UpdateApplicationLifecycle();

            SignUpButton.onClick.AddListener(SignUpHandler);
            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://scholar-ac37c.firebaseio.com/");
            LogInButton.onClick.AddListener(LogInHandler);
        }

        public void TakeInfo()
        {
            LogInButton.onClick.AddListener(LogInHandler);
        }

        public void SignUpHandler()
        {
            MainMenu.SetActive(false);
            SignUpMenu.SetActive(true);

            RegisterButton.onClick.AddListener(CheckSignUp);

        }

        private void LogInHandler()
        {
            MainMenu.SetActive(false);
            LogInMenu.SetActive(true);

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
                User user = new User(Username, Password, email, userid);
                //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://scholar-ac37c.firebaseio.com/");
                //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

                WriteNewUser(user);
            }
        }

        public void CheckLogIn()
        {

        }

        private void WriteNewUser(User user)
        {
            //Dictionary<string, Object> dict = new Dictionary<string, Object>();
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://scholar-ac37c.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            //string json = JsonUtility.ToJson(user);

            //reference.Child("Users").Push();
            string key = reference.Child("Users").Push().Key;
            user.SetUserId(key);
            string json = JsonUtility.ToJson(user);
            reference.Child("Users").Child(key).SetRawJsonValueAsync(json);
            //reference.Child("Users").Child(key).SetValueAsync();

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
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
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
