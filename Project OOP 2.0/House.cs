using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP_2._0
{
    internal class House
    {
        //fields
        private const string map = @"
+--------+-----[Window]---+------[Window]----+---------+
|        |                |                  |         |
|  Bath       Bedroom     |  Master Bedroom       Bath |
| room 1 |                |                  |   room 2|
+--------+---  -----------+---------------  -+---------+
|                         |                  |
|                         |                  |
[Win]                     |              [Win]
|       Living Room              Kitchen     |
|                         |                  |
|                         |                  |
+---  ------[Window]-------+-----[Window]-----+
|                                            |
|                                            |
|                   Garage                   |
|                                            |
+--------------------------------------------+
";
        //Methods
        public void displayMap()
        {
            Console.WriteLine(map);

        }
    }
}
