using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

using Nancy;

namespace WebProcessManager.Modules
{
    public class ProcessListModule : NancyModule
    {
        public ProcessListModule() : base("/list")
        {
            Get["/"] = HandleProcesses;

            if(Settings.Options.ListSystemProcesses)
                Get["/system"] = HandleSystemProcesses;
        }

        object HandleProcesses(dynamic p)
        {
            return Response.AsNJson(Settings.Processes);
        }

        object HandleSystemProcesses(dynamic p)
        {
            try
            {
                var pr = Process.GetProcesses(Environment.MachineName);
                return Response.AsNJson(pr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: {0}", ex.Message);
                Console.WriteLine(ex.StackTrace);

                return "Houve um erro na solicitação! Consulte o terminal para mais informações!";
            }
        }
    }
}