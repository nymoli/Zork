using System;
using System.Collections.Generic;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        public Player Player { get; }
        
        public IOutputService Output { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);            
        }

        public bool Take(string itemName)
        {
            Item itemToTake = World.ItemsByName.GetValueOrDefault(itemName);
            
            if (Player.CurrentRoom.Inventory.Contains(itemToTake))
            {
                Player.Inventory.Add(itemToTake);
                Player.CurrentRoom.Inventory.Remove(itemToTake);
                return true;
            }
            else
            {
                return false;
            }
           
        }

        public bool Drop(string itemName)
        {
            Item itemToDrop = World.ItemsByName.GetValueOrDefault(itemName);

            if (Player.Inventory.Contains(itemToDrop))
            {
                Player.CurrentRoom.Inventory.Add(itemToDrop);
                Player.Inventory.Remove(itemToDrop);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Run(IOutputService output)
        {
            Output = output;

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
                char  separator = ' ';
                string[] commandTokens = inputString.Split(separator);
                
                string verb = null;
                string subject = null;
                if (commandTokens.Length == 0)
                {
                    continue;
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

                Commands command = ToCommand(verb);
                string outputString;
                switch (command)
                {
                    case Commands.QUIT:
                        isRunning = false;
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.LOOK:
                        outputString = $"{Player.CurrentRoom.Description}\n";
                        foreach (Item item in Player.CurrentRoom.Inventory)
                        {
                            outputString += $"{item.Description}\n";
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
                        if (subject != null)
                        {
                            if(Take(subject))
                            {
                                outputString = "Taken.";
                            }
                            else
                            {
                                outputString = "There is no such item.";
                            }
                        }
                        else
                        {
                            outputString = "There is no item to be taken.";                                                        
                        }                        
                        break;

                    case Commands.DROP:
                        if (subject != null)
                        {
                            if (Drop(subject))
                            {
                                outputString = "Dropped.";
                            }
                            else
                            {
                                outputString = "There is no such item.";
                            }
                        }
                        else
                        {
                            outputString = "There is no item to be dropped.";
                        }
                        
                        break;

                    case Commands.INVENTORY:
                        if (Player.Inventory.Count >= 1)
                        {
                            outputString = null;
                            
                            foreach(Item item in Player.Inventory)
                            {
                                outputString += $"{item.Description}\n";
                            }
                        }
                        else
                        {
                            outputString = "Inventory is empty.";
                        }                                                
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }

                if (outputString != null)
                {
                    Output.WriteLine(outputString);
                }
                               
                
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }
}
