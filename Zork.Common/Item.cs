namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }

        public int Points { get; }

        public Item(string name, string lookDescription, string inventoryDescription, int points)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
            Points = points;
        }

        public override string ToString() => Name;
    }
}