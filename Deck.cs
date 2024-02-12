using System;
using System.Collections.Generic;
using System.Linq; // currently only needed if we use alternate shuffle method

namespace RaceTo21
{
    public class Deck
    {
       // List<string> cards = new List<string>();

        List<Card> cards = new List<Card>(); // building the deck using Card class instead of strings

        public Deck()
        {
            Console.WriteLine("*********** Building deck...");
            string[] suits = { "S", "H", "C", "D" };

            for (int cardVal = 1; cardVal <= 13; cardVal++)
            {
                foreach (string cardSuit in suits)
                {
                    string cardName;
                    string fullName;
                    switch (cardVal)
                    {
                        case 1:
                            cardName = "A";
                            fullName = "Ace of "; // full name
                            break;
                        case 11:
                            cardName = "J";
                            fullName = "Jack of ";
                            break;
                        case 12:
                            cardName = "Q";
                            fullName = "Queen of ";
                            break;
                        case 13:
                            cardName = "K";
                            fullName = "King of ";
                            break;
                        default:
                            cardName = cardVal.ToString();
                            fullName = cardVal.ToString() + " of "; // full name for numbers
                            break;
                    }
                    switch (cardSuit) // full name for suits
                    {
                        case "S":
                            fullName = fullName + "Spades"; // the fullname is adding the suits
                            break;
                        case "H":
                            fullName = fullName + "Hearts";
                            break;
                        case "C":
                            fullName = fullName + "Clubs";
                            break;
                        case "D":
                            fullName = fullName + "Diamonds";
                            break;
                    }
                    string cardID = cardName + cardSuit; // makes the cardID
                    Card card = new Card(cardID, fullName); // makes new Card object with cardID and fullName 
                    cards.Add(card); // adds card objects into cards list
                }
            }
        }

        public void Shuffle()
        {
            Console.WriteLine("Shuffling Cards...");

            Random rng = new Random();

            // one-line method that uses Linq:
            // cards = cards.OrderBy(a => rng.Next()).ToList();

            // multi-line method that uses Array notation on a list!
            // (this should be easier to understand)
            for (int i=0; i<cards.Count; i++)
            {
                Card tmp = cards[i]; // changed to Card class
                int swapindex = rng.Next(cards.Count);
                cards[i] = cards[swapindex];
                cards[swapindex] = tmp;
            }
        }

        /* Maybe we can make a variation on this that's more useful,
         * but at the moment it's just really to confirm that our 
         * shuffling method(s) worked! And normally we want our card 
         * table to do all of the displaying, don't we?!
         */

        public void ShowAllCards()
        {
            for (int i=0; i<cards.Count; i++)
            {
                Console.Write(i+":"+cards[i].ID); // a list property can look like an Array! 
                if (i < cards.Count -1)
                {
                    Console.Write(" ");
                } else
                {
                    Console.WriteLine("");
                }
            }
        }

        public Card DealTopCard() // changed string to card
        {
            Card card = cards[cards.Count - 1]; // extract card object from the top of the deck
            cards.RemoveAt(cards.Count - 1);
            // Console.WriteLine("I'm giving you " + card);
            return card; // returns a card object
        }
    }
}

