using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP_2._0
{
    internal abstract class Item //OOP Concept applied: ABSTRACTION (Item is an abstract class that cannot be instantiated directly)
    {
        //OOP Concept applied: ENCAPSULATION (private fields, and public properties)
        // 1. Private Fields
        private string name;
        private string houseSpaceLocation;

        // 2. Public Properties 
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string HouseSpaceLocation
        {
            get { return houseSpaceLocation; }
            set { houseSpaceLocation = value; }
        }

        // 3. Constructor
        public Item(string givenName, string givenHouseSpaceLocation)
        {
            Name = givenName;
            HouseSpaceLocation = givenHouseSpaceLocation;
        }
    }

    internal class PrimaryItem : Item //OOP Concept applied: INHERITANCE (PrimaryItem is a subclass of Item)
    {
        //OOP Concept applied: Encapsulation (private fields, and public properties)
        // Private Field
        private List<SecondaryItem> availableSecondaryItem; //OOP Concept applied: COLLECTIONS (List of SecondaryItem)

        // Public Property
        public List<SecondaryItem> AvailableSecondaryItem
        {
            get { return availableSecondaryItem; }
            set { availableSecondaryItem = value; }
        }

        public PrimaryItem(string givenName, string givenHouseSpaceLocation) : base(givenName, givenHouseSpaceLocation)
        {
            this.AvailableSecondaryItem = new List<SecondaryItem>();
        }
    }

    internal class SecondaryItem : Item //OOP Concept applied: INHERITANCE (SecondaryItem is a subclass of Item)
    {
        public SecondaryItem(string givenName, string givenHouseSpaceLocation) : base(givenName, givenHouseSpaceLocation)
        {
        }
    }
}