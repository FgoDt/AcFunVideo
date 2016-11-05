using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace AcFunVideo.Class
{
   public class Utils
    {
        public static string GetMD5String(string src)
        {
            HashAlgorithmProvider hashAlgorithmProvider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(src, 0);
            IBuffer buffer2 = hashAlgorithmProvider.HashData(buffer);
            return CryptographicBuffer.EncodeToHexString(buffer2);
        }

        public static long currentTimeMillis()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1,0,0,0)).TotalSeconds;
        }

        public static long GetTimeToUNIX(DateTime dt)
        {
            return (long)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        //public static long GetTimeToUNIX(TimeSpan ts)
        //{
        //    return (long)(ts-(new DateTime(1970,1,1,0,0,0))).
        //}

        public static string ACAESENCODE(string value ,string key  )
        {
            var buffKey = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            SymmetricKeyAlgorithmProvider aes =
                SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcb);
            byte[] vds = Encoding.UTF8.GetBytes(value);
            int k = (int)aes.BlockLength;
            int vs = vds.Length;
            int i = vs;
            if (vs%k!=0)
            {
                i = vs + (k - vs % k);
            }
            var data = new byte[i];
            Array.Copy(vds, 0, data, 0, vds.Length);
            var toDecryptBuffer = CryptographicBuffer.CreateFromByteArray(data);
            var symmetricKey = aes.CreateSymmetricKey(buffKey);
            var buffEncrypted = CryptographicEngine.Encrypt(symmetricKey, toDecryptBuffer, null);
            var str = CryptographicBuffer.EncodeToBase64String(buffEncrypted);
            return str;
        }

        public static string ACAESDecode(string value,string key)
        {
            var buffkey = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            IBuffer toDecryptBuffer = CryptographicBuffer.DecodeFromBase64String(value);
            SymmetricKeyAlgorithmProvider aes =
                SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcb);

            // Create a symmetric key.
            var symetricKey = aes.CreateSymmetricKey(buffkey);
            var buffDecrypted = CryptographicEngine.Decrypt(symetricKey, toDecryptBuffer, null);

            string strDecrypted = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffDecrypted);
            return strDecrypted;
        }

    }
}
