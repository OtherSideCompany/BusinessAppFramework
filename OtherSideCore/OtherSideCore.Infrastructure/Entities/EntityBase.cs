using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OtherSideCore.Infrastructure.Entities
{
   public abstract class EntityBase
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id { get; set; }
      public DateTime CreationDate { get; set; }
      public int? CreatedById { get; set; }
      public string? CreatedByName { get; set; }
      public DateTime LastModifiedDateTime { get; set; }
      public int? LastModifiedById { get; set; }
      public string? LastModifiedByName { get; set; }

   }
}
