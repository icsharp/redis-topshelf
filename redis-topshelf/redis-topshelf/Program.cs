using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Topshelf;
using Topshelf.Logging;

namespace redis_topshelf
{
	class Program
	{
		static string _ServiceName = ConfigurationManager.AppSettings[Constants.REDIS_SERVICE_NAME];
		static string _ServiceDisplayName = ConfigurationManager.AppSettings[Constants.REDIS_SERVICE_DISPALYNAME];
		static string _ServiceDescription = ConfigurationManager.AppSettings[Constants.REDIS_SERVICE_DESCRIPTION];
		static int Main(string[] args)
		{
			return (int)HostFactory.Run(x =>
			   {
				   x.UseLog4Net("log4net.config");

				   x.RunAsLocalSystem();
				   x.SetServiceName(_ServiceName);
				   x.SetDisplayName(_ServiceDisplayName);
				   x.SetDescription(_ServiceDescription);

				   x.Service<RedisService>();

				   x.EnableServiceRecovery(r => { r.RestartService(1); });
			   });
		}
	}
}
