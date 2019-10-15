using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Liyanjie.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class RSAHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static (string PublicKey, string PrivateKey) GenerateKeys()
        {
            using var rsa = RSA.Create();
            return (SerializeParameters(rsa.ExportParameters(false)), SerializeParameters(rsa.ExportParameters(true)));
        }

        internal static string SerializeParameters(RSAParameters parameters)
        {
            using var stream = new MemoryStream();
            using var xmlWriter = XmlWriter.Create(stream);
            new XmlSerializer(typeof(RSAParameters)).Serialize(xmlWriter, parameters);
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        internal static RSAParameters DeserializeParameters(string xmlString)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
            using var xmlReader = XmlReader.Create(stream);
            return (RSAParameters)new XmlSerializer(typeof(RSAParameters)).Deserialize(xmlReader);
        }
    }
}
