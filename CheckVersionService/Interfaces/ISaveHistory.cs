using CheckVersionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckVersionService.Interfaces
{
    public interface ISaveHistory
    {
        Task Save(FileInfo fileInfo);
    }
}
