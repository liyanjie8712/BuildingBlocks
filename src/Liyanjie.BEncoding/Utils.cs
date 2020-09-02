using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Liyanjie.BEncoding
{
    public static class Utils
    {
        public static Encoding ExtendedASCIIEncoding { get; private set; }

        static Utils()
        {
            // Extended ASCII encoding - http://stackoverflow.com/questions/4623650/encode-to-single-byte-extended-ascii-values
            ExtendedASCIIEncoding = Encoding.GetEncoding(1252);
        }

        /// <summary>
        /// Read a file, and parse it a bencoded object.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>A bencoded object.</returns>
        public static IBEncodingType DecodeFile(string filePath)
        {
            using var fileStream = File.OpenRead(filePath);
            return Decode(fileStream);
        }

        /// <summary>
        /// Parse a bencoded object from a string. 
        /// Warning: Beware of encodings.
        /// </summary>
        /// <seealso cref="ExtendedASCIIEncoding"/>
        /// <param name="input">The bencoded string to parse.</param>
        /// <returns>A bencoded object.</returns>
        public static IBEncodingType Decode(string input)
        {
            var bytes = ExtendedASCIIEncoding.GetBytes(input);
            return Decode(bytes);
        }
        public static IBEncodingType Decode(byte[] bytes)
        {
            var bytesConsumed = 0;
            return Decode(bytes, ref bytesConsumed);
        }
        public static IBEncodingType Decode(byte[] bytes, ref int bytesConsumed)
        {
            return Decode(new MemoryStream(bytes), ref bytesConsumed);
        }

        /// <summary>
        /// Parse a bencoded stream (for example a file).
        /// </summary>
        /// <param name="inputStream">The bencoded stream to parse.</param>
        /// <returns>A bencoded object.</returns>
        public static IBEncodingType Decode(Stream inputStream)
        {
            var bytesConsumed = 0;
            return Decode(inputStream, ref bytesConsumed);
        }
        public static IBEncodingType Decode(Stream inputStream, ref int bytesConsumed)
        {
            using var reader = new BinaryReader(inputStream, ExtendedASCIIEncoding);
            return Decode(reader, ref bytesConsumed);
        }
        internal static IBEncodingType Decode(BinaryReader reader, ref int bytesConsumed)
        {
            var next = (char)reader.PeekChar();

            switch (next)
            {
                case 'i':
                    // Integer
                    return BInteger.Decode(reader, ref bytesConsumed);

                case 'l':
                    // List
                    return BList.Decode(reader, ref bytesConsumed);

                case 'd':
                    // List
                    return BDictionary.Decode(reader, ref bytesConsumed);

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    // String
                    return BString.Decode(reader, ref bytesConsumed);
            }

            return null;
        }

        /// <summary>
        /// Encode the given object to a string.
        /// Warning: Beware of encodings, take special care when using it further.
        /// </summary>
        /// <seealso cref="ExtendedASCIIEncoding"/>
        /// <param name="encoder">The bencode object to encode.</param>
        /// <returns>A bencoded string with the object.</returns>
        public static string EncodeString(IBEncodingType encoder)
        {
            using var memory = new MemoryStream();
            Encode(encoder, memory);
            memory.Position = 0;

            using var reader = new StreamReader(memory, ExtendedASCIIEncoding);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Encode the given object to a series of bytes.
        /// </summary>
        /// <param name="encoder">The bencode object to encode.</param>
        /// <returns>A bencoded string of the object in Extended ASCII Encoding.</returns>
        public static byte[] EncodeBytes(IBEncodingType encoder)
        {
            using var memory = new MemoryStream();
            Encode(encoder, memory);
            memory.Position = 0;

            using var reader = new BinaryReader(memory, ExtendedASCIIEncoding);
            return reader.ReadBytes((int)memory.Length);
        }

        /// <summary>
        /// Encode the given object to a stream.
        /// </summary>
        /// <param name="encoder">The object to encode.</param>
        /// <param name="stream">The stream to write to.</param>
        public static void Encode(IBEncodingType encoder, Stream stream)
        {
            var writer = new BinaryWriter(stream, ExtendedASCIIEncoding);
            encoder.Encode(writer);
            writer.Flush();
        }

        /// <summary>
        /// Calculates the InfoHash from a torrent. You must supply the "info" dictionary from the torrent.
        /// </summary>
        /// <param name="torrentInfo">The "info" dictionary.</param>
        /// <example>
        /// This example, will load a torrent, take the "info" dictionary out of the root dictionary and hash this.
        /// <code>
        /// BDict torrent = BencodingUtils.DecodeFile(@"torrentFile.torrent") as BDict;
        /// byte[] infoHash = BencodingUtils.CalculateTorrentInfoHash(torrent["info"] as BDict);
        /// </code>
        /// 
        /// The "infoHash" byte array now contains 20 bytes with the SHA-1 infoHash.
        /// </example>
        /// <returns></returns>
        public static byte[] ComputeTorrentInfoHash(BDictionary torrentInfo)
        {
            // Take the "info" dictionary provided, and encode it
            var infoBytes = EncodeBytes(torrentInfo);

            // Hash the encoded dictionary
            using var sha1 = SHA1.Create();
            return sha1.ComputeHash(infoBytes);
        }
    }
}
