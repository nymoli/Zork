using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Zork.Common;

namespace Zork.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            const string defaultRoomsFilename = @"Content\Game.json";
            string gameFilename = (args.Length > 0 ? args[(int)CommandLineArguments.GameFilename] : defaultRoomsFilename);
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(gameFilename));

            var input = new ConsoleInputService();
            var output = new ConsoleOutputService();

            Console.WriteLine("Welcome to Zork!");
            game.Run(output);
            Console.WriteLine("Finished.");
        }

        static void Take(string itemName)
        {
            foreach (Item item in _worldItems)
            {
                Item itemToTake = null;
                if (string.Compare(item.Name, itemName, ignoreCase: true) == 0)
                {
                    itemToTake = item;
                    break;
                }

                if(itemToTake == null)
                {
                    throw new ArgumentException("No such item exists.");
                }
            }
        }

        static void Drop(string itemName)
        {
            foreach (Item item in _worldItems)
            {
                Item itemToDrop = null;
                if (string.Compare(item.Name, itemName, ignoreCase: true) == 0)
                {
                    itemToDrop = item;
                    break;
                }

                if(itemToDrop == null)
                {
                    throw new ArgumentException("There are no items to drop.");
                }
            }
        }

        void AddToPlayersInventory(Item addItem)
        {

        }

        void RemoveFromPlayersInventory(Item removeItem)
        {

        }

        void AddItemToRoom(Item addItem)
        {

        }

        void RemoveItemFromRoom(Item removeItem)
        {

        }

        private static List<Item> _playerInventory;
        private static List<Item> _roomInventory;
        private static Item[] _worldItems;

        private enum CommandLineArguments
        {
            GameFilename = 0
        }
    }
}