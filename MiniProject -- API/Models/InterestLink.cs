using System.ComponentModel.DataAnnotations.Schema;

namespace MiniProject____API.Models
{
    public class InterestLink
    {

        public int Id { get; set; }
        public string Url { get; set; }
        public int InterestId { get; set; }
        public Interest Interest { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }

    }
}
