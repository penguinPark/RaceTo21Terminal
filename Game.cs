using System;
using System.Collections.Generic;

namespace RaceTo21
{
    public class Game
    {
        int numberOfPlayers; // number of players in current game
        List<Player> players = new List<Player>(); // list of objects containing player data
        CardTable cardTable; // object in charge of displaying game information
        Deck deck = new Deck(); // deck of cards
        int currentPlayer = 0; // current player on list
        public Task nextTask; // keeps track of game state through enum Task
        private bool cheating = false; // lets you cheat for testing purposes if true
        Player previousWinner; // to keep track of the player who won
        int winningScore; // variable to represent the winning total score

        public Game(CardTable c)
        {
            cardTable = c;
            deck.Shuffle();
            deck.ShowAllCards();
            nextTask = Task.GetNumberOfPlayers; // changed so it doesn't use strings but enum Task
        }

        /* Adds a player to the current game
         * Called by DoNextTask() method
         */
        public void AddPlayer(string n)
        {
            players.Add(new Player(n));
        }

        /* Figures out what task to do next in game
         * as represented by field nextTask
         * Calls methods required to complete task
         * then sets nextTask.
         */
        public void DoNextTask()
        {
            Console.WriteLine("================================"); // this line should be elsewhere right?
            if (nextTask == Task.GetNumberOfPlayers)
            {
                numberOfPlayers = cardTable.GetNumberOfPlayers();
                nextTask = Task.GetNames;
            }
            else if (nextTask == Task.GetNames)
            {
                for (var count = 1; count <= numberOfPlayers; count++)
                {
                    var name = cardTable.GetPlayerName(count);
                    AddPlayer(name); // NOTE: player list will start from 0 index even though we use 1 for our count here to make the player numbering more human-friendly
                }
                nextTask = Task.IntroducePlayers;
            }
            else if (nextTask == Task.IntroducePlayers)
            {
                cardTable.ShowPlayers(players);
                nextTask = Task.AgreedScore; // goes to agreedScore
            } else if (nextTask == Task.AgreedScore)
            {
                winningScore = cardTable.GetAgreedScore(winningScore, players); // goes to the method GetAgreedScore
                nextTask = Task.PlayerTurn;
            }
            else if (nextTask == Task.PlayerTurn)
            {
                Player player = players[currentPlayer];
               // cardTable.ShowHands(players);
                if (player.status == PlayerStatus.active)
                {
                    if (cardTable.OfferACard(player))
                    {
                        Card card = deck.DealTopCard(); // card object is returned here
                        player.cards.Add(card);
                        player.score = ScoreHand(player);
                        if (player.score > 21)
                        {
                            player.status = PlayerStatus.bust;
                            int counter = 0; // counting how many players bust
                            Player notBusted = null; // assigned to null just in case this situation doesn't run and causes an error
                            for (int i = 0; i < numberOfPlayers; i++) // created a loop to check how many players bust
                            {
                                if (players[i].status == PlayerStatus.bust) // if the player busts...
                                {
                                    counter++; // ...counter will go up
                                }
                                else
                                {
                                    notBusted = players[i]; // whoever doesn't bust will be saved
                                }
                            }
                            if (numberOfPlayers - 1 == counter) // if the numberOfPlayers-1 busted (meaning 1 did not bust)
                            {
                                Player winner = notBusted; // whoever didn't bust will win
                                cardTable.ShowHands(players);
                                cardTable.AnnounceWinner(winner); // announce winner
                                winner.status = PlayerStatus.win; // wins the game                        
                                previousWinner = winner; // keeps the winner in this variable
                                nextTask = Task.GameOver; // ends the game
                            }
                        }
                        else if (player.score == 21)
                        {
                            Player winner = player; // current player is winner
                            cardTable.AnnounceWinner(winner); // announce winner
                            player.status = PlayerStatus.win; // wins the game
                            DoFinalScoring(); // goes to do final scoring
                            previousWinner = winner; // keeps the winner in this variable
                            nextTask = Task.GameOver; // ends the game 
                             
                        }
                    }
                    else
                    {
                        player.status = PlayerStatus.stay;
                    }
                }
                cardTable.ShowHand(player);
                if (player.status != PlayerStatus.win) // if the player status did not win [ so that the player who reaches 21 will end the game... ]
                {
                    nextTask = Task.CheckForEnd; // task will check for the end
                }
            }
            else if (nextTask == Task.CheckForEnd)
            {
                if (!CheckActivePlayers())
                {
                    Player winner = DoFinalScoring();        
                    previousWinner = winner;
                    cardTable.AnnounceWinner(winner);
                    nextTask = Task.GameOver;
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer > players.Count - 1)
                    {
                        currentPlayer = 0; // back to the first player...
                    }
                    nextTask = Task.PlayerTurn;
                }
            }
            else // we shouldn't get here...
            { 
                nextTask = Task.Done;
            }
        }

