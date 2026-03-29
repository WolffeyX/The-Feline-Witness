using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Project_OOP_2._0.Cat;

namespace Project_OOP_2._0
{
    internal abstract class Scene
    {
        //Properties
        public string Name { get; set; }
        public string resetColorField = "\x1b[0m"; //ASNI code
        public bool ValidateActionResult { get; set; } //This property is used to store the result of the action that the player has taken, whether it is valid or not. This will be used in the PlayerMovementLoop() method to determine whether the player can move to the next scene or not.
        //Methods
        public void delayedText(string text, int speed, string textColor, string resetColor)
        {
            //Since text color is an ANSI escape code, that starts with \x1b, the Console.Write() method will not display the actual string itself, but instead will interpret it as an instruction to change the text color in the console. So, when we call Console.Write(textColor), it will change the color of the text that follows it in the console output to the color specified by the ANSI escape code in textColor.
            Console.Write(textColor);

            // 1. CLEAR BUFFER: Remove every key input 
            //Console.KeyAvailable Property is used to get a value which shows whether a key press is available in the input stream.
            //Or in another word , it checks whether there are any key presses that have been made by the user but have not yet been read by the program. If there are key presses available in the input stream, it returns true; otherwise, it returns false.
            //Input stream is the buffer that holds the key presses until they are read by the program. When a key is pressed, it is stored in the input stream until the program reads it using Console.ReadKey() or similar methods. If there are any key presses in the input stream, Console.KeyAvailable will return true, indicating that there is a key press available to be read. If there are no key presses in the input stream, it will return false.
            //Console.ReadKey(true) method is used to READ A KEY PRESS IN THE INPUT STREAM. The true parameter indicates that the key press should not be displayed in the console. When this method is called, it will read the next key press from the input stream and return it as a ConsoleKeyInfo object. If there are no key presses available in the input stream, it will block until a key press is available.
            //This block of code is like this "read and discard the key input in the Input Stream while there is a key input in the InputStram"
            //This effectively clears the input buffer of any key presses that may have been made by the user before calling this method, ensuring that any subsequent key presses will be processed correctly without interference from previous inputs.
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

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

        //POLYMORPHISM: Method Overloading
        public void delayedText(string text, int speed, string textColor, string resetColor, bool newLine)
        {
            Console.Write(textColor);
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

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
            if (newLine)
            {
                Console.WriteLine();
            }
        }

        public void getName(Character charac, string objectName)
        { 
            bool isNameValid = false;
            do
            {
                delayedText($"Enter the {objectName}'s name: ", 50, resetColorField, resetColorField, false);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    charac.Name = input;
                    isNameValid = true;
                }
                else
                {
                    Console.WriteLine("Name cannot be empty. Please enter a valid name.");
                }
            } while (isNameValid == false);
        }


        public void displayItemsAvailable(HouseSpace givenHouseSpace)
        {
            for (int i = 0; i < givenHouseSpace.itemsAvailable.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {givenHouseSpace.itemsAvailable[i].Name}");
            }
        }

        public void goTo(HouseSpace houseSpace)
        {
            delayedText($"Going to {houseSpace.Name} .....", 50, resetColorField, resetColorField);
            Console.WriteLine($"Available items/furnitures in {houseSpace.Name}:\n");
            displayItemsAvailable(houseSpace);
        }

