using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models.Events
{
    public class TextureEvent
    {
        public string DefinitionId { get; set; }
        public string TexturePath { get; set; }
        public byte[] Base64Texture { get; set; }
    }
}
