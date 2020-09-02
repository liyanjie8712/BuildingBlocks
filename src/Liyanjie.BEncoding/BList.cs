using System;
using System.Collections.Generic;
using System.IO;

namespace Liyanjie.BEncoding
{
    public class BList : List<IBEncodingType>, IEquatable<BList>, IEquatable<IList<IBEncodingType>>, IBEncodingType
    {
        /// <summary>
        /// Decode the next token as a list.
        /// Assumes the next token is a list.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="bytesConsumed"></param>
        /// <returns>Decoded list</returns>
        public static BList Decode(BinaryReader inputStream, ref int bytesConsumed)
        {
            // Get past 'l'
            inputStream.Read();
            bytesConsumed++;

            var list = new BList();

            // Read elements till an 'e'
            while (inputStream.PeekChar() != 'e')
            {
                list.Add(Utils.Decode(inputStream, ref bytesConsumed));
            }

            // Get past 'e'
            inputStream.Read();
            bytesConsumed++;

            return list;
        }

        public void Encode(BinaryWriter writer)
        {
            // Write header
            writer.Write('l');

            // Write elements
            foreach (IBEncodingType item in this)
            {
                item.Encode(writer);
            }

            // Write footer
            writer.Write('e');
        }

        public bool Equals(BList obj)
        {
            IList<IBEncodingType> other = obj;

            return Equals(other);
        }
        public bool Equals(IList<IBEncodingType> other)
        {
            if (other == null)
                return false;

            if (other.Count != Count)
                return false;

            for (int i = 0; i < Count; i++)
            {
                // Lists cannot have nulls
                if (!other[i].Equals(this[i]))
                    // Not ok
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BList other))
                return false;
            return Equals(other);
        }
        public override int GetHashCode()
        {
            var i = 1;

            foreach (var item in this)
            {
                i ^= item.GetHashCode();
            }

            return i;
        }

        /// <summary>
        /// Adds a specified value.
        /// Must not be null.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException">If the value is null</exception>
        public new void Add(IBEncodingType value)
        {
            base.Add(value ?? throw new ArgumentNullException("value"));
        }

        /// <summary>
        /// Adds a range of specified values.
        /// None of them must be null.
        /// </summary>
        /// <param name="values"></param>
        /// <exception cref="ArgumentNullException">If any of the values is null</exception>
        public new void AddRange(IEnumerable<IBEncodingType> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            foreach (IBEncodingType value in values)
            {
                base.Add(value ?? throw new ArgumentNullException("values"));
            }
        }

        /// <summary>
        /// Gets or sets a specified value.
        /// The value must not be null.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If the value is null</exception>
        public new IBEncodingType this[int index]
        {
            get { return base[index]; }
            set { base[index] = value ?? throw new ArgumentNullException("value"); }
        }
    }
}
