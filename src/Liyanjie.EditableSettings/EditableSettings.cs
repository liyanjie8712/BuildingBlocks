using System;
using System.Collections.Concurrent;
using System.IO;
#if NETSTANDARD2_0
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
#endif

namespace Liyanjie.EditableSettings
{
    /// <summary>
    /// netstandard2.0 支持监控文件变化，net45 不支持
    /// </summary>
    public class EditableSettings
    {
        readonly ConcurrentDictionary<Type, (string FileName, object Instance)> store = new ConcurrentDictionary<Type, (string, object)>();

        readonly string rootPath;
        readonly Func<object, string> serialize;
        readonly Func<string, Type, object> deserialize;
#if NETSTANDARD2_0
        readonly IFileProvider fileProvider;
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="serialize"></param>
        /// <param name="deserialize"></param>
        public EditableSettings(
            string rootPath,
            Func<object, string> serialize,
            Func<string, Type, object> deserialize)
        {
            this.rootPath = rootPath;
            this.serialize = serialize;
            this.deserialize = deserialize;
#if NETSTANDARD2_0
            this.fileProvider = new PhysicalFileProvider(rootPath);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSettings"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public EditableSettings Map<TSettings>(string fileName)
        {
            var type = typeof(TSettings);
            store[type] = (fileName, Get(fileName, type));

#if NETSTANDARD2_0
            ChangeToken.OnChange(() => fileProvider.Watch(Path.GetFileName(fileName)), () => Get<TSettings>());
#endif

            return this;
        }

        /// <summary>
        /// 获取 Settings
        /// </summary>
        /// <typeparam name="TSettings"></typeparam>
        /// <returns></returns>
        public TSettings Get<TSettings>()
        {
            var type = typeof(TSettings);
            if (store.TryGetValue(type, out var settings))
            {
                if (settings.Instance == null)
                {
                    settings.Instance = Get(settings.FileName, typeof(TSettings));
                    store[type] = settings;
                }
                return (TSettings)settings.Instance;
            }

            return default;
        }

        /// <summary>
        /// 保存 Settings
        /// </summary>
        /// <typeparam name="TSettings"></typeparam>
        /// <param name="settings"></param>
        public void Set<TSettings>(TSettings settings)
        {
            var type = typeof(TSettings);
            if (store.ContainsKey(type))
            {
                store[type] = (store[type].FileName, settings);
                Set(store[type].FileName, settings);
            }
        }

        object Get(string fileName, Type type)
        {
            return deserialize(File.ReadAllText(Path.Combine(rootPath, fileName)), type);
        }
        void Set(string fileName, object obj)
        {
            File.WriteAllText(Path.Combine(rootPath, fileName), serialize(obj));
        }
    }
}
