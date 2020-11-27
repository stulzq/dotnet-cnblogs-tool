using System;
using System.IO;
using System.Text;

namespace Dotnetcnblog.Utils
{
    /// <summary>
    /// 获取文件的编码格式
    /// </summary>
    public class FileEncodingType
    {
        /// <summary>
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Encoding GetType(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var r = GetType(fs);
            fs.Close();
            return r;
        }

        /// <summary>
        /// 通过给定的文件流，判断文件的编码类型
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static Encoding GetType(FileStream fs)
        {
            byte[] unicode = {0xFF, 0xFE, 0x41};
            byte[] unicodeBig = {0xFE, 0xFF, 0x00};
            byte[] utf8 = {0xEF, 0xBB, 0xBF}; //带BOM 
            var reVal = Encoding.Default;

            var r = new BinaryReader(fs, Encoding.Default);
            int.TryParse(fs.Length.ToString(), out var i);
            var ss = r.ReadBytes(i);
            if (IsUtf8Bytes(ss) || ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF)
                reVal = Encoding.UTF8;
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
                reVal = Encoding.BigEndianUnicode;
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41) reVal = Encoding.Unicode;
            r.Close();
            return reVal;
        }

        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUtf8Bytes(byte[] data)
        {
            var charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
            foreach (var t in data)
            {
                var curByte = t; //当前分析的字节. 
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前 
                        while (((curByte <<= 1) & 0x80) != 0) charByteCounter++;
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6) return false;
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1 
                    if ((curByte & 0xC0) != 0x80) return false;
                    charByteCounter--;
                }
            }

            if (charByteCounter > 1) throw new Exception("非预期的byte格式");
            return true;
        }
    }
}