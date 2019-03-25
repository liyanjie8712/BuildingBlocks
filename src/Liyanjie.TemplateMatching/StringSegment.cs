using System;

namespace Liyanjie.TemplateMatching
{
    public struct StringSegment : IEquatable<StringSegment>, IEquatable<string>
    {
        /// <summary>
        /// Gets the <see cref="T:System.String" /> buffer for this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.
        /// </summary>
        public string Buffer { get; private set; }

        /// <summary>
        /// Gets the offset within the buffer for this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// Gets the length of this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets the value of this segment as a <see cref="T:System.String" />.
        /// </summary>
        public string Value
        {
            get
            {
                if (!this.HasValue)
                {
                    return null;
                }
                return this.Buffer.Substring(this.Offset, this.Length);
            }
        }

        /// <summary>
        /// Gets whether or not this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> contains a valid value.
        /// </summary>
        public bool HasValue
        {
            get
            {
                return this.Buffer != null;
            }
        }

        /// <summary>
        /// Initializes an instance of the <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> struct.
        /// </summary>
        /// <param name="buffer">
        /// The original <see cref="T:System.String" />. The <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> includes the whole <see cref="T:System.String" />.
        /// </param>
        public StringSegment(string buffer)
        {
            this.Buffer = buffer;
            this.Offset = 0;
            this.Length = ((buffer != null) ? buffer.Length : 0);
        }

