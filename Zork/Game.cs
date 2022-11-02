using System;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        public Player Player { get; }

        public Item Item { get; }

        public IOutputService Output { get; private set; }

        public Game(World world, string startingLocation, Item item)
        {
            World = world;
            Player = new Player(World, startingLocation);
            Item = item;
        }

        public void Run(IOutputService output)
        {
            Output = output ?? throw new ArgumentNullException(nameof(output));

            Room previousRoom = null;
            bool isRunning = true;
            while (isRunning)
            {
                Output.WriteLine(Player.CurrentRoom);
                if (previousRoom != Player.CurrentRoom)
                {
                    Output.WriteLine(Player.CurrentRoom.Description);
                    previousRoom = Player.CurrentRoom;
                }

                Output.Write("> ");

                string inputString = Console.ReadLine().Trim();
                char separator = ' ';
                string[] commandTokens = inputString.Split(separator);

                string commandInput = null;
                string itemInput = null;
                if (commandTokens.Length == 0)
                {
                    continue;
                }
                else if (commandTokens.Length == 1)
                {
                    commandInput = commandTokens[0];

                }
                else
                {
                    commandInput = commandTokens[0];
                    itemInput = commandTokens[1];
                }

                Commands command = ToCommand(commandInput);
                string outputString;
                switch (command)
                {
                    case Commands.QUIT:
                        isRunning = false;
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.LOOK:
                        outputString = Player.CurrentRoom.Description;
                        foreach (Item item in Player.CurrentRoom.Inventory)
                        {
                            outputString = Item.LookDescription;
                        }
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        Directions direction = (Directions)command;
                        if (Player.Move(direction))
                        {
                            outputString = $"You moved {direction}.";
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                        break;

                    case Commands.TAKE:
                        if (string.IsNullOrEmpty(itemInput))
                        {
                            outputString = "This command requires a subject.\n";
                        }
                        else
                        {
                            Take(itemInput);
                            outputString = $"{itemInput} taken.";
                        }
                        break;

                    case Commands.DROP:
                        if (string.IsNullOrEmpty(itemInput))
                        {
                            outputString = "This command requires a subject.\n";
                        }
                        else
                        {
                            Drop(itemInput);
                            outputString = $"{itemInput} dropped.";
                        }                        
                        break;

                    case Commands.INVENTORY:
                        
                        outputString = null;
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Output.WriteLine(outputString);
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }

    
}