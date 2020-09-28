using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckVersionService.Models
{
    public sealed class FolderConfig
    {
        public string Path { get; set; }

        public TimeSpan Period { get; set; }
        public FolderConfig(string path, TimeSpan period)
        {
            Path = path;
            Period = period;
        }
    }
}
