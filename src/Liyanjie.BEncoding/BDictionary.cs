using System;
using System.Collections.Generic;
using System.IO;

namespace Liyanjie.BEncoding
{
    [Serializable]
    public class BDictionary : Dictionary<string, IBEncodingType>, IEquatable<BDictionary>, IEquatable<Dictionary<string, IBEncodingType>>, IBEncodingType
    {
        /// <summary>
        /// Decode the next token as a dictionary.
        /// Assumes the next token is a dictionary.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="bytesConsumed"></param>
        /// <returns>Decoded dictionary</returns>
        public static BDictionary Decode(BinaryReader inputStream, ref int bytesConsumed)
        {
            // Get past 'd'
            inputStream.ReadByte();

            bytesConsumed++;
            var result = new BDictionary();

            // Read elements till an 'e'
            while (inputStream.PeekChar() != 'e')
            {
                var key = BString.Decode(inputStream, ref bytesConsumed);    // Key
                var value = Utils.Decode(inputStream, ref bytesConsumed);  // Value

                result[key.Value] = value;
            }

            // Get past 'e'
            inputStream.Read();
            bytesConsumed++;

            return result;
        }

        public void Encode(BinaryWriter writer)
        {
            // Write header
            writer.Write('d');

            // Write elements
            foreach (KeyValuePair<string, IBEncodingType> item in this)
            {
                // Write key
                var key = new BString
                {
                    Value = item.Key
                };

                key.Encode(writer);

                // Write value
                item.Value.Encode(writer);
            }

            // Write footer
            writer.Write('e');
        }

        public bool Equals(BDictionary obj)
        {
            return Equals((Dictionary<string, IBEncodingType>)obj);
        }
        public bool Equals(Dictionary<string, IBEncodingType> other)
        {
            if (other == null)
                return false;

            if (other.Count != Count)
                return false;

            foreach (var key in Keys)
            {
                if (!other.ContainsKey(key))
                    return false;

                // Dictionaries cannot have nulls
                if (!other[key].Equals(this[key]))
                {
                    // Not ok
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BDictionary other))
                return false;
            return Equals(other);
        }
        public override int GetHashCode()
        {
            var output = 1;
            foreach (var pair in this)
            {
                output ^= pair.GetHashCode();
            }
            return output;
        }

        /// <summary>
        /// Adds a specified value.
        /// Must not be null.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException">If the value is null</exception>
        public new void Add(string key, IBEncodingType value)
        {
            base.Add(key, value ?? throw new ArgumentNullException("value"));
        }

        /// <summary>
        /// Gets or sets a value. 
        /// Values must not be null.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If the value is null</exception>
        public new IBEncodingType this[string index]
        {
            get { return base[index]; }
            set { base[index] = value ?? throw new ArgumentNullException("value"); }
        }
    }
}
