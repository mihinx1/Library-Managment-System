using Core.DAL.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.Dal.Models
{
    public class TEBookCopy : Entity
    {
        public int PublishedYear { get; set; }

        [Required]
        public Guid BookID { get; set; }
        public virtual TEBook Book { get; set; }

        [Required]
        public Guid PublisherID { get; set; }
        public virtual TEPublisher Publisher { get; set; }

        public virtual ICollection<TECheckout> Checkouts { get; set; }
    }
}