        /// <summary>
        /// Initializes an instance of the <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> struct.
        /// </summary>
        /// <param name="buffer">The original <see cref="T:System.String" /> used as buffer.</param>
        /// <param name="offset">The offset of the segment within the <paramref name="buffer" />.</param>
        /// <param name="length">The length of the segment.</param>
        public StringSegment(string buffer, int offset, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            if (offset + length > buffer.Length)
            {
                throw new ArgumentException("InvalidOffsetLength");
            }
            this.Buffer = buffer;
            this.Offset = offset;
            this.Length = length;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj != null && obj is StringSegment && this.Equals((StringSegment)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><code>true</code> if the current object is equal to the other parameter; otherwise, <code>false</code>.</returns>
        public bool Equals(StringSegment other)
        {
            return this.Equals(other, StringComparison.Ordinal);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns><code>true</code> if the current object is equal to the other parameter; otherwise, <code>false</code>.</returns>
        public bool Equals(StringSegment other, StringComparison comparisonType)
        {
            int length = other.Length;
            return this.Length == length && string.Compare(this.Buffer, this.Offset, other.Buffer, other.Offset, length, comparisonType) == 0;
        }

        /// <summary>
        /// Checks if the specified <see cref="T:System.String" /> is equal to the current <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.
        /// </summary>
        /// <param name="text">The <see cref="T:System.String" /> to compare with the current <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.</param>
        /// <returns><code>true</code> if the specified <see cref="T:System.String" /> is equal to the current <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />; otherwise, <code>false</code>.</returns>
        public bool Equals(string text)
        {
            return this.Equals(text, StringComparison.Ordinal);
        }

        /// <summary>
        /// Checks if the specified <see cref="T:System.String" /> is equal to the current <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.
        /// </summary>
        /// <param name="text">The <see cref="T:System.String" /> to compare with the current <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns><code>true</code> if the specified <see cref="T:System.String" /> is equal to the current <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />; otherwise, <code>false</code>.</returns>
        public bool Equals(string text, StringComparison comparisonType)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            int length = text.Length;
            return this.HasValue && this.Length == length && string.Compare(this.Buffer, this.Offset, text, 0, length, comparisonType) == 0;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This GetHashCode is expensive since it allocates on every call.
        /// However this is required to ensure we retain any behavior (such as hash code randomization) that
        /// string.GetHashCode has.
        /// </remarks>
        public override int GetHashCode()
        {
            if (!this.HasValue)
            {
                return 0;
            }
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Checks if two specified <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> have the same value.
        /// </summary>
        /// <param name="left">The first <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> to compare, or <code>null</code>.</param>
        /// <param name="right">The second <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> to compare, or <code>null</code>.</param>
        /// <returns><code>true</code> if the value of <paramref name="left" /> is the same as the value of <paramref name="right" />; otherwise, <code>false</code>.</returns>
        public static bool operator ==(StringSegment left, StringSegment right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks if two specified <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> have different values.
        /// </summary>
        /// <param name="left">The first <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> to compare, or <code>null</code>.</param>
        /// <param name="right">The second <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> to compare, or <code>null</code>.</param>
        /// <returns><code>true</code> if the value of <paramref name="left" /> is different from the value of <paramref name="right" />; otherwise, <code>false</code>.</returns>
        public static bool operator !=(StringSegment left, StringSegment right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Checks if the beginning of this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> matches the specified <see cref="T:System.String" /> when compared using the specified <paramref name="comparisonType" />.
        /// </summary>
        /// <param name="text">The <see cref="T:System.String" />to compare.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns><code>true</code> if <paramref name="text" /> matches the beginning of this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />; otherwise, <code>false</code>.</returns>
        public bool StartsWith(string text, StringComparison comparisonType)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            int length = text.Length;
            return this.HasValue && this.Length >= length && string.Compare(this.Buffer, this.Offset, text, 0, length, comparisonType) == 0;
        }

        /// <summary>
        /// Checks if the end of this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> matches the specified <see cref="T:System.String" /> when compared using the specified <paramref name="comparisonType" />.
        /// </summary>
        /// <param name="text">The <see cref="T:System.String" />to compare.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns><code>true</code> if <paramref name="text" /> matches the end of this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />; otherwise, <code>false</code>.</returns>
        public bool EndsWith(string text, StringComparison comparisonType)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            int length = text.Length;
            return this.HasValue && this.Length >= length && string.Compare(this.Buffer, this.Offset + this.Length - length, text, 0, length, comparisonType) == 0;
        }

        /// <summary>
        /// Retrieves a substring from this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.
        /// The substring starts at the position specified by <paramref name="offset" /> and has the specified <paramref name="length" />.
        /// </summary>
        /// <param name="offset">The zero-based starting character position of a substring in this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A <see cref="T:System.String" /> that is equivalent to the substring of length <paramref name="length" /> that begins at <paramref name="offset" /> in this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /></returns>
        public string Substring(int offset, int length)
        {
            if (!this.HasValue)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (offset < 0 || offset + length > this.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (length < 0 || this.Offset + offset + length > this.Buffer.Length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            return this.Buffer.Substring(this.Offset + offset, length);
        }

        /// <summary>
        /// Retrieves a <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> that represents a substring from this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.
        /// The <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> starts at the position specified by <paramref name="offset" /> and has the specified <paramref name="length" />.
        /// </summary>
        /// <param name="offset">The zero-based starting character position of a substring in this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> that is equivalent to the substring of length <paramref name="length" /> that begins at <paramref name="offset" /> in this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /></returns>
        public StringSegment Subsegment(int offset, int length)
        {
            if (!this.HasValue)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (offset < 0 || offset + length > this.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (length < 0 || this.Offset + offset + length > this.Buffer.Length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            return new StringSegment(this.Buffer, this.Offset + offset, length);
        }

        /// <summary>
        /// Gets the zero-based index of the first occurrence of the character <paramref name="c" /> in this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.
        /// The search starts at <paramref name="start" /> and examines a specified number of <paramref name="count" /> character positions.
        /// </summary>
        /// <param name="c">The Unicode character to seek.</param>
        /// <param name="start">The zero-based index position at which the search starts. </param>
        /// <param name="count">The number of characters to examine.</param>
        /// <returns>The zero-based index position of <paramref name="c" /> from the beginning of the <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> if that character is found, or -1 if it is not.</returns>
        public int IndexOf(char c, int start, int count)
        {
            if (start < 0 || this.Offset + start > this.Buffer.Length)
            {
                throw new ArgumentOutOfRangeException("start");
            }
            if (count < 0 || this.Offset + start + count > this.Buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            int num = this.Buffer.IndexOf(c, start + this.Offset, count);
            if (num != -1)
            {
                return num - this.Offset;
            }
            return num;
        }

        /// <summary>
        /// Gets the zero-based index of the first occurrence of the character <paramref name="c" /> in this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.
        /// The search starts at <paramref name="start" />.
        /// </summary>
        /// <param name="c">The Unicode character to seek.</param>
        /// <param name="start">The zero-based index position at which the search starts. </param>
        /// <returns>The zero-based index position of <paramref name="c" /> from the beginning of the <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> if that character is found, or -1 if it is not.</returns>
        public int IndexOf(char c, int start)
        {
            return this.IndexOf(c, start, this.Length - start);
        }

        /// <summary>
        /// Gets the zero-based index of the first occurrence of the character <paramref name="c" /> in this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.
        /// </summary>
        /// <param name="c">The Unicode character to seek.</param>
        /// <returns>The zero-based index position of <paramref name="c" /> from the beginning of the <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> if that character is found, or -1 if it is not.</returns>
        public int IndexOf(char c)
        {
            return this.IndexOf(c, 0, this.Length);
        }

        /// <summary>
        /// Removes all leading and trailing whitespaces.
        /// </summary>
        /// <returns>The trimmed <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.</returns>
        public StringSegment Trim()
        {
            return this.TrimStart().TrimEnd();
        }

        /// <summary>
        /// Removes all leading whitespaces.
        /// </summary>
        /// <returns>The trimmed <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.</returns>
        public StringSegment TrimStart()
        {
            int num = this.Offset;
            while (num < this.Offset + this.Length && char.IsWhiteSpace(this.Buffer, num))
            {
                num++;
            }
            return new StringSegment(this.Buffer, num, this.Offset + this.Length - num);
        }

        /// <summary>
        /// Removes all trailing whitespaces.
        /// </summary>
        /// <returns>The trimmed <see cref="T:Microsoft.Extensions.Primitives.StringSegment" />.</returns>
        public StringSegment TrimEnd()
        {
            int num = this.Offset + this.Length - 1;
            while (num >= this.Offset && char.IsWhiteSpace(this.Buffer, num))
            {
                num--;
            }
            return new StringSegment(this.Buffer, this.Offset, num - this.Offset + 1);
        }

        /// <summary>
        /// Returns the <see cref="T:System.String" /> represented by this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> or <code>String.Empty</code> if the <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> does not contain a value.
        /// </summary>
        /// <returns>The <see cref="T:System.String" /> represented by this <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> or <code>String.Empty</code> if the <see cref="T:Microsoft.Extensions.Primitives.StringSegment" /> does not contain a value.</returns>
        public override string ToString()
        {
            return this.Value ?? string.Empty;
        }
    }
}
