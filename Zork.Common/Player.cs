using System;
using System.Collections.Generic;

namespace Zork.Common
{
    public class Player
    {
        public event EventHandler<Room> LocationChanged;

        public event EventHandler<int> MovesChanged;

        public event EventHandler<int> MovesLeftChanged;

        public event EventHandler<int> ScoreChanged;
        
        public Room CurrentRoom
        {
            get => _currentRoom;
            set
            {
                if (_currentRoom != value)
                {
                    _currentRoom = value;
                    LocationChanged?.Invoke(this, _currentRoom);
                }
            }
        }

        public int Moves
        {
            get => _moves;
            
            set
            {
                if (_moves != value)
                {
                    _moves = value;
                    MovesChanged?.Invoke(this, _moves);
                }
            }
        }

        private int _moves;

        public int MovesLeft
        {
            get => _movesLeft;

            set
            {
                if (_movesLeft != value)
                {
                    _movesLeft = value;
                    MovesLeftChanged?.Invoke(this, _movesLeft);
                }
            }
        }

        private int _movesLeft = 35;

        public int Score
        {
            get => _score;
            
            set
            {
                if (_score != value)
                {
                    _score = value;
                    ScoreChanged?.Invoke(this, _score);
                }
            }
        }

        private int _score;

        public IEnumerable<Item> Inventory => _inventory;

        public Player(World world, string startingLocation)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            _inventory = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (didMove)
            {
                CurrentRoom = neighbor;
            }

            return didMove;
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            if (_inventory.Contains(itemToAdd))
            {
                throw new Exception($"Item {itemToAdd} already exists in inventory.");
            }

            _inventory.Add(itemToAdd);
        }

        public void RemoveItemFromInventory(Item itemToRemove)
        {
            if (_inventory.Remove(itemToRemove) == false)
            {
                throw new Exception("Could not remove item from inventory.");
            }
        }

        private readonly World _world;
        private Room _currentRoom;
        private readonly List<Item> _inventory;
    }
}
