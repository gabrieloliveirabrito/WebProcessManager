using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace WebProcessManager
{
    public class Settings
    {
        public class Process
        {
            public string Tag { get; set; }
            public string Command { get; set; }
            public string Arguments { get; set; }
            public string WorkingDirectory { get; set; }
        }

        public static class Options
        {
            public static bool ListSystemProcesses { get; set; }
        }

        public static string URL { get; set; }
        public static Process[] Processes { get; set; }

        public static bool Load()
        {
            var err = string.Empty;
            var p = Path.Combine(Environment.CurrentDirectory, "settings.xml");

            if (!File.Exists(p))
            {
                err = "O arquivo settings.xml não pode ser encontrado!";
                goto error;
            }

            using (var str = File.OpenRead(p))
            {
                var doc = new XmlDocument();
                doc.Load(str);

                var urlNode = doc.DocumentElement.GetElementsByTagName("url");
                if (urlNode == null || urlNode.Count == 0)
                {
                    err = "Não foi possível encontrar o nó 'url' no arquivo de configuração!";
                    goto error;
                }
                URL = urlNode[0].InnerText;

                if(!URL.EndsWith("/"))
                {
                    err = "A URL do servidor deve terminar com \"/\" !";
                    goto error;
                }

                var optsNode = doc.DocumentElement.GetElementsByTagName("options");
                if (optsNode == null || optsNode.Count == 0)
                {
                    err = "Não foi possível encontrar o nó 'options' no arquivo de configuração!";
                    goto error;
                }
                else if (!HandleOptionsNode(optsNode[0], ref err))
                    goto error;

                var processesNode = doc.DocumentElement.GetElementsByTagName("processes");
                if(processesNode == null || processesNode.Count == 0)
                {
                    err = "Não foi possível encontrar o nó 'processes' no arquivo de configuração!";
                    goto error;
                }

                Processes = new Process[processesNode[0].ChildNodes.Count];
                var c = 0;

                foreach(XmlNode node in processesNode[0].ChildNodes)
                {
                    var process = HandleProcessNode(node, ref err);
                    if (process == null)
                        goto error;
                    else
                        Processes[c++] = process;
                }

                var pr = Processes.GroupBy(m => m.Tag).Select(m => m.First()).ToArray();
                if (Processes.Length != pr.Length)
                    Console.WriteLine("AVISO: Existem processos com tag idênticas no arquivo de configuração, o servidor irá ignorar tais processos!");
                Processes = pr;

                return true;
            }

            error:
            {
                Console.WriteLine("ERRO: {0}", err);
                Console.WriteLine("Falha ao iniciar o servidor!!");
                return false;
            }
        }

        private static bool HandleOptionsNode(XmlNode node, ref string err)
        {
            var lSPnode = node.SelectSingleNode("listSystemProcesses");
            if (lSPnode != null)
                Options.ListSystemProcesses = Convert.ToBoolean(lSPnode.InnerText);
            else
            {
                err = "O nó 'listSystemProcesses' não pode ser encontrado!";
                return false;
            }

            return true;
        }

        private static Process HandleProcessNode(XmlNode node, ref string err)
        {
            var tagAttr = node.Attributes["tag"];
            if(tagAttr == null)
            {
                err = "Um dos nós de processo não contém o atributo 'tag'!";
                return null;
            }

            var commandAttr = node.Attributes["command"];
            if (commandAttr == null)
            {
                err = "Um dos nós de processo não contém o atributo 'command'!";
                return null;
            }

            var proc = new Process();
            proc.Tag = tagAttr.Value;
            proc.Command = commandAttr.Value;

            var argsAttr = node.Attributes["arguments"];
            proc.Arguments = argsAttr == null ? string.Empty : argsAttr.Value;

            var workDir = node.Attributes["workDir"];
            proc.WorkingDirectory = workDir == null ? string.Empty : workDir.Value;

            return proc;
        }
    }
}