using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

namespace GoogleARCore.Examples.HelloAR
{
    public class User : MonoBehaviour
    {
        public string Username;
        public string Password;
        public string Email;

        public User(string name, string pass, string email)
        {
            Username = name;
            Password = pass;
            Email = email;
        }

        public void SetUsername(string name)
        {
            Username = name;
        }

        public void SetPassword(string pass)
        {
            Password = pass;
        }

        public void SetEmail(string email)
        {
            Email = email;
        }

        public string GetEmail()
        {
            return (Email);
        }

        public string GetUsername()
        {
            return (Username);
        }

        public string GetPassword()
        {
            return (Password);
        }
    }
}
