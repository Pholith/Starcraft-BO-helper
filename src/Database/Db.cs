using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Starcraft_BO_helper
{
    public class Db
    {

        // Template of a Singleton
        private static Db _instance;
        static readonly object instanceLock = new object();

        public static Db Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (instanceLock)
                    {
                        if (_instance == null)
                            _instance = new Db();
                    }
                }
                return _instance;
            }
        }
        // End template of a Singleton

        public List<Key> skipKey = new List<Key>();
        public bool onlySkipMode = false;


        private Db()
        {

        }

    }
}
