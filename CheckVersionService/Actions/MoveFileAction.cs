using CheckVersionService.Interfaces;
using CheckVersionService.Models;
using log4net;
using System;
using System.IO;
using System.Linq;
using FileInfo = CheckVersionService.Models.FileInfo;

namespace CheckVersionService.Actions
{
    public class MoveFileAction : IAction
    {
        private readonly ILog _log;
        private string _rootFolder;
        public MoveFileAction(ILog log)
        {
            _log = log;
        }

        private void СheckDirectory(string rootFolder, string folderName)
        {
            string directory = $"{rootFolder}\\{folderName}";
            if (!Directory.Exists(directory))
            {
                _log.Info($"Create directory [{folderName}]");
                Directory.CreateDirectory(directory);
            }
            else
            {
                _log.Debug($"Directory [{folderName}] exists");
            }
        }
        private void СopyFile(string fromPath, string toPath, string extension)
        {
            if (!File.Exists(toPath))
            {
                File.Copy(fromPath, toPath);
            }
            else
            {
                _log.Warn($"File [{fromPath}] exist, create copy with time");

                string[] filePaths = toPath.Split('\\');
                string[] fileNamePaths = filePaths.LastOrDefault().Split('.');

                string fileName = fileNamePaths.FirstOrDefault();
                string fileExt = fileNamePaths.LastOrDefault();

                string outFilePath = $"{_rootFolder}\\{extension}\\{fileName}_{DateTime.Now:dd.MM.yyyy_hh.mm.ss}.{fileExt}";

                File.Move(toPath, outFilePath); // Переименовываем старый файл 
                File.Copy(fromPath, toPath, true); // Создаём копию
            }
        }

        /// <summary>
        /// Выполняет копирование файла в зависимости от расширения
        /// </summary>
        /// <param name="fileInfo"></param>
        public void Execute(FileInfo fileInfo, string rootFolder)
        {
            _rootFolder = rootFolder;

            string fileName = fileInfo.Path.Split('\\').LastOrDefault();
            string ext = fileInfo.Path.Split('.').LastOrDefault();
            if (!ext.Equals(string.Empty))
            {
                switch (ext)
                {
                    case "txt":
                        СheckDirectory(rootFolder, "txt");
                        СopyFile(fileInfo.Path, $"{rootFolder}\\txt\\{fileName}", "txt");
                        break;
                    case "xml":
                        СheckDirectory(rootFolder, "xml");
                        СopyFile(fileInfo.Path, $"{rootFolder}\\xml\\{fileName}", "xml");
                        break;
                    default:
                        _log.Error($"Wrong file extension [{ext}]");
                        break;
                }
            }
            else
            {
                _log.Error($"Wrong path [{fileInfo.Path}]");
            }
        }
    }
}
