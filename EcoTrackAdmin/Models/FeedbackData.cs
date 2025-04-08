using EcoTrackAdmin.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
namespace EcoTrackAdmin.Models
{
    public class FeedbackData
    {
        public string Types { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }
        [Key]
        public int cid { get; set; }
    }
}
