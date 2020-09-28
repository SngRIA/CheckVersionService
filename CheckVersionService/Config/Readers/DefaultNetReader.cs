using CheckVersionService.Config.Elements;
using CheckVersionService.Interfaces;
using CheckVersionService.Models;
using log4net;
using System;
using System.Collections.Generic;

namespace CheckVersionService.Config.Readers
{
    public class DefaultNetReader : IConfig
    {
        private CheckVersionConfig _versionConfig;
        private ILog _log;
        public DefaultNetReader(ILog log)
        {
            _log = log;
            _versionConfig = CheckVersionConfig.GetConfig();
        }

        // Возвращает папки с периодом поверки
        public IEnumerable<FolderConfig> GetFolders()
        {
            foreach (FolderElement folder in _versionConfig.Folders)
            {
                if (TimeSpan.TryParse(folder.Every, out TimeSpan time))
                {
                    yield return new FolderConfig(folder.Path, time);
                }
                else
                {
                    _log.Warn("Config value [Folders.Folder.Every] has wrong format: " + folder.Every);
                }
            }
        }
    }
}
