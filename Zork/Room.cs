namespace Zork
{
    public class Room
    {
        public override string ToString() => Name;

        public string Name { get; }

        public string Description { get; set; }

        public Room(string name, string description = null)
        {
            Name = name;
            Description = description;
        }
    }
}