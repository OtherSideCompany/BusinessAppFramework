
namespace OtherSideCore.Application.Services
{
   public interface IFileSerializationService
   {
      void SerializeToFile(string filePath, object objectToSerialize);
      object DeserializeFromFile(string filePath, Type type);
   }
}
