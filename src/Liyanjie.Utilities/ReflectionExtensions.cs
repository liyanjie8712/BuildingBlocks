using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TOutput Translate<TOutput>(this object input)
        {
            var translated = new Dictionary<(Type, Type, object), object>();

            var output = (TOutput)Translate(typeof(TOutput),  input, translated);

            translated.Clear();

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="input"></param>
        /// <param name="additional"></param>
        /// <returns></returns>
        public static TOutput Translate<TInput, TOutput>(this TInput input, Action<TInput, TOutput> additional)
        {
            var output = Translate<TOutput>(input);

            additional?.Invoke(input, output);

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="input"></param>
        /// <param name="additional"></param>
        /// <returns></returns>
        public static async Task<TOutput> TranslateAsync<TInput, TOutput>(this TInput input, Func<TInput, TOutput, Task> additional)
        {
            var output = Translate<TOutput>(input);

            if (additional != null)
                await additional.Invoke(input, output);

            return output;
        }

        static object Translate(Type outputType,  object input, Dictionary<(Type, Type, object), object> translated)
        {
            if (input == null)
                return null;

            var inputType = input.GetType();

            if (translated.TryGetValue((outputType, inputType, input), out object output))
                return output;

            if (outputType.IsInstanceOfType(input))
                return input;

            if (outputType.IsAssignableFrom(inputType))
                return input;

            var inputTypeInfo = inputType.GetTypeInfo();

            if (inputTypeInfo.IsEnum && (inputType == typeof(short) || inputType == typeof(ushort) || inputType == typeof(int) || inputType == typeof(uint) || inputType == typeof(long) || inputType == typeof(ulong)))
                return Convert.ChangeType(Enum.Format(inputType, input, "D"), outputType);

            var outputTypeInfo = outputType.GetTypeInfo();

            if (outputTypeInfo.IsEnum && (inputType == typeof(short) || inputType == typeof(ushort) || inputType == typeof(int) || inputType == typeof(uint) || inputType == typeof(long) || inputType == typeof(ulong)))
                return Enum.ToObject(outputType, input);

            var typeofIEnumerable = typeof(IEnumerable);
            if (typeofIEnumerable.IsAssignableFrom(outputType) && typeofIEnumerable.IsAssignableFrom(inputType))
            {
                var outputElementType = outputType.HasElementType
                    ? outputType.GetElementType()
                    : outputType.IsConstructedGenericType
                        ? outputType.GenericTypeArguments[0]
                        : null;
                var inputArray = Enumerable.ToArray((input as IEnumerable).Cast<object>());
                var outputArray = Array.CreateInstance(outputElementType ?? typeof(object), inputArray.Length);
                inputArray.Select(_ => outputElementType == null ? _ : Translate(outputElementType,  _, translated)).ToArray().CopyTo(outputArray, 0);
                return outputArray;
            }

            if (outputType == typeof(string))
                return input?.ToString();

            if (outputTypeInfo.IsValueType)
            {
                if ("Nullable`1" == outputType.Name)
                    try
                    {
                        return Convert.ChangeType(input, outputType.GenericTypeArguments[0]);
                    }
                    catch
                    {
                        return null;
                    }

                try
                {
                    return Convert.ChangeType(input, outputType);
                }
                catch { }
            }

            if (outputTypeInfo.IsInterface)
                return null;

            if (outputTypeInfo.IsAbstract)
                return null;

            output = Activator.CreateInstance(outputType);
            translated.Add((outputType, inputType, input), output);

            var inputProperties = inputType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var outputProperties = outputType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var outputProperty in outputProperties)
            {
                if (!outputProperty.CanWrite)
                    continue;

                var inputProperty = inputProperties.FirstOrDefault(_ => _.Name == outputProperty.Name);
                if (inputProperty == null || !inputProperty.CanRead)
                    continue;

                outputProperty.SetValue(output, Translate(outputProperty.PropertyType, inputProperty.GetValue(input), translated));
            }

            return output;
        }
    }
}