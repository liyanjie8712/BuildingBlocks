using System;
using System.Globalization;
using System.IO;

namespace Liyanjie.BEncoding
{
    public class BInteger : IBEncodingType, IComparable<long>, IEquatable<long>, IEquatable<BInteger>, IComparable<BInteger>
    {
        private BInteger()
        {
            Value = 0;
        }
        public BInteger(long value)
        {
            Value = value;
        }

        public long Value { get; set; }

        /// <summary>
        /// Decode the next token as a int.
        /// Assumes the next token is a int.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="bytesConsumed"></param>
        /// <returns>Decoded int</returns>
        public static BInteger Decode(BinaryReader inputStream, ref int bytesConsumed)
        {
            // Get past 'i'
            inputStream.Read();
            bytesConsumed++;

            // Read numbers till an 'e'
            string number = "";
            char ch;

            while ((ch = inputStream.ReadChar()) != 'e')
            {
                number += ch;

                bytesConsumed++;
            }

            bytesConsumed++;

            BInteger res = new BInteger { Value = long.Parse(number) };

            return res;
        }

        public void Encode(BinaryWriter writer)
        {
            // Write header
            writer.Write('i');

            // Write value
            writer.Write(Value.ToString(CultureInfo.InvariantCulture).ToCharArray());

            // Write footer
            writer.Write('e');
        }

        public int CompareTo(long other)
        {
            if (Value < other)
                return -1;

            if (Value > other)
                return 1;

            return 0;
        }
        public int CompareTo(BInteger other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (Value < other.Value)
                return -1;

            if (Value > other.Value)
                return 1;

            return 0;
        }

        public bool Equals(BInteger other)
        {
            if (other == null)
                return false;

            return Equals(other.Value);
        }
        public bool Equals(long other)
        {
            return Value == other;
        }

        public override bool Equals(object obj)
        {
            BInteger other = obj as BInteger;

            return Equals(other);
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("{0}", Value);
        }

        public static implicit operator BInteger(long x)
        {
            return new BInteger(x);
        }
        public static implicit operator long(BInteger x)
        {
            return x.Value;
        }
        public static implicit operator BInteger(int x)
        {
            return new BInteger(x);
        }
        public static implicit operator int(BInteger x)
        {
            return (int)x.Value;
        }
    }
}
