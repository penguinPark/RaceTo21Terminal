using System;
using System.Collections.Generic;

namespace RaceTo21
{
	public class Player
	{ 
		private string name; // encapsulated this variable
		public List<Card> cards = new List<Card>();
		public PlayerStatus status = PlayerStatus.active;
		public int score; // encapsulated this variable
		internal object card;
		private int totalScore; // to calculate the player's total scores for the game && encapsulated this variable

		public Player(string n)
		{
			name = n;
			totalScore = 0;
		}

		public string Name // used getter / setter for the name variable 
        {
            get { return name; } 
			set { name = value;  }
        }
		
		public int Score // used getter / setter for the name variable 
		{
			get { return score; }
			set { score = value; }
		}

		public int TotalScore // used getter / setter for the name variable 
		{
			get { return totalScore; }
			set { totalScore = value; }
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

