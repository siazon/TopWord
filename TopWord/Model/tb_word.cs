using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopWord.Model
{
     public class tb_word
    {
        public string keyid { get; set; }
        public string word { get; set; }
        public string  phonetic { get; set; }
        public string meaning { get; set; }
        public string sentence { get; set; }
        public int type { get; set; }
        public string header { get; set; }
        public int seq { get; set; }
    }
}
