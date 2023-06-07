using System;

namespace Webapi
{
    public class UserActivityTracker
    {
        public event EventHandler<string> UserLoggedIn;
        public event EventHandler<string> UserLoggedOut;
        public event EventHandler<string> UserRegistered;

        public void OnUserLoggedIn(string username)
        {
            UserLoggedIn?.Invoke(this, username);
        }

        public void OnUserLoggedOut(string username)
        {
            UserLoggedOut?.Invoke(this, username);
        }

        public void OnUserRegistered(string username)
        {
            UserRegistered?.Invoke(this, username);
        }
    }
}
