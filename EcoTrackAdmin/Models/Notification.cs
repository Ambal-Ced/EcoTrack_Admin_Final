using System.ComponentModel.DataAnnotations;

namespace EcoTrackAdmin.Models
{
    public class Notification
    {
        public string Message { get; set; }
        public string Nid { get; set; }
        [Key]
        public int Id { get; set; }
    }
}
