using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KunTaiServiceLibrary
{
    interface IController
    {
        
        string getDataItem(string text);

        string addDataItem(string text);

        string editDataItem(string text);
        
        string deleteDataItem(string text);

        string getDataItemDetails(string text);

    }
}