        public void FinalTask() // created FinalTask() LEVEL2 on the homework pdf where: At end of round, each player is asked if they want to keep playing. If a player says no, they are removed from the player list. If only 1 player remains, that player is the winner(equivalent to everyone else “folding” in a card game)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].totalScore >= winningScore)
                {
                    Console.WriteLine(players[i].name + " won the whole game! YIPPEEEEEEE!!!!!!");
                    winTheGame();
                    return;
                }
            }
            List<Player> finishedPlayers = new List<Player>(); // created a new player list to hold the finishedPlayers
            int playerCount = numberOfPlayers; // counter for numberOfPlayers
            for (int i = 0; i < playerCount; i++) // created for loop to go through the players after game finished
            {
                Console.Write(players[i].name + " Do you want to continue playing? (Y/N)"); // does player want to keep playing?
                string choice = Console.ReadLine(); 
                if (!(choice.ToUpper().StartsWith("Y") || choice.ToUpper().StartsWith("N"))) // if they do not type Y or N
                {
                    Console.WriteLine("Please answer Y(es) or N(o)!");
                    i--; // so it goes back to the current player and asks them again
                }
                else if (choice.ToUpper().StartsWith("N")) // if they type N
                {
                    finishedPlayers.Add(players[i]); // will add players to the finishedPlayers list
                    numberOfPlayers--; // will count down the numberOfPlayers decreases when added to the finishedPlayers list
                }
            }
            foreach (Player finishedplayer in finishedPlayers) // goes through each item in finishedPlayers and references each player with the finishedPlayer variable
            {
                players.Remove(finishedplayer); 

            }
            if (players.Count == 1) // if the number of remaining players is 1
            {
                cardTable.AnnounceWinner(players[0]); // announce winner
            }
            else if (players.Count == 0) // if the number of remaining players is 0
            {
                Console.Write("Everyone quit... LAME... >:("); // >:(
            } else
            { 
                nextTask = Task.PlayerTurn; // goes to playerturn
                currentPlayer = 0; // current player resets to 0

                Console.WriteLine("Shuffling Players...");
                Restart(); // goes to method Restart that I made
            }
        }

        public void winTheGame() // restarts the whole game after there is a true winner
        {
            currentPlayer = 0;
            winningScore = 0; // resets the winningScore
            players = new List<Player>(); // resets the list of players
            nextTask = Task.GetNumberOfPlayers; // goes back to the first task
            
        }

        public void PlayerShuffle() // I created this method to shuffle all the players after the game is over. 
        {
            Console.WriteLine("Shuffling Players..."); // letting players know that the players are being shuffled
            Random random = new Random();
            for (int i = 0; i < players.Count; i++) // loop to shuffle the players. Inspired by the Deck Shuffle method in the Deck Class.
            {
                Player tempPlayer = players[i]; 
                int swapping = random.Next(players.Count);
                players[i] = players[swapping];
                players[swapping] = tempPlayer;
            }
            if (previousWinner.status == PlayerStatus.active) // this is to make sure that the players still shuffle even after the winner leaves. So if the winner stays, this code will run.
            {
                int winTrack = players.IndexOf(previousWinner); // keeps track if the index the previous winner was on
                Player lastPlayer = players[players.Count - 1]; // keeps the last player object
                players[winTrack] = lastPlayer; // the position that the winning player was in gets replaced with the last player
                players[players.Count - 1] = previousWinner; // the winning player goes into the last position of the list
            }
        }

        public void Restart() // made method Restart to refresh the player and deck when a new game starts
        {
            foreach (Player player in players)
            {
                player.Restart(); // calls the Restart method made in the Player class to reset the player score, status, and cards
            }
            deck = new Deck(); // new deck
            deck.Shuffle(); // shuffles deck
            deck.ShowAllCards();
            PlayerShuffle(); // shuffles players!
        }
        public int ScoreHand(Player player)
        {
            int score = 0;
            if (cheating == true && player.status == PlayerStatus.active)
            {
                string response = null;
                while (int.TryParse(response, out score) == false)
                {
                    Console.Write("OK, what should player " + player.name + "'s score be?");
                    response = Console.ReadLine();
                }
                return score;
            }
            else
            {
                foreach (Card card in player.cards)
                {
                    //string faceValue = card.Remove(card.Length - 1);
                    string faceValue = card.ID[0].ToString(); // made string faceValue to show the cardName string that the player has
                    switch (faceValue)
                    {
                        case "K":
                        case "Q":
                        case "J":
                        case "1": // to make sure that the 10 card will score +10
                            score = score + 10;
                            break;
                        case "A":
                            score = score + 1;
                            break;
                        default:
                            score = score + int.Parse(faceValue);
                            break;
                    }
                }
            }
            return score;
        }

        public bool CheckActivePlayers()
        {
            foreach (var player in players)
            {
                if (player.status == PlayerStatus.active)
                {
                    return true; // at least one player is still going!
                }
            }
            return false; // everyone has stayed or busted, or someone won!
        }

        public Player DoFinalScoring()
        {
            Console.WriteLine("DOING THE FINAL SCORING");
            int highScore = 0;
            foreach (var player in players)
            {
                if (player.status == PlayerStatus.win) // someone hit 21
                {
                    highScore = player.score; // so winning player can be returned
                }
                if (player.status == PlayerStatus.stay) // still could win...
                {
                    if (player.score > highScore)
                    {
                        highScore = player.score;
                    }
                }
                if (player.status == PlayerStatus.bust)
                {
                    player.totalScore -= (player.score - 21);
                }
            }
            if (highScore > 0) // someone scored, anyway!
            {
                // find the FIRST player in list who meets win condition
                Player winner = players.Find(player => player.score == highScore); 
                winner.totalScore += winner.score; // the winner's + winner's with the highest score when they stay total score will be updated with their score
                cardTable.showFinalTotalScores(players); // shows all the player's final scores
                return winner; // returns the winner
            }
            cardTable.showFinalTotalScores(players);
            return null; // everyone must have busted because nobody won!
        }
    }
}
