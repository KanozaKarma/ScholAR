using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

namespace GoogleARCore.Examples.ScholAR
{
    public class User : MonoBehaviour
    {
        public string Username;
        public string Password;
        public string Email;
        public string userId;

        public User(string userid, string name, string pass, string email)
        {
            this.userId = userid;
            this.Username = name;
            this.Password = pass;
            this.Email = email;
        }

        public void SetUserId(string userid)
        {
            this.userId = userid;
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

        public string GetUserId()
        {
            return (userId);
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
