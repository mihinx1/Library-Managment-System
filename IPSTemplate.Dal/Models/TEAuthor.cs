using Core.DAL.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.Dal.Models
{
    public class TEAuthor : Entity
    {
        [Required, MaxLength(20)]
        public string FirstName { get; set; }

        [Required, MaxLength(20)]
        public string LastName { get; set; }

        public string Nationality { get; set; }

        public int BirthYear { get; set; }
    }
}
