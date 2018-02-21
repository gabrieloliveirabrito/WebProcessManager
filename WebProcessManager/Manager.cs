using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WebProcessManager
{
    using Enums;

    public static class Manager
    {
        static Dictionary<string, Process> processes;

        static Manager()
        {
            processes = new Dictionary<string, Process>();
        }

        public static StartResult StartProcess(string tag)
        {
            try
            {
                var pr = Settings.Processes.FirstOrDefault(p => p.Tag == tag);
                if (pr == null)
                    return StartResult.TagNotFound;
                else
                {
                    Process p;
                    if (!processes.TryGetValue(tag, out p) || p == null || p.HasExited)
                    {
                        var i = new ProcessStartInfo
                        {
                            FileName = pr.Command,
                            Arguments = pr.Arguments,
                            WorkingDirectory = pr.WorkingDirectory,
                            UseShellExecute = true
                        };

                        p = Process.Start(i);
                        processes[tag] = p;

                        return StartResult.Started;
                    }
                    else
                        return StartResult.AlreadyRunning;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERRO: {0}", ex.Message);
                Console.WriteLine(ex.StackTrace);

                return StartResult.Error;
            }
        }

        public static StopResult StopProcess(string tag)
        {
            try
            {
                var pr = Settings.Processes.FirstOrDefault(p => p.Tag == tag);
                if (pr == null)
                    return StopResult.TagNotFound;
                else
                {
                    Process p;
                    if (processes.TryGetValue(tag, out p))
                    {
                        if (p == null || p.HasExited)
                            return StopResult.NotRunning;
                        else
                        {
                            p.Kill();
                            return StopResult.Stopped;
                        }
                    }
                    else
                        return StopResult.NotStarted;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: {0}", ex.Message);
                Console.WriteLine(ex.StackTrace);

                return StopResult.Error;
            }
        }

        public static ProcessState GetProcessState(string tag)
        {
            try
            {
                var pr = Settings.Processes.FirstOrDefault(p => p.Tag == tag);
                if (pr == null)
                    return ProcessState.TagNotFound;
                else
                {
                    Process p;
                    if (processes.TryGetValue(tag, out p))
                        if (p == null || p.HasExited)
                            return ProcessState.NotRunning;
                        else
                            return ProcessState.Running;
                    else
                        return ProcessState.NotStarted;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: {0}", ex.Message);
                Console.WriteLine(ex.StackTrace);

                return ProcessState.Error;
            }
        }

        public static bool GetProcessState(string tag, out Process process)
        {
            process = null;
            try
            {
                var pr = Settings.Processes.FirstOrDefault(p => p.Tag == tag);
                if (pr != null)
                {
                    Process p;
                    if (processes.TryGetValue(tag, out p))
                    {
                        process = p;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO: {0}", ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return false;
        }
    }
}