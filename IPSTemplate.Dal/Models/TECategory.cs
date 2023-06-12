using Core.DAL.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.Dal.Models
{
    public class TECategory : Entity
    {
        [Required, MaxLength(20)]
        public string Name { get; set; }

        //[Required, MaxLength(7), MinLength(7)]
        [Required]
        public string Color { get; set; }
    }
}
