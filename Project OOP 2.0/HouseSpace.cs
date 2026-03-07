using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP_2._0
{
    internal class HouseSpace
    {
        public string Name { get; set; }
        public List<PrimaryItem> itemsAvailable = new List<PrimaryItem>();

        //constructor
        public HouseSpace(string givenName)
        {
            Name = givenName;
        }

        //Methods
        
    }
}
