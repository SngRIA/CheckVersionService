using CheckVersionService.Interfaces;
using System.IO;

namespace CheckVersionService.Config.Validators
{
    public class ConfigValidator
    {
        // Проверка валидности периода и указанной папки
        public bool IsValid(IConfig config)
        {
            bool result = true;

            foreach(var folder in config.GetFolders())
            { 
                if(!Directory.Exists(folder.Path) && folder.Period == null)
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
