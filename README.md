# The Feline Witness

## Group Members

• Ahmad Zafran Bin Faizal (24005345)
• Muhammad Wafi Bin Azman (24005379)
• Muhammad Asyraaf Bin Mustafa 24005305
• Humaira Rayyan Binti Haslah (24005364)
• Lee Xin Yue (24005697)
• Amier Aiman Bin Mohamad Faizal (24005402)

## Brief Description

**The Feline Witness** is a menu-driven, console-based interactive fiction game built entirely in C#.

The game places the player in the unique perspective of a household pet cat. After witnessing a suspicious conversation, the cat discovers that the husband is having a secret affair while the wife is away on an outstation trip. The ultimate goal of the game is for the feline protagonist to investigate the house, interact with various items, perform actions, as well as progress through missions such as hiding keys, creating chaos, engaging in combat and ultimately find a way to expose the truth and inform the wife. This project demonstrates the application of Object-Oriented Programming (OOP) principles through a structured game system involving classes, inheritance, and polymorphic behaviors.

## System Features

• Interactive house exploration system
• Scene-based gameplay (Intro–Scene6)
• Player-controlled character (MainCharacter Cat)
• Object interaction system (Primary & Secondary Items)
• Turn-based combat system (Scene 5)
• Dynamic dialogue system with typing effect
• Mission-based progression system

## OOP Concepts Used

1. Encapsulation
   Data such as player health and inventory is protected using private variables and controlled through methods.
   Example:
   • StatusIndicator manages HP
   • MainCharacter stores current location and carried item

2. Inheritance
   The system uses hierarchical class structures to reuse code efficiently.
   Examples:
   • Character → Cat → MainCharacter
   • Item → PrimaryItem → SecondaryItem
   • Scene → Intro, Scene1, Scene2, Scene3...
3. Polymorphism
   Different scenes implement their own logic using overridden methods.
   Examples:
   • playScene() behaves differently in each scene
   • validateAction() ensures correct actions based on the mission

4. Abstraction
   The usage of abstract class.
   Example:
   • Scene class

## ⚙️ Installation & Setup Guide

### Prerequisites

To run this game smoothly without any compatibility issues, you will need:

- **Visual Studio 2019 or newer** (Highly Recommended)
- **.NET Framework 4.7.2**

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
