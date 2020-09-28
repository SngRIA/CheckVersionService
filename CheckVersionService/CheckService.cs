using CheckVersionService.Actions;
using CheckVersionService.Checks;
using CheckVersionService.Interfaces;
using CheckVersionService.Models;
using CheckVersionService.Readers;
using CheckVersionService.Saves;
using CheckVersionService.Services;
using log4net;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace CheckVersionService
{
    public partial class CheckService : ServiceBase
    {
        private readonly Container _container;
        private readonly ICollection<FolderConfig> _configFolders;
        private readonly ICollection<FolderHistory> _folderHistories;
        private readonly ILog _log;
        public CheckService(ICollection<FolderConfig> configFolders)
        {
            InitializeComponent();

            ServiceName = "CheckService";
            CanStop = true;

            _folderHistories = new List<FolderHistory>();

            _configFolders = configFolders;

            _container = new Container();

            _container.Register(() => Logger.Logger.Log);
            _container.Register<ISaveHistory, FileSaveHistory>();
            _container.Register<ICheckHistory, MD5CheckHistory>();
            _container.Register<IReadHistory, FileReadHistory>();
            _container.Register<FileServiceFolder>();
            _container.Register<IAction, MoveFileAction>();

            _container.Verify();

            _log = _container.GetInstance<ILog>();
        }
        protected override void OnStart(string[] args)
        {
            FileServiceFolder fileService = _container.GetInstance<FileServiceFolder>();
            IAction action = _container.GetInstance<IAction>();

            _log.Info("OnStart");

            foreach (var folder in _configFolders)
            {
                _log.Info($"time: {folder.Period}, tick: {folder.Period.TotalSeconds}");

                FolderHistory folderHistory = new FolderHistory(_log, action, fileService);
                folderHistory.InitTrace(folder.Path, folder.Period);

                _folderHistories.Add(folderHistory);
            }
        }

        protected override void OnStop()
        {
            foreach (var folderHistory in _folderHistories)
            {
                folderHistory.StopTrace();
            }

            _log.Info("Stop");
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadKey();
            this.OnStop();
        }
    }
}
