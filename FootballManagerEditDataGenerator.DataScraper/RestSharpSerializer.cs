using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serialization;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballManagerEditDataGenerator.DataScraper
{
    internal class RestSharpSerializer : IRestSerializer
    {
        public string[] SupportedContentTypes => new string[] { "application/json" };

        public DataFormat DataFormat => DataFormat.Xml;

        public string ContentType { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>()
                {
                    new WikimediaJsonConverter()
                }
            });
        }

        public string Serialize(Parameter parameter)
        {
            return Serialize(parameter.Value);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }

    internal class WikimediaJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsClass && !objectType.IsPrimitive && objectType != typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            //var target = JsonConvert.DeserializeObject(jObject);
            var target = Activator.CreateInstance(objectType);

            if (jObject.ContainsKey("text") && jObject["text"] is JObject && ((JObject)jObject["text"]).ContainsKey("*"))
            {
                jObject["text"] = ((JObject)jObject["text"]).GetValue("*").ToString();
                //target = ((JObject)jObject["text"]).GetValue("*").ToString();
            }
            else if (jObject.ContainsKey("*"))
            {
                //target = jObject.GetValue("*").ToString();
            }

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Not intended to write");
        }

        public override bool CanWrite => false;
    }
}
