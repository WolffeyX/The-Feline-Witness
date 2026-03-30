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
    internal abstract class Scene //OOP Concept applied: ABSTRACTION (Scene is an abstract class that defines common properties and methods for different scenes in the game, but cannot be instantiated on its own)
    {
        //OOP Concept applied: ENCAPSULATION (private fields, and public properties)
        // 1. Private Fields
        private string name;
        private string resetColorField = "\x1b[0m"; 
        private string tangerine = "\x1b[38;2;255;153;51m"; 
        private bool validateActionResult;

        // 2. Public Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string ResetColorField
        {
            get { return resetColorField; }
            set { resetColorField = value; }
        }

        public string Tangerine 
        {
            get { return tangerine; }
        }

        public bool ValidateActionResult
        {
            get { return validateActionResult; }
            set { validateActionResult = value; }
        }

        // 3. Methods
        public void delayedText(string text, int speed, string textColor, string resetColor)
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
        //OOP Concept applied: POLYMORPHISM (Method Overloading - two methods with the same name but different parameters)
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
                delayedText($"Enter the {objectName}'s name: ", 50, ResetColorField, ResetColorField, false);
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
            for (int i = 0; i < givenHouseSpace.ItemsAvailable.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {givenHouseSpace.ItemsAvailable[i].Name}");
            }
        }

        public void goTo(HouseSpace houseSpace)
        {
            delayedText($"Going to {houseSpace.Name} .....", 50, ResetColorField, ResetColorField);
        }

        public virtual void exploreHouse(GameEngine engine)
        {
            Console.WriteLine($"\n[Current Location: {engine.MainCharacterCat.CurrentLocation?.Name ?? "Not set"}]");
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
                    engine.House.displayMap();
                    Console.WriteLine("===========================================================================");
                    Console.WriteLine("\n");
                }
                else if (input == 'C')
                {
                    Console.WriteLine("\n");
                    Console.WriteLine($"[Current Location: {engine.MainCharacterCat.CurrentLocation.Name}]\n");
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
                        engine.MainCharacterCat.CurrentLocation = selectedRoom;
                    }
                    else
                    {
                        Console.WriteLine("Invalid room selection.");
                    }
                }
                else if (input == 'E')
                {
                    var currentRoom = engine.MainCharacterCat.CurrentLocation;
                    if (currentRoom == null || currentRoom.ItemsAvailable.Count == 0)
                    {
                        Console.WriteLine("There is nothing to explore here.");
                        return;
                    }

                    Console.WriteLine($"\n");
                    Console.WriteLine($"Available items/furnitures in {currentRoom.Name}:\n");
                    displayItemsAvailable(currentRoom);
                    Console.Write("Select item number to inspect: ");

                    if (int.TryParse(Console.ReadLine(), out int pItemChoice) && pItemChoice >= 1 && pItemChoice <= currentRoom.ItemsAvailable.Count)
                    //int.TryParse(string input, out int result)
                    //This method tries to convert the string input (first parameter) into an integer.
                    //If it succeeds, it returns true and assigns the converted integer to the second parameter. 
                    //out int pItemChoice : Create an int variable named pItemChoice, and
                    //assign the value that has been parsed from the user input.
                    {
                        var selectedPrimary = currentRoom.ItemsAvailable[pItemChoice - 1] as PrimaryItem;
                        delayedText($"Selected Item: {selectedPrimary.Name} ", 30, ResetColorField, ResetColorField);
                        delayedText($"Going to {selectedPrimary.Name} .....", 50, ResetColorField, ResetColorField);

                        if (selectedPrimary == null || selectedPrimary.AvailableSecondaryItem.Count == 0)
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
                            delayedText($"Selected Item: {selectedSecondary.Name} ", 30, ResetColorField, ResetColorField);

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
                                ValidateActionResult = validateAction(selectedSecondary, selectedAction);
                                if (ValidateActionResult == false)
                                {
                                    Console.WriteLine("Invalid action / Nothing to do here");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid action selection."); //OOP Concept applied: Exeption Handling
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Please try again");
            }
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - child classes can provide specific implementation of this method based on the scene's requirement)
        public virtual void playScene(GameEngine engine)
        {
        }

        //OOP Concept applied: POLYMORPHISM (Method Overloading)
        public virtual bool validateAction(SecondaryItem item, ActionType action)
        {
            return false;
        }

        public virtual bool validateAction(PrimaryItem item)
        {
            return false;
        }
    }

    internal class IntroScene : Scene //OOP Concept applied: INHERITANCE (IntroScene is a child class that inherits from the abstract class Scene, and provides specific implementation for the playScene method)
    {
        public IntroScene(string givenName)
        {
            this.Name = givenName;
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

            

            delayedText(titleArt, 10, Tangerine, ResetColorField);
            delayedText(catASCII, 10, Tangerine, ResetColorField);
            Console.WriteLine("\n=======================================================");
            Console.WriteLine("                  Press 'ENTER' to start...            ");
            Console.WriteLine("=======================================================\n");

            Console.ReadLine();
            delayedText("Welcome to The Feline Witness :).", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText("This game is about a cat, that witnesses an event that changes it and its owner life, who is a woman, and a wife.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText("First, let's give the cat a name.", 50, ResetColorField, ResetColorField);
            getName(engine.MainCharacterCat, "cat");
            delayedText("Perfect...", 50, ResetColorField, ResetColorField);
            Console.Clear();
        }
    }

    internal class Scene1 : Scene
    {
        public Scene1(string givenName)
        {
            this.Name = givenName;
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override bool validateAction(SecondaryItem item, ActionType action)
        {
            bool isValid = false;
            if (item.Name == "Husband Smartphone" && action == ActionType.Observe)
            {
                isValid = true;
            }
            return isValid;
        }
        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override void playScene(GameEngine engine)
        {
            string scene1 = @"
             =======================================================
                            SCENE 1: SUSPICIOUS
             =======================================================
            ";

            engine.MainCharacterCat.CurrentLocation = engine.HouseSpaceList[5];

            delayedText(scene1, 10, Tangerine, ResetColorField);
            delayedText($"Meet {engine.MainCharacterCat.Name}. {engine.MainCharacterCat.Name} was a cat owned by a husband and wife who lived happily in a nice house.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText("Let's give the husband a name.", 50, ResetColorField, ResetColorField);
            getName(engine.Husband, "husband");
            delayedText($"Perfect..., the husband's name now is {engine.Husband.Name}. Now let's give the wife a name.", 50, ResetColorField, ResetColorField);
            getName(engine.Wife, "wife");
            delayedText($"Great..., the wife's name now is {engine.Wife.Name}.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            Console.Clear();

            delayedText($"One day, {engine.Wife.Name} had to go out of town for work (outstation). " +
                $"According to {engine.Husband.Name}, he also had an outstation trip and had to leave the house that day as well. " +
                $"However, {engine.Wife.Name} had to leave first", 50, ResetColorField, ResetColorField);

            engine.Wife.displayDialogue($"\"Bye bye darling. Love you.\"", 50, ResetColorField, ResetColorField);
            engine.Husband.displayDialogue($"\"Bye honey. Love you too.\"", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"{engine.Wife.Name} then left the house...", 50, ResetColorField, ResetColorField);
            Console.Clear();
            delayedText($"However, upon closer observation... even though {engine.Husband.Name} claimed he had work to do out of town, he showed no signs of leaving. ", 50, ResetColorField, ResetColorField);
            delayedText("Instead, he was quite relaxed that day, constantly using his smartphone, texting someone.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"Occasionally, he would chuckle while typing. " +
                $"The clock struck 9:00 AM, but there were no signs that the husband was going anywhere.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"He was still texting to someone on a sofa in the living room, while {engine.MainCharacterCat.Name} was observing him from its position. ", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name} started to feel curious. What was {engine.Husband.Name} actually doing?", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            Console.Clear();

            delayedText($"{engine.MainCharacterCat.Name} decided to observe {engine.Husband.Name}'s phone. It wanted to do this by approaching him on a sofa and then sitting on his lap.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            Console.WriteLine("\n===========================================================================");
            Console.WriteLine($"MISSION: {engine.MainCharacterCat.Name} wanted to investigate the husband.");
            Console.WriteLine($"Find where {engine.Husband.Name} sat, and observe his phone");
            Console.WriteLine("===========================================================================\n");

            do
            {
                exploreHouse(engine);
            } while (ValidateActionResult == false);

            ValidateActionResult = false;
            Console.Clear();
            delayedText($"There was a woman's name displayed on the husband's phone screen, and {engine.Husband.Name} was texting to that person. ", 50, ResetColorField, ResetColorField);
            delayedText($"Give this woman a name.", 50, ResetColorField, ResetColorField);
            getName(engine.Mistress, "mistress");
            Console.WriteLine();
            Console.WriteLine("=======================================================");
            Console.WriteLine($"ONLINE CHAT, (By {engine.MainCharacterCat.Name}'s perspective)");
            Console.WriteLine("=======================================================\n");
            engine.Husband.displayDialogue($"\"Babe, my wife isn't home this time, outstation. Want to meet up?\"", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            engine.Mistress.displayDialogue($"\"Oh really? Yayyy! Where do you want to meet? The cafe we always go to?\"", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"{engine.MainCharacterCat.Name} was puzzled. Who is \"{engine.Mistress.Name}\"? And what was {engine.Husband.Name} doing?", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            engine.Husband.displayDialogue($"\"I'm ok with that...\"", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            engine.Mistress.displayDialogue($"\"Actually, I’m craving that cafe's white chocolate macadamia cake. You know, the cafe that we frequently go to... That’s my favorite...\"", 50, ResetColorField, ResetColorField);
            engine.Husband.displayDialogue($"\"Speaking of cake, have you forgotten that I know how to make cakes too? I told you before that I’m good at baking.\"", 50, ResetColorField, ResetColorField);
            engine.Mistress.displayDialogue($"\"Really babe?\"", 50, ResetColorField, ResetColorField);
            engine.Husband.displayDialogue($"\"Yes, seriously, I can bake. And I have an idea. Instead of us going to the cafe, how about I bake a white chocolate macadamia cake for you, " +
                $"and you come over to eat at my place? You’ve never been to my house, right?\"", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            engine.Mistress.displayDialogue($"\"Wait, are you serious? Inviting me to the house? What if your wife finds out?\"", 50, ResetColorField, ResetColorField);
            engine.Husband.displayDialogue($"\"Relax babe. Do you want it or not? I honestly want to bake for you... let me bake this morning, you come over in the evening, have some cake, " +
                $"and then perhaps we can watch Netflix together...\"", 50, ResetColorField, ResetColorField);
            engine.Husband.displayDialogue($"\"I really wanna see you\"", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            engine.Mistress.displayDialogue($"\"Umm, yeah, sounds interesting. Is it the same location you shared before? What time can I come?\"", 50, ResetColorField, ResetColorField);
            engine.Husband.displayDialogue($"\"Yes same location. Is 3 PM okay?\"", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            engine.Mistress.displayDialogue($"\"3 PM is perfect. See you there.. ;)\"", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            Console.Clear();
            delayedText($".....", 70, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name} was stunned. It had to process several shocking facts:", 50, ResetColorField, ResetColorField);
            delayedText($"1. {engine.Husband.Name} was cheating; he has another woman named \"{engine.Mistress.Name}\".", 50, ResetColorField, ResetColorField);
            delayedText($"2. {engine.Husband.Name} seems to have had a secret relationship with {engine.Mistress.Name} for a while (based on the phrase \"the cafe we always go to\").", 50, ResetColorField, ResetColorField);
            delayedText($"3. {engine.Wife.Name} ({engine.Husband.Name}'s wife), knew nothing about this.", 50, ResetColorField, ResetColorField);
            delayedText($"4. {engine.Husband.Name} was planning to meet up with {engine.Mistress.Name} IN THIS HOUSE", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"End of Scene 1...", 50, ResetColorField, ResetColorField);
        }
    }

    internal class Scene2 : Scene //OOP Concept applied: INHERITANCE (Scene2 is a child class that inherits from the abstract class Scene, and provides specific implementation for the playScene method and validateAction methods)
    {
        public Scene2(string givenName)
        {
            this.Name = givenName;
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override bool validateAction(SecondaryItem item, ActionType action)
        {
            bool isValid = false;
            if (item.Name == "Car Key" && action == ActionType.Grab)
            {
                isValid = true;
            }
            return isValid;
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
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
            Console.WriteLine($"\n[Current Location: {engine.MainCharacterCat.CurrentLocation?.Name ?? "Not set"}]");
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
                    engine.House.displayMap();
                    Console.WriteLine("===========================================================================");
                    Console.WriteLine("\n");
                }
                else if (input == 'C')
                {
                    Console.WriteLine("\n");
                    Console.WriteLine($"[Current Location: {engine.MainCharacterCat.CurrentLocation.Name}]\n");
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
                        engine.MainCharacterCat.CurrentLocation = selectedRoom;
                    }
                    else
                    {
                        Console.WriteLine("Invalid room selection.");
                    }
                }
                else if (input == 'E')
                {
                    var currentRoom = engine.MainCharacterCat.CurrentLocation;
                    if (currentRoom == null || currentRoom.ItemsAvailable.Count == 0)
                    {
                        Console.WriteLine("There is nothing to explore here.");
                        return;
                    }

                    Console.WriteLine($"\n");
                    Console.WriteLine($"Available items/furnitures in {currentRoom.Name}:\n");
                    displayItemsAvailable(currentRoom);
                    Console.Write("Select item number to inspect: ");

                    if (int.TryParse(Console.ReadLine(), out int pItemChoice) && pItemChoice >= 1 && pItemChoice <= currentRoom.ItemsAvailable.Count)
                    {
                        var selectedPrimary = currentRoom.ItemsAvailable[pItemChoice - 1] as PrimaryItem;
                        if (selectedPrimary.Name == "Laundry Basket with stack of clothes")
                        {
                            delayedText($"Congratulations,your guess is correct... ", 30, ResetColorField, ResetColorField);
                            delayedText($"Selected Item: {selectedPrimary.Name} ", 30, ResetColorField, ResetColorField);
                            delayedText($"Going to {selectedPrimary.Name} .....", 50, ResetColorField, ResetColorField);
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
                                    delayedText($"Wrong action....but the item you just interacted with is correct", 30, ResetColorField, ResetColorField);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid action selection.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("This is not a suitable place to hide the car key....");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Wrong primary item!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Please try again");
            }
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override void playScene(GameEngine engine)
        {
            engine.MainCharacterCat.CurrentLocation = engine.HouseSpaceList[5];
            string scene2 = @"
            =======================================================
                            SCENE 2: THE MEETUP
            =======================================================
            ";
            delayedText(scene2, 10, Tangerine, ResetColorField);
            engine.Husband.displayDialogue($"\"get up {engine.MainCharacterCat.Name}. Papa wants to shower...\"", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name} finally snapped out of his shock. He jumped down from the sofa to the floor.", 50, ResetColorField, ResetColorField);
            engine.Husband.displayDialogue($"\"Shower, then go buy groceries, then bake the cake, then she comes over... wow, it's gonna be a great day\"", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name} knew he had to stop this meeting. He thought of hiding the car keys. ", 50, ResetColorField, ResetColorField);
            Console.WriteLine("\n===========================================================================");
            Console.WriteLine($"MISSION: Find {engine.Husband.Name}'s car key");
            Console.WriteLine($"Hint: It is located on a thing, that people always put another things on it");
            Console.WriteLine("===========================================================================\n");
            do
            {
                exploreHouse(engine);
            } while (ValidateActionResult == false);
            ValidateActionResult = false;
            Console.Clear();
            delayedText($"Congratulations, you found {engine.Husband.Name}'s car key.", 50, ResetColorField, ResetColorField);
            delayedText($"Now {engine.MainCharacterCat.Name} wanted to hide the car key in one of the items in the house.", 50, ResetColorField, ResetColorField);
            Console.WriteLine("\n===========================================================================");
            Console.WriteLine($"MISSION: Hide {engine.Husband.Name}'s car key...");
            Console.WriteLine($"Hint: Where do people put their dirty clothes into?");
            Console.WriteLine("===========================================================================\n");
            string missionName = "Hide the car key";
            do
            {
                exploreHouse(engine, missionName);
            } while (ValidateActionResult == false);
            ValidateActionResult = false;
            delayedText($"{engine.MainCharacterCat.Name} buried the car key deep inside a pile of dirty clothes in a laundry basket.", 50, ResetColorField, ResetColorField);
            delayedText($"Then, it returned to the living room to watch {engine.Husband.Name}'s next move. ", 50, ResetColorField, ResetColorField);
            engine.MainCharacterCat.CurrentLocation = engine.HouseSpaceList[5];
            Console.ReadLine();
            delayedText($"After showering and getting ready, {engine.Husband.Name} looked for his keys. He checked the bedside table where he usually left them. Nothing. He searched the whole room. Still nothing.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"Then, he remembered his keys had a location tracking feature. He used the app on his phone to play a sound. Beep... beep... {engine.Husband.Name} followed the sound and found his keys in the dirty laundry basket.", 50, ResetColorField, ResetColorField);
            engine.Husband.displayDialogue($"\"How did my key end up here? I don't remember putting it here...\"", 50, ResetColorField, ResetColorField);
            delayedText($"But he ignored the feeling, started the engine, and went out to buy ingredients for the cake.", 50, ResetColorField, ResetColorField);
            delayedText($"End of Scene 2...", 50, ResetColorField, ResetColorField);
        }
    }

    internal class Scene3 : Scene //OOP Concept applied: INHERITANCE (Scene3 is a child class that inherits from the abstract class Scene, and provides specific implementation for the playScene method)
    {
        public Scene3(string givenName)
        {
            this.Name = givenName;
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override bool validateAction(SecondaryItem item, ActionType action)
        {
            if (item.Name == "Sack Opening" && action == ActionType.Claw)
            {
                return true;
            }

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

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override void exploreHouse(GameEngine engine)
        {
            ValidateActionResult = false;

            Console.WriteLine($"\n[Current Location: {engine.MainCharacterCat.CurrentLocation?.Name ?? "Not set"}]");
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
                    engine.House.displayMap();
                    Console.WriteLine("===========================================================================");
                    Console.WriteLine("\n");
                }
                else if (input == 'C')
                {
                    Console.WriteLine("\n");
                    Console.WriteLine($"[Current Location: {engine.MainCharacterCat.CurrentLocation.Name}]\n");
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
                        engine.MainCharacterCat.CurrentLocation = selectedRoom;
                    }
                    else
                    {
                        Console.WriteLine("Invalid room selection.");
                    }
                }
                else if (input == 'E')
                {
                    var currentRoom = engine.MainCharacterCat.CurrentLocation;
                    if (currentRoom == null || currentRoom.ItemsAvailable.Count == 0)
                    {
                        Console.WriteLine("There is nothing to explore here.");
                        return;
                    }

                    Console.WriteLine($"\n");
                    Console.WriteLine($"Available items/furnitures in {currentRoom.Name}:\n");
                    displayItemsAvailable(currentRoom);
                    Console.Write("Select item number to inspect: ");

                    if (int.TryParse(Console.ReadLine(), out int pItemChoice) && pItemChoice >= 1 && pItemChoice <= currentRoom.ItemsAvailable.Count)
                    {
                        var selectedPrimary = currentRoom.ItemsAvailable[pItemChoice - 1] as PrimaryItem;

                        if (selectedPrimary == null)
                        {
                            Console.WriteLine("This item cannot be inspected.");
                            return;
                        }

                        delayedText($"Selected Item: {selectedPrimary.Name} ", 30, ResetColorField, ResetColorField);
                        delayedText($"Going to {selectedPrimary.Name} .....", 50, ResetColorField, ResetColorField);

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
                            delayedText($"Selected Item: {selectedSecondary.Name} ", 30, ResetColorField, ResetColorField);

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

                                ValidateActionResult = validateAction(selectedSecondary, selectedAction);

                                if (ValidateActionResult == false)
                                {
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
                    Console.WriteLine("Invalid input! Please press M, E, or C.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Please try again");
            }
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override void playScene(GameEngine engine)
        {
            string scene3 = @"
             =======================================================
                          SCENE 3: THE KIBBLE CHAOS
             =======================================================
            ";
            delayedText(scene3, 10, Tangerine, ResetColorField);
            var garageSpace = engine.HouseSpaceList.FirstOrDefault(space => space.Name == "Garage");
            if (garageSpace != null)
            {
                var husbandCar = garageSpace.ItemsAvailable.FirstOrDefault(item => item.Name == "Husband Car");
                if (husbandCar != null)
                {
                    garageSpace.ItemsAvailable.Remove(husbandCar);
                }
            }

            delayedText($"{engine.MainCharacterCat.Name} said to himself, \"Oh, that wasn't enough. I need to do something else to cancel this meeting...\"", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name} roamed the house looking for another distraction.", 50, ResetColorField, ResetColorField);
            delayedText($"MISSION: Find the new sack of food in the Garage and create a distraction!", 50, Tangerine, ResetColorField);

            bool scene3Completed = false;

            while (!scene3Completed)
            {
                exploreHouse(engine);

                if (engine.MainCharacterCat.CurrentLocation.Name == "Garage" && ValidateActionResult == true)
                {
                    delayedText($"\nIn the garage, {engine.MainCharacterCat.Name} saw the brand new 5kg sack of cat food.", 50, ResetColorField, ResetColorField);
                    delayedText("An idea struck in its mind", 50, Tangerine, ResetColorField);

                    TearBagAnimation(Tangerine, ResetColorField);

                    delayedText($"{engine.MainCharacterCat.Name} clawed at the sack aggressively until it tore open!", 50, ResetColorField, ResetColorField);
                    delayedText($"It then scattered the kibble all over the garage floor, creating a massive mess to delay the date.", 50, ResetColorField, ResetColorField);

                    delayedText("\nMISSION ACCOMPLISHED: The garage is now a kibble minefield.", 60, Tangerine, ResetColorField);

                    scene3Completed = true;
                }
            }
        }

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
                Console.SetCursorPosition(0, 5);
                Console.WriteLine(color + frame + reset);
                Thread.Sleep(400);
            }
            Console.WriteLine("\n *ZRUPPPP* \n");
            Thread.Sleep(500);
        }
    }

    internal class Scene4 : Scene ////OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
    {
        public Scene4(string givenName)
        {
            this.Name = givenName;
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override bool validateAction(SecondaryItem item, ActionType action)
        {
            bool isValid = false;
            if (item.Name == "Grocery Bag" && action == ActionType.Shove)
            {
                isValid = true;
            }
            return isValid;
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override void playScene(GameEngine engine)
        {
            string scene4 = @"
             =======================================================
                          SCENE 4: THE EGGS & THE CAGE
             =======================================================
            ";

            var garage = engine.HouseSpaceList.FirstOrDefault(space => space.Name == "Garage");
            var husbandCar = garage.ItemsAvailable.FirstOrDefault(item => item.Name == "HusbandCar");
            if (husbandCar != null)
            {

            }

            var kitchen = engine.HouseSpaceList.FirstOrDefault(space => space.Name == "Kitchen");
            var kitchenBarTable = kitchen.ItemsAvailable.FirstOrDefault(item => item.Name == "Bar Table") as PrimaryItem;

            SecondaryItem groceryBag = new SecondaryItem("Grocery Bag", kitchen.Name);
            kitchenBarTable.AvailableSecondaryItem.Add(groceryBag);

            delayedText(scene4, 10, Tangerine, ResetColorField);

            delayedText($"Upon returning home, {engine.Husband.Name} was shocked to see cat food scattered all over the garage floor", 50, ResetColorField, ResetColorField);

            engine.Husband.displayDialogue($"\"Hah! How did the cat food bag get torn? And it's everywhere!\"", 50, ResetColorField, ResetColorField);

            Console.ReadLine();

            delayedText($"After parking, he entered the house holding the grocery bags. He saw {engine.MainCharacterCat.Name} sitting on his mat, stiff, pretending not to look. " +
                $"{engine.Husband.Name} put the groceries in the kitchen, then picked {engine.MainCharacterCat.Name} up and looked him in the eye.", 50, ResetColorField, ResetColorField);

            engine.Husband.displayDialogue($"\"{engine.MainCharacterCat.Name} did you tear the food bag and make a mess?\"", 50, ResetColorField, ResetColorField);

            Console.ReadLine();

            delayedText($"{engine.MainCharacterCat.Name} just meowed, effectively admitting it in cat language.", 50, ResetColorField, ResetColorField);

            engine.Husband.displayDialogue($"\"It must be you, {engine.MainCharacterCat.Name}." +
                $" Who else would it be?\"", 50, ResetColorField, ResetColorField);

            Console.ReadLine();

            delayedText($"{engine.Husband.Name} said sternly. Then his voice softened.", 50, ResetColorField, ResetColorField);

            engine.Husband.displayDialogue($"\"Sigh, {engine.MainCharacterCat.Name}, {engine.MainCharacterCat.Name}... why are you acting up today?\"", 50, ResetColorField, ResetColorField);

            Console.ReadLine();

            delayedText($"He put {engine.MainCharacterCat.Name} down and grabbed a broom to clean the garage.", 50, ResetColorField, ResetColorField);

            delayedText($"Seizing the oppurtunity while {engine.Husband.Name} swept the garage, it wanted to create another mess at another place.", 50, ResetColorField, ResetColorField);

            Console.Clear();

            Console.WriteLine("\n===========================================================================");
            Console.WriteLine($"MISSION: {engine.MainCharacterCat.Name} wanted to create a mess at one of the house space again.");
            Console.WriteLine($"HINT: Where do you think {engine.Husband.Name} put the grocery bag at the house?");
            Console.WriteLine("===========================================================================\n");

            engine.MainCharacterCat.CurrentLocation = engine.HouseSpaceList[5];

            do
            {
                exploreHouse(engine);
            } while (ValidateActionResult == false);

            ValidateActionResult = false;
            Console.WriteLine();
            delayedText($"It saw a carton of eggs inside the grocery bag, located on top of the bar table.", 50, ResetColorField, ResetColorField);
            Console.Clear();

            delayedText($"With all his might, it leaped and shoved the GroceryBag off the kitchen's bar table.", 50, ResetColorField, ResetColorField);

            delayedText($"SPLAT.", 50, ResetColorField, ResetColorField);

            delayedText($"{engine.Husband.Name} snapped.", 50, ResetColorField, ResetColorField);

            engine.Husband.displayDialogue($"\"{engine.MainCharacterCat.Name}!! What is wrong with you?! Argh... why are you so aggressive today? Tearing food bag, now breaking the eggs!\"",
                50, ResetColorField, ResetColorField);

            delayedText($"{engine.MainCharacterCat.Name} only replied,", 50, ResetColorField, ResetColorField);

            engine.MainCharacterCat.displayDialogue($"\"Meow.\"", 50, ResetColorField, ResetColorField);

            engine.Husband.displayDialogue($"\"Sorry but Papa has to put you in the cage for a while.\"", 50, ResetColorField, ResetColorField);

            delayedText($"{engine.MainCharacterCat.Name} was placed in the cage located in the garage. Even though the cage was spacious with two levels, {engine.MainCharacterCat.Name} was trapped." +
                $"He could no longer interfere.", 50, ResetColorField, ResetColorField);

            delayedText($"After locking the cage,{engine.Husband.Name} took out his phone and called {engine.Mistress.Name}.", 50, ResetColorField, ResetColorField);

            engine.Mistress.displayDialogue($"\"Hey babe, I had some issues earlier... I'm just starting to bake now. Can you come a bit later? Maybe 5 PM?\"", 50, ResetColorField, ResetColorField);

            engine.Husband.displayDialogue($"\"Oh, okay...\"", 50, ResetColorField, ResetColorField);

            engine.Mistress.displayDialogue($"\"Okay baby, bye...\"", 50, ResetColorField, ResetColorField);

            delayedText($"{engine.MainCharacterCat.Name} heard the conversation.", 50, ResetColorField, ResetColorField);

            engine.MainCharacterCat.displayDialogue($"\"So {engine.Mistress.Name} will arrive at 5 PM...\"", 50, ResetColorField, ResetColorField);

            delayedText($"he thought.", 50, ResetColorField, ResetColorField);

            engine.MainCharacterCat.displayDialogue($"\"There is nothing else I can do now.\"", 50, ResetColorField, ResetColorField);

            delayedText($"{engine.Husband.Name} went to the kitchen and started baking.", 50, ResetColorField, ResetColorField);

            engine.MainCharacterCat.CurrentLocation = engine.HouseSpaceList[6];

            delayedText($"End of Scene 4...", 50, ResetColorField, ResetColorField);
        }
    }

    internal class Scene5 : Scene //OOP Concept applied: INHERITANCE (Scene5 is a child class that inherits from the abstract class Scene, and provides specific implementation for the playScene method and validateAction methods)
    {
        public Scene5(string givenName)
        {
            this.Name = givenName;
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override bool validateAction(SecondaryItem item, ActionType action)
        {
            bool isValid = false;
            if (item.Name == "CCTV wire" && action == ActionType.Shove)
            {
                isValid = true;
            }
            return isValid;
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
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
            Console.WriteLine($"\n[Current Location: {engine.MainCharacterCat.CurrentLocation?.Name ?? "Not set"}]");
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
                    engine.House.displayMap();
                    Console.WriteLine("===========================================================================");
                    Console.WriteLine("\n");
                }
                else if (input == 'C')
                {
                    Console.WriteLine("\n");
                    Console.WriteLine($"[Current Location: {engine.MainCharacterCat.CurrentLocation.Name}]\n");
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
                        engine.MainCharacterCat.CurrentLocation = selectedRoom;
                    }
                    else
                    {
                        Console.WriteLine("Invalid room selection.");
                    }
                }
                else if (input == 'E')
                {
                    var currentRoom = engine.MainCharacterCat.CurrentLocation;
                    if (currentRoom == null || currentRoom.ItemsAvailable.Count == 0)
                    {
                        Console.WriteLine("There is nothing to explore here.");
                        return;
                    }

                    Console.WriteLine($"\n");
                    Console.WriteLine($"Available items/furnitures in {currentRoom.Name}:\n");
                    displayItemsAvailable(currentRoom);
                    Console.Write("Select item number to inspect: ");

                    if (int.TryParse(Console.ReadLine(), out int pItemChoice) && pItemChoice >= 1 && pItemChoice <= currentRoom.ItemsAvailable.Count)

                    {
                        var selectedPrimary = currentRoom.ItemsAvailable[pItemChoice - 1] as PrimaryItem;
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Please try again");
            }
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override void playScene(GameEngine engine)
        {
            string scene5 = @"
             =======================================================
                        SCENE 5: THE SHOWDOWN & THE CAMERA
             =======================================================
            ";

            engine.MainCharacterCat.CurrentLocation = engine.HouseSpaceList[5];

            delayedText(scene5, 10, Tangerine, ResetColorField);
            delayedText("At 5:07 PM, a blue car arrived and parked in front of the gate.", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name} watched as {engine.Husband.Name} opened the gate. The car pulled into the garage. A woman stepped out.", 50, ResetColorField, ResetColorField);
            delayedText($"\"Baby..!\" said {engine.Husband.Name}.", 50, ResetColorField, ResetColorField);
            delayedText("\"Yeah baby...... wow, nice house, eh\" the woman replied.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"\"Come inside. Are you ready to taste my White Chocolate Macadamia cake?\"", 50, ResetColorField, ResetColorField);
            delayedText($"\"Ready! I hope it tastes really good. Eh, a cat! You have a cat too?\" the woman asked, pointing at the cage.", 50, ResetColorField, ResetColorField);
            delayedText($"\"{engine.Husband.Name} walked to the cage, unlocked it, picked {engine.MainCharacterCat.Name} up, and brought him to {engine.Mistress.Name}.\"", 50, ResetColorField, ResetColorField);
            delayedText($"\"I bring my cats too. I just picked them up from the grooming service,\" the woman said, petting {engine.MainCharacterCat.Name}'s head.", 50, ResetColorField, ResetColorField);
            delayedText($"She went to her car and brought out two Persian cats, one is grey and one is white.", 50, ResetColorField, ResetColorField);

            delayedText("Let's put a name for the grey cat... ", 50, ResetColorField, ResetColorField);
            getName(engine.GreyCat, "grey cat");

            delayedText("Let's put a name for the white cat... ", 50, ResetColorField, ResetColorField);
            getName(engine.WhiteCat, "white cat");

            delayedText($"\"The grey one is {engine.GreyCat.Name}, and the white one is {engine.WhiteCat.Name},\" she said.", 50, ResetColorField, ResetColorField);
            delayedText($"\"Wow, you got very pretty cats there,\" said {engine.Husband.Name}.", 50, ResetColorField, ResetColorField);
            delayedText($"\"Thank you...\" said {engine.Mistress.Name}.", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name} confirmed it. This woman was definitely {engine.Mistress.Name}.", 50, ResetColorField, ResetColorField);
            delayedText($"\"Okay, let's go inside. You can bring your cat in.\"", 50, ResetColorField, ResetColorField);

            Thread.Sleep(1500);
            delayedText("...", 200, ResetColorField, ResetColorField);

            delayedText($"The atmosphere in the living room was romantic. {engine.Husband.Name} and {engine.Mistress.Name} sat close on the sofa, enjoying the freshly baked cake while watching a movie on Netflix.", 50, ResetColorField, ResetColorField);
            delayedText($"\"It's delicious, I didn't expect you could bake,\" {engine.Mistress.Name} praised, feeding a piece of cake to {engine.Husband.Name}.", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name} watched from his mat with a restless tail. His eyes were fixed on the TV cabinet.", 50, ResetColorField, ResetColorField);
            delayedText($"Behind that cabinet was the main switch for the Smart Home CCTV. Before {engine.Mistress.Name} arrived, {engine.Husband.Name} had turned off the switch so the camera would be \"Offline.\" {engine.MainCharacterCat.Name} knew this because the small blue light on the camera was off.", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name} knew that if the switch was pressed again, the camera would reactivate, and a notification would be sent to {engine.Wife.Name}'s phone: \"CCTV Living Room is Online\".", 50, ResetColorField, ResetColorField);
            Console.WriteLine("\n===========================================================================");
            Console.WriteLine($"MISSION: GO TO THE BACK OF THE TV CABINET.");
            Console.WriteLine("===========================================================================\n");
            string missionName = "GO TO THE BACK OF THE TV CABINET";
            do
            {
                exploreHouse(engine, missionName);
            } while (ValidateActionResult == false);
            ValidateActionResult = false;
            delayedText($"[CHECKPOINT] {engine.MainCharacterCat.Name} began to move. He walked slowly, trying to approach the TV cabinet.", 50, ResetColorField, ResetColorField);
            delayedText($"However, his movement was detected by {engine.WhiteCat.Name} and {engine.GreyCat.Name}. They jumped down from the sofa and blocked {engine.MainCharacterCat.Name}'s path.", 50, ResetColorField, ResetColorField);
            delayedText($"They weren't just blocking him; they were guarding their new \"master's\" territory. {engine.GreyCat.Name} hissed loud, its fur standing on end, making it look twice {engine.MainCharacterCat.Name}'s size.", 50, ResetColorField, ResetColorField);
            delayedText($"\"Meow!\" (Move!), {engine.MainCharacterCat.Name} warned. {engine.GreyCat.Name} replied with a swift swipe of its claws, nicking {engine.MainCharacterCat.Name}'s left ear. A drop of blood fell.", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name}'s patience snapped. He remembered {engine.Wife.Name}'s gentle pets, the food she gave, the love she poured out. He would not let this house be taken over by intruders.", 50, ResetColorField, ResetColorField);

            bool completeCombat1 = false;
            while (!completeCombat1)
            {
                delayedText($"[MISSION 1: COMBAT INITIATED] {engine.MainCharacterCat.Name} VS {engine.GreyCat.Name}", 30, ResetColorField, ResetColorField);
                bool wonFight1 = CombatLoop(engine.MainCharacterCat, engine.GreyCat);

                if (!wonFight1)
                {
                    delayedText($"[GAME OVER] {engine.MainCharacterCat.Name} was defeated... Restarting from the checkpoint...", 50, ResetColorField, ResetColorField);
                    Thread.Sleep(2000);
                    engine.MainCharacterCat.HP = 80;
                    engine.GreyCat.HP = 80;
                    continue;
                }
                completeCombat1 = true;
            }

            delayedText($"{engine.GreyCat.Name} is severely weakened and scurries away! Realizing who the true \"Alpha\" was, {engine.GreyCat.Name} scurried away to hide behind the dining table, trembling in fear.", 50, ResetColorField, ResetColorField);
            delayedText($"But the fight wasn't over. {engine.WhiteCat.Name} suddenly ambushed {engine.MainCharacterCat.Name} from behind!", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name} gain some HP", 50, ResetColorField, ResetColorField);
            engine.MainCharacterCat.HP += 40;

            bool completeCombat2 = false;
            while (!completeCombat2)
            {
                delayedText($"[MISSION 2: COMBAT INITIATED] {engine.MainCharacterCat.Name} VS {engine.WhiteCat.Name}", 30, ResetColorField, ResetColorField);
                bool wonFight2 = CombatLoop(engine.MainCharacterCat, engine.WhiteCat);

                if (!wonFight2)
                {
                    delayedText($"[GAME OVER] {engine.MainCharacterCat.Name} was defeated... Restarting from the checkpoint...", 50, ResetColorField, ResetColorField);
                    Thread.Sleep(2000);
                    engine.MainCharacterCat.HP = 80;
                    engine.GreyCat.HP = 80;
                    continue;
                }
                completeCombat2 = true;
            }

            delayedText($"{engine.WhiteCat.Name} was severely weakened and scurries away!", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.WhiteCat.Name} immediately retreated, sliding under the sofa to join its sibling.", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.MainCharacterCat.Name} stood tall, chest heaving, scanning the room with fiery eyes. {engine.MainCharacterCat.Name} Wins!", 50, ResetColorField, ResetColorField);

            delayedText($"Without wasting time, {engine.MainCharacterCat.Name} ran back to the TV cabinet. He saw the CCTV wire hanging loose.", 50, ResetColorField, ResetColorField);
            Console.WriteLine("\n===========================================================================");
            Console.WriteLine($"MISSION: GO TO THE BACK OF THE TV CABINET.");
            Console.WriteLine("===========================================================================\n");





            do
            {
                exploreHouse(engine);
            } while (ValidateActionResult == false);
            ValidateActionResult = false;
            bool isCameraOnline = false;
            while (!isCameraOnline)
            {
                isCameraOnline = PlugInWireMiniGame(engine.MainCharacterCat);

                if (!isCameraOnline)
                {
                    delayedText($"{engine.MainCharacterCat.Name} shook off the failure and gathered his strength to try again...", 50, "\x1b[38;2;255;153;51m", ResetColorField);
                    Thread.Sleep(1000);
                }
            }
            Console.Clear();
            Thread.Sleep(1500);
            delayedText("...", 200, ResetColorField, ResetColorField);

            delayedText($"Meanwhile, hundreds of kilometers away: {engine.Wife.Name}'s smartphone dinged. A notification appeared: [Smart Home]: Living Room Camera is now ONLINE.", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.Wife.Name}, resting in her hotel room, was confused. \"Huh? Was the CCTV offline earlier?\" She opened the app to see what was happening. Her heart stopped. On the screen, she clearly saw {engine.Husband.Name} sitting with a strange woman on their sofa.", 50, ResetColorField, ResetColorField);
            delayedText($"Without hesitating, {engine.Wife.Name} pressed the Screenshot button.", 50, ResetColorField, ResetColorField);

            delayedText("Ring... Ring...", 100, ResetColorField, ResetColorField);
            delayedText($"{engine.Husband.Name}'s phone on the coffee table rang. The name \"Wife\" flashed on the screen. {engine.Husband.Name} signaled {engine.Mistress.Name} to be quiet. He picked up the phone, feigning a calm voice.", 50, ResetColorField, ResetColorField);
            delayedText($"\"Hello honey... why are you calling? I was just about to sleep, pretty tired.\"", 50, ResetColorField, ResetColorField);
            delayedText($"{engine.Wife.Name} asked in a voice that was terrifyingly calm, \"Where are you?\"", 50, ResetColorField, ResetColorField);
            delayedText($"\"At the hotel, honey. Like I said, I'm outstation too. Just got out of the shower. Are you okay?\" {engine.Husband.Name} lied without guilt.", 50, ResetColorField, ResetColorField);
            delayedText($"\"Oh... at the hotel...\" {engine.Wife.Name} replied. \"Open WhatsApp for a second.\"", 50, ResetColorField, ResetColorField);
            delayedText($"\"Why?\"", 50, ResetColorField, ResetColorField);
            delayedText($"\"Just open it.\"", 50, ResetColorField, ResetColorField);

            delayedText($"The line was still connected. {engine.Husband.Name} pulled the phone away from his ear and opened WhatsApp. A picture message had just come in.", 50, ResetColorField, ResetColorField);
            delayedText($"It was a screenshot of him and {engine.Mistress.Name} sitting on the sofa, taken from the CCTV angle, one minute ago. Below the picture, there was a short sentence typed in capital letters:", 50, ResetColorField, ResetColorField);
            delayedText($"\"THEN WHAT IS THIS?\"", 100, ResetColorField, ResetColorField);

            delayedText($"{engine.Husband.Name}'s face went pale. The blood drained from his head. The phone nearly slipped from his hand. He looked up at the CCTV in the corner of the ceiling, which was now glowing with a steady blue light.", 50, ResetColorField, ResetColorField);
            delayedText($"\"Honey... I... I can explain...\" His voice trembled.", 50, ResetColorField, ResetColorField);

            delayedText("[SCENE 5 COMPLETE]", 30, ResetColorField, ResetColorField);
        }

        private bool PlugInWireMiniGame(MainCharacter playerCat)
        {
            Console.Clear();
            Console.WriteLine("=======================================================");
            Console.WriteLine("                 MINI-GAME INITIATED                   ");
            Console.WriteLine("=======================================================\n");
            delayedText($"Action: {playerCat.Name} bit the wire and pulled it toward the socket.", 30, ResetColorField, ResetColorField);
            Console.WriteLine("It's tough! You need to use your entire body weight to shove it in!");
            Console.WriteLine("\nINSTRUCTIONS:");
            Console.WriteLine("Mash the [SPACEBAR] repeatedly to build momentum!");
            Console.WriteLine("You have 5 seconds to fill the progress bar.");
            Console.WriteLine("\nPress ENTER when you are ready...");
            Console.ReadLine();

            int targetPresses = 25;
            int currentPresses = 0;
            int timeLimitSeconds = 5;

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            while (Console.KeyAvailable) Console.ReadKey(true);

            while (timer.Elapsed.TotalSeconds < timeLimitSeconds && currentPresses < targetPresses)
            {
                DrawProgressBar(currentPresses, targetPresses, timeLimitSeconds - (int)timer.Elapsed.TotalSeconds);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Spacebar)
                    {
                        currentPresses++;
                    }
                }
                Thread.Sleep(15);
            }
            DrawProgressBar(currentPresses, targetPresses, Math.Max(0, timeLimitSeconds - (int)timer.Elapsed.TotalSeconds));
            timer.Stop();
            Console.WriteLine("\n");

            if (currentPresses >= targetPresses)
            {
                delayedText("SUCCESS!", 20, "\x1b[32m", ResetColorField);
                delayedText($"Using all your strength, {playerCat.Name} shove the plug back into the wall outlet!", 40, ResetColorField, ResetColorField);
                delayedText("Click.", 50, ResetColorField, ResetColorField);
                delayedText("The light on the ceiling camera blinked red, then turned solid blue. ONLINE.", 50, ResetColorField, ResetColorField);
                return true;
            }
            else
            {
                delayedText("FAILED!", 20, "\x1b[31m", ResetColorField);
                delayedText("Oof! Your paws slip on the floor. The heavy plug falls out of the socket.", 40, ResetColorField, ResetColorField);
                delayedText("You need to try again!", 40, ResetColorField, ResetColorField);
                return false;
            }
        }

        private void DrawProgressBar(int current, int target, int timeLeft)
        {
            int barSize = 25;
            int progress = (int)((double)current / target * barSize);

            if (progress > barSize) progress = barSize;

            string filled = new string('█', progress);
            string empty = new string('-', barSize - progress);

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
                        delayedText($"{playerCat.Name} uses {Cat.FightingOptions.Claw}! Deals {damageDealt} damage.", 20, ResetColorField, ResetColorField);
                        break;
                    case "2":
                        damageDealt = rng.Next(10, 31);
                        delayedText($"{playerCat.Name} uses {Cat.FightingOptions.Kick}! Deals {damageDealt} damage.", 20, ResetColorField, ResetColorField);
                        break;
                    case "3":
                        if (rng.Next(0, 100) < 30)
                        {
                            delayedText($"{playerCat.Name} uses {Cat.FightingOptions.Bite}... but misses!", 20, ResetColorField, ResetColorField);
                        }
                        else
                        {
                            damageDealt = rng.Next(20, 41);
                            delayedText($"{playerCat.Name} lands a devastating {Cat.FightingOptions.Claw}! Deals {damageDealt} damage.", 20, ResetColorField, ResetColorField);
                        }
                        break;
                    default:
                        delayedText("Invalid move! You lost your turn.", 20, ResetColorField, ResetColorField);
                        break;
                }

                enemyCat.HP -= damageDealt;

                if (enemyCat.HP <= 2) break;

                int enemyDamage = rng.Next(10, 25);
                playerCat.HP -= enemyDamage;
                delayedText($"{enemyCat.Name} strikes back! Deals {enemyDamage} damage to {playerCat.Name}.", 20, ResetColorField, ResetColorField);
            }

            return playerCat.HP > 0;
        }
    }

    internal class Scene6 : Scene ////OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
    {
        public Scene6(string givenName)
        {
            this.Name = givenName;
        }

        //OOP Concept applied: POLYMORPHISM (Method Overriding - specific implementation for this scene, different from other scenes)
        public override void playScene(GameEngine engine)
        {
            string scene6Banner = @"
            =======================================================
                        SCENE 6: A NEW BEGINNING
            =======================================================
        ";

            delayedText(scene6Banner, 10, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText("One Month Later...", 100, ResetColorField, ResetColorField);
            Console.ReadLine();
            Console.Clear();

            delayedText($"The atmosphere in the new apartment still felt foreign.", 50, ResetColorField, ResetColorField);
            delayedText($"The smell of fresh paint mixed with the scent of cardboard boxes that hadn't been fully unpacked.", 50, ResetColorField, ResetColorField);
            delayedText($"This living room was smaller than the old house, but for some reason, the air felt lighter and less suffocating.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();

            delayedText($"{engine.MainCharacterCat.Name} sat on top of a box, staring out the window at a cityscape he didn't recognize.", 50, ResetColorField, ResetColorField);
            delayedText($"He no longer saw the garden of the old house. Only tall buildings.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();

            delayedText($"{engine.MainCharacterCat.Name} recalled who {engine.Mistress.Name} really was.", 50, ResetColorField, ResetColorField);
            delayedText($"During the huge argument on the night of the incident, it was revealed that {engine.Mistress.Name} was actually {engine.Husband.Name}'s old friend from university.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"They had 'found' each other again on social media three months ago.", 50, ResetColorField, ResetColorField);
            delayedText($"It started with liking pictures, then commenting, and finally led to secret meetings at {engine.Husband.Name}'s favorite cafe—", 50, ResetColorField, ResetColorField);
            delayedText($"the same cafe where {engine.Husband.Name} had taken {engine.Wife.Name} when they first started dating.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"Turns out, {engine.Husband.Name} was trying to relive his old romance, but with a different woman.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();

            delayedText($"{engine.Mistress.Name} wasn't a total stranger; she was the past that {engine.Husband.Name} chose to make his future,", 50, ResetColorField, ResetColorField);
            delayedText($"destroying the present he had built with {engine.Wife.Name}.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();

            delayedText($"{engine.MainCharacterCat.Name} meowed softly. His heart felt heavy.", 70, ResetColorField, ResetColorField);
            Console.ReadLine();

            delayedText($"Truthfully, {engine.MainCharacterCat.Name} was sad. What cat wouldn't be sad seeing his family broken apart?", 50, ResetColorField, ResetColorField);
            delayedText($"He missed the times {engine.Husband.Name} stroked his head while watching football.", 50, ResetColorField, ResetColorField);
            delayedText($"He missed the couple's laughter that once filled the living room.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();

            delayedText($"But {engine.MainCharacterCat.Name} knew he couldn't let the deception continue.", 50, ResetColorField, ResetColorField);
            delayedText($"He couldn't bear to see {engine.Wife.Name}—the owner who loved him the most, who fed him, who nursed him when he was sick—living in a lie.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"To {engine.MainCharacterCat.Name}, loyalty was everything.", 70, ResetColorField, ResetColorField);
            delayedText($"If the Head of the House was willing to betray that trust, he didn't deserve to be part of the family anymore.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"Let this home be a little quieter, as long as there was no more betrayal.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            Console.Clear();

            delayedText($"The door opened.", 70, ResetColorField, ResetColorField);
            delayedText($"{engine.Wife.Name} walked in.", 50, ResetColorField, ResetColorField);
            delayedText($"Her face looked calmer than it had in weeks, even though her eyes were still slightly puffy.", 50, ResetColorField, ResetColorField);
            delayedText($"She saw {engine.MainCharacterCat.Name} sitting quietly on the box by the window.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();

            engine.Wife.displayDialogue($"\"{engine.MainCharacterCat.Name}...\"", 80, ResetColorField, ResetColorField);
            Console.ReadLine();

            delayedText($"{engine.MainCharacterCat.Name} trotted over to {engine.Wife.Name}, rubbing his body gently against her legs.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();

            delayedText($"{engine.Wife.Name} picked {engine.MainCharacterCat.Name} up and hugged him tight.", 50, ResetColorField, ResetColorField);
            engine.Wife.displayDialogue($"\"Now it's just the two of us, {engine.MainCharacterCat.Name}.\"", 60, ResetColorField, ResetColorField);
            engine.Wife.displayDialogue($"\"Thank you for 'telling' Mama that day. If you hadn't... who knows how long I would have been fooled.\"", 60, ResetColorField, ResetColorField);
            Console.ReadLine();

            delayedText($"{engine.Wife.Name} kissed {engine.MainCharacterCat.Name} softly on the head.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();
            delayedText($"Outside the window, a light rain began to fall...", 70, ResetColorField, ResetColorField);
            delayedText($"...as if washing away all the dirt and bitter memories of the old house,", 50, ResetColorField, ResetColorField);
            delayedText($"giving them both a chance to start a new life.", 50, ResetColorField, ResetColorField);
            Console.ReadLine();

            delayedText($"{engine.MainCharacterCat.Name} closed his eyes, feeling safe in his owner's arms.", 60, ResetColorField, ResetColorField);
            delayedText($"He knew he had done the right thing.", 80, ResetColorField, ResetColorField);
            Console.ReadLine();
            Console.Clear();

            string ending = @"
            =======================================================

                            ~ T H E   E N D ~

                Thank you for playing The Feline Witness.

            =======================================================
        ";
            delayedText(ending, 40, Tangerine, ResetColorField);
            Console.ReadLine();
        }
    }
}