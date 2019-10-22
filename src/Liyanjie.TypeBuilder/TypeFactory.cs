using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

using Liyanjie.TypeBuilder.Internals;

namespace Liyanjie.TypeBuilder
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeFactory
    {
        // EmptyTypes is used to indicate that we are looking for someting without any parameters. 
        static readonly Type[] emptyTypes = new Type[0];

        static readonly ConcurrentDictionary<string, Type> generatedTypes = new ConcurrentDictionary<string, Type>();

        static readonly ModuleBuilder moduleBuilder;

        // Some objects we cache
        static readonly CustomAttributeBuilder compilerGeneratedAttributeBuilder = new CustomAttributeBuilder(typeof(CompilerGeneratedAttribute).GetTypeInfo().GetConstructor(emptyTypes), new object[0]);
        static readonly CustomAttributeBuilder debuggerBrowsableAttributeBuilder = new CustomAttributeBuilder(typeof(DebuggerBrowsableAttribute).GetTypeInfo().GetConstructor(new[] { typeof(DebuggerBrowsableState) }), new object[] { DebuggerBrowsableState.Never });
        static readonly CustomAttributeBuilder debuggerHiddenAttributeBuilder = new CustomAttributeBuilder(typeof(DebuggerHiddenAttribute).GetTypeInfo().GetConstructor(emptyTypes), new object[0]);

        static readonly ConstructorInfo objectCtor = typeof(object).GetTypeInfo().GetConstructor(emptyTypes);
        static readonly MethodInfo objectToString = typeof(object).GetTypeInfo().GetMethod("ToString", BindingFlags.Instance | BindingFlags.Public);

        static readonly ConstructorInfo stringBuilderCtor = typeof(StringBuilder).GetTypeInfo().GetConstructor(emptyTypes);
        static readonly MethodInfo stringBuilderAppend = typeof(StringBuilder).GetTypeInfo().GetMethod("Append", new[] { typeof(string) });

        static readonly Type equalityComparer = typeof(EqualityComparer<>);
        static readonly Type equalityComparerGenericArgument = equalityComparer.GetTypeInfo().GetGenericArguments()[0];

        static readonly MethodInfo equalityComparerDefault = equalityComparer.GetTypeInfo().GetMethod("get_Default", BindingFlags.Static | BindingFlags.Public);
        static readonly MethodInfo equalityComparerEquals = equalityComparer.GetTypeInfo().GetMethod("Equals", new[] { equalityComparerGenericArgument, equalityComparerGenericArgument });
        static readonly MethodInfo equalityComparerGetHashCode = equalityComparer.GetTypeInfo().GetMethod("GetHashCode", new[] { equalityComparerGenericArgument });

        static readonly Func<string, string> escape = _ => _.Replace(@"\", @"\\").Replace(@"|", @"\|");

        static int index = -1;

        static TypeFactory()
        {
            var assemblyName = new AssemblyName("Liyanjie.TypeFactory._, Version=0.0.0.0");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            moduleBuilder = assemblyBuilder.DefineDynamicModule("Liyanjie.TypeFactory._");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static Type CreateType(IDictionary<string, Type> properties)
        {
            Check.HasNoNulls(properties.Values, nameof(properties));

            var types = properties.Select(p => p.Value).ToArray();
            var names = properties.Select(p => p.Key).ToArray();

            // Anonymous classes are generics based. The generic classes are distinguished by number of parameters and name of parameters.
            // The specific types of the parameters are the generic arguments.
            // We recreate this by creating a fullName composed of all the property names, separated by a "|".
            var fullName = string.Join("|", names.Select(escape).ToArray());


            if (!generatedTypes.TryGetValue(fullName, out var type))
            {
                // We create only a single class at a time, through this lock
                // Note that this is a variant of the double-checked locking.
                // It is safe because we are using a thread safe class.
                lock (generatedTypes)
                {
                    var index = Interlocked.Increment(ref TypeFactory.index);

                    var name = names.Length != 0 ? $"<>f__AnonymousType{index}`{names.Length}" : $"<>f__AnonymousType{index}";

                    var typeBuilder = moduleBuilder.DefineType(name, TypeAttributes.AnsiClass | TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoLayout | TypeAttributes.BeforeFieldInit, typeof(TypeBase));
                    typeBuilder.SetCustomAttribute(compilerGeneratedAttributeBuilder);

                    GenericTypeParameterBuilder[] generics;

                    if (names.Length != 0)
                    {
                        var genericNames = names.Select(genericName => $"<{genericName}>j__TPar").ToArray();
                        generics = typeBuilder.DefineGenericParameters(genericNames);
                        foreach (var b in generics)
                        {
                            b.SetCustomAttribute(compilerGeneratedAttributeBuilder);
                        }
                    }
                    else
                    {
                        generics = new GenericTypeParameterBuilder[0];
                    }

                    var fields = new FieldBuilder[names.Length];

                    // There are two for cycles because we want to have all the getter methods before all the other methods
                    for (int i = 0; i < names.Length; i++)
                    {
                        // field
                        fields[i] = typeBuilder.DefineField($"<{names[i]}>i__Field", generics[i].AsType(), FieldAttributes.Private | FieldAttributes.InitOnly);
                        fields[i].SetCustomAttribute(debuggerBrowsableAttributeBuilder);

                        var property = typeBuilder.DefineProperty(names[i], PropertyAttributes.None, CallingConventions.HasThis, generics[i].AsType(), emptyTypes);

                        // getter
                        var getter = typeBuilder.DefineMethod($"get_{names[i]}", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName, CallingConventions.HasThis, generics[i].AsType(), null);
                        getter.SetCustomAttribute(compilerGeneratedAttributeBuilder);
                        var ilGeneratorGetter = getter.GetILGenerator();
                        ilGeneratorGetter.Emit(OpCodes.Ldarg_0);
                        ilGeneratorGetter.Emit(OpCodes.Ldfld, fields[i]);
                        ilGeneratorGetter.Emit(OpCodes.Ret);
                        property.SetGetMethod(getter);

                        // setter
                        var setter = typeBuilder.DefineMethod($"set_{names[i]}", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName, CallingConventions.HasThis, null, new[] { generics[i].AsType() });
                        setter.SetCustomAttribute(compilerGeneratedAttributeBuilder);

                        // workaround for https://github.com/dotnet/corefx/issues/7792
                        setter.DefineParameter(1, ParameterAttributes.In, generics[i].Name);

                        var ilGeneratorSetter = setter.GetILGenerator();
                        ilGeneratorSetter.Emit(OpCodes.Ldarg_0);
                        ilGeneratorSetter.Emit(OpCodes.Ldarg_1);
                        ilGeneratorSetter.Emit(OpCodes.Stfld, fields[i]);
                        ilGeneratorSetter.Emit(OpCodes.Ret);
                        property.SetSetMethod(setter);
                    }

                    // ToString()
                    var toString = typeBuilder.DefineMethod("ToString", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, CallingConventions.HasThis, typeof(string), emptyTypes);
                    toString.SetCustomAttribute(debuggerHiddenAttributeBuilder);
                    var ilGeneratorToString = toString.GetILGenerator();
                    ilGeneratorToString.DeclareLocal(typeof(StringBuilder));
                    ilGeneratorToString.Emit(OpCodes.Newobj, stringBuilderCtor);
                    ilGeneratorToString.Emit(OpCodes.Stloc_0);

                    // Equals
                    var equals = typeBuilder.DefineMethod("Equals", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, CallingConventions.HasThis, typeof(bool), new[] { typeof(object) });
                    equals.SetCustomAttribute(debuggerHiddenAttributeBuilder);
                    equals.DefineParameter(1, ParameterAttributes.In, "value");

                    var ilGeneratorEquals = equals.GetILGenerator();
                    ilGeneratorEquals.DeclareLocal(typeBuilder.AsType());
                    ilGeneratorEquals.Emit(OpCodes.Ldarg_1);
                    ilGeneratorEquals.Emit(OpCodes.Isinst, typeBuilder.AsType());
                    ilGeneratorEquals.Emit(OpCodes.Stloc_0);
                    ilGeneratorEquals.Emit(OpCodes.Ldloc_0);

                    var equalsLabel = ilGeneratorEquals.DefineLabel();

                    // GetHashCode()
                    var getHashCode = typeBuilder.DefineMethod("GetHashCode", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, CallingConventions.HasThis, typeof(int), emptyTypes);
                    getHashCode.SetCustomAttribute(debuggerHiddenAttributeBuilder);
                    var ilGeneratorGetHashCode = getHashCode.GetILGenerator();
                    ilGeneratorGetHashCode.DeclareLocal(typeof(int));

                    if (names.Length == 0)
                    {
                        ilGeneratorGetHashCode.Emit(OpCodes.Ldc_I4_0);
                    }
                    else
                    {
                        // As done by Roslyn
                        // Note that initHash can vary, because string.GetHashCode() isn't "stable" for different compilation of the code
                        int initHash = 0;

                        for (int i = 0; i < names.Length; i++)
                        {
                            initHash = unchecked(initHash * (-1521134295) + fields[i].Name.GetHashCode());
                        }

                        // Note that the CSC seems to generate a different seed for every anonymous class
                        ilGeneratorGetHashCode.Emit(OpCodes.Ldc_I4, initHash);
                    }

                    for (int i = 0; i < names.Length; i++)
                    {
                        var equalityComparerT = equalityComparer.MakeGenericType(generics[i].AsType());

                        // Equals()
                        var equalityComparerTDefault = System.Reflection.Emit.TypeBuilder.GetMethod(equalityComparerT, equalityComparerDefault);
                        var equalityComparerTEquals = System.Reflection.Emit.TypeBuilder.GetMethod(equalityComparerT, equalityComparerEquals);

                        // Illegal one-byte branch at position: 9. Requested branch was: 143.
                        // So replace OpCodes.Brfalse_S to OpCodes.Brfalse
                        ilGeneratorEquals.Emit(OpCodes.Brfalse, equalsLabel);
                        ilGeneratorEquals.Emit(OpCodes.Call, equalityComparerTDefault);
                        ilGeneratorEquals.Emit(OpCodes.Ldarg_0);
                        ilGeneratorEquals.Emit(OpCodes.Ldfld, fields[i]);
                        ilGeneratorEquals.Emit(OpCodes.Ldloc_0);
                        ilGeneratorEquals.Emit(OpCodes.Ldfld, fields[i]);
                        ilGeneratorEquals.Emit(OpCodes.Callvirt, equalityComparerTEquals);

                        // GetHashCode();
                        var equalityComparerTGetHashCode = System.Reflection.Emit.TypeBuilder.GetMethod(equalityComparerT, equalityComparerGetHashCode);
                        ilGeneratorGetHashCode.Emit(OpCodes.Stloc_0);
                        ilGeneratorGetHashCode.Emit(OpCodes.Ldc_I4, -1521134295);
                        ilGeneratorGetHashCode.Emit(OpCodes.Ldloc_0);
                        ilGeneratorGetHashCode.Emit(OpCodes.Mul);
                        ilGeneratorGetHashCode.Emit(OpCodes.Call, equalityComparerTDefault);
                        ilGeneratorGetHashCode.Emit(OpCodes.Ldarg_0);
                        ilGeneratorGetHashCode.Emit(OpCodes.Ldfld, fields[i]);
                        ilGeneratorGetHashCode.Emit(OpCodes.Callvirt, equalityComparerTGetHashCode);
                        ilGeneratorGetHashCode.Emit(OpCodes.Add);

                        // ToString();
                        ilGeneratorToString.Emit(OpCodes.Ldloc_0);
                        ilGeneratorToString.Emit(OpCodes.Ldstr, i == 0 ? $"{{ {names[i]} = " : $", {names[i]} = ");
                        ilGeneratorToString.Emit(OpCodes.Callvirt, stringBuilderAppend);
                        ilGeneratorToString.Emit(OpCodes.Pop);
                        ilGeneratorToString.Emit(OpCodes.Ldloc_0);
                        ilGeneratorToString.Emit(OpCodes.Ldarg_0);
                        ilGeneratorToString.Emit(OpCodes.Ldfld, fields[i]);
                        ilGeneratorToString.Emit(OpCodes.Box, generics[i].AsType());
                        ilGeneratorToString.Emit(OpCodes.Callvirt, stringBuilderAppend);
                        ilGeneratorToString.Emit(OpCodes.Pop);
                    }

                    // .ctor default
                    var constructorDef = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.HasThis, emptyTypes);
                    constructorDef.SetCustomAttribute(debuggerHiddenAttributeBuilder);

                    var ilGeneratorConstructorDef = constructorDef.GetILGenerator();
                    ilGeneratorConstructorDef.Emit(OpCodes.Ldarg_0);
                    ilGeneratorConstructorDef.Emit(OpCodes.Call, objectCtor);
                    ilGeneratorConstructorDef.Emit(OpCodes.Ret);

                    // .ctor with params
                    var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.HasThis, generics.Select(p => p.AsType()).ToArray());
                    constructor.SetCustomAttribute(debuggerHiddenAttributeBuilder);

                    var ilGeneratorConstructor = constructor.GetILGenerator();
                    ilGeneratorConstructor.Emit(OpCodes.Ldarg_0);
                    ilGeneratorConstructor.Emit(OpCodes.Call, objectCtor);

                    for (int i = 0; i < names.Length; i++)
                    {
                        constructor.DefineParameter(i + 1, ParameterAttributes.None, names[i]);
                        ilGeneratorConstructor.Emit(OpCodes.Ldarg_0);

                        if (i == 0)
                        {
                            ilGeneratorConstructor.Emit(OpCodes.Ldarg_1);
                        }
                        else if (i == 1)
                        {
                            ilGeneratorConstructor.Emit(OpCodes.Ldarg_2);
                        }
                        else if (i == 2)
                        {
                            ilGeneratorConstructor.Emit(OpCodes.Ldarg_3);
                        }
                        else if (i < 255)
                        {
                            ilGeneratorConstructor.Emit(OpCodes.Ldarg_S, (byte)(i + 1));
                        }
                        else
                        {
                            // Ldarg uses a ushort, but the Emit only accepts short, so we use a unchecked(...), cast to short and let the CLR interpret it as ushort.
                            ilGeneratorConstructor.Emit(OpCodes.Ldarg, unchecked((short)(i + 1)));
                        }

                        ilGeneratorConstructor.Emit(OpCodes.Stfld, fields[i]);
                    }

                    ilGeneratorConstructor.Emit(OpCodes.Ret);

                    // Equals()
                    if (names.Length == 0)
                    {
                        ilGeneratorEquals.Emit(OpCodes.Ldnull);
                        ilGeneratorEquals.Emit(OpCodes.Ceq);
                        ilGeneratorEquals.Emit(OpCodes.Ldc_I4_0);
                        ilGeneratorEquals.Emit(OpCodes.Ceq);
                    }
                    else
                    {
                        ilGeneratorEquals.Emit(OpCodes.Ret);
                        ilGeneratorEquals.MarkLabel(equalsLabel);
                        ilGeneratorEquals.Emit(OpCodes.Ldc_I4_0);
                    }

                    ilGeneratorEquals.Emit(OpCodes.Ret);

                    // GetHashCode()
                    ilGeneratorGetHashCode.Emit(OpCodes.Stloc_0);
                    ilGeneratorGetHashCode.Emit(OpCodes.Ldloc_0);
                    ilGeneratorGetHashCode.Emit(OpCodes.Ret);

                    // ToString()
                    ilGeneratorToString.Emit(OpCodes.Ldloc_0);
                    ilGeneratorToString.Emit(OpCodes.Ldstr, names.Length == 0 ? "{ }" : " }");
                    ilGeneratorToString.Emit(OpCodes.Callvirt, stringBuilderAppend);
                    ilGeneratorToString.Emit(OpCodes.Pop);
                    ilGeneratorToString.Emit(OpCodes.Ldloc_0);
                    ilGeneratorToString.Emit(OpCodes.Callvirt, objectToString);
                    ilGeneratorToString.Emit(OpCodes.Ret);

                    type = typeBuilder.CreateTypeInfo().AsType();

                    type = generatedTypes.GetOrAdd(fullName + "|_1", type);
                }
            }

            if (types.Length != 0)
            {
                type = type.MakeGenericType(types);
            }

            return type;
        }
    }
}
