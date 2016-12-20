using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Busines.IDAO
{
    public interface FileUpload_IDal
    {
        bool SaveImages(string unSubscribeID, string fileType, string extName, string imagePath, string webUrl, string fileName, string showName, string tableNames);
        DataTable GetFilesByUnsubscribeID(string unsubscribeID);
        bool DeleteUploadFileByFileID(string FileID);
    }
}
