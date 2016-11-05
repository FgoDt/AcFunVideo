using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AcFunVideo
{
   public class ACDEBUG
    {
        public static void Print(string msg)
        {
            Debug.WriteLine("*************************************************");
            Debug.WriteLine("->AVMSG : " + msg);
            Debug.WriteLine("*************************************************");
        }
    }
}
