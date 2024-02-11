using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceTo21
{  
    public class Card
    {
        public string ID; 
        public string Name;

        public Card(string ID, string Name)
        {
            this.ID = ID; // assigning ID in object card
            this.Name = Name; // assigning Name in object card
        }
    }
}
