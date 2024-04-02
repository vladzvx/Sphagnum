namespace Sphagnum.Common.Contracts.Login
{
    public class UserData
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Hostname { get; set; } = string.Empty;
        public UserRights UserRights { get; set; } =
            UserRights.MessagesConsuming |
            UserRights.MessagesPublishing;
    }
}
