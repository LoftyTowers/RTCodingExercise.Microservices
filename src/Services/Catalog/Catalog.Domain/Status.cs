namespace Catalog.Domain
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Plate> Plates { get; set; }
    }
}