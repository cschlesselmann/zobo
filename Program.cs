using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using zobo.Configuration.Input;
using zobo.Configuration.Output;

namespace zobo
{
    class Program
    {
        static void Main(string[] args)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy
                {
                    OverrideSpecifiedNames = false
                }
            };
            var converters = new List<JsonConverter> { new StringEnumConverter(), new TargetDefinitionConverter() };
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Converters = converters
            };

            Parser.Default.ParseArguments<CLIOptions>(args)
                   .WithParsed<CLIOptions>(options =>
                   {
                       var input = new StreamReader(options.ZoneFile);
                       string jsonString;
                       if (options.ZoneFile.EndsWith(".yaml") || options.ZoneFile.EndsWith(".yml"))
                       {
                           jsonString = ToJson(input.ReadToEnd());
                       }
                       else
                       {
                           jsonString = input.ReadToEnd();
                       }
                       var zoneConfig = JsonConvert.DeserializeObject<ZoneConfig>(jsonString, jsonSettings);

                       var vyosConfig = new VyosConfiguration(zoneConfig, options);
                       vyosConfig.PrintRules();
                   });

        }

        private static string ToJson(string yaml)
        {
            dynamic yamlObject = new DeserializerBuilder()
                      .WithNamingConvention(new UnderscoredNamingConvention())
                      .Build()
                      .Deserialize<dynamic>(yaml);

            return JsonConvert.SerializeObject(yamlObject);
        }
    }
}
