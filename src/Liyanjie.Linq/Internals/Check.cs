using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Liyanjie.Linq.Internals
{
    internal static class Check
    {
        public static T Condition<T>(T value, Predicate<T> condition, string parameterName)
        {
            NotNull(condition, nameof(condition));
            NotNull(value, nameof(value));

            if (!condition(value))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentOutOfRangeException(parameterName);
            }

            return value;
        }

        public static T NotNull<T>(T value, string parameterName)
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static T NotNull<T>(T value, string parameterName, string propertyName)
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, nameof(parameterName));
                NotEmpty(propertyName, nameof(propertyName));

                throw new ArgumentException(argumentPropertyNull(propertyName, parameterName));
            }

            return value;
        }

        public static IList<T> NotEmpty<T>(IList<T> value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Count == 0)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(collectionArgumentIsEmpty(parameterName));
            }

            return value;
        }

        public static string NotEmpty(string value, string parameterName)
        {
            Exception e = null;
            if (ReferenceEquals(value, null))
            {
                e = new ArgumentNullException(parameterName);
            }
            else if (value.Trim().Length == 0)
            {
                e = new ArgumentException(argumentIsEmpty(parameterName));
            }

            if (e != null)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw e;
            }

            return value;
        }

        public static string NullButNotEmpty(string value, string parameterName)
        {
            if (!ReferenceEquals(value, null) && (value.Length == 0))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(argumentIsEmpty(parameterName));
            }

            return value;
        }

        public static IList<T> HasNoNulls<T>(IList<T> value, string parameterName) where T : class
        {
            NotNull(value, parameterName);

            if (value.Any(_ => _ == null))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(parameterName);
            }

            return value;
        }

        public static Type ValidEntityType(Type type, string parameterName)
        {
            if (!type.GetTypeInfo().IsClass)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(invalidEntityType(type, parameterName));
            }

            return type;
        }

        /// <summary>
        /// The property '{property}' of the argument '{argument}' cannot be null.
        /// </summary>
        private static string argumentPropertyNull(string property, string argument)
        {
            return string.Format(CultureInfo.CurrentCulture, $"The property '{property}' of the argument '{argument}' cannot be null.", property, argument);
        }

        /// <summary>
        /// The string argument '{argumentName}' cannot be empty.
        /// </summary>
        private static string argumentIsEmpty(string argumentName)
        {
            return string.Format(CultureInfo.CurrentCulture, $"The string argument '{argumentName}' cannot be empty.", argumentName);
        }

        /// <summary>
        /// The entity type '{type}' provided for the argument '{argumentName}' must be a reference type.
        /// </summary>
        private static string invalidEntityType(Type type, string argumentName)
        {
            return string.Format(CultureInfo.CurrentCulture, $"The entity type '{type}' provided for the argument '{argumentName}' must be a reference type.", type, argumentName);
        }

        /// <summary>
        /// The collection argument '{argumentName}' must contain at least one element.
        /// </summary>
        private static string collectionArgumentIsEmpty(string argumentName)
        {
            return string.Format(CultureInfo.CurrentCulture, $"The collection argument '{argumentName}' must contain at least one element.", argumentName);
        }
    }
}