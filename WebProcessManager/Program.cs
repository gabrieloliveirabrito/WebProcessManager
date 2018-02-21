using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Nancy;
using Nancy.Hosting.Self;

namespace WebProcessManager
{
    class Program
    {
        static void Main(string[] args)
        {
            if(Settings.Load())
            {
                try
                {
                    var config = new HostConfiguration();
                    config.RewriteLocalhost = false;

                    var host = new NancyHost(config, new Uri(Settings.URL));
                    host.Start();

                    Console.WriteLine("Servidor iniciado com successo!");
                }
                catch(Exception ex)
                {
                    Console.WriteLine("ERRO: {0}", ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }

            Console.WriteLine("Pressione enter para finalizar o processo!");
            Console.ReadLine();
        }
    }
}
