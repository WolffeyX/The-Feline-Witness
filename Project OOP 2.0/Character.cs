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
        private int hp;

        //Properties
        public string Name { get; set; }
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

        //Methods
        public void displayDialogue(string text, int speed, string textColor, string resetColor)
        {
            //Since text color is an ANSI escape code, that starts with \x1b, the Console.Write() method will not display the actual string itself, but instead will interpret it as an instruction to change the text color in the console. So, when we call Console.Write(textColor), it will change the color of the text that follows it in the console output to the color specified by the ANSI escape code in textColor.

            Console.WriteLine();
            Console.Write(textColor);
            // 1. CLEAR BUFFER: Remove every key input 
            //Console.KeyAvailable Property is used to get a value which shows whether a key press is available in the input stream.
            //Or in another word , it checks whether there are any key presses that have been made by the user but have not yet been read by the program. If there are key presses available in the input stream, it returns true; otherwise, it returns false.
            //Input stream is the buffer that holds the key presses until they are read by the program. When a key is pressed, it is stored in the input stream until the program reads it using Console.ReadKey() or similar methods. If there are any key presses in the input stream, Console.KeyAvailable will return true, indicating that there is a key press available to be read. If there are no key presses in the input stream, it will return false.
            //Console.ReadKey(true) method is used to READ A KEY PRESS IN THE INPUT STREAM. The true parameter indicates that the key press should not be displayed in the console. When this method is called, it will read the next key press from the input stream and return it as a ConsoleKeyInfo object. If there are no key presses available in the input stream, it will block until a key press is available.
            //This block of code is like this "read and discard the key input in the Input Stream while there is a key input in the InputStream"
            //This effectively clears the input buffer of any key presses that may have been made by the user before calling this method, ensuring that any subsequent key presses will be processed correctly without interference from previous inputs.
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            Console.Write($"{this.Name}: ");

            bool skipDelay = false;

            foreach (char c in text)
            {
                Console.Write(c);

                if (!skipDelay) //if skipDelay is false
                {
                    // 2. RADAR: If player press any key
                    if (Console.KeyAvailable)
                    {
                        skipDelay = true; // Cut the delay!
                        Console.ReadKey(true); // Discard the key press that was made by the user in ordere to skip the delay, so that it won't interfere with any subsequent key presses.
                    }
                    else
                    {
                        Thread.Sleep(speed);
                    }
                }
            }

            // Reset the console text color to default 
            Console.Write(resetColor);
            Console.WriteLine();
        }
    }

    internal class Cat : Character
    {
        public enum ActionType
        {
            Observe,
            Grab,
            Bury,
            Claw,
            Shove,
            Scatter
        }
        
        public enum FightingOptions
        {
            Claw,
            Kick,
            Bite,
            //Can add one more fighting option here if you want or change above options as well.
        }

        //private actionType catActionType;
        private FightingOptions catFightingOptions;

        //public actionType ActionType
        //{
        //    get { return catActionType; }
        //    set { catActionType = value; }
        //}

        public FightingOptions CatFightingOptions
        {
            get { return catFightingOptions; }
            set { catFightingOptions = value; }
        }




        //public StatusIndicator HealthStatus { get; set; }

        public Cat()
        {
            HP = 100;
        }
    }

    internal class MainCharacter : Cat
    {
        //Properties
        public HouseSpace currentLocation { get; set; }
        public SecondaryItem cucarryingItem { get; set; }

    }
}
