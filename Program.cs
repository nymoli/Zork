using System;

namespace Zork
{
    class Program
    {
        enum Commands
        {
            QUIT,
            LOOK,
            NORTH,
            SOUTH,
            EAST,
            WEST,
            UNKNOWN
        }

        private static string CurrentRoom
        {
            get
            {
                return Rooms[Location.Column];
            }
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            Commands command = Commands.UNKNOWN;                       
            while (command != Commands.QUIT)
            {
                Console.Write(CurrentRoom);
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                string outputString = null;
                switch (command)
                {
                    case Commands.QUIT:
                        outputString = "Thank you for playing.";
                        break;

                    case Commands.LOOK:
                        outputString = "This is an open field west of a white house, with a boarded front door. \nA rubber mat saying 'Welcome to Zork!' lies by the door.";
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if (Move(command) == false)
                        {
                            outputString = "The way is shut!";
                        }
                        break;

                    case Commands.UNKNOWN:
                        outputString = "Unknown command.";
                        break;

                }
                
                Console.WriteLine(outputString);
            }
           
        }

        private static bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid direction");

            bool isValidMove = true;
            switch (command)
            {
                case Commands.EAST when Location.Column < Rooms.GetLength(1) - 1:
                    Location.Column++;
                    break;

                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    break;

                default:
                    isValidMove = false;
                    break;
            }

            return isValidMove;
        }

        private static Commands ToCommand(string commandString) => (Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.UNKNOWN);

        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static readonly string[,] Rooms =
        {
            
            { "Forest", "West of House", "Behind House", "Clearing", "Canyon View" }
            
        };

        private static readonly List<Commands> Directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        private static (int Column) Location = (1);
    }
}


