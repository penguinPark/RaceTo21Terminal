using System;
namespace RaceTo21
{
	public enum Task // enum Task that will be used by Game.nextTask
	{
		GetNumberOfPlayers,
		GetNames,
		IntroducePlayers,
		AgreedScore,
		PlayerTurn,
		CheckForEnd,
		GameOver,
		Done // added Done to consider as the 'end of the game'
	}
}