using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Liyanjie.DesktopWebHost
{
    class WebHostManager
    {
        static readonly string[] startup = ConfigurationManager.AppSettings["Startup"].Split(',', StringSplitOptions.RemoveEmptyEntries);
        static readonly string[] urls = ConfigurationManager.AppSettings["Urls"].Split(',', StringSplitOptions.RemoveEmptyEntries);
        static IHost webhost;

        internal static void StartWebHost()
        {
            foreach (var item in Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", new EnumerationOptions
            {
                RecurseSubdirectories = true,
            }))
            {
                try
                {
                    Assembly.LoadFrom(item);
                }
                catch { }
            }
            try
            {
                webhost = Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup(Assembly.LoadFrom(startup[0]).GetType(startup[1]));
                        webBuilder.UseUrls(urls);
                        webBuilder.ConfigureLogging(logging => logging.AddProvider(new Logging.MyLoggerProvider()));
                    })
                    .Build();
                webhost.RunAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal static void CloseWebHost()
        {
            if (webhost is not null)
            {
                webhost.StopAsync().ConfigureAwait(false);
                webhost.Dispose();
            }
        }

        internal static void OpenInBrowser()
        {
            var url = urls.FirstOrDefault() ?? "http://localhost:5000";
            if (url.IndexOf('*') > 0)
                url = url.Replace("*", "localhost");

            System.Diagnostics.Process.Start("explorer", url);
        }
    }
}
