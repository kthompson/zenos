using System.IO;

namespace Zenos.Framework
{
    public class Section : StringWriter
    {
        public string Name { get; private set; }

        public Section(string name)
        {
            this.Name = name;
        }
    }
}