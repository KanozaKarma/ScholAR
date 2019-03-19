using System.Collections;
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

        private bool m_IsQuitting = false;

        //FirebaseApp.
        //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        public void Update()
        {
            _UpdateApplicationLifecycle();

            SignUpButton.onClick.AddListener(SignUpHandler);
            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://scholar-ac37c.firebaseio.com/");
        }

        public void TakeInfo()
        {
            SignUpButton.onClick.AddListener(SignUpHandler);
        }

        public void SignUpHandler()
        {
            MainMenu.SetActive(false);
            SignUpMenu.SetActive(true);

            RegisterButton.onClick.AddListener(CheckSignUp);

        }

        public void CheckSignUp()
        {
            string Username = SignUpUsername.text;
            string Password = SignUpPassword.text;
            string confirmPassword = ConfirmPassword.text;
            string email = SignUpEmail.text;

            if (Username.Equals(null))
            {
                _ShowAndroidToastMessage("Please Enter a Username");
            }

            if (Password.Equals(null))
            {
                _ShowAndroidToastMessage("Please Enter a Password");
            }

            if (email.Equals(null))
            {
                _ShowAndroidToastMessage("Please Enter an Email");
            }

            if(!email.Contains("@"))
            {
                _ShowAndroidToastMessage("Please enter a valid email");
            }

            if (!Password.Equals(confirmPassword))
            {
                _ShowAndroidToastMessage("The passwords do not match");
                Debug.Log("The passwords do not match");
            }
            else
            {
                User user = new User(Username, Password, email);
                //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://scholar-ac37c.firebaseio.com/");
                //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

                writeNewUser(user);
            }
        }

        private void writeNewUser(User user)
        {
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://scholar-ac37c.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            string json = JsonUtility.ToJson(user);

            //reference.Child("Users").Push();
            string key = reference.Child("Users").Push().Key;
            reference.Child("Users").Child(key).SetRawJsonValueAsync(json);
            reference.Child("Users").Child(key).SetValueAsync(key);
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
