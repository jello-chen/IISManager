using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Readers;
using System;
using System.IO;
using System.Linq;

namespace IISManager.Core.Utils
{
    public class ZipUtil
    {
        public static bool UnRar(Stream stream, string destination, bool ExtractFullPath = true, bool Overwrite = true, bool PreserveFileTime = true)
        {
            try
            {
                destination = PrepareDirectory(destination);
                var options = new ExtractionOptions { ExtractFullPath = ExtractFullPath, Overwrite = Overwrite, PreserveFileTime = PreserveFileTime };
                using (var archive = RarArchive.Open(stream))
                {
                    foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                    {
                        entry.WriteToDirectory(destination, options);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string UnZip(string src, string destination, bool ExtractFullPath = true, bool Overwrite = true, bool PreserveFileTime = true, bool isDirectory = false)
        {
            try
            {
                var options = new ExtractionOptions { ExtractFullPath = ExtractFullPath, Overwrite = Overwrite, PreserveFileTime = PreserveFileTime };
                using (var archive = ZipArchive.Open(src))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.IsDirectory == isDirectory)
                            entry.WriteToDirectory(destination, options);
                    }
                }
                return string.Empty;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool UnZip(Stream src, string destination, bool ExtractFullPath = true, bool Overwrite = true, bool PreserveFileTime = true, bool isDirectory = false)
        {
            try
            {
                var options = new ExtractionOptions { ExtractFullPath = ExtractFullPath, Overwrite = Overwrite, PreserveFileTime = PreserveFileTime };
                using (var archive = ZipArchive.Open(src))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.IsDirectory == isDirectory)
                            entry.WriteToDirectory(destination, options);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Zip(string src, string savePath)
        {
            try
            {
                using (var archive = ZipArchive.Create())
                {
                    archive.AddAllFromDirectory(src);
                    using (var stream = new FileStream(savePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        archive.SaveTo(stream);
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool IsRar(Stream stream)
        {
            return RarArchive.IsRarFile(stream);
        }

        public static bool IsZip(Stream stream, string password = null)
        {
            return ZipArchive.IsZipFile(stream, password);
        }

        private static string PrepareDirectory(string destination)
        {
            if (Path.IsPathRooted(destination))
            {
                if (!Directory.Exists(destination))
                    Directory.CreateDirectory(destination);
                return destination;
            }
            else
            {
                var absoluteDestination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destination);
                if (!Directory.Exists(absoluteDestination))
                    Directory.CreateDirectory(absoluteDestination);
                return absoluteDestination;
            }
        }
    }
}
