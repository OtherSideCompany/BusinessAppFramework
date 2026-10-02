using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAppFramework.Infrastructure.Entities
{
    public class UserSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; } = default!;

        [Required]
        public string Values { get; set; } = "{}";
    }
}
