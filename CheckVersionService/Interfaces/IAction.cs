using CheckVersionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckVersionService.Interfaces
{
    public interface IAction
    {
        void Execute(FileInfo fileInfo, string rootFolder);
    }
}
