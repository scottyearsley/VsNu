using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsNu
{
    public class AssemblyRef
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public static AssemblyRef Create(string name)
        {
            var assembyRef = new AssemblyRef();
            var split = name.Split(',');

            for (int index = 0; index < split.Length; index++)
            {
                var item = split[index];

                switch (index)
                {
                    case 0:
                        assembyRef.Name = item;
                        break;
                    case 1:
                        assembyRef.Version = item.Split('=')[1];
                        break;
                    default:
                        break;
                }
            }

            return assembyRef;
        }
    }
}
