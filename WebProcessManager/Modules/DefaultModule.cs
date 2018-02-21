using System;
using System.Linq;
using System.Text;

using Nancy;

namespace WebProcessManager.Modules
{
    public class DefaultModule : NancyModule
    {
        public DefaultModule()
        {
            Get["/"] = HandleIndex;
        }

        object HandleIndex(dynamic p)
        {
            var c = "Projeto desenvolvido por Gabriel Oliveira Brito (tDarkFall)<br/>" + Environment.NewLine;
            c += $"Sistema com {Settings.Processes.Length} processos no arquivo de configuração<br/>" + Environment.NewLine;
            c += $"Para ver os processos, acesse {Settings.URL}list para ver os processos cadastrados (em formato JSON)<br/>" + Environment.NewLine;
            c += $"Para ver os processos, acesse {Settings.URL}list/system para ver os processos do sistema (em formato JSON)<br/>" + Environment.NewLine;
            c += $"Para ver o gerenciamento, acesse {Settings.URL}management, retorna uma enumeração em JSON<br/>" + Environment.NewLine;
            c += $"Para iniciar um processo, acesse {Settings.URL}management/start/{{TAG DO PROCESSO}}, retorna uma enumeração em JSON<br/>" + Environment.NewLine;
            c += $"Para fechar um processo, acesse {Settings.URL}management/stop/{{TAG DO PROCESSO}}, retorna uma enumeração em JSON<br/>" + Environment.NewLine;
            c += $"Para obter o estado um processo, acesse {Settings.URL}management/state/{{TAG DO PROCESSO}}, retorna uma enumeração em JSON<br/>" + Environment.NewLine;
            //c += $"Para ver os processos, acesse {Settings.URL}list para ver os processos cadastrados (em formato JSON)<br/>" + Environment.NewLine;

            return c;
        }
    }
}
