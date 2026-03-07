using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP_2._0
{
    internal abstract class Item
    {
        //Properties
        public string Name { get; set; }
        public string HouseSpaceLocation { get; set; }

        //Constructor
        public Item(string givenName, string givenHouseSpaceLocation)
        {
            Name = givenName;
            HouseSpaceLocation = givenHouseSpaceLocation;
        }
    }

    internal class PrimaryItem : Item
    {
        //Properties
        public List<SecondaryItem> AvailableSecondaryItem { get; set; }
        //Constructor
        public PrimaryItem(string givenName, string givenHouseSpaceLocation) : base(givenName, givenHouseSpaceLocation)
        {
            // Kena initialize list ni bila Item baru dicipta
            AvailableSecondaryItem = new List<SecondaryItem>();
        }
    }

    internal class SecondaryItem : Item
    {
        //Constructor
        public SecondaryItem(string givenName, string givenHouseSpaceLocation) : base(givenName, givenHouseSpaceLocation)
        {
        }


    }
}
