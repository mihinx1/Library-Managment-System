using Core.DAL.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.Dal.Models
{
    public class TEPublisher : Entity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Location { get; set; }

        [Required, MaxLength(50)]
        public string Email { get; set; }
    }
}
