using MetaWeblogClient;

namespace Dotnetcnblog.Command
{
    public class CommandContext
    {
        public string AppConfigFilePath { get; set; }
        public byte[] EncryptKey => new byte[] { 21, 52, 33, 78, 52, 45 };
        public BlogConnectionInfo ConnectionInfo { get; set; }

    }
}