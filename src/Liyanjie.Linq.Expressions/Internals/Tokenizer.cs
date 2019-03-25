using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liyanjie.Linq.Expressions.Exceptions;

namespace Liyanjie.Linq.Expressions.Internals
{
    /// <summary>
    /// 
    /// </summary>
    internal class Tokenizer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IList<Token> Parse(string input)
        {
            var chars = $"{input}\0"
                .Select(_ => new Char
                {
                    Id = _.Id(),
                    Value = _,
                })
                .ToList();
            var tokens = new List<Token>();
            var charId = CharId.Empty;
            var charIndex = 0;
            var sb = new StringBuilder();
            for (int i = 0; i < chars.Count; i++)
            {
                var @char = chars[i];
                switch (charId)
                {
                    case CharId.Unknow:
                        goto Exception;
                    case CharId.Empty:
                        charId = @char.Id;
                        charIndex = i;
                        sb.Clear();
                        sb.Append(@char.Value);
                        break;
                    case CharId.Exclam:
                        #region !
                        if (@char.Id == CharId.Equal)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.NotEqual,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                        }
                        else
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Not,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.DoubleQuote:
                        #region "
                        if (@char.Id == CharId.DoubleQuote)
                        {
                            var s = sb.ToString().Trim('"');
                            tokens.Add(new Token
                            {
                                Id = TokenId.String,
                                Index = charIndex,
                                Length = i - charIndex,
                                Value = s,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else
                            sb.Append(@char.Value);
                        #endregion
                        break;
                    case CharId.Sharp:
                        goto Exception;
                    case CharId.Dollar:
                        #region $
                        if (@char.Id == CharId.Underline || @char.Id == CharId.Letter || @char.Id == CharId.Digit)
                            sb.Append(@char.Value);
                        else
                        {
                            var s = sb.ToString();
                            if (s.Length == 1)
                                tokens.Add(new Token
                                {
                                    Id = TokenId.Parameter,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                    Value = s,
                                });
                            else
                                tokens.Add(new Token
                                {
                                    Id = TokenId.Variable,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                    Value = s,
                                });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.Modulo:
                        #region %
                        if (@char.Id == CharId.Equal)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.ModuloAssign,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Modulo,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.And:
                        #region &
                        if (@char.Id == CharId.And)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.AndAlso,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else if (@char.Id == CharId.Equal)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.AndAssign,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.And,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.SingleQuote:
                        #region '
                        if (@char.Id == CharId.SingleQuote)
                        {
                            var s = sb.ToString().Trim('\'');
                            if (s.Length == 1)
                                tokens.Add(new Token
                                {
                                    Id = TokenId.Char,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                    Value = char.Parse(s),
                                });
                            else
                                tokens.Add(new Token
                                {
                                    Id = TokenId.String,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                    Value = char.Parse(s),
                                });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else
                            sb.Append(@char.Value);
                        #endregion
                        break;
                    case CharId.LeftParenthesis:
                        #region (
                        tokens.Add(new Token
                        {
                            Id = TokenId.LeftParenthesis,
                            Index = charIndex,
                            Length = i - charIndex,
                        });
                        sb.Clear();
                        sb.Append(@char.Value);
                        charId = @char.Id;
                        charIndex = i;
                        #endregion
                        break;
                    case CharId.RightParenthesis:
                        #region )
                        tokens.Add(new Token
                        {
                            Id = TokenId.RightParenthesis,
                            Index = charIndex,
                            Length = i - charIndex,
                        });
                        sb.Clear();
                        sb.Append(@char.Value);
                        charId = @char.Id;
                        charIndex = i;
                        #endregion
                        break;
                    case CharId.Asterisk:
                        #region *
                        if (@char.Id == CharId.Equal)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.MultiplyAssign,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Multiply,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.Plus:
                        #region +
                        if (@char.Id == CharId.Plus)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.IncrementAssign,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else if (@char.Id == CharId.Equal)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.AddAssign,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Add,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.Comma:
                        #region ,
                        tokens.Add(new Token
                        {
                            Id = TokenId.Comma,
                            Index = charIndex,
                            Length = i - charIndex,
                        });
                        sb.Clear();
                        sb.Append(@char.Value);
                        charId = @char.Id;
                        charIndex = i;
                        #endregion
                        break;
                    case CharId.Minus:
                        #region -
                        if (@char.Id == CharId.Minus)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.DecrementAssign,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else if (@char.Id == CharId.Equal)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.SubtractAssign,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Subtract,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.Dot:
                        #region .
                        tokens.Add(new Token
                        {
                            Id = TokenId.Access,
                            Index = charIndex,
                            Length = i - charIndex,
                        });
                        sb.Clear();
                        sb.Append(@char.Value);
                        charId = @char.Id;
                        charIndex = i;
                        #endregion
                        break;
                    case CharId.Slash:
                        #region /
                        if (@char.Id == CharId.Equal)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.DivideAssign,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Divide,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.Digit:
                        #region [0-9]
                        if (@char.Id == CharId.Digit || @char.Id == CharId.Letter || (@char.Id == CharId.Dot && sb.ToString().IndexOf('.') < 0))
                            sb.Append(@char.Value);
                        else
                        {
                            var s = sb.ToString();
                            if (s.EndsWith("M", StringComparison.OrdinalIgnoreCase))
                            {
                                if (decimal.TryParse(s.Substring(0, s.Length - 1), out decimal @uint))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.Decimal,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = @uint,
                                    });
                                else
                                    ThrowConvertException(s, typeof(decimal).Name, input, charIndex);
                            }
                            else if (s.EndsWith("D", StringComparison.OrdinalIgnoreCase))
                            {
                                if (double.TryParse(s.Substring(0, s.Length - 1), out double @uint))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.Double,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = @uint,
                                    });
                                else
                                    ThrowConvertException(s, typeof(double).Name, input, charIndex);
                            }
                            else if (s.EndsWith("F", StringComparison.OrdinalIgnoreCase))
                            {
                                if (float.TryParse(s.Substring(0, s.Length - 1), out float @uint))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.Float,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = @uint,
                                    });
                                else
                                    ThrowConvertException(s, typeof(float).Name, input, charIndex);
                            }
                            else if (s.EndsWith("UL", StringComparison.OrdinalIgnoreCase))
                            {
                                if (ulong.TryParse(s.Substring(0, s.Length - 2), out ulong @uint))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.ULong,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = @uint,
                                    });
                                else
                                    ThrowConvertException(s, typeof(ulong).Name, input, charIndex);
                            }
                            else if (s.EndsWith("L", StringComparison.OrdinalIgnoreCase))
                            {
                                if (long.TryParse(s.Substring(0, s.Length - 1), out long @uint))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.Long,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = @uint,
                                    });
                                else
                                    ThrowConvertException(s, typeof(long).Name, input, charIndex);
                            }
                            else if (s.EndsWith("U", StringComparison.OrdinalIgnoreCase))
                            {
                                if (uint.TryParse(s.Substring(0, s.Length - 1), out uint @uint))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.UInt,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = @uint,
                                    });
                                else if (ulong.TryParse(s.Substring(0, s.Length - 1), out ulong @ulong))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.ULong,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = @ulong,
                                    });
                                else
                                    ThrowConvertException(s, typeof(ulong).Name, input, charIndex);
                            }
                            else if (s.IndexOf('.') > 0)
                            {
                                if (double.TryParse(s, out double @double))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.Double,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = double.Parse(s),
                                    });
                                else
                                    ThrowConvertException(s, typeof(double).Name, input, charIndex);
                            }
                            else
                            {
                                if (int.TryParse(s, out int @int))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.Int,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = @int,
                                    });
                                else if (long.TryParse(s, out long @long))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.Long,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = @long,
                                    });
                                else
                                    ThrowConvertException(s, "Int", input, charIndex);
                            }
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.Colon:
                        #region :
                        tokens.Add(new Token
                        {
                            Id = TokenId.Option,
                            Index = charIndex,
                            Length = i - charIndex,
                        });
                        sb.Clear();
                        sb.Append(@char.Value);
                        charId = @char.Id;
                        charIndex = i;
                        #endregion
                        break;
                    case CharId.Semicolon:
                        goto Exception;
                    case CharId.LessThan:
                        #region <
                        if (@char.Id == CharId.Equal)
                        {
                            if (sb.Length == 2)
                                tokens.Add(new Token
                                {
                                    Id = TokenId.LeftShiftAssign,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                });
                            else
                                tokens.Add(new Token
                                {
                                    Id = TokenId.LessThanOrEqual,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else if (@char.Id == CharId.LessThan)
                            sb.Append(@char.Value);
                        else
                        {
                            if (sb.Length == 2)
                                tokens.Add(new Token
                                {
                                    Id = TokenId.LeftShift,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                });
                            else
                                tokens.Add(new Token
                                {
                                    Id = TokenId.LessThan,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.Equal:
                        #region =
                        if (@char.Id == CharId.Equal)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Equal,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Assign,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.GreaterThan:
                        #region >
                        if (@char.Id == CharId.Equal)
                        {
                            if (sb.Length == 2)
                                tokens.Add(new Token
                                {
                                    Id = TokenId.RightShiftAssign,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                });
                            else
                                tokens.Add(new Token
                                {
                                    Id = TokenId.GreaterThanOrEqual,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else if (@char.Id == CharId.GreaterThan)
                            sb.Append(@char.Value);
                        else
                        {
                            if (sb.Length == 2)
                                tokens.Add(new Token
                                {
                                    Id = TokenId.RightShift,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                });
                            else
                                tokens.Add(new Token
                                {
                                    Id = TokenId.GreaterThan,
                                    Index = charIndex,
                                    Length = i - charIndex,
                                });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.Question:
                        #region ?
                        if (@char.Id == CharId.Dot)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.NullableAccess,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else if (@char.Id == CharId.Question)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Coalesce,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Predicate,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.At:
                        break;
                    case CharId.Letter:
                        #region [a-zA-Z_]
                        if (@char.Id == CharId.Underline || @char.Id == CharId.Letter || @char.Id == CharId.Digit)
                            sb.Append(@char.Value);
                        else
                        {
                            if ((@char.Id == CharId.LeftBrace || @char.Id == CharId.LeftBracket) && sb.ToString().Equals("new", StringComparison.OrdinalIgnoreCase))
                            {
                                if (@char.Id == CharId.LeftBrace)
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.NewObject,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                    });
                                else if (@char.Id == CharId.LeftBracket)
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.NewArray,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                    });
                            }
                            else if (@char.Id == CharId.LeftParenthesis)
                            {
                                var s = sb.ToString().ToUpper();
                                switch (s)
                                {
                                    case "STRING":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseString,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "CHAR":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseChar,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "INT":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseInt,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "UINT":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseUInt,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "LONG":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseLong,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "ULONG":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseULong,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "DOUBLE":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseDouble,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "FLOAT":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseFloat,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "DECIMAL":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseDecimal,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "BOOL":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseBool,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "GUID":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseGuid,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "DATETIME":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseDateTime,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    case "DATETIMEOFFSET":
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.ParseDateTimeOffset,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                        });
                                        break;
                                    default:
                                        tokens.Add(new Token
                                        {
                                            Id = TokenId.MethodCall,
                                            Index = charIndex,
                                            Length = i - charIndex,
                                            Value = s,
                                        });
                                        break;
                                }
                            }
                            else
                            {
                                var s = sb.ToString();
                                if ("true".Equals(s) || "false".Equals(s))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.Bool,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = bool.Parse(s),
                                    });
                                else if ("in".Equals(s))
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.In,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                    });
                                else
                                    tokens.Add(new Token
                                    {
                                        Id = TokenId.Property,
                                        Index = charIndex,
                                        Length = i - charIndex,
                                        Value = s,
                                    });
                            }
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.LeftBracket:
                        #region [
                        if (@char.Id == CharId.LeftBracket)
                            goto Exception;
                        tokens.Add(new Token
                        {
                            Id = TokenId.LeftBracket,
                            Index = charIndex,
                            Length = i - charIndex,
                        });
                        sb.Clear();
                        sb.Append(@char.Value);
                        charId = @char.Id;
                        charIndex = i;
                        #endregion
                        break;
                    case CharId.Backslash:
                        break;
                    case CharId.RightBracket:
                        #region ]
                        tokens.Add(new Token
                        {
                            Id = TokenId.RightBracket,
                            Index = charIndex,
                            Length = i - charIndex,
                        });
                        sb.Clear();
                        sb.Append(@char.Value);
                        charId = @char.Id;
                        charIndex = i;
                        #endregion
                        break;
                    case CharId.Caret:
                        #region ^
                        tokens.Add(new Token
                        {
                            Id = TokenId.ExclusiveOr,
                            Index = charIndex,
                            Length = i - charIndex,
                        });
                        sb.Clear();
                        sb.Append(@char.Value);
                        charId = @char.Id;
                        charIndex = i;
                        #endregion
                        break;
                    case CharId.Underline:
                        #region _
                        if (@char.Id == CharId.Underline || @char.Id == CharId.Letter || @char.Id == CharId.Digit)
                        {
                            sb.Append(@char.Value);
                            charId = CharId.Letter;
                            charIndex = i;
                        }
                        else
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Property,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.Backquote:
                        goto Exception;
                    case CharId.LeftBrace:
                        #region {
                        if (@char.Id == CharId.LeftBrace)
                            goto Exception;
                        tokens.Add(new Token
                        {
                            Id = TokenId.LeftBrace,
                            Index = charIndex,
                            Length = i - charIndex,
                        });
                        sb.Clear();
                        sb.Append(@char.Value);
                        charId = @char.Id;
                        charIndex = i;
                        #endregion
                        break;
                    case CharId.Bar:
                        #region |
                        if (@char.Id == CharId.Bar)
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.OrElse,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            charId = CharId.Empty;
                            charIndex = i;
                        }
                        else
                        {
                            tokens.Add(new Token
                            {
                                Id = TokenId.Or,
                                Index = charIndex,
                                Length = i - charIndex,
                            });
                            sb.Clear();
                            sb.Append(@char.Value);
                            charId = @char.Id;
                            charIndex = i;
                        }
                        #endregion
                        break;
                    case CharId.RightBrace:
                        #region }
                        tokens.Add(new Token
                        {
                            Id = TokenId.RightBrace,
                            Index = charIndex,
                            Length = i - charIndex,
                        });
                        sb.Clear();
                        sb.Append(@char.Value);
                        charId = @char.Id;
                        charIndex = i;
                        #endregion
                        break;
                    case CharId.Tilde:
                        #region ~
                        tokens.Add(new Token
                        {
                            Id = TokenId.BitComplement,
                            Index = charIndex,
                            Length = i - charIndex,
                        });
                        sb.Clear();
                        sb.Append(@char.Value);
                        charId = @char.Id;
                        charIndex = i;
                        #endregion
                        break;
                    default:
                        goto Exception;
                }

                continue;

                Exception:
                var segment_Before = charIndex + sb.Length - 7 >= 0 ? input.Substring(charIndex + sb.Length - 7, 7) : input.Substring(0, charIndex + sb.Length);
                throw new TokenParseException($"无法识别的“{@char.Value}”，在索引位置“{charIndex + sb.Length}”：…{segment_Before}`{@char.Value}`");
            }

            //if (tokens.Any(_ => _.Id == TokenId.Assign || _.Id == TokenId.DivideAssign || _.Id == TokenId.MultiplyAssign || _.Id == TokenId.ModuloAssign || _.Id == TokenId.AddAssign || _.Id == TokenId.SubtractAssign || _.Id == TokenId.LeftShiftAssign || _.Id == TokenId.RightShiftAssign || _.Id == TokenId.AndAssign || _.Id == TokenId.ExclusiveOrAssign || _.Id == TokenId.OrAssign))
            //{
            //    var token = tokens.First(_ => _.Id == TokenId.Assign || _.Id == TokenId.DivideAssign || _.Id == TokenId.MultiplyAssign || _.Id == TokenId.ModuloAssign || _.Id == TokenId.AddAssign || _.Id == TokenId.SubtractAssign || _.Id == TokenId.LeftShiftAssign || _.Id == TokenId.RightShiftAssign || _.Id == TokenId.AndAssign || _.Id == TokenId.ExclusiveOrAssign || _.Id == TokenId.OrAssign);
            //    throw new ExpressionParseException($"赋值运算符“{token.Id.Description()}”暂不支持", input, token);
            //}

            Parse(tokens);

            return tokens;
        }

        static void Parse(IList<Token> tokens)
        {
            ParseConvert(tokens);
            ParseNew(tokens);
            ParseMethod(tokens);
        }

        static void ParseConvert(IList<Token> tokens)
        {
            bool predicate(Token _) => false
                || _.Id == TokenId.ParseString
                || _.Id == TokenId.ParseChar
                || _.Id == TokenId.ParseInt
                || _.Id == TokenId.ParseUInt
                || _.Id == TokenId.ParseLong
                || _.Id == TokenId.ParseULong
                || _.Id == TokenId.ParseDouble
                || _.Id == TokenId.ParseFloat
                || _.Id == TokenId.ParseDecimal
                || _.Id == TokenId.ParseBool
                || _.Id == TokenId.ParseGuid
                || _.Id == TokenId.ParseDateTime
                || _.Id == TokenId.ParseDateTimeOffset;
            while (tokens.Any(predicate))
            {
                var token = tokens.First(predicate);
                var index_LeftParenthesis = tokens.IndexOf(token) + 1;
                var index_RightParenthesis = -1;
                var parentheses = 0;
                for (int i = index_LeftParenthesis + 1; i < tokens.Count; i++)
                {
                    if (tokens[i].Id == TokenId.LeftParenthesis)
                        parentheses++;
                    else if (tokens[i].Id == TokenId.RightParenthesis)
                    {
                        if (parentheses > 0)
                            parentheses--;
                        else
                            index_RightParenthesis = i;
                    }
                }
                var _tokens = tokens.Where((_, i) => i > index_LeftParenthesis && i < index_RightParenthesis).ToList();
                for (int i = index_LeftParenthesis; i <= index_RightParenthesis; i++)
                {
                    tokens.RemoveAt(index_LeftParenthesis);
                }
                Parse(_tokens);
                token.Id = (TokenId)Enum.Parse(typeof(TokenId), token.Id.ToString().Substring(5));
                token.Value = _tokens;
            }
        }

        static void ParseNew(IList<Token> tokens)
        {
            while (tokens.Any(_ => _.Id == TokenId.NewArray || _.Id == TokenId.NewObject))
            {
                var token = tokens.First(_ => _.Id == TokenId.NewArray || _.Id == TokenId.NewObject);
                TokenId leftId = 0, rightId = 0;
                if (token.Id == TokenId.NewArray)
                {
                    leftId = TokenId.LeftBracket;
                    rightId = TokenId.RightBracket;
                }
                else if (token.Id == TokenId.NewObject)
                {
                    leftId = TokenId.LeftBrace;
                    rightId = TokenId.RightBrace;
                }
                var index_LeftBra = tokens.IndexOf(token) + 1;
                var index_RightBra = -1;
                var parentheses = 0;
                for (int i = index_LeftBra + 1; i < tokens.Count; i++)
                {
                    if (tokens[i].Id == leftId)
                        parentheses++;
                    else if (tokens[i].Id == rightId)
                    {
                        if (parentheses > 0)
                            parentheses--;
                        else
                            index_RightBra = i;
                    }
                }
                var _tokens = tokens.Where((_, i) => i > index_LeftBra && i < index_RightBra).ToList();
                for (int i = index_LeftBra; i <= index_RightBra; i++)
                {
                    tokens.RemoveAt(index_LeftBra);
                }
                Parse(_tokens);
                token.Id = (TokenId)Enum.Parse(typeof(TokenId), token.Id.ToString().Substring(3));
                token.Value = _tokens;
            }
        }

        static void ParseMethod(IList<Token> tokens)
        {
            while (tokens.Any(_ => _.Id == TokenId.MethodCall))
            {
                var token = tokens.First(_ => _.Id == TokenId.MethodCall);
                var index_LeftParenthesis = tokens.IndexOf(token) + 1;
                var index_RightParenthesis = -1;
                var parentheses = 0;
                for (int i = index_LeftParenthesis + 1; i < tokens.Count; i++)
                {
                    if (tokens[i].Id == TokenId.LeftParenthesis)
                        parentheses++;
                    else if (tokens[i].Id == TokenId.RightParenthesis)
                    {
                        if (parentheses > 0)
                            parentheses--;
                        else
                            index_RightParenthesis = i;
                    }
                }
                var _tokens = tokens.Where((_, i) => i > index_LeftParenthesis && i < index_RightParenthesis).ToList();
                for (int i = index_LeftParenthesis; i <= index_RightParenthesis; i++)
                {
                    tokens.RemoveAt(index_LeftParenthesis);
                }
                Parse(_tokens);
                token.Id = TokenId.Method;
                token.Value = new KeyValuePair<string, IList<Token>>(token.Value, _tokens);
            }
        }

        static void ThrowConvertException(string @string, string typeName, string input, int index)
        {
            var segment_Before = index - 7 >= 0 ? input.Substring(index - 7, 7) : input.Substring(0, index);
            throw new TokenParseException($"将“{@string}”转换为“{typeName}”类型出错，在索引位置“{index}”：…{segment_Before}`{@string}`");
        }
    }
}