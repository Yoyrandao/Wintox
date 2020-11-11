using System;
using System.Windows.Forms;

using Autofac;

using Microsoft.Extensions.Configuration;

using Wintox.Common;
using Wintox.Common.Hash;
using Wintox.Helpers;
using Wintox.Lib.LowLevelProcessing;
using Wintox.Lib.Models;

namespace Wintox
{
	public static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			var container = InitializeContainer();
			
			Application.Run(container.Resolve<TrayContext>());
		}

		private static IContainer InitializeContainer()
		{
			var builder = new ContainerBuilder();
			var config  = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.SetBasePath(Environment.CurrentDirectory)
				.Build();

			builder.Register(c => config).As<IConfiguration>();
			builder.RegisterType<ExcludingSettings>();
			
			builder.RegisterType<ToolStripItemConverter>().As<IConverter<OpenedWindow, ToolStripMenuItem>>();
			builder.RegisterType<Sha1HashProvider>().As<IHashProvider>();

			builder.RegisterType<LowLevelProcessor>().As<ILowLevelProcessor>();
			builder.RegisterType<TrayContext>();

			return builder.Build();
		}
	}
}