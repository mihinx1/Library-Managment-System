using Core.DAL.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.Dal.Models
{
    public class TEBook : Entity
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Language { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int TotalPages { get; set; }

        [Required]
        public Guid CategoryID { get; set; }
        public virtual TECategory Category { get; set; }

        //public Guid? PublisherID { get; set; }
        //public virtual TEPublisher Publisher { get; set; }

        //[Required]
        //[MaxLength(50)]
        //public string Genre { get; set; }

        //public Guid? AuthorID { get; set; }
        //public virtual TEAuthor Author { get; set; }
    }
}
