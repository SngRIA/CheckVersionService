using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckVersionService.Config.Elements
{
    public class FolderCollection : ConfigurationElementCollection
    {
        public FolderElement this[int index]
        {
            get
            {
                return (FolderElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        public FolderElement this[string key]
        {
            get
            {
                return (FolderElement)BaseGet(key);
            }
            set
            {
                if (BaseGet(key) != null)
                    BaseRemoveAt(BaseIndexOf(BaseGet(key)));

                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FolderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FolderElement)element).Path;
        }
    }
}
