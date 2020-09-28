using CheckVersionService.Enums;
using System;

namespace CheckVersionService.Models
{
    public sealed class FileInfo
    {
        public string Path { get; set; }
        public string Hash { get; set; }
        public FileChangeStatus ChangeStatus { get; set; }
        public DateTime ChangeTime { get; set; }

        public FileInfo(string path)
        {
            Path = path;
        }
    }
}
