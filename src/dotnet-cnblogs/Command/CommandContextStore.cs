namespace Dotnetcnblog.Command
{
    public class CommandContextStore
    {
        private static CommandContext _context;
        public static void Set(CommandContext context)
        {
            _context = context;
        }

        public static CommandContext Get()
        {
            return _context;
        }
    }
}