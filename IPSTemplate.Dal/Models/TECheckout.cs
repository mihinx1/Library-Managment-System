using Core.DAL.Infrastructure;
using IPSTemplate.Dal.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.Dal.Models
{
    public class TECheckout : Entity
    {
        //[Required]
        //public bool BookReturned { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? PlanedDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsReturned { get; set; }
        public string BookTitle { get; set; }

        public Guid UserID { get; set; }
        public virtual TEIdentityUser User { get; set; }
        //public Guid MemberID { get; set; }
        //public virtual TEMember Member { get; set; }

        public Guid BookCopyID { get; set; }
        public virtual TEBookCopy BookCopy { get; set; }
    }
}
