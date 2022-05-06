using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorySync
{
    public class SyncObject
    {
        public string name;
        public string direction;
        public Provider localProvider = new Provider();
        public Provider remoteProvider = new Provider();
    }
}
