using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models
{
    public class ItemDefinition : IComparable<ItemDefinition>
    {
        public string Definition;

        public int CompareTo(ItemDefinition obj)
        {
            return string.Compare(Definition, obj.Definition);
        }
    }
}
