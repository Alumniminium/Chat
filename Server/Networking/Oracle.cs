using Server.Entities;

namespace Server.Networking
{
    public static class Oracle
    {
        public static User GetUserFromId(int userId)
        {
            Collections.Users.TryGetValue(userId, out var user);
            return user;
        }
    }
}