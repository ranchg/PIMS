using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSI.DataAccess
{
    public class DataFactory
    {
        private static SSI.DataAccess.Database db = null;
        private static readonly object locker = new object();

        public static IDatabase Database()
        {
            return Database("ConnectionString");
        }

        public static IDatabase Database(string connString)
        {
            if (db == null)
            {
                return (db = new SSI.DataAccess.Database(connString));
            }
            lock (locker)
            {
                return db;
            }
        }
    }
}
