using CheckVersionService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckVersionService.Services
{
    public class FileServiceFolder
    {
        public FileServiceFolder(IReadHistory read, ICheckHistory check, ISaveHistory save)
        {
            Read = read;
            Check = check;
            Save = save;
        }

        public IReadHistory Read { get; }

        public ICheckHistory Check { get; }

        public ISaveHistory Save { get; }
    }
}
