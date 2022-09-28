using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.NoDepsUtils
{
    public class AvoidRunProgramTwice : IDisposable
    {
        Mutex mutex;
        public AvoidRunProgramTwice(string appGuid)
        {
            mutex = new Mutex(false, "Global\\" + appGuid);
        }

        public bool canRunApp()
        {
            if (!mutex.WaitOne(0, false))
                return false;
            else
                return true;
        }

        public void Dispose()
        {
            mutex?.Close();
            mutex = null;
        }
    }
}
