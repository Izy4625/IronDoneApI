namespace IronDomeApi.Models
{
    public class Attack
    {
        public Guid? Id { get; set; }
        public string Origin {  get; set; }
        public string type { get; set; }
        public string? Status { get; set; }
        public DateTime? launched {  get; set; }
    }
}
