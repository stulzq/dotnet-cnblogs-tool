using System.Drawing;
using Colorful;

namespace Dotnetcnblog.Utils
{
    public class ConsoleHelper
    {
        static readonly Color MsgColor = Color.FromArgb(0, 204, 51);
        static readonly Color ErrorColor = Color.FromArgb(153, 0, 51);

        public static void PrintMsg(string text)
        {
            Console.WriteLine(text, MsgColor);
        }

        public static void PrintError(string text)
        {
            Console.WriteLine(text, ErrorColor);
        }
    }
}