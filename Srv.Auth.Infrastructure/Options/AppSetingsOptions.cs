using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Srv.Auth.Repository.Options
{
    public class AppSetingsOptions
    {
        public string DbConnectionString { get; set; }
        public string MySqlVersion { get; set; }
    }
}
