using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP_2._0
{
    internal class StatusIndicator
    {
        // Property to save the health points (HP) of the character
        public int HP { get; set; }

        // Constructor to initialize the HP when creating a new StatusIndicator object
        public StatusIndicator(int startingHP)
        {
            HP = startingHP;
        }

        // Method to show the current health status of the character
        public void showHealth(string characterName)
        {
            Console.WriteLine($"{characterName}'s HP: {this.HP}/100");
        }
    }
}
