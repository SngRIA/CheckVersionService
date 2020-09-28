using CheckVersionService.Models;
using System.Threading.Tasks;

namespace CheckVersionService.Interfaces
{
    public interface ICheckHistory
    {
        Task<string> GetCalcValue(FileInfo file);
        bool IsChanged(FileInfo newFile, FileInfo oldFile);
    }
}
