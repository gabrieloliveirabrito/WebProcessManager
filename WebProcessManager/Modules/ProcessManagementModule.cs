using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

using Nancy;

namespace WebProcessManager.Modules
{
    using Enums;
    public class ProcessManagementModule : NancyModule
    {
        public ProcessManagementModule() : base("/management")
        {
            Get["/"] = HandleIndex;
            Get["/start/{tag}"] = HandleStart;
            Get["/stop/{tag}"] = HandleStop;
            Get["/state/{tag}"] = HandleState;
        }

        object HandleIndex(dynamic p)
        {
            var dict = new Dictionary<string, object>();
            foreach(var process in Settings.Processes)
            {
                var state = Manager.GetProcessState(process.Tag);
                if (state == ProcessState.Running)
                {
                    Process pr;
                    if (Manager.GetProcessState(process.Tag, out pr))
                        dict[process.Tag] = pr;
                    else
                        dict[process.Tag] = "ERRO!";
                }
                else
                    dict[process.Tag] = state;
            }
            return Response.AsNJson(dict);
        }

        object HandleStart(dynamic p)
        {
            if (p.tag.HasValue)
                return Response.AsNJson(Manager.StartProcess(p.tag.Value as string));
            else
                return HttpStatusCode.NotFound;
        }

        object HandleStop(dynamic p)
        {
            if (p.tag.HasValue)
                return Response.AsNJson(Manager.StopProcess(p.tag.Value as string));
            else
                return HttpStatusCode.NotFound;
        }

        object HandleState(dynamic p)
        {
            if (p.tag.HasValue)
                return Response.AsNJson(Manager.GetProcessState(p.tag.Value as string));
            else
                return HttpStatusCode.NotFound;
        }
    }
}