using System.Collections.ObjectModel;
using System.Linq;

namespace Zenos.Framework
{
    public class Sections : Collection<Section>
    {
        public Section this[string name]
        {
            get { 
                var section = this.FirstOrDefault(s => s.Name == name);
                if (section == null)
                {
                    section = new Section(name);
                    this.Add(section);
                }

                return section;
            }
        }
    }
}