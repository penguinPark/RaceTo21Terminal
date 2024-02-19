using System;
using System.Collections.Generic;

namespace RaceTo21
{
    public class CardTable
    {
        public CardTable()
        {
            Console.WriteLine("Setting Up Table...");
        }

        /* Shows the name of each player and introduces them by table position.
         * Is called by Game object.
         * Game object provides list of players.
         * Calls Introduce method on each player object.
         */
        public void ShowPlayers(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Introduce(i+1); // List is 0-indexed but user-friendly player positions would start with 1...
            }
        }

        /* Gets the user input for number of players.
         * Is called by Game object.
         * Returns number of players to Game object.
         */
        public int GetNumberOfPlayers()
        {
            Console.Write("How many players? ");
            string response = Console.ReadLine();
            int numberOfPlayers;
            while (int.TryParse(response, out numberOfPlayers) == false
                || numberOfPlayers < 2 || numberOfPlayers > 8)
            {
                Console.WriteLine("Invalid number of players.");
                Console.Write("How many players?");
                response = Console.ReadLine();
            }
            return numberOfPlayers;
        }

        /* Gets the name of a player
         * Is called by Game object
         * Game object provides player number
         * Returns name of a player to Game object
         */
        public string GetPlayerName(int playerNum)
        {
            Console.Write("What is the name of player# " + playerNum + "? ");
            string response = Console.ReadLine();
            while (response.Length < 1)
            {
                Console.WriteLine("Invalid name.");
                Console.Write("What is the name of player# " + playerNum + "? ");
                response = Console.ReadLine();
            }
            return response;
        }

        public int GetAgreedScore(int winningScore, List<Player> player) // this method was made to get an agreed winning score amongst the players
        {
            bool agreed = false; 
            int position = 0; // first player on the list
            while (!agreed) // while not everyone agrees
            {
                Console.Write("What should the winning total score be, " + player[position].name + "? "); 
                string response = Console.ReadLine(); // their input
                int numberResponse = int.Parse(response); // response as a number
                int agreeTrack = 0; // made to count the number of 'agreeds'
                for (int i = 0; i < player.Count; i++)
                {
                    if (i != position) // will not ask the person who input the number
                    {
                        Console.Write("Are you okay with this score " + player[i].name + "? Reply: Y/N ");
                        string scoreResponse = Console.ReadLine();
                        if (scoreResponse.ToUpper().StartsWith("Y"))
                        {
                            agreeTrack++; // this will increase
                        }
                        else if (!(scoreResponse.ToUpper().StartsWith("Y") || scoreResponse.ToUpper().StartsWith("N"))) {
                            Console.Write("Please put Y or N...........");
                            i--; // goes back to current player and asks them again
                        }
                    }
                }
                if (agreeTrack == player.Count - 1) // if the agreeTrack == the playercount - 1, then everyone except the person who wanted the winning score agreed
                {
                    return numberResponse; // everyone agreed to this score
                }
                position++; // this increases IF not everyone agrees to the winning score, going to the next player in the list
                if (position > player.Count - 1 ) // if it goes through every player on the list, it'll go back to the initial player on the list and ask the questions again
                {
                    position = 0; // goes back to initial player
                }
            }
            return 0; // should never get here...
        }

        public bool OfferACard(Player player)
        {
            if (player.cards.Count == 0)
            {
                Console.WriteLine("A card was given at the start"); // this is so that everyone gets an initial card
                return true;
            }
            while (true)
            {
                Console.Write(player.name + ", do you want a card? (Y/N)");
                string response = Console.ReadLine();
                if (response.ToUpper().StartsWith("Y")) 
                {
                    return true;
                }
                else if (response.ToUpper().StartsWith("N"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Please answer Y(es) or N(o)!");
                }
            }
        }

        public void ShowHand(Player player)
        {
            if (player.cards.Count > 0)
            {
                foreach (Card card in player.cards) // changed to card class
                {
                    Console.Write(player.name + " has "); 
                    Console.Write(card.Name + ", "); // showing card full names
                }
                Console.Write("=" + player.score + "/21 ");
                if (player.status != PlayerStatus.active)
                {
                    Console.Write("(" + player.status.ToString().ToUpper() + ")");
                }
                Console.WriteLine();
            }
        }

        public void showFinalTotalScores(List<Player> players) // I made this method to show the total final scores of every player after each round
        {
            foreach (Player player in players)
            {
                Console.WriteLine(player.name + "'s score is " + player.totalScore);
            }
        }

        public void ShowHands(List<Player> players)
        {
            foreach (Player player in players)
            {
                ShowHand(player);
            }
        }


        public void AnnounceWinner(Player player)
        {
            if (player != null)
            {
                Console.WriteLine(player.name + " wins this round!"); // changed to each round
            }
            else
            {
                Console.WriteLine("Everyone busted!");
            }
            Console.Write("Press <Enter> to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
    }
}