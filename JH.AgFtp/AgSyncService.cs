using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JH.AgFtp
{
    public class AgSyncService : IAgSyncService
    {
        private static readonly string FtpAddress = ConfigurationManager.AppSettings["FtpAddress"];
        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"];
        private static readonly string Password = ConfigurationManager.AppSettings["Password"];

        private FtpWebRequest _ftpWebRequest;

        /// <summary>
        ///     List files in datetime range, compare size, and then download the necessary parts.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetFiles()
        {
            var list = ListDirectory(string.Empty);
            var directories = list as IList<string> ?? list.ToList();
            return directories.Select(DownloadDir).SelectMany(p => p);

            //var result = await Task.WhenAll(tasks);
            //return result.SelectMany(p => p);
            //var downloads = new List<string>();
            //foreach (var dir in directories)
            //{
            //    downloads.AddRange(await DownloadDir(dir));
            //}
            //return downloads;
        }

        private void Connect(string path)
        {
            _ftpWebRequest = (FtpWebRequest) WebRequest.Create(new Uri($"{FtpAddress}/{path}"));

            _ftpWebRequest.UseBinary = true;
            _ftpWebRequest.UsePassive = true;
            _ftpWebRequest.KeepAlive = false;
            _ftpWebRequest.Timeout = 120000;

            _ftpWebRequest.Credentials = new NetworkCredential(UserName, Password);
        }

        private IEnumerable<string> ListDirectory(string path)
        {
            Connect(path);
            _ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;

            using (var ftpResponse = _ftpWebRequest.GetResponse())
            {
                using (var sr =
                    new StreamReader(ftpResponse.GetResponseStream() ?? throw new InvalidOperationException(),
                        Encoding.Default))
                {
                    while (true)
                    {
                        var line = sr.ReadLine();
                        if (string.IsNullOrEmpty(line)) break;
                        yield return line;
                    }
                }
            }
        }

        private long GetFileSize(string filePath)
        {
            Connect(filePath);
            _ftpWebRequest.Method = WebRequestMethods.Ftp.GetFileSize;

            using (var ftpResponse = _ftpWebRequest.GetResponse())
            {
                return ftpResponse.ContentLength;
            }
        }

        private void DownloadFile(string filePath)
        {
            Connect(filePath);
            _ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            using (var ftpResponse = _ftpWebRequest.GetResponse())
            {
                using (var s = ftpResponse.GetResponseStream() ?? throw new InvalidOperationException())
                {
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        const int length = 2048;
                        var buffer = new byte[length];
                        var bytesRead = s.Read(buffer, 0, length);
                        while (bytesRead > 0)
                        {
                            fs.Write(buffer, 0, bytesRead);
                            bytesRead = s.Read(buffer, 0, length);
                        }
                    }
                }
            }
        }

        private IEnumerable<string> DownloadDir(string dir)
        {
            var list = ListDirectory(dir + "//" + DateTime.Now.ToString("yyyyMMdd"));
            var remoteFiles = list as IList<string> ?? list.ToList();

            if (!Directory.Exists(dir + "//" + DateTime.Now.ToString("yyyyMMdd")))
                Directory.CreateDirectory(dir + "//" + DateTime.Now.ToString("yyyyMMdd"));

            var localFiles = new DirectoryInfo(dir + "//" + DateTime.Now.ToString("yyyyMMdd")).GetFiles("*.*");
            return remoteFiles.Select(remoteFile =>
            {
                var remoteFileSize = GetFileSize(dir + "//" + remoteFile);
                if (remoteFileSize <= 0) return null;
                if (localFiles.Any(p => p.Name == remoteFile))
                {
                    var localFile = localFiles.Single(p => p.Name == remoteFile);
                    if (localFile.Length == remoteFileSize)
                        return null;
                    localFile.Delete();
                }

                DownloadFile(dir + "//" + remoteFile);
                return remoteFile;
            });

            //foreach (var remoteFile in remoteFiles)
            //{
            //    var remoteFileSize = GetFileSize(dir + "//" + remoteFile);
            //    if (remoteFileSize > 0)
            //    {
            //        if (localFiles.Any(p => p.Name == remoteFile))
            //        {
            //            var localFile = localFiles.Single(p => p.Name == remoteFile);
            //            if (localFile.Length == remoteFileSize)
            //                continue;
            //            localFile.Delete();
            //        }
            //        DownloadFile(dir + "//" + remoteFile);
            //    }
            //    else
            //    {
            //        remoteFiles.Remove(remoteFile);
            //    }
            //}
            //return remoteFiles;
        }

        /// <summary>
        ///     Different kinds of parser.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAgDataObject> ParseFile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Save the parsed date to database.
        /// </summary>
        /// <returns></returns>
        public bool SaveData()
        {
            throw new NotImplementedException();
        }
    }
}