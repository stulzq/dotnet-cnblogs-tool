// #region File Annotation
// 
// Author：Zhiqiang Li
// 
// FileName：TeaHelper.cs
// 
// Project：CnBlogPublishTool
// 
// CreateDate：2018/05/15
// 
// Note: The reference to this document code must not delete this note, and indicate the source!
// 
// #endregion

using System;

namespace CnBlogPublishTool.Util
{
    /// <summary>
	/// TEA（Tiny Encryption Algorithm）是一种小型的对称加密解密算法，支持128位密码，
	/// 与BlowFish一样TEA每次只能加密/解密8字节数据。TEA特点是速度快、效率高，实现也
	/// 非常简单。由于针对TEA的攻击不断出现，所以TEA也发展出几个版本，分别是XTEA、Block TEA和XXTEA。
	/// Copy from http://www.cnblogs.com/linzheng/archive/2011/09/14/2176767.html
	/// </summary>
	public class TeaHelper
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="key">key  1-16 character</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, byte[] key)
        {

            byte[] dataBytes;
            if (data.Length % 2 == 0)
            {
                dataBytes = data;

            }
            else
            {
                dataBytes = new byte[data.Length + 1];
                Array.Copy(data, 0, dataBytes, 0, data.Length);
                dataBytes[data.Length] = 0x0;

            }
            byte[] result = new byte[dataBytes.Length * 4];
            uint[] formattedKey = FormatKey(key);
            uint[] tempData = new uint[2];
            for (int i = 0; i < dataBytes.Length; i += 2)
            {
                tempData[0] = dataBytes[i];
                tempData[1] = dataBytes[i + 1];
                Encrypt(tempData, formattedKey);
                Array.Copy(ConvertUIntToByteArray(tempData[0]), 0, result, i * 4, 4);
                Array.Copy(ConvertUIntToByteArray(tempData[1]), 0, result, i * 4 + 4, 4);
            }
            return result;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="key">密钥 1-16 字符</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            uint[] formattedKey = FormatKey(key);
            int x = 0;
            uint[] tempData = new uint[2];
            byte[] dataBytes = new byte[data.Length / 8 * 2];
            for (int i = 0; i < data.Length; i += 8)
            {
                tempData[0] = ConvertByteArrayToUInt(data, i);
                tempData[1] = ConvertByteArrayToUInt(data, i + 4);
                Decode(tempData, formattedKey);
                dataBytes[x++] = (byte)tempData[0];
                dataBytes[x++] = (byte)tempData[1];
            }
            //修剪添加的空字符
            if (dataBytes[dataBytes.Length - 1] == 0x0)
            {
                byte[] result = new byte[dataBytes.Length - 1];
                Array.Copy(dataBytes, 0, result, 0, dataBytes.Length - 1);
                return result;
            }
            return dataBytes;

        }

        private static uint[] FormatKey(byte[] key)
        {
            if (key.Length == 0)
                throw new ArgumentException("Key must be between 1 and 16 characters in length");
            byte[] refineKey = new byte[16];
            if (key.Length < 16)
            {
                Array.Copy(key, 0, refineKey, 0, key.Length);
                for (int k = key.Length; k < 16; k++)
                {
                    refineKey[k] = 0x20;
                }
            }
            else
            {
                Array.Copy(key, 0, refineKey, 0, 16);
            }
            uint[] formattedKey = new uint[4];
            int j = 0;
            for (int i = 0; i < refineKey.Length; i += 4)
                formattedKey[j++] = ConvertByteArrayToUInt(refineKey, i);
            return formattedKey;
        }

        #region Tea Algorithm
        static void Encrypt(uint[] v, uint[] k)
        {
            uint y = v[0];
            uint z = v[1];
            uint sum = 0;
            uint delta = 0x9e3779b9;
            uint n = 16;
            while (n-- > 0)
            {
                sum += delta;
                y += (z << 4) + k[0] ^ z + sum ^ (z >> 5) + k[1];
                z += (y << 4) + k[2] ^ y + sum ^ (y >> 5) + k[3];
            }
            v[0] = y;
            v[1] = z;
        }

        static void Decode(uint[] v, uint[] k)
        {
            uint n = 16;
            uint sum;
            uint y = v[0];
            uint z = v[1];
            uint delta = 0x9e3779b9;
            /*
            * 由于进行16轮运算，所以将delta左移4位，减16次后刚好为0.
            */
            sum = delta << 4;
            while (n-- > 0)
            {
                z -= (y << 4) + k[2] ^ y + sum ^ (y >> 5) + k[3];
                y -= (z << 4) + k[0] ^ z + sum ^ (z >> 5) + k[1];
                sum -= delta;
            }
            v[0] = y;
            v[1] = z;
        }
        #endregion

        private static byte[] ConvertUIntToByteArray(uint v)
        {
            byte[] result = new byte[4];
            result[0] = (byte)(v & 0xFF);
            result[1] = (byte)((v >> 8) & 0xFF);
            result[2] = (byte)((v >> 16) & 0xFF);
            result[3] = (byte)((v >> 24) & 0xFF);
            return result;
        }

        private static uint ConvertByteArrayToUInt(byte[] v, int offset)
        {
            if (offset + 4 > v.Length) return 0;
            uint output;
            output = (uint)v[offset];
            output |= (uint)(v[offset + 1] << 8);
            output |= (uint)(v[offset + 2] << 16);
            output |= (uint)(v[offset + 3] << 24);
            return output;
        }
    }
}