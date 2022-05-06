using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorySync
{
    public class Provider
    {
        public string path;
        public SyncFilters syncFilter;
        public SyncOptions syncOptions;
    }

    public class SyncFilters
    {
        public string AttributeExcludeMask = "";
        public string FileNameExcludes = "";
        public string FileNameIncludes = "";
        public string SubdirectoryExcludes = "";
    }

    public class SyncOptions
    {
        public bool None;
        public bool CompareFileStreams;
        public bool ExplicitDetectChanges;
        public bool RecycleConflictLoserFiles;
        public bool RecycleDeletedFiles;
        public bool RecyclePreviousFileOnUpdates;
    }
}
