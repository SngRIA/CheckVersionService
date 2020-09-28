using CheckVersionService.Models;
using System.Threading.Tasks;

namespace CheckVersionService.Interfaces
{
    public interface IReadHistory
    {
        Task<FolderInfo> GetActualHistory(string folder);
        Task<FolderInfo> GetCachedFilesHistory(string folder);
    }
}
