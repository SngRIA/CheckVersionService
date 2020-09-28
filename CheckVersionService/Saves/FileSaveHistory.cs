using CheckVersionService.Interfaces;
using CheckVersionService.Models;
using log4net;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileInfo = CheckVersionService.Models.FileInfo;

namespace CheckVersionService.Saves
{
    public class FileSaveHistory : ISaveHistory
    {
        private readonly ILog _log;
        public FileSaveHistory(ILog log)
        {
            _log = log;
        }

        /// <summary>
        /// Сохраняет файл в указанную директорию
        /// </summary>
        /// <param name="fileInfo">Информация о файле</param>
        public async Task Save(FileInfo fileInfo)
        {
            string[] pathSplit = fileInfo.Path.Split('.');
            string clearPath = pathSplit.FirstOrDefault(); // Удаляем расширение файла (.*)
            string ext = pathSplit.LastOrDefault();// Получаем расширение файла (.*);

            if (!Directory.Exists(clearPath))
            {
                _log.Warn($"Directory [{clearPath}] not found, create new directory");
                await Task.Factory.StartNew(() => Directory.CreateDirectory(clearPath));
            }

            string[] files = await Task.Factory.StartNew(() => Directory.GetFiles(clearPath));

            string newFileName = files.Length.ToString(); // Получаем количество файлов в директории
            await Task.Factory.StartNew(() => File.Copy(fileInfo.Path, $"{clearPath}/{newFileName}.{ext}")); // Копируем новую весию файла
        }
    }
}
