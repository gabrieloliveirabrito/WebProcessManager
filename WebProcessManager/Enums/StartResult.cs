using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProcessManager.Enums
{
    public enum StartResult
    {
        TagNotFound,
        Started,
        AlreadyRunning,
        Error,
        Unknown
    }
}