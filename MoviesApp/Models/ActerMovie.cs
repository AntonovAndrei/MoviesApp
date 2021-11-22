using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.Models
{
    public class ActerMovie
    {
        public int ActerId { get; set; }
        public virtual Acter Acter { get; set; }
        
        
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }
    }
}