using BusinessAppFramework.Application.Interfaces;
using System;
using System.IO;
using System.Text.Json;

namespace BusinessAppFramework.Infrastructure.FileSerialization
{
   public class JsonFileSerializationService : IFileSerializationService
   {
      public void SerializeToFile(string filePath, object objectToSerialize)
      {
         var options = new JsonSerializerOptions { WriteIndented = true };
         var json = JsonSerializer.Serialize(objectToSerialize, options);

         var directory = Path.GetDirectoryName(filePath);

         if (!Directory.Exists(directory))
         {
            Directory.CreateDirectory(directory);
         }

         File.WriteAllText(filePath, json);
      }

      public object DeserializeFromFile(string filePath, Type type)
      {
         if (File.Exists(filePath))
         {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize(json, type);
         }
         else
         {
            var defaultInstance = Activator.CreateInstance(type);
            SerializeToFile(filePath, defaultInstance);
            return defaultInstance;
         }
      }
   }
}
