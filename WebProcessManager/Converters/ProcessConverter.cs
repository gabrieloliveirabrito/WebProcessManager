using System;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Newtonsoft.Json;

namespace WebProcessManager.Converters
{
    public class ProcessConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Process) == objectType;
        }

        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var p = (Process)value;

            writer.WriteStartObject();

            writer.WritePropertyName("ID");
            writer.WriteValue(p.Id);

            writer.WritePropertyName("ProcessName");
            writer.WriteValue(p.ProcessName);

            writer.WritePropertyName("MachineName");
            writer.WriteValue(p.MachineName);

            writer.WritePropertyName("StartTime");
            writer.WriteValue(p.StartTime);

            writer.WritePropertyName("MainWindowTitle");
            writer.WriteValue(p.MainWindowTitle);

            if(p.StartInfo != null)
            {
                writer.WritePropertyName("StartInfo");
                writer.WriteStartObject();

                writer.WritePropertyName("Filename");
                writer.WriteValue(p.StartInfo.FileName);

                writer.WritePropertyName("Arguments");
                writer.WriteValue(p.StartInfo.Arguments);

                writer.WritePropertyName("WorkingDirectory");
                writer.WriteValue(p.StartInfo.WorkingDirectory);

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }
}