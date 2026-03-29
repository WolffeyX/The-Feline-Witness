using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
                                if (ValidateActionResult == false)
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

        public virtual bool validateAction(SecondaryItem item, ActionType action)
        {
            return false; //Please overrride this
        }

        public virtual bool validateAction(PrimaryItem item)
        {
            return false; // default behavior, override in child classes as needed
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

    internal class Scene2 : Scene
    {
        //Constructor
        public Scene2(string givenName)
        {
            Name = givenName;
        }

        public override bool validateAction(SecondaryItem item, ActionType action)
        {
            bool isValid = false;
            if (item.Name == "Car Key" && action == ActionType.Grab)
            {
                isValid = true;
            }
            return isValid;
        }

        public bool validateAction(PrimaryItem item, ActionType action)
        {
            bool isValid = false;
            if (item.Name == "Laundry Basket with stack of clothes" && action == ActionType.Bury)
            {
                isValid = true;
            }

            return isValid;
        }

        public void exploreHouse(GameEngine engine, string givenMissionName)
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

                    {
                        var selectedPrimary = currentRoom.itemsAvailable[pItemChoice - 1] as PrimaryItem;
                        if (selectedPrimary.Name == "Laundry Basket with stack of clothes") 
                        {
                            delayedText($"Congratulations,your guess is correct... ", 30, resetColorField, resetColorField);
                            delayedText($"Selected Item: {selectedPrimary.Name} ", 30, resetColorField, resetColorField);
                            delayedText($"Going to {selectedPrimary.Name} .....", 50, resetColorField, resetColorField);
                            Console.WriteLine($"\nWhat do you want to do with {selectedPrimary.Name}?\n");
                            var actions = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();
                            for (int i = 0; i < actions.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {actions[i]}");
                            }
                            Console.Write("Select action number: ");
                            if (int.TryParse(Console.ReadLine(), out int actionChoice) && actionChoice >= 1 && actionChoice <= actions.Count)
                            {
                                ActionType selectedAction = actions[actionChoice - 1];
                                ValidateActionResult = validateAction(selectedPrimary, selectedAction);
                                if (ValidateActionResult == false)
                                {
                                    delayedText($"Wrong action....but the item you just interacted with is correct", 30, resetColorField, resetColorField);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid action selection.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("This is not a suiable place to hide the car key....");
                        }
                        
                    }
                    else
                    {
                        throw new InvalidOperationException("Wrong primary item!");
                    }
                }
            }
            catch (Exception ex) //Exception Handling
            {
                Console.WriteLine($"Error: {ex.Message}. Please try again");
            }
        }

        public override void playScene(GameEngine engine)
        {
            engine.mainCharacterCat.currentLocation = engine.HouseSpaceList[5]; 
            string scene2 = @"
            =======================================================
                            SCENE 2: THE MEETUP
             =======================================================
            ";
            Console.WriteLine(scene2);
            engine.husband.displayDialogue($"\"get up {engine.mainCharacterCat.Name}. Papa wants to shower...\"", 50, resetColorField, resetColorField);
            delayedText($"{engine.mainCharacterCat.Name} finally snapped out of his shock. He jumped down from the sofa to the floor.", 50, resetColorField, resetColorField);
            engine.husband.displayDialogue($"\"Shower, then go buy groceries, then bake the cake, then she comes over... wow, it's gonna be a great day\"", 50, resetColorField, resetColorField);
            delayedText($"{engine.mainCharacterCat.Name} knew he had to stop this meeting. He thought of hiding the car keys. ", 50, resetColorField, resetColorField);
            Console.WriteLine("\n===========================================================================");
            Console.WriteLine($"MISSION: Find {engine.husband.Name}'s car key");
            Console.WriteLine($"Hint: It is located on a thing, that people always put another things on it");
            Console.WriteLine("===========================================================================\n");
            do
            {
                exploreHouse(engine);
            } while (ValidateActionResult == false);
            ValidateActionResult = false; //reset the ValidateActionResult for the next use in this scene
            Console.Clear();
            delayedText($"Congratulations, you found {engine.husband.Name}'s car key.", 50, resetColorField, resetColorField);
            delayedText($"Now {engine.mainCharacterCat.Name} wanted to hide the car key in one of the items in the house.", 50, resetColorField, resetColorField);
            Console.WriteLine("\n===========================================================================");
            Console.WriteLine($"MISSION: Hide {engine.husband.Name}'s car key...");
            Console.WriteLine("===========================================================================\n");
            string missionName = "Hide the car key";
            do
            {
                exploreHouse(engine, missionName);
            } while (ValidateActionResult == false);
            ValidateActionResult = false;
            delayedText($"{engine.mainCharacterCat.Name} buried the car key deep inside a pile of dirty clothes in a laundry basket.", 50, resetColorField, resetColorField);
            delayedText($"Then, it returned to the living room to watch {engine.husband.Name}'s next move. ", 50, resetColorField, resetColorField);
            engine.mainCharacterCat.currentLocation = engine.HouseSpaceList[5];
            Console.ReadLine();
            delayedText($"After showering and getting ready, {engine.husband.Name} looked for his keys. He checked the bedside table where he usually left them. Nothing. He searched the whole room. Still nothing.", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText($"Then, he remembered his keys had a location tracking feature. He used the app on his phone to play a sound. Beep... beep... {engine.husband.Name} followed the sound and found his keys in the dirty laundry basket.", 50, resetColorField, resetColorField);
            engine.husband.displayDialogue($"\"How did my key end up here? I don't remember putting it here...\"", 50, resetColorField, resetColorField);
            delayedText($"But he ignored the feeling, started the engine, and went out to buy ingredients for the cake.", 50, resetColorField, resetColorField);
            delayedText($"End of Scene 2...", 50, resetColorField, resetColorField);
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
            var garageSpace = engine.HouseSpaceList.FirstOrDefault(space => space.Name == "Garage");
            if (garageSpace != null)
            {
                var husbandCar = garageSpace.itemsAvailable.FirstOrDefault(item => item.Name == "Husband Car");
                if (husbandCar != null)
                {
                    garageSpace.itemsAvailable.Remove(husbandCar);
                }
            }

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

    internal class Scene4 : Scene
    {
        //Constructor
        public Scene4(string givenName)
        {
            Name = givenName;
        }

        public override bool validateAction(SecondaryItem item, ActionType action)
        {
            bool isValid = false;
            if (item.Name == "Grocery Bag" && action == ActionType.Shove)
            {
                isValid = true;
            }
            return isValid;
        }

        public override void playScene(GameEngine engine)
        {
            string scene4 = @"
             =======================================================
                          SCENE 4: THE EGGS & THE CAGE
             =======================================================
            ";

            // unhide GarageHusbandCar
            var garage = engine.HouseSpaceList.FirstOrDefault(space => space.Name == "Garage");
            var husbandCar = garage.itemsAvailable.FirstOrDefault(item => item.Name == "HusbandCar");
            if (husbandCar != null)
            {
                //husbandCar.IsHidden = false; // unhide
            }

            // add grocery bag
            // cari Kitchen asal dari game engine
            var kitchen = engine.HouseSpaceList.FirstOrDefault(space => space.Name == "Kitchen");

            // cari bar table asal
            var kitchenBarTable = kitchen.itemsAvailable.FirstOrDefault(item => item.Name == "Bar Table") as PrimaryItem;

            // tambah grocery bag ke bar table asal
            SecondaryItem groceryBag = new SecondaryItem("Grocery Bag", kitchen.Name);
            kitchenBarTable.AvailableSecondaryItem.Add(groceryBag);

            delayedText(scene4, 10, resetColorField, resetColorField);

            delayedText($"Upon returning home, {engine.husband.Name} was shocked to see cat food scattered all over the garage floor", 50, resetColorField, resetColorField);

            engine.husband.displayDialogue($"\"Hah! How did the cat food bag get torn? And it's everywhere!\"", 50, resetColorField, resetColorField);

            Console.ReadLine();

            delayedText($"After parking, he entered the house holding the grocery bags. He saw {engine.mainCharacterCat.Name} sitting on his mat, stiff, pretending not to look. " +
                $"{engine.husband.Name} put the groceries in the kitchen, then picked {engine.mainCharacterCat.Name} up and looked him in the eye.", 50, resetColorField, resetColorField);

            engine.husband.displayDialogue($"\"{engine.mainCharacterCat.Name} did you tear the food bag and make a mess?\"", 50, resetColorField, resetColorField);

            Console.ReadLine();

            delayedText($"{engine.mainCharacterCat.Name} just meowed, effectively admitting it in cat language.", 50, resetColorField, resetColorField);

            engine.husband.displayDialogue($"\"It must be you, {engine.mainCharacterCat.Name}." +
                $" Who else would it be?\"", 50, resetColorField, resetColorField);

            Console.ReadLine();

            delayedText($"{engine.husband.Name} said sternly. Then his voice softened.", 50, resetColorField, resetColorField);

            engine.husband.displayDialogue($"\"Sigh, {engine.mainCharacterCat.Name}, {engine.mainCharacterCat.Name}... why are you acting up today?\"", 50, resetColorField, resetColorField);

            Console.ReadLine();

            delayedText($"He put {engine.mainCharacterCat.Name} down and grabbed a broom to clean the garage.", 50, resetColorField, resetColorField);

            delayedText($"Seizing the oppurtunity while {engine.husband.Name} swept the garage, it wanted to create another mess at another place.", 50, resetColorField, resetColorField);

            Console.Clear();

            Console.WriteLine("\n===========================================================================");
            Console.WriteLine($"MISSION: The player needs to create a mess at one of the house space again.");
            Console.WriteLine("===========================================================================\n");

            engine.mainCharacterCat.currentLocation = engine.HouseSpaceList[5]; // remove this later

            do
            {
                exploreHouse(engine);
            } while (ValidateActionResult == false);

            ValidateActionResult = false; // reset the ValidateActionResult for the next use in this scene
            Console.WriteLine();
            delayedText($"It saw a carton of eggs inside the grocery bag, located on top of the bar table.", 50, resetColorField, resetColorField);
            Console.Clear();

            delayedText($"With all his might, it leaped and shoved the GroceryBag off the kitchen's bar table.", 50, resetColorField, resetColorField);

            delayedText($"SPLAT.", 50, resetColorField, resetColorField);

            delayedText($"{engine.husband.Name} snapped.", 50, resetColorField, resetColorField);

            engine.husband.displayDialogue($"\"{engine.mainCharacterCat.Name}!! What is wrong with you?! Argh... why are you so aggressive today? Tearing food bag, now breaking the eggs!\"",
                50, resetColorField, resetColorField);

            delayedText($"{engine.mainCharacterCat.Name} only replied,", 50, resetColorField, resetColorField);

            engine.mainCharacterCat.displayDialogue($"\"Meow.\"", 50, resetColorField, resetColorField);

            engine.husband.displayDialogue($"\"Sorry but Papa has to put you in the cage for a while.\"", 50, resetColorField, resetColorField);

            delayedText($"{engine.mainCharacterCat.Name} was placed in the cage located in the garage. Even though the cage was spacious with two levels, {engine.mainCharacterCat.Name} was trapped." +
                $"He could no longer interfere.", 50, resetColorField, resetColorField);

            delayedText($"After locking the cage,{engine.husband.Name} took out his phone and called {engine.mistress.Name}.", 50, resetColorField, resetColorField);

            engine.mistress.displayDialogue($"\"Hey babe, I had some issues earlier... I'm just starting to bake now. Can you come a bit later? Maybe 5 PM?\"", 50, resetColorField, resetColorField);

            engine.husband.displayDialogue($"\"Oh, okay...\"", 50, resetColorField, resetColorField);

            engine.mistress.displayDialogue($"\"Okay baby, bye...\"", 50, resetColorField, resetColorField);

            delayedText($"{engine.mainCharacterCat.Name} heard the conversation.", 50, resetColorField, resetColorField);

            engine.mainCharacterCat.displayDialogue($"\"So {engine.mistress.Name} will arrive at 5 PM...\"", 50, resetColorField, resetColorField);

            delayedText($"he thought.", 50, resetColorField, resetColorField);

            engine.mainCharacterCat.displayDialogue($"\"There is nothing else I can do now.\"", 50, resetColorField, resetColorField);

            delayedText($"{engine.husband.Name} went to the kitchen and started baking.", 50, resetColorField, resetColorField);

            engine.mainCharacterCat.currentLocation = engine.HouseSpaceList[6]; // player moves to the garage

            delayedText($"End of Scene 4...", 50, resetColorField, resetColorField);
        }
    }

    internal class Scene5 : Scene
    {
        public Scene5(string givenName)
        {
            Name = givenName;
        }

        public override bool validateAction(SecondaryItem item, ActionType action)
        {
            bool isValid = false;
            if (item.Name == "CCTV wire" && action == ActionType.Shove)
            {
                isValid = true;
            }
            return isValid;
        }

        public override bool validateAction(PrimaryItem item)
        {
            if (item.Name == "TV Cabinet")
            {
                return true;
            }
            return false;
        }

        public void exploreHouse(GameEngine engine, string givenMissionName)
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

                    {
                        var selectedPrimary = currentRoom.itemsAvailable[pItemChoice - 1] as PrimaryItem;
                        ValidateActionResult = validateAction(selectedPrimary);
                        if (ValidateActionResult == false)
                        {
                            Console.WriteLine("\nInteracting with this item won't help. Keep searching!");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Wrong primary item!");
                    }
                }
            }
            catch (Exception ex) //Exception Handling
            {
                Console.WriteLine($"Error: {ex.Message}. Please try again");
            }
        }

        public override void playScene(GameEngine engine)
        {
            string scene5 = @"
             =======================================================
                        SCENE 5: THE SHOWDOWN & THE CAMERA
             =======================================================
            ";

            engine.mainCharacterCat.currentLocation = engine.HouseSpaceList[5]; //Set the current location of the cat to the first room in the house, which is the living room

            delayedText(scene5, 10, resetColorField, resetColorField);
            delayedText("At 5:07 PM, a blue car arrived and parked in front of the gate.", 50, resetColorField, resetColorField);
            delayedText($"{engine.mainCharacterCat.Name} watched as {engine.husband.Name} opened the gate. The car pulled into the garage. A woman stepped out.", 50, resetColorField, resetColorField);
            delayedText($"\"Baby..!\" said {engine.husband.Name}.", 50, resetColorField, resetColorField);
            delayedText("\"Yeah baby...... wow, nice house, eh\" the woman replied.", 50, resetColorField, resetColorField);
            delayedText($"\"Come inside. Are you ready to taste my White Chocolate Macadamia cake?\"", 50, resetColorField, resetColorField);
            delayedText($"\"Ready! I hope it tastes really good. Eh, a cat! You have a cat too?\" the woman asked, pointing at the cage.", 50, resetColorField, resetColorField);
            delayedText($"\"{engine.husband.Name} walked to the cage, unlocked it, picked {engine.mainCharacterCat.Name} up, and brought him to {engine.mistress.Name}.\"", 50, resetColorField, resetColorField);
            delayedText($"\"I bring my cats too. I just picked them up from the grooming service,\" the woman said, petting {engine.mainCharacterCat.Name}'s head.", 50, resetColorField, resetColorField);
            delayedText($"She went to her car and brought out two Persian cats, one is grey and one is white.", 50, resetColorField, resetColorField);

            delayedText("Let's put a name for the grey cat... ", 50, resetColorField, resetColorField);
            getName(engine.greyCat, "grey cat");

            delayedText("Let's put a name for the white cat... ", 50, resetColorField, resetColorField);
            getName(engine.whiteCat, "white cat");

            delayedText($"\"The grey one is {engine.greyCat.Name}, and the white one is {engine.whiteCat.Name},\" she said.", 50, resetColorField, resetColorField);
            delayedText($"\"Wow, you got very pretty cats there,\" said {engine.husband.Name}.", 50, resetColorField, resetColorField);
            delayedText($"\"Thank you...\" said {engine.mistress.Name}.", 50, resetColorField, resetColorField);
            delayedText($"{engine.mainCharacterCat.Name} confirmed it. This woman was definitely {engine.mistress.Name}.", 50, resetColorField, resetColorField);
            delayedText($"\"Okay, let's go inside. You can bring your cat in.\"", 50, resetColorField, resetColorField);

            Thread.Sleep(1500);
            delayedText("...", 200, resetColorField, resetColorField);

            delayedText($"The atmosphere in the living room was romantic. {engine.husband.Name} and {engine.mistress.Name} sat close on the sofa, enjoying the freshly baked cake while watching a movie on Netflix.", 50, resetColorField, resetColorField);
            delayedText($"\"It's delicious, I didn't expect you could bake,\" {engine.mistress.Name} praised, feeding a piece of cake to {engine.husband.Name}.", 50, resetColorField, resetColorField);
            delayedText($"{engine.mainCharacterCat.Name} watched from his mat with a restless tail. His eyes were fixed on the TV cabinet.", 50, resetColorField, resetColorField);
            delayedText($"Behind that cabinet was the main switch for the Smart Home CCTV. Before {engine.mistress.Name} arrived, {engine.husband.Name} had turned off the switch so the camera would be \"Offline.\" {engine.mainCharacterCat.Name} knew this because the small blue light on the camera was off.", 50, resetColorField, resetColorField);
            delayedText($"{engine.mainCharacterCat.Name} knew that if the switch was pressed again, the camera would reactivate, and a notification would be sent to {engine.wife.Name}'s phone: \"CCTV Living Room is Online\".", 50, resetColorField, resetColorField);
            Console.WriteLine("\n===========================================================================");
            Console.WriteLine($"MISSION: GO TO THE BACK OF THE TV CABINET.");
            Console.WriteLine("===========================================================================\n");
            string missionName = "GO TO THE BACK OF THE TV CABINET";
            do
            {
                exploreHouse(engine, missionName);
            } while (ValidateActionResult == false);
            ValidateActionResult = false; // reset the ValidateActionResult 
            delayedText($"[CHECKPOINT] {engine.mainCharacterCat.Name} began to move. He walked slowly, trying to approach the TV cabinet.", 50, resetColorField, resetColorField);
            delayedText($"However, his movement was detected by {engine.whiteCat.Name} and {engine.greyCat.Name}. They jumped down from the sofa and blocked {engine.mainCharacterCat.Name}'s path.", 50, resetColorField, resetColorField);
            delayedText($"They weren't just blocking him; they were guarding their new \"master's\" territory. {engine.greyCat.Name} hissed loud, its fur standing on end, making it look twice {engine.mainCharacterCat.Name}'s size.", 50, resetColorField, resetColorField);
            delayedText($"\"Meow!\" (Move!), {engine.mainCharacterCat.Name} warned. {engine.greyCat.Name} replied with a swift swipe of its claws, nicking {engine.mainCharacterCat.Name}'s left ear. A drop of blood fell.", 50, resetColorField, resetColorField);
            delayedText($"{engine.mainCharacterCat.Name}'s patience snapped. He remembered {engine.wife.Name}'s gentle pets, the food she gave, the love she poured out. He would not let this house be taken over by intruders.", 50, resetColorField, resetColorField);

            bool completeCombat1 = false;
            while (!completeCombat1)
            {
                // Combat 1
                delayedText($"[MISSION 1: COMBAT INITIATED] {engine.mainCharacterCat.Name} VS {engine.greyCat.Name}", 30, resetColorField, resetColorField);
                bool wonFight1 = CombatLoop(engine.mainCharacterCat, engine.greyCat);

                if (!wonFight1)
                {
                    delayedText($"[GAME OVER] {engine.mainCharacterCat.Name} was defeated... Restarting from the checkpoint...", 50, resetColorField, resetColorField);
                    Thread.Sleep(2000);
                    engine.mainCharacterCat.HP = 80; // reset HP
                    engine.greyCat.HP = 80; // reset enemy HP
                    continue; // Loop back
                }
                completeCombat1 = true;
            }

            delayedText($"{engine.greyCat.Name} is severely weakened and scurries away! Realizing who the true \"Alpha\" was, {engine.greyCat.Name} scurried away to hide behind the dining table, trembling in fear.", 50, resetColorField, resetColorField);
            delayedText($"But the fight wasn't over. {engine.whiteCat.Name} suddenly ambushed {engine.mainCharacterCat.Name} from behind!", 50, resetColorField, resetColorField);
            delayedText($"{engine.whiteCat.Name} gain some HP", 50, resetColorField, resetColorField);
            engine.mainCharacterCat.HP += 40;

            bool completeCombat2 = false;
            while (!completeCombat2)
            {
                // Combat 2
                delayedText($"[MISSION 2: COMBAT INITIATED] {engine.mainCharacterCat.Name} VS {engine.whiteCat.Name}", 30, resetColorField, resetColorField);
                bool wonFight2 = CombatLoop(engine.mainCharacterCat, engine.whiteCat);

                if (!wonFight2)
                {
                    delayedText($"[GAME OVER] {engine.mainCharacterCat.Name} was defeated... Restarting from the checkpoint...", 50, resetColorField, resetColorField);
                    Thread.Sleep(2000);
                    engine.mainCharacterCat.HP = 80; // reset HP
                    engine.greyCat.HP = 80; // reset enemy HP
                    continue; // Loop back
                }
                completeCombat2 = true;
            }

            delayedText($"{engine.whiteCat.Name} was severely weakened and scurries away!", 50, resetColorField, resetColorField);
            delayedText($"{engine.whiteCat.Name} immediately retreated, sliding under the sofa to join its sibling.", 50, resetColorField, resetColorField);
            delayedText($"{engine.mainCharacterCat.Name} stood tall, chest heaving, scanning the room with fiery eyes. {engine.mainCharacterCat.Name} Wins!", 50, resetColorField, resetColorField);
            // The Climax
            delayedText($"Without wasting time, {engine.mainCharacterCat.Name} ran back to the TV cabinet. He saw the CCTV wire hanging loose.", 50, resetColorField, resetColorField);
            do
            {
                exploreHouse(engine);
            } while (ValidateActionResult == false);
            ValidateActionResult = false; // reset the ValidateActionResult
            bool isCameraOnline = false;
            while (!isCameraOnline)
            {
                isCameraOnline = PlugInWireMiniGame(engine.mainCharacterCat);

                if (!isCameraOnline)
                {
                    delayedText($"{engine.mainCharacterCat.Name} shook off the failure and gathered his strength to try again...", 50, "\x1b[38;2;255;153;51m", resetColorField);
                    Thread.Sleep(1000);
                }
            }
            Console.Clear(); 
            Thread.Sleep(1500);
            delayedText("...", 200, resetColorField, resetColorField);

            delayedText($"Meanwhile, hundreds of kilometers away: {engine.wife.Name}'s smartphone dinged. A notification appeared: [Smart Home]: Living Room Camera is now ONLINE.", 50, resetColorField, resetColorField);
            delayedText($"{engine.wife.Name}, resting in her hotel room, was confused. \"Huh? Was the CCTV offline earlier?\" She opened the app to see what was happening. Her heart stopped. On the screen, she clearly saw {engine.husband.Name} sitting with a strange woman on their sofa.", 50, resetColorField, resetColorField);
            delayedText($"Without hesitating, {engine.wife.Name} pressed the Screenshot button.", 50, resetColorField, resetColorField);

            delayedText("Ring... Ring...", 100, resetColorField, resetColorField);
            delayedText($"{engine.husband.Name}'s phone on the coffee table rang. The name \"Wife\" flashed on the screen. {engine.husband.Name} signaled {engine.mistress.Name} to be quiet. He picked up the phone, feigning a calm voice.", 50, resetColorField, resetColorField);
            delayedText($"\"Hello honey... why are you calling? I was just about to sleep, pretty tired.\"", 50, resetColorField, resetColorField);
            delayedText($"{engine.wife.Name} asked in a voice that was terrifyingly calm, \"Where are you?\"", 50, resetColorField, resetColorField);
            delayedText($"\"At the hotel, honey. Like I said, I'm outstation too. Just got out of the shower. Are you okay?\" {engine.husband.Name} lied without guilt.", 50, resetColorField, resetColorField);
            delayedText($"\"Oh... at the hotel...\" {engine.wife.Name} replied. \"Open WhatsApp for a second.\"", 50, resetColorField, resetColorField);
            delayedText($"\"Why?\"", 50, resetColorField, resetColorField);
            delayedText($"\"Just open it.\"", 50, resetColorField, resetColorField);

            delayedText($"The line was still connected. {engine.husband.Name} pulled the phone away from his ear and opened WhatsApp. A picture message had just come in.", 50, resetColorField, resetColorField);
            delayedText($"It was a screenshot of him and {engine.mistress.Name} sitting on the sofa, taken from the CCTV angle, one minute ago. Below the picture, there was a short sentence typed in capital letters:", 50, resetColorField, resetColorField);
            delayedText($"\"THEN WHAT IS THIS?\"", 100, resetColorField, resetColorField);

            delayedText($"{engine.husband.Name}'s face went pale. The blood drained from his head. The phone nearly slipped from his hand. He looked up at the CCTV in the corner of the ceiling, which was now glowing with a steady blue light.", 50, resetColorField, resetColorField);
            delayedText($"\"Honey... I... I can explain...\" His voice trembled.", 50, resetColorField, resetColorField);

            delayedText("[SCENE 5 COMPLETE]", 30, resetColorField, resetColorField);
        }

        

        private bool PlugInWireMiniGame(MainCharacter playerCat)
        {
            Console.Clear();
            Console.WriteLine("=======================================================");
            Console.WriteLine("                 MINI-GAME INITIATED                   ");
            Console.WriteLine("=======================================================\n");
            delayedText($"Action: {playerCat.Name} bit the wire and pulled it toward the socket.", 30, resetColorField, resetColorField);
            Console.WriteLine("It's tough! You need to use your entire body weight to shove it in!");
            Console.WriteLine("\nINSTRUCTIONS:");
            Console.WriteLine("Mash the [SPACEBAR] repeatedly to build momentum!");
            Console.WriteLine("You have 5 seconds to fill the progress bar.");
            Console.WriteLine("\nPress ENTER when you are ready...");
            Console.ReadLine();

            int targetPresses = 25; // Number of spacebar hits needed
            int currentPresses = 0;
            int timeLimitSeconds = 5;

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            // Clear the input buffer before starting the mashing phase
            while (Console.KeyAvailable) Console.ReadKey(true);

            while (timer.Elapsed.TotalSeconds < timeLimitSeconds && currentPresses < targetPresses)
            {
                // Draw the progress bar dynamically
                DrawProgressBar(currentPresses, targetPresses, timeLimitSeconds - (int)timer.Elapsed.TotalSeconds);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Spacebar)
                    {
                        currentPresses++;
                    }
                }

                // Small sleep to prevent CPU hogging
                Thread.Sleep(15);
            }
            DrawProgressBar(currentPresses, targetPresses, Math.Max(0, timeLimitSeconds - (int)timer.Elapsed.TotalSeconds));
            timer.Stop();
            Console.WriteLine("\n"); // Move to a new line after the progress bar finishes drawing

            // Win/Loss Condition
            if (currentPresses >= targetPresses)
            {
                // Success Text
                delayedText("SUCCESS!", 20, "\x1b[32m", resetColorField); // Green text
                delayedText($"Using all your strength, {playerCat.Name} shove the plug back into the wall outlet!", 40, resetColorField, resetColorField);
                delayedText("Click.", 50, resetColorField, resetColorField);
                delayedText("The light on the ceiling camera blinked red, then turned solid blue. ONLINE.", 50, resetColorField, resetColorField);
                return true;
            }
            else
            {
                // Fail Text
                delayedText("FAILED!", 20, "\x1b[31m", resetColorField); // Red text
                delayedText("Oof! Your paws slip on the floor. The heavy plug falls out of the socket.", 40, resetColorField, resetColorField);
                delayedText("You need to try again!", 40, resetColorField, resetColorField);
                return false;
            }
        }

        private void DrawProgressBar(int current, int target, int timeLeft)
        {
            int barSize = 25; // Width of the progress bar in console characters
            int progress = (int)((double)current / target * barSize);

            if (progress > barSize) progress = barSize;

            string filled = new string('█', progress);
            string empty = new string('-', barSize - progress);

            // \r returns the cursor to the beginning of the line to overwrite the previous bar frame
            Console.Write($"\rForce: [{filled}{empty}] {current}/{target} | Time Left: {timeLeft}s   ");
        }

        private bool CombatLoop(MainCharacter playerCat, Cat enemyCat)
        {
            Random rng = new Random();

            while (playerCat.HP > 0 && enemyCat.HP > 2)
            {
                Console.WriteLine($"\n--- HP | {playerCat.Name}: {playerCat.HP} | {enemyCat.Name}: {enemyCat.HP} ---");
                Console.WriteLine("Choose your attack:");
                Console.WriteLine($"1. {Cat.FightingOptions.Claw} (15-25 Damage)");
                Console.WriteLine($"2. {Cat.FightingOptions.Kick} (10-30 Damage)");
                Console.WriteLine($"3. {Cat.FightingOptions.Bite} (20-40 Damage, 30% chance to miss)");
                Console.Write("Action: ");

                string choice = Console.ReadLine();
                int damageDealt = 0;

                switch (choice)
                {
                    case "1":
                        damageDealt = rng.Next(15, 26);
                        delayedText($"{playerCat.Name} uses {Cat.FightingOptions.Claw}! Deals {damageDealt} damage.", 20, resetColorField, resetColorField);
                        break;
                    case "2":
                        damageDealt = rng.Next(10, 31);
                        delayedText($"{playerCat.Name} uses {Cat.FightingOptions.Kick}! Deals {damageDealt} damage.", 20, resetColorField, resetColorField);
                        break;
                    case "3":
                        if (rng.Next(0, 100) < 30)
                        {
                            delayedText($"{playerCat.Name} uses {Cat.FightingOptions.Bite}... but misses!", 20, resetColorField, resetColorField);
                        }
                        else
                        {
                            damageDealt = rng.Next(20, 41);
                            delayedText($"{playerCat.Name} lands a devastating {Cat.FightingOptions.Claw}! Deals {damageDealt} damage.", 20, resetColorField, resetColorField);
                        }
                        break;
                    default:
                        delayedText("Invalid move! You lost your turn.", 20, resetColorField, resetColorField);
                        break;
                }

                enemyCat.HP -= damageDealt;

                if (enemyCat.HP <= 2) break;

                int enemyDamage = rng.Next(10, 25);
                playerCat.HP -= enemyDamage;
                delayedText($"{enemyCat.Name} strikes back! Deals {enemyDamage} damage to {playerCat.Name}.", 20, resetColorField, resetColorField);
            }

            return playerCat.HP > 0;
        }
    } 

    internal class Scene6 : Scene
    {
        public Scene6(string givenName)
        {
            Name = givenName;
        }

        public override void playScene(GameEngine engine)
        {
            string scene6Banner = @"
            =======================================================
                        SCENE 6: A NEW BEGINNING
            =======================================================
        ";

            string Tangerine = "\x1b[38;2;255;153;51m";
            string SoftBlue = "\x1b[38;2;135;206;235m]";

            delayedText(scene6Banner, 10, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText("One Month Later...", 100, resetColorField, resetColorField);
            Console.ReadLine();
            Console.Clear();

            delayedText($"The atmosphere in the new apartment still felt foreign.", 50, resetColorField, resetColorField);
            delayedText($"The smell of fresh paint mixed with the scent of cardboard boxes that hadn't been fully unpacked.", 50, resetColorField, resetColorField);
            delayedText($"This living room was smaller than the old house, but for some reason, the air felt lighter and less suffocating.", 50, resetColorField, resetColorField);
            Console.ReadLine();

            delayedText($"{engine.mainCharacterCat.Name} sat on top of a box, staring out the window at a cityscape he didn't recognize.", 50, resetColorField, resetColorField);
            delayedText($"He no longer saw the garden of the old house. Only tall buildings.", 50, resetColorField, resetColorField);
            Console.ReadLine();

            delayedText($"{engine.mainCharacterCat.Name} recalled who {engine.mistress.Name} really was.", 50, resetColorField, resetColorField);
            delayedText($"During the huge argument on the night of the incident, it was revealed that {engine.mistress.Name} was actually {engine.husband.Name}'s old friend from university.", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText($"They had 'found' each other again on social media three months ago.", 50, resetColorField, resetColorField);
            delayedText($"It started with liking pictures, then commenting, and finally led to secret meetings at {engine.husband.Name}'s favorite cafe—", 50, resetColorField, resetColorField);
            delayedText($"the same cafe where {engine.husband.Name} had taken {engine.wife.Name} when they first started dating.", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText($"Turns out, {engine.husband.Name} was trying to relive his old romance, but with a different woman.", 50, resetColorField, resetColorField);
            Console.ReadLine();

            delayedText($"{engine.mistress.Name} wasn't a total stranger; she was the past that {engine.husband.Name} chose to make his future,", 50, resetColorField, resetColorField);
            delayedText($"destroying the present he had built with {engine.wife.Name}.", 50, resetColorField, resetColorField);
            Console.ReadLine();

            delayedText($"{engine.mainCharacterCat.Name} meowed softly. His heart felt heavy.", 70, resetColorField, resetColorField);
            Console.ReadLine();

            delayedText($"Truthfully, {engine.mainCharacterCat.Name} was sad. What cat wouldn't be sad seeing his family broken apart?", 50, resetColorField, resetColorField);
            delayedText($"He missed the times {engine.husband.Name} stroked his head while watching football.", 50, resetColorField, resetColorField);
            delayedText($"He missed the couple's laughter that once filled the living room.", 50, resetColorField, resetColorField);
            Console.ReadLine();

            delayedText($"But {engine.mainCharacterCat.Name} knew he couldn't let the deception continue.", 50, resetColorField, resetColorField);
            delayedText($"He couldn't bear to see {engine.wife.Name}—the owner who loved him the most, who fed him, who nursed him when he was sick—living in a lie.", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText($"To {engine.mainCharacterCat.Name}, loyalty was everything.", 70, resetColorField, resetColorField);
            delayedText($"If the Head of the House was willing to betray that trust, he didn't deserve to be part of the family anymore.", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText($"Let this home be a little quieter, as long as there was no more betrayal.", 50, resetColorField, resetColorField);
            Console.ReadLine();
            Console.Clear();

            delayedText($"The door opened.", 70, resetColorField, resetColorField);
            delayedText($"{engine.wife.Name} walked in.", 50, resetColorField, resetColorField);
            delayedText($"Her face looked calmer than it had in weeks, even though her eyes were still slightly puffy.", 50, resetColorField, resetColorField);
            delayedText($"She saw {engine.mainCharacterCat.Name} sitting quietly on the box by the window.", 50, resetColorField, resetColorField);
            Console.ReadLine();

            engine.wife.displayDialogue($"\"{engine.mainCharacterCat.Name}...\"", 80, resetColorField, resetColorField);
            Console.ReadLine();

            delayedText($"{engine.mainCharacterCat.Name} trotted over to {engine.wife.Name}, rubbing his body gently against her legs.", 50, resetColorField, resetColorField);
            Console.ReadLine();

            delayedText($"{engine.wife.Name} picked {engine.mainCharacterCat.Name} up and hugged him tight.", 50, resetColorField, resetColorField);
            engine.wife.displayDialogue($"\"Now it's just the two of us, {engine.mainCharacterCat.Name}.\"", 60, resetColorField, resetColorField);
            engine.wife.displayDialogue($"\"Thank you for 'telling' Mama that day. If you hadn't... who knows how long I would have been fooled.\"", 60, resetColorField, resetColorField);
            Console.ReadLine();

            delayedText($"{engine.wife.Name} kissed {engine.mainCharacterCat.Name} softly on the head.", 50, resetColorField, resetColorField);
            Console.ReadLine();
            delayedText($"Outside the window, a light rain began to fall...", 70, resetColorField, resetColorField);
            delayedText($"...as if washing away all the dirt and bitter memories of the old house,", 50, resetColorField, resetColorField);
            delayedText($"giving them both a chance to start a new life.", 50, resetColorField, resetColorField);
            Console.ReadLine();

            delayedText($"{engine.mainCharacterCat.Name} closed his eyes, feeling safe in his owner's arms.", 60, resetColorField, resetColorField);
            delayedText($"He knew he had done the right thing.", 80, resetColorField, resetColorField);
            Console.ReadLine();
            Console.Clear();

            string ending = @"
            =======================================================

                            ~ T H E   E N D ~

                Thank you for playing The Feline Witness.

            =======================================================
        ";
            delayedText(ending, 40, Tangerine, resetColorField);
            Console.ReadLine();
        }
    }
}

