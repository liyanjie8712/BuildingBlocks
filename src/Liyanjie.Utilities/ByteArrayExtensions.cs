using System.IO;
using System.Text;

using Liyanjie.Utilities;

namespace System.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodeMode"></param>
        /// <returns></returns>
        public static byte[] Encode(this byte[] input, EncodeMode encodeMode)
        {
            using HashAlgorithm encoder = encodeMode switch
            {
                EncodeMode.MD5 => MD5.Create(),
                EncodeMode.SHA1 => SHA1.Create(),
                EncodeMode.SHA256 => SHA256.Create(),
                EncodeMode.SHA384 => SHA384.Create(),
                EncodeMode.SHA512 => SHA512.Create(),
                _ => null,
            };
            return encoder?.ComputeHash(input);
        }

        #region Aes

        /// <summary>
        /// Aes加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">长度32字符</param>
        /// <param name="iv">长度16字符，为空时使用ECB模式，非空时使用CBC模式</param>
        /// <returns></returns>
        public static byte[] AesEncrypt(this byte[] input, string key, string iv = null)
        {
            using var aes = CreateAes(key, iv);
            using var encryptor = aes.CreateEncryptor();
            return encryptor.TransformFinalBlock(input, 0, input.Length);
        }

        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">长度32字符</param>
        /// <param name="iv">长度16字符，为空时使用ECB模式，非空时使用CBC模式</param>
        /// <returns></returns>
        public static byte[] AesDecrypt(this byte[] input, string key, string iv = null)
        {
            using var aes = CreateAes(key, iv);
            using var decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(input, 0, input.Length);
        }

        static Aes CreateAes(string key, string iv)
        {
            var aes = Aes.Create();
            aes.Mode = string.IsNullOrWhiteSpace(iv) ? CipherMode.ECB : CipherMode.CBC;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Padding = PaddingMode.PKCS7;
            if (!string.IsNullOrWhiteSpace(iv))
                aes.IV = Encoding.UTF8.GetBytes(iv);
            return aes;
        }

        #endregion

        #region TripleDES

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="iv">长度8字符</param>
        /// <param name="key">长度24字符</param>
        /// <returns></returns>
        public static byte[] TripleDESEncrypt(this byte[] input, string key, string iv = null)
        {
            using var tripleDES = CreateTripleDES(key, iv);
            using var encryptor = tripleDES.CreateEncryptor();
            return encryptor.TransformFinalBlock(input, 0, input.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="iv">长度8字符</param>
        /// <param name="key">长度24字符</param>
        /// <returns></returns>
        public static byte[] TripleDESDecrypt(this byte[] input, string key, string iv = null)
        {
            using var tripleDES = CreateTripleDES(key, iv);
            using var decryptor = tripleDES.CreateDecryptor();
            return decryptor.TransformFinalBlock(input, 0, input.Length);
        }

        static TripleDES CreateTripleDES(string key, string iv)
        {
            var tripleDES = TripleDES.Create();
            tripleDES.Mode = string.IsNullOrWhiteSpace(iv) ? CipherMode.ECB : CipherMode.CBC;
            tripleDES.Key = Encoding.UTF8.GetBytes(key);
            tripleDES.Padding = PaddingMode.PKCS7;
            if (!string.IsNullOrWhiteSpace(iv))
                tripleDES.IV = Encoding.UTF8.GetBytes(iv);
            return tripleDES;
        }

        #endregion

        #region RSA

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="publicKeyString">公钥</param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(this byte[] input, string publicKeyString)
            => _RSAEncrypt(input, publicKeyString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKeyString">密钥</param>
        /// <returns></returns>
        public static byte[] RSADecrypt(this byte[] input, string privateKeyString)
            => _RSADecrypt(input, privateKeyString);

#if NETSTANDARD1_3 || NETSTANDARD2_0
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="publicKeyString">公钥</param>
        /// <param name="encryptionPadding">OaepSHA1|OaepSHA256|OaepSHA384|OaepSHA512|Pkcs1</param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(this byte[] input, string publicKeyString, RSAEncryptionPadding encryptionPadding)
            => _RSAEncrypt(input, publicKeyString, encryptionPadding);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKeyString">密钥</param>
        /// <param name="encryptionPadding">OaepSHA1|OaepSHA256|OaepSHA384|OaepSHA512|Pkcs1</param>
        /// <returns></returns>
        public static byte[] RSADecrypt(this byte[] input, string privateKeyString, RSAEncryptionPadding encryptionPadding)
            => _RSADecrypt(input, privateKeyString, encryptionPadding);
#endif

        static byte[] _RSAEncrypt(this byte[] input, string publicKeyString
#if NETSTANDARD1_3 || NETSTANDARD2_0
            , RSAEncryptionPadding encryptionPadding = null
#endif
            )
        {
#if NETSTANDARD1_3 || NETSTANDARD2_0
            encryptionPadding = encryptionPadding ?? RSAEncryptionPadding.Pkcs1;
#endif
            using var rsa = CreateRSA(publicKeyString);
            var bufferSize = rsa.KeySize / 8 - 11;    //加密块最大长度限制

            if (input.Length <= bufferSize)
            {
                return
#if NETSTANDARD1_3 || NETSTANDARD2_0
                    rsa.Encrypt(input, encryptionPadding)
#elif NET45
                    rsa.EncryptValue(input)
#endif
                    ;
            }
            else
            {
                using var originalStream = new MemoryStream(input);
                using var encryptedStream = new MemoryStream();
                var buffer = new byte[bufferSize];
                var readSize = originalStream.Read(buffer, 0, bufferSize);

                while (readSize > 0)
                {
                    var tmpBuffer = new byte[readSize];
                    Array.Copy(buffer, 0, tmpBuffer, 0, readSize);
                    var tmpEncrypted =
#if NETSTANDARD1_3 || NETSTANDARD2_0
                        rsa.Encrypt(tmpBuffer, encryptionPadding)
#elif NET45
                        rsa.EncryptValue(tmpBuffer)
#endif
                        ;
                    encryptedStream.Write(tmpEncrypted, 0, tmpEncrypted.Length);

                    readSize = originalStream.Read(buffer, 0, bufferSize);
                }
                return encryptedStream.ToArray();
            }
        }

        static byte[] _RSADecrypt(this byte[] input, string privateKeyString
#if NETSTANDARD1_3 || NETSTANDARD2_0
            , RSAEncryptionPadding encryptionPadding = null
#endif
            )
        {
#if NETSTANDARD1_3 || NETSTANDARD2_0
            encryptionPadding = encryptionPadding ?? RSAEncryptionPadding.Pkcs1;
#endif
            using var rsa = CreateRSA(privateKeyString);
            var maxBlockSize = rsa.KeySize / 8;    //解密块最大长度限制

            if (input.Length <= maxBlockSize)
            {
                return
#if NETSTANDARD1_3 || NETSTANDARD2_0
                    rsa.Decrypt(input, encryptionPadding)
#elif NET45
                    rsa.DecryptValue(input)
#endif
                    ;
            }
            else
            {
                using var encryptedStream = new MemoryStream(input);
                using var decryptedStream = new MemoryStream();
                var buffer = new byte[maxBlockSize];
                var blockSize = encryptedStream.Read(buffer, 0, maxBlockSize);

                while (blockSize > 0)
                {
                    var tmpBuffer = new byte[blockSize];
                    Array.Copy(buffer, 0, tmpBuffer, 0, blockSize);
                    var tmpDecrypted =
#if NETSTANDARD1_3 || NETSTANDARD2_0
                        rsa.Decrypt(tmpBuffer, encryptionPadding)
#elif NET45
                        rsa.DecryptValue(tmpBuffer)
#endif
                        ;
                    decryptedStream.Write(tmpDecrypted, 0, tmpDecrypted.Length);

                    blockSize = encryptedStream.Read(buffer, 0, maxBlockSize);
                }
                return decryptedStream.ToArray();
            }
        }

        static RSA CreateRSA(string key)
        {
            var rsa = RSA.Create();
            rsa.ImportParameters(RSAHelper.DeserializeParameters(key));
            return rsa;
        }

        #endregion
    }
}
