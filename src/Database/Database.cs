using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starcraft_BO_helper
{
    public class Database
    {

        // Template of a Singleton
        private static Database _instance;
        static readonly object instanceLock = new object();

        public static Database instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (instanceLock)
                    {
                        if (_instance == null)
                            _instance = new Database();
                    }
                }
                return _instance;
            }
        }
        // End template of a Singleton

        private Database()
        {

        }


    }

}
