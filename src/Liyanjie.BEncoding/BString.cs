using System;
using System.Globalization;
using System.IO;

namespace Liyanjie.BEncoding
{
    public class BString : IEquatable<string>, IEquatable<BString>, IComparable<string>, IComparable<BString>, IBEncodingType
    {
        internal BString() { }

        public BString(string value)
        {
            Value = value;
        }

        private string _Value = string.Empty;
        public string Value
        {
            get { return _Value; }
            set
            {
                if (value != null)
                    _Value = value;
            }
        }

        public byte[] ByteValue { get; set; }

        /// <summary>
        /// Decode the next token as a string.
        /// Assumes the next token is a string.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="bytesConsumed"></param>
        /// <returns>Decoded string</returns>
        public static BString Decode(BinaryReader inputStream, ref int bytesConsumed)
        {
            // Read up to ':'
            var numberLength = "";
            char ch;

            while ((ch = inputStream.ReadChar()) != ':')
            {
                numberLength += ch;
                bytesConsumed++;
            }

            bytesConsumed++;

            // Read chars out
            var data = inputStream.ReadBytes(int.Parse(numberLength));
            bytesConsumed += int.Parse(numberLength);
            
            return new BString
            {
                Value = Utils.ExtendedASCIIEncoding.GetString(data),
                ByteValue = data
            };
        }

        public void Encode(BinaryWriter writer)
        {
            var ascii = ByteValue ?? Utils.ExtendedASCIIEncoding.GetBytes(Value);

            // Write length
            writer.Write(Utils.ExtendedASCIIEncoding.GetBytes(ascii.Length.ToString(CultureInfo.InvariantCulture)));

            // Write seperator
            writer.Write(':');

            // Write ASCII representation
            writer.Write(ascii);
        }

        public int CompareTo(string other)
        {
            return StringComparer.InvariantCulture.Compare(Value, other);
        }
        public int CompareTo(BString other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            return CompareTo(other.Value);
        }

        public bool Equals(BString other)
        {
            if (other == null)
                return false;

            if (Equals(other, this))
                return true;

            return Equals(other.Value, Value);
        }
        public bool Equals(string other)
        {
            if (other == null)
                return false;

            return Equals(Value, other);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BString other))
                return false;

            return Equals(other);
        }
        public override int GetHashCode()
        {
            // Value should never be null
            return Value.GetHashCode();
        }
        public override string ToString()
        {
            return Value;
        }

        public static implicit operator BString(string x)
        {
            return new BString(x);
        }
        public static implicit operator string(BString x)
        {
            return x.Value;
        }
    }
}
