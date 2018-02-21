using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy;
using Nancy.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebProcessManager
{
    using Converters;

    public static class Extensions
    {
        static JsonSerializerSettings settings;

        static Extensions()
        {
            settings = new JsonSerializerSettings();
            settings.Converters = typeof(Extensions).Assembly.GetTypes().Where(t => typeof(JsonConverter).IsAssignableFrom(t)).Select(t => Activator.CreateInstance(t) as JsonConverter).ToArray();
            settings.Converters = settings.Converters.Concat(new[] { new StringEnumConverter() }).ToArray();
            settings.Formatting = Formatting.Indented;
        }

        public static Response AsNJson(this IResponseFormatter frmt, object o)
        {            
            var n = JsonConvert.SerializeObject(o, settings);
            return frmt.AsText(n);
        }
    }
}