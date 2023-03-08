using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sixbasiclib
{
   public  class MicrosoftBasic2Token
    {
       public byte Token {get; set;}
       public string ASCII { get; set; } = string.Empty;
       public byte[] PETSCII { get; set; }= new byte[0];
       public bool TrailingSpace { get; set; }
       public bool LeadingSpace { get; set; }
       public bool CanExpand { get; set; }
      
    }
}
