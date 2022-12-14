using System;
using System.Linq;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        [JsonIgnore]
        public Player Player { get; }

        [JsonIgnore]
        public IInputService Input { get; private set; }

        [JsonIgnore]
        public IOutputService Output { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IInputService input, IOutputService output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));

            IsRunning = true;
            Input.InputReceived += OnInputReceived;
            Output.WriteLine("Welcome to Zork! Your objective is to find every item in this world.");
            Look();
            Output.WriteLine($"\n{Player.CurrentRoom}");
        }

        public void OnInputReceived(object sender, string inputString)
        {
            char separator = ' ';
            string[] commandTokens = inputString.Split(separator);

            string verb;
            string subject = null;
            if (commandTokens.Length == 0)
            {
                return;
            }
            else if (commandTokens.Length == 1)
            {
                verb = commandTokens[0];
            }
            else
            {
                verb = commandTokens[0];
                subject = commandTokens[1];
            }

            Room previousRoom = Player.CurrentRoom;
            Commands command = ToCommand(verb);
            switch (command)
            {
                case Commands.Quit:
                    IsRunning = false;
                    Output.WriteLine("Thank you for playing!");
                    break;

                case Commands.Look:
                    Look();
                    break;

                case Commands.North:
                case Commands.South:
                case Commands.East:
                case Commands.West:
                    Directions direction = (Directions)command;
                    Output.WriteLine(Player.Move(direction) ? $"You moved {direction}." : "The way is shut!");
                    break;

                case Commands.Take:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Take(subject);
                    }
                    break;

                case Commands.Drop:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Drop(subject);
                    }
                    break;

                case Commands.Inventory:
                    if (Player.Inventory.Count() == 0)
                    {
                        Output.WriteLine("You are empty handed.");
                    }
                    else
                    {
                        Output.WriteLine("You are carrying:");
                        foreach (Item item in Player.Inventory)
                        {
                            Output.WriteLine(item.InventoryDescription);
                        }
                    }
                    break;
              
                case Commands.Score:
                    Output.WriteLine($"Your score would be {Player.Score}.");
                    break;

                case Commands.Moves:
                    Output.WriteLine($"You have made {Player.Moves} moves.");
                    Output.WriteLine($"You have {Player.MovesLeft} moves remaining.");
                    break;
                
                case Commands.Restart:
                    Output.WriteLine("Are you sure you wish to restart? Yes/No");
                    if (verb == "Yes")
                    {
                        Restart();
                    }
                    else if (verb == "No")
                    {
                        Output.WriteLine($"\n{Player.CurrentRoom}");
                    }
                    else
                    {
                        Output.WriteLine($"Are you sure you wish to restart? Yes/No\n{Player.CurrentRoom}");
                    }
                    break;

                default:
                    Output.WriteLine("Unknown command.");
                    break;
            }

            if (command != Commands.Unknown)
            {
                Player.Moves++;
                Player.MovesLeft--;
            }

            if (Player.MovesLeft == 0)
            {
                Output.WriteLine("Game Over. Please restart to try again.");
            }

            if (ReferenceEquals(previousRoom, Player.CurrentRoom) == false)
            {
                Look();
            }

            Output.WriteLine($"\n{Player.CurrentRoom}");

            if (Player.Score == 51 && Player.MovesLeft >= 0)
            {
                Output.WriteLine("You found every item! You win! Thank you for playing!");
            }
        }
        
        private void Look()
        {
            Output.WriteLine(Player.CurrentRoom.Description);
            foreach (Item item in Player.CurrentRoom.Inventory)
            {
                Output.WriteLine(item.LookDescription);
            }
        }

        bool takenOnce = false;

        private void Take(string itemName)
        {           
            Item itemToTake = Player.CurrentRoom.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToTake == null)
            {
                Output.WriteLine("You can't see any such thing.");                
            }
            else
            {
                Player.AddItemToInventory(itemToTake);
                Player.CurrentRoom.RemoveItemFromInventory(itemToTake);
                Output.WriteLine("Taken.");
                if (takenOnce == false)
                {
                    Player.Score += itemToTake.Score;
                }
                else
                {
                    itemToTake.Score = 0;
                }                              
            }
        }

        private void Drop(string itemName)
        {
            Item itemToDrop = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToDrop == null)
            {
                Output.WriteLine("You can't see any such thing.");                
            }
            else
            {
                Player.CurrentRoom.AddItemToInventory(itemToDrop);
                Player.RemoveItemFromInventory(itemToDrop);
                Output.WriteLine("Dropped.");
                takenOnce = true;
            }
        }

        public void Restart()
        {
            Console.Clear();
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}