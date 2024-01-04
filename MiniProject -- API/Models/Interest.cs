namespace MiniProject____API.Models
{
    public class Interest
    {
        public string InterestId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }


        public virtual ICollection<Person> People { get; set; }
        public virtual ICollection<InterestLink> InterestLinks { get; set; }
    }
}
