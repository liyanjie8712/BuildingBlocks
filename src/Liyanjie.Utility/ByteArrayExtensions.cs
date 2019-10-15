using System.IO;
using System.Text;

using Liyanjie.Utility;

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
        public static byte[] Encoded(this byte[] input, EncodeMode encodeMode)
        {
            HashAlgorithm encoder = null;
            switch (encodeMode)
            {
                case EncodeMode.MD5:
                    encoder = MD5.Create();
                    break;
                case EncodeMode.SHA1:
                    encoder = SHA1.Create();
                    break;
                case EncodeMode.SHA256:
                    encoder = SHA256.Create();
                    break;
                case EncodeMode.SHA384:
                    encoder = SHA384.Create();
                    break;
                case EncodeMode.SHA512:
                    encoder = SHA512.Create();
                    break;
            }
            if (encoder == null)
                return input;
            else
                using (encoder)
                {
                    return encoder.ComputeHash(input);
                }
        }

        #region Aes

        /// <summary>
        /// Aes加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">长度32字符</param>
        /// <param name="iv">长度16字符，为空时使用ECB模式，非空时使用CBC模式</param>
        /// <returns></returns>
        public static byte[] Encrypted_Aes(this byte[] input, string key, string iv = null)
        {
            using (var aes = CreateAes(key, iv))
            using (var encryptor = aes.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(input, 0, input.Length);
            }
        }

        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">长度32字符</param>
        /// <param name="iv">长度16字符，为空时使用ECB模式，非空时使用CBC模式</param>
        /// <returns></returns>
        public static byte[] Decrypted_Aes(this byte[] input, string key, string iv = null)
        {
            using (var aes = CreateAes(key, iv))
            using (var decryptor = aes.CreateDecryptor())
            {
                return decryptor.TransformFinalBlock(input, 0, input.Length);
            }
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
        public static byte[] Encrypted_TripleDES(this byte[] input, string key, string iv = null)
        {
            using (var tripleDES = CreateTripleDES(key, iv))
            using (var encryptor = tripleDES.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(input, 0, input.Length);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="iv">长度8字符</param>
        /// <param name="key">长度24字符</param>
        /// <returns></returns>
        public static byte[] Decrypted_TripleDES(this byte[] input, string key, string iv = null)
        {
            using (var tripleDES = CreateTripleDES(key, iv))
            using (var decryptor = tripleDES.CreateDecryptor())
            {
                return decryptor.TransformFinalBlock(input, 0, input.Length);
            }
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
        public static byte[] Encrypted_RSA(this byte[] input, string publicKeyString)
            => _Encrypted_RSA(input, publicKeyString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKeyString">密钥</param>
        /// <returns></returns>
        public static byte[] Decrypted_RSA(this byte[] input, string privateKeyString)
            => _Decrypted_RSA(input, privateKeyString);

#if NETSTANDARD1_3 || NETSTANDARD2_0
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="publicKeyString">公钥</param>
        /// <param name="encryptionPadding">OaepSHA1|OaepSHA256|OaepSHA384|OaepSHA512|Pkcs1</param>
        /// <returns></returns>
        public static byte[] Encrypted_RSA(this byte[] input, string publicKeyString, RSAEncryptionPadding encryptionPadding)
            => _Encrypted_RSA(input, publicKeyString, encryptionPadding);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKeyString">密钥</param>
        /// <param name="encryptionPadding">OaepSHA1|OaepSHA256|OaepSHA384|OaepSHA512|Pkcs1</param>
        /// <returns></returns>
        public static byte[] Decrypted_RSA(this byte[] input, string privateKeyString, RSAEncryptionPadding encryptionPadding)
            => _Decrypted_RSA(input, privateKeyString, encryptionPadding);
#endif

        static byte[] _Encrypted_RSA(this byte[] input, string publicKeyString
#if NETSTANDARD1_3 || NETSTANDARD2_0
            , RSAEncryptionPadding encryptionPadding = null
#endif
            )
        {
#if NETSTANDARD1_3 || NETSTANDARD2_0
            encryptionPadding = encryptionPadding ?? RSAEncryptionPadding.Pkcs1;
#endif
            byte[] output = null;

            using (var rsa = CreateRSA(publicKeyString))
            {
                var bufferSize = rsa.KeySize / 8 - 11;    //加密块最大长度限制

                if (input.Length <= bufferSize)
                {
                    output =
#if NETSTANDARD1_3 || NETSTANDARD2_0
                        rsa.Encrypt(input, encryptionPadding)
#elif NET45
                        rsa.EncryptValue(input)
#endif
                        ;
                }
                else
                {
                    using (var originalStream = new MemoryStream(input))
                    using (var encryptedStream = new MemoryStream())
                    {
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
                        output = encryptedStream.ToArray();
                    }
                }
            }

            return output;
        }

        static byte[] _Decrypted_RSA(this byte[] input, string privateKeyString
#if NETSTANDARD1_3 || NETSTANDARD2_0
            , RSAEncryptionPadding encryptionPadding = null
#endif
            )
        {
#if NETSTANDARD1_3 || NETSTANDARD2_0
            encryptionPadding = encryptionPadding ?? RSAEncryptionPadding.Pkcs1;
#endif
            byte[] output = null;
            using (var rsa = CreateRSA(privateKeyString))
            {
                var maxBlockSize = rsa.KeySize / 8;    //解密块最大长度限制

                if (input.Length <= maxBlockSize)
                {
                    output =
#if NETSTANDARD1_3 || NETSTANDARD2_0
                        rsa.Decrypt(input, encryptionPadding)
#elif NET45
                        rsa.DecryptValue(input)
#endif
                        ;
                }
                else
                {
                    using (var encryptedStream = new MemoryStream(input))
                    using (var decryptedStream = new MemoryStream())
                    {
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
                        output = decryptedStream.ToArray();
                    }
                }
            }
            return output;
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
