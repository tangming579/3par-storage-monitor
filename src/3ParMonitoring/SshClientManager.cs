using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ParMonitoring
{
    public class SshClientManager
    {
        private string sshHost;
        private string sshUser;
        private string sshPassword;
        private uint sshPort = 22;

        public SshClientManager(string sshHost, string sshUser, string sshPassword)
        {
            this.sshHost = sshHost;
            this.sshUser = sshUser;
            this.sshPassword = sshPassword;
        }
        public SshClientManager(string sshHost, string sshUser, string sshPassword, uint sshPort)
        {
            this.sshHost = sshHost;
            this.sshUser = sshUser;
            this.sshPassword = sshPassword;
            this.sshPort = sshPort;
        }
    }
}
