using CheckVersionService.Enums;
using CheckVersionService.Interfaces;
using CheckVersionService.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileInfo = CheckVersionService.Models.FileInfo;

namespace CheckVersionService.Readers
{
    public class FileReadHistory : IReadHistory
    {
        private readonly ILog _log;
        private readonly ICheckHistory _check;
        public FileReadHistory(ILog log, ICheckHistory check)
        {
            _log = log;
            _check = check;
        }

        /// <summary>
        /// Подгрузка актуальных данных из директории
        /// </summary>
        /// <param name="folder">Директория подгрузки</param>
        /// <param name="extension">Расширение файла, указывать без точки</param>
        /// <returns></returns>
        public async Task<FolderInfo> GetActualHistory(string folder)
        {
            if(Directory.Exists(folder))
            {
                FolderInfo folderInfo = new FolderInfo(folder);

                foreach(var pathFile in Directory.GetFiles(folder))
                {
                    _log.Debug($"Work with [{pathFile}]");

                    var fileInfo = new FileInfo(pathFile);

                    fileInfo.Hash = await _check.GetCalcValue(fileInfo);
                    fileInfo.ChangeStatus = FileChangeStatus.Nothing;
                    fileInfo.ChangeTime = File.GetLastWriteTime(pathFile);

                    _log.Debug($"Add file [{pathFile}] to memory");
                    folderInfo.Files.Add(fileInfo);
                }

                return folderInfo;
            }
            else
            {
                _log.Error($"Folder {folder} is not found");
                return FolderInfo.Empty;
            }
        }


        private IEnumerable<string> GetExtensions(string directory)
        {
            ICollection<string> fileExtensions = new List<string>();

            foreach (var file in Directory.GetFiles(directory)) // Поиск расширений файла
            {
                int extStartIndex = file.IndexOf('.');
                fileExtensions.Add(file.Substring(extStartIndex));
            }

            return fileExtensions;
        }

        /// <summary>
        /// Подгрузка файлов из кэша
        /// </summary>
        /// <param name="folder">Директория подгрузки</param>
        /// <param name="extension">Расширение файла, указывать без точки</param>
        /// <returns></returns>
        public async Task<FolderInfo> GetCachedFilesHistory(string folder)
        {
            if (!Directory.Exists(folder))
            {
                _log.Error($"Folder {folder} is not found");
                return FolderInfo.Empty;
            }

            FolderInfo folderInfo = new FolderInfo(folder);
            
            foreach (var cacheDirectory in Directory.GetDirectories(folder)) // Получаем данные
            {
                if (cacheDirectory.EndsWith("txt", StringComparison.OrdinalIgnoreCase)
                    || cacheDirectory.EndsWith("xml", StringComparison.OrdinalIgnoreCase)) // Игнорирование 'системных' папок
                {
                    _log.Debug("Skip folder");
                    continue;
                }

                _log.Debug($"Get cache from {cacheDirectory}");
                // {file}.* => {file}/[0...x] - паттерн кэша

                IEnumerable<string> extensions = GetExtensions(cacheDirectory);
                foreach (var ext in extensions) // Поиск файлов по расширению
                {

                    var lastFilePath = Directory.GetFiles(cacheDirectory, $"*.{ext}").LastOrDefault(); // Получаем последний файл
                    if (lastFilePath != null)
                    {
                        var fileExtPath = lastFilePath.Split('.').LastOrDefault(); // Получаем расширение файла
                        var fileWithExtension = cacheDirectory + '.' + fileExtPath; // Полный путь до нужного файла

                        _log.Debug($"Add cached file [{cacheDirectory}] to memory");
                        var fileInfo = new FileInfo(fileWithExtension);

                        fileInfo.Hash = await _check.GetCalcValue(fileInfo);
                        fileInfo.ChangeStatus = FileChangeStatus.Nothing;
                        fileInfo.ChangeTime = File.GetLastWriteTime(fileWithExtension);

                        folderInfo.Files.Add(fileInfo);
                    }
                    else
                    {
                        _log.Error($"Folder {folder} is not contains cache files");
                    }

                }
            }

            return folderInfo;
        }
    }
}
