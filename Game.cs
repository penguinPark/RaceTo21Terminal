﻿using System;
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
                nextTask = Task.PlayerTurn;
            }
            else if (nextTask == Task.PlayerTurn)
            {
                cardTable.ShowHands(players);
                Player player = players[currentPlayer];
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
                                cardTable.AnnounceWinner(winner); // announce winner
                                winner.status = PlayerStatus.win; // wins the game
                                nextTask = Task.GameOver; // ends the game
                            }
                        }
                        else if (player.score == 21)
                        {
                            Player winner = player; // current player is winner
                            cardTable.AnnounceWinner(winner); // announce winner
                            player.status = PlayerStatus.win; // wins the game
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

        public void FinalTask() // created FinalTask() LEVEL2 on the homework pdf where: At end of round, each player is asked if they want to keep playing. If a player says no, they are removed from the player list.If only 1 player remains, that player is the winner(equivalent to everyone else “folding” in a card game)
        {
            List<Player> finishedPlayers = new List<Player>(); // created a new player list to hold the finishedPlayers
            int playerCount = numberOfPlayers; // counter for numberOfPlayers
            for (int i = 0; i < playerCount; i++) // created for loop to go through the players after game finished
            {
                Console.Write(players[i].name + " Do you want to continue playing? (Y/N)"); // does player want to keep playing?
                string choice = Console.ReadLine(); 
                if (choice.ToUpper().StartsWith("Y")) // if they type Y
                {
                }
                else if (choice.ToUpper().StartsWith("N")) // if they type N
                {
                    finishedPlayers.Add(players[i]); // will add players to the finishedPlayers list
                    numberOfPlayers--; // will count down the numberOfPlayers decreases when added to the finishedPlayers list
                }
                else
                {
                    Console.WriteLine("Please answer Y(es) or N(o)!"); // if they do not type Y or N
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
                Console.Write("Everyone quit... LAME... >:("); // > : (
            } else
            {
                // Console.Write("dsadhhsudkas " + players.Count.ToString()); 
                nextTask = Task.PlayerTurn; // goes to playerturn
                currentPlayer = 0; // current player resets to 0
                Restart(); // goes to method Restart that I made
            }
        }
        
        public void Restart() // made method Restart to refresh the player and deck when a new game starts
        {
            foreach (Player player in players)
            {
                player.Restart(); // calls the Restart method made in the Player class to reset the player score, status and cards
            }
            deck = new Deck(); // new deck
            deck.Shuffle(); // shuffles
            deck.ShowAllCards(); 
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
            int highScore = 0;
            foreach (var player in players)
            {
                cardTable.ShowHand(player);
                if (player.status == PlayerStatus.win) // someone hit 21
                {
                    return player;
                }
                if (player.status == PlayerStatus.stay) // still could win...
                {
                    if (player.score > highScore)
                    {
                        highScore = player.score;
                    }
                }
                // if busted don't bother checking!
            }
            if (highScore > 0) // someone scored, anyway!
            {
                // find the FIRST player in list who meets win condition
                return players.Find(player => player.score == highScore);
            }
            return null; // everyone must have busted because nobody won!
        }
    }
}
