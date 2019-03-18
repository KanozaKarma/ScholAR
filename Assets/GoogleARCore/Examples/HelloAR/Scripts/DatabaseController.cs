using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

namespace GoogleARCore.Examples.HelloAR
{
    public class DatabaseController : MonoBehaviour
    {
        public Button SignUpButton;
        //public Text Username;
        //public Text Password;
        public InputField LogInUsername;
        public InputField LogInPassword;

        public InputField SignUpUsername;
        public InputField SignUpPassword;
        public InputField ConfirmPassword;
        public InputField SignUpEmail;

        public void TakeInfo()
        {
            SignUpButton.onClick.AddListener(SignUpHandler);
        }
        public void SignUpHandler()
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

            if (!Password.Equals(confirmPassword))
            {
                _ShowAndroidToastMessage("The passwords do not match");
                Debug.Log("The passwords do not match");
            }
            else
            {
                User user = new User(Username, Password, email);
                //DatabaseReference databaseReference = FirebaseDatabase.GetInstance("https://scholar-ac37c.firebaseio.com/").;
            }

            //public 

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
}
