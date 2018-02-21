using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace WebProcessManager
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.AfterRequest += HandleCharset;
            pipelines.OnError.AddItemToStartOfPipeline(HandleException);
        }

        private Task HandleCharset(NancyContext ctx, CancellationToken c)
        {
            if (ctx.Response.ContentType.StartsWith("text"))
                ctx.Response.ContentType += "; charset=utf-8";
            return Task.FromResult(0);
        }

        private dynamic HandleException(NancyContext ctx, Exception err)
        {
            Console.WriteLine("ERRO: {0}", err.Message);
            Console.WriteLine(err.StackTrace);
            return "Houve um erro na solicitação! Consulte o terminal para mais informações!";
        }
    }
}