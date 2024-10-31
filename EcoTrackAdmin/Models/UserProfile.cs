using EcoTrackAdmin.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace EcoTrackAdmin.Models
{
    public class UserProfile
    {
        [Key]
        [Required]
        public string UniqueUserName { get; set; }

        // Foreign key reference to ApplicationUser
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
