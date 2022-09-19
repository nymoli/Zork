namespace Zork
{
    public class Room
    {
        private string mName;
        public string Name
        {
            get
            {
                return mName;
            }
        }

        public override string ToString() => Name;

        private string mDescription;
        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mName = value;
            }
        }

        public Room(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}