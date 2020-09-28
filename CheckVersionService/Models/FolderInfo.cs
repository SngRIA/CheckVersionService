using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckVersionService.Models
{
    public sealed class FolderInfo
    {
        public static FolderInfo Empty = new FolderInfo("empty");

        public string Path { get; set; }
        public ICollection<FileInfo> Files { get; set; }
        public FolderInfo(string path)
        {
            Path = path;
            Files = new List<FileInfo>();
        }
    }
}
