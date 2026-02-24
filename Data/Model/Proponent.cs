namespace ProjectManager.Data.Model
{
    public class Proponent
    {
        public int ID { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public DateTime createdOn { get; set; } = DateTime.Now;
    }
}
