using CheckVersionService.Config.Readers;
using CheckVersionService.Config.Validators;
using CheckVersionService.Interfaces;
using CheckVersionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace CheckVersionService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logger.Logger.Log.Info("Startup");

            IConfig config = new DefaultNetReader(Logger.Logger.Log);
            ConfigValidator validator = new ConfigValidator();

            if (!validator.IsValid(config))
            {
                Logger.Logger.Log.Error("Config not valid");
                throw new FormatException("Config not valid");
            }

            IEnumerable<FolderConfig> folders = config.GetFolders();

            if (Environment.UserInteractive) // Проверка в каком состоянии запущено приложение
            {
                // Запуещно как консольное приложение
                CheckService service1 = new CheckService(folders.ToArray());
                service1.TestStartupAndStop(args);
            }
            else
            {
                // Запуещно как сервис
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new CheckService(folders.ToArray())
                };
                ServiceBase.Run(ServicesToRun);
            }

            Logger.Logger.Log.Info("Close app");
        }
    }
}
