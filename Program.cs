using System;

namespace RaceTo21
{
    class Program
    {
        static void Main(string[] args)
        {
            CardTable cardTable = new CardTable();
            Game game = new Game(cardTable);
            while (game.nextTask != Task.GameOver) // changed to Task.GameOver
            {
                game.DoNextTask();
            }
        }
    }
}

