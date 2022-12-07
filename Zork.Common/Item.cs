namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }

        public int Score { get; set; }

        public Item(string name, string lookDescription, string inventoryDescription, int score)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
            Score = score;
        }

        public override string ToString() => Name;
    }
}