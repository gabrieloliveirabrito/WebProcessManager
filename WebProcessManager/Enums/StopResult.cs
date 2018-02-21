using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProcessManager.Enums
{
    public enum StopResult
    {
        Stopped,
        NotRunning,
        NotStarted,
        TagNotFound,
        Error,
        Unknown
    }
}