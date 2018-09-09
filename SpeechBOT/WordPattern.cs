using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechBOT
{
    public class WordPattern
    {
        public string Key { get; set; }

        public bool IsProcessCommand { get; set; }

        public string Command { get; set; }

        public string Reply { get; set; }
    }
}
