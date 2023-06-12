using Core.DAL.Infrastructure;

namespace IPSTemplate.Dal.Models
{
    public class TEBookAuthor : Entity
    {
        public Guid AuthorID { get; set; }
        public virtual TEAuthor Author { get; set; }

        public Guid BookID { get; set; }
        public virtual TEBook Book { get; set; }
    }
}
