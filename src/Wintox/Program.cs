using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autofac;

using Microsoft.Extensions.Configuration;

using NonInvasiveKeyboardHookLibrary;

using Serilog;

using Wintox.Common.Hash;
using Wintox.Common.Settings;
using Wintox.Helpers;
using Wintox.Helpers.Converters;
using Wintox.Lib.LowLevelProcessing;
using Wintox.Lib.Models;

namespace Wintox
{
	public static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.Run(InitializeContainer().Resolve<TrayContext>());
		}

		private static IContainer InitializeContainer()
		{
			var builder = new ContainerBuilder();

			var config = Environment.GetEnvironmentVariable("IS_DEVELOP") == null
				             ? "appsettings.json"
				             : "appsettings.Development.json";

			_configuration = new ConfigurationBuilder()
			                 .AddJsonFile(config)
			                 .SetBasePath(Environment.CurrentDirectory)
			                 .Build();

			builder.Register(c => _configuration).As<IConfiguration>();
			builder.RegisterType<ExcludingSettings>();

			builder.Register(_ => new ShortcutManager(new KeyboardHookManager())).As<IShortcutManager>();
			builder.RegisterType<ToolStripItemConverter>().As<IConverter<OpenedWindow, ToolStripMenuItem>>();
			builder.RegisterType<Sha1HashProvider>().As<IHashProvider>();
			builder.RegisterType<ExceptionShield>().As<IExceptionShield>();

			builder.RegisterType<LowLevelProcessor>().As<ILowLevelProcessor>();
			builder.RegisterType<TrayContext>();

			builder.RegisterInstance(new OptionsContext { Visible = false });
			
			InitializeLogger();

			return builder.Build();
		}

		private static void InitializeLogger()
		{
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(_configuration, "Serilog")
				.CreateLogger();
		}

		private static IConfiguration _configuration;
	}
}