namespace SE1614_Group4_Project_API.Utils
{
    public class Constants
    {
        public enum Role
        {
            Admin,
            Editorial,
            Writer,
            User,
            Guest
        }

        public const string ERR001 = "This field must be fill";
        public const string ERR002 = "User is not exist!";
        public const string ERR003 = "Need to login first";
    }
}