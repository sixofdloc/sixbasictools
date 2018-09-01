using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SixBASIC
{
   public  class MicrosoftBasic2Token
    {
       public byte Token {get; set;}
       public string ASCII { get; set; }
       public byte[] PETSCII { get; set; }
       public bool TrailingSpace { get; set; }
       public bool LeadingSpace { get; set; }
       public bool CanExpand { get; set; }
      
    }
}
