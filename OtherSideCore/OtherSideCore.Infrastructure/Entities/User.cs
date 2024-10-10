using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtherSideCore.Infrastructure.Entities
{
   [Table("Users")]
   public class User : EntityBase
   {
      public bool IsActive { get; set; }

      [Required]
      [StringLength(50)]
      public string FirstName { get; set; }

      [Required]
      [StringLength(50)]
      public string LastName { get; set; }

      [Required]
      [StringLength(50)]
      public string UserName { get; set; }

      [Required]
      [StringLength(64, MinimumLength = 64)]
      public string PasswordHash { get; set; }
   }
}
