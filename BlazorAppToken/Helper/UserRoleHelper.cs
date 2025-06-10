namespace BlazorApptToken.Helper
{
    public static class UserRoleHelper
    {
        public static bool IsAdmin(string role)
        {
            return role?.ToLower() == "admin";
        }
    }
}
