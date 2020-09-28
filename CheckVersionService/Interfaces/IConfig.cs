using CheckVersionService.Models;
using System;
using System.Collections.Generic;

namespace CheckVersionService.Interfaces
{
    public interface IConfig
    {
        IEnumerable<FolderConfig> GetFolders();
    }
}
