namespace BusinessAppFramework.Domain
{
   public interface IIndexable
   {
      int Id { get; }
      int Index { get; set; }
   }
}
