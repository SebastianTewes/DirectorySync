using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using System.Xml.Serialization;



namespace DirectorySync
{
    class Program
    {
        static void Main(string[] args)
        {
            bool debug = false;
            // default parameter "../../config.xml"
            if (args.Length == 1)
            {
                if (args[0] == "-h" | args[0] == "--help")
                {
                    Console.Write(
                        "The directorySync programm will sync the in local and remote provider configured\r\n" + 
                        "folders corresponding on the sync filter and sync options once the program is executed (manual sync)\r\n\r\n" + 
                        "-h, --help                             | Show command options\r\n" +
                        "-o, --option                           | Show parameter options\r\n" +
                        "-e, --example                          | Show config.xml example\r\n" +
                        "-d, --debug   (before config)          | Show used config.xml content befor sync"
                        );
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                if (args[0] == "-o" | args[0] == "--option")
                {
                    Console.Write(
                        "[direction]\r\n" +
                        "   Download                            | sync remote provider to local provider\r\n" +
                        "   DownloadAndUpload                   | first sync remote provider to local provider\r\n" +
                        "                                         then from remote provider to local provider\r\n" +
                        "   Upload                              | sync from local provider to remote provider\r\n" +
                        "   UploadAndDownload                   | first sync remote provider to local provider\r\n" +
                        "                                         then from remote provider to local provider\r\n\r\n" +
                        "[path]                                 | folder to sync\r\n" +
                        "\r\n" +
                        "[syncFilter]                           | FileSyncScopeFilter\r\n" +
                        "   [AttributeExcludeMask]              | comma seperated list of attributes that are used\r\n" + 
                        "       Archive                           to exclude files and folders from the scope\r\n" +
                        "       Compressed\r\n" +
                        "       Device\r\n" +
                        "       Directory\r\n" +
                        "       Encrypted\r\n" +
                        "       Hidden\r\n" +
                        "       IntegrityStream\r\n" +
                        "       Normal\r\n" +
                        "       NoScrubData\r\n" +
                        "       NotContentIndexed\r\n" +
                        "       Offline\r\n" +
                        "       ReadOnly\r\n" +
                        "       ReparsePoint\r\n" +
                        "       SparseFile\r\n" +
                        "       System\r\n" +
                        "       Temporary\r\n" +
                        "\r\n" +
                        "   [FileNameExcludes]                  - comma separated list e.g. *.csv, *.txt to exclude files from the scope\r\n" +
                        "   [FileNameIncludes]                  - comma seperated list e.g. hello.*, *.data to include files to the scope\r\n" +
                        "   [SubdirectoryExcludes]              - comma seperated list of relative paths to exclude from the scope\r\n\r\n" +
                        "[syncOptions]                          - FileSyncOptions [true/false]\r\n" +
                        "   None                                - If this value is set, the provider will use its default configuration options.\r\n" +
                        "                                         Setting any of the other flags overrides this setting. This is the default\r\n" + 
                        "                                         setting. \r\n" +
                        "   CompareFileStreams                  - If this value is set, the provider will compute a hash value for each file\r\n" +
                        "                                         that is based on the contents of the whole file stream and use this value to\r\n" +
                        "                                         compare files during change detection. This option is expensive and will slow\r\n" +
                        "                                         synchronization, but provides more robust change detection. If this value is\r\n" +
                        "                                         not set, an algorithm that compares modification times, file sizes, file names,\r\n" +
                        "                                         and file attributes will be used to determine whether a file has changed. \r\n" +
                        "   ExplicitDetectChanges               - If this value is set, the provider will perform change detection only when\r\n" + 
                        "                                         DetectChanges is called. If this value is not set, change detection is\r\n" +
                        "                                         implicitly done on the first call to the provider's GetChangeBatch or Process-\r\n" +
                        "                                         ChangeBatch method.\r\n" + 
                        "   RecycleConflictLoserFiles           - If this value is set, the provider will move files that are conflict losers to\r\n" +
                        "                                         the recycle bin. If this value is not set, the provider will move the files to\r\n" +
                        "                                         a specified location. Or, if no location is specified, the files will be\r\n" +
                        "                                         permanently deleted. \r\n" +
                        "   RecycleDeletedFiles                 - If this value is set, the provider will move files deleted during change\r\n" + 
                        "                                         application to the recycle bin. If this value is not set, files will be\r\n" + 
                        "                                         permanently deleted. \r\n" +
                        "   RecyclePreviousFileOnUpdates        - If this value is set, the provider will move files overwritten during change\r\n" + 
                        "                                         application to the recycle bin. If this value is not set, files will be over-\r\n" + 
                        "                                         written in place and any data in the old file will be lost. \r\n"
                        );
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                if (args[0] == "-e" | args[0] == "--example")
                {
                    Console.Write(
                        "<?xml version=\"1.0\" encoding=\"IBM437\"?>" +
                        "< ArrayOfSyncObject xmlns: xsd = \"http://www.w3.org/2001/XMLSchema\" xmlns: xsi = \"http://www.w3.org/2001/XMLSchema-instance\" >\r\n" +
                        "  < SyncObject >\r\n" +
                        "    < name > TestSync </ name >\r\n" +
                        "    < direction > Upload </ direction >\r\n" +
                        "    < localProvider >\r\n" +
                        "      < path > C:\\Testumgebung\\SyncTarget2 </ path >\r\n" +
                        "      < syncFilter >\r\n" +
                        "        < AttributeExcludeMask />\r\n" +
                        "        < FileNameExcludes />\r\n" +
                        "        < FileNameIncludes />\r\n" +
                        "        < SubdirectoryExcludes />\r\n" +
                        "      </ syncFilter >\r\n" +
                        "      < syncOptions >\r\n" +
                        "        < None > false </ None >\r\n" +
                        "        < CompareFileStreams > false </ CompareFileStreams >\r\n" +
                        "        < ExplicitDetectChanges > false </ ExplicitDetectChanges >\r\n" +
                        "        < RecycleConflictLoserFiles > false </ RecycleConflictLoserFiles >\r\n" +
                        "        < RecycleDeletedFiles > false </ RecycleDeletedFiles >\r\n" +
                        "        < RecyclePreviousFileOnUpdates > false </ RecyclePreviousFileOnUpdates >\r\n" +
                        "      </ syncOptions >\r\n" +
                        "    </ localProvider >\r\n" +
                        "    < remoteProvider >\r\n" +
                        "      < path >\\\\data\\Teamwork\\Tewes\\cmdlet </ path >\r\n" +
                        "      < syncFilter >\r\n" +
                        "        < AttributeExcludeMask />\r\n" +
                        "        < FileNameExcludes />\r\n" +
                        "        < FileNameIncludes > \" *.* \" </ FileNameIncludes >\r\n" +
                        "        < SubdirectoryExcludes />\r\n" +
                        "      </ syncFilter >\r\n" +
                        "      < syncOptions >\r\n" +
                        "        < None > false </ None >\r\n" +
                        "        < CompareFileStreams > false </ CompareFileStreams >\r\n" +
                        "        < ExplicitDetectChanges > false </ ExplicitDetectChanges >\r\n" +
                        "        < RecycleConflictLoserFiles > false </ RecycleConflictLoserFiles >\r\n" +
                        "        < RecycleDeletedFiles > false </ RecycleDeletedFiles >\r\n" +
                        "        < RecyclePreviousFileOnUpdates > false </ RecyclePreviousFileOnUpdates >\r\n" +
                        "      </ syncOptions >\r\n" +
                        "    </ remoteProvider >\r\n" +
                        "  </ SyncObject >\r\n" +
                        "</ ArrayOfSyncObject > "
                        );
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            else if(args[0] != "-d" && args[0] != "--debug")
            {
                Console.WriteLine("invalid parameters");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                
            }

            string configFile;
            if (args[0] == "-d" || args[0] == "--debug")
            {
                configFile = args[1];
                debug = true;
            }
            else
                configFile = args[0];


            // Konfiguration einlesen
            List<SyncObject> syncObjects = new List<SyncObject>();
            XmlSerializer serializer = new XmlSerializer(syncObjects.GetType());
            using (Stream reader = new FileStream(configFile, FileMode.Open))
            {
                syncObjects = (List<SyncObject>)serializer.Deserialize(reader);
            }
            // eingelesen

            if(debug)
            {
                serializer.Serialize(Console.Out, syncObjects);
                Console.ReadKey();
            }

            //////////////////////////////
            // Auf gültige Werte prüfen //
            //////////////////////////////
            ///
            // Angegebene Verzeichnisse müssen existieren, Oder evtl. auch über xml explizit sagen dass Verzeichnis angelegt werden soll
            //
            ///////////////////////////////////
            // Auf gültige Werte prüfen ende //
            ///////////////////////////////////



            // Sync über für alle Objekte durchführen  
            foreach (SyncObject s in syncObjects)
            {
                SyncOrchestrator orchestrator = new SyncOrchestrator();
                FileSyncScopeFilter syncFilterLocal = new FileSyncScopeFilter();
                FileSyncScopeFilter syncFilterRemote = new FileSyncScopeFilter();

                FileSyncOptions syncOptionLocal = FileSyncOptions.None;
                FileSyncOptions syncOptionRemote = FileSyncOptions.None;
                //orchestrator.LocalProvider = new FileSyncProvider(s.localProvider.path, syncFilterLocal, syncOptionLocal);
                //orchestrator.RemoteProvider = new FileSyncProvider(s.remoteProvider.path, syncFilterRemote, syncOptionRemote);

                // Dirction festlegen
                switch (s.direction)
                {
                    case "Download":
                        orchestrator.Direction = SyncDirectionOrder.Download;
                        break;
                    case "DownloadAndUpload":
                        orchestrator.Direction = SyncDirectionOrder.DownloadAndUpload;
                        break;
                    case "Upload":
                        orchestrator.Direction = SyncDirectionOrder.Upload;
                        break;
                    case "UploadAndDownload":
                        orchestrator.Direction = SyncDirectionOrder.UploadAndDownload;
                        break;
                }

                // FileSyncScopeFilter Local
                if(s.localProvider.syncFilter.AttributeExcludeMask != null && s.localProvider.syncFilter.AttributeExcludeMask != "")
                {
                    List<string> attributeExcludeMaskList = new List<string>();
                    attributeExcludeMaskList = s.localProvider.syncFilter.AttributeExcludeMask.Split(',').ToList();
                    foreach (string st in attributeExcludeMaskList)
                    {
                        switch (st)
                        {
                            case "Archive":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.Archive;
                                break;
                            case "Compressed":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.Compressed;
                                break;
                            case "Device":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.Device;
                                break;
                            case "Directory":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.Directory;
                                break;
                            case "Encrypted":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.Encrypted;
                                break;
                            case "Hidden":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.Hidden;
                                break;
                            case "IntegrityStream":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.IntegrityStream;
                                break;
                            case "Normal":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.Normal;
                                break;
                            case "NoScrubData":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.NoScrubData;
                                break;
                            case "NotContentIndexed":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.NotContentIndexed;
                                break;
                            case "Offline":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.Offline;
                                break;
                            case "ReadOnly":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.ReadOnly;
                                break;
                            case "ReparsePoint":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.ReparsePoint;
                                break;
                            case "SparseFile":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.SparseFile;
                                break;
                            case "System":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.System;
                                break;
                            case "Temporary":
                                syncFilterLocal.AttributeExcludeMask |= FileAttributes.Temporary;
                                break;
                        }
                    }
                }

                if(s.localProvider.syncFilter.FileNameExcludes != null && s.localProvider.syncFilter.FileNameExcludes != "")
                {
                    List<string> fileNameExcludesList = new List<string>();
                    fileNameExcludesList = s.localProvider.syncFilter.FileNameExcludes.Split(',').ToList();
                    foreach(string st in fileNameExcludesList)
                    {
                        syncFilterLocal.FileNameExcludes.Add(st);
                    }
                }
                if(s.localProvider.syncFilter.FileNameIncludes != null && s.localProvider.syncFilter.FileNameIncludes != "")
                {
                    List<string> fileNameIncludesList = new List<string>();
                    fileNameIncludesList = s.localProvider.syncFilter.FileNameIncludes .Split(',').ToList();
                    foreach (string st in fileNameIncludesList)
                    {
                        syncFilterLocal.FileNameIncludes.Add(st);
                        Console.WriteLine();
                        Console.WriteLine(st);
                    }
                }
                if(s.localProvider.syncFilter.SubdirectoryExcludes != null && s.localProvider.syncFilter.SubdirectoryExcludes != "")
                {
                    List<string> subDirectoryExcludesList = new List<string>();
                    subDirectoryExcludesList = s.localProvider.syncFilter.SubdirectoryExcludes.Split(',').ToList();
                    foreach (string st in subDirectoryExcludesList)
                    {
                        syncFilterLocal.FileNameIncludes.Add(st);
                    }
                }


                // FileSyncScopeFilter Remote
                if (s.remoteProvider.syncFilter.AttributeExcludeMask != null && s.remoteProvider.syncFilter.AttributeExcludeMask != "")
                {
                    List<string> attributeExcludeMaskList = new List<string>();
                    attributeExcludeMaskList = s.remoteProvider.syncFilter.AttributeExcludeMask.Split(',').ToList();
                    foreach (string st in attributeExcludeMaskList)
                    {
                        switch (st)
                        {
                            case "Archive":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.Archive;
                                break;
                            case "Compressed":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.Compressed;
                                break;
                            case "Device":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.Device;
                                break;
                            case "Directory":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.Directory;
                                break;
                            case "Encrypted":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.Encrypted;
                                break;
                            case "Hidden":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.Hidden;
                                break;
                            case "IntegrityStream":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.IntegrityStream;
                                break;
                            case "Normal":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.Normal;
                                break;
                            case "NoScrubData":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.NoScrubData;
                                break;
                            case "NotContentIndexed":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.NotContentIndexed;
                                break;
                            case "Offline":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.Offline;
                                break;
                            case "ReadOnly":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.ReadOnly;
                                break;
                            case "ReparsePoint":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.ReparsePoint;
                                break;
                            case "SparseFile":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.SparseFile;
                                break;
                            case "System":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.System;
                                break;
                            case "Temporary":
                                syncFilterRemote.AttributeExcludeMask |= FileAttributes.Temporary;
                                break;
                        }
                    }
                }

                if (s.remoteProvider.syncFilter.FileNameExcludes != null && s.remoteProvider.syncFilter.FileNameExcludes != "")
                {
                    List<string> fileNameExcludesList = new List<string>();
                    fileNameExcludesList = s.remoteProvider.syncFilter.FileNameExcludes.Split(',').ToList();
                    foreach (string st in fileNameExcludesList)
                    {
                        syncFilterRemote.FileNameExcludes.Add(st);
                    }
                }
                if (s.remoteProvider.syncFilter.FileNameIncludes != null && s.remoteProvider.syncFilter.FileNameIncludes != "")
                {
                    List<string> fileNameIncludesList = new List<string>();
                    fileNameIncludesList = s.remoteProvider.syncFilter.FileNameIncludes.Split(',').ToList();
                    foreach (string st in fileNameIncludesList)
                    {
                        syncFilterRemote.FileNameIncludes.Add(st);
                    }
                }
                if (s.remoteProvider.syncFilter.SubdirectoryExcludes != null && s.remoteProvider.syncFilter.SubdirectoryExcludes != "")
                {
                    List<string> subDirectoryExcludesList = new List<string>();
                    subDirectoryExcludesList = s.remoteProvider.syncFilter.SubdirectoryExcludes.Split(',').ToList();
                    foreach (string st in subDirectoryExcludesList)
                    {
                        syncFilterRemote.FileNameIncludes.Add(st);
                    }
                }

                // SyncOptions local initialisieren
                if (s.localProvider.syncOptions.None)
                {
                    syncOptionLocal |= FileSyncOptions.None;
                }
                if (s.localProvider.syncOptions.CompareFileStreams)
                {
                    syncOptionLocal |= FileSyncOptions.CompareFileStreams;
                }
                if (s.localProvider.syncOptions.ExplicitDetectChanges)
                {
                    syncOptionLocal |= FileSyncOptions.ExplicitDetectChanges;
                }
                if (s.localProvider.syncOptions.RecycleConflictLoserFiles)
                {
                    syncOptionLocal |= FileSyncOptions.RecycleConflictLoserFiles;
                }
                if (s.localProvider.syncOptions.RecycleDeletedFiles)
                {
                    syncOptionLocal |= FileSyncOptions.RecycleDeletedFiles;
                }
                if (s.localProvider.syncOptions.RecyclePreviousFileOnUpdates)
                {
                    syncOptionLocal |= FileSyncOptions.RecyclePreviousFileOnUpdates;
                }

                // SyncOptions remote initialisieren
                if (s.remoteProvider.syncOptions.None)
                {
                    syncOptionRemote |= FileSyncOptions.None;
                }
                if (s.remoteProvider.syncOptions.CompareFileStreams)
                {
                    syncOptionRemote |= FileSyncOptions.CompareFileStreams;
                }
                if (s.remoteProvider.syncOptions.ExplicitDetectChanges)
                {
                    syncOptionRemote |= FileSyncOptions.ExplicitDetectChanges;
                }
                if (s.remoteProvider.syncOptions.RecycleConflictLoserFiles)
                {
                    syncOptionRemote |= FileSyncOptions.RecycleConflictLoserFiles;
                }
                if (s.remoteProvider.syncOptions.RecycleDeletedFiles)
                {
                    syncOptionRemote |= FileSyncOptions.RecycleDeletedFiles;
                }
                if (s.remoteProvider.syncOptions.RecyclePreviousFileOnUpdates)
                {
                    syncOptionRemote |= FileSyncOptions.RecyclePreviousFileOnUpdates;
                }


                // Kopieren des letzten Meta-Files falls vorhanden
                if (File.Exists(s.localProvider.path + "\\filesync-source.metadata"))
                    File.Copy(s.localProvider.path + "\\filesync-source.metadata", s.remoteProvider.path + "\\filesync.metadata", true);


                Console.WriteLine($"SyncObjectName : {s.name}");
                Console.WriteLine($"LocalProvider");
                Console.WriteLine($"  Path          : {s.localProvider.path}");
                Console.WriteLine($"  SyncFilter    : {s.localProvider.syncFilter}");
                Console.WriteLine($"  SyncOptions   : {s.localProvider.syncOptions}");
                Console.WriteLine($"RemoteProvider");
                Console.WriteLine($"  Path          : {s.remoteProvider.path}");
                Console.WriteLine($"  SyncFilter    : {s.remoteProvider.syncFilter}");
                Console.WriteLine($"  SyncOptions   : {s.remoteProvider.syncOptions}");
                Console.WriteLine($"Direction       : {s.direction}");
                Console.WriteLine("--------------------------------------------------");

                string msg;

                // FileSyncProvider kann erst nach setzen der syncFilter/syncOption erstellt werden
                orchestrator.LocalProvider = new FileSyncProvider(s.localProvider.path, syncFilterLocal, syncOptionLocal);
                orchestrator.RemoteProvider = new FileSyncProvider(s.remoteProvider.path, syncFilterRemote, syncOptionRemote);
                try
                {
                    // Synchronize data between the two providers.
                    SyncOperationStatistics stats = orchestrator.Synchronize();

                    // Display statistics for the synchronization operation.
                    msg = "Synchronization succeeded!\n\n" +
                        stats.DownloadChangesApplied + " download changes applied\n" +
                        stats.DownloadChangesFailed + " download changes failed\n" +
                        stats.UploadChangesApplied + " upload changes applied\n" +
                        stats.UploadChangesFailed + " upload changes failed";
                }
                catch (Exception ex)
                {
                    msg = "Synchronization failed! Here's why: \n\n" + ex.Message;
                }
                File.Copy(s.remoteProvider.path + "\\filesync.metadata", s.localProvider.path + "\\filesync-source.metadata", true);
                File.Delete(s.remoteProvider.path + "\\filesync.metadata");
                Console.WriteLine(msg, "Synchronization Results");
            }
        Console.ReadKey();
        }
    }
}