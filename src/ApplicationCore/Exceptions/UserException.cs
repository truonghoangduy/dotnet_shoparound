using System;
namespace ApplicationCore.Exceptions

{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string email) : base("Email : " + email + "Not found")
        {
        }
    }
}