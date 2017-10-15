using System.Collections.ObjectModel;
using System.Linq;

namespace Zenos.Framework
{
    public class Sections : Collection<Section>
    {
        public Section this[string name]
        {
            get {
                return this.FirstOrDefault(s => s.Name == name) ?? CreateSection(name);
            }
        }

        private Section CreateSection(string name)
        {
            var section = new Section(name);
            this.Add(section);
            return section;
        }
    }
}