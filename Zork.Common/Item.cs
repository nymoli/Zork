namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string InventoryDescription { get; }

        public string Description { get; }

        public Item(string name, string inventoryDescription, string description)
        {
            Name = name;
            InventoryDescription = inventoryDescription;
            Description = description;
        }
    }
}