using System;
using sixbasiclib.Enums;
namespace sixbasiclib
{
    public class Element
    {
		public ElementType ElementType { get; set; } 
        public string Value { get; set; } = string.Empty;

        public Element()
        {
        }
    }
}
