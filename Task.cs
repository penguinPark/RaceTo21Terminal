using System;
namespace RaceTo21
{
	public enum Task // enum Task that will be used by Game.nextTask
	{
		GetNumberOfPlayers,
		GetNames,
		IntroducePlayers,
		PlayerTurn,
		CheckForEnd,
		GameOver
	}
}
