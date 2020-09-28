using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckVersionService.Config.Elements
{
    public class FolderElement : ConfigurationElement
    {
        // Период проверки
        [ConfigurationProperty("period", IsKey = true, IsRequired = true)]
        public string Every
        {
            get
            {
                return (string)base["period"];
            }
            set
            {
                base["period"] = value;
            }
        }

        // Путь к папке
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)base["path"];
            }
            set
            {
                base["path"] = value;
            }
        }
    }
}
