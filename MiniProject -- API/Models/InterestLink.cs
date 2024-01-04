using System.ComponentModel.DataAnnotations.Schema;

namespace MiniProject____API.Models
{
    public class InterestLink
    {

        public string Id { get; set; }
        public string Url { get; set; }
        public string InterestId { get; set; }
        public Interest Interest { get; set; }
        public string PersonId { get; set; }
        public Person Person { get; set; }

    }
}
