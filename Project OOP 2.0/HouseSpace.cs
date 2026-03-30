using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP_2._0
{
    internal class HouseSpace
    {
        //OOP Concept applied: ENCAPSULATION (private fields, and public properties)

        // 1. Private Fields
        private string name;
        private List<PrimaryItem> itemsAvailable;

        // 2. Public Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<PrimaryItem> ItemsAvailable //OOP Concept applied: COLLECTIONS (List of PrimaryItem)
        {
            get { return itemsAvailable; }
            set { itemsAvailable = value; }
        }

        // 3. Constructor
        public HouseSpace(string givenName)
        {
            this.Name = givenName;
            this.ItemsAvailable = new List<PrimaryItem>();
        }
    }
}