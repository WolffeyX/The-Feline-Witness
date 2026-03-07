# The Feline Witness 🐾

> **Project Status:** 🚧 Work in Progress (WIP) - Currently building Scene 2.

## 📖 Brief Description
**The Feline Witness** is a menu-driven, console-based interactive fiction game built entirely in C#. 

The game places the player in the unique perspective of a household pet cat. After witnessing a suspicious conversation, the cat discovers that the husband is having a secret affair while the wife is away on an outstation trip. The ultimate goal of the game is for the feline protagonist to investigate the house, interact with various items, and ultimately find a way to expose the truth and inform the wife.

---

## 🎯 Key Features & OOP Implementation
This project strictly adheres to the four main pillars of Object-Oriented Programming (OOP) to ensure a clean, scalable, and professional software architecture.

* **Encapsulation:** Sensitive data (such as the ASCII map layout) is hidden using `private` modifiers, while character attributes are safely accessed through C# Properties (`{ get; set; }`).
* **Inheritance:** The project features a robust class hierarchy. For example, `MainCharacter` inherits from `Cat`, which in turn inherits from the base `Character` class. Scenes and Items follow a similar parent-child structure.
* **Polymorphism:** * *Run-time (Overriding):* Each specific scene (e.g., `IntroScene`, `Scene1`) overrides the `playScene()` and `validateAction()` methods from the parent `Scene` class to execute unique story logic.
  * *Compile-time (Overloading):* The `delayedText()` method is overloaded to handle different text rendering preferences (with or without new lines).
* **Abstraction:** Core concepts like `Scene` and `Item` are implemented as `abstract` classes. They act as blueprints, ensuring that only specific, concrete items and scenes can be instantiated in the game world.

---

## 🏗️ Class Architecture & Functions

* **`GameEngine`**
  The central "brain" of the game. It initializes the house, sets up the characters, holds the overall game state, and triggers the scenes sequentially.
* **`Scene` (Base) / `IntroScene` / `Scene1`**
  Handles the narrative flow, typing-effect dialogues, and player movement loops. Each child scene contains its own specific script and mission validation logic.
* **`Character` / `Cat` / `MainCharacter`**
  Stores character data (names, current locations) and manages how dialogue is printed to the console (including a feature to skip typing delays via the input stream).
* **`House` & `HouseSpace`**
  `House` holds the ASCII map layout, while `HouseSpace` represents individual rooms (e.g., Living Room, Master Bedroom) that contain specific items to be explored.
* **`Item` / `PrimaryItem` / `SecondaryItem`**
  Represents interactable objects. Primary items (like furniture) act as containers for Secondary items (like a smartphone or car key) which the player can observe or grab.

---

## ⚙️ Installation & Setup Guide

### Prerequisites
To run this game smoothly without any compatibility issues, you will need:
* **Visual Studio 2019 or newer** (Highly Recommended)
* **.NET Framework 4.7.2**

### How to Run the Game
1. **Clone the repository:**
   Download the ZIP file or run the following command in your terminal:
   `git clone https://github.com/YourUsername/The-Feline-Witness.git`
2. **Open the Solution:**
   Navigate to the downloaded folder and double-click on `Project OOP 2.0.sln` to open the project in Visual Studio.
3. **Build and Run:**
   Press **F5** or click the **Start** button in Visual Studio to compile and launch the game console.
4. **Play:**
   Follow the on-screen menu prompts. Type the corresponding numbers or characters and press Enter to navigate the house and interact with items.
