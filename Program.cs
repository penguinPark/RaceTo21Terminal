using System;

namespace RaceTo21
{
    class Program
    {
        static void Main(string[] args)
        {
            CardTable cardTable = new CardTable();
            Game game = new Game(cardTable);
            while (game.nextTask != Task.Done) // changed to Task.GameOver
            {
                if (game.nextTask == Task.GameOver) // if the game nextTask is GameOver it will go to the FinalTask method that I made
                {
                    game.FinalTask(); // created final task for LEVEL 2 TASK on the homework
                }
                game.DoNextTask(); // to continue the game
            }
        }
    }
}

