using OtherSideCore.Adapter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtherSideCore.Infrastructure.Entities
{
   [Table("Users")]
   public class User : IEntity
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id { get; set; }

      public HistoryInfo HistoryInfo { get; set; } = new HistoryInfo();

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
