using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project_OOP_2._0
{
    internal class Character
    {
        //OOP Concept applied: ENCAPSULATION (private fields, and public properties)
        // 1. Private Fields
        private string name;
        private int hp;

        // 2. Public Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int HP
        {
            get { return hp; }
            set
            {
                if (value > 100)
                {
                    hp = 100;
                }
                else if (value < 0)
                {
                    hp = 0;
                }
                else
                {
                    hp = value;
                }
            }
        }

        // 3. Methods
        public void displayDialogue(string text, int speed, string textColor, string resetColor)
        {
            Console.WriteLine();
            Console.Write(textColor);

            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            Console.Write($"{this.Name}: ");

            bool skipDelay = false;

            foreach (char c in text)
            {
                Console.Write(c);

                if (!skipDelay)
                {
                    if (Console.KeyAvailable)
                    {
                        skipDelay = true;
                        Console.ReadKey(true);
                    }
                    else
                    {
                        Thread.Sleep(speed);
                    }
                }
            }

            Console.Write(resetColor);
            Console.WriteLine();
        }
    }

    internal class Cat : Character //OOP Concept applied: INHERITANCE (Cat is a subclass of Character)
    {
        public enum ActionType
        {
            Observe, Grab, Bury, Claw, Shove, Scatter
        }

        public enum FightingOptions
        {
            Claw, Kick, Bite,
        }

        //OOP Concept applied: ENCAPSULATION (private fields, and public properties)

        // Private Fields
        private FightingOptions catFightingOptions;

        // Public Properties
        public FightingOptions CatFightingOptions
        {
            get { return catFightingOptions; }
            set { catFightingOptions = value; }
        }

        public Cat()
        {
            this.HP = 100;
        }
    }

    internal class MainCharacter : Cat //OOP Concept applied: INHERITANCE (MainCharacter is a subclass of Cat)
    {
        //OOP Concept applied: ENCAPSULATION (private fields, and public properties)
        // Private Fields
        private HouseSpace currentLocation;
        private SecondaryItem carryingItem; 

        // Public Properties
        public HouseSpace CurrentLocation
        {
            get { return currentLocation; }
            set { currentLocation = value; }
        }

        public SecondaryItem CarryingItem
        {
            get { return carryingItem; }
            set { carryingItem = value; }
        }
    }
}