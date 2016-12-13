using System;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System.Text;

namespace KunTaiServiceLibrary
{
    public class Zip
    {

        #region 压缩文件、文件夹

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZipPath">要压缩文件的路径</param>
        /// <param name="zipedFilePath">压缩文件的路径</param>
        /// <param name="compressionLevel">压缩级别(0(无压缩) -9(压缩率最高)</param>
        public static void CompressionFile(string fileToZipPath, string zipedFilePath, int compressionLevel)
        {
            //如果文件没有找到，则报错   
            if (!System.IO.File.Exists(fileToZipPath))
            {
                throw new FileNotFoundException("文件：" + fileToZipPath + "没有找到！");
            }

            if (zipedFilePath == string.Empty)
            {
                zipedFilePath = Path.GetFileNameWithoutExtension(fileToZipPath) + ".zip";
            }

            if (Path.GetExtension(zipedFilePath) != ".zip")
            {
                zipedFilePath = zipedFilePath + ".zip";
            }

            ////如果指定位置目录不存在，创建该目录  
            //string zipedDir = ZipedFile.Substring(0,ZipedFile.LastIndexOf("/"));  
            //if (!Directory.Exists(zipedDir))  
            //    Directory.CreateDirectory(zipedDir);  

            //被压缩文件名称  
            string filename = fileToZipPath.Substring(fileToZipPath.LastIndexOf('/') + 1);

            System.IO.FileStream StreamToZip = new System.IO.FileStream(fileToZipPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.FileStream ZipFile = System.IO.File.Create(zipedFilePath);
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
            ZipEntry ZipEntry = new ZipEntry(filename);
            ZipStream.PutNextEntry(ZipEntry);
            ZipStream.SetLevel(compressionLevel);
            byte[] buffer = new byte[2048];
            System.Int32 size = StreamToZip.Read(buffer, 0, buffer.Length);
            ZipStream.Write(buffer, 0, size);
            try
            {
                while (size < StreamToZip.Length)
                {
                    int sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                    ZipStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                ZipStream.Finish();
                ZipStream.Close();
                StreamToZip.Close();
            }
        }

        /// <summary>
        /// 压缩文件夹的方法
        /// </summary>
        /// <param name="directoryToZipPath">要压缩文件夹的路径</param>
        /// <param name="zipedFilePath">压缩文件的路径</param>
        /// <param name="compressionLevel">压缩级别(0(无压缩) -9(压缩率最高)</param>
        public static void CompressionDirectory(string directoryToZipPath, string zipedFilePath, int compressionLevel)
        {
            //压缩文件为空时默认与压缩文件夹同一级目录  
            if (zipedFilePath == string.Empty)
            {
                zipedFilePath = directoryToZipPath.Substring(directoryToZipPath.LastIndexOf("/") + 1);
                zipedFilePath = directoryToZipPath.Substring(0, directoryToZipPath.LastIndexOf("/")) + "//" + zipedFilePath + ".zip";
            }

            if (Path.GetExtension(zipedFilePath) != ".zip")
            {
                zipedFilePath = zipedFilePath + ".zip";
            }

            using (ZipOutputStream zipoutputstream = new ZipOutputStream(File.Create(zipedFilePath)))
            {
                zipoutputstream.SetLevel(compressionLevel);
                Crc32 crc = new Crc32();
                Hashtable fileList = getAllFies(directoryToZipPath);
                foreach (DictionaryEntry item in fileList)
                {
                    FileStream fs = File.OpenRead(item.Key.ToString());
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ZipEntry entry = new ZipEntry(item.Key.ToString().Substring(directoryToZipPath.Length + 1));
                    entry.DateTime = (DateTime)item.Value;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipoutputstream.PutNextEntry(entry);
                    zipoutputstream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>  
        /// 获取所有文件  
        /// </summary>  
        /// <returns></returns>  
        private static Hashtable getAllFies(string directory)
        {
            Hashtable FilesList = new Hashtable();
            DirectoryInfo fileDire = new DirectoryInfo(directory);
            if (!fileDire.Exists)
            {
                throw new System.IO.FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }

            getAllDirFiles(fileDire, FilesList);
            getAllDirsFiles(fileDire.GetDirectories(), FilesList);
            return FilesList;
        }
        /// <summary>  
        /// 获取一个文件夹下的所有文件夹里的文件  
        /// </summary>  
        /// <param name="dirs"></param>  
        /// <param name="filesList"></param>  
        private static void getAllDirsFiles(DirectoryInfo[] dirs, Hashtable filesList)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                foreach (FileInfo file in dir.GetFiles("*.*"))
                {
                    filesList.Add(file.FullName, file.LastWriteTime);
                }
                getAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }
        /// <summary>  
        /// 获取一个文件夹下的文件  
        /// </summary>  
        /// <param name="strDirName">目录名称</param>  
        /// <param name="filesList">文件列表HastTable</param>  
        private static void getAllDirFiles(DirectoryInfo dir, Hashtable filesList)
        {
            foreach (FileInfo file in dir.GetFiles("*.*"))
            {
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }

        #endregion

        #region 解压文件

        /// <summary>  
        /// 解压zip格式的文件
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径</param>  
        /// <param name="unZipDirectory">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>
        public static void Decompression(string zipFilePath, string unZipDirectory)
        {
            if (zipFilePath == string.Empty)
            {
                throw new Exception("压缩文件不能为空！");
            }
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("压缩文件不存在！");
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
            if (unZipDirectory == string.Empty)
                unZipDirectory = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDirectory.EndsWith("/"))
                unZipDirectory += "/";
            if (!Directory.Exists(unZipDirectory))
                Directory.CreateDirectory(unZipDirectory);

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(unZipDirectory + directoryName);
                    }
                    if (!directoryName.EndsWith("/"))
                        directoryName += "/";
                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(unZipDirectory + theEntry.Name))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        #endregion

    }
}
