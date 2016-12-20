using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Busines.MODEL
{
    public class Month
    {
        public string ID
        {
            get;
            set;
        }
        public string Name
        {
            set
            {
                Name= ID + " 月";
            }
        }
    }
}
