using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Scene4_GroupProjectOOP.Cat;

namespace Scene4_GroupProjectOOP
{
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

        public void chooseAction(SecondaryItem item)
        {
            while (true)
            {
                Console.WriteLine($"\nWhat do you want to do with {item.Name}?");

                var actions = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();

                for (int i = 0; i < actions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {actions[i]}");
                }

                Console.Write("Enter choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice) &&
                    choice >= 1 && choice <= actions.Count)
                {
                    ActionType selectedAction = actions[choice - 1];

                    if (validateAction(item, selectedAction))
                    {
                        Console.WriteLine("Action success! Mess created 😼");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid action! Try again.\n");
                    }
                }
            }
        }

        public override void exploreHouse(GameEngine engine)  // move to kitchen, go to kitchen bar table, choose grocery bag to interact
        {
            // add GroceryBag
            
            HouseSpace selectedRoom = null;

            while (true)
            {
                // player pilih tempat buat mess (kitchen)
                Console.Write("Choose a house space to create a mess.");
                Console.WriteLine();

                // display list house space
                for (int i = 0; i < engine.HouseSpaceList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {engine.HouseSpaceList[i].Name}");
                }

                Console.Write("Enter choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice) &&
                    choice >= 1 && choice <= engine.HouseSpaceList.Count)
                {
                    selectedRoom = engine.HouseSpaceList[choice - 1];

                    if (selectedRoom.Name.ToLower() == "kitchen")
                    {
                        Console.WriteLine("Correct! You go to the kitchen.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Wrong place! Try again.\n");
                    }
                }
            }

            // nk ke mana (kitchen bar table)
            PrimaryItem selectedPrimary = null;

            while (true)
            {
                Console.WriteLine($"\nWhere in {selectedRoom.Name}?");

                for (int i = 0; i < selectedRoom.itemsAvailable.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {selectedRoom.itemsAvailable[i].Name}");
                }

                Console.Write("Enter choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice) &&
                    choice >= 1 && choice <= selectedRoom.itemsAvailable.Count)
                {
                    selectedPrimary = selectedRoom.itemsAvailable[choice - 1] as PrimaryItem;

                    if (selectedPrimary.Name.ToLower() == "bar table")
                    {
                        Console.WriteLine("Correct! You go to the bar table.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Wrong area! Try again.\n");
                    }
                }
            }

            // interact dgn apa (grocery bag)
            var selectedSecondary = default(SecondaryItem);

            while (true)
            {
                Console.WriteLine($"\nWhat item at {selectedPrimary.Name}?");

                for (int i = 0; i < selectedPrimary.AvailableSecondaryItem.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {selectedPrimary.AvailableSecondaryItem[i].Name}");
                }

                Console.Write("Enter choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice) &&
                    choice >= 1 && choice <= selectedPrimary.AvailableSecondaryItem.Count)
                {
                    selectedSecondary = selectedPrimary.AvailableSecondaryItem[choice - 1];

                    if (selectedSecondary.Name.ToLower() == "grocery bag")
                    {
                        Console.WriteLine("Correct! You chose grocery bag.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Wrong item! Try again.\n");
                    }
                }
            }

            // shove grocery bag
            while (true)
            {
                Console.WriteLine($"\nWhat do you want to do with {selectedSecondary.Name}?");

                var actions = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();

                for (int i = 0; i < actions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {actions[i]}");
                }

                Console.Write("Enter choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= actions.Count)
                {
                    ActionType selectedAction = actions[choice - 1];

                    if (validateAction(selectedSecondary, selectedAction))
                    {
                        Console.WriteLine($"You {selectedAction} the {selectedSecondary.Name} 😼");

                        // cari kitchen & bar table asal
                        var kitchen = engine.HouseSpaceList.FirstOrDefault(space => space.Name == "Kitchen");
                        var kitchenBarTable = kitchen.itemsAvailable.FirstOrDefault(item => item.Name == "Bar Table") as PrimaryItem;

                        if (selectedAction == ActionType.Shove)
                        {
                            kitchenBarTable.AvailableSecondaryItem.Remove(selectedSecondary); // remove grocery bag
                            Console.WriteLine("The Grocery Bag has been shoved away and is no longer available.");
                        }

                        break;
                    }
                    else
                    {
                        Console.WriteLine("That action can't be done. Try again.\n");
                    }
                }
            }
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

            exploreHouse(engine);

            Console.WriteLine();
            delayedText($"It saw a carton of eggs inside the grocery bag, located on top of the bar table.", 50, resetColorField, resetColorField);

            ValidateActionResult = false; // reset the ValidateActionResult for the next use in this scene
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

            Console.WriteLine();
            delayedText($"End of Scene 4...", 50, resetColorField, resetColorField);
        }
    }
}