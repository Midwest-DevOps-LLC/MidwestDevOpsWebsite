using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.Utility.Standard
{
    public static class ConnectionHandler
    {
        public static string connectionString
        {
            get; set;
        }

        public static string GetConnectionString()
        {
            return TextHasher.Decrypt(connectionString);
        }
    }
}
