using CheckVersionService.Enums;
using CheckVersionService.Interfaces;
using CheckVersionService.Models;
using CheckVersionService.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileInfo = CheckVersionService.Models.FileInfo;

namespace CheckVersionService
{
    public sealed class FolderHistory
    {
        private readonly ILog _log;
        private readonly IAction _action;
        private readonly FileServiceFolder _fileService;

        private FolderInfo _actualFolderInfo;
        private FolderInfo _cachedFolderInfo;

        private string _folder;
        private Timer _timer;

        public FolderHistory(ILog log, IAction action, FileServiceFolder fileService)
        {
            _log = log;
            _action = action;
            _fileService = fileService;
        }


        /// <summary>
        /// Инициализирует отслеживание директории
        /// </summary>
        /// <param name="folder">Директория  для отслеживания</param>
        /// <param name="period">Период отслеживания</param>
        public async void InitTrace(string folder, TimeSpan period)
        {
            _log.Info("Initialization trace");

            _folder = folder;
            _actualFolderInfo = await _fileService.Read.GetActualHistory(_folder);

            try
            {
                _cachedFolderInfo = await _fileService.Read.GetCachedFilesHistory(folder);
            } 
            catch (AggregateException)
            {
                _log.Error("Error: can't read file from cache");
            }
            finally
            {
                if (_cachedFolderInfo?.Files.Count > 0 || _cachedFolderInfo == null)
                    _cachedFolderInfo = _actualFolderInfo;
            }

            _timer = new Timer(async (state) => await StartTraceAsync(), folder, 0, (int)period.TotalMilliseconds);
        }

        private FileInfo GetFileEqualsFileCache(FileInfo file, ICollection<FileInfo> collection)
        {
            FileInfo info = new FileInfo(file.Path);

            foreach (var fileInfo in collection)
            {
                if(fileInfo.Path == file.Path)
                {
                    info = fileInfo;
                }
            }

            return info;
        }

        /// <summary>
        /// Запуск отслеживания
        /// </summary>
        /// <returns></returns>
        public async Task StartTraceAsync()
        {
            _log.Info($"Start trace: {_folder}");

            _actualFolderInfo = await _fileService.Read.GetActualHistory(_folder);

            foreach (var cachedFile in _cachedFolderInfo.Files)
            {
                FileInfo actualFile = GetFileEqualsFileCache(cachedFile, _actualFolderInfo.Files); // Получаем из актуальной коллекции эквивалент кэшированного файла 

                if(!File.Exists(actualFile.Path)) // Проверяет существует ли файл
                {
                    actualFile.ChangeStatus = FileChangeStatus.Change;
                    _log.Info($"File [{actualFile.Path}] deleted");

                    continue;
                }

                if (_fileService.Check.IsChanged(actualFile, cachedFile))
                {
                    actualFile.ChangeStatus = FileChangeStatus.Change;
                    _log.Info($"File [{actualFile.Path}] changed");

                    _action.Execute(actualFile, _folder);
                    _log.Debug($"Execute action");

                    await _fileService.Save.Save(actualFile);
                }
                else
                {
                    actualFile.ChangeStatus = FileChangeStatus.Nothing;
                }
            }

            _cachedFolderInfo = _actualFolderInfo;
        }

        /// <summary>
        /// Остановка отслеживания, сохраняет актуальное состояние в кэш
        /// </summary>
        /// <returns></returns>
        public void StopTrace()
        {
            foreach (var folder in _actualFolderInfo.Files)
            {
                _fileService.Save.Save(folder);
            }
        }
    }
}
