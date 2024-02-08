namespace Sphagnum.Abstractions
{
    public class SphagnumSettings
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Hostname { get; set; } = string.Empty;
        public RequestedUserRights UserRights { get; set; } =
            RequestedUserRights.MessagesConsuming |
            RequestedUserRights.MessagesPublishing;
    }
}
