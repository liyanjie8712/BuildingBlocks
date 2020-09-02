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
            BDictionary res = new BDictionary();

            // Read elements till an 'e'
            while (inputStream.PeekChar() != 'e')
            {
                // Key
                BString key = BString.Decode(inputStream, ref bytesConsumed);

                // Value
                IBEncodingType value = Utils.Decode(inputStream, ref bytesConsumed);

                res[key.Value] = value;
            }

            // Get past 'e'
            inputStream.Read();
            bytesConsumed++;

            return res;
        }

        public void Encode(BinaryWriter writer)
        {
            // Write header
            writer.Write('d');

            // Write elements
            foreach (KeyValuePair<string, IBEncodingType> item in this)
            {
                // Write key
                BString key = new BString
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
            Dictionary<string, IBEncodingType> other = obj;

            return Equals(other);
        }
        public bool Equals(Dictionary<string, IBEncodingType> other)
        {
            if (other == null)
                return false;

            if (other.Count != Count)
                return false;

            foreach (string key in Keys)
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
            BDictionary other = obj as BDictionary;

            return Equals(other);
        }
        public override int GetHashCode()
        {
            var output = 1;
            foreach (KeyValuePair<string, IBEncodingType> pair in this)
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
            if (value == null)
                throw new ArgumentNullException("value");

            base.Add(key, value);
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
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                base[index] = value;
            }
        }
    }
}