        public virtual void exploreHouse(GameEngine engine)
        {
            Console.WriteLine($"\n[Current Location: {engine.mainCharacterCat.currentLocation?.Name ?? "Not set"}]");
            Console.WriteLine();
            Console.Write("Press 'M' to display the house map, 'E' to identify available primary(main) items in the current location, and 'C' to go to another location: ");
            try //if the user input is not M, E, or C, throw an exception and catch it in the catch block, then prompt the user to try again
            {
                char input = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();

                if (input == 'M')
                {
                    Console.WriteLine("===========================================================================");
                    Console.WriteLine("\nHOUSE MAP:");
                    engine.house.displayMap();
                    Console.WriteLine("===========================================================================");
                    Console.WriteLine("\n");
                }
                else if (input == 'C')
                {
                    Console.WriteLine("\n");
                    Console.WriteLine($"[Current Location: {engine.mainCharacterCat.currentLocation.Name}]\n");
                    Console.WriteLine($"Available locations (rooms/space) in the house:\n");
                    for (int i = 0; i < engine.HouseSpaceList.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {engine.HouseSpaceList[i].Name}");
                    }
                    Console.WriteLine();
                    Console.Write("Select room/space number to go to: ");

                    if (int.TryParse(Console.ReadLine(), out int roomChoice) && roomChoice >= 1 && roomChoice <= engine.HouseSpaceList.Count)
                    {
                        HouseSpace selectedRoom = engine.HouseSpaceList[roomChoice - 1];
                        goTo(selectedRoom);
                        engine.mainCharacterCat.currentLocation = selectedRoom; // Update current location after moving
                    }
                    else
                    {
                        Console.WriteLine("Invalid room selection.");
                    }
                }
                else if (input == 'E')
                {
                    // Logic senaraikan PrimaryItem -> Pilih Secondary Item -> Pilih Action
                    // Jika user pilih SecondaryItem, panggil method interface tu:
                    //selectedSecondaryItem.Interact(selectedAction, this);

                    // === LOGIK EXPLORE BILIK (CARI BARANG & BUAT ACTION) ===
                    var currentRoom = engine.mainCharacterCat.currentLocation;
                    if (currentRoom == null || currentRoom.itemsAvailable.Count == 0)
                    {
                        Console.WriteLine("There is nothing to explore here.");
                        return;
                    }

                    // 1. Listing down all primary items in the current location
                    Console.WriteLine($"\n");
                    Console.WriteLine($"Available items/furnitures in {currentRoom.Name}:\n");
                    displayItemsAvailable(currentRoom);
                    Console.Write("Select item number to inspect: ");

                    if (int.TryParse(Console.ReadLine(), out int pItemChoice) && pItemChoice >= 1 && pItemChoice <= currentRoom.itemsAvailable.Count)
                    //int.TryParse(string input, out int result)
                    //This method tries to convert the string input (first parameter) into an integer.
                    //If it succeeds, it returns true and assigns the converted integer to the second parameter. 
                    //out int pItemChoice : Create an int variable named pItemChoice, and
                    //assign the value that has been parsed from the user input.
                    {
                        // Kita kena cast sebagai PrimaryItem untuk akses list SecondaryItem di dalamnya
                        var selectedPrimary = currentRoom.itemsAvailable[pItemChoice - 1] as PrimaryItem;
                        delayedText($"Selected Item: {selectedPrimary.Name} ", 30, resetColorField, resetColorField);
                        delayedText($"Going to {selectedPrimary.Name} .....", 50, resetColorField, resetColorField);

                        if (selectedPrimary == null || selectedPrimary.AvailableSecondaryItem.Count == 0)
                        {
                            Console.WriteLine("Nothing to do here. (No usable items on/approximate to this item)");
                            return;
                        }

                        // 2. Senaraikan Secondary Item
                        Console.WriteLine($"\nItems found on/at {selectedPrimary.Name}:\n");
                        for (int i = 0; i < selectedPrimary.AvailableSecondaryItem.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {selectedPrimary.AvailableSecondaryItem[i].Name}");
                        }
                        Console.Write("Select item to interact with: ");

                        if (int.TryParse(Console.ReadLine(), out int sItemChoice) && sItemChoice >= 1 && sItemChoice <= selectedPrimary.AvailableSecondaryItem.Count)
                        {
                            var selectedSecondary = selectedPrimary.AvailableSecondaryItem[sItemChoice - 1];
                            delayedText($"Selected Item: {selectedSecondary.Name} ", 30, resetColorField, resetColorField);

                            // 3. Senaraikan Action yang wujud dalam Enum ActionType
                            Console.WriteLine($"\nWhat do you want to do with {selectedSecondary.Name}?\n");
                            var actions = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();
                            //Enum.GetValues(typeof(ActionType)) : This method retrieves an array of the values of the constants in the specified enumeration (ActionType).
                            //Cast<ActionType>() : After getting the values in the array, that are casted to ActionType
                            //ToList() : The casted values are then converted into a List<ActionType> for easier manipulation.
                            //
                            for (int i = 0; i < actions.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {actions[i]}");
                            }
                            Console.Write("Select action number: ");

                            if (int.TryParse(Console.ReadLine(), out int actionChoice) && actionChoice >= 1 && actionChoice <= actions.Count)
                            {
                                ActionType selectedAction = actions[actionChoice - 1];
                                ValidateActionResult = validateAction(selectedSecondary, selectedAction);
                                if ( ValidateActionResult == false)
                                {
                                    Console.WriteLine("Invalid action / Nothing to do here");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid action selection.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid secondary item selection.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid item selection.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Invalid input!!!");
                }
            }
            catch (Exception ex) //Exception Handling
            {
                Console.WriteLine($"Error: {ex.Message}. Please try again");
            }
        }

        public virtual void playScene(GameEngine engine)
        {
            // This method will be overridden by the child class to play the scene
        }

        public virtual bool validateAction(SecondaryItem item, ActionType action) { 
            return false; //Please overrride this
        }
    }

    internal class IntroScene : Scene
    {
        //Constructor
        public IntroScene(string givenName)
        {
            Name = givenName;
        }


        public override void playScene(GameEngine engine)
        {
            string titleArt = @"
              _____ _           ___    _ _              __      ___ _                     
             |_   _| |_  ___   | __|__| (_)_ _  ___     \ \    / (_) |_ _ _  ___ ______   
               | | | ' \/ -_)  | _/ -_) | | ' \/ -_)     \ \/\/ /| |  _| ' \/ -_|_-<_-<   
               |_| |_||_\___|  |_|\___|_|_|_||_\___|      \_/\_/ |_|\__|_||_\___/__/__/   
            ";

            string catASCII = @"
                   ,_     _,
                   |\\___//|
                   |=Q   Q=|
                   \=._Y_.=/
                    )  `  (    ,
                   /       \  ((
                   |       |   ))
                  /| |   | |\_//
                  \| |._.| |/-`
                   '""'   '""'
             ";

            //---INTRO---
            //Creating colors
            string Tangerine = "\x1b[38;2;255;153;51m"; //ASNI
            string resetColor = "\x1b[0m";

            //Display the tile in ascii art...
            delayedText(titleArt, 10, Tangerine, resetColor);
            delayedText(catASCII, 10, Tangerine, resetColor);
            Console.WriteLine("\n=======================================================");
            Console.WriteLine("                  Press 'ENTER' to start...            ");
            Console.WriteLine("=======================================================\n");

            Console.ReadLine();
            delayedText("Welcome to The Feline Witness :).", 50, resetColor, resetColor);
            Console.ReadLine();
            delayedText("This game is about a cat, that witnesses an event that changes it and its owner life, who is a woman, and a wife.", 50, resetColor, resetColor);
            Console.ReadLine();
            delayedText("First, let's give the cat a name.", 50, resetColor, resetColor);
            //delayedText("Enter the cat's name:", 50, resetColor, resetColor, false);
            //engine.mainCharacterCat.Name = Console.ReadLine();
            getName(engine.mainCharacterCat, "cat");
            delayedText("Perfect...", 50, resetColor, resetColor);
            Console.Clear();
        }
    }

    internal class Scene1 : Scene
    {
        //Constructor
        public Scene1(string givenName)
        {
            Name = givenName;
        }
        public override bool validateAction(SecondaryItem item, ActionType action)
        {
            bool isValid = false;
            if (item.Name == "Husband Smartphone" && action == ActionType.Observe)
            {
                isValid = true;
            }

            return isValid;
        }

        public override void playScene(GameEngine engine)
        {
            
            string scene1 = @"
             =======================================================
                            SCENE 1: SUSPICIOUS
             =======================================================
            ";

            engine.mainCharacterCat.currentLocation = engine.HouseSpaceList[5]; //Set the current location of the cat to the first room in the house, which is the living room

            delayedText(scene1, 10, resetColorField, resetColorField);
            delayedText($"Meet {engine.mainCharacterCat.Name}. {engine.mainCharacterCat.Name} was a cat owned by a husband and wife who lived happily in a nice house", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText("Let's give the husband a name.", 50, resetColorField, resetColorField);
            //delayedText("Enter the husband's name: ", 50, resetColorField, resetColorField, false);
            //engine.husband.Name = Console.ReadLine();
            getName(engine.husband, "husband");
            delayedText($"Perfect..., the husband's name now is {engine.husband.Name}. Now let's give the wife a name.", 50, resetColorField, resetColorField);
            //delayedText("Enter the wife's name: ", 50, resetColorField, resetColorField, false);
            //engine.wife.Name = Console.ReadLine();
            getName(engine.wife, "wife");
            delayedText($"Great..., the wife's name now is {engine.wife.Name}.", 50, resetColorField, resetColorField);
            Console.ReadLine();
            Console.Clear();

            delayedText($"One day, {engine.wife.Name} had to go out of town for work (outstation). " +
                $"According to {engine.husband.Name}, he also had an outstation trip and had to leave the house that day as well. " +
                $"However, {engine.wife.Name} had to leave first", 50, resetColorField, resetColorField);

            engine.wife.displayDialogue($"\"Bye bye darling. Love you.\"", 50, resetColorField, resetColorField);
            engine.husband.displayDialogue($"\"Bye honey. Love you too.\"", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText($"{engine.wife.Name} then left the house...", 50, resetColorField, resetColorField);
            Console.Clear();
            delayedText($"However, upon closer observation... even though {engine.husband.Name} claimed he had work to do out of town, he showed no signs of leaving. ", 50, resetColorField, resetColorField);
            delayedText("Instead, he was quite relaxed that day, constantly using his smartphone, texting someone.", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText($"Occasionally, he would chuckle while typing. " +
                $"The clock struck 9:00 AM, but there were no signs that the husband was going anywhere.", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText($"He was still texting to someone on a sofa in the living room, while {engine.mainCharacterCat.Name} was observing him from its position. ", 50, resetColorField, resetColorField);
            delayedText($"{engine.mainCharacterCat.Name} started to feel curious. What was {engine.husband.Name} actually doing?", 50, resetColorField, resetColorField);
            Console.ReadLine();
            Console.Clear();

            delayedText($"{engine.mainCharacterCat.Name} decided to observe {engine.husband.Name}'s phone. It wanted to do this by approaching him on a sofa and then sitting on his lap.", 50, resetColorField, resetColorField);
            Console.ReadLine();
            Console.WriteLine("\n===========================================================================");
            Console.WriteLine($"MISSION: {engine.mainCharacterCat.Name} wanted to investigate the husband.");
            Console.WriteLine($"Find where {engine.husband.Name} sat, and observe his phone");
            Console.WriteLine("===========================================================================\n");

            do
            {
                exploreHouse(engine);
            } while (ValidateActionResult == false);

            ValidateActionResult = false; //reset the ValidateActionResult for the next use in this scene
            Console.Clear();
            delayedText($"There was a woman's name displayed on the husband's phone screen, and {engine.husband.Name} was texting to that person. ", 50, resetColorField, resetColorField);
            delayedText($"Give this woman's name a name.", 50, resetColorField, resetColorField);
            //Console.Write("Enter the name: ");
            //engine.mistress.Name = Console.ReadLine();
            getName(engine.mistress, "mistress");
            Console.WriteLine();
            Console.WriteLine("=======================================================");
            Console.WriteLine($"ONLINE CHAT, (By {engine.mainCharacterCat.Name}'s perspective)");
            Console.WriteLine("=======================================================\n");
            engine.husband.displayDialogue($"\"Babe, my wife isn't home this time, outstation. Want to meet up?\"", 50, resetColorField, resetColorField);
            Console.ReadLine();
            engine.mistress.displayDialogue($"\"Oh really? Yayyy! Where do you want to meet? The cafe we always go to?\"", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText($"{engine.mainCharacterCat.Name} was puzzled. Who is \"{engine.mistress.Name}\"? And what was {engine.husband.Name} doing?", 50, resetColorField, resetColorField);
            Console.ReadLine();
            engine.husband.displayDialogue($"\"I'm ok with that...\"", 50, resetColorField, resetColorField);
            Console.ReadLine();
            engine.husband.displayDialogue($"\"Yes, seriously, I can bake. And I have an idea. Instead of us going to the cafe, how about I bake a white chocolate macadamia cake for you, " +
                $"and you come over to eat at my place? You’ve never been to my house, right?\"", 50, resetColorField, resetColorField);
            Console.ReadLine();
            engine.mistress.displayDialogue($"\"Wait, are you serious? Inviting me to the house? What if your wife finds out?\"", 50, resetColorField, resetColorField);
            engine.husband.displayDialogue($"\"Relax babe. Do you want it or not? I honestly want to bake for you... let me bake this morning, you come over in the evening, have some cake, " +
                $"and then perhaps we can watch Netflix together...\"", 50, resetColorField, resetColorField);
            engine.husband.displayDialogue($"\"I really wanna see you\"", 50, resetColorField, resetColorField);
            Console.ReadLine();
           engine.mistress.displayDialogue($"\"Umm, yeah, sounds interesting. Is it the same location you shared before? What time can I come?\"", 50, resetColorField, resetColorField);
            engine.husband.displayDialogue($"\"Yes same location. Is 3 PM okay?\"", 50, resetColorField, resetColorField);
            Console.ReadLine();
            engine.mistress.displayDialogue($"\"3 PM is perfect. See you there.. ;)\"", 50, resetColorField, resetColorField);
            Console.ReadLine();
            Console.Clear();
            delayedText($".....", 70, resetColorField, resetColorField);
            delayedText($"{engine.mainCharacterCat.Name} was stunned. It had to process several shocking facts:", 50, resetColorField, resetColorField);
            delayedText($"1. {engine.husband.Name} was cheating; he has another woman named \"{engine.mistress.Name}\".", 50, resetColorField, resetColorField);
            delayedText($"2. {engine.husband.Name} seems to have had a secret relationship with {engine.mistress.Name} for a while (based on the phrase \"the cafe we always go to\").", 50, resetColorField, resetColorField);
            delayedText($"3. {engine.wife.Name} ({engine.husband.Name}'s wife), knew nothing about this.", 50, resetColorField, resetColorField);
            delayedText($"4. {engine.husband.Name} was planning to meet up with {engine.mistress.Name} IN THIS HOUSE", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText($"End of Scene 1...", 50, resetColorField, resetColorField);
        }

    }

    internal class Scene3 : Scene
    {
        //Constructor
        public Scene3(string givenName)
        {
            Name = givenName;
        }

        public override bool validateAction(SecondaryItem item, ActionType action)
        {
            // 1. Check for the SUCCESS (Claw + Sack Opening)
            if (item.Name == "Sack Opening" && action == ActionType.Claw)
            {
                return true; // This will trigger the animation in playScene
            }

            // 2. Handle the "Wrong" items using their SECONDARY names
            if (item.Name == "Tires")
            {
                Console.WriteLine("\nThe tires are too tough for my claws. I need something breakable.");
            }
            else if (item.Name == "Cage Door")
            {
                Console.WriteLine("\nI'm not locking myself in there!");
            }
            else if (item.Name == "Smelly Shoes")
            {
                Console.WriteLine("\n*Sniff sniff*... smelly shoes... but not a big enough mess to cancel a date.");
            }
            else if (item.Name == "Sack Opening" && action != ActionType.Claw)
            {
                Console.WriteLine($"\nSimply {action}-ing the bag won't work. You need to use your claws (Option 4)!");
            }
            else
            {
                Console.WriteLine($"\nInteracting with {item.Name} won't help. Keep searching the Garage!");
            }

            return false;
        }

        public override void exploreHouse(GameEngine engine)
        {
            // RESET the result at the start of every turn so previous 
            // actions don't interfere with the current choice.
            ValidateActionResult = false;

            Console.WriteLine($"\n[Current Location: {engine.mainCharacterCat.currentLocation?.Name ?? "Not set"}]");
            Console.WriteLine();
            Console.Write("Press 'M' to display the house map, 'E' to identify available primary(main) items in the current location, and 'C' to go to another location: ");

            try
            {
                char input = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();

                if (input == 'M')
                {
                    Console.WriteLine("===========================================================================");
                    Console.WriteLine("\nHOUSE MAP:");
                    engine.house.displayMap();
                    Console.WriteLine("===========================================================================");
                    Console.WriteLine("\n");
                }
                else if (input == 'C')
                {
                    Console.WriteLine("\n");
                    Console.WriteLine($"[Current Location: {engine.mainCharacterCat.currentLocation.Name}]\n");
                    Console.WriteLine($"Available locations (rooms/space) in the house:\n");
                    for (int i = 0; i < engine.HouseSpaceList.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {engine.HouseSpaceList[i].Name}");
                    }
                    Console.WriteLine();
                    Console.Write("Select room/space number to go to: ");

                    if (int.TryParse(Console.ReadLine(), out int roomChoice) && roomChoice >= 1 && roomChoice <= engine.HouseSpaceList.Count)
                    {
                        HouseSpace selectedRoom = engine.HouseSpaceList[roomChoice - 1];
                        goTo(selectedRoom);
                        engine.mainCharacterCat.currentLocation = selectedRoom;
                    }
                    else
                    {
                        Console.WriteLine("Invalid room selection.");
                    }
                }
                else if (input == 'E')
                {
                    var currentRoom = engine.mainCharacterCat.currentLocation;
                    if (currentRoom == null || currentRoom.itemsAvailable.Count == 0)
                    {
                        Console.WriteLine("There is nothing to explore here.");
                        return;
                    }

                    Console.WriteLine($"\n");
                    Console.WriteLine($"Available items/furnitures in {currentRoom.Name}:\n");
                    displayItemsAvailable(currentRoom);
                    Console.Write("Select item number to inspect: ");

                    if (int.TryParse(Console.ReadLine(), out int pItemChoice) && pItemChoice >= 1 && pItemChoice <= currentRoom.itemsAvailable.Count)
                    {
                        var selectedPrimary = currentRoom.itemsAvailable[pItemChoice - 1] as PrimaryItem;

                        // IMPORTANT FIX: Check if selectedPrimary is null after casting
                        if (selectedPrimary == null)
                        {
                            Console.WriteLine("This item cannot be inspected.");
                            return;
                        }

                        delayedText($"Selected Item: {selectedPrimary.Name} ", 30, resetColorField, resetColorField);
                        delayedText($"Going to {selectedPrimary.Name} .....", 50, resetColorField, resetColorField);

                        // This check is now safe because you added secondary items in GameEngine
                        if (selectedPrimary.AvailableSecondaryItem.Count == 0)
                        {
                            Console.WriteLine("Nothing to do here. (No usable items on/approximate to this item)");
                            return;
                        }

                        Console.WriteLine($"\nItems found on/at {selectedPrimary.Name}:\n");
                        for (int i = 0; i < selectedPrimary.AvailableSecondaryItem.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {selectedPrimary.AvailableSecondaryItem[i].Name}");
                        }
                        Console.Write("Select item to interact with: ");

                        if (int.TryParse(Console.ReadLine(), out int sItemChoice) && sItemChoice >= 1 && sItemChoice <= selectedPrimary.AvailableSecondaryItem.Count)
                        {
                            var selectedSecondary = selectedPrimary.AvailableSecondaryItem[sItemChoice - 1];
                            delayedText($"Selected Item: {selectedSecondary.Name} ", 30, resetColorField, resetColorField);

                            Console.WriteLine($"\nWhat do you want to do with {selectedSecondary.Name}?\n");
                            var actions = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();

                            for (int i = 0; i < actions.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {actions[i]}");
                            }
                            Console.Write("Select action number: ");

                            if (int.TryParse(Console.ReadLine(), out int actionChoice) && actionChoice >= 1 && actionChoice <= actions.Count)
                            {
                                ActionType selectedAction = actions[actionChoice - 1];

                                // This calls the validateAction you wrote at the top of Scene3
                                ValidateActionResult = validateAction(selectedSecondary, selectedAction);

                                if (ValidateActionResult == false)
                                {
                                    // If validateAction returned false, it means it wasn't the "winning" action
                                    // (But your funny messages inside validateAction will still have printed!)
                                    Console.WriteLine("\n[Action completed, but the mission continues...]");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid action selection.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid secondary item selection.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid item selection.");
                    }
                }
                else
                {
                    // Using a simple message instead of throwing an Exception to prevent the game from crashing
                    Console.WriteLine("Invalid input! Please press M, E, or C.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Please try again");
            }
        }

        public override void playScene(GameEngine engine)
        {
            string Tangerine = "\x1b[38;2;255;153;51m";
            string reset = "\x1b[0m";
            string header = @"
             =======================================================
                          SCENE 3: THE KIBBLE CHAOS
             =======================================================
            ";
            delayedText(header, 10, Tangerine, reset);

            delayedText($"{engine.mainCharacterCat.Name} said to himself, \"Oh, that wasn't enough. I need to do something else to cancel this meeting...\"", 50, reset, reset);
            delayedText($"{engine.mainCharacterCat.Name} roamed the house looking for another distraction.", 50, reset, reset);
            delayedText($"MISSION: Find the new sack of food in the Garage and create a distraction!", 50, Tangerine, reset);

            bool scene3Completed = false;

            while (!scene3Completed)
            {
                // 1. This runs the menu. Inside here, ValidateActionResult becomes TRUE 
                // only if user picks "Sack Opening" and "Claw".
                exploreHouse(engine);

                // 2. CHECK SUCCESS: We look for the location and the result from validateAction
                if (engine.mainCharacterCat.currentLocation.Name == "Garage" && ValidateActionResult == true)
                {
                    // We move the mission logic here so it triggers immediately
                    delayedText($"\nIn the garage, {engine.mainCharacterCat.Name} saw the brand new 5kg sack of cat food.", 50, reset, reset);
                    delayedText("An idea struck him.", 50, Tangerine, reset);

                    // TRIGGER THE ANIMATION
                    TearBagAnimation(Tangerine, reset);

                    delayedText($"{engine.mainCharacterCat.Name} clawed at the sack aggressively until it tore open!", 50, reset, reset);
                    delayedText($"He then scattered the kibble all over the garage floor, creating a massive mess to delay the date.", 50, reset, reset);

                    delayedText("\nMISSION ACCOMPLISHED: The garage is now a kibble minefield.", 60, Tangerine, reset);

                    scene3Completed = true; // This ends the loop
                }
            }
        }

        // Animation method
        private void TearBagAnimation(string color, string reset)
        {
            string frame1 = @"
                |-------|
                | KIBBLE|
                |  5KG  |
                |       |
                |_______|";

            string frame2 = @"
                |--- ---|
                | KI /LE|
                |  5/ G |
                |  /    |
                |_/_____|";

            string frame3 = @"
                |--   --|
                | K / \E|
                |  /   \|
                | / . . \
                |/ . . . \";

            string[] frames = { frame1, frame2, frame3 };

            Console.Clear();
            foreach (string frame in frames)
            {
                Console.SetCursorPosition(0, 5); // to keep the bag in the same place
                Console.WriteLine(color + frame + reset);
                Thread.Sleep(400);
            }
            Console.WriteLine("\n *ZRUPPPP* \n");
            Thread.Sleep(500);
        }
    }
}
