using System;
using System.Collections.Generic;

namespace RaceTo21
{
	public class Player
	{
		public string name;
		public List<Card> cards = new List<Card>();
		public PlayerStatus status = PlayerStatus.active;
		public int score;
        internal object card;

        public Player(string n)
		{
			name = n;
        }

		/* Introduces player by name
		 * Called by CardTable object
		 */
		public void Introduce(int playerNum)
		{
			Console.WriteLine("Hello, my name is " + name + " and I am player #" + playerNum);
		}

		public void Restart() // created to make a reset all the player attributes for a new round
        {
			score = 0; // score resets to 0
			cards = new List<Card>(); // makes a new deck of cards
			status = PlayerStatus.active; // resets the player status
	}
	}
}

