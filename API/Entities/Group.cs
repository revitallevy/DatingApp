using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Group(string name)
    {
        [Key]
        public string Name { get; set; } = name;

        // Navigation property for related connections
        public ICollection<Connection> Connections { get; set; } = [];
    }
}
