using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using Topshelf;
using Topshelf.Logging;

namespace redis_topshelf
{
	public class RedisService : ServiceControl
	{
		private readonly LogWriter _log = HostLogger.Get<RedisService>();
		private string _redisPath = ConfigurationManager.AppSettings[Constants.REDIS_PAHT];
		private string _redisServer, _redisConf, _redisCli;
		private int _port;
		private Process _redisServerConsole;
		bool VerifyArgumnets()
		{
			if (!Directory.Exists(_redisPath))
			{
				_log.ErrorFormat("Redis directory not found->{0}", _redisPath);
				return false;
			}
			_redisServer = Path.Combine(_redisPath, ConfigurationManager.AppSettings[Constants.REDIS_SERVER]);
			_redisConf = Path.Combine(_redisPath, ConfigurationManager.AppSettings[Constants.REDIS_CONF]);
			_redisCli = Path.Combine(_redisPath, ConfigurationManager.AppSettings[Constants.REDIS_CLI]);

			_port = FindPort(_redisConf);

			return true;
		}
		public bool Start(HostControl hostControl)
		{
			if (!VerifyArgumnets()) return false;

			_log.Info("RedisService Starting...");

			try
			{
				ProcessStartInfo cfg = new ProcessStartInfo(_redisServer);
				cfg.UseShellExecute = false;
				cfg.Arguments = Path.GetFileName(_redisConf);
				cfg.WorkingDirectory = _redisPath;
				using (_redisServerConsole = new Process { StartInfo = cfg })
				{
					_redisServerConsole.Start();
				}
			}
			catch (Exception ex)
			{
				_log.ErrorFormat("Starting redis console server occured exception:{0}", ex);
				return false;
			}

			_log.Info("RedisService Started");
			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			if (_redisServerConsole != null)
			{
				try
				{
					//using reids command "shutdown" to close the server.
					ProcessStartInfo cfg = new ProcessStartInfo(_redisCli);
					cfg.UseShellExecute = false;
					cfg.Arguments = string.Format(" -p {0} shutdown", _port);
					cfg.WorkingDirectory = _redisPath;
					using (var redisClient = new Process { StartInfo = cfg })
					{
						redisClient.Start();
					}
				}
				catch (Exception ex)
				{
					_log.ErrorFormat("Using command -shutdown to close server occured exception:{0}", ex);
					return false;
				}

			}

			Thread.Sleep(500);

			_log.Info("RedisService Stopped");
			return true;
		}

		private int FindPort(string conf)
		{
			using (var reader = new StreamReader(conf))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
					if (line.IndexOf("port") == 0)
						return int.Parse(line.Substring(5, line.Length - 5));
			}
			return 6379;
		}
	}
}
