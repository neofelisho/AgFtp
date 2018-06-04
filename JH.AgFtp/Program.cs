using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.Serialization;
using FluentFTP;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace JH.AgFtp
{
    public class Program
    {
        private static readonly string FtpAddress = ConfigurationManager.AppSettings["FtpAddress"];
        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"];
        private static readonly string Password = ConfigurationManager.AppSettings["Password"];

        public static void Main(string[] args)
        {
            var beginTime = DateTime.UtcNow.AddHours(-4).AddMinutes(-2); //AG伺服器的時區是EST(美國東部時區夏令時)

            var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("agftp");
            container.CreateIfNotExists();

            //var localPaths = new List<string>();
            Console.WriteLine("--- Starting To Sync AG Data ---");
            using (var client = new FtpClient(FtpAddress, new NetworkCredential(UserName, Password)))
            {
                client.Connect();
                var dirs = client.GetListing();
                foreach (var dir in dirs)
                {
                    var subdirs = client.GetListing(dir.FullName).Where(p => p.Modified >= beginTime).ToList();

                    foreach (var subdir in subdirs)
                    {
                        if (subdir.Name == "lostAndfound")
                        {
                            var lostSubDirs = client.GetListing(subdir.FullName).Where(p => p.Modified >= beginTime);
                            foreach (var lostSubDir in lostSubDirs)
                                //var localPath = Path.Combine(dir.Name, subdir.Name, lostSubDir.Name);
                                SyncFilesToCloud(container, client, lostSubDir.FullName, beginTime);
                        }
                        else
                        {
                            //var localPath = Path.Combine(dir.Name, subdir.Name);
                            SyncFilesToCloud(container, client, subdir.FullName, beginTime);
                        }

                        foreach (var blob in container.ListBlobs())
                            Console.WriteLine(blob.Uri);
                    }
                }
            }

            //var list = new List<IAgDataObject>();
            //foreach (var localPath in localPaths)
            //{
            //    //ParseXml(localPath, list);
            //}

            Console.WriteLine("--- Finished To Sync AG Data ---");
            Console.ReadLine();
        }

        private static void SyncFilesToCloud(CloudBlobContainer container, IFtpClient client, string directoryFullPath,
            DateTime beginTime)
        {
            var remoteFiles = client.GetListing(directoryFullPath).Where(p => p.Modified >= beginTime).ToList();

            foreach (var remoteFile in remoteFiles)
            {
                if (remoteFile.Size <= 0)
                    continue;

                if (!client.Download(out var buffer, remoteFile.FullName)) continue;

                var blockBlob = container.GetBlockBlobReference(remoteFile.FullName.Substring(1));
                if (blockBlob.Exists())
                {
                    blockBlob.FetchAttributes();
                    if (blockBlob.Properties.Length == remoteFile.Size)
                        Console.WriteLine($"File {remoteFile.FullName} exists on cloud.");
                    continue;
                }
                blockBlob.UploadFromByteArray(buffer,0,buffer.Length);
                Console.WriteLine($"Upload {remoteFile.FullName} to cloud.");
            }
        }

        private static void ParseXml(string localPath, ICollection<IAgDataObject> list)
        {
            var fragments = File.ReadAllText(localPath);
            var xml = "<root>" + fragments + "</root>";
            var doc = XDocument.Parse(xml);
            var rows = doc.Descendants("root").Elements("row");
            foreach (var row in rows)
            {
                var dataType = row.Attribute("dataType")?.Value;
                XmlSerializer serializer;
                switch (dataType)
                {
                    case "BR":
                        serializer = new XmlSerializer(typeof(AgLottery));
                        list.Add((AgLottery) serializer.Deserialize(row.CreateReader()));
                        break;
                    case "EBR":
                        serializer = new XmlSerializer(typeof(AgElectronic));
                        list.Add((AgElectronic) serializer.Deserialize(row.CreateReader()));
                        break;
                    case "HSR":
                        serializer = new XmlSerializer(typeof(AgHunter));
                        list.Add((AgHunter) serializer.Deserialize(row.CreateReader()));
                        break;
                    case "TR":
                        serializer = new XmlSerializer(typeof(AgTransfer));
                        list.Add((AgTransfer) serializer.Deserialize(row.CreateReader()));
                        break;
                    default:
                        continue;
                }
            }
        }

        private static IEnumerable<string> DownloadFiles(string localPath, IFtpClient client, string directoryFullPath,
            DateTime beginTime)
        {
            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);
            var localFiles = new DirectoryInfo(localPath).GetFiles("*.*");

            var remoteFiles = client.GetListing(directoryFullPath).Where(p => p.Modified >= beginTime);
            var downloads = new List<string>();
            var localFullPaths = new List<string>();
            foreach (var remoteFile in remoteFiles)
            {
                if (remoteFile.Size <= 0)
                    continue;
                if (localFiles.Any(p => p.Name == remoteFile.Name))
                {
                    var localFile = localFiles.Single(p => p.Name == remoteFile.Name);
                    if (localFile.Length == remoteFile.Size)
                        continue;
                }
                downloads.Add(remoteFile.FullName);
                localFullPaths.Add(Path.Combine(localPath, remoteFile.Name));
            }
            client.DownloadFiles(localPath, downloads);
            Console.WriteLine($"Download {downloads.Count} records from {directoryFullPath}...");
            return localFullPaths;
        }
    }
}