using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace helper
{
    public class ColorChange
    {
        public string rgbTohex(byte R, byte G, byte B)
        {
            string hexcolor;
           hexcolor=ColorTranslator.FromHtml(String.Format("#{0:X2}{1:X2}{2:X2}", cRGB.R, cRGB.G, cRGB.B)).Name.Remove(0, 2);
        }
    }
}
