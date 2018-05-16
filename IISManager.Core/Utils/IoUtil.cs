using System;
using System.IO;

namespace IISManager.Core.Utils
{
    public class IoUtil
    {
        public static string CopyDirectory(string sourceDirPath, string saveDirPath, bool isOverride = true, Func<string, bool> ignoreFunc = null)
        {
            try
            {
                if (!Directory.Exists(saveDirPath))
                {
                    Directory.CreateDirectory(saveDirPath);
                }
                string[] files = Directory.GetFiles(sourceDirPath);
                foreach (string file in files)
                {
                    string filename = Path.GetFileName(file);
                    if (ignoreFunc != null && ignoreFunc(filename)) continue;
                    string pFilePath = Path.Combine(saveDirPath, filename);
                    File.Copy(file, pFilePath, isOverride);
                }

                string[] dirs = Directory.GetDirectories(sourceDirPath);
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, Path.Combine(saveDirPath, Path.GetFileName(dir)));
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
