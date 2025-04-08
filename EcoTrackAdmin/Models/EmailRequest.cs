using System.ComponentModel.DataAnnotations;

namespace EcoTrackAdmin.Models
{
    public class EmailRequest
    {
        public string currentEmail { get; set; }
        public string newEmail { get; set; }
        public string userId { get; set; }
        [Key]
        public int requestid { get; set; }
    }
}
