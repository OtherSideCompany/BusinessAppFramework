using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAppFramework.Infrastructure.Entities
{
    /// <summary>
    /// One row per settings key. Values are stored as JSON so adding a property
    /// to a settings class, or a new settings class, needs no schema change.
    /// </summary>
    public class ModuleSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; } = default!;

        [Required]
        public string Values { get; set; } = "{}";
    }
}
