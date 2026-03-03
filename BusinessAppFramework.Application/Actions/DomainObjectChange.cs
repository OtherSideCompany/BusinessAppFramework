namespace BusinessAppFramework.Application.Actions
{
   public class DomainObjectChange
   {
      public int DomainObjectId { get; set; }
      public ChangeType ChangeType { get; set; }
   }
}
