using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckVersionService.Config.Elements
{
    public class CheckVersionConfig : ConfigurationSection
    {
        public static CheckVersionConfig GetConfig()
        {
            return (CheckVersionConfig)ConfigurationManager.GetSection("CheckVersion") ?? new CheckVersionConfig();
        }

        [ConfigurationProperty("Folders")]
        [ConfigurationCollection(typeof(FolderCollection), AddItemName = "Folder")]
        public FolderCollection Folders
        {
            get
            {
                return (FolderCollection)this["Folders"];
            }
        }
    }
}
